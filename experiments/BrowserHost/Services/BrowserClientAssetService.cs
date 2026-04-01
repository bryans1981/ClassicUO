namespace BrowserHost.Services;

public interface IBrowserClientAssetService
{
    ValueTask<BrowserRuntimeBootstrapState> GetBootstrapStateAsync(BrowserRuntimeBootstrapRequest? request = null);
    ValueTask<BrowserSharedSeamRuntimeBootstrapState> GetSharedSeamBootstrapStateAsync(BrowserRuntimeBootstrapRequest? request = null);
    ValueTask<BrowserClientBootstrapHandoff> GetBrowserBootstrapHandoffAsync(BrowserRuntimeBootstrapRequest? request = null);
    ValueTask<BrowserRuntimeCacheState> GetCacheStateAsync();
    ValueTask<BrowserAssetWarmResult> WarmAssetAsync(string path);
    ValueTask ClearCachesAsync();
    ValueTask<BrowserClientTileDataDetails> GetFirstTileDataEntryAsync(string path = "/uo/tiledata.mul");
    ValueTask<BrowserClientClilocDetails> GetFirstClilocEntryAsync(string path = "/uo/cliloc.enu");
    ValueTask<BrowserClientHuesDetails> GetFirstHueGroupAsync(string path = "/uo/hues.mul");
    ValueTask<BrowserClientSharedSeamTileDataDetails> GetSharedSeamFirstTileDataEntryAsync(string path = "/uo/tiledata.mul");
    ValueTask<BrowserClientSharedSeamClilocDetails> GetSharedSeamFirstClilocEntryAsync(string path = "/uo/cliloc.enu");
    ValueTask<BrowserClientSharedSeamHuesDetails> GetSharedSeamFirstHueGroupAsync(string path = "/uo/hues.mul");
}

public sealed class BrowserClientAssetService : IBrowserClientAssetService
{
    private readonly IBrowserRuntimeBootstrapAssets _runtimeBootstrapAssets;
    private readonly IBrowserSharedSeamRuntimeBootstrapAssets _sharedSeamRuntimeBootstrapAssets;
    private readonly BrowserSharedSeamAssetService _sharedSeamAssetService;
    private readonly BrowserTileDataReaderService _tileDataReaderService;
    private readonly BrowserClilocReaderService _clilocReaderService;
    private readonly BrowserHuesReaderService _huesReaderService;

    public BrowserClientAssetService(
        IBrowserRuntimeBootstrapAssets runtimeBootstrapAssets,
        IBrowserSharedSeamRuntimeBootstrapAssets sharedSeamRuntimeBootstrapAssets,
        BrowserSharedSeamAssetService sharedSeamAssetService,
        BrowserTileDataReaderService tileDataReaderService,
        BrowserClilocReaderService clilocReaderService,
        BrowserHuesReaderService huesReaderService
    )
    {
        _runtimeBootstrapAssets = runtimeBootstrapAssets;
        _sharedSeamRuntimeBootstrapAssets = sharedSeamRuntimeBootstrapAssets;
        _sharedSeamAssetService = sharedSeamAssetService;
        _tileDataReaderService = tileDataReaderService;
        _clilocReaderService = clilocReaderService;
        _huesReaderService = huesReaderService;
    }

    public ValueTask<BrowserRuntimeBootstrapState> GetBootstrapStateAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        return _runtimeBootstrapAssets.PrepareAsync(request);
    }

    public ValueTask<BrowserSharedSeamRuntimeBootstrapState> GetSharedSeamBootstrapStateAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        return _sharedSeamRuntimeBootstrapAssets.PrepareAsync(request);
    }

    public async ValueTask<BrowserClientBootstrapHandoff> GetBrowserBootstrapHandoffAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserSharedSeamRuntimeBootstrapState state = await _sharedSeamRuntimeBootstrapAssets.PrepareAsync(request);

        BrowserClientBootstrapAssetDescriptor[] assets =
        [
            new BrowserClientBootstrapAssetDescriptor
            {
                Id = "tiledata",
                Path = state.TileDataPath,
                Exists = state.TileData.Exists,
                ReadSucceeded = state.TileData.ReadSucceeded,
                LoadedOnDemand = state.TileData.LoadedOnDemand,
                UsedParsedCache = state.TileData.UsedParsedCache,
                Length = state.TileData.Length,
                Summary = string.IsNullOrWhiteSpace(state.TileData.FirstLandName)
                    ? $"texture {state.TileData.FirstLandTextureId}"
                    : state.TileData.FirstLandName
            },
            new BrowserClientBootstrapAssetDescriptor
            {
                Id = "cliloc",
                Path = state.ClilocPath,
                Exists = state.Cliloc.Exists,
                ReadSucceeded = state.Cliloc.ReadSucceeded,
                LoadedOnDemand = state.Cliloc.LoadedOnDemand,
                UsedParsedCache = state.Cliloc.UsedParsedCache,
                Length = state.Cliloc.Length,
                Summary = state.Cliloc.FirstEntryNumber == 0
                    ? string.Empty
                    : $"{state.Cliloc.FirstEntryNumber}: {state.Cliloc.FirstEntryText}"
            },
            new BrowserClientBootstrapAssetDescriptor
            {
                Id = "hues",
                Path = state.HuesPath,
                Exists = state.Hues.Exists,
                ReadSucceeded = state.Hues.ReadSucceeded,
                LoadedOnDemand = state.Hues.LoadedOnDemand,
                UsedParsedCache = state.Hues.UsedParsedCache,
                Length = state.Hues.Length,
                Summary = string.IsNullOrWhiteSpace(state.Hues.FirstHueName)
                    ? $"header {state.Hues.FirstGroupHeader}"
                    : state.Hues.FirstHueName
            }
        ];

        return new BrowserClientBootstrapHandoff
        {
            IsReady = state.IsReady,
            ReadyAssetCount = state.LoadedCount,
            CacheHits = state.CacheHits,
            TotalMs = state.TotalMs,
            Assets = assets,
            Summary = state.IsReady
                ? $"Bootstrap handoff ready with {state.LoadedCount} seam-backed asset(s)."
                : $"Bootstrap handoff incomplete with {state.LoadedCount} ready asset(s)."
        };
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

    public async ValueTask<BrowserClientSharedSeamTileDataDetails> GetSharedSeamFirstTileDataEntryAsync(string path = "/uo/tiledata.mul")
    {
        BrowserSharedSeamTileDataResult result = await _sharedSeamAssetService.ReadFirstTileDataEntryAsync(path);

        return new BrowserClientSharedSeamTileDataDetails
        {
            Path = result.Path,
            Exists = result.Exists,
            LoadedOnDemand = result.LoadedOnDemand,
            ReadSucceeded = result.ReadSucceeded,
            UsedParsedCache = result.UsedParsedCache,
            Length = result.Length,
            IsOldFormat = result.IsOldFormat,
            Header = result.Header,
            FirstLandFlags = result.FirstLandFlags,
            FirstLandTextureId = result.FirstLandTextureId,
            FirstLandName = result.FirstLandName,
            TotalMs = result.TotalMs,
            Error = result.Error
        };
    }

    public async ValueTask<BrowserClientSharedSeamClilocDetails> GetSharedSeamFirstClilocEntryAsync(string path = "/uo/cliloc.enu")
    {
        BrowserSharedSeamClilocResult result = await _sharedSeamAssetService.ReadFirstClilocEntryAsync(path);

        return new BrowserClientSharedSeamClilocDetails
        {
            Path = result.Path,
            Exists = result.Exists,
            LoadedOnDemand = result.LoadedOnDemand,
            ReadSucceeded = result.ReadSucceeded,
            UsedParsedCache = result.UsedParsedCache,
            Length = result.Length,
            Header1 = result.Header1,
            Header2 = result.Header2,
            FirstEntryNumber = result.FirstEntryNumber,
            FirstEntryFlag = result.FirstEntryFlag,
            FirstEntryTextLength = result.FirstEntryTextLength,
            FirstEntryText = result.FirstEntryText,
            TotalMs = result.TotalMs,
            Error = result.Error
        };
    }

    public async ValueTask<BrowserClientSharedSeamHuesDetails> GetSharedSeamFirstHueGroupAsync(string path = "/uo/hues.mul")
    {
        BrowserSharedSeamHuesResult result = await _sharedSeamAssetService.ReadFirstHuesEntryAsync(path);

        return new BrowserClientSharedSeamHuesDetails
        {
            Path = result.Path,
            Exists = result.Exists,
            LoadedOnDemand = result.LoadedOnDemand,
            ReadSucceeded = result.ReadSucceeded,
            UsedParsedCache = result.UsedParsedCache,
            Length = result.Length,
            FirstGroupHeader = result.FirstGroupHeader,
            FirstPaletteColor16 = result.FirstPaletteColor16,
            FirstHueName = result.FirstHueName,
            TotalMs = result.TotalMs,
            Error = result.Error
        };
    }
}
