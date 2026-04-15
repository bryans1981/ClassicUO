using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserHostReadyState
{
    ValueTask<BrowserClientNativeBrowserHostReadyStateResult> BuildAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserHostReadyStateService : IBrowserClientNativeBrowserHostReadyState
{
    private readonly IBrowserClientNativeBrowserHostController _nativeBrowserHostController;

    public BrowserClientNativeBrowserHostReadyStateService(IBrowserClientNativeBrowserHostController nativeBrowserHostController)
    {
        _nativeBrowserHostController = nativeBrowserHostController;
    }

    public async ValueTask<BrowserClientNativeBrowserHostReadyStateResult> BuildAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserHostControllerResult nativeBrowserHostController = await _nativeBrowserHostController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserHostReadyStateResult result = new()
        {
            ProfileId = nativeBrowserHostController.ProfileId,
            AssetRootPath = nativeBrowserHostController.AssetRootPath,
            ProfilesRootPath = nativeBrowserHostController.ProfilesRootPath,
            CacheRootPath = nativeBrowserHostController.CacheRootPath,
            ConfigRootPath = nativeBrowserHostController.ConfigRootPath,
            SettingsFilePath = nativeBrowserHostController.SettingsFilePath,
            StartupProfilePath = nativeBrowserHostController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserHostController.ReadyAssetCount,
            CacheHits = nativeBrowserHostController.CacheHits,
            NativeBrowserHostReady = nativeBrowserHostController.IsReady,
            NativeBrowserHostVersion = nativeBrowserHostController.NativeBrowserHostVersion,
            NativeBrowserHostSummary = nativeBrowserHostController.Summary,
            BrowserRenderReady = nativeBrowserHostController.BrowserRenderReady,
            BrowserRenderReadyVersion = nativeBrowserHostController.BrowserRenderReadyVersion,
            BrowserRenderSummary = nativeBrowserHostController.BrowserRenderSummary,
            BrowserInputReady = nativeBrowserHostController.BrowserInputReady,
            BrowserInputReadyVersion = nativeBrowserHostController.BrowserInputReadyVersion,
            BrowserInputSummary = nativeBrowserHostController.BrowserInputSummary,
            LaunchReady = nativeBrowserHostController.LaunchReady,
            LaunchControllerVersion = nativeBrowserHostController.LaunchControllerVersion,
            LaunchSummary = nativeBrowserHostController.LaunchSummary,
            RequiredAssets = nativeBrowserHostController.RequiredAssets
        };

        result.ReadinessChecks =
        [
            nativeBrowserHostController.IsReady ? "native-browser-host-ready" : "native-browser-host-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserHostReadyStateVersion = "browser-native-browser-host-ready-state-v1";
        result.NativeBrowserHostReadyStateStages =
        [
            "bind-native-browser-host-controller",
            "publish-browser-native-browser-host-ready-state"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser host ready state ready for profile '{result.ProfileId}' with {result.NativeBrowserHostReadyStateStages.Length} stage(s)."
            : $"Browser-native browser host ready state blocked for profile '{result.ProfileId}' with {result.NativeBrowserHostReadyStateStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserHostReadyStateResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserHostReadyStateVersion { get; set; } = string.Empty;
    public string[] NativeBrowserHostReadyStateStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserHostReady { get; set; }
    public string NativeBrowserHostVersion { get; set; } = string.Empty;
    public string NativeBrowserHostSummary { get; set; } = string.Empty;
    public bool BrowserRenderReady { get; set; }
    public string BrowserRenderReadyVersion { get; set; } = string.Empty;
    public string BrowserRenderSummary { get; set; } = string.Empty;
    public bool BrowserInputReady { get; set; }
    public string BrowserInputReadyVersion { get; set; } = string.Empty;
    public string BrowserInputSummary { get; set; } = string.Empty;
    public bool LaunchReady { get; set; }
    public string LaunchControllerVersion { get; set; } = string.Empty;
    public string LaunchSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
