namespace BrowserHost.Services;

public sealed class BrowserHuesReaderService
{
    private readonly BrowserAssetLoaderHarnessService _loaderHarness;

    public BrowserHuesReaderService(BrowserAssetLoaderHarnessService loaderHarness)
    {
        _loaderHarness = loaderHarness;
    }

    public async ValueTask<BrowserHuesReadResult> ReadAsync(string path)
    {
        BrowserHuesReadResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserAssetLoadResult<BrowserHuesReadResult> loadResult = await _loaderHarness.LoadParsedAsync(
                path,
                static bytes => bytes,
                static bytes => BrowserHuesParser.ParseFirstGroup(string.Empty, bytes),
                $"hues-raw:{path}",
                $"hues-first-entry:{path}"
            );

            result.Exists = loadResult.Exists;
            result.Length = loadResult.Length;
            result.UsedParsedCache = loadResult.UsedParsedCache;

            if (!loadResult.Exists || !loadResult.HasValue)
            {
                result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
                return result;
            }

            BrowserHuesReadResult parsed = loadResult.Value;
            result.FirstGroupHeader = parsed.FirstGroupHeader;
            result.FirstPaletteColor16 = parsed.FirstPaletteColor16;
            result.FirstHueName = parsed.FirstHueName;
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
