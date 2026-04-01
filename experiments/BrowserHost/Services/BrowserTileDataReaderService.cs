namespace BrowserHost.Services;

public sealed class BrowserTileDataReaderService
{
    private readonly BrowserAssetLoaderHarnessService _loaderHarness;

    public BrowserTileDataReaderService(BrowserAssetLoaderHarnessService loaderHarness)
    {
        _loaderHarness = loaderHarness;
    }

    public async ValueTask<BrowserTileDataReadResult> ReadAsync(string path)
    {
        BrowserTileDataReadResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserAssetLoadResult<BrowserTileDataReadResult> loadResult = await _loaderHarness.LoadParsedAsync(
                path,
                static bytes => bytes,
                static bytes => BrowserTileDataParser.ParseFirstLand(string.Empty, bytes),
                $"tiledata-raw:{path}",
                $"tiledata-first-land:{path}"
            );

            result.Exists = loadResult.Exists;
            result.Length = loadResult.Length;

            if (!loadResult.Exists || !loadResult.HasValue)
            {
                result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
                return result;
            }

            BrowserTileDataReadResult parsed = loadResult.Value;

            result.IsOldFormat = parsed.IsOldFormat;
            result.Header = parsed.Header;
            result.FirstLandFlags = parsed.FirstLandFlags;
            result.FirstLandTextureId = parsed.FirstLandTextureId;
            result.FirstLandName = parsed.FirstLandName;
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

}
