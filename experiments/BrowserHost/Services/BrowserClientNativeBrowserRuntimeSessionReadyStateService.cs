using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeSessionReadyState
{
    ValueTask<BrowserClientNativeBrowserRuntimeSessionReadyStateResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeSessionReadyStateService : IBrowserClientNativeBrowserRuntimeSessionReadyState
{
    private readonly IBrowserClientNativeBrowserRuntimeSessionState _nativeBrowserRuntimeSessionState;
    private readonly IBrowserClientRuntimeBrowserSessionStabilityReadyState _browserSessionStabilityReadyState;

    public BrowserClientNativeBrowserRuntimeSessionReadyStateService(
        IBrowserClientNativeBrowserRuntimeSessionState nativeBrowserRuntimeSessionState,
        IBrowserClientRuntimeBrowserSessionStabilityReadyState browserSessionStabilityReadyState
    )
    {
        _nativeBrowserRuntimeSessionState = nativeBrowserRuntimeSessionState;
        _browserSessionStabilityReadyState = browserSessionStabilityReadyState;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeSessionReadyStateResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeSessionStateResult nativeBrowserRuntimeSessionState = await _nativeBrowserRuntimeSessionState.PrepareAsync(request, profileId);
        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult browserSessionStabilityReadyState = await _browserSessionStabilityReadyState.BuildAsync(profileId);

        BrowserClientNativeBrowserRuntimeSessionReadyStateResult result = new()
        {
            ProfileId = nativeBrowserRuntimeSessionState.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeSessionState.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeSessionState.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeSessionState.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeSessionState.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeSessionState.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeSessionState.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeSessionState.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeSessionState.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeSessionState.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeSessionState.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeSessionState.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeSessionState.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeSessionState.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeSessionState.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeSessionState.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeSessionState.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeSessionState.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeSessionState.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeSessionState.NativeBrowserRuntimeLoopReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeSessionState.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeSessionState.NativeBrowserRuntimeLoopSummary,
            NativeBrowserRuntimeExecutionSliceReady = nativeBrowserRuntimeSessionState.NativeBrowserRuntimeExecutionSliceReady,
            NativeBrowserRuntimeExecutionSliceVersion = nativeBrowserRuntimeSessionState.NativeBrowserRuntimeExecutionSliceVersion,
            NativeBrowserRuntimeExecutionSliceSummary = nativeBrowserRuntimeSessionState.NativeBrowserRuntimeExecutionSliceSummary,
            NativeBrowserNetworkReady = nativeBrowserRuntimeSessionState.NativeBrowserNetworkReady,
            NativeBrowserNetworkVersion = nativeBrowserRuntimeSessionState.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserRuntimeSessionState.NativeBrowserNetworkSummary,
            NativeBrowserTransportReady = nativeBrowserRuntimeSessionState.NativeBrowserTransportReady,
            NativeBrowserTransportVersion = nativeBrowserRuntimeSessionState.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserRuntimeSessionState.NativeBrowserTransportSummary,
            TransportEndpoint = nativeBrowserRuntimeSessionState.TransportEndpoint,
            TransportMode = nativeBrowserRuntimeSessionState.TransportMode,
            LaunchMode = nativeBrowserRuntimeSessionState.LaunchMode,
            ContractVersion = nativeBrowserRuntimeSessionState.ContractVersion,
            WebSocketRuntimeExecutionReady = nativeBrowserRuntimeSessionState.WebSocketRuntimeExecutionReady,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserRuntimeSessionState.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserRuntimeSessionState.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserRuntimeSessionState.WebSocketRuntimeExecutionMode,
            WebSocketRuntimeExecutionVersion = nativeBrowserRuntimeSessionState.WebSocketRuntimeExecutionVersion,
            WebSocketRuntimeExecutionSummary = nativeBrowserRuntimeSessionState.WebSocketRuntimeExecutionSummary,
            BrowserSessionReady = nativeBrowserRuntimeSessionState.BrowserSessionReady,
            BrowserSessionVersion = nativeBrowserRuntimeSessionState.BrowserSessionVersion,
            BrowserSessionSummary = nativeBrowserRuntimeSessionState.BrowserSessionSummary,
            BrowserSessionStabilityReady = browserSessionStabilityReadyState.IsReady,
            BrowserSessionStabilityReadyStateVersion = browserSessionStabilityReadyState.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = browserSessionStabilityReadyState.Summary,
            WebSocketSessionEndpoint = nativeBrowserRuntimeSessionState.WebSocketSessionEndpoint,
            WebSocketSessionScheme = nativeBrowserRuntimeSessionState.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserRuntimeSessionState.WebSocketSessionProtocol,
            WebSocketSessionReady = nativeBrowserRuntimeSessionState.WebSocketSessionReady,
            WebSocketSessionVersion = nativeBrowserRuntimeSessionState.WebSocketSessionVersion,
            WebSocketSessionSummary = nativeBrowserRuntimeSessionState.WebSocketSessionSummary,
            ReadyAssetCount = nativeBrowserRuntimeSessionState.ReadyAssetCount,
            CacheHits = nativeBrowserRuntimeSessionState.CacheHits,
            AssetRootPath = nativeBrowserRuntimeSessionState.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntimeSessionState.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntimeSessionState.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntimeSessionState.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntimeSessionState.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntimeSessionState.StartupProfilePath,
            RequiredAssets = nativeBrowserRuntimeSessionState.RequiredAssets,
            ReadinessChecks =
            [
                nativeBrowserRuntimeSessionState.IsReady ? "native-browser-runtime-session-state-ready" : "native-browser-runtime-session-state-blocked",
                browserSessionStabilityReadyState.IsReady ? "browser-session-stability-ready" : "browser-session-stability-blocked"
            ]
        };

        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeSessionReadyStateVersion = "browser-native-browser-runtime-session-ready-state-v1";
        result.NativeBrowserRuntimeSessionReadyStateStages =
        [
            "bind-native-browser-runtime-session-state",
            "bind-browser-session-stability-ready-state",
            "publish-browser-native-browser-runtime-session-ready-state"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime session ready-state ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionReadyStateStages.Length} stage(s) and websocket session {result.WebSocketSessionProtocol}."
            : $"Browser-native browser runtime session ready-state blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionReadyStateStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeSessionReadyStateResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeSessionReadyStateVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeSessionReadyStateStages { get; set; } = Array.Empty<string>();
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
