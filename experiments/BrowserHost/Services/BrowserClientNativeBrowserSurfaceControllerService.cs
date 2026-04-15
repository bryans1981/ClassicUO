using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserSurfaceController
{
    ValueTask<BrowserClientNativeBrowserSurfaceControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserSurfaceControllerService : IBrowserClientNativeBrowserSurfaceController
{
    private readonly IBrowserClientNativeBrowserHostController _nativeBrowserHostController;
    private readonly IBrowserClientRuntimeBrowserSurfaceReadyState _browserSurfaceReadyState;

    public BrowserClientNativeBrowserSurfaceControllerService(
        IBrowserClientNativeBrowserHostController nativeBrowserHostController,
        IBrowserClientRuntimeBrowserSurfaceReadyState browserSurfaceReadyState
    )
    {
        _nativeBrowserHostController = nativeBrowserHostController;
        _browserSurfaceReadyState = browserSurfaceReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserSurfaceControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserHostControllerResult nativeBrowserHostController = await _nativeBrowserHostController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserSurfaceReadyStateResult browserSurfaceReadyState = await _browserSurfaceReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserSurfaceControllerResult result = new()
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
            BrowserSurfaceReady = browserSurfaceReadyState.IsReady,
            BrowserSurfaceReadyStateVersion = browserSurfaceReadyState.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSummary = browserSurfaceReadyState.Summary
        };

        result.RequiredAssets = nativeBrowserHostController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserHostController.IsReady ? "native-browser-host-ready" : "native-browser-host-blocked",
            browserSurfaceReadyState.IsReady ? "browser-surface-ready" : "browser-surface-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserSurfaceVersion = "browser-native-browser-surface-controller-v1";
        result.NativeBrowserSurfaceStages =
        [
            "bind-native-browser-host-controller",
            "bind-browser-surface-ready-state",
            "publish-browser-native-browser-surface"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser surface controller ready for profile '{result.ProfileId}' with {result.NativeBrowserSurfaceStages.Length} stage(s)."
            : $"Browser-native browser surface controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserSurfaceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserSurfaceControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserSurfaceVersion { get; set; } = string.Empty;
    public string[] NativeBrowserSurfaceStages { get; set; } = Array.Empty<string>();
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
    public bool BrowserSurfaceReady { get; set; }
    public string BrowserSurfaceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSurfaceSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
