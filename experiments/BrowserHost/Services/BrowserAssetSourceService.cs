namespace BrowserHost.Services;

public interface IBrowserAssetStreamSource
{
    ValueTask<bool> FileExistsAsync(string path);
    ValueTask<BrowserStoredFilesResult> ListFilesUnderPathAsync(string path);
    ValueTask<int> GetFileLengthAsync(string path);
    ValueTask<byte[]> ReadAllBytesAsync(string path);
    ValueTask<Stream> OpenReadAsync(string path);
    ValueTask<BrowserAssetSourceCacheSummary> GetCacheSummaryAsync();
    ValueTask<BrowserAssetWarmResult> WarmPathAsync(string path);
    ValueTask ClearCacheAsync();
}

public sealed class BrowserAssetSourceService : IBrowserAssetStreamSource
{
    private readonly BrowserStorageService _storageService;
    private readonly Dictionary<string, byte[]> _cachedFiles = new(StringComparer.OrdinalIgnoreCase);

    public BrowserAssetSourceService(BrowserStorageService storageService)
    {
        _storageService = storageService;
    }

    public ValueTask<bool> FileExistsAsync(string path)
    {
        return FileExistsCoreAsync(path);
    }

    public async ValueTask<BrowserStoredFilesResult> ListFilesUnderPathAsync(string path)
    {
        BrowserStoredFilesResult result = await _storageService.ListFilesUnderPathAsync(path);
        result.FileNames = result.FileNames
            .Select(EnsureLeadingSlash)
            .ToArray();

        return result;
    }

    public async ValueTask<int> GetFileLengthAsync(string path)
    {
        BrowserFileBytesResult result = await ReadBytesResultAsync(path);
        return result.Exists ? result.Length : 0;
    }

    public async ValueTask<byte[]> ReadAllBytesAsync(string path)
    {
        string resolvedPath = await ResolvePathAsync(path);

        if (string.IsNullOrEmpty(resolvedPath))
        {
            throw new FileNotFoundException(path);
        }

        if (_cachedFiles.TryGetValue(resolvedPath, out byte[]? cachedBytes))
        {
            return cachedBytes;
        }

        BrowserFileBytesResult result = await ReadBytesResultAsync(resolvedPath);

        if (!result.Exists)
        {
            throw new FileNotFoundException(path);
        }

        byte[] bytes = string.IsNullOrEmpty(result.Base64) ? Array.Empty<byte>() : Convert.FromBase64String(result.Base64);
        _cachedFiles[resolvedPath] = bytes;
        return bytes;
    }

    public async ValueTask<Stream> OpenReadAsync(string path)
    {
        byte[] bytes = await ReadAllBytesAsync(path);
        return new MemoryStream(bytes, writable: false);
    }

    public ValueTask<BrowserAssetSourceCacheSummary> GetCacheSummaryAsync()
    {
        BrowserAssetSourceCacheSummary summary = new()
        {
            EntryCount = _cachedFiles.Count,
            TotalBytes = _cachedFiles.Values.Sum(static x => (long) x.Length),
            Paths = _cachedFiles.Keys.OrderBy(static x => x, StringComparer.OrdinalIgnoreCase).ToArray()
        };

        return ValueTask.FromResult(summary);
    }

    public async ValueTask<BrowserAssetWarmResult> WarmPathAsync(string path)
    {
        BrowserAssetWarmResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            string resolvedPath = await ResolvePathAsync(path);

            if (string.IsNullOrEmpty(resolvedPath))
            {
                result.Error = "File does not exist.";
                return result;
            }

            result.Path = resolvedPath;
            result.WasCached = _cachedFiles.ContainsKey(resolvedPath);
            byte[] bytes = await ReadAllBytesAsync(resolvedPath);
            result.Succeeded = true;
            result.Length = bytes.Length;
            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
            return result;
        }
    }

    public ValueTask ClearCacheAsync()
    {
        _cachedFiles.Clear();
        return ValueTask.CompletedTask;
    }

    private async ValueTask<bool> FileExistsCoreAsync(string path)
    {
        string resolvedPath = await ResolvePathAsync(path);
        return !string.IsNullOrEmpty(resolvedPath);
    }

    private async ValueTask<BrowserFileBytesResult> ReadBytesResultAsync(string path)
    {
        string resolvedPath = await ResolvePathAsync(path);

        if (string.IsNullOrEmpty(resolvedPath))
        {
            return new BrowserFileBytesResult
            {
                Path = path,
                Exists = false
            };
        }

        BrowserFileBytesResult result = await _storageService.ReadBytesBase64Async(resolvedPath);
        result.Path = EnsureLeadingSlash(result.Path);

        return result;
    }

    private async ValueTask<string> ResolvePathAsync(string path)
    {
        string normalizedPath = EnsureLeadingSlash(path);

        if (await _storageService.FileExistsAsync(normalizedPath))
        {
            return normalizedPath;
        }

        string parentPath = GetParentPath(normalizedPath);
        BrowserStoredFilesResult listing = await _storageService.ListFilesUnderPathAsync(parentPath);

        foreach (string fileName in listing.FileNames)
        {
            string candidate = EnsureLeadingSlash(fileName);

            if (string.Equals(candidate, normalizedPath, StringComparison.OrdinalIgnoreCase))
            {
                return candidate;
            }
        }

        return string.Empty;
    }

    private static string GetParentPath(string path)
    {
        int lastSeparator = path.LastIndexOf('/');

        if (lastSeparator <= 0)
        {
            return "/";
        }

        return path[..lastSeparator];
    }

    private static string EnsureLeadingSlash(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        return path[0] == '/' ? path : "/" + path;
    }
}
