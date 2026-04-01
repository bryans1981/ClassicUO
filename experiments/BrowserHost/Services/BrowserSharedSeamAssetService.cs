namespace BrowserHost.Services;

public sealed class BrowserSharedSeamAssetService
{
    private readonly BrowserFileSystemBridgeService _bridgeService;
    private readonly BrowserProcessedAssetCacheService _processedAssetCache;

    public BrowserSharedSeamAssetService(
        BrowserFileSystemBridgeService bridgeService,
        BrowserProcessedAssetCacheService processedAssetCache
    )
    {
        _bridgeService = bridgeService;
        _processedAssetCache = processedAssetCache;
    }

    public async ValueTask<BrowserSharedSeamTileDataResult> ReadFirstTileDataEntryAsync(string path = "/uo/tiledata.mul")
    {
        var result = new BrowserSharedSeamTileDataResult
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserFileSystemBridgeReadResult readResult = await _bridgeService.ReadBytesThroughSharedSeamAsync(path);
            result.Path = readResult.Path;
            result.Exists = readResult.Exists;
            result.LoadedOnDemand = readResult.LoadedOnDemand;
            result.ReadSucceeded = readResult.ReadSucceeded;
            result.Length = readResult.Length;

            if (readResult.Exists && readResult.ReadSucceeded)
            {
                string cacheKey = $"shared-seam:tiledata:{readResult.Path}";
                result.UsedParsedCache = _processedAssetCache.Contains(cacheKey);

                BrowserTileDataReadResult parsed = await _processedAssetCache.GetOrAddAsync(
                    cacheKey,
                    () => ValueTask.FromResult(BrowserTileDataParser.ParseFirstLand(readResult.Path, readResult.Bytes))
                );

                result.IsOldFormat = parsed.IsOldFormat;
                result.Header = parsed.Header;
                result.FirstLandFlags = parsed.FirstLandFlags;
                result.FirstLandTextureId = parsed.FirstLandTextureId;
                result.FirstLandName = parsed.FirstLandName;
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

    public async ValueTask<BrowserSharedSeamClilocResult> ReadFirstClilocEntryAsync(string path = "/uo/cliloc.enu")
    {
        var result = new BrowserSharedSeamClilocResult
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserFileSystemBridgeReadResult readResult = await _bridgeService.ReadBytesThroughSharedSeamAsync(path);
            result.Path = readResult.Path;
            result.Exists = readResult.Exists;
            result.LoadedOnDemand = readResult.LoadedOnDemand;
            result.ReadSucceeded = readResult.ReadSucceeded;
            result.Length = readResult.Length;

            if (readResult.Exists && readResult.ReadSucceeded)
            {
                string cacheKey = $"shared-seam:cliloc:{readResult.Path}";
                result.UsedParsedCache = _processedAssetCache.Contains(cacheKey);

                BrowserClilocReadResult parsed = await _processedAssetCache.GetOrAddAsync(
                    cacheKey,
                    () => ValueTask.FromResult(BrowserClilocParser.ParseFirstEntry(readResult.Path, readResult.Bytes))
                );

                result.Header1 = parsed.Header1;
                result.Header2 = parsed.Header2;
                result.FirstEntryNumber = parsed.FirstEntryNumber;
                result.FirstEntryFlag = parsed.FirstEntryFlag;
                result.FirstEntryTextLength = parsed.FirstEntryTextLength;
                result.FirstEntryText = parsed.FirstEntryText;
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

    public async ValueTask<BrowserSharedSeamHuesResult> ReadFirstHuesEntryAsync(string path = "/uo/hues.mul")
    {
        var result = new BrowserSharedSeamHuesResult
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserFileSystemBridgeReadResult readResult = await _bridgeService.ReadBytesThroughSharedSeamAsync(path);
            result.Path = readResult.Path;
            result.Exists = readResult.Exists;
            result.LoadedOnDemand = readResult.LoadedOnDemand;
            result.ReadSucceeded = readResult.ReadSucceeded;
            result.Length = readResult.Length;

            if (readResult.Exists && readResult.ReadSucceeded)
            {
                string cacheKey = $"shared-seam:hues:{readResult.Path}";
                result.UsedParsedCache = _processedAssetCache.Contains(cacheKey);

                BrowserHuesReadResult parsed = await _processedAssetCache.GetOrAddAsync(
                    cacheKey,
                    () => ValueTask.FromResult(BrowserHuesParser.ParseFirstGroup(readResult.Path, readResult.Bytes))
                );

                result.FirstGroupHeader = parsed.FirstGroupHeader;
                result.FirstPaletteColor16 = parsed.FirstPaletteColor16;
                result.FirstHueName = parsed.FirstHueName;
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
}

public sealed class BrowserSharedSeamTileDataResult
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public bool IsOldFormat { get; set; }
    public uint Header { get; set; }
    public string FirstLandFlags { get; set; } = string.Empty;
    public int FirstLandTextureId { get; set; }
    public string FirstLandName { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserSharedSeamClilocResult
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public int Header1 { get; set; }
    public short Header2 { get; set; }
    public int FirstEntryNumber { get; set; }
    public byte FirstEntryFlag { get; set; }
    public short FirstEntryTextLength { get; set; }
    public string FirstEntryText { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserSharedSeamHuesResult
{
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public uint FirstGroupHeader { get; set; }
    public int FirstPaletteColor16 { get; set; }
    public string FirstHueName { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}
