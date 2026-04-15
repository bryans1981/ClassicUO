using ClassicUO.Utility;
using Microsoft.JSInterop;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserCanvasFrame
{
    ValueTask<BrowserClientNativeBrowserCanvasFrameResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserCanvasFrameService : IBrowserClientNativeBrowserCanvasFrame
{
    private readonly IBrowserClientNativeBrowserCanvasHost _nativeBrowserCanvasHost;
    private readonly IJSRuntime _jsRuntime;

    public BrowserClientNativeBrowserCanvasFrameService(
        IBrowserClientNativeBrowserCanvasHost nativeBrowserCanvasHost,
        IJSRuntime jsRuntime
    )
    {
        _nativeBrowserCanvasHost = nativeBrowserCanvasHost;
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<BrowserClientNativeBrowserCanvasFrameResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserCanvasHostResult nativeBrowserCanvasHost = await _nativeBrowserCanvasHost.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserCanvasFrameResult result = new()
        {
            ProfileId = nativeBrowserCanvasHost.ProfileId,
            AssetRootPath = nativeBrowserCanvasHost.AssetRootPath,
            ProfilesRootPath = nativeBrowserCanvasHost.ProfilesRootPath,
            CacheRootPath = nativeBrowserCanvasHost.CacheRootPath,
            ConfigRootPath = nativeBrowserCanvasHost.ConfigRootPath,
            SettingsFilePath = nativeBrowserCanvasHost.SettingsFilePath,
            StartupProfilePath = nativeBrowserCanvasHost.StartupProfilePath,
            ReadyAssetCount = nativeBrowserCanvasHost.ReadyAssetCount,
            CacheHits = nativeBrowserCanvasHost.CacheHits,
            NativeBrowserCanvasHostReady = nativeBrowserCanvasHost.IsReady,
            NativeBrowserCanvasHostVersion = nativeBrowserCanvasHost.NativeBrowserCanvasHostVersion,
            NativeBrowserCanvasHostSummary = nativeBrowserCanvasHost.Summary,
            BrowserCanvasContainerId = nativeBrowserCanvasHost.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserCanvasHost.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserCanvasHost.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserCanvasHost.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserCanvasHost.BrowserCanvasContextType,
            BrowserCanvasContextAcquired = nativeBrowserCanvasHost.BrowserCanvasContextAcquired
        };

        result.RequiredAssets = nativeBrowserCanvasHost.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserCanvasHost.IsReady ? "native-browser-canvas-host-ready" : "native-browser-canvas-host-blocked"
        ];

        if (!nativeBrowserCanvasHost.IsReady)
        {
            result.NativeBrowserCanvasFrameVersion = "browser-native-browser-canvas-frame-v1";
            result.NativeBrowserCanvasFrameStages =
            [
                "bind-native-browser-canvas-host",
                "browser-canvas-frame-blocked"
            ];
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser-native browser canvas frame blocked for profile '{result.ProfileId}' with {result.NativeBrowserCanvasFrameStages.Length} stage(s).";
            return result;
        }

        BrowserClientNativeBrowserCanvasFrameProbeResult probeResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserCanvasFrameProbeResult>(
            "classicuoRender.renderCanvasFrame",
            nativeBrowserCanvasHost.BrowserCanvasContainerId,
            nativeBrowserCanvasHost.BrowserCanvasElementId,
            nativeBrowserCanvasHost.BrowserCanvasWidth,
            nativeBrowserCanvasHost.BrowserCanvasHeight,
            nativeBrowserCanvasHost.BrowserCanvasContextType,
            "ClassicUO browser canvas ready",
            "#0f172a",
            "#f8fafc"
        );

        result.BrowserFrameRendered = probeResult.Rendered;
        result.BrowserFrameContextType = probeResult.ContextType;
        result.BrowserFrameText = probeResult.Text;
        result.BrowserFrameError = probeResult.Error;
        result.NativeBrowserCanvasFrameVersion = "browser-native-browser-canvas-frame-v1";
        result.NativeBrowserCanvasFrameStages =
        [
            "bind-native-browser-canvas-host",
            "render-browser-canvas-frame",
            "publish-browser-native-browser-canvas-frame"
        ];
        result.ReadinessChecks =
        [
            nativeBrowserCanvasHost.IsReady ? "native-browser-canvas-host-ready" : "native-browser-canvas-host-blocked",
            probeResult.Rendered ? "browser-canvas-frame-ready" : "browser-canvas-frame-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser canvas frame ready for profile '{result.ProfileId}' with {result.NativeBrowserCanvasFrameStages.Length} stage(s) using {result.BrowserFrameContextType}."
            : $"Browser-native browser canvas frame blocked for profile '{result.ProfileId}' with {result.NativeBrowserCanvasFrameStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserCanvasFrameProbeResult
{
    public bool Rendered { get; set; }
    public string ContextType { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientNativeBrowserCanvasFrameResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserCanvasFrameVersion { get; set; } = string.Empty;
    public string[] NativeBrowserCanvasFrameStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserCanvasHostReady { get; set; }
    public string NativeBrowserCanvasHostVersion { get; set; } = string.Empty;
    public string NativeBrowserCanvasHostSummary { get; set; } = string.Empty;
    public string BrowserCanvasContainerId { get; set; } = string.Empty;
    public string BrowserCanvasElementId { get; set; } = string.Empty;
    public int BrowserCanvasWidth { get; set; }
    public int BrowserCanvasHeight { get; set; }
    public string BrowserCanvasContextType { get; set; } = string.Empty;
    public bool BrowserCanvasContextAcquired { get; set; }
    public bool BrowserFrameRendered { get; set; }
    public string BrowserFrameContextType { get; set; } = string.Empty;
    public string BrowserFrameText { get; set; } = string.Empty;
    public string BrowserFrameError { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
