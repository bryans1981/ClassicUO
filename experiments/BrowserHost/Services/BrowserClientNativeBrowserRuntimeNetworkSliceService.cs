using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeNetworkSlice
{
    ValueTask<BrowserClientNativeBrowserRuntimeNetworkSliceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeNetworkSliceService : IBrowserClientNativeBrowserRuntimeNetworkSlice
{
    private readonly IBrowserClientNativeBrowserRuntimeExecutionSlice _nativeBrowserRuntimeExecutionSlice;
    private readonly IBrowserClientNativeBrowserNetworkController _nativeBrowserNetworkController;
    private readonly IBrowserClientNativeBrowserTransportController _nativeBrowserTransportController;

    public BrowserClientNativeBrowserRuntimeNetworkSliceService(
        IBrowserClientNativeBrowserRuntimeExecutionSlice nativeBrowserRuntimeExecutionSlice,
        IBrowserClientNativeBrowserNetworkController nativeBrowserNetworkController,
        IBrowserClientNativeBrowserTransportController nativeBrowserTransportController
    )
    {
        _nativeBrowserRuntimeExecutionSlice = nativeBrowserRuntimeExecutionSlice;
        _nativeBrowserNetworkController = nativeBrowserNetworkController;
        _nativeBrowserTransportController = nativeBrowserTransportController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeNetworkSliceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeExecutionSliceResult nativeBrowserRuntimeExecutionSlice = await _nativeBrowserRuntimeExecutionSlice.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserNetworkControllerResult nativeBrowserNetworkController = await _nativeBrowserNetworkController.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserTransportControllerResult nativeBrowserTransportController = await _nativeBrowserTransportController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeNetworkSliceResult result = new()
        {
            ProfileId = nativeBrowserRuntimeExecutionSlice.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeExecutionSlice.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeExecutionSlice.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeExecutionSlice.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeExecutionSlice.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeExecutionSlice.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeExecutionSlice.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeExecutionSlice.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeExecutionSlice.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeExecutionSlice.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeExecutionSlice.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeExecutionSlice.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeExecutionSlice.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeExecutionSlice.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeExecutionSlice.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeExecutionSlice.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeExecutionSlice.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeExecutionSlice.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeExecutionSlice.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeExecutionSlice.NativeBrowserRuntimeLoopReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeExecutionSlice.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeExecutionSlice.NativeBrowserRuntimeLoopSummary,
            NativeBrowserRuntimeExecutionSliceReady = nativeBrowserRuntimeExecutionSlice.IsReady,
            NativeBrowserRuntimeExecutionSliceVersion = nativeBrowserRuntimeExecutionSlice.NativeBrowserRuntimeExecutionSliceVersion,
            NativeBrowserRuntimeExecutionSliceSummary = nativeBrowserRuntimeExecutionSlice.Summary,
            NativeBrowserNetworkReady = nativeBrowserNetworkController.IsReady,
            NativeBrowserNetworkVersion = nativeBrowserNetworkController.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserNetworkController.Summary,
            NativeBrowserTransportReady = nativeBrowserTransportController.IsReady,
            NativeBrowserTransportVersion = nativeBrowserTransportController.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserTransportController.Summary,
            TransportEndpoint = nativeBrowserTransportController.TransportEndpoint,
            TransportMode = nativeBrowserTransportController.TransportMode,
            LaunchMode = nativeBrowserTransportController.LaunchMode,
            ContractVersion = nativeBrowserTransportController.ContractVersion,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserRuntimeExecutionSlice.TransportEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserRuntimeExecutionSlice.LaunchMode,
            WebSocketRuntimeExecutionMode = nativeBrowserNetworkController.TransportMode,
            WebSocketRuntimeExecutionReady = nativeBrowserRuntimeExecutionSlice.IsReady && nativeBrowserNetworkController.IsReady,
            WebSocketRuntimeExecutionVersion = nativeBrowserRuntimeExecutionSlice.NativeBrowserRuntimeExecutionSliceVersion,
            WebSocketRuntimeExecutionSummary = nativeBrowserRuntimeExecutionSlice.Summary
        };

        result.ReadinessChecks =
        [
            nativeBrowserRuntimeExecutionSlice.IsReady ? "native-browser-runtime-execution-slice-ready" : "native-browser-runtime-execution-slice-blocked",
            nativeBrowserNetworkController.IsReady ? "native-browser-network-ready" : "native-browser-network-blocked",
            nativeBrowserTransportController.IsReady ? "native-browser-transport-ready" : "native-browser-transport-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeNetworkSliceVersion = "browser-native-browser-runtime-network-slice-v1";
        result.NativeBrowserRuntimeNetworkSliceStages =
        [
            "bind-native-browser-runtime-execution-slice",
            "bind-native-browser-network-controller",
            "bind-native-browser-transport-controller",
            "publish-browser-native-browser-runtime-network-slice"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime network slice ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeNetworkSliceStages.Length} stage(s) and endpoint {result.TransportEndpoint}."
            : $"Browser-native browser runtime network slice blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeNetworkSliceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeNetworkSliceResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeNetworkSliceVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeNetworkSliceStages { get; set; } = Array.Empty<string>();
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
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
