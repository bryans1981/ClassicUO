using ClassicUO.Utility.Platforms;

namespace BrowserHost.Services;

public sealed class BrowserFileSystemBridgeService
{
    private readonly IBrowserAssetStreamSource _assetStreamSource;
    private readonly InMemoryBrowserBinaryAssetSource _binaryAssetSource = new();
    private readonly HashSet<string> _loadedPaths = new(StringComparer.OrdinalIgnoreCase);

    public BrowserFileSystemBridgeService(IBrowserAssetStreamSource assetStreamSource)
    {
        _assetStreamSource = assetStreamSource;
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

        var loaded = new List<string>();

        foreach (string path in candidatePaths.Where(static x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!await _assetStreamSource.FileExistsAsync(path))
            {
                continue;
            }

            byte[] bytes = await _assetStreamSource.ReadAllBytesAsync(path);
            _binaryAssetSource.AddFile(path, bytes);
            _loadedPaths.Add(BrowserFileSystem.NormalizePath(path));
            loaded.Add(BrowserFileSystem.NormalizePath(path));
        }

        BrowserFileSystem.SetProvider(new BrowserBinaryAssetStorageProvider(_binaryAssetSource));

        return new BrowserFileSystemBridgeState
        {
            Activated = true,
            LoadedCount = loaded.Count,
            LoadedPaths = loaded.OrderBy(static x => x, StringComparer.OrdinalIgnoreCase).ToArray()
        };
    }

    public ValueTask<BrowserFileSystemBridgeProbeResult> ProbePathAsync(string path)
    {
        var result = new BrowserFileSystemBridgeProbeResult
        {
            Path = BrowserFileSystem.NormalizePath(path)
        };

        var started = DateTime.UtcNow;

        try
        {
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

        return ValueTask.FromResult(result);
    }

    public ValueTask<BrowserFileSystemBridgeState> GetStateAsync()
    {
        return ValueTask.FromResult(new BrowserFileSystemBridgeState
        {
            Activated = _loadedPaths.Count > 0,
            LoadedCount = _loadedPaths.Count,
            LoadedPaths = _loadedPaths.OrderBy(static x => x, StringComparer.OrdinalIgnoreCase).ToArray()
        });
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
    public bool ReadSucceeded { get; set; }
    public long Length { get; set; }
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}
