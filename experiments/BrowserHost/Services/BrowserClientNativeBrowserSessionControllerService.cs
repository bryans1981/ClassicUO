using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserSessionController
{
    ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserSessionControllerService : IBrowserClientNativeBrowserSessionController
{
    private readonly IBrowserClientNativeBrowserRuntimeExecutionController _nativeBrowserRuntimeExecutionController;
    private readonly IBrowserClientRuntimeBrowserSessionStabilityReadyState _browserSessionStabilityReadyState;

    public BrowserClientNativeBrowserSessionControllerService(
        IBrowserClientNativeBrowserRuntimeExecutionController nativeBrowserRuntimeExecutionController,
        IBrowserClientRuntimeBrowserSessionStabilityReadyState browserSessionStabilityReadyState
    )
    {
        _nativeBrowserRuntimeExecutionController = nativeBrowserRuntimeExecutionController;
        _browserSessionStabilityReadyState = browserSessionStabilityReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeExecutionControllerResult nativeBrowserRuntimeExecutionController = await _nativeBrowserRuntimeExecutionController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult browserSessionStabilityReadyState = await _browserSessionStabilityReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserSessionControllerResult result = new()
        {
            ProfileId = nativeBrowserRuntimeExecutionController.ProfileId,
            AssetRootPath = nativeBrowserRuntimeExecutionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntimeExecutionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntimeExecutionController.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntimeExecutionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntimeExecutionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntimeExecutionController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserRuntimeExecutionController.ReadyAssetCount,
            CacheHits = nativeBrowserRuntimeExecutionController.CacheHits,
            NativeBrowserRuntimeExecutionReady = nativeBrowserRuntimeExecutionController.IsReady,
            NativeBrowserRuntimeExecutionVersion = nativeBrowserRuntimeExecutionController.NativeBrowserRuntimeExecutionVersion,
            NativeBrowserRuntimeExecutionSummary = nativeBrowserRuntimeExecutionController.Summary,
            BrowserSessionStabilityReady = browserSessionStabilityReadyState.IsReady,
            BrowserSessionStabilityReadyStateVersion = browserSessionStabilityReadyState.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = browserSessionStabilityReadyState.Summary,
            TransportMode = nativeBrowserRuntimeExecutionController.TransportMode,
            TransportEndpoint = nativeBrowserRuntimeExecutionController.TransportEndpoint,
            RuntimeExecutionMode = nativeBrowserRuntimeExecutionController.RuntimeExecutionMode,
            LaunchMode = nativeBrowserRuntimeExecutionController.LaunchMode,
            ContractVersion = nativeBrowserRuntimeExecutionController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserRuntimeExecutionController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserRuntimeExecutionController.IsReady ? "native-browser-runtime-execution-ready" : "native-browser-runtime-execution-blocked",
            browserSessionStabilityReadyState.IsReady ? "browser-session-stability-ready" : "browser-session-stability-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserSessionVersion = "browser-native-browser-session-controller-v1";
        result.NativeBrowserSessionStages =
        [
            "bind-native-browser-runtime-execution-controller",
            "bind-browser-session-stability-ready-state",
            "publish-browser-native-browser-session"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser session controller ready for profile '{result.ProfileId}' with {result.NativeBrowserSessionStages.Length} stage(s)."
            : $"Browser-native browser session controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserSessionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserSessionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserSessionStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserRuntimeExecutionReady { get; set; }
    public string NativeBrowserRuntimeExecutionVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeExecutionSummary { get; set; } = string.Empty;
    public bool BrowserSessionStabilityReady { get; set; }
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySummary { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string RuntimeExecutionMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
