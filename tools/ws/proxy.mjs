import websockify from '@maximegris/node-websockify';

/**
 * Note: this won't work on OSI or any shard that uses a different IP for the gameserver than the loginserver
 */
const args = process.argv.slice(2);
const targetIndex = args.indexOf('--target');
const target = targetIndex >= 0 && args[targetIndex + 1] ? args[targetIndex + 1] : '127.0.0.1:2593';

websockify({ source: '127.0.0.1:2594', target });
