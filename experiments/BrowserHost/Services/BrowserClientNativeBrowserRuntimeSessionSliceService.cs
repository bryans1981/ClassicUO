using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeSessionSlice
{
    ValueTask<BrowserClientNativeBrowserRuntimeSessionSliceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeSessionSliceService : IBrowserClientNativeBrowserRuntimeSessionSlice
{
    private readonly IBrowserClientNativeBrowserRuntimeNetworkSlice _nativeBrowserRuntimeNetworkSlice;
    private readonly IBrowserClientNativeBrowserSessionController _nativeBrowserSessionController;

    public BrowserClientNativeBrowserRuntimeSessionSliceService(
        IBrowserClientNativeBrowserRuntimeNetworkSlice nativeBrowserRuntimeNetworkSlice,
        IBrowserClientNativeBrowserSessionController nativeBrowserSessionController
    )
    {
        _nativeBrowserRuntimeNetworkSlice = nativeBrowserRuntimeNetworkSlice;
        _nativeBrowserSessionController = nativeBrowserSessionController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeSessionSliceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeNetworkSliceResult nativeBrowserRuntimeNetworkSlice = await _nativeBrowserRuntimeNetworkSlice.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserSessionControllerResult nativeBrowserSessionController = await _nativeBrowserSessionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeSessionSliceResult result = new()
        {
            ProfileId = nativeBrowserRuntimeNetworkSlice.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeNetworkSlice.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeNetworkSlice.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeNetworkSlice.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeNetworkSlice.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeNetworkSlice.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeNetworkSlice.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeNetworkSlice.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeNetworkSlice.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeNetworkSlice.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeNetworkSlice.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeNetworkSlice.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeNetworkSlice.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeNetworkSlice.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeNetworkSlice.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeNetworkSlice.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeNetworkSlice.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeNetworkSlice.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeNetworkSlice.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeNetworkSlice.NativeBrowserRuntimeLoopReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeNetworkSlice.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeNetworkSlice.NativeBrowserRuntimeLoopSummary,
            NativeBrowserRuntimeExecutionSliceReady = nativeBrowserRuntimeNetworkSlice.NativeBrowserRuntimeExecutionSliceReady,
            NativeBrowserRuntimeExecutionSliceVersion = nativeBrowserRuntimeNetworkSlice.NativeBrowserRuntimeExecutionSliceVersion,
            NativeBrowserRuntimeExecutionSliceSummary = nativeBrowserRuntimeNetworkSlice.NativeBrowserRuntimeExecutionSliceSummary,
            NativeBrowserNetworkReady = nativeBrowserRuntimeNetworkSlice.NativeBrowserNetworkReady,
            NativeBrowserNetworkVersion = nativeBrowserRuntimeNetworkSlice.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserRuntimeNetworkSlice.NativeBrowserNetworkSummary,
            NativeBrowserTransportReady = nativeBrowserRuntimeNetworkSlice.NativeBrowserTransportReady,
            NativeBrowserTransportVersion = nativeBrowserRuntimeNetworkSlice.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserRuntimeNetworkSlice.NativeBrowserTransportSummary,
            TransportEndpoint = nativeBrowserRuntimeNetworkSlice.TransportEndpoint,
            TransportMode = nativeBrowserRuntimeNetworkSlice.TransportMode,
            LaunchMode = nativeBrowserRuntimeNetworkSlice.LaunchMode,
            ContractVersion = nativeBrowserRuntimeNetworkSlice.ContractVersion,
            WebSocketRuntimeExecutionReady = nativeBrowserRuntimeNetworkSlice.WebSocketRuntimeExecutionReady,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserRuntimeNetworkSlice.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserRuntimeNetworkSlice.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserRuntimeNetworkSlice.WebSocketRuntimeExecutionMode,
            WebSocketRuntimeExecutionVersion = nativeBrowserRuntimeNetworkSlice.WebSocketRuntimeExecutionVersion,
            WebSocketRuntimeExecutionSummary = nativeBrowserRuntimeNetworkSlice.WebSocketRuntimeExecutionSummary,
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
                nativeBrowserRuntimeNetworkSlice.IsReady ? "native-browser-runtime-network-ready" : "native-browser-runtime-network-blocked",
                nativeBrowserSessionController.IsReady ? "browser-session-ready" : "browser-session-blocked"
            ]
        };

        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeSessionSliceVersion = "browser-native-browser-runtime-session-slice-v1";
        result.NativeBrowserRuntimeSessionSliceStages =
        [
            "bind-native-browser-runtime-network-slice",
            "bind-native-browser-session-controller",
            "publish-browser-native-browser-runtime-session-slice"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime session slice ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionSliceStages.Length} stage(s) and websocket endpoint {result.WebSocketSessionEndpoint}."
            : $"Browser-native browser runtime session slice blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeSessionSliceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeSessionSliceResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeSessionSliceVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeSessionSliceStages { get; set; } = Array.Empty<string>();
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
