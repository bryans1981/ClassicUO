using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeSessionController
{
    ValueTask<BrowserClientNativeBrowserRuntimeSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeSessionControllerService : IBrowserClientNativeBrowserRuntimeSessionController
{
    private readonly IBrowserClientNativeBrowserWebSocketRuntimeSessionController _nativeBrowserWebSocketRuntimeSessionController;
    private readonly IBrowserClientRuntimeBrowserSessionStabilityReadyState _browserSessionStabilityReadyState;

    public BrowserClientNativeBrowserRuntimeSessionControllerService(
        IBrowserClientNativeBrowserWebSocketRuntimeSessionController nativeBrowserWebSocketRuntimeSessionController,
        IBrowserClientRuntimeBrowserSessionStabilityReadyState browserSessionStabilityReadyState
    )
    {
        _nativeBrowserWebSocketRuntimeSessionController = nativeBrowserWebSocketRuntimeSessionController;
        _browserSessionStabilityReadyState = browserSessionStabilityReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult nativeBrowserWebSocketRuntimeSessionController = await _nativeBrowserWebSocketRuntimeSessionController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult browserSessionStabilityReadyState = await _browserSessionStabilityReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserRuntimeSessionControllerResult result = new()
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
            NativeBrowserWebSocketRuntimeSessionReady = nativeBrowserWebSocketRuntimeSessionController.IsReady,
            NativeBrowserWebSocketRuntimeSessionVersion = nativeBrowserWebSocketRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionVersion,
            NativeBrowserWebSocketRuntimeSessionSummary = nativeBrowserWebSocketRuntimeSessionController.Summary,
            WebSocketRuntimeSessionId = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeSessionId,
            WebSocketRuntimeSessionState = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeSessionState,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserWebSocketRuntimeSessionController.WebSocketRuntimeExecutionMode,
            WebSocketSessionScheme = "ws",
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
        result.NativeBrowserRuntimeSessionVersion = "browser-native-browser-runtime-session-controller-v1";
        result.NativeBrowserRuntimeSessionStages =
        [
            "bind-native-browser-websocket-runtime-session-controller",
            "bind-browser-session-stability-ready-state",
            "publish-browser-native-browser-runtime-session"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime session controller ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionStages.Length} stage(s)."
            : $"Browser-native browser runtime session controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeSessionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeSessionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeSessionStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserWebSocketRuntimeSessionReady { get; set; }
    public string NativeBrowserWebSocketRuntimeSessionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketRuntimeSessionSummary { get; set; } = string.Empty;
    public string WebSocketRuntimeSessionId { get; set; } = string.Empty;
    public string WebSocketRuntimeSessionState { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionEndpoint { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionProtocol { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionMode { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
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
