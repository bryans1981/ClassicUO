namespace BrowserHost.Services;

public interface IBrowserSharedSeamRuntimeBootstrapAssets
{
    ValueTask<BrowserSharedSeamRuntimeBootstrapState> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null);
}

public sealed class BrowserSharedSeamBootstrapService : IBrowserSharedSeamRuntimeBootstrapAssets
{
    private readonly BrowserSharedSeamAssetService _sharedSeamAssetService;

    public BrowserSharedSeamBootstrapService(BrowserSharedSeamAssetService sharedSeamAssetService)
    {
        _sharedSeamAssetService = sharedSeamAssetService;
    }

    public async ValueTask<BrowserSharedSeamRuntimeBootstrapState> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserRuntimeBootstrapRequest effectiveRequest = request ?? new BrowserRuntimeBootstrapRequest();
        BrowserSharedSeamBootstrapSnapshot snapshot = await LoadSnapshotAsync(effectiveRequest);

        return new BrowserSharedSeamRuntimeBootstrapState
        {
            TileDataPath = effectiveRequest.TileDataPath,
            ClilocPath = effectiveRequest.ClilocPath,
            HuesPath = effectiveRequest.HuesPath,
            IsReady = snapshot.IsReady,
            LoadedCount = snapshot.LoadedCount,
            CacheHits = snapshot.CacheHits,
            TotalMs = snapshot.TotalMs,
            TileData = new BrowserSharedSeamRuntimeTileDataSummary
            {
                Exists = snapshot.TileData.Exists,
                ReadSucceeded = snapshot.TileData.ReadSucceeded,
                LoadedOnDemand = snapshot.TileData.LoadedOnDemand,
                UsedParsedCache = snapshot.TileData.UsedParsedCache,
                Length = snapshot.TileData.Length,
                FirstLandName = snapshot.TileData.FirstLandName,
                FirstLandTextureId = snapshot.TileData.FirstLandTextureId
            },
            Cliloc = new BrowserSharedSeamRuntimeClilocSummary
            {
                Exists = snapshot.Cliloc.Exists,
                ReadSucceeded = snapshot.Cliloc.ReadSucceeded,
                LoadedOnDemand = snapshot.Cliloc.LoadedOnDemand,
                UsedParsedCache = snapshot.Cliloc.UsedParsedCache,
                Length = snapshot.Cliloc.Length,
                FirstEntryNumber = snapshot.Cliloc.FirstEntryNumber,
                FirstEntryText = snapshot.Cliloc.FirstEntryText
            },
            Hues = new BrowserSharedSeamRuntimeHuesSummary
            {
                Exists = snapshot.Hues.Exists,
                ReadSucceeded = snapshot.Hues.ReadSucceeded,
                LoadedOnDemand = snapshot.Hues.LoadedOnDemand,
                UsedParsedCache = snapshot.Hues.UsedParsedCache,
                Length = snapshot.Hues.Length,
                FirstGroupHeader = snapshot.Hues.FirstGroupHeader,
                FirstHueName = snapshot.Hues.FirstHueName
            }
        };
    }

    public async ValueTask<BrowserSharedSeamBootstrapSnapshot> LoadSnapshotAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserRuntimeBootstrapRequest effectiveRequest = request ?? new BrowserRuntimeBootstrapRequest();
        var started = DateTime.UtcNow;

        BrowserSharedSeamTileDataResult tileData = await _sharedSeamAssetService.ReadFirstTileDataEntryAsync(effectiveRequest.TileDataPath);
        BrowserSharedSeamClilocResult cliloc = await _sharedSeamAssetService.ReadFirstClilocEntryAsync(effectiveRequest.ClilocPath);
        BrowserSharedSeamHuesResult hues = await _sharedSeamAssetService.ReadFirstHuesEntryAsync(effectiveRequest.HuesPath);

        return new BrowserSharedSeamBootstrapSnapshot
        {
            TileData = tileData,
            Cliloc = cliloc,
            Hues = hues,
            LoadedCount = CountLoaded(tileData, cliloc, hues),
            CacheHits = CountCacheHits(tileData, cliloc, hues),
            IsReady = tileData.Exists && tileData.ReadSucceeded
                && cliloc.Exists && cliloc.ReadSucceeded
                && hues.Exists && hues.ReadSucceeded,
            TotalMs = (DateTime.UtcNow - started).TotalMilliseconds
        };
    }

    private static int CountLoaded(
        BrowserSharedSeamTileDataResult tileData,
        BrowserSharedSeamClilocResult cliloc,
        BrowserSharedSeamHuesResult hues
    )
    {
        int count = 0;

        if (tileData.Exists && tileData.ReadSucceeded)
        {
            count++;
        }

        if (cliloc.Exists && cliloc.ReadSucceeded)
        {
            count++;
        }

        if (hues.Exists && hues.ReadSucceeded)
        {
            count++;
        }

        return count;
    }

    private static int CountCacheHits(
        BrowserSharedSeamTileDataResult tileData,
        BrowserSharedSeamClilocResult cliloc,
        BrowserSharedSeamHuesResult hues
    )
    {
        int count = 0;

        if (tileData.UsedParsedCache)
        {
            count++;
        }

        if (cliloc.UsedParsedCache)
        {
            count++;
        }

        if (hues.UsedParsedCache)
        {
            count++;
        }

        return count;
    }
}

public sealed class BrowserSharedSeamBootstrapSnapshot
{
    public bool IsReady { get; set; }
    public int LoadedCount { get; set; }
    public int CacheHits { get; set; }
    public double TotalMs { get; set; }
    public BrowserSharedSeamTileDataResult TileData { get; set; } = new();
    public BrowserSharedSeamClilocResult Cliloc { get; set; } = new();
    public BrowserSharedSeamHuesResult Hues { get; set; } = new();
}
