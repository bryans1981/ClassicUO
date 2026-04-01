using ClassicUO.Utility;

namespace BrowserHost.Services;

public sealed class BrowserClientBootstrapHandoff
{
    public bool IsReady { get; set; }
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public double TotalMs { get; set; }
    public BrowserClientBootstrapAssetDescriptor[] Assets { get; set; } = Array.Empty<BrowserClientBootstrapAssetDescriptor>();
    public string Summary { get; set; } = string.Empty;
}

public sealed class BrowserClientBootstrapAssetDescriptor
{
    public string Id { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public string Summary { get; set; } = string.Empty;
}
