using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeLoop
{
    ValueTask<BrowserClientNativeBrowserRuntimeLoopResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeLoopService : IBrowserClientNativeBrowserRuntimeLoop
{
    private readonly IBrowserClientNativeBrowserFramePump _nativeBrowserFramePump;
    private readonly IBrowserClientNativeBrowserInputController _nativeBrowserInputController;

    public BrowserClientNativeBrowserRuntimeLoopService(
        IBrowserClientNativeBrowserFramePump nativeBrowserFramePump,
        IBrowserClientNativeBrowserInputController nativeBrowserInputController
    )
    {
        _nativeBrowserFramePump = nativeBrowserFramePump;
        _nativeBrowserInputController = nativeBrowserInputController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeLoopResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserFramePumpResult nativeBrowserFramePump = await _nativeBrowserFramePump.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserInputControllerResult nativeBrowserInputController = await _nativeBrowserInputController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeLoopResult result = new()
        {
            ProfileId = nativeBrowserFramePump.ProfileId,
            BrowserCanvasContainerId = nativeBrowserFramePump.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserFramePump.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserFramePump.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserFramePump.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserFramePump.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserFramePump.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserFramePump.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserFramePump.BrowserFrameText,
            BrowserFrameError = nativeBrowserFramePump.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserFramePump.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserFramePump.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserFramePump.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserFramePump.IsReady,
            NativeBrowserFramePumpVersion = nativeBrowserFramePump.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserFramePump.Summary,
            BrowserInputReady = nativeBrowserInputController.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserInputController.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserInputController.BrowserInputSummary
        };

        result.RequiredAssets = nativeBrowserInputController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserFramePump.IsReady ? "native-browser-frame-pump-ready" : "native-browser-frame-pump-blocked",
            nativeBrowserInputController.IsReady ? "native-browser-input-ready" : "native-browser-input-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeLoopVersion = "browser-native-browser-runtime-loop-v1";
        result.NativeBrowserRuntimeLoopStages =
        [
            "bind-native-browser-frame-pump",
            "bind-native-browser-input-controller",
            "publish-browser-native-browser-runtime-loop"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime loop ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeLoopStages.Length} stage(s)."
            : $"Browser-native browser runtime loop blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeLoopStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeLoopResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeLoopVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeLoopStages { get; set; } = Array.Empty<string>();
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
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
