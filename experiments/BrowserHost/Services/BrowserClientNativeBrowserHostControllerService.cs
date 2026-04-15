using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserHostController
{
    ValueTask<BrowserClientNativeBrowserHostControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserHostControllerService : IBrowserClientNativeBrowserHostController
{
    private readonly IBrowserClientNativeBrowserRuntime _nativeBrowserRuntime;
    private readonly IBrowserClientRuntimeBrowserRenderReadyState _browserRenderReadyState;
    private readonly IBrowserClientRuntimeBrowserInputReadyState _browserInputReadyState;
    private readonly IBrowserClientRuntimeLaunchController _runtimeLaunchController;

    public BrowserClientNativeBrowserHostControllerService(
        IBrowserClientNativeBrowserRuntime nativeBrowserRuntime,
        IBrowserClientRuntimeBrowserRenderReadyState browserRenderReadyState,
        IBrowserClientRuntimeBrowserInputReadyState browserInputReadyState,
        IBrowserClientRuntimeLaunchController runtimeLaunchController
    )
    {
        _nativeBrowserRuntime = nativeBrowserRuntime;
        _browserRenderReadyState = browserRenderReadyState;
        _browserInputReadyState = browserInputReadyState;
        _runtimeLaunchController = runtimeLaunchController;
    }

    public async ValueTask<BrowserClientNativeBrowserHostControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeResult nativeBrowserRuntime = await _nativeBrowserRuntime.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserRenderReadyStateResult browserRenderReadyState = await _browserRenderReadyState.BuildAsync(profileId);
        BrowserClientRuntimeBrowserInputReadyStateResult browserInputReadyState = await _browserInputReadyState.BuildAsync(profileId);
        BrowserClientRuntimeLaunchControllerResult runtimeLaunchController = await _runtimeLaunchController.ControlAsync(profileId);

        BrowserClientNativeBrowserHostControllerResult result = new()
        {
            ProfileId = nativeBrowserRuntime.ProfileId,
            AssetRootPath = nativeBrowserRuntime.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntime.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntime.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntime.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntime.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntime.StartupProfilePath,
            ReadyAssetCount = nativeBrowserRuntime.ReadyAssetCount,
            CacheHits = nativeBrowserRuntime.CacheHits,
            NativeBrowserRuntimeReady = nativeBrowserRuntime.IsReady,
            NativeBrowserRuntimeVersion = nativeBrowserRuntime.NativeBrowserRuntimeVersion,
            NativeBrowserRuntimeSummary = nativeBrowserRuntime.Summary,
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

        result.RequiredAssets = nativeBrowserRuntime.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserRuntime.IsReady ? "native-browser-runtime-ready" : "native-browser-runtime-blocked",
            browserRenderReadyState.IsReady ? "browser-render-ready" : "browser-render-blocked",
            browserInputReadyState.IsReady ? "browser-input-ready" : "browser-input-blocked",
            runtimeLaunchController.IsReady ? "runtime-launch-ready" : "runtime-launch-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserHostVersion = "browser-native-browser-host-v1";
        result.NativeBrowserHostStages =
        [
            "bind-native-browser-runtime",
            "bind-browser-render-ready-state",
            "bind-browser-input-ready-state",
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
