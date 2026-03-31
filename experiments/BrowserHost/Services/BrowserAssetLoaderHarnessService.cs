namespace BrowserHost.Services;

public sealed class BrowserAssetLoaderHarnessService
{
    private readonly IBrowserAssetStreamSource _assetSourceService;
    private readonly BrowserProcessedAssetCacheService _processedCache;

    public BrowserAssetLoaderHarnessService(IBrowserAssetStreamSource assetSourceService, BrowserProcessedAssetCacheService processedCache)
    {
        _assetSourceService = assetSourceService;
        _processedCache = processedCache;
    }

    public async ValueTask<BrowserAssetLoadResult<TParsed>> LoadParsedAsync<TProcessed, TParsed>(
        string path,
        Func<byte[], TProcessed> preprocess,
        Func<TProcessed, TParsed> parse,
        string processedCacheKey,
        string parsedCacheKey
    )
    {
        BrowserAssetLoadResult<TParsed> result = new()
        {
            Path = path
        };

        result.Exists = await _assetSourceService.FileExistsAsync(path);

        if (!result.Exists)
        {
            return result;
        }

        byte[] bytes = await _assetSourceService.ReadAllBytesAsync(path);
        result.Length = bytes.Length;

        result.UsedProcessedCache = _processedCache.Contains(processedCacheKey);
        TProcessed processed = await _processedCache.GetOrAddAsync(
            processedCacheKey,
            () => ValueTask.FromResult(preprocess(bytes))
        );

        result.UsedParsedCache = _processedCache.Contains(parsedCacheKey);
        result.Value = await _processedCache.GetOrAddAsync(
            parsedCacheKey,
            () => ValueTask.FromResult(parse(processed))
        );
        result.HasValue = true;

        return result;
    }
}

public sealed class BrowserAssetLoadResult<T>
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool UsedProcessedCache { get; set; }
    public bool UsedParsedCache { get; set; }
    public bool HasValue { get; set; }
    public T Value { get; set; } = default!;
}
