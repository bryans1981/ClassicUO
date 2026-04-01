namespace BrowserHost.Services;

public sealed class BrowserClilocReaderService
{
    private readonly BrowserAssetLoaderHarnessService _loaderHarness;

    public BrowserClilocReaderService(BrowserAssetLoaderHarnessService loaderHarness)
    {
        _loaderHarness = loaderHarness;
    }

    public async ValueTask<BrowserClilocReadResult> ReadAsync(string path)
    {
        BrowserClilocReadResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserAssetLoadResult<BrowserClilocReadResult> loadResult = await _loaderHarness.LoadParsedAsync(
                path,
                static bytes => bytes,
                static bytes => BrowserClilocParser.ParseFirstEntry(string.Empty, bytes),
                $"cliloc-decompressed:{path}",
                $"cliloc-first-entry:{path}"
            );

            result.Exists = loadResult.Exists;
            result.Length = loadResult.Length;
            result.UsedProcessedCache = loadResult.UsedProcessedCache;

            if (!loadResult.Exists || !loadResult.HasValue)
            {
                result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
                return result;
            }

            BrowserClilocReadResult parsed = loadResult.Value;
            result.Header1 = parsed.Header1;
            result.Header2 = parsed.Header2;
            result.FirstEntryNumber = parsed.FirstEntryNumber;
            result.FirstEntryFlag = parsed.FirstEntryFlag;
            result.FirstEntryTextLength = parsed.FirstEntryTextLength;
            result.FirstEntryText = parsed.FirstEntryText;
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
