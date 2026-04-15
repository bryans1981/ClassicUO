using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserSessionController
{
    ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserSessionControllerService : IBrowserClientNativeBrowserSessionController
{
    private readonly IBrowserClientNativeBrowserWebSocketRuntimeSessionController _nativeBrowserWebSocketRuntimeSessionController;
    private readonly IBrowserClientRuntimeBrowserSessionStabilityReadyState _browserSessionStabilityReadyState;

    public BrowserClientNativeBrowserSessionControllerService(
        IBrowserClientNativeBrowserWebSocketRuntimeSessionController nativeBrowserWebSocketRuntimeSessionController,
        IBrowserClientRuntimeBrowserSessionStabilityReadyState browserSessionStabilityReadyState
    )
    {
        _nativeBrowserWebSocketRuntimeSessionController = nativeBrowserWebSocketRuntimeSessionController;
        _browserSessionStabilityReadyState = browserSessionStabilityReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult nativeBrowserWebSocketRuntimeSessionController = await _nativeBrowserWebSocketRuntimeSessionController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult browserSessionStabilityReadyState = await _browserSessionStabilityReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserSessionControllerResult result = new()
        {
            ProfileId = nativeBrowserWebSocketRuntimeSessionController.ProfileId,
            AssetRootPath = nativeBrowserWebSocketRuntimeSessionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserWebSocketRuntimeSessionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserWebSocketRuntimeSessionController.CacheRootPath,
            ConfigRootPath = nativeBrowserWebSocketRuntimeSessionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserWebSocketRuntimeSessionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserWebSocketRuntimeSessionController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserWebSocketRuntimeSessionController.ReadyAssetCount,
            CacheHits = nativeBrowserWebSocketRuntimeSessionController.CacheHits,
            NativeBrowserRuntimeExecutionReady = nativeBrowserWebSocketRuntimeSessionController.NativeBrowserWebSocketRuntimeExecutionReady,
            NativeBrowserRuntimeExecutionVersion = nativeBrowserWebSocketRuntimeSessionController.NativeBrowserWebSocketRuntimeExecutionVersion,
            NativeBrowserRuntimeExecutionSummary = nativeBrowserWebSocketRuntimeSessionController.NativeBrowserWebSocketRuntimeExecutionSummary,
            NativeBrowserWebSocketSessionReady = nativeBrowserWebSocketRuntimeSessionController.IsReady,
            NativeBrowserWebSocketSessionVersion = nativeBrowserWebSocketRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionVersion,
            NativeBrowserWebSocketSessionSummary = nativeBrowserWebSocketRuntimeSessionController.Summary,
            WebSocketSessionEndpoint = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionEndpoint,
            WebSocketSessionScheme = "ws",
            WebSocketSessionProtocol = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionProtocol,
            BrowserSessionStabilityReady = browserSessionStabilityReadyState.IsReady,
            BrowserSessionStabilityReadyStateVersion = browserSessionStabilityReadyState.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = browserSessionStabilityReadyState.Summary,
            TransportMode = nativeBrowserWebSocketRuntimeSessionController.TransportMode,
            TransportEndpoint = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionEndpoint,
            RuntimeExecutionMode = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionMode,
            LaunchMode = nativeBrowserWebSocketRuntimeSessionController.LaunchMode,
            ContractVersion = nativeBrowserWebSocketRuntimeSessionController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserWebSocketRuntimeSessionController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserWebSocketRuntimeSessionController.IsReady ? "native-browser-websocket-runtime-session-ready" : "native-browser-websocket-runtime-session-blocked",
            browserSessionStabilityReadyState.IsReady ? "browser-session-stability-ready" : "browser-session-stability-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserSessionVersion = "browser-native-browser-session-controller-v4";
        result.NativeBrowserSessionStages =
        [
            "bind-native-browser-websocket-runtime-session-controller",
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
    public bool NativeBrowserWebSocketSessionReady { get; set; }
    public string NativeBrowserWebSocketSessionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketSessionSummary { get; set; } = string.Empty;
    public string WebSocketSessionEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
    public string WebSocketSessionProtocol { get; set; } = string.Empty;
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
