namespace BrowserHost.Services;

public sealed class BrowserVirtualFileBridgeService
{
    public const string AssetsRoot = "/uo";
    public const string ProfilesRoot = "/profiles";
    public const string CacheRoot = "/cache";
    public const string ConfigRoot = "/config";

    private readonly BrowserStorageService _storageService;

    public BrowserVirtualFileBridgeService(BrowserStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<BrowserVirtualFileProbeResult> ProbePathAsync(string path)
    {
        string normalizedPath = NormalizePath(path);

        try
        {
            long started = Environment.TickCount64;
            bool exists = await _storageService.FileExistsAsync(normalizedPath);
            long afterExists = Environment.TickCount64;
            var textResult = await _storageService.ReadTextAsync(normalizedPath);
            long afterText = Environment.TickCount64;
            var bytesResult = await _storageService.ReadBytesBase64Async(normalizedPath);
            long afterBytes = Environment.TickCount64;

            return new BrowserVirtualFileProbeResult
            {
                Path = normalizedPath,
                Exists = exists || textResult.Exists || bytesResult.Exists,
                TextPreview = textResult.Exists ? Truncate(textResult.Text, 200) : string.Empty,
                ByteLength = bytesResult.Exists ? bytesResult.Length : 0,
                ExistsMs = afterExists - started,
                TextReadMs = afterText - afterExists,
                BytesReadMs = afterBytes - afterText,
                TotalMs = afterBytes - started,
                Error = FirstError(textResult.Error, bytesResult.Error)
            };
        }
        catch (Exception ex)
        {
            return new BrowserVirtualFileProbeResult
            {
                Path = normalizedPath,
                Error = ex.Message
            };
        }
    }

    public async Task<BrowserVirtualFileWriteProbeResult> WriteTextProbeAsync(string path, string contents)
    {
        string normalizedPath = NormalizePath(path);

        try
        {
            long started = Environment.TickCount64;
            BrowserFileWriteResult writeResult = await _storageService.WriteTextAsync(normalizedPath, contents);
            long finished = Environment.TickCount64;

            return new BrowserVirtualFileWriteProbeResult
            {
                Path = normalizedPath,
                Succeeded = writeResult.Succeeded,
                ByteLength = writeResult.Length,
                WriteMs = finished - started,
                Error = writeResult.Error
            };
        }
        catch (Exception ex)
        {
            return new BrowserVirtualFileWriteProbeResult
            {
                Path = normalizedPath,
                Error = ex.Message
            };
        }
    }

    public Task<BrowserVirtualFileProbeResult> ProbeAssetPathAsync(string relativePath)
    {
        return ProbePathAsync(Combine(AssetsRoot, relativePath));
    }

    public Task<BrowserVirtualFileWriteProbeResult> WriteAssetTextProbeAsync(string relativePath, string contents)
    {
        return WriteTextProbeAsync(Combine(AssetsRoot, relativePath), contents);
    }

    public async Task<BrowserVirtualRootSummary[]> GetRootSummariesAsync()
    {
        BrowserStoredFilesResult assets = await _storageService.ListFilesUnderPathAsync(AssetsRoot);
        BrowserStoredFilesResult profiles = await _storageService.ListFilesUnderPathAsync(ProfilesRoot);
        BrowserStoredFilesResult cache = await _storageService.ListFilesUnderPathAsync(CacheRoot);
        BrowserStoredFilesResult config = await _storageService.ListFilesUnderPathAsync(ConfigRoot);

        return
        [
            ToSummary("Assets", AssetsRoot, assets),
            ToSummary("Profiles", ProfilesRoot, profiles),
            ToSummary("Cache", CacheRoot, cache),
            ToSummary("Config", ConfigRoot, config)
        ];
    }

    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Combine(AssetsRoot, "probe.txt");
        }

        return path.StartsWith('/') ? path : "/" + path;
    }

    private static string Combine(string root, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return root;
        }

        return $"{root.TrimEnd('/')}/{relativePath.TrimStart('/').Replace('\\', '/')}";
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value;
        }

        return value[..maxLength] + "...";
    }

    private static string FirstError(params string[] errors)
    {
        foreach (string error in errors)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }
        }

        return string.Empty;
    }

    private static BrowserVirtualRootSummary ToSummary(string name, string rootPath, BrowserStoredFilesResult result)
    {
        return new BrowserVirtualRootSummary
        {
            Name = name,
            RootPath = rootPath,
            FileCount = result.FileNames.Length,
            SampleFiles = result.FileNames.Take(5).ToArray(),
            Error = result.Error
        };
    }
}

public sealed class BrowserVirtualFileProbeResult
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public int ByteLength { get; set; }
    public long ExistsMs { get; set; }
    public long TextReadMs { get; set; }
    public long BytesReadMs { get; set; }
    public long TotalMs { get; set; }
    public string TextPreview { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserVirtualFileWriteProbeResult
{
    public string Path { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public int ByteLength { get; set; }
    public long WriteMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserVirtualRootSummary
{
    public string Name { get; set; } = string.Empty;
    public string RootPath { get; set; } = string.Empty;
    public int FileCount { get; set; }
    public string[] SampleFiles { get; set; } = Array.Empty<string>();
    public string Error { get; set; } = string.Empty;
}
