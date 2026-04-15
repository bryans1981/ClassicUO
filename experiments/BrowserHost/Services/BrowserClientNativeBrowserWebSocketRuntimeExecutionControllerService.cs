using ClassicUO.Utility;
using Microsoft.JSInterop;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserWebSocketRuntimeExecutionController
{
    ValueTask<BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerService : IBrowserClientNativeBrowserWebSocketRuntimeExecutionController
{
    private readonly IBrowserClientNativeBrowserWebSocketSessionController _nativeBrowserWebSocketSessionController;
    private readonly IJSRuntime _jsRuntime;

    public BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerService(
        IBrowserClientNativeBrowserWebSocketSessionController nativeBrowserWebSocketSessionController,
        IJSRuntime jsRuntime
    )
    {
        _nativeBrowserWebSocketSessionController = nativeBrowserWebSocketSessionController;
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserWebSocketSessionControllerResult nativeBrowserWebSocketSessionController = await _nativeBrowserWebSocketSessionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult result = new()
        {
            ProfileId = nativeBrowserWebSocketSessionController.ProfileId,
            AssetRootPath = nativeBrowserWebSocketSessionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserWebSocketSessionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserWebSocketSessionController.CacheRootPath,
            ConfigRootPath = nativeBrowserWebSocketSessionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserWebSocketSessionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserWebSocketSessionController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserWebSocketSessionController.ReadyAssetCount,
            CacheHits = nativeBrowserWebSocketSessionController.CacheHits,
            NativeBrowserWebSocketSessionReady = nativeBrowserWebSocketSessionController.IsReady,
            NativeBrowserWebSocketSessionVersion = nativeBrowserWebSocketSessionController.NativeBrowserWebSocketSessionVersion,
            NativeBrowserWebSocketSessionSummary = nativeBrowserWebSocketSessionController.Summary,
            WebSocketSessionEndpoint = nativeBrowserWebSocketSessionController.WebSocketSessionEndpoint,
            WebSocketSessionScheme = nativeBrowserWebSocketSessionController.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserWebSocketSessionController.WebSocketSessionProtocol,
            TransportMode = nativeBrowserWebSocketSessionController.TransportMode,
            LaunchMode = nativeBrowserWebSocketSessionController.LaunchMode,
            ContractVersion = nativeBrowserWebSocketSessionController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserWebSocketSessionController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserWebSocketSessionController.IsReady ? "native-browser-websocket-session-ready" : "native-browser-websocket-session-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));

        if (!result.IsReady)
        {
            result.NativeBrowserWebSocketRuntimeExecutionVersion = "browser-native-browser-websocket-runtime-execution-controller-v1";
            result.NativeBrowserWebSocketRuntimeExecutionStages =
            [
                "bind-native-browser-websocket-session-controller",
                "browser-websocket-runtime-blocked"
            ];
            result.WebSocketRuntimeExecutionMode = "browser-runtime-blocked";
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser-native browser websocket runtime execution controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketRuntimeExecutionStages.Length} stage(s).";
            return result;
        }

        BrowserClientNativeBrowserWebSocketProbeResult probeResult = await _jsRuntime.InvokeAsync<BrowserClientNativeBrowserWebSocketProbeResult>(
            "classicuoProbe.connectBrowserRuntimeWebSocket",
            nativeBrowserWebSocketSessionController.WebSocketSessionEndpoint,
            nativeBrowserWebSocketSessionController.WebSocketSessionProtocol
        );

        result.WebSocketConnected = probeResult.Opened && probeResult.MessageReceived && probeResult.Closed && string.IsNullOrWhiteSpace(probeResult.Error);
        result.WebSocketMessageReceived = probeResult.MessageReceived;
        result.WebSocketMessage = probeResult.Message;
        result.WebSocketCloseCode = probeResult.CloseCode;
        result.WebSocketError = probeResult.Error;
        result.WebSocketRuntimeExecutionEndpoint = probeResult.Endpoint;
        result.WebSocketRuntimeExecutionProtocol = probeResult.Protocol;
        result.NativeBrowserWebSocketRuntimeExecutionVersion = "browser-native-browser-websocket-runtime-execution-controller-v1";
        result.NativeBrowserWebSocketRuntimeExecutionStages =
        [
            "bind-native-browser-websocket-session-controller",
            "connect-browser-runtime-websocket",
            "receive-browser-runtime-handshake",
            "publish-browser-websocket-runtime-execution"
        ];
        result.WebSocketRuntimeExecutionMode = result.WebSocketConnected ? "browser-websocket-runtime-execution" : "browser-runtime-websocket-blocked";
        result.IsReady = result.WebSocketConnected;
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser websocket runtime execution controller ready for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketRuntimeExecutionStages.Length} stage(s)."
            : $"Browser-native browser websocket runtime execution controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketRuntimeExecutionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserWebSocketProbeResult
{
    public string Endpoint { get; set; } = string.Empty;
    public string Protocol { get; set; } = string.Empty;
    public bool Opened { get; set; }
    public bool MessageReceived { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Closed { get; set; }
    public int CloseCode { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientNativeBrowserWebSocketRuntimeExecutionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserWebSocketRuntimeExecutionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserWebSocketRuntimeExecutionStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserWebSocketSessionReady { get; set; }
    public string NativeBrowserWebSocketSessionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketSessionSummary { get; set; } = string.Empty;
    public string WebSocketSessionEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
    public string WebSocketSessionProtocol { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public bool WebSocketConnected { get; set; }
    public bool WebSocketMessageReceived { get; set; }
    public string WebSocketMessage { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionEndpoint { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionProtocol { get; set; } = string.Empty;
    public int WebSocketCloseCode { get; set; }
    public string WebSocketError { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionMode { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
