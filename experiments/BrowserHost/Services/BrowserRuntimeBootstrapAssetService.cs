namespace BrowserHost.Services;

public interface IBrowserRuntimeBootstrapAssets
{
    ValueTask<BrowserRuntimeBootstrapState> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null);
    ValueTask<BrowserRuntimeCacheState> GetCacheStateAsync();
    ValueTask<BrowserAssetWarmResult> WarmAssetAsync(string path);
    ValueTask ClearCachesAsync();
}

public sealed class BrowserRuntimeBootstrapAssetService : IBrowserRuntimeBootstrapAssets
{
    private readonly BrowserBootstrapAssetService _bootstrapAssetService;
    private readonly BrowserAssetSourceService _assetSourceService;
    private readonly BrowserProcessedAssetCacheService _processedCacheService;

    public BrowserRuntimeBootstrapAssetService(
        BrowserBootstrapAssetService bootstrapAssetService,
        BrowserAssetSourceService assetSourceService,
        BrowserProcessedAssetCacheService processedCacheService
    )
    {
        _bootstrapAssetService = bootstrapAssetService;
        _assetSourceService = assetSourceService;
        _processedCacheService = processedCacheService;
    }

    public async ValueTask<BrowserRuntimeBootstrapState> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserBootstrapAssetRequest loaderRequest = request is null
            ? new BrowserBootstrapAssetRequest()
            : new BrowserBootstrapAssetRequest
            {
                TileDataPath = request.TileDataPath,
                ClilocPath = request.ClilocPath,
                HuesPath = request.HuesPath
            };

        BrowserBootstrapAssetSnapshot snapshot = await _bootstrapAssetService.LoadBootstrapSnapshotAsync(loaderRequest);

        return new BrowserRuntimeBootstrapState
        {
            TileDataPath = loaderRequest.TileDataPath,
            ClilocPath = loaderRequest.ClilocPath,
            HuesPath = loaderRequest.HuesPath,
            IsReady = snapshot.LoadedCount == 3,
            LoadedCount = snapshot.LoadedCount,
            CacheHits = snapshot.UsedCacheCount,
            TotalMs = snapshot.TotalMs,
            TileData = new BrowserRuntimeTileDataSummary
            {
                Exists = snapshot.TileData.Exists,
                Length = snapshot.TileData.Length,
                FirstLandName = snapshot.TileData.FirstLandName,
                FirstLandTextureId = snapshot.TileData.FirstLandTextureId
            },
            Cliloc = new BrowserRuntimeClilocSummary
            {
                Exists = snapshot.Cliloc.Exists,
                Length = snapshot.Cliloc.Length,
                FirstEntryNumber = snapshot.Cliloc.FirstEntryNumber,
                FirstEntryText = snapshot.Cliloc.FirstEntryText,
                UsedProcessedCache = snapshot.Cliloc.UsedProcessedCache
            },
            Hues = new BrowserRuntimeHuesSummary
            {
                Exists = snapshot.Hues.Exists,
                Length = snapshot.Hues.Length,
                FirstGroupHeader = snapshot.Hues.FirstGroupHeader,
                FirstHueName = snapshot.Hues.FirstHueName,
                UsedParsedCache = snapshot.Hues.UsedParsedCache
            }
        };
    }

    public async ValueTask<BrowserRuntimeCacheState> GetCacheStateAsync()
    {
        BrowserAssetSourceCacheSummary rawCache = await _assetSourceService.GetCacheSummaryAsync();
        BrowserProcessedAssetCacheSummary processedCache = _processedCacheService.GetSummary();

        return new BrowserRuntimeCacheState
        {
            RawAssetCache = rawCache,
            ProcessedAssetCache = processedCache
        };
    }

    public ValueTask<BrowserAssetWarmResult> WarmAssetAsync(string path)
    {
        return _assetSourceService.WarmPathAsync(path);
    }

    public async ValueTask ClearCachesAsync()
    {
        await _assetSourceService.ClearCacheAsync();
        _processedCacheService.Clear();
    }
}
