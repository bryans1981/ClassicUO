window.classicuoProbe = {
  getEnvironment: function () {
    const hasWindow = typeof window !== 'undefined';
    const hasIndexedDb = typeof indexedDB !== 'undefined';
    const hasFileSystemAccessApi = hasWindow && 'showOpenFilePicker' in window;
    const hasOpfsApi = !!(navigator.storage && navigator.storage.getDirectory);
    const hasSharedArrayBuffer = typeof SharedArrayBuffer !== 'undefined';

    return {
      hasWindow,
      hasIndexedDb,
      hasFileSystemAccessApi,
      hasOpfsApi,
      hasSharedArrayBuffer,
      userAgent: hasWindow ? window.navigator.userAgent : '',
      origin: hasWindow ? window.location.origin : ''
    };
  }
};

function hasOpfsApi() {
  return !!(navigator.storage && navigator.storage.getDirectory);
}

const classicuoRoots = {
  assets: '/uo',
  profiles: '/profiles',
  cache: '/cache',
  config: '/config'
};
const classicuoReadCache = new Map();
const hostedSeedManifestPath = '/local-uo/manifest.json';

function normalizeVirtualPath(path) {
  if (!path) {
    return '/';
  }

  const segments = String(path)
    .replace(/\\/g, '/')
    .split('/')
    .filter(Boolean);
  const normalized = [];

  for (const segment of segments) {
    if (segment === '.') {
      continue;
    }

    if (segment === '..') {
      if (normalized.length > 0) {
        normalized.pop();
      }

      continue;
    }

    normalized.push(segment);
  }

  return normalized.length === 0 ? '/' : `/${normalized.join('/')}`;
}

function splitVirtualPath(path) {
  return normalizeVirtualPath(path).split('/').filter(Boolean);
}

async function getAssetsDir() {
  const root = await navigator.storage.getDirectory();
  return root.getDirectoryHandle('classicuo-assets', { create: true });
}

async function getDirectoryHandleForVirtualPath(path, create) {
  let dir = await getAssetsDir();
  const segments = splitVirtualPath(path);

  for (const segment of segments) {
    dir = await dir.getDirectoryHandle(segment, { create });
  }

  return dir;
}

async function getFileHandleForVirtualPath(path, create) {
  const segments = splitVirtualPath(path);

  if (segments.length === 0) {
    throw new Error('A file path is required.');
  }

  const fileName = segments.pop();
  const dir = segments.length === 0
    ? await getAssetsDir()
    : await getDirectoryHandleForVirtualPath(segments.join('/'), create);

  return dir.getFileHandle(fileName, { create });
}

async function listAssetFilesRecursive() {
  const fileNames = [];

  async function visit(handle, prefix) {
    for await (const [name, child] of handle.entries()) {
      const childPath = prefix ? `${prefix}/${name}` : name;

      if (child.kind === 'file') {
        fileNames.push(childPath);
      } else if (child.kind === 'directory') {
        await visit(child, childPath);
      }
    }
  }

  const dir = await getAssetsDir();
  await visit(dir, '');
  fileNames.sort((a, b) => a.localeCompare(b));
  return fileNames;
}

async function listFilesUnderPath(path) {
  const normalizedPath = normalizeVirtualPath(path);
  const fileNames = [];

  async function visit(handle, prefix) {
    for await (const [name, child] of handle.entries()) {
      const childPath = prefix ? `${prefix}/${name}` : name;

      if (child.kind === 'file') {
        fileNames.push(childPath);
      } else if (child.kind === 'directory') {
        await visit(child, childPath);
      }
    }
  }

  const segments = splitVirtualPath(normalizedPath);
  let current = await getAssetsDir();

  for (const segment of segments) {
    try {
      current = await current.getDirectoryHandle(segment, { create: false });
    } catch (error) {
      if (error && error.name === 'NotFoundError') {
        return [];
      }

      throw error;
    }
  }

  await visit(current, normalizedPath === '/' ? '' : normalizedPath.slice(1));
  fileNames.sort((a, b) => a.localeCompare(b));
  return fileNames;
}

async function deleteDirectoryContents(handle) {
  let deletedEntries = 0;

  for await (const [name, child] of handle.entries()) {
    if (child.kind === 'directory') {
      deletedEntries += await deleteDirectoryContents(child);
      await handle.removeEntry(name, { recursive: true });
      deletedEntries += 1;
    } else {
      await handle.removeEntry(name);
      deletedEntries += 1;
    }
  }

  return deletedEntries;
}

async function fileExists(path) {
  try {
    await getFileHandleForVirtualPath(path, false);
    return true;
  } catch (error) {
    if (error && error.name === 'NotFoundError') {
      return false;
    }

    throw error;
  }
}

async function readFileAsArrayBuffer(path) {
  const handle = await getFileHandleForVirtualPath(path, false);
  const file = await handle.getFile();
  return file.arrayBuffer();
}

function decodeFixedString(bytes, offset, length) {
  const slice = bytes.subarray(offset, offset + length);
  return new TextDecoder().decode(slice).replace(/\0+$/, '');
}

function looksLikeTileName(value) {
  if (!value) {
    return false;
  }

  const trimmed = value.trim();

  if (!trimmed) {
    return false;
  }

  return /^[A-Za-z0-9 !'().,_-]+$/.test(trimmed);
}

function parseTileDataFirstLand(bytes, isOldFormat) {
  const view = new DataView(bytes.buffer, bytes.byteOffset, bytes.byteLength);
  let offset = 4;
  let flags = '0';

  if (isOldFormat) {
    flags = String(view.getUint32(offset, true));
    offset += 4;
  } else {
    flags = view.getBigUint64(offset, true).toString();
    offset += 8;
  }

  const textureId = view.getUint16(offset, true);
  offset += 2;
  const name = decodeFixedString(bytes, offset, 20);

  return {
    isOldFormat,
    header: view.getUint32(0, true),
    firstLandFlags: flags,
    firstLandTextureId: textureId,
    firstLandName: name
  };
}

async function getFileSize(path) {
  const handle = await getFileHandleForVirtualPath(path, false);
  const file = await handle.getFile();
  return file.size;
}

function getExtension(path) {
  const normalized = normalizeVirtualPath(path);
  const fileName = normalized.split('/').pop() ?? '';
  const extensionIndex = fileName.lastIndexOf('.');

  if (extensionIndex <= 0 || extensionIndex === fileName.length - 1) {
    return '(none)';
  }

  return fileName.slice(extensionIndex).toLowerCase();
}

function getFileName(path) {
  const normalized = normalizeVirtualPath(path);
  return normalized.split('/').pop()?.toLowerCase() ?? '';
}

function anyFileMatches(fileNames, patterns) {
  return fileNames.filter((fileName) => patterns.some((pattern) => pattern.test(fileName)));
}

function buildBootstrapReadiness(entries) {
  const fileNames = entries.map((entry) => getFileName(entry.path));
  const recommendedImportSet = [
    'tiledata.mul',
    'hues.mul',
    'map0.mul',
    'art.mul',
    'artidx.mul',
    'gumpart.mul',
    'gumpidx.mul',
    'cliloc.enu'
  ];
  const checks = [
    {
      name: 'Core metadata',
      description: 'Required by the current client startup validation and core tile loading.',
      acceptedPatterns: ['tiledata.mul', 'hues.mul'],
      matches: anyFileMatches(fileNames, [/^tiledata\.mul$/, /^hues\.mul$/])
    },
    {
      name: 'Map data',
      description: 'At least one map source is needed for world data.',
      acceptedPatterns: ['map0.mul', 'map0LegacyMUL.uop'],
      matches: anyFileMatches(fileNames, [/^map0\.mul$/, /^map0legacymul\.uop$/])
    },
    {
      name: 'Art data',
      description: 'Classic art assets must exist as MUL/IDX or UOP.',
      acceptedPatterns: ['art.mul + artidx.mul', 'artlegacymul.uop'],
      matches: anyFileMatches(fileNames, [/^art\.mul$/, /^artidx\.mul$/, /^artlegacymul\.uop$/])
    },
    {
      name: 'Gump data',
      description: 'UI art is needed as MUL/IDX or UOP.',
      acceptedPatterns: ['gumpart.mul + gumpidx.mul', 'gumpartlegacymul.uop'],
      matches: anyFileMatches(fileNames, [/^gumpart\.mul$/, /^gumpidx\.mul$/, /^gumpartlegacymul\.uop$/])
    },
    {
      name: 'Localization',
      description: 'At least one cliloc file should exist for text resources.',
      acceptedPatterns: ['cliloc.*'],
      matches: anyFileMatches(fileNames, [/^cliloc\..+/])
    }
  ];

  for (const check of checks) {
    check.isSatisfied = false;

    if (check.name === 'Core metadata') {
      check.isSatisfied = check.matches.includes('tiledata.mul') && check.matches.includes('hues.mul');
    } else if (check.name === 'Art data') {
      check.isSatisfied =
        check.matches.includes('artlegacymul.uop') ||
        (check.matches.includes('art.mul') && check.matches.includes('artidx.mul'));
    } else if (check.name === 'Gump data') {
      check.isSatisfied =
        check.matches.includes('gumpartlegacymul.uop') ||
        (check.matches.includes('gumpart.mul') && check.matches.includes('gumpidx.mul'));
    } else {
      check.isSatisfied = check.matches.length > 0;
    }
  }

  const passedChecks = checks.filter((check) => check.isSatisfied).length;
  const missingRecommendedFiles = recommendedImportSet.filter((fileName) => !fileNames.includes(fileName));
  const primaryReadTargetPath = fileNames.includes('tiledata.mul') ? '/uo/tiledata.mul' : '';
  const primaryReadTargetReason = primaryReadTargetPath
    ? 'This is the current hard startup gate in ClassicUO and the cleanest first real asset-read target.'
    : 'Import tiledata.mul first. It is the current desktop startup gate and the best first real browser asset-read target.';

  return {
    isReady: passedChecks === checks.length,
    passedChecks,
    totalChecks: checks.length,
    primaryReadTargetPath,
    primaryReadTargetReason,
    recommendedImportSet,
    missingRecommendedFiles,
    checks: checks.map((check) => ({
      name: check.name,
      description: check.description,
      isSatisfied: check.isSatisfied,
      acceptedPatterns: check.acceptedPatterns,
      matches: check.matches.sort((a, b) => a.localeCompare(b))
    }))
  };
}

async function importHostedSeedEntries(entries) {
  const result = {
    hasOpfsApi: hasOpfsApi(),
    importedCount: 0,
    importedFileNames: [],
    error: ''
  };

  if (!result.hasOpfsApi) {
    result.error = 'OPFS API is not available in this browser.';
    return result;
  }

  for (const entry of entries) {
    const relativePath = normalizeVirtualPath(`/${entry.relativePath || ''}`);
    const response = await fetch(`/local-uo${relativePath}`, { cache: 'no-store' });

    if (!response.ok) {
      result.error = `Failed to fetch hosted seed file ${relativePath} (status ${response.status}).`;
      return result;
    }

    const bytes = new Uint8Array(await response.arrayBuffer());
    const destinationPath = normalizeVirtualPath(`${classicuoRoots.assets}${relativePath}`);
    const writeResult = await window.classicuoFs.writeBytesBase64(destinationPath, uint8ArrayToBase64(bytes));

    if (!writeResult.succeeded) {
      result.error = writeResult.error || `Failed to import ${destinationPath}.`;
      return result;
    }

    result.importedFileNames.push(destinationPath);
  }

  result.importedCount = result.importedFileNames.length;
  result.importedFileNames.sort((a, b) => a.localeCompare(b));
  return result;
}

async function importSelectedFiles(options) {
  const result = {
    hasOpfsApi: hasOpfsApi(),
    importedCount: 0,
    importedFileNames: [],
    error: ''
  };

  if (!result.hasOpfsApi) {
    result.error = 'OPFS API is not available in this browser.';
    return result;
  }

  try {
    const input = document.createElement('input');
    input.type = 'file';
    input.multiple = true;

    if (options && options.directory) {
      input.setAttribute('webkitdirectory', '');
    }

    const files = await new Promise((resolve) => {
      input.addEventListener('change', () => resolve(Array.from(input.files ?? [])), { once: true });
      input.click();
    });

    if (!files.length) {
      return result;
    }

    for (const file of files) {
      const relativePath = options && options.directory && file.webkitRelativePath
        ? `/${file.webkitRelativePath}`
        : `/${file.name}`;
      const destinationPath = normalizeVirtualPath(`${options && options.basePath ? options.basePath : '/'}${relativePath}`);
      const bytes = new Uint8Array(await file.arrayBuffer());
      const writeResult = await window.classicuoFs.writeBytesBase64(destinationPath, uint8ArrayToBase64(bytes));

      if (!writeResult.succeeded) {
        result.error = writeResult.error || `Failed to import ${destinationPath}.`;
        return result;
      }

      result.importedFileNames.push(destinationPath);
    }

    result.importedCount = result.importedFileNames.length;
    result.importedFileNames.sort((a, b) => a.localeCompare(b));
    return result;
  } catch (error) {
    result.error = error instanceof Error ? error.message : String(error);
    return result;
  }
}

function uint8ArrayToBase64(bytes) {
  let binary = '';
  const chunkSize = 0x8000;

  for (let i = 0; i < bytes.length; i += chunkSize) {
    const chunk = bytes.subarray(i, i + chunkSize);
    binary += String.fromCharCode(...chunk);
  }

  return btoa(binary);
}

function base64ToUint8Array(base64) {
  const binary = atob(base64);
  const bytes = new Uint8Array(binary.length);

  for (let i = 0; i < binary.length; i++) {
    bytes[i] = binary.charCodeAt(i);
  }

  return bytes;
}

async function fetchHostedSeedManifest() {
  const response = await fetch(hostedSeedManifestPath, { cache: 'no-store' });

  if (!response.ok) {
    if (response.status === 404) {
      return null;
    }

    throw new Error(`Hosted seed manifest request failed with status ${response.status}.`);
  }

  return response.json();
}

window.classicuoFs = {
  runSmokeTest: async function () {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      writeSucceeded: false,
      readSucceeded: false,
      readText: '',
      fileNames: [],
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      const writeResult = await window.classicuoFs.writeText('/probe.txt', `classicuo browser spike ${new Date().toISOString()}`);

      if (!writeResult.succeeded) {
        result.error = writeResult.error || 'Write failed.';
        return result;
      }

      result.writeSucceeded = true;

      const readResult = await window.classicuoFs.readText('/probe.txt');

      if (!readResult.exists) {
        result.error = readResult.error || 'Read failed.';
        return result;
      }

      result.readText = readResult.text;
      result.readSucceeded = true;
      result.fileNames = await listAssetFilesRecursive();

      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  importFiles: async function () {
    return importSelectedFiles({ directory: false, basePath: classicuoRoots.assets });
  },

  importDirectory: async function () {
    return importSelectedFiles({ directory: true, basePath: classicuoRoots.assets });
  },

  listFiles: async function () {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      fileNames: [],
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      result.fileNames = await listAssetFilesRecursive();
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  listFilesUnderPath: async function (path) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      path: normalizeVirtualPath(path),
      fileNames: [],
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      result.fileNames = await listFilesUnderPath(result.path);
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  resetStorage: async function () {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      succeeded: false,
      deletedEntries: 0,
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      const dir = await getAssetsDir();
      result.deletedEntries = await deleteDirectoryContents(dir);
      result.succeeded = true;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  fileExists: async function (path) {
    if (!hasOpfsApi()) {
      return false;
    }

    return fileExists(path);
  },

  readText: async function (path) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      exists: false,
      path: normalizeVirtualPath(path),
      text: '',
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      if (!await fileExists(path)) {
        return result;
      }

      const buffer = await readFileAsArrayBuffer(path);
      result.text = new TextDecoder().decode(buffer);
      result.exists = true;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  writeText: async function (path, text) {
    const bytes = new TextEncoder().encode(text ?? '');
    return window.classicuoFs.writeBytesBase64(path, uint8ArrayToBase64(bytes));
  },

  readBytesBase64: async function (path) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      exists: false,
      path: normalizeVirtualPath(path),
      base64: '',
      length: 0,
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      if (!await fileExists(path)) {
        return result;
      }

      const bytes = new Uint8Array(await readFileAsArrayBuffer(path));
      result.base64 = uint8ArrayToBase64(bytes);
      result.length = bytes.length;
      result.exists = true;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  writeBytesBase64: async function (path, base64) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      succeeded: false,
      path: normalizeVirtualPath(path),
      length: 0,
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      const fileHandle = await getFileHandleForVirtualPath(path, true);
      const writable = await fileHandle.createWritable();
      const bytes = base64ToUint8Array(base64 ?? '');
      await writable.write(bytes);
      await writable.close();
      result.length = bytes.length;
      result.succeeded = true;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  benchmarkReadPath: async function (path, iterations) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      path: normalizeVirtualPath(path),
      exists: false,
      iterations: Number(iterations) || 1,
      length: 0,
      totalMs: 0,
      averageMs: 0,
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      if (!await fileExists(result.path)) {
        return result;
      }

      result.exists = true;
      const started = performance.now();

      for (let i = 0; i < result.iterations; i++) {
        const bytes = new Uint8Array(await readFileAsArrayBuffer(result.path));
        result.length = bytes.length;
      }

      result.totalMs = performance.now() - started;
      result.averageMs = result.iterations > 0 ? result.totalMs / result.iterations : result.totalMs;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  preloadPath: async function (path) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      succeeded: false,
      path: normalizeVirtualPath(path),
      length: 0,
      totalMs: 0,
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      if (!await fileExists(result.path)) {
        result.error = 'File does not exist.';
        return result;
      }

      const started = performance.now();
      const bytes = new Uint8Array(await readFileAsArrayBuffer(result.path));
      result.totalMs = performance.now() - started;
      result.length = bytes.length;
      classicuoReadCache.set(result.path, bytes);
      result.succeeded = true;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  getPreloadCacheSummary: async function () {
    let totalBytes = 0;
    const paths = Array.from(classicuoReadCache.keys()).sort((a, b) => a.localeCompare(b));

    for (const path of paths) {
      totalBytes += classicuoReadCache.get(path)?.length ?? 0;
    }

    return {
      entryCount: paths.length,
      totalBytes,
      paths
    };
  },

  clearPreloadCache: async function () {
    let totalBytes = 0;

    for (const value of classicuoReadCache.values()) {
      totalBytes += value?.length ?? 0;
    }

    classicuoReadCache.clear();

    return {
      hasOpfsApi: hasOpfsApi(),
      succeeded: true,
      path: '',
      length: totalBytes,
      totalMs: 0,
      error: ''
    };
  },

  getAssetManifest: async function () {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      rootPath: classicuoRoots.assets,
      fileCount: 0,
      totalBytes: 0,
      entries: [],
      extensions: [],
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      const paths = await listFilesUnderPath(classicuoRoots.assets);
      const entries = [];
      const extensionSet = new Set();

      for (const path of paths) {
        const size = await getFileSize(path);
        const extension = getExtension(path);
        extensionSet.add(extension);

        entries.push({
          path,
          extension,
          size,
          isPreloaded: classicuoReadCache.has(path)
        });

        result.totalBytes += size;
      }

      entries.sort((a, b) => a.path.localeCompare(b.path));
      result.entries = entries;
      result.fileCount = entries.length;
      result.extensions = Array.from(extensionSet).sort((a, b) => a.localeCompare(b));
      result.bootstrap = buildBootstrapReadiness(entries);
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  getHostedSeedManifest: async function () {
    const result = {
      available: false,
      rootPath: classicuoRoots.assets,
      fileCount: 0,
      totalBytes: 0,
      entries: [],
      error: ''
    };

    try {
      const manifest = await fetchHostedSeedManifest();

      if (!manifest) {
        result.error = 'No hosted local seed manifest was found.';
        return result;
      }

      result.available = true;
      result.rootPath = manifest.rootPath || classicuoRoots.assets;
      result.fileCount = Array.isArray(manifest.entries) ? manifest.entries.length : 0;
      result.totalBytes = Number(manifest.totalBytes) || 0;
      result.entries = Array.isArray(manifest.entries) ? manifest.entries : [];
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  importHostedSeed: async function () {
    try {
      const manifest = await fetchHostedSeedManifest();

      if (!manifest || !Array.isArray(manifest.entries) || manifest.entries.length === 0) {
        return {
          hasOpfsApi: hasOpfsApi(),
          importedCount: 0,
          importedFileNames: [],
          error: 'No hosted local seed files are available.'
        };
      }

      return importHostedSeedEntries(manifest.entries);
    } catch (error) {
      return {
        hasOpfsApi: hasOpfsApi(),
        importedCount: 0,
        importedFileNames: [],
        error: error instanceof Error ? error.message : String(error)
      };
    }
  },

  importHostedRecommendedSeed: async function () {
    try {
      const manifest = await fetchHostedSeedManifest();

      if (!manifest || !Array.isArray(manifest.entries) || manifest.entries.length === 0) {
        return {
          hasOpfsApi: hasOpfsApi(),
          importedCount: 0,
          importedFileNames: [],
          error: 'No hosted local seed files are available.'
        };
      }

      const wanted = new Set([
        'tiledata.mul',
        'hues.mul',
        'map0.mul',
        'map0legacymul.uop',
        'art.mul',
        'artidx.mul',
        'artlegacymul.uop',
        'gumpart.mul',
        'gumpidx.mul',
        'gumpartlegacymul.uop',
        'cliloc.enu'
      ]);

      const filteredEntries = manifest.entries.filter((entry) => wanted.has(String(entry.relativePath || '').split('/').pop().toLowerCase()));

      if (filteredEntries.length === 0) {
        return {
          hasOpfsApi: hasOpfsApi(),
          importedCount: 0,
          importedFileNames: [],
          error: 'No recommended hosted seed files were found in the local bundle.'
        };
      }

      return importHostedSeedEntries(filteredEntries);
    } catch (error) {
      return {
        hasOpfsApi: hasOpfsApi(),
        importedCount: 0,
        importedFileNames: [],
        error: error instanceof Error ? error.message : String(error)
      };
    }
  },

  probeTileData: async function (path) {
    const result = {
      hasOpfsApi: hasOpfsApi(),
      path: normalizeVirtualPath(path),
      exists: false,
      length: 0,
      isOldFormat: false,
      header: 0,
      firstLandFlags: '0',
      firstLandTextureId: 0,
      firstLandName: '',
      totalMs: 0,
      error: ''
    };

    if (!result.hasOpfsApi) {
      result.error = 'OPFS API is not available in this browser.';
      return result;
    }

    try {
      if (!await fileExists(result.path)) {
        return result;
      }

      const started = performance.now();
      const bytes = new Uint8Array(await readFileAsArrayBuffer(result.path));
      result.exists = true;
      result.length = bytes.length;
      const oldFormat = parseTileDataFirstLand(bytes, true);
      const newFormat = parseTileDataFirstLand(bytes, false);
      const selected = looksLikeTileName(oldFormat.firstLandName) && !looksLikeTileName(newFormat.firstLandName)
        ? oldFormat
        : (!looksLikeTileName(oldFormat.firstLandName) && looksLikeTileName(newFormat.firstLandName) ? newFormat : oldFormat);

      result.isOldFormat = selected.isOldFormat;
      result.header = selected.header;
      result.firstLandFlags = selected.firstLandFlags;
      result.firstLandTextureId = selected.firstLandTextureId;
      result.firstLandName = selected.firstLandName;
      result.totalMs = performance.now() - started;
      return result;
    } catch (error) {
      result.error = error instanceof Error ? error.message : String(error);
      return result;
    }
  },

  downloadTextFile: async function (fileName, contents) {
    const blob = new Blob([contents ?? ''], { type: 'application/json;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName || 'classicuo-browser-report.json';
    document.body.appendChild(anchor);
    anchor.click();
    anchor.remove();
    setTimeout(() => URL.revokeObjectURL(url), 0);
  }
};
