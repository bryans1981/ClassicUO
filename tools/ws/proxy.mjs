import net from 'node:net';
import ws from 'ws';

const { Server: WebSocketServer } = ws;

const args = process.argv.slice(2);
const targetIndex = args.indexOf('--target');
const target = targetIndex >= 0 && args[targetIndex + 1] ? args[targetIndex + 1] : '127.0.0.1:2593';
const [targetHost, targetPortRaw] = target.split(':');
const targetPort = Number.parseInt(targetPortRaw || '2593', 10);
const sourceHost = '127.0.0.1';
const sourcePort = 2594;

if (!targetHost || Number.isNaN(targetPort)) {
  throw new Error(`Invalid --target value: ${target}`);
}

const server = new WebSocketServer({
  host: sourceHost,
  port: sourcePort,
  perMessageDeflate: false
});

console.log(`proxying from ${sourceHost}:${sourcePort} to ${targetHost}:${targetPort}`);

server.on('connection', (browserSocket, request) => {
  const browserAddress = request.socket.remoteAddress || 'unknown';
  console.log(`browser websocket open from ${browserAddress}`);

  const targetSocket = net.createConnection({ host: targetHost, port: targetPort });
  let upstreamConnected = false;
  let browserMessageCount = 0;
  let targetDataCount = 0;

  const closeBrowser = (code, reason) => {
    if (browserSocket.readyState === 0 || browserSocket.readyState === 1) {
      try {
        browserSocket.close(code, reason);
      } catch (error) {
        console.warn('browser websocket close failed', error);
      }
    }
  };

  const closeTarget = () => {
    if (!targetSocket.destroyed) {
      targetSocket.destroy();
    }
  };

  browserSocket.on('message', (data, isBinary) => {
    browserMessageCount += 1;
    if (browserMessageCount <= 10) {
      const messageLength = isBinary ? data.length : Buffer.byteLength(String(data));
      console.log(`browser websocket message ${browserMessageCount}: ${messageLength} bytes`);
    }

    if (!upstreamConnected) {
      return;
    }

    const payload = isBinary ? data : Buffer.from(data);
    targetSocket.write(payload);
  });

  browserSocket.on('close', (code, reason) => {
    console.log(`browser websocket closed: ${code} ${reason?.toString() || ''}`.trim());
    closeTarget();
  });

  browserSocket.on('error', (error) => {
    console.warn(`browser websocket error: ${error?.message || error}`);
  });

  targetSocket.on('connect', () => {
    upstreamConnected = true;
    console.log(`connected to target ${targetHost}:${targetPort}`);
  });

  targetSocket.on('data', (chunk) => {
    targetDataCount += 1;
    if (targetDataCount <= 10) {
      console.log(`target data ${targetDataCount}: ${chunk.length} bytes`);
    }

    if (browserSocket.readyState === 1) {
      browserSocket.send(chunk, { binary: true });
    }
  });

  targetSocket.on('close', (hadError) => {
    console.log(`target disconnected${hadError ? ' with error' : ''}`);
    closeBrowser(1011, 'target disconnected');
  });

  targetSocket.on('error', (error) => {
    console.warn(`target socket error: ${error?.message || error}`);
    closeBrowser(1011, error?.message || 'target socket error');
  });

});

server.on('listening', () => {
  console.log(`websocket relay listening on ws://${sourceHost}:${sourcePort}`);
});

server.on('error', (error) => {
  console.error(`relay server error: ${error?.message || error}`);
  process.exitCode = 1;
});
