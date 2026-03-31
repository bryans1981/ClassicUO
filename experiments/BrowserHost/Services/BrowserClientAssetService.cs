namespace BrowserHost.Services;

public interface IBrowserClientAssetService
{
    ValueTask<BrowserRuntimeBootstrapState> GetBootstrapStateAsync(BrowserRuntimeBootstrapRequest? request = null);
    ValueTask<BrowserRuntimeCacheState> GetCacheStateAsync();
    ValueTask<BrowserAssetWarmResult> WarmAssetAsync(string path);
    ValueTask ClearCachesAsync();
    ValueTask<BrowserClientTileDataDetails> GetFirstTileDataEntryAsync(string path = "/uo/tiledata.mul");
    ValueTask<BrowserClientClilocDetails> GetFirstClilocEntryAsync(string path = "/uo/cliloc.enu");
    ValueTask<BrowserClientHuesDetails> GetFirstHueGroupAsync(string path = "/uo/hues.mul");
}

public sealed class BrowserClientAssetService : IBrowserClientAssetService
{
    private readonly IBrowserRuntimeBootstrapAssets _runtimeBootstrapAssets;
    private readonly BrowserTileDataReaderService _tileDataReaderService;
    private readonly BrowserClilocReaderService _clilocReaderService;
    private readonly BrowserHuesReaderService _huesReaderService;

    public BrowserClientAssetService(
        IBrowserRuntimeBootstrapAssets runtimeBootstrapAssets,
        BrowserTileDataReaderService tileDataReaderService,
        BrowserClilocReaderService clilocReaderService,
        BrowserHuesReaderService huesReaderService
    )
    {
        _runtimeBootstrapAssets = runtimeBootstrapAssets;
        _tileDataReaderService = tileDataReaderService;
        _clilocReaderService = clilocReaderService;
        _huesReaderService = huesReaderService;
    }

    public ValueTask<BrowserRuntimeBootstrapState> GetBootstrapStateAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        return _runtimeBootstrapAssets.PrepareAsync(request);
    }

    public ValueTask<BrowserRuntimeCacheState> GetCacheStateAsync()
    {
        return _runtimeBootstrapAssets.GetCacheStateAsync();
    }

    public ValueTask<BrowserAssetWarmResult> WarmAssetAsync(string path)
    {
        return _runtimeBootstrapAssets.WarmAssetAsync(path);
    }

    public ValueTask ClearCachesAsync()
    {
        return _runtimeBootstrapAssets.ClearCachesAsync();
    }

    public async ValueTask<BrowserClientTileDataDetails> GetFirstTileDataEntryAsync(string path = "/uo/tiledata.mul")
    {
        BrowserTileDataReadResult result = await _tileDataReaderService.ReadAsync(path);

        return new BrowserClientTileDataDetails
        {
            Exists = result.Exists,
            Length = result.Length,
            IsOldFormat = result.IsOldFormat,
            Header = result.Header,
            FirstLandFlags = result.FirstLandFlags,
            FirstLandName = result.FirstLandName,
            FirstLandTextureId = result.FirstLandTextureId,
            TotalMs = result.TotalMs,
            Error = result.Error
        };
    }

    public async ValueTask<BrowserClientClilocDetails> GetFirstClilocEntryAsync(string path = "/uo/cliloc.enu")
    {
        BrowserClilocReadResult result = await _clilocReaderService.ReadAsync(path);

        return new BrowserClientClilocDetails
        {
            Exists = result.Exists,
            Length = result.Length,
            Header1 = result.Header1,
            Header2 = result.Header2,
            FirstEntryNumber = result.FirstEntryNumber,
            FirstEntryFlag = result.FirstEntryFlag,
            FirstEntryTextLength = result.FirstEntryTextLength,
            FirstEntryText = result.FirstEntryText,
            UsedProcessedCache = result.UsedProcessedCache,
            TotalMs = result.TotalMs,
            Error = result.Error
        };
    }

    public async ValueTask<BrowserClientHuesDetails> GetFirstHueGroupAsync(string path = "/uo/hues.mul")
    {
        BrowserHuesReadResult result = await _huesReaderService.ReadAsync(path);

        return new BrowserClientHuesDetails
        {
            Exists = result.Exists,
            Length = result.Length,
            FirstGroupHeader = result.FirstGroupHeader,
            FirstHueName = result.FirstHueName,
            FirstPaletteColor16 = result.FirstPaletteColor16,
            UsedParsedCache = result.UsedParsedCache,
            TotalMs = result.TotalMs,
            Error = result.Error
        };
    }
}
