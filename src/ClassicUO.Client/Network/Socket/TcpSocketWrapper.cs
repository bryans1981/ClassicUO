using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ClassicUO.Utility.Logging;

namespace ClassicUO.Network.Socket;

sealed class TcpSocketWrapper : SocketWrapper
{
    private TcpClient _socket;
    private int _disconnectNotified;

    public override bool IsConnected => _socket?.Client?.Connected ?? false;

    public override EndPoint LocalEndPoint => _socket?.Client?.LocalEndPoint;


    public override void Connect(Uri uri)
    {
        if (IsConnected)
            return;

        _socket = new TcpClient();
        _socket.NoDelay = true;
        _disconnectNotified = 0;

        try
        {
            _socket.Connect(uri.Host, uri.Port);

            if (!IsConnected)
            {
                InvokeOnError(SocketError.NotConnected);

                return;
            }

            InvokeOnConnected();
        }
        catch (SocketException socketEx)
        {
            Log.Error($"error while connecting {socketEx}");
            InvokeOnError(socketEx.SocketErrorCode);
        }
        catch (Exception ex)
        {
            Log.Error($"error while connecting {ex}");
            InvokeOnError(SocketError.SocketError);
        }
    }

    public override void Send(byte[] buffer, int offset, int count)
    {
        var stream = _socket.GetStream();
        stream.Write(buffer, offset, count);
        stream.Flush();
    }

    public override int Read(byte[] buffer)
    {
        if (!IsConnected)
            return 0;

        var available = Math.Min(buffer.Length, _socket.Available);
        var done = 0;

        var stream = _socket.GetStream();

        while (done < available)
        {
            var toRead = Math.Min(buffer.Length, available - done);
            var read = stream.Read(buffer, done, toRead);

            if (read <= 0)
            {
                InvokeOnDisconnected();
                Disconnect();

                return 0;
            }

            done += read;
        }

        return done;
    }

    public override void Disconnect()
    {
        SignalDisconnected();
        _socket?.Close();
        Dispose();
    }

    public override void Dispose()
    {
        _socket?.Dispose();
    }

    private void SignalDisconnected()
    {
        if (Interlocked.Exchange(ref _disconnectNotified, 1) == 0)
        {
            InvokeOnDisconnected();
        }
    }
}
