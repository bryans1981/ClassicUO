using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientBootstrapAdapter
{
    ValueTask<BrowserClientStartupContext> PrepareStartupContextAsync(BrowserRuntimeBootstrapRequest? request = null);
}

public sealed class BrowserClientBootstrapAdapterService : IBrowserClientBootstrapAdapter
{
    private readonly IBrowserClientAssetService _browserClientAssetService;

    public BrowserClientBootstrapAdapterService(IBrowserClientAssetService browserClientAssetService)
    {
        _browserClientAssetService = browserClientAssetService;
    }

    public async ValueTask<BrowserClientStartupContext> PrepareStartupContextAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserClientBootstrapHandoff handoff = await _browserClientAssetService.GetBrowserBootstrapHandoffAsync(request);

        BrowserClientBootstrapAssetDescriptor? tileData = handoff.Assets.FirstOrDefault(static x => x.Id == "tiledata");
        BrowserClientBootstrapAssetDescriptor? cliloc = handoff.Assets.FirstOrDefault(static x => x.Id == "cliloc");
        BrowserClientBootstrapAssetDescriptor? hues = handoff.Assets.FirstOrDefault(static x => x.Id == "hues");

        BrowserClientStartupAsset[] startupAssets =
        [
            CreateStartupAsset(tileData, isRequired: true),
            CreateStartupAsset(cliloc, isRequired: true),
            CreateStartupAsset(hues, isRequired: true)
        ];

        return new BrowserClientStartupContext
        {
            IsReady = handoff.IsReady,
            AssetRootPath = handoff.AssetRootPath,
            ProfilesRootPath = handoff.ProfilesRootPath,
            CacheRootPath = handoff.CacheRootPath,
            ConfigRootPath = handoff.ConfigRootPath,
            SettingsFilePath = BrowserVirtualPaths.ConfigFile("client.settings.json"),
            StartupProfilePath = BrowserVirtualPaths.ProfileFile("default", "profile.json"),
            ReadyAssetCount = handoff.ReadyAssetCount,
            CacheHits = handoff.CacheHits,
            TotalMs = handoff.TotalMs,
            Assets = startupAssets,
            Summary = BuildSummary(startupAssets)
        };
    }

    private static BrowserClientStartupAsset CreateStartupAsset(BrowserClientBootstrapAssetDescriptor? asset, bool isRequired)
    {
        if (asset is null)
        {
            return new BrowserClientStartupAsset
            {
                Id = string.Empty,
                Path = string.Empty,
                IsRequired = isRequired
            };
        }

        return new BrowserClientStartupAsset
        {
            Id = asset.Id,
            Path = asset.Path,
            Exists = asset.Exists,
            ReadSucceeded = asset.ReadSucceeded,
            LoadedOnDemand = asset.LoadedOnDemand,
            UsedParsedCache = asset.UsedParsedCache,
            Length = asset.Length,
            IsRequired = isRequired,
            Summary = asset.Summary
        };
    }

    private static string BuildSummary(BrowserClientStartupAsset[] startupAssets)
    {
        int requiredReadyCount = startupAssets.Count(static x => x.IsRequired && x.Exists && x.ReadSucceeded);
        return requiredReadyCount == 3
            ? $"Startup context ready with {requiredReadyCount} required asset(s) under {BrowserVirtualPaths.AssetsRoot}."
            : $"Startup context blocked with {requiredReadyCount} required asset(s) ready under {BrowserVirtualPaths.AssetsRoot}.";
    }
}

public sealed class BrowserClientStartupContext
{
    public bool IsReady { get; set; }
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public double TotalMs { get; set; }
    public BrowserClientStartupAsset[] Assets { get; set; } = Array.Empty<BrowserClientStartupAsset>();
    public string Summary { get; set; } = string.Empty;
}

public sealed class BrowserClientStartupAsset
{
    public string Id { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool LoadedOnDemand { get; set; }
    public bool UsedParsedCache { get; set; }
    public long Length { get; set; }
    public bool IsRequired { get; set; }
    public string Summary { get; set; } = string.Empty;
}
