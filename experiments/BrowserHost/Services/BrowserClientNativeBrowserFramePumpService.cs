using ClassicUO.Utility;
using Microsoft.JSInterop;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserFramePump
{
    ValueTask<BrowserClientNativeBrowserFramePumpResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserFramePumpService : IBrowserClientNativeBrowserFramePump
{
    private readonly IBrowserClientNativeBrowserCanvasFrame _nativeBrowserCanvasFrame;
    private readonly IJSRuntime _jsRuntime;

    public BrowserClientNativeBrowserFramePumpService(
        IBrowserClientNativeBrowserCanvasFrame nativeBrowserCanvasFrame,
        IJSRuntime jsRuntime
    )
    {
        _nativeBrowserCanvasFrame = nativeBrowserCanvasFrame;
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<BrowserClientNativeBrowserFramePumpResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserCanvasFrameResult nativeBrowserCanvasFrame = await _nativeBrowserCanvasFrame.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserFramePumpResult result = new()
        {
            ProfileId = nativeBrowserCanvasFrame.ProfileId,
            BrowserCanvasContainerId = nativeBrowserCanvasFrame.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserCanvasFrame.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserCanvasFrame.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserCanvasFrame.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserCanvasFrame.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserCanvasFrame.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserCanvasFrame.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserCanvasFrame.BrowserFrameText,
            BrowserFrameError = nativeBrowserCanvasFrame.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserCanvasFrame.IsReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserCanvasFrame.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserCanvasFrame.Summary
        };

        result.ReadinessChecks =
        [
            nativeBrowserCanvasFrame.IsReady ? "native-browser-canvas-frame-ready" : "native-browser-canvas-frame-blocked"
        ];

        if (!nativeBrowserCanvasFrame.IsReady)
        {
            result.NativeBrowserFramePumpVersion = "browser-native-browser-frame-pump-v1";
            result.NativeBrowserFramePumpStages =
            [
                "bind-native-browser-canvas-frame",
                "browser-frame-pump-blocked"
            ];
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser-native browser frame pump blocked for profile '{result.ProfileId}' with {result.NativeBrowserFramePumpStages.Length} stage(s).";
            return result;
        }

        BrowserClientNativeBrowserFramePumpProbeResult startResult;
        BrowserClientNativeBrowserFramePumpProbeResult probeResult;
        string pumpError = string.Empty;

        try
        {
            startResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserFramePumpProbeResult>(
                "classicuoRender.startCanvasFramePump",
                nativeBrowserCanvasFrame.BrowserCanvasElementId,
                "ClassicUO browser frame pump",
                16
            );

            await Task.Delay(75);

            probeResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserFramePumpProbeResult>(
                "classicuoRender.probeCanvasFramePump",
                nativeBrowserCanvasFrame.BrowserCanvasElementId
            );

            await _jsRuntime.InvokeVoidAsync(
                "classicuoRender.stopCanvasFramePump",
                nativeBrowserCanvasFrame.BrowserCanvasElementId
            );
        }
        catch (JSException jsException)
        {
            pumpError = jsException.Message;
            startResult = new BrowserClientNativeBrowserFramePumpProbeResult
            {
                Error = pumpError
            };
            probeResult = new BrowserClientNativeBrowserFramePumpProbeResult
            {
                Error = pumpError
            };
        }

        result.FramePumpStarted = startResult.Started;
        result.FramePumpActive = probeResult.Active;
        result.FrameCount = probeResult.FrameCount;
        result.IntervalMs = probeResult.IntervalMs;
        result.LastFrameAt = probeResult.LastFrameAt;
        result.FramePumpError = probeResult.Error;
        result.NativeBrowserFramePumpVersion = "browser-native-browser-frame-pump-v1";

        if (string.IsNullOrWhiteSpace(pumpError))
        {
            result.NativeBrowserFramePumpStages =
            [
                "bind-native-browser-canvas-frame",
                "start-browser-frame-pump",
                "probe-browser-frame-pump",
                "stop-browser-frame-pump",
                "publish-browser-native-browser-frame-pump"
            ];
            result.ReadinessChecks =
            [
                nativeBrowserCanvasFrame.IsReady ? "native-browser-canvas-frame-ready" : "native-browser-canvas-frame-blocked",
                startResult.Started ? "browser-frame-pump-started-ready" : "browser-frame-pump-started-blocked",
                probeResult.FrameCount > 0 ? "browser-frame-pump-observed-ready" : "browser-frame-pump-observed-blocked"
            ];
            result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        }
        else
        {
            result.NativeBrowserFramePumpStages =
            [
                "bind-native-browser-canvas-frame",
                "browser-frame-pump-deferred"
            ];
            result.ReadinessChecks =
            [
                nativeBrowserCanvasFrame.IsReady ? "native-browser-canvas-frame-ready" : "native-browser-canvas-frame-blocked",
                "browser-frame-pump-deferred"
            ];
            result.IsReady = nativeBrowserCanvasFrame.IsReady;
        }

        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? (string.IsNullOrWhiteSpace(pumpError)
                ? $"Browser-native browser frame pump ready for profile '{result.ProfileId}' with {result.NativeBrowserFramePumpStages.Length} stage(s) and {result.FrameCount} pumped frame(s)."
                : $"Browser-native browser frame pump deferred for profile '{result.ProfileId}' with {result.NativeBrowserFramePumpStages.Length} stage(s).")
            : $"Browser-native browser frame pump blocked for profile '{result.ProfileId}' with {result.NativeBrowserFramePumpStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserFramePumpProbeResult
{
    public bool Started { get; set; }
    public bool Active { get; set; }
    public int FrameCount { get; set; }
    public int IntervalMs { get; set; }
    public string LastFrameAt { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientNativeBrowserFramePumpResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserFramePumpVersion { get; set; } = string.Empty;
    public string[] NativeBrowserFramePumpStages { get; set; } = Array.Empty<string>();
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
    public bool FramePumpStarted { get; set; }
    public bool FramePumpActive { get; set; }
    public int FrameCount { get; set; }
    public int IntervalMs { get; set; }
    public string LastFrameAt { get; set; } = string.Empty;
    public string FramePumpError { get; set; } = string.Empty;
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
