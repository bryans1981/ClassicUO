using ClassicUO.Utility;
using Microsoft.JSInterop;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserCanvasHost
{
    ValueTask<BrowserClientNativeBrowserCanvasHostResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserCanvasHostService : IBrowserClientNativeBrowserCanvasHost
{
    private readonly IBrowserClientNativeBrowserRenderController _nativeBrowserRenderController;
    private readonly IJSRuntime _jsRuntime;

    public BrowserClientNativeBrowserCanvasHostService(
        IBrowserClientNativeBrowserRenderController nativeBrowserRenderController,
        IJSRuntime jsRuntime
    )
    {
        _nativeBrowserRenderController = nativeBrowserRenderController;
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<BrowserClientNativeBrowserCanvasHostResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRenderControllerResult nativeBrowserRenderController = await _nativeBrowserRenderController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserCanvasHostResult result = new()
        {
            ProfileId = nativeBrowserRenderController.ProfileId,
            AssetRootPath = nativeBrowserRenderController.AssetRootPath,
            ProfilesRootPath = nativeBrowserRenderController.ProfilesRootPath,
            CacheRootPath = nativeBrowserRenderController.CacheRootPath,
            ConfigRootPath = nativeBrowserRenderController.ConfigRootPath,
            SettingsFilePath = nativeBrowserRenderController.SettingsFilePath,
            StartupProfilePath = nativeBrowserRenderController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserRenderController.ReadyAssetCount,
            CacheHits = nativeBrowserRenderController.CacheHits,
            NativeBrowserRenderReady = nativeBrowserRenderController.IsReady,
            NativeBrowserRenderVersion = nativeBrowserRenderController.NativeBrowserRenderVersion,
            NativeBrowserRenderSummary = nativeBrowserRenderController.Summary
        };

        result.RequiredAssets = nativeBrowserRenderController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserRenderController.IsReady ? "native-browser-render-ready" : "native-browser-render-blocked"
        ];

        if (!nativeBrowserRenderController.IsReady)
        {
            result.NativeBrowserCanvasHostVersion = "browser-native-browser-canvas-host-v1";
            result.NativeBrowserCanvasHostStages =
            [
                "bind-native-browser-render-controller",
                "browser-canvas-host-blocked"
            ];
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser-native browser canvas host blocked for profile '{result.ProfileId}' with {result.NativeBrowserCanvasHostStages.Length} stage(s).";
            return result;
        }

        BrowserClientNativeBrowserCanvasHostProbeResult probeResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserCanvasHostProbeResult>(
            "classicuoRender.ensureCanvasHost",
            "classicuo-native-canvas-host",
            "classicuo-native-canvas",
            1280,
            720
        );

        result.BrowserCanvasMounted = probeResult.Mounted;
        result.BrowserCanvasCreated = probeResult.Created;
        result.BrowserCanvasContainerId = probeResult.ContainerId;
        result.BrowserCanvasElementId = probeResult.CanvasId;
        result.BrowserCanvasWidth = probeResult.Width;
        result.BrowserCanvasHeight = probeResult.Height;
        result.BrowserCanvasContextType = probeResult.ContextType;
        result.BrowserCanvasContextAcquired = probeResult.ContextAcquired;
        result.BrowserCanvasError = probeResult.Error;
        result.NativeBrowserCanvasHostVersion = "browser-native-browser-canvas-host-v1";
        result.NativeBrowserCanvasHostStages =
        [
            "bind-native-browser-render-controller",
            "ensure-browser-canvas-host",
            "probe-browser-canvas-context",
            "publish-browser-native-browser-canvas-host"
        ];
        result.ReadinessChecks =
        [
            nativeBrowserRenderController.IsReady ? "native-browser-render-ready" : "native-browser-render-blocked",
            probeResult.ContextAcquired ? "browser-canvas-ready" : "browser-canvas-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser canvas host ready for profile '{result.ProfileId}' with {result.NativeBrowserCanvasHostStages.Length} stage(s) using {result.BrowserCanvasContextType}."
            : $"Browser-native browser canvas host blocked for profile '{result.ProfileId}' with {result.NativeBrowserCanvasHostStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserCanvasHostProbeResult
{
    public string ContainerId { get; set; } = string.Empty;
    public string CanvasId { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Mounted { get; set; }
    public bool Created { get; set; }
    public string ContextType { get; set; } = string.Empty;
    public bool ContextAcquired { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientNativeBrowserCanvasHostResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserCanvasHostVersion { get; set; } = string.Empty;
    public string[] NativeBrowserCanvasHostStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserRenderReady { get; set; }
    public string NativeBrowserRenderVersion { get; set; } = string.Empty;
    public string NativeBrowserRenderSummary { get; set; } = string.Empty;
    public bool BrowserCanvasMounted { get; set; }
    public bool BrowserCanvasCreated { get; set; }
    public string BrowserCanvasContainerId { get; set; } = string.Empty;
    public string BrowserCanvasElementId { get; set; } = string.Empty;
    public int BrowserCanvasWidth { get; set; }
    public int BrowserCanvasHeight { get; set; }
    public string BrowserCanvasContextType { get; set; } = string.Empty;
    public bool BrowserCanvasContextAcquired { get; set; }
    public string BrowserCanvasError { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
