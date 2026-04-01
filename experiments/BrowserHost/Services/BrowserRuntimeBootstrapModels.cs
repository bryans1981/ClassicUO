namespace BrowserHost.Services;

public sealed class BrowserRuntimeBootstrapRequest
{
    public string TileDataPath { get; set; } = "/uo/tiledata.mul";
    public string ClilocPath { get; set; } = "/uo/cliloc.enu";
    public string HuesPath { get; set; } = "/uo/hues.mul";
}

public sealed class BrowserRuntimeBootstrapState
{
    public string TileDataPath { get; set; } = string.Empty;
    public string ClilocPath { get; set; } = string.Empty;
    public string HuesPath { get; set; } = string.Empty;
    public bool IsReady { get; set; }
    public int LoadedCount { get; set; }
    public int CacheHits { get; set; }
    public double TotalMs { get; set; }
    public BrowserRuntimeTileDataSummary TileData { get; set; } = new();
    public BrowserRuntimeClilocSummary Cliloc { get; set; } = new();
    public BrowserRuntimeHuesSummary Hues { get; set; } = new();
}

public sealed class BrowserSharedSeamRuntimeBootstrapState
{
    public string TileDataPath { get; set; } = string.Empty;
    public string ClilocPath { get; set; } = string.Empty;
    public string HuesPath { get; set; } = string.Empty;
    public bool IsReady { get; set; }
    public int LoadedCount { get; set; }
    public int CacheHits { get; set; }
    public double TotalMs { get; set; }
    public BrowserSharedSeamRuntimeTileDataSummary TileData { get; set; } = new();
    public BrowserSharedSeamRuntimeClilocSummary Cliloc { get; set; } = new();
    public BrowserSharedSeamRuntimeHuesSummary Hues { get; set; } = new();
}

public sealed class BrowserRuntimeTileDataSummary
{
    public bool Exists { get; set; }
    public int Length { get; set; }
    public string FirstLandName { get; set; } = string.Empty;
    public int FirstLandTextureId { get; set; }
}

public sealed class BrowserRuntimeClilocSummary
{
    public bool Exists { get; set; }
    public int Length { get; set; }
    public int FirstEntryNumber { get; set; }
    public string FirstEntryText { get; set; } = string.Empty;
    public bool UsedProcessedCache { get; set; }
}

public sealed class BrowserRuntimeHuesSummary
{
    public bool Exists { get; set; }
    public int Length { get; set; }
    public uint FirstGroupHeader { get; set; }
    public string FirstHueName { get; set; } = string.Empty;
    public bool UsedParsedCache { get; set; }
}

public sealed class BrowserRuntimeCacheState
{
    public BrowserAssetSourceCacheSummary RawAssetCache { get; set; } = new();
    public BrowserProcessedAssetCacheSummary ProcessedAssetCache { get; set; } = new();
}

public sealed class BrowserSharedSeamRuntimeTileDataSummary
{
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public string FirstLandName { get; set; } = string.Empty;
    public int FirstLandTextureId { get; set; }
}

public sealed class BrowserSharedSeamRuntimeClilocSummary
{
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public int FirstEntryNumber { get; set; }
    public string FirstEntryText { get; set; } = string.Empty;
}

public sealed class BrowserSharedSeamRuntimeHuesSummary
{
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public uint FirstGroupHeader { get; set; }
    public string FirstHueName { get; set; } = string.Empty;
}
