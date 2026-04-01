using ClassicUO.Utility.Platforms;

namespace BrowserHost.Services;

public sealed class BrowserFileSystemBridgeService
{
    private readonly IBrowserAssetStreamSource _assetStreamSource;
    private readonly BridgeBinaryAssetSource _binaryAssetSource;
    private bool _providerActivated;

    public BrowserFileSystemBridgeService(IBrowserAssetStreamSource assetStreamSource)
    {
        _assetStreamSource = assetStreamSource;
        _binaryAssetSource = new BridgeBinaryAssetSource(assetStreamSource);
    }

    public async ValueTask<BrowserFileSystemBridgeState> ActivateBootstrapAssetsAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserRuntimeBootstrapRequest effectiveRequest = request ?? new BrowserRuntimeBootstrapRequest();
        string[] candidatePaths =
        {
            effectiveRequest.TileDataPath,
            effectiveRequest.ClilocPath,
            effectiveRequest.HuesPath
        };

        EnsureProviderActivated();
        await _binaryAssetSource.EnsureFilesLoadedAsync(candidatePaths);

        return new BrowserFileSystemBridgeState
        {
            Activated = _providerActivated,
            LoadedCount = _binaryAssetSource.LoadedCount,
            LoadedPaths = _binaryAssetSource.GetLoadedPaths()
        };
    }

    public async ValueTask<BrowserFileSystemBridgeProbeResult> ProbePathAsync(string path)
    {
        var result = new BrowserFileSystemBridgeProbeResult
        {
            Path = BrowserFileSystem.NormalizePath(path)
        };

        var started = DateTime.UtcNow;

        try
        {
            EnsureProviderActivated();
            result.LoadedOnDemand = await _binaryAssetSource.EnsureFileLoadedAsync(result.Path);

            var fileSystem = new BrowserFileSystem();
            result.Exists = fileSystem.FileExists(result.Path);

            if (result.Exists)
            {
                result.Length = fileSystem.GetFileLength(result.Path);

                using Stream stream = fileSystem.OpenRead(result.Path);
                result.ReadSucceeded = stream.Length == result.Length;
            }

            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
        }

        return result;
    }

    public async ValueTask<BrowserFileSystemBridgeReadResult> ReadBytesThroughSharedSeamAsync(string path)
    {
        var result = new BrowserFileSystemBridgeReadResult
        {
            Path = BrowserFileSystem.NormalizePath(path)
        };

        var started = DateTime.UtcNow;

        try
        {
            EnsureProviderActivated();
            result.LoadedOnDemand = await _binaryAssetSource.EnsureFileLoadedAsync(result.Path);

            var fileSystem = new BrowserFileSystem();
            result.Exists = fileSystem.FileExists(result.Path);

            if (result.Exists)
            {
                result.Length = fileSystem.GetFileLength(result.Path);

                using Stream stream = fileSystem.OpenRead(result.Path);
                using MemoryStream buffer = new MemoryStream();
                stream.CopyTo(buffer);
                result.Bytes = buffer.ToArray();
                result.ReadSucceeded = result.Bytes.Length == result.Length;
            }

            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
        }

        return result;
    }

    public ValueTask<BrowserFileSystemBridgeState> GetStateAsync()
    {
        return ValueTask.FromResult(new BrowserFileSystemBridgeState
        {
            Activated = _providerActivated,
            LoadedCount = _binaryAssetSource.LoadedCount,
            LoadedPaths = _binaryAssetSource.GetLoadedPaths()
        });
    }

    private void EnsureProviderActivated()
    {
        if (_providerActivated)
        {
            return;
        }

        BrowserFileSystem.SetProvider(new BrowserBinaryAssetStorageProvider(_binaryAssetSource));
        _providerActivated = true;
    }
}

public sealed class BrowserFileSystemBridgeState
{
    public bool Activated { get; set; }
    public int LoadedCount { get; set; }
    public string[] LoadedPaths { get; set; } = Array.Empty<string>();
}

public sealed class BrowserFileSystemBridgeProbeResult
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool ReadSucceeded { get; set; }
    public long Length { get; set; }
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserFileSystemBridgeReadResult
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool ReadSucceeded { get; set; }
    public long Length { get; set; }
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

internal sealed class BridgeBinaryAssetSource : IBrowserBinaryAssetSource
{
    private readonly IBrowserAssetStreamSource _assetStreamSource;
    private readonly Dictionary<string, byte[]> _files = new(StringComparer.OrdinalIgnoreCase);

    public BridgeBinaryAssetSource(IBrowserAssetStreamSource assetStreamSource)
    {
        _assetStreamSource = assetStreamSource;
    }

    public int LoadedCount => _files.Count;

    public async ValueTask<bool> EnsureFileLoadedAsync(string path)
    {
        string normalizedPath = BrowserFileSystem.NormalizePath(path);

        if (_files.ContainsKey(normalizedPath))
        {
            return false;
        }

        if (!await _assetStreamSource.FileExistsAsync(normalizedPath))
        {
            return false;
        }

        byte[] bytes = await _assetStreamSource.ReadAllBytesAsync(normalizedPath);
        _files[normalizedPath] = bytes;
        return true;
    }

    public async ValueTask EnsureFilesLoadedAsync(IEnumerable<string> paths)
    {
        foreach (string path in paths.Where(static x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            await EnsureFileLoadedAsync(path);
        }
    }

    public string[] GetLoadedPaths()
    {
        return _files.Keys.OrderBy(static x => x, StringComparer.OrdinalIgnoreCase).ToArray();
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
        if (_files.TryGetValue(BrowserFileSystem.NormalizePath(path), out byte[]? buffer))
        {
            bytes = buffer;
            return true;
        }

        bytes = default;
        return false;
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
