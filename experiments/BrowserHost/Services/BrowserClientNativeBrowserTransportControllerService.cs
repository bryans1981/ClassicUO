using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserTransportController
{
    ValueTask<BrowserClientNativeBrowserTransportControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserTransportControllerService : IBrowserClientNativeBrowserTransportController
{
    private readonly IBrowserClientNativeBrowserNetworkController _nativeBrowserNetworkController;

    public BrowserClientNativeBrowserTransportControllerService(
        IBrowserClientNativeBrowserNetworkController nativeBrowserNetworkController
    )
    {
        _nativeBrowserNetworkController = nativeBrowserNetworkController;
    }

    public async ValueTask<BrowserClientNativeBrowserTransportControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserNetworkControllerResult nativeBrowserNetworkController = await _nativeBrowserNetworkController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserTransportControllerResult result = new()
        {
            ProfileId = nativeBrowserNetworkController.ProfileId,
            AssetRootPath = nativeBrowserNetworkController.AssetRootPath,
            ProfilesRootPath = nativeBrowserNetworkController.ProfilesRootPath,
            CacheRootPath = nativeBrowserNetworkController.CacheRootPath,
            ConfigRootPath = nativeBrowserNetworkController.ConfigRootPath,
            SettingsFilePath = nativeBrowserNetworkController.SettingsFilePath,
            StartupProfilePath = nativeBrowserNetworkController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserNetworkController.ReadyAssetCount,
            CacheHits = nativeBrowserNetworkController.CacheHits,
            NativeBrowserNetworkReady = nativeBrowserNetworkController.IsReady,
            NativeBrowserNetworkVersion = nativeBrowserNetworkController.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserNetworkController.Summary,
            TransportMode = nativeBrowserNetworkController.TransportMode,
            LaunchMode = nativeBrowserNetworkController.LaunchMode,
            ContractVersion = nativeBrowserNetworkController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserNetworkController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserNetworkController.IsReady ? "native-browser-network-ready" : "native-browser-network-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserTransportVersion = "browser-native-browser-transport-controller-v1";
        result.NativeBrowserTransportStages =
        [
            "bind-native-browser-network-controller",
            "publish-browser-native-browser-transport"
        ];
        result.TransportEndpoint = result.IsReady ? "ws://localhost:5100/browser-runtime" : string.Empty;
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser transport controller ready for profile '{result.ProfileId}' with {result.NativeBrowserTransportStages.Length} stage(s)."
            : $"Browser-native browser transport controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserTransportStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserTransportControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserTransportVersion { get; set; } = string.Empty;
    public string[] NativeBrowserTransportStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserNetworkReady { get; set; }
    public string NativeBrowserNetworkVersion { get; set; } = string.Empty;
    public string NativeBrowserNetworkSummary { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
