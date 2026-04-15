using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserHostController
{
    ValueTask<BrowserClientNativeBrowserHostControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserHostControllerService : IBrowserClientNativeBrowserHostController
{
    private readonly IBrowserClientRuntimeBrowserSurfaceReadyState _browserSurfaceReadyState;
    private readonly IBrowserClientRuntimeLaunchController _runtimeLaunchController;

    public BrowserClientNativeBrowserHostControllerService(
        IBrowserClientRuntimeBrowserSurfaceReadyState browserSurfaceReadyState,
        IBrowserClientRuntimeLaunchController runtimeLaunchController
    )
    {
        _browserSurfaceReadyState = browserSurfaceReadyState;
        _runtimeLaunchController = runtimeLaunchController;
    }

    public async ValueTask<BrowserClientNativeBrowserHostControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSurfaceReadyStateResult browserSurfaceReadyState = await _browserSurfaceReadyState.BuildAsync(profileId);
        BrowserClientRuntimeLaunchControllerResult runtimeLaunchController = await _runtimeLaunchController.ControlAsync(profileId);

        BrowserClientNativeBrowserHostControllerResult result = new()
        {
            ProfileId = browserSurfaceReadyState.ProfileId,
            AssetRootPath = browserSurfaceReadyState.AssetRootPath,
            ProfilesRootPath = browserSurfaceReadyState.ProfilesRootPath,
            CacheRootPath = browserSurfaceReadyState.CacheRootPath,
            ConfigRootPath = browserSurfaceReadyState.ConfigRootPath,
            SettingsFilePath = browserSurfaceReadyState.SettingsFilePath,
            StartupProfilePath = browserSurfaceReadyState.StartupProfilePath,
            ReadyAssetCount = browserSurfaceReadyState.ReadyAssetCount,
            CacheHits = 0,
            NativeBrowserRuntimeReady = browserSurfaceReadyState.IsReady,
            NativeBrowserRuntimeVersion = browserSurfaceReadyState.BrowserSurfaceReadyStateVersion,
            NativeBrowserRuntimeSummary = browserSurfaceReadyState.BrowserSurfaceReadySummary,
            BrowserRenderReady = browserSurfaceReadyState.IsReady,
            BrowserRenderReadyVersion = browserSurfaceReadyState.BrowserSurfaceReadyStateVersion,
            BrowserRenderSummary = browserSurfaceReadyState.BrowserSurfaceReadySummary,
            BrowserInputReady = browserSurfaceReadyState.IsReady,
            BrowserInputReadyVersion = browserSurfaceReadyState.BrowserSurfaceReadyStateVersion,
            BrowserInputSummary = browserSurfaceReadyState.BrowserSurfaceReadySummary,
            LaunchReady = runtimeLaunchController.IsReady,
            LaunchControllerVersion = runtimeLaunchController.LaunchControllerVersion,
            LaunchSummary = runtimeLaunchController.Summary
        };

        result.RequiredAssets = browserSurfaceReadyState.RequiredAssets;
        result.ReadinessChecks =
        [
            browserSurfaceReadyState.IsReady ? "browser-surface-ready" : "browser-surface-blocked",
            runtimeLaunchController.IsReady ? "runtime-launch-ready" : "runtime-launch-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserHostVersion = "browser-native-browser-host-v4";
        result.NativeBrowserHostStages =
        [
            "bind-browser-surface-ready-state",
            "bind-runtime-launch-controller",
            "publish-browser-native-browser-host"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser host ready for profile '{result.ProfileId}' with {result.NativeBrowserHostStages.Length} stage(s)."
            : $"Browser-native browser host blocked for profile '{result.ProfileId}' with {result.NativeBrowserHostStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserHostControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserHostVersion { get; set; } = string.Empty;
    public string[] NativeBrowserHostStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserRuntimeReady { get; set; }
    public string NativeBrowserRuntimeVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeSummary { get; set; } = string.Empty;
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
