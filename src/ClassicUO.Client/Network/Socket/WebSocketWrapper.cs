using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using ClassicUO.Utility.Platforms;
using ClassicUO.Utility.Logging;
using TcpSocket = System.Net.Sockets.Socket;
using static System.Buffers.ArrayPool<byte>;

namespace ClassicUO.Network.Socket;

/// <summary>
/// Handles websocket connections to shards that support it. `ws(s)://[hostname]` as the ip in settings.json.
/// For testing see `tools/ws/README.md` 
/// </summary>
sealed class WebSocketWrapper : SocketWrapper
{
    private const int MAX_RECEIVE_BUFFER_SIZE = 1024 * 1024; // 1MB
    private const int WS_KEEP_ALIVE_INTERVAL = 5;            // seconds

    private ClientWebSocket _webSocket;
    private TcpSocket _rawSocket;
    private Task _receiveTask;
    private int _disconnectNotified;
    private int _browserSocketHandle;
    private bool _browserSocketConnected;

    public override bool IsConnected => PlatformHelper.IsBrowser
        ? _browserSocketConnected
        : _webSocket?.State is WebSocketState.Connecting or WebSocketState.Open;
    public override EndPoint LocalEndPoint => PlatformHelper.IsBrowser
        ? new IPEndPoint(IPAddress.Loopback, 0)
        : _rawSocket?.LocalEndPoint;
    public bool IsCanceled => _tokenSource.IsCancellationRequested;

    private CancellationTokenSource _tokenSource = new();
    private CircularBuffer _receiveStream;

    public override void Connect(Uri uri)
    {
        if (PlatformHelper.IsBrowser)
        {
            BrowserRuntimeStatusReporter.Report("browser-ws-connect-begin", uri.ToString());
            _ = ConnectAsync(uri);
            return;
        }

        ConnectAsync(uri).Wait();
    }

    public override void Send(byte[] buffer, int offset, int count)
    {
        var copy = Shared.Rent(count);
        Buffer.BlockCopy(buffer, offset, copy, 0, count);
        SendCopy(copy, count);
    }

    private void SendCopy(byte[] copy, int count)
    {
        try
        {
            if (_tokenSource == null || _tokenSource.IsCancellationRequested)
            {
                return;
            }

            if (PlatformHelper.IsBrowser)
            {
                if (_browserSocketHandle == 0)
                {
                    return;
                }

                BrowserRuntimeStatusReporter.Report("browser-socket-send-call", $"{_browserSocketHandle}:{count}");
                BrowserWebSocketInterop.Send(_browserSocketHandle, copy, count);
                BrowserRuntimeStatusReporter.Report("browser-socket-send-complete", $"{_browserSocketHandle}:{count}");
                return;
            }

            if (_webSocket == null)
            {
                return;
            }

            _webSocket.SendAsync(copy.AsMemory().Slice(0, count), WebSocketMessageType.Binary, true, _tokenSource.Token)
                .GetAwaiter()
                .GetResult();
        }
        catch (OperationCanceledException)
        {
            // Disconnect or reconnect in progress.
        }
        catch (ObjectDisposedException)
        {
            // Disconnect or reconnect in progress.
        }
        catch (WebSocketException ex)
        {
            BrowserRuntimeStatusReporter.Report("browser-socket-send-error", $"WebSocketException:{ex.Message}");
            Log.Trace($"WebSocket send error: {ex.Message}");
        }
        catch (Exception ex)
        {
            BrowserRuntimeStatusReporter.Report("browser-socket-send-error", $"{ex.GetType().Name}:{ex.Message}");
            Log.Trace($"Unexpected websocket send error: {ex}");
        }
        finally
        {
            Shared.Return(copy);
        }
    }

    public override int Read(byte[] buffer)
    {
        if (_receiveStream == null)
        {
            return 0;
        }

        lock (_receiveStream)
        {
            return _receiveStream.Dequeue(buffer, 0, buffer.Length);
        }
    }

    public async Task ConnectAsync(Uri uri, CancellationTokenSource tokenSource = null)
    {
        if (IsConnected)
            return;

        _tokenSource = tokenSource == null || tokenSource.IsCancellationRequested
            ? new CancellationTokenSource()
            : tokenSource;
        _receiveTask = null;
        _receiveStream = new CircularBuffer();
        _disconnectNotified = 0;
        _browserSocketHandle = 0;
        _browserSocketConnected = false;

        try
        {
            if (PlatformHelper.IsBrowser)
            {
                await ConnectBrowserSocketAsync(uri);
                if (_browserSocketHandle != 0)
                {
                    BrowserRuntimeStatusReporter.Report("browser-ws-connect-state", "proxy-ready");
                    _browserSocketConnected = true;
                    BrowserRuntimeStatusReporter.Report("browser-ws-connected", uri.ToString());
                    InvokeOnConnected();
                    _receiveTask = StartBrowserReceiveAsync();
                }
                else
                {
                    BrowserRuntimeStatusReporter.Report("browser-ws-connect-state", "pending");
                    InvokeOnError(SocketError.NotConnected);
                }
                return;
            }
            else
            {
                await ConnectWebSocketAsyncCore(uri);
            }

            BrowserRuntimeStatusReporter.Report("browser-ws-connect-state", _webSocket?.State.ToString() ?? "null");

            if (IsConnected)
            {
                InvokeOnConnected();
            }
            else
            {
                BrowserRuntimeStatusReporter.Report("browser-ws-connect-not-connected", _webSocket?.State.ToString() ?? "null");
                InvokeOnError(SocketError.NotConnected);
            }
        }
        catch (WebSocketException ex)
        {
            BrowserRuntimeStatusReporter.Report("browser-ws-connect-error", $"{ex.GetType().Name}:{ex.Message}");
            SocketError error = ex.InnerException?.InnerException switch
            {
                SocketException socketException => socketException.SocketErrorCode,
                _ => SocketError.SocketError
            };

            Log.Error($"Error {ex.GetType().Name} {error} while connecting to {uri} {ex}");
            InvokeOnError(error);
        }
        catch (Exception ex)
        {
            BrowserRuntimeStatusReporter.Report("browser-ws-connect-error", $"{ex.GetType().Name}:{ex.Message}");
            Log.Error($"Unknown Error {ex.GetType().Name} while connecting to {uri} {ex}");
            InvokeOnError(SocketError.SocketError);
        }
        finally
        {
            if (!IsConnected)
            {
                _webSocket?.Dispose();
                _webSocket = null;
                _rawSocket?.Dispose();
                _rawSocket = null;
            }
        }
    }


    private async Task ConnectWebSocketAsyncCore(Uri uri)
    {
        if (PlatformHelper.IsBrowser)
        {
            throw new InvalidOperationException("Browser websocket connect should use ConnectBrowserSocketAsync().");
        }

        // Take control of creating the raw socket, turn off Nagle, also lets us peek at `Available` bytes.
        _rawSocket = new TcpSocket(SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true
        };

        _webSocket = new ClientWebSocket();
        _webSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(WS_KEEP_ALIVE_INTERVAL); // ping/pong

        using var httpClient = new HttpClient
        (
            new SocketsHttpHandler
            {
                ConnectCallback = async (context, token) =>
                {
                    try
                    {
                        await _rawSocket.ConnectAsync(context.DnsEndPoint, token);

                        return new NetworkStream(_rawSocket, ownsSocket: true);
                    }
                    catch
                    {
                        _rawSocket?.Dispose();
                        _rawSocket = null;
                        _webSocket?.Dispose();
                        _webSocket = null;

                        throw;
                    }
                }
            }
        );


        await _webSocket.ConnectAsync(uri, httpClient, _tokenSource.Token);

        Log.Trace($"Connected WebSocket: {uri}");

        // Keep the receive loop alive for the lifetime of the websocket connection.
        _receiveTask = StartReceiveAsync();
    }

    private async Task StartReceiveAsync()
    {
        if (PlatformHelper.IsBrowser)
        {
            await StartBrowserReceiveAsync();
            return;
        }

        var buffer = Shared.Rent(4096);
        var memory = buffer.AsMemory();
        var position = 0;

        try
        {
            while (IsConnected)
            {
                GrowReceiveBufferIfNeeded(ref buffer, ref memory);

                var receiveResult = await _webSocket.ReceiveAsync(memory.Slice(position), _tokenSource.Token);

                // Ignoring message types:
                // 1. WebSocketMessageType.Text: shouldn't be sent by the server, though might be useful for multiplexing commands
                // 2. WebSocketMessageType.Close: will be handled by IsConnected
                if (receiveResult.MessageType == WebSocketMessageType.Binary)
                    position += receiveResult.Count;

                if (!receiveResult.EndOfMessage)
                    continue;

                lock (_receiveStream)
                {
                    _receiveStream.Enqueue(buffer, 0, position);
                }

                position = 0;
            }
        }
        catch (OperationCanceledException)
        {
            Log.Trace("WebSocket OperationCanceledException on websocket " + (IsCanceled ? "(was requested)" : "(remote cancelled)"));
            BrowserRuntimeStatusReporter.Report("browser-ws-receive-canceled", IsCanceled ? "requested" : "remote");
        }
        catch (Exception e)
        {
            Log.Trace($"WebSocket error in StartReceiveAsync {e}");
            BrowserRuntimeStatusReporter.Report("browser-ws-receive-error", e.GetType().Name);
            InvokeOnError(SocketError.SocketError);
        }
        finally
        {
            Shared.Return(buffer);
        }

        if (!IsCanceled)
        {
            InvokeOnError(SocketError.ConnectionReset);
        }

        Disconnect();
    }

    // This is probably unnecessary, but WebSocket frames can be up to 2^63 bytes so we put some cap on it, yet to see packets larger than 4KB come through.
    // We peek the raw tcp socket available bytes, grow if the frame is bigger, we're naively assuming no compression.
    private void GrowReceiveBufferIfNeeded(ref byte[] buffer, ref Memory<byte> memory)
    {
        if (_rawSocket == null)
            return;

        if (_rawSocket.Available <= buffer.Length)
            return;

        if (_rawSocket.Available > MAX_RECEIVE_BUFFER_SIZE)
            throw new SocketException((int)SocketError.MessageSize, $"WebSocket message frame too large: {_rawSocket.Available} > {MAX_RECEIVE_BUFFER_SIZE}");

        Log.Trace($"WebSocket growing receive buffer {buffer.Length} bytes to {_rawSocket.Available} bytes");

        Shared.Return(buffer);
        buffer = Shared.Rent(_rawSocket.Available);
        memory = buffer.AsMemory();
    }

    public override void Disconnect()
    {
        BrowserRuntimeStatusReporter.Report("browser-ws-disconnect", PlatformHelper.IsBrowser
            ? (_browserSocketHandle != 0 ? (_browserSocketConnected ? "open" : "pending") : "null")
            : _webSocket?.State.ToString() ?? "null");
        if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
        {
            _tokenSource.Cancel();
        }

        if (PlatformHelper.IsBrowser && _browserSocketHandle != 0)
        {
            try
            {
                BrowserWebSocketInterop.Close(_browserSocketHandle);
            }
            catch
            {
                // Best effort.
            }
            finally
            {
                BrowserWebSocketInterop.Unregister(_browserSocketHandle);
                _browserSocketHandle = 0;
                _browserSocketConnected = false;
            }
        }

        try
        {
            if (_webSocket != null && _webSocket.State is WebSocketState.Open or WebSocketState.Connecting)
            {
                _webSocket.Abort();
            }
        }
        catch
        {
            // Best effort. The browser transport is expected to recover by reconnecting.
        }
        finally
        {
            SignalDisconnected();
            _webSocket?.Dispose();
            _webSocket = null;
            _rawSocket?.Dispose();
            _rawSocket = null;
            _receiveStream = null;
            _receiveTask = null;
            _browserSocketConnected = false;
        }
    }

    private async Task WaitForBrowserProxyReadyAsync(int handle, CancellationToken cancellationToken)
    {
        BrowserRuntimeStatusReporter.Report("browser-ws-wait-proxy-ready", handle.ToString());

        while (!cancellationToken.IsCancellationRequested)
        {
            if (BrowserWebSocketInterop.ProxyReady(handle))
            {
                BrowserRuntimeStatusReporter.Report("browser-ws-proxy-ready", handle.ToString());
                return;
            }

            await Task.Delay(100, cancellationToken);
        }
    }

    private async Task ConnectBrowserSocketAsync(Uri uri)
    {
        BrowserRuntimeStatusReporter.Report("browser-ws-connect-async", uri.ToString());
        _browserSocketHandle = BrowserWebSocketInterop.Connect(uri.ToString());
        BrowserRuntimeStatusReporter.Report("browser-ws-connect-handle", _browserSocketHandle.ToString());
        BrowserRuntimeStatusReporter.Report("browser-ws-connect-state", "pending-open");
        BrowserRuntimeStatusReporter.Report("browser-ws-await-open", _browserSocketHandle.ToString());
        await Task.CompletedTask;
    }

    private async Task StartBrowserReceiveAsync()
    {
        try
        {
            while (IsConnected && !_tokenSource.IsCancellationRequested)
            {
                var drainedAny = false;
                while (BrowserWebSocketInterop.TryDequeue(_browserSocketHandle, out var received) && received is { Length: > 0 })
                {
                    drainedAny = true;
                    lock (_receiveStream)
                    {
                        _receiveStream.Enqueue(received, 0, received.Length);
                    }
                }

                if (drainedAny)
                {
                    continue;
                }

                if (BrowserWebSocketInterop.IsClosed(_browserSocketHandle))
                {
                    BrowserRuntimeStatusReporter.Report("browser-ws-receive-closed", "closed");
                    break;
                }

                await BrowserWebSocketInterop.WaitForMessageAsync(_browserSocketHandle, _tokenSource.Token);
            }
        }
        catch (OperationCanceledException)
        {
            BrowserRuntimeStatusReporter.Report("browser-ws-receive-canceled", IsCanceled ? "requested" : "remote");
        }
        catch (Exception ex)
        {
            BrowserRuntimeStatusReporter.Report("browser-ws-receive-error", ex.GetType().Name);
            Log.Trace($"WebSocket error in browser receive loop {ex}");
            InvokeOnError(SocketError.SocketError);
        }
        finally
        {
            if (_browserSocketHandle != 0)
            {
                try
                {
                    BrowserWebSocketInterop.Close(_browserSocketHandle);
                }
                catch
                {
                    // Best effort.
                }
                finally
                {
                    BrowserWebSocketInterop.Unregister(_browserSocketHandle);
                    _browserSocketHandle = 0;
                    _browserSocketConnected = false;
                }
            }
        }

        if (!IsCanceled)
        {
            InvokeOnError(SocketError.ConnectionReset);
        }
    }

    public override void Dispose()
    {
        Disconnect();
    }

    private void SignalDisconnected()
    {
        if (Interlocked.Exchange(ref _disconnectNotified, 1) == 0)
        {
            InvokeOnDisconnected();
        }
    }
}
