using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;

namespace ClassicUO.Network.Socket;

internal static partial class BrowserWebSocketInterop
{
    private sealed class BrowserSocketState
    {
        public readonly TaskCompletionSource<bool> Opened = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public readonly ConcurrentQueue<byte[]> Incoming = new();
        public readonly SemaphoreSlim Signal = new(0);
        public volatile bool Closed;
        public volatile string? Error;
    }

    private static int _nextHandle = 1;
    private static readonly ConcurrentDictionary<int, BrowserSocketState> _states = new();

    [JSImport("connect", "classicuo.browser-socket")]
    private static partial void ConnectCore(int handle, string uri);

    [JSImport("send", "classicuo.browser-socket")]
    internal static partial void Send(int handle, byte[] bytes, int count);

    [JSImport("close", "classicuo.browser-socket")]
    internal static partial void Close(int handle);

    [JSImport("isOpen", "classicuo.browser-socket")]
    internal static partial bool IsOpen(int handle);

    [JSImport("proxyReady", "classicuo.browser-socket")]
    internal static partial bool ProxyReady(int handle);

    [JSImport("flushPending", "classicuo.browser-socket")]
    internal static partial int FlushPending(int handle);

    internal static Task WaitForOpenAsync(int handle, CancellationToken cancellationToken)
    {
        return GetState(handle).Opened.Task.WaitAsync(cancellationToken);
    }

    internal static int Connect(string uri)
    {
        var handle = Register();
        ConnectCore(handle, uri);
        return handle;
    }

    internal static bool TryDequeue(int handle, out byte[]? payload)
    {
        payload = null;

        if (!_states.TryGetValue(handle, out var state))
        {
            return false;
        }

        return state.Incoming.TryDequeue(out payload);
    }

    internal static Task WaitForMessageAsync(int handle, CancellationToken cancellationToken)
    {
        if (!_states.TryGetValue(handle, out var state))
        {
            return Task.CompletedTask;
        }

        return state.Signal.WaitAsync(cancellationToken);
    }

    internal static bool IsClosed(int handle)
    {
        return _states.TryGetValue(handle, out var state) && state.Closed;
    }

    [JSExport]
    public static void OnOpen(int handle)
    {
        if (_states.TryGetValue(handle, out var state))
        {
            state.Opened.TrySetResult(true);
        }
    }

    [JSExport]
    public static void OnMessage(int handle, byte[] bytes)
    {
        if (_states.TryGetValue(handle, out var state))
        {
            state.Incoming.Enqueue(bytes);
            state.Signal.Release();
        }
    }

    [JSExport]
    public static void OnClose(int handle)
    {
        if (_states.TryGetValue(handle, out var state))
        {
            state.Closed = true;
            if (!state.Opened.Task.IsCompleted)
            {
                state.Opened.TrySetException(new System.Net.WebSockets.WebSocketException("Browser websocket closed before opening."));
            }

            state.Signal.Release();
        }
    }

    [JSExport]
    public static void OnError(int handle, string message)
    {
        if (_states.TryGetValue(handle, out var state))
        {
            state.Error = message;
            if (!state.Opened.Task.IsCompleted)
            {
                state.Opened.TrySetException(new System.Net.WebSockets.WebSocketException(message));
            }

            state.Signal.Release();
        }
    }

    internal static int Register()
    {
        var handle = Interlocked.Increment(ref _nextHandle);
        _states[handle] = new BrowserSocketState();
        return handle;
    }

    internal static void Unregister(int handle)
    {
        if (_states.TryRemove(handle, out var state))
        {
            state.Closed = true;
            state.Signal.Release();
            state.Signal.Dispose();
        }
    }

    private static BrowserSocketState GetState(int handle)
    {
        if (_states.TryGetValue(handle, out var state))
        {
            return state;
        }

        throw new System.InvalidOperationException($"Unknown browser websocket handle {handle}");
    }
}
