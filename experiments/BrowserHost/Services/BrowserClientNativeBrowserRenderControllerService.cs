using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRenderController
{
    ValueTask<BrowserClientNativeBrowserRenderControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRenderControllerService : IBrowserClientNativeBrowserRenderController
{
    private readonly IBrowserClientNativeBrowserSurfaceController _nativeBrowserSurfaceController;
    private readonly IBrowserClientRuntimeBrowserRenderReadyState _browserRenderReadyState;

    public BrowserClientNativeBrowserRenderControllerService(
        IBrowserClientNativeBrowserSurfaceController nativeBrowserSurfaceController,
        IBrowserClientRuntimeBrowserRenderReadyState browserRenderReadyState
    )
    {
        _nativeBrowserSurfaceController = nativeBrowserSurfaceController;
        _browserRenderReadyState = browserRenderReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserRenderControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserSurfaceControllerResult nativeBrowserSurfaceController = await _nativeBrowserSurfaceController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserRenderReadyStateResult browserRenderReadyState = await _browserRenderReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserRenderControllerResult result = new()
        {
            ProfileId = nativeBrowserSurfaceController.ProfileId,
            AssetRootPath = nativeBrowserSurfaceController.AssetRootPath,
            ProfilesRootPath = nativeBrowserSurfaceController.ProfilesRootPath,
            CacheRootPath = nativeBrowserSurfaceController.CacheRootPath,
            ConfigRootPath = nativeBrowserSurfaceController.ConfigRootPath,
            SettingsFilePath = nativeBrowserSurfaceController.SettingsFilePath,
            StartupProfilePath = nativeBrowserSurfaceController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserSurfaceController.ReadyAssetCount,
            CacheHits = nativeBrowserSurfaceController.CacheHits,
            NativeBrowserSurfaceReady = nativeBrowserSurfaceController.IsReady,
            NativeBrowserSurfaceVersion = nativeBrowserSurfaceController.NativeBrowserSurfaceVersion,
            NativeBrowserSurfaceSummary = nativeBrowserSurfaceController.Summary,
            BrowserRenderReady = browserRenderReadyState.IsReady,
            BrowserRenderReadyStateVersion = browserRenderReadyState.BrowserRenderReadyStateVersion,
            BrowserRenderSummary = browserRenderReadyState.Summary
        };

        result.RequiredAssets = nativeBrowserSurfaceController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserSurfaceController.IsReady ? "native-browser-surface-ready" : "native-browser-surface-blocked",
            browserRenderReadyState.IsReady ? "browser-render-ready" : "browser-render-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRenderVersion = "browser-native-browser-render-controller-v1";
        result.NativeBrowserRenderStages =
        [
            "bind-native-browser-surface-controller",
            "bind-browser-render-ready-state",
            "publish-browser-native-browser-render"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser render controller ready for profile '{result.ProfileId}' with {result.NativeBrowserRenderStages.Length} stage(s)."
            : $"Browser-native browser render controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserRenderStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRenderControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRenderVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRenderStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserSurfaceReady { get; set; }
    public string NativeBrowserSurfaceVersion { get; set; } = string.Empty;
    public string NativeBrowserSurfaceSummary { get; set; } = string.Empty;
    public bool BrowserRenderReady { get; set; }
    public string BrowserRenderReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRenderSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
