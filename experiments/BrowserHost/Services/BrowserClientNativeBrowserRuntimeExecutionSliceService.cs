using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeExecutionSlice
{
    ValueTask<BrowserClientNativeBrowserRuntimeExecutionSliceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeExecutionSliceService : IBrowserClientNativeBrowserRuntimeExecutionSlice
{
    private readonly IBrowserClientNativeBrowserRuntimeLoop _nativeBrowserRuntimeLoop;
    private readonly IBrowserClientNativeBrowserTransportController _nativeBrowserTransportController;
    private readonly IBrowserClientNativeBrowserWebSocketRuntimeExecutionController _nativeBrowserWebSocketRuntimeExecutionController;
    private readonly IBrowserClientNativeBrowserWebSocketRuntimeSessionController _nativeBrowserWebSocketRuntimeSessionController;

    public BrowserClientNativeBrowserRuntimeExecutionSliceService(
        IBrowserClientNativeBrowserRuntimeLoop nativeBrowserRuntimeLoop,
        IBrowserClientNativeBrowserTransportController nativeBrowserTransportController,
        IBrowserClientNativeBrowserWebSocketRuntimeExecutionController nativeBrowserWebSocketRuntimeExecutionController,
        IBrowserClientNativeBrowserWebSocketRuntimeSessionController nativeBrowserWebSocketRuntimeSessionController
    )
    {
        _nativeBrowserRuntimeLoop = nativeBrowserRuntimeLoop;
        _nativeBrowserTransportController = nativeBrowserTransportController;
        _nativeBrowserWebSocketRuntimeExecutionController = nativeBrowserWebSocketRuntimeExecutionController;
        _nativeBrowserWebSocketRuntimeSessionController = nativeBrowserWebSocketRuntimeSessionController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeExecutionSliceResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeLoopResult nativeBrowserRuntimeLoop = await _nativeBrowserRuntimeLoop.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserTransportControllerResult nativeBrowserTransportController = await _nativeBrowserTransportController.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult nativeBrowserWebSocketRuntimeExecutionController = await _nativeBrowserWebSocketRuntimeExecutionController.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult nativeBrowserWebSocketRuntimeSessionController = await _nativeBrowserWebSocketRuntimeSessionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeExecutionSliceResult result = new()
        {
            ProfileId = nativeBrowserRuntimeLoop.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeLoop.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeLoop.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeLoop.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeLoop.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeLoop.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeLoop.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeLoop.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeLoop.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeLoop.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeLoop.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeLoop.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeLoop.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeLoop.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeLoop.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeLoop.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeLoop.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeLoop.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeLoop.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeLoop.IsReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeLoop.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeLoop.Summary,
            TransportEndpoint = nativeBrowserTransportController.TransportEndpoint,
            TransportMode = nativeBrowserTransportController.TransportMode,
            LaunchMode = nativeBrowserTransportController.LaunchMode,
            ContractVersion = nativeBrowserTransportController.ContractVersion,
            NativeBrowserWebSocketRuntimeExecutionReady = nativeBrowserWebSocketRuntimeExecutionController.IsReady,
            NativeBrowserWebSocketRuntimeExecutionVersion = nativeBrowserWebSocketRuntimeExecutionController.NativeBrowserWebSocketRuntimeExecutionVersion,
            NativeBrowserWebSocketRuntimeExecutionSummary = nativeBrowserWebSocketRuntimeExecutionController.Summary,
            NativeBrowserWebSocketRuntimeSessionReady = nativeBrowserWebSocketRuntimeSessionController.IsReady,
            NativeBrowserWebSocketRuntimeSessionVersion = nativeBrowserWebSocketRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionVersion,
            NativeBrowserWebSocketRuntimeSessionSummary = nativeBrowserWebSocketRuntimeSessionController.Summary
        };

        result.ReadinessChecks =
        [
            nativeBrowserRuntimeLoop.IsReady ? "native-browser-runtime-loop-ready" : "native-browser-runtime-loop-blocked",
            nativeBrowserTransportController.IsReady ? "native-browser-transport-ready" : "native-browser-transport-blocked",
            nativeBrowserWebSocketRuntimeExecutionController.IsReady ? "native-browser-websocket-runtime-execution-ready" : "native-browser-websocket-runtime-execution-blocked",
            nativeBrowserWebSocketRuntimeSessionController.IsReady ? "native-browser-websocket-runtime-session-ready" : "native-browser-websocket-runtime-session-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeExecutionSliceVersion = "browser-native-browser-runtime-execution-slice-v1";
        result.NativeBrowserRuntimeExecutionSliceStages =
        [
            "bind-native-browser-runtime-loop",
            "bind-native-browser-transport-controller",
            "bind-native-browser-websocket-runtime-execution-controller",
            "bind-native-browser-websocket-runtime-session-controller",
            "publish-browser-native-browser-runtime-execution-slice"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime execution slice ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeExecutionSliceStages.Length} stage(s) and transport {result.TransportMode}."
            : $"Browser-native browser runtime execution slice blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeExecutionSliceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeExecutionSliceResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeExecutionSliceVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeExecutionSliceStages { get; set; } = Array.Empty<string>();
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
    public string TransportEndpoint { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public bool NativeBrowserWebSocketRuntimeExecutionReady { get; set; }
    public string NativeBrowserWebSocketRuntimeExecutionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketRuntimeExecutionSummary { get; set; } = string.Empty;
    public bool NativeBrowserWebSocketRuntimeSessionReady { get; set; }
    public string NativeBrowserWebSocketRuntimeSessionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketRuntimeSessionSummary { get; set; } = string.Empty;
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
