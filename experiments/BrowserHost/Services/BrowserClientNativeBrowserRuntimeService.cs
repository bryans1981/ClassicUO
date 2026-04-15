using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntime
{
    ValueTask<BrowserClientNativeBrowserRuntimeResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeService : IBrowserClientNativeBrowserRuntime
{
    private readonly IBrowserClientNativeClientBootstrapController _nativeClientBootstrapController;
    private readonly IBrowserClientRuntimeBrowserRenderReadyState _browserRenderReadyState;
    private readonly IBrowserClientRuntimeBrowserInputReadyState _browserInputReadyState;
    private readonly IBrowserClientRuntimeLaunchController _runtimeLaunchController;

    public BrowserClientNativeBrowserRuntimeService(
        IBrowserClientNativeClientBootstrapController nativeClientBootstrapController,
        IBrowserClientRuntimeBrowserRenderReadyState browserRenderReadyState,
        IBrowserClientRuntimeBrowserInputReadyState browserInputReadyState,
        IBrowserClientRuntimeLaunchController runtimeLaunchController
    )
    {
        _nativeClientBootstrapController = nativeClientBootstrapController;
        _browserRenderReadyState = browserRenderReadyState;
        _browserInputReadyState = browserInputReadyState;
        _runtimeLaunchController = runtimeLaunchController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeClientBootstrapControllerResult nativeClientBootstrapController = await _nativeClientBootstrapController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserRenderReadyStateResult browserRenderReadyState = await _browserRenderReadyState.BuildAsync(profileId);
        BrowserClientRuntimeBrowserInputReadyStateResult browserInputReadyState = await _browserInputReadyState.BuildAsync(profileId);
        BrowserClientRuntimeLaunchControllerResult runtimeLaunchController = await _runtimeLaunchController.ControlAsync(profileId);

        BrowserClientNativeBrowserRuntimeResult result = new()
        {
            ProfileId = nativeClientBootstrapController.ProfileId,
            AssetRootPath = nativeClientBootstrapController.AssetRootPath,
            ProfilesRootPath = nativeClientBootstrapController.ProfilesRootPath,
            CacheRootPath = nativeClientBootstrapController.CacheRootPath,
            ConfigRootPath = nativeClientBootstrapController.ConfigRootPath,
            SettingsFilePath = nativeClientBootstrapController.SettingsFilePath,
            StartupProfilePath = nativeClientBootstrapController.StartupProfilePath,
            ReadyAssetCount = nativeClientBootstrapController.ReadyAssetCount,
            CacheHits = nativeClientBootstrapController.CacheHits,
            NativeClientBootstrapReady = nativeClientBootstrapController.IsReady,
            NativeClientBootstrapVersion = nativeClientBootstrapController.NativeClientBootstrapVersion,
            NativeClientBootstrapSummary = nativeClientBootstrapController.Summary,
            BrowserRenderReady = browserRenderReadyState.IsReady,
            BrowserRenderReadyVersion = browserRenderReadyState.BrowserRenderReadyStateVersion,
            BrowserRenderSummary = browserRenderReadyState.Summary,
            BrowserInputReady = browserInputReadyState.IsReady,
            BrowserInputReadyVersion = browserInputReadyState.BrowserInputReadyStateVersion,
            BrowserInputSummary = browserInputReadyState.Summary,
            LaunchReady = runtimeLaunchController.IsReady,
            LaunchControllerVersion = runtimeLaunchController.LaunchControllerVersion,
            LaunchSummary = runtimeLaunchController.Summary
        };

        result.RequiredAssets = nativeClientBootstrapController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeClientBootstrapController.IsReady ? "native-client-bootstrap-ready" : "native-client-bootstrap-blocked",
            browserRenderReadyState.IsReady ? "browser-render-ready" : "browser-render-blocked",
            browserInputReadyState.IsReady ? "browser-input-ready" : "browser-input-blocked",
            runtimeLaunchController.IsReady ? "runtime-launch-ready" : "runtime-launch-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeVersion = "browser-native-browser-runtime-v1";
        result.NativeBrowserRuntimeStages =
        [
            "bind-native-client-bootstrap",
            "bind-browser-render-ready-state",
            "bind-browser-input-ready-state",
            "bind-runtime-launch-controller",
            "publish-browser-native-browser-runtime"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeStages.Length} stage(s)."
            : $"Browser-native browser runtime blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeClientBootstrapReady { get; set; }
    public string NativeClientBootstrapVersion { get; set; } = string.Empty;
    public string NativeClientBootstrapSummary { get; set; } = string.Empty;
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
