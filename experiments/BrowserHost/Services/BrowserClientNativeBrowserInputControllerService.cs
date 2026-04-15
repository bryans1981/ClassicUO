using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserInputController
{
    ValueTask<BrowserClientNativeBrowserInputControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserInputControllerService : IBrowserClientNativeBrowserInputController
{
    private readonly IBrowserClientNativeBrowserRenderController _nativeBrowserRenderController;
    private readonly IBrowserClientRuntimeBrowserInputReadyState _browserInputReadyState;

    public BrowserClientNativeBrowserInputControllerService(
        IBrowserClientNativeBrowserRenderController nativeBrowserRenderController,
        IBrowserClientRuntimeBrowserInputReadyState browserInputReadyState
    )
    {
        _nativeBrowserRenderController = nativeBrowserRenderController;
        _browserInputReadyState = browserInputReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserInputControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRenderControllerResult nativeBrowserRenderController = await _nativeBrowserRenderController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserInputReadyStateResult browserInputReadyState = await _browserInputReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserInputControllerResult result = new()
        {
            ProfileId = nativeBrowserRenderController.ProfileId,
            AssetRootPath = nativeBrowserRenderController.AssetRootPath,
            ProfilesRootPath = nativeBrowserRenderController.ProfilesRootPath,
            CacheRootPath = nativeBrowserRenderController.CacheRootPath,
            ConfigRootPath = nativeBrowserRenderController.ConfigRootPath,
            SettingsFilePath = nativeBrowserRenderController.SettingsFilePath,
            StartupProfilePath = nativeBrowserRenderController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserRenderController.ReadyAssetCount,
            CacheHits = nativeBrowserRenderController.CacheHits,
            NativeBrowserRenderReady = nativeBrowserRenderController.IsReady,
            NativeBrowserRenderVersion = nativeBrowserRenderController.NativeBrowserRenderVersion,
            NativeBrowserRenderSummary = nativeBrowserRenderController.Summary,
            BrowserInputReady = browserInputReadyState.IsReady,
            BrowserInputReadyStateVersion = browserInputReadyState.BrowserInputReadyStateVersion,
            BrowserInputSummary = browserInputReadyState.Summary
        };

        result.RequiredAssets = nativeBrowserRenderController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserRenderController.IsReady ? "native-browser-render-ready" : "native-browser-render-blocked",
            browserInputReadyState.IsReady ? "browser-input-ready" : "browser-input-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserInputVersion = "browser-native-browser-input-controller-v1";
        result.NativeBrowserInputStages =
        [
            "bind-native-browser-render-controller",
            "bind-browser-input-ready-state",
            "publish-browser-native-browser-input"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser input controller ready for profile '{result.ProfileId}' with {result.NativeBrowserInputStages.Length} stage(s)."
            : $"Browser-native browser input controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserInputStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserInputControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserInputVersion { get; set; } = string.Empty;
    public string[] NativeBrowserInputStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserRenderReady { get; set; }
    public string NativeBrowserRenderVersion { get; set; } = string.Empty;
    public string NativeBrowserRenderSummary { get; set; } = string.Empty;
    public bool BrowserInputReady { get; set; }
    public string BrowserInputReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInputSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
