using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserSessionController
{
    ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserSessionControllerService : IBrowserClientNativeBrowserSessionController
{
    private readonly IBrowserClientNativeBrowserWebSocketRuntimeExecutionController _nativeBrowserWebSocketRuntimeExecutionController;
    private readonly IBrowserClientRuntimeBrowserSessionStabilityReadyState _browserSessionStabilityReadyState;

    public BrowserClientNativeBrowserSessionControllerService(
        IBrowserClientNativeBrowserWebSocketRuntimeExecutionController nativeBrowserWebSocketRuntimeExecutionController,
        IBrowserClientRuntimeBrowserSessionStabilityReadyState browserSessionStabilityReadyState
    )
    {
        _nativeBrowserWebSocketRuntimeExecutionController = nativeBrowserWebSocketRuntimeExecutionController;
        _browserSessionStabilityReadyState = browserSessionStabilityReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult nativeBrowserWebSocketRuntimeExecutionController = await _nativeBrowserWebSocketRuntimeExecutionController.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult browserSessionStabilityReadyState = await _browserSessionStabilityReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserSessionControllerResult result = new()
        {
            ProfileId = nativeBrowserWebSocketRuntimeExecutionController.ProfileId,
            AssetRootPath = nativeBrowserWebSocketRuntimeExecutionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserWebSocketRuntimeExecutionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserWebSocketRuntimeExecutionController.CacheRootPath,
            ConfigRootPath = nativeBrowserWebSocketRuntimeExecutionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserWebSocketRuntimeExecutionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserWebSocketRuntimeExecutionController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserWebSocketRuntimeExecutionController.ReadyAssetCount,
            CacheHits = nativeBrowserWebSocketRuntimeExecutionController.CacheHits,
            NativeBrowserRuntimeExecutionReady = nativeBrowserWebSocketRuntimeExecutionController.NativeBrowserWebSocketSessionReady,
            NativeBrowserRuntimeExecutionVersion = nativeBrowserWebSocketRuntimeExecutionController.NativeBrowserWebSocketSessionVersion,
            NativeBrowserRuntimeExecutionSummary = nativeBrowserWebSocketRuntimeExecutionController.NativeBrowserWebSocketSessionSummary,
            NativeBrowserWebSocketSessionReady = nativeBrowserWebSocketRuntimeExecutionController.IsReady,
            NativeBrowserWebSocketSessionVersion = nativeBrowserWebSocketRuntimeExecutionController.NativeBrowserWebSocketRuntimeExecutionVersion,
            NativeBrowserWebSocketSessionSummary = nativeBrowserWebSocketRuntimeExecutionController.Summary,
            WebSocketSessionEndpoint = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionEndpoint,
            WebSocketSessionScheme = nativeBrowserWebSocketRuntimeExecutionController.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionProtocol,
            BrowserSessionStabilityReady = browserSessionStabilityReadyState.IsReady,
            BrowserSessionStabilityReadyStateVersion = browserSessionStabilityReadyState.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = browserSessionStabilityReadyState.Summary,
            TransportMode = nativeBrowserWebSocketRuntimeExecutionController.TransportMode,
            TransportEndpoint = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionEndpoint,
            RuntimeExecutionMode = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionMode,
            LaunchMode = nativeBrowserWebSocketRuntimeExecutionController.LaunchMode,
            ContractVersion = nativeBrowserWebSocketRuntimeExecutionController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserWebSocketRuntimeExecutionController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserWebSocketRuntimeExecutionController.IsReady ? "native-browser-websocket-runtime-execution-ready" : "native-browser-websocket-runtime-execution-blocked",
            browserSessionStabilityReadyState.IsReady ? "browser-session-stability-ready" : "browser-session-stability-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserSessionVersion = "browser-native-browser-session-controller-v3";
        result.NativeBrowserSessionStages =
        [
            "bind-native-browser-websocket-runtime-execution-controller",
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
