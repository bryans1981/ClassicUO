namespace BrowserHost.Services;

public sealed class BrowserBootstrapAssetService
{
    private readonly BrowserTileDataReaderService _tileDataReaderService;
    private readonly BrowserClilocReaderService _clilocReaderService;
    private readonly BrowserHuesReaderService _huesReaderService;

    public BrowserBootstrapAssetService(
        BrowserTileDataReaderService tileDataReaderService,
        BrowserClilocReaderService clilocReaderService,
        BrowserHuesReaderService huesReaderService
    )
    {
        _tileDataReaderService = tileDataReaderService;
        _clilocReaderService = clilocReaderService;
        _huesReaderService = huesReaderService;
    }

    public async ValueTask<BrowserBootstrapAssetSnapshot> LoadBootstrapSnapshotAsync(BrowserBootstrapAssetRequest? request = null)
    {
        BrowserBootstrapAssetRequest effectiveRequest = request ?? new BrowserBootstrapAssetRequest();
        var started = DateTime.UtcNow;

        BrowserTileDataReadResult tileData = await _tileDataReaderService.ReadAsync(effectiveRequest.TileDataPath);
        BrowserClilocReadResult cliloc = await _clilocReaderService.ReadAsync(effectiveRequest.ClilocPath);
        BrowserHuesReadResult hues = await _huesReaderService.ReadAsync(effectiveRequest.HuesPath);

        return new BrowserBootstrapAssetSnapshot
        {
            TileData = tileData,
            Cliloc = cliloc,
            Hues = hues,
            LoadedCount = CountLoaded(tileData, cliloc, hues),
            UsedCacheCount = CountCacheHits(tileData, cliloc, hues),
            TotalMs = (DateTime.UtcNow - started).TotalMilliseconds
        };
    }

    private static int CountLoaded(BrowserTileDataReadResult tileData, BrowserClilocReadResult cliloc, BrowserHuesReadResult hues)
    {
        int count = 0;

        if (tileData.Exists)
        {
            count++;
        }

        if (cliloc.Exists)
        {
            count++;
        }

        if (hues.Exists)
        {
            count++;
        }

        return count;
    }

    private static int CountCacheHits(BrowserTileDataReadResult tileData, BrowserClilocReadResult cliloc, BrowserHuesReadResult hues)
    {
        int count = 0;

        if (cliloc.UsedProcessedCache)
        {
            count++;
        }

        if (hues.UsedParsedCache)
        {
            count++;
        }

        if (tileData.Length > 0)
        {
            // Tiledata currently runs through the parsed harness without an explicit surfaced cache flag.
            // Keep the aggregate conservative until tiledata exposes the same signal.
        }

        return count;
    }
}
