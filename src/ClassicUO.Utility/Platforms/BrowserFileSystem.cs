// SPDX-License-Identifier: BSD-2-Clause

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ClassicUO.Utility.Platforms
{
    public interface IBrowserStorageProvider
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        Stream OpenRead(string path);
        Stream OpenReadWrite(string path);
        Stream OpenWrite(string path);
        Stream OpenAppend(string path);
        long GetFileLength(string path);
        void CopyFile(string sourcePath, string destinationPath, bool overwrite);
        void DeleteFile(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }

    public interface IBrowserBinaryAssetSource
    {
        bool FileExists(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        Stream OpenReadStream(string path);
        bool TryReadFile(string path, out ReadOnlyMemory<byte> bytes);
    }

    public sealed class BrowserBinaryAssetStorageProvider : IBrowserStorageProvider
    {
        private readonly IBrowserBinaryAssetSource _source;

        public BrowserBinaryAssetStorageProvider(IBrowserBinaryAssetSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public bool FileExists(string path) => _source.FileExists(BrowserFileSystem.NormalizePath(path));

        public bool DirectoryExists(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            return _source.GetFiles(normalizedPath).Length != 0;
        }

        public void CreateDirectory(string path) => throw ReadOnly(path);

        public string[] GetFiles(string path) => _source.GetFiles(BrowserFileSystem.NormalizePath(path));

        public string[] GetFiles(string path, string searchPattern) => _source.GetFiles(BrowserFileSystem.NormalizePath(path), searchPattern);

        public Stream OpenRead(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);

            if (!_source.FileExists(normalizedPath))
            {
                throw new FileNotFoundException(normalizedPath);
            }

            return _source.OpenReadStream(normalizedPath);
        }

        public Stream OpenReadWrite(string path) => throw ReadOnly(path);

        public Stream OpenWrite(string path) => throw ReadOnly(path);

        public Stream OpenAppend(string path) => throw ReadOnly(path);

        public long GetFileLength(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);

            if (!_source.TryReadFile(normalizedPath, out ReadOnlyMemory<byte> bytes))
            {
                throw new FileNotFoundException(normalizedPath);
            }

            return bytes.Length;
        }

        public void CopyFile(string sourcePath, string destinationPath, bool overwrite) => throw ReadOnly(destinationPath);

        public void DeleteFile(string path) => throw ReadOnly(path);

        public string ReadAllText(string path)
        {
            using Stream stream = OpenRead(path);
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            return reader.ReadToEnd();
        }

        public void WriteAllText(string path, string contents) => throw ReadOnly(path);

        private static NotSupportedException ReadOnly(string path)
        {
            return new NotSupportedException($"Browser asset path '{BrowserFileSystem.NormalizePath(path)}' is read-only.");
        }
    }

    public sealed class RootedBrowserStorageProvider : IBrowserStorageProvider
    {
        private readonly IBrowserStorageProvider _assetsProvider;
        private readonly IBrowserStorageProvider _profilesProvider;
        private readonly IBrowserStorageProvider _cacheProvider;
        private readonly IBrowserStorageProvider _configProvider;

        public RootedBrowserStorageProvider
        (
            IBrowserStorageProvider assetsProvider,
            IBrowserStorageProvider profilesProvider,
            IBrowserStorageProvider cacheProvider,
            IBrowserStorageProvider configProvider
        )
        {
            _assetsProvider = assetsProvider ?? throw new ArgumentNullException(nameof(assetsProvider));
            _profilesProvider = profilesProvider ?? throw new ArgumentNullException(nameof(profilesProvider));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        public bool FileExists(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.FileExists(entry.RelativePath);
        }

        public bool DirectoryExists(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.DirectoryExists(entry.RelativePath);
        }

        public void CreateDirectory(string path)
        {
            var entry = Resolve(path);
            entry.Provider.CreateDirectory(entry.RelativePath);
        }

        public string[] GetFiles(string path)
        {
            var entry = Resolve(path);
            return Prefix(entry, entry.Provider.GetFiles(entry.RelativePath));
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            var entry = Resolve(path);
            return Prefix(entry, entry.Provider.GetFiles(entry.RelativePath, searchPattern));
        }

        public Stream OpenRead(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.OpenRead(entry.RelativePath);
        }

        public Stream OpenReadWrite(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.OpenReadWrite(entry.RelativePath);
        }

        public Stream OpenWrite(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.OpenWrite(entry.RelativePath);
        }

        public Stream OpenAppend(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.OpenAppend(entry.RelativePath);
        }

        public long GetFileLength(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.GetFileLength(entry.RelativePath);
        }

        public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        {
            var source = Resolve(sourcePath);
            var destination = Resolve(destinationPath);

            if (ReferenceEquals(source.Provider, destination.Provider))
            {
                source.Provider.CopyFile(source.RelativePath, destination.RelativePath, overwrite);
                return;
            }

            using Stream input = source.Provider.OpenRead(source.RelativePath);
            using Stream output = destination.Provider.OpenWrite(destination.RelativePath);
            input.CopyTo(output);
        }

        public void DeleteFile(string path)
        {
            var entry = Resolve(path);
            entry.Provider.DeleteFile(entry.RelativePath);
        }

        public string ReadAllText(string path)
        {
            var entry = Resolve(path);
            return entry.Provider.ReadAllText(entry.RelativePath);
        }

        public void WriteAllText(string path, string contents)
        {
            var entry = Resolve(path);
            entry.Provider.WriteAllText(entry.RelativePath, contents);
        }

        private ResolvedPath Resolve(string path)
        {
            BrowserVirtualPathInfo pathInfo = BrowserVirtualPaths.Classify(path);

            return pathInfo.Root switch
            {
                BrowserVirtualRoot.Assets => new ResolvedPath(_assetsProvider, BrowserVirtualPaths.AssetsRoot, ToRelative(pathInfo.RelativePath)),
                BrowserVirtualRoot.Profiles => new ResolvedPath(_profilesProvider, BrowserVirtualPaths.ProfilesRoot, ToRelative(pathInfo.RelativePath)),
                BrowserVirtualRoot.Cache => new ResolvedPath(_cacheProvider, BrowserVirtualPaths.CacheRoot, ToRelative(pathInfo.RelativePath)),
                BrowserVirtualRoot.Config => new ResolvedPath(_configProvider, BrowserVirtualPaths.ConfigRoot, ToRelative(pathInfo.RelativePath)),
                _ => throw new InvalidOperationException($"Browser path '{pathInfo.FullPath}' is not under a known virtual root.")
            };
        }

        private static string[] Prefix(ResolvedPath resolvedPath, string[] relativePaths)
        {
            return relativePaths.Select(path => BrowserVirtualPaths.Combine(resolvedPath.RootPath, path)).ToArray();
        }

        private static string ToRelative(string relativePath)
        {
            return string.IsNullOrWhiteSpace(relativePath) ? "/" : "/" + relativePath.TrimStart('/');
        }

        private readonly struct ResolvedPath
        {
            public ResolvedPath(IBrowserStorageProvider provider, string rootPath, string relativePath)
            {
                Provider = provider;
                RootPath = rootPath;
                RelativePath = relativePath;
            }

            public IBrowserStorageProvider Provider { get; }
            public string RootPath { get; }
            public string RelativePath { get; }
        }
    }

    internal sealed class BrowserFileSystem : IClassicUOFileSystem
    {
        private static IBrowserStorageProvider _provider;

        public static void SetProvider(IBrowserStorageProvider provider)
        {
            _provider = provider;
        }

        internal static bool IsProviderConfigured => _provider is not null;

        private static PlatformNotSupportedException NotReady(string operation)
        {
            return new PlatformNotSupportedException($"Browser filesystem operation '{operation}' is not implemented yet.");
        }

        internal static string NormalizePath(string path)
        {
            return BrowserVirtualPaths.Normalize(path);
        }

        public bool FileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return _provider?.FileExists(NormalizePath(path)) ?? false;
        }

        public bool DirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return _provider?.DirectoryExists(NormalizePath(path)) ?? false;
        }

        public void CreateDirectory(string path)
        {
            _provider?.CreateDirectory(NormalizePath(path));
        }

        public string[] GetFiles(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Array.Empty<string>();
            }

            return _provider?.GetFiles(NormalizePath(path)) ?? Array.Empty<string>();
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Array.Empty<string>();
            }

            return _provider?.GetFiles(NormalizePath(path), searchPattern) ?? Array.Empty<string>();
        }

        public Stream OpenRead(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(OpenRead));
            }

            return _provider.OpenRead(NormalizePath(path));
        }

        public Stream OpenWrite(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(OpenWrite));
            }

            return _provider.OpenWrite(NormalizePath(path));
        }

        public Stream OpenReadWrite(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(OpenReadWrite));
            }

            return _provider.OpenReadWrite(NormalizePath(path));
        }

        public Stream OpenAppend(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(OpenAppend));
            }

            return _provider.OpenAppend(NormalizePath(path));
        }

        public long GetFileLength(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(GetFileLength));
            }

            return _provider.GetFileLength(NormalizePath(path));
        }

        public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(CopyFile));
            }

            _provider.CopyFile(NormalizePath(sourcePath), NormalizePath(destinationPath), overwrite);
        }

        public void DeleteFile(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(DeleteFile));
            }

            _provider.DeleteFile(NormalizePath(path));
        }

        public string ReadAllText(string path)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(ReadAllText));
            }

            return _provider.ReadAllText(NormalizePath(path));
        }

        public void WriteAllText(string path, string contents)
        {
            if (_provider is null)
            {
                throw NotReady(nameof(WriteAllText));
            }

            _provider.WriteAllText(NormalizePath(path), contents);
        }
    }

    public sealed class InMemoryBrowserStorageProvider : IBrowserStorageProvider
    {
        private readonly Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _directories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "/"
        };

        public bool FileExists(string path) => _files.ContainsKey(BrowserFileSystem.NormalizePath(path));

        public bool DirectoryExists(string path) => _directories.Contains(BrowserFileSystem.NormalizePath(path));

        public void CreateDirectory(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            EnsureDirectory(normalizedPath);
        }

        public string[] GetFiles(string path)
        {
            string directoryPath = BrowserFileSystem.NormalizePath(path);

            return _files.Keys
                         .Where(filePath => string.Equals(GetDirectoryName(filePath), directoryPath, StringComparison.OrdinalIgnoreCase))
                         .OrderBy(filePath => filePath, StringComparer.OrdinalIgnoreCase)
                         .ToArray();
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return GetFiles(path)
                   .Where(filePath => MatchesSearchPattern(filePath, searchPattern))
                   .ToArray();
        }

        public Stream OpenRead(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);

            if (!_files.TryGetValue(normalizedPath, out byte[] buffer))
            {
                throw new FileNotFoundException(normalizedPath);
            }

            return new MemoryStream(buffer, writable: false);
        }

        public Stream OpenWrite(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            string directoryPath = GetDirectoryName(normalizedPath);

            EnsureDirectory(directoryPath);

            return new CommitOnDisposeStream(bytes => _files[normalizedPath] = bytes);
        }

        public Stream OpenReadWrite(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            string directoryPath = GetDirectoryName(normalizedPath);

            EnsureDirectory(directoryPath);

            byte[] existing = _files.TryGetValue(normalizedPath, out byte[] current) ? current : Array.Empty<byte>();
            return new CommitOnDisposeStream(bytes => _files[normalizedPath] = bytes, existing, resetPosition: true);
        }

        public Stream OpenAppend(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            string directoryPath = GetDirectoryName(normalizedPath);

            EnsureDirectory(directoryPath);

            byte[] existing = _files.TryGetValue(normalizedPath, out byte[] current) ? current : Array.Empty<byte>();
            return new CommitOnDisposeStream(bytes => _files[normalizedPath] = bytes, existing);
        }

        public long GetFileLength(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);

            if (!_files.TryGetValue(normalizedPath, out byte[] buffer))
            {
                throw new FileNotFoundException(normalizedPath);
            }

            return buffer.LongLength;
        }

        public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        {
            string normalizedSourcePath = BrowserFileSystem.NormalizePath(sourcePath);
            string normalizedDestinationPath = BrowserFileSystem.NormalizePath(destinationPath);

            if (!_files.TryGetValue(normalizedSourcePath, out byte[] buffer))
            {
                throw new FileNotFoundException(normalizedSourcePath);
            }

            if (!overwrite && _files.ContainsKey(normalizedDestinationPath))
            {
                throw new IOException($"The destination file '{normalizedDestinationPath}' already exists.");
            }

            EnsureDirectory(GetDirectoryName(normalizedDestinationPath));
            _files[normalizedDestinationPath] = (byte[]) buffer.Clone();
        }

        public void DeleteFile(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            _files.Remove(normalizedPath);
        }

        public string ReadAllText(string path)
        {
            using Stream stream = OpenRead(path);
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            return reader.ReadToEnd();
        }

        public void WriteAllText(string path, string contents)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            string directoryPath = GetDirectoryName(normalizedPath);

            EnsureDirectory(directoryPath);
            _files[normalizedPath] = Encoding.UTF8.GetBytes(contents ?? string.Empty);
        }

        private void EnsureDirectory(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);

            if (_directories.Contains(normalizedPath))
            {
                return;
            }

            string current = "/";

            foreach (string segment in normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                current = current == "/" ? "/" + segment : current + "/" + segment;
                _directories.Add(current);
            }
        }

        private static string GetDirectoryName(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            int index = normalizedPath.LastIndexOf('/');

            if (index <= 0)
            {
                return "/";
            }

            return normalizedPath[..index];
        }

        private static bool MatchesSearchPattern(string path, string searchPattern)
        {
            string fileName = Path.GetFileName(path);

            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "*")
            {
                return true;
            }

            if (searchPattern.StartsWith("*.", StringComparison.Ordinal))
            {
                string extension = searchPattern[1..];
                return fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(fileName, searchPattern, StringComparison.OrdinalIgnoreCase);
        }

        private sealed class CommitOnDisposeStream : MemoryStream
        {
            private readonly Action<byte[]> _commit;

            public CommitOnDisposeStream(Action<byte[]> commit)
            {
                _commit = commit;
            }

            public CommitOnDisposeStream(Action<byte[]> commit, byte[] initialBuffer, bool resetPosition = false) : base()
            {
                _commit = commit;
                Write(initialBuffer, 0, initialBuffer.Length);
                Position = resetPosition ? 0 : Length;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _commit(ToArray());
                }

                base.Dispose(disposing);
            }
        }
    }

    public sealed class InMemoryBrowserBinaryAssetSource : IBrowserBinaryAssetSource
    {
        private readonly Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

        public void AddFile(string path, byte[] bytes)
        {
            _files[BrowserFileSystem.NormalizePath(path)] = bytes ?? Array.Empty<byte>();
        }

        public bool FileExists(string path) => _files.ContainsKey(BrowserFileSystem.NormalizePath(path));

        public string[] GetFiles(string path)
        {
            string directoryPath = BrowserFileSystem.NormalizePath(path);

            return _files.Keys
                         .Where(filePath => string.Equals(GetDirectoryName(filePath), directoryPath, StringComparison.OrdinalIgnoreCase))
                         .OrderBy(filePath => filePath, StringComparer.OrdinalIgnoreCase)
                         .ToArray();
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return GetFiles(path)
                   .Where(filePath => MatchesSearchPattern(filePath, searchPattern))
                   .ToArray();
        }

        public bool TryReadFile(string path, out ReadOnlyMemory<byte> bytes)
        {
            if (_files.TryGetValue(BrowserFileSystem.NormalizePath(path), out byte[] buffer))
            {
                bytes = buffer;
                return true;
            }

            bytes = default;
            return false;
        }

        public Stream OpenReadStream(string path)
        {
            if (TryReadFile(path, out ReadOnlyMemory<byte> bytes))
            {
                if (MemoryMarshal.TryGetArray(bytes, out ArraySegment<byte> segment) && segment.Array is not null)
                {
                    return new MemoryStream(segment.Array, segment.Offset, segment.Count, writable: false, publiclyVisible: true);
                }

                return new MemoryStream(bytes.ToArray(), writable: false);
            }

            throw new FileNotFoundException(BrowserFileSystem.NormalizePath(path));
        }

        private static string GetDirectoryName(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);
            int index = normalizedPath.LastIndexOf('/');

            if (index <= 0)
            {
                return "/";
            }

            return normalizedPath[..index];
        }

        private static bool MatchesSearchPattern(string path, string searchPattern)
        {
            string fileName = Path.GetFileName(path);

            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "*")
            {
                return true;
            }

            if (searchPattern.StartsWith("*.", StringComparison.Ordinal))
            {
                string extension = searchPattern[1..];
                return fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(fileName, searchPattern, StringComparison.OrdinalIgnoreCase);
        }
    }
}
