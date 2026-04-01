using Microsoft.JSInterop;

namespace BrowserHost.Services;

public sealed class BrowserStorageService
{
    private readonly IJSRuntime _js;

    public BrowserStorageService(IJSRuntime js)
    {
        _js = js;
    }

    public ValueTask<BrowserStorageSmokeTestResult> RunSmokeTestAsync()
    {
        return _js.InvokeAsync<BrowserStorageSmokeTestResult>("classicuoFs.runSmokeTest");
    }

    public ValueTask<BrowserAssetImportResult> ImportFilesAsync()
    {
        return _js.InvokeAsync<BrowserAssetImportResult>("classicuoFs.importFiles");
    }

    public ValueTask<BrowserAssetImportResult> ImportDirectoryAsync()
    {
        return _js.InvokeAsync<BrowserAssetImportResult>("classicuoFs.importDirectory");
    }

    public ValueTask<BrowserStoredFilesResult> ListFilesAsync()
    {
        return _js.InvokeAsync<BrowserStoredFilesResult>("classicuoFs.listFiles");
    }

    public ValueTask<BrowserStoredFilesResult> ListFilesUnderPathAsync(string path)
    {
        return _js.InvokeAsync<BrowserStoredFilesResult>("classicuoFs.listFilesUnderPath", path);
    }

    public ValueTask<BrowserStorageResetResult> ResetStorageAsync()
    {
        return _js.InvokeAsync<BrowserStorageResetResult>("classicuoFs.resetStorage");
    }

    public ValueTask<bool> FileExistsAsync(string path)
    {
        return _js.InvokeAsync<bool>("classicuoFs.fileExists", path);
    }

    public ValueTask<BrowserFileTextResult> ReadTextAsync(string path)
    {
        return _js.InvokeAsync<BrowserFileTextResult>("classicuoFs.readText", path);
    }

    public ValueTask<BrowserFileWriteResult> WriteTextAsync(string path, string contents)
    {
        return _js.InvokeAsync<BrowserFileWriteResult>("classicuoFs.writeText", path, contents);
    }

    public ValueTask<BrowserFileBytesResult> ReadBytesBase64Async(string path)
    {
        return _js.InvokeAsync<BrowserFileBytesResult>("classicuoFs.readBytesBase64", path);
    }

    public ValueTask<BrowserFileWriteResult> WriteBytesBase64Async(string path, string base64)
    {
        return _js.InvokeAsync<BrowserFileWriteResult>("classicuoFs.writeBytesBase64", path, base64);
    }

    public ValueTask<BrowserReadBenchmarkResult> BenchmarkReadPathAsync(string path, int iterations)
    {
        return _js.InvokeAsync<BrowserReadBenchmarkResult>("classicuoFs.benchmarkReadPath", path, iterations);
    }

    public ValueTask<BrowserPreloadResult> PreloadPathAsync(string path)
    {
        return _js.InvokeAsync<BrowserPreloadResult>("classicuoFs.preloadPath", path);
    }

    public ValueTask<BrowserPreloadCacheSummary> GetPreloadCacheSummaryAsync()
    {
        return _js.InvokeAsync<BrowserPreloadCacheSummary>("classicuoFs.getPreloadCacheSummary");
    }

    public ValueTask<BrowserPreloadResult> ClearPreloadCacheAsync()
    {
        return _js.InvokeAsync<BrowserPreloadResult>("classicuoFs.clearPreloadCache");
    }

    public ValueTask<BrowserAssetManifestResult> GetAssetManifestAsync()
    {
        return _js.InvokeAsync<BrowserAssetManifestResult>("classicuoFs.getAssetManifest");
    }

    public ValueTask<BrowserHostedSeedManifestResult> GetHostedSeedManifestAsync()
    {
        return _js.InvokeAsync<BrowserHostedSeedManifestResult>("classicuoFs.getHostedSeedManifest");
    }

    public ValueTask<BrowserAssetImportResult> ImportHostedSeedAsync()
    {
        return _js.InvokeAsync<BrowserAssetImportResult>("classicuoFs.importHostedSeed");
    }

    public ValueTask<BrowserAssetImportResult> ImportHostedRecommendedSeedAsync()
    {
        return _js.InvokeAsync<BrowserAssetImportResult>("classicuoFs.importHostedRecommendedSeed");
    }

    public ValueTask<BrowserTileDataProbeResult> ProbeTileDataAsync(string path)
    {
        return _js.InvokeAsync<BrowserTileDataProbeResult>("classicuoFs.probeTileData", path);
    }

    public ValueTask DownloadTextFileAsync(string fileName, string contents)
    {
        return _js.InvokeVoidAsync("classicuoFs.downloadTextFile", fileName, contents);
    }
}

public sealed class BrowserStorageSmokeTestResult
{
    public bool HasOpfsApi { get; set; }
    public bool WriteSucceeded { get; set; }
    public bool ReadSucceeded { get; set; }
    public string ReadText { get; set; } = string.Empty;
    public string[] FileNames { get; set; } = Array.Empty<string>();
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserAssetImportResult
{
    public bool HasOpfsApi { get; set; }
    public int ImportedCount { get; set; }
    public string[] ImportedFileNames { get; set; } = Array.Empty<string>();
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserStoredFilesResult
{
    public bool HasOpfsApi { get; set; }
    public string Path { get; set; } = string.Empty;
    public string[] FileNames { get; set; } = Array.Empty<string>();
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserStorageResetResult
{
    public bool HasOpfsApi { get; set; }
    public bool Succeeded { get; set; }
    public int DeletedEntries { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserFileTextResult
{
    public bool HasOpfsApi { get; set; }
    public bool Exists { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserFileBytesResult
{
    public bool HasOpfsApi { get; set; }
    public bool Exists { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Base64 { get; set; } = string.Empty;
    public int Length { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserFileWriteResult
{
    public bool HasOpfsApi { get; set; }
    public bool Succeeded { get; set; }
    public string Path { get; set; } = string.Empty;
    public int Length { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserReadBenchmarkResult
{
    public bool HasOpfsApi { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public int Iterations { get; set; }
    public int Length { get; set; }
    public double TotalMs { get; set; }
    public double AverageMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserPreloadResult
{
    public bool HasOpfsApi { get; set; }
    public bool Succeeded { get; set; }
    public string Path { get; set; } = string.Empty;
    public int Length { get; set; }
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserPreloadCacheSummary
{
    public int EntryCount { get; set; }
    public long TotalBytes { get; set; }
    public string[] Paths { get; set; } = Array.Empty<string>();
}

public sealed class BrowserAssetManifestResult
{
    public bool HasOpfsApi { get; set; }
    public string RootPath { get; set; } = string.Empty;
    public int FileCount { get; set; }
    public long TotalBytes { get; set; }
    public BrowserAssetManifestEntry[] Entries { get; set; } = Array.Empty<BrowserAssetManifestEntry>();
    public string[] Extensions { get; set; } = Array.Empty<string>();
    public BrowserAssetBootstrapReadiness Bootstrap { get; set; } = new();
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserAssetManifestEntry
{
    public string Path { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public bool IsPreloaded { get; set; }
}

public sealed class BrowserHostedSeedManifestResult
{
    public bool Available { get; set; }
    public string RootPath { get; set; } = string.Empty;
    public int FileCount { get; set; }
    public long TotalBytes { get; set; }
    public BrowserHostedSeedManifestEntry[] Entries { get; set; } = Array.Empty<BrowserHostedSeedManifestEntry>();
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserHostedSeedManifestEntry
{
    public string RelativePath { get; set; } = string.Empty;
    public long Size { get; set; }
}

public sealed class BrowserAssetBootstrapReadiness
{
    public bool IsReady { get; set; }
    public int PassedChecks { get; set; }
    public int TotalChecks { get; set; }
    public string PrimaryReadTargetPath { get; set; } = string.Empty;
    public string PrimaryReadTargetReason { get; set; } = string.Empty;
    public string[] RecommendedImportSet { get; set; } = Array.Empty<string>();
    public string[] MissingRecommendedFiles { get; set; } = Array.Empty<string>();
    public BrowserAssetBootstrapCheck[] Checks { get; set; } = Array.Empty<BrowserAssetBootstrapCheck>();
}

public sealed class BrowserAssetBootstrapCheck
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSatisfied { get; set; }
    public string[] AcceptedPatterns { get; set; } = Array.Empty<string>();
    public string[] Matches { get; set; } = Array.Empty<string>();
}

public sealed class BrowserTileDataProbeResult
{
    public bool HasOpfsApi { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool IsOldFormat { get; set; }
    public uint Header { get; set; }
    public string FirstLandFlags { get; set; } = string.Empty;
    public int FirstLandTextureId { get; set; }
    public string FirstLandName { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClilocProbeResult
{
    public bool HasOpfsApi { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool UsedProcessedCache { get; set; }
    public int Header1 { get; set; }
    public short Header2 { get; set; }
    public int FirstEntryNumber { get; set; }
    public byte FirstEntryFlag { get; set; }
    public short FirstEntryTextLength { get; set; }
    public string FirstEntryText { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserHuesProbeResult
{
    public bool HasOpfsApi { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool UsedParsedCache { get; set; }
    public uint FirstGroupHeader { get; set; }
    public int FirstPaletteColor16 { get; set; }
    public string FirstHueName { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserAssetWarmResult
{
    public bool Succeeded { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool WasCached { get; set; }
    public int Length { get; set; }
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserAssetSourceCacheSummary
{
    public int EntryCount { get; set; }
    public long TotalBytes { get; set; }
    public string[] Paths { get; set; } = Array.Empty<string>();
}
