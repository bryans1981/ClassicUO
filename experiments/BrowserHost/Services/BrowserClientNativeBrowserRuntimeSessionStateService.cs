using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeSessionState
{
    ValueTask<BrowserClientNativeBrowserRuntimeSessionStateResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeSessionStateService : IBrowserClientNativeBrowserRuntimeSessionState
{
    private readonly IBrowserClientNativeBrowserRuntimeSessionSlice _nativeBrowserRuntimeSessionSlice;
    private readonly IBrowserClientNativeBrowserRuntimeSessionController _nativeBrowserRuntimeSessionController;

    public BrowserClientNativeBrowserRuntimeSessionStateService(
        IBrowserClientNativeBrowserRuntimeSessionSlice nativeBrowserRuntimeSessionSlice,
        IBrowserClientNativeBrowserRuntimeSessionController nativeBrowserRuntimeSessionController
    )
    {
        _nativeBrowserRuntimeSessionSlice = nativeBrowserRuntimeSessionSlice;
        _nativeBrowserRuntimeSessionController = nativeBrowserRuntimeSessionController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeSessionStateResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeSessionSliceResult nativeBrowserRuntimeSessionSlice = await _nativeBrowserRuntimeSessionSlice.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserRuntimeSessionControllerResult nativeBrowserRuntimeSessionController = await _nativeBrowserRuntimeSessionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeSessionStateResult result = new()
        {
            ProfileId = nativeBrowserRuntimeSessionSlice.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeSessionSlice.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeSessionSlice.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeSessionSlice.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeSessionSlice.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeSessionSlice.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeSessionSlice.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeSessionSlice.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeSessionSlice.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeSessionSlice.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeSessionSlice.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeSessionSlice.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeSessionSlice.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeSessionSlice.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeSessionSlice.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeSessionSlice.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeSessionSlice.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeSessionSlice.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeSessionSlice.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeSessionSlice.NativeBrowserRuntimeLoopReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeSessionSlice.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeSessionSlice.NativeBrowserRuntimeLoopSummary,
            NativeBrowserRuntimeExecutionSliceReady = nativeBrowserRuntimeSessionSlice.NativeBrowserRuntimeExecutionSliceReady,
            NativeBrowserRuntimeExecutionSliceVersion = nativeBrowserRuntimeSessionSlice.NativeBrowserRuntimeExecutionSliceVersion,
            NativeBrowserRuntimeExecutionSliceSummary = nativeBrowserRuntimeSessionSlice.NativeBrowserRuntimeExecutionSliceSummary,
            NativeBrowserNetworkReady = nativeBrowserRuntimeSessionSlice.NativeBrowserNetworkReady,
            NativeBrowserNetworkVersion = nativeBrowserRuntimeSessionSlice.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserRuntimeSessionSlice.NativeBrowserNetworkSummary,
            NativeBrowserTransportReady = nativeBrowserRuntimeSessionSlice.NativeBrowserTransportReady,
            NativeBrowserTransportVersion = nativeBrowserRuntimeSessionSlice.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserRuntimeSessionSlice.NativeBrowserTransportSummary,
            TransportEndpoint = nativeBrowserRuntimeSessionSlice.TransportEndpoint,
            TransportMode = nativeBrowserRuntimeSessionSlice.TransportMode,
            LaunchMode = nativeBrowserRuntimeSessionSlice.LaunchMode,
            ContractVersion = nativeBrowserRuntimeSessionSlice.ContractVersion,
            WebSocketRuntimeExecutionReady = nativeBrowserRuntimeSessionSlice.WebSocketRuntimeExecutionReady,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserRuntimeSessionSlice.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserRuntimeSessionSlice.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserRuntimeSessionSlice.WebSocketRuntimeExecutionMode,
            WebSocketRuntimeExecutionVersion = nativeBrowserRuntimeSessionSlice.WebSocketRuntimeExecutionVersion,
            WebSocketRuntimeExecutionSummary = nativeBrowserRuntimeSessionSlice.WebSocketRuntimeExecutionSummary,
            BrowserSessionReady = nativeBrowserRuntimeSessionSlice.BrowserSessionReady,
            BrowserSessionVersion = nativeBrowserRuntimeSessionSlice.BrowserSessionVersion,
            BrowserSessionSummary = nativeBrowserRuntimeSessionSlice.BrowserSessionSummary,
            BrowserSessionStabilityReady = nativeBrowserRuntimeSessionSlice.BrowserSessionStabilityReady,
            BrowserSessionStabilityReadyStateVersion = nativeBrowserRuntimeSessionSlice.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = nativeBrowserRuntimeSessionSlice.BrowserSessionStabilitySummary,
            WebSocketSessionEndpoint = nativeBrowserRuntimeSessionSlice.WebSocketSessionEndpoint,
            WebSocketSessionScheme = nativeBrowserRuntimeSessionSlice.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserRuntimeSessionSlice.WebSocketSessionProtocol,
            WebSocketSessionReady = nativeBrowserRuntimeSessionSlice.WebSocketSessionReady,
            WebSocketSessionVersion = nativeBrowserRuntimeSessionSlice.WebSocketSessionVersion,
            WebSocketSessionSummary = nativeBrowserRuntimeSessionSlice.WebSocketSessionSummary,
            ReadyAssetCount = nativeBrowserRuntimeSessionSlice.ReadyAssetCount,
            CacheHits = nativeBrowserRuntimeSessionSlice.CacheHits,
            AssetRootPath = nativeBrowserRuntimeSessionSlice.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntimeSessionSlice.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntimeSessionSlice.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntimeSessionSlice.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntimeSessionSlice.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntimeSessionSlice.StartupProfilePath,
            RequiredAssets = nativeBrowserRuntimeSessionSlice.RequiredAssets,
            ReadinessChecks =
            [
                nativeBrowserRuntimeSessionSlice.IsReady ? "native-browser-runtime-session-slice-ready" : "native-browser-runtime-session-slice-blocked",
                nativeBrowserRuntimeSessionController.IsReady ? "native-browser-runtime-session-controller-ready" : "native-browser-runtime-session-controller-blocked"
            ]
        };

        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeSessionStateVersion = "browser-native-browser-runtime-session-state-v1";
        result.NativeBrowserRuntimeSessionStateStages =
        [
            "bind-native-browser-runtime-session-slice",
            "bind-native-browser-runtime-session-controller",
            "publish-browser-native-browser-runtime-session-state"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime session state ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionStateStages.Length} stage(s) and websocket session {result.WebSocketSessionProtocol}."
            : $"Browser-native browser runtime session state blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionStateStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeSessionStateResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeSessionStateVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeSessionStateStages { get; set; } = Array.Empty<string>();
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
