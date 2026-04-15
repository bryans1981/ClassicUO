using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserWebSocketRuntimeSessionController
{
    ValueTask<BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserWebSocketRuntimeSessionControllerService : IBrowserClientNativeBrowserWebSocketRuntimeSessionController
{
    private readonly IBrowserClientNativeBrowserWebSocketRuntimeExecutionController _nativeBrowserWebSocketRuntimeExecutionController;

    public BrowserClientNativeBrowserWebSocketRuntimeSessionControllerService(
        IBrowserClientNativeBrowserWebSocketRuntimeExecutionController nativeBrowserWebSocketRuntimeExecutionController
    )
    {
        _nativeBrowserWebSocketRuntimeExecutionController = nativeBrowserWebSocketRuntimeExecutionController;
    }

    public async ValueTask<BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult nativeBrowserWebSocketRuntimeExecutionController = await _nativeBrowserWebSocketRuntimeExecutionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult result = new()
        {
            ProfileId = nativeBrowserWebSocketRuntimeExecutionController.ProfileId,
            AssetRootPath = nativeBrowserWebSocketRuntimeExecutionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserWebSocketRuntimeExecutionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserWebSocketRuntimeExecutionController.CacheRootPath,
            ConfigRootPath = nativeBrowserWebSocketRuntimeExecutionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserWebSocketRuntimeExecutionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserWebSocketRuntimeExecutionController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserWebSocketRuntimeExecutionController.ReadyAssetCount,
            CacheHits = nativeBrowserWebSocketRuntimeExecutionController.CacheHits,
            NativeBrowserWebSocketRuntimeExecutionReady = nativeBrowserWebSocketRuntimeExecutionController.IsReady,
            NativeBrowserWebSocketRuntimeExecutionVersion = nativeBrowserWebSocketRuntimeExecutionController.NativeBrowserWebSocketRuntimeExecutionVersion,
            NativeBrowserWebSocketRuntimeExecutionSummary = nativeBrowserWebSocketRuntimeExecutionController.Summary,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserWebSocketRuntimeExecutionController.WebSocketRuntimeExecutionMode,
            TransportMode = nativeBrowserWebSocketRuntimeExecutionController.TransportMode,
            LaunchMode = nativeBrowserWebSocketRuntimeExecutionController.LaunchMode,
            ContractVersion = nativeBrowserWebSocketRuntimeExecutionController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserWebSocketRuntimeExecutionController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserWebSocketRuntimeExecutionController.IsReady ? "native-browser-websocket-runtime-execution-ready" : "native-browser-websocket-runtime-execution-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserWebSocketRuntimeSessionVersion = "browser-native-browser-websocket-runtime-session-controller-v1";
        result.NativeBrowserWebSocketRuntimeSessionStages =
        [
            "bind-native-browser-websocket-runtime-execution-controller",
            "capture-browser-runtime-websocket-handshake",
            "publish-browser-native-browser-websocket-runtime-session"
        ];
        result.WebSocketRuntimeSessionId = result.IsReady
            ? $"browser-runtime-session-{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}"
            : string.Empty;
        result.WebSocketRuntimeSessionState = result.IsReady ? "connected" : "blocked";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser websocket runtime session controller ready for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketRuntimeSessionStages.Length} stage(s)."
            : $"Browser-native browser websocket runtime session controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketRuntimeSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserWebSocketRuntimeSessionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserWebSocketRuntimeSessionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserWebSocketRuntimeSessionStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserWebSocketRuntimeExecutionReady { get; set; }
    public string NativeBrowserWebSocketRuntimeExecutionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketRuntimeExecutionSummary { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionEndpoint { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionProtocol { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionMode { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string WebSocketRuntimeSessionId { get; set; } = string.Empty;
    public string WebSocketRuntimeSessionState { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
