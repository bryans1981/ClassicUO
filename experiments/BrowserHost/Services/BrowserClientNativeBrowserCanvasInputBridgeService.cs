using ClassicUO.Utility;
using Microsoft.JSInterop;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserCanvasInputBridge
{
    ValueTask<BrowserClientNativeBrowserCanvasInputBridgeResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserCanvasInputBridgeService : IBrowserClientNativeBrowserCanvasInputBridge
{
    private readonly IBrowserClientNativeBrowserCanvasFrame _nativeBrowserCanvasFrame;
    private readonly IJSRuntime _jsRuntime;

    public BrowserClientNativeBrowserCanvasInputBridgeService(
        IBrowserClientNativeBrowserCanvasFrame nativeBrowserCanvasFrame,
        IJSRuntime jsRuntime
    )
    {
        _nativeBrowserCanvasFrame = nativeBrowserCanvasFrame;
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<BrowserClientNativeBrowserCanvasInputBridgeResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserCanvasFrameResult nativeBrowserCanvasFrame = await _nativeBrowserCanvasFrame.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserCanvasInputBridgeResult result = new()
        {
            ProfileId = nativeBrowserCanvasFrame.ProfileId,
            AssetRootPath = nativeBrowserCanvasFrame.AssetRootPath,
            ProfilesRootPath = nativeBrowserCanvasFrame.ProfilesRootPath,
            CacheRootPath = nativeBrowserCanvasFrame.CacheRootPath,
            ConfigRootPath = nativeBrowserCanvasFrame.ConfigRootPath,
            SettingsFilePath = nativeBrowserCanvasFrame.SettingsFilePath,
            StartupProfilePath = nativeBrowserCanvasFrame.StartupProfilePath,
            ReadyAssetCount = nativeBrowserCanvasFrame.ReadyAssetCount,
            CacheHits = nativeBrowserCanvasFrame.CacheHits,
            NativeBrowserCanvasFrameReady = nativeBrowserCanvasFrame.IsReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserCanvasFrame.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserCanvasFrame.Summary,
            BrowserCanvasContainerId = nativeBrowserCanvasFrame.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserCanvasFrame.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserCanvasFrame.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserCanvasFrame.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserCanvasFrame.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserCanvasFrame.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserCanvasFrame.BrowserFrameContextType
        };

        result.RequiredAssets = nativeBrowserCanvasFrame.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserCanvasFrame.IsReady ? "native-browser-canvas-frame-ready" : "native-browser-canvas-frame-blocked"
        ];

        if (!nativeBrowserCanvasFrame.IsReady)
        {
            result.NativeBrowserCanvasInputBridgeVersion = "browser-native-browser-canvas-input-bridge-v1";
            result.NativeBrowserCanvasInputBridgeStages =
            [
                "bind-native-browser-canvas-frame",
                "browser-canvas-input-bridge-blocked"
            ];
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser-native browser canvas input bridge blocked for profile '{result.ProfileId}' with {result.NativeBrowserCanvasInputBridgeStages.Length} stage(s).";
            return result;
        }

        BrowserClientNativeBrowserCanvasInputBridgeProbeResult probeResult;
        BrowserClientNativeBrowserCanvasInputBridgeProbeResult probeStateResult;
        string bridgeError = string.Empty;

        try
        {
            probeResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserCanvasInputBridgeProbeResult>(
                "classicuoRender.installCanvasInputBridge",
                nativeBrowserCanvasFrame.BrowserCanvasElementId
            );

            probeStateResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserCanvasInputBridgeProbeResult>(
                "classicuoRender.probeCanvasInputBridge",
                nativeBrowserCanvasFrame.BrowserCanvasElementId
            );
        }
        catch (JSException jsException)
        {
            bridgeError = jsException.Message;
            probeResult = new BrowserClientNativeBrowserCanvasInputBridgeProbeResult
            {
                Error = bridgeError
            };
            probeStateResult = new BrowserClientNativeBrowserCanvasInputBridgeProbeResult
            {
                Error = bridgeError
            };
        }

        result.InputBridgeInstalled = probeResult.Installed;
        result.InputBridgeActive = probeStateResult.Active;
        result.PointerDownCount = probeStateResult.PointerDownCount;
        result.PointerUpCount = probeStateResult.PointerUpCount;
        result.PointerMoveCount = probeStateResult.PointerMoveCount;
        result.KeyDownCount = probeStateResult.KeyDownCount;
        result.KeyUpCount = probeStateResult.KeyUpCount;
        result.FocusCount = probeStateResult.FocusCount;
        result.BlurCount = probeStateResult.BlurCount;
        result.LastEvent = probeStateResult.LastEvent;
        result.InputBridgeError = probeStateResult.Error;
        result.NativeBrowserCanvasInputBridgeVersion = "browser-native-browser-canvas-input-bridge-v1";
        if (string.IsNullOrWhiteSpace(bridgeError))
        {
            result.NativeBrowserCanvasInputBridgeStages =
            [
                "bind-native-browser-canvas-frame",
                "install-canvas-input-bridge",
                "probe-canvas-input-bridge",
                "publish-browser-native-browser-canvas-input-bridge"
            ];
            result.ReadinessChecks =
            [
                nativeBrowserCanvasFrame.IsReady ? "native-browser-canvas-frame-ready" : "native-browser-canvas-frame-blocked",
                probeResult.Installed ? "browser-canvas-input-installed-ready" : "browser-canvas-input-installed-blocked",
                probeStateResult.Active ? "browser-canvas-input-ready" : "browser-canvas-input-blocked"
            ];
            result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        }
        else
        {
            result.NativeBrowserCanvasInputBridgeStages =
            [
                "bind-native-browser-canvas-frame",
                "install-canvas-input-bridge-deferred",
                "probe-canvas-input-bridge-deferred",
                "publish-browser-native-browser-canvas-input-bridge-deferred"
            ];
            result.ReadinessChecks =
            [
                nativeBrowserCanvasFrame.IsReady ? "native-browser-canvas-frame-ready" : "native-browser-canvas-frame-blocked",
                "browser-canvas-input-bridge-deferred"
            ];
            result.IsReady = nativeBrowserCanvasFrame.IsReady;
        }

        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? (string.IsNullOrWhiteSpace(bridgeError)
                ? $"Browser-native browser canvas input bridge ready for profile '{result.ProfileId}' with {result.NativeBrowserCanvasInputBridgeStages.Length} stage(s) and {result.PointerDownCount} pointer-down probe(s)."
                : $"Browser-native browser canvas input bridge deferred for profile '{result.ProfileId}' with {result.NativeBrowserCanvasInputBridgeStages.Length} stage(s).")
            : $"Browser-native browser canvas input bridge blocked for profile '{result.ProfileId}' with {result.NativeBrowserCanvasInputBridgeStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserCanvasInputBridgeProbeResult
{
    public bool Installed { get; set; }
    public bool Active { get; set; }
    public int PointerDownCount { get; set; }
    public int PointerUpCount { get; set; }
    public int PointerMoveCount { get; set; }
    public int KeyDownCount { get; set; }
    public int KeyUpCount { get; set; }
    public int FocusCount { get; set; }
    public int BlurCount { get; set; }
    public string LastEvent { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientNativeBrowserCanvasInputBridgeResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserCanvasInputBridgeVersion { get; set; } = string.Empty;
    public string[] NativeBrowserCanvasInputBridgeStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserCanvasFrameReady { get; set; }
    public string NativeBrowserCanvasFrameVersion { get; set; } = string.Empty;
    public string NativeBrowserCanvasFrameSummary { get; set; } = string.Empty;
    public string BrowserCanvasContainerId { get; set; } = string.Empty;
    public string BrowserCanvasElementId { get; set; } = string.Empty;
    public int BrowserCanvasWidth { get; set; }
    public int BrowserCanvasHeight { get; set; }
    public string BrowserCanvasContextType { get; set; } = string.Empty;
    public bool BrowserFrameRendered { get; set; }
    public string BrowserFrameContextType { get; set; } = string.Empty;
    public bool InputBridgeInstalled { get; set; }
    public bool InputBridgeActive { get; set; }
    public int PointerDownCount { get; set; }
    public int PointerUpCount { get; set; }
    public int PointerMoveCount { get; set; }
    public int KeyDownCount { get; set; }
    public int KeyUpCount { get; set; }
    public int FocusCount { get; set; }
    public int BlurCount { get; set; }
    public string LastEvent { get; set; } = string.Empty;
    public string InputBridgeError { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
