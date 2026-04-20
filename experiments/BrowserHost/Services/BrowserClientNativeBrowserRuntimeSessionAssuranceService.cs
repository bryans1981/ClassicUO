using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeSessionAssurance
{
    ValueTask<BrowserClientNativeBrowserRuntimeSessionAssuranceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeSessionAssuranceService : IBrowserClientNativeBrowserRuntimeSessionAssurance
{
    private readonly IBrowserClientNativeBrowserRuntimeSessionReadyState _nativeBrowserRuntimeSessionReadyState;
    private readonly IBrowserClientNativeBrowserSessionController _nativeBrowserSessionController;

    public BrowserClientNativeBrowserRuntimeSessionAssuranceService(
        IBrowserClientNativeBrowserRuntimeSessionReadyState nativeBrowserRuntimeSessionReadyState,
        IBrowserClientNativeBrowserSessionController nativeBrowserSessionController
    )
    {
        _nativeBrowserRuntimeSessionReadyState = nativeBrowserRuntimeSessionReadyState;
        _nativeBrowserSessionController = nativeBrowserSessionController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeSessionAssuranceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeSessionReadyStateResult nativeBrowserRuntimeSessionReadyState = await _nativeBrowserRuntimeSessionReadyState.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserSessionControllerResult nativeBrowserSessionController = await _nativeBrowserSessionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeSessionAssuranceResult result = new()
        {
            ProfileId = nativeBrowserRuntimeSessionReadyState.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeSessionReadyState.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeSessionReadyState.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeSessionReadyState.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeSessionReadyState.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeSessionReadyState.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeSessionReadyState.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeSessionReadyState.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeSessionReadyState.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeSessionReadyState.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeSessionReadyState.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeSessionReadyState.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeSessionReadyState.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeSessionReadyState.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeSessionReadyState.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeSessionReadyState.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeSessionReadyState.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeSessionReadyState.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeSessionReadyState.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeSessionReadyState.NativeBrowserRuntimeLoopReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeSessionReadyState.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeSessionReadyState.NativeBrowserRuntimeLoopSummary,
            NativeBrowserRuntimeExecutionSliceReady = nativeBrowserRuntimeSessionReadyState.NativeBrowserRuntimeExecutionSliceReady,
            NativeBrowserRuntimeExecutionSliceVersion = nativeBrowserRuntimeSessionReadyState.NativeBrowserRuntimeExecutionSliceVersion,
            NativeBrowserRuntimeExecutionSliceSummary = nativeBrowserRuntimeSessionReadyState.NativeBrowserRuntimeExecutionSliceSummary,
            NativeBrowserNetworkReady = nativeBrowserRuntimeSessionReadyState.NativeBrowserNetworkReady,
            NativeBrowserNetworkVersion = nativeBrowserRuntimeSessionReadyState.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserRuntimeSessionReadyState.NativeBrowserNetworkSummary,
            NativeBrowserTransportReady = nativeBrowserRuntimeSessionReadyState.NativeBrowserTransportReady,
            NativeBrowserTransportVersion = nativeBrowserRuntimeSessionReadyState.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserRuntimeSessionReadyState.NativeBrowserTransportSummary,
            TransportEndpoint = nativeBrowserRuntimeSessionReadyState.TransportEndpoint,
            TransportMode = nativeBrowserRuntimeSessionReadyState.TransportMode,
            LaunchMode = nativeBrowserRuntimeSessionReadyState.LaunchMode,
            ContractVersion = nativeBrowserRuntimeSessionReadyState.ContractVersion,
            WebSocketRuntimeExecutionReady = nativeBrowserRuntimeSessionReadyState.WebSocketRuntimeExecutionReady,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserRuntimeSessionReadyState.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserRuntimeSessionReadyState.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserRuntimeSessionReadyState.WebSocketRuntimeExecutionMode,
            WebSocketRuntimeExecutionVersion = nativeBrowserRuntimeSessionReadyState.WebSocketRuntimeExecutionVersion,
            WebSocketRuntimeExecutionSummary = nativeBrowserRuntimeSessionReadyState.WebSocketRuntimeExecutionSummary,
            BrowserSessionReady = nativeBrowserSessionController.IsReady,
            BrowserSessionVersion = nativeBrowserSessionController.NativeBrowserSessionVersion,
            BrowserSessionSummary = nativeBrowserSessionController.Summary,
            BrowserSessionStabilityReady = nativeBrowserSessionController.BrowserSessionStabilityReady,
            BrowserSessionStabilityReadyStateVersion = nativeBrowserSessionController.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = nativeBrowserSessionController.BrowserSessionStabilitySummary,
            WebSocketSessionEndpoint = nativeBrowserSessionController.WebSocketSessionEndpoint,
            WebSocketSessionScheme = nativeBrowserSessionController.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserSessionController.WebSocketSessionProtocol,
            WebSocketSessionReady = nativeBrowserSessionController.NativeBrowserWebSocketSessionReady,
            WebSocketSessionVersion = nativeBrowserSessionController.NativeBrowserWebSocketSessionVersion,
            WebSocketSessionSummary = nativeBrowserSessionController.NativeBrowserWebSocketSessionSummary,
            ReadyAssetCount = nativeBrowserSessionController.ReadyAssetCount,
            CacheHits = nativeBrowserSessionController.CacheHits,
            AssetRootPath = nativeBrowserSessionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserSessionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserSessionController.CacheRootPath,
            ConfigRootPath = nativeBrowserSessionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserSessionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserSessionController.StartupProfilePath,
            RequiredAssets = nativeBrowserSessionController.RequiredAssets,
            ReadinessChecks =
            [
                nativeBrowserRuntimeSessionReadyState.IsReady ? "native-browser-runtime-session-ready-state-ready" : "native-browser-runtime-session-ready-state-blocked",
                nativeBrowserSessionController.IsReady ? "native-browser-session-controller-ready" : "native-browser-session-controller-blocked"
            ]
        };

        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeSessionAssuranceVersion = "browser-native-browser-runtime-session-assurance-v1";
        result.NativeBrowserRuntimeSessionAssuranceStages =
        [
            "bind-native-browser-runtime-session-ready-state",
            "bind-native-browser-session-controller",
            "publish-browser-native-browser-runtime-session-assurance"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime session assurance ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionAssuranceStages.Length} stage(s) and websocket session {result.WebSocketSessionProtocol}."
            : $"Browser-native browser runtime session assurance blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeSessionAssuranceResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeSessionAssuranceVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeSessionAssuranceStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string BrowserCanvasContainerId { get; set; } = string.Empty;
    public string BrowserCanvasElementId { get; set; } = string.Empty;
    public int BrowserCanvasWidth { get; set; }
    public int BrowserCanvasHeight { get; set; }
    public string BrowserCanvasContextType { get; set; } = string.Empty;
    public bool BrowserFrameRendered { get; set; }
    public string BrowserFrameContextType { get; set; } = string.Empty;
    public string BrowserFrameText { get; set; } = string.Empty;
    public string BrowserFrameError { get; set; } = string.Empty;
    public bool NativeBrowserCanvasFrameReady { get; set; }
    public string NativeBrowserCanvasFrameVersion { get; set; } = string.Empty;
    public string NativeBrowserCanvasFrameSummary { get; set; } = string.Empty;
    public bool NativeBrowserFramePumpReady { get; set; }
    public string NativeBrowserFramePumpVersion { get; set; } = string.Empty;
    public string NativeBrowserFramePumpSummary { get; set; } = string.Empty;
    public bool BrowserInputReady { get; set; }
    public string BrowserInputReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInputSummary { get; set; } = string.Empty;
    public bool NativeBrowserRuntimeLoopReady { get; set; }
    public string NativeBrowserRuntimeLoopVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeLoopSummary { get; set; } = string.Empty;
    public bool NativeBrowserRuntimeExecutionSliceReady { get; set; }
    public string NativeBrowserRuntimeExecutionSliceVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeExecutionSliceSummary { get; set; } = string.Empty;
    public bool NativeBrowserNetworkReady { get; set; }
    public string NativeBrowserNetworkVersion { get; set; } = string.Empty;
    public string NativeBrowserNetworkSummary { get; set; } = string.Empty;
    public bool NativeBrowserTransportReady { get; set; }
    public string NativeBrowserTransportVersion { get; set; } = string.Empty;
    public string NativeBrowserTransportSummary { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public bool WebSocketRuntimeExecutionReady { get; set; }
    public string WebSocketRuntimeExecutionEndpoint { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionProtocol { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionMode { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionVersion { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionSummary { get; set; } = string.Empty;
    public bool BrowserSessionReady { get; set; }
    public string BrowserSessionVersion { get; set; } = string.Empty;
    public string BrowserSessionSummary { get; set; } = string.Empty;
    public bool BrowserSessionStabilityReady { get; set; }
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySummary { get; set; } = string.Empty;
    public bool WebSocketSessionReady { get; set; }
    public string WebSocketSessionEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
    public string WebSocketSessionProtocol { get; set; } = string.Empty;
    public string WebSocketSessionVersion { get; set; } = string.Empty;
    public string WebSocketSessionSummary { get; set; } = string.Empty;
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
