using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserWebSocketSessionController
{
    ValueTask<BrowserClientNativeBrowserWebSocketSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserWebSocketSessionControllerService : IBrowserClientNativeBrowserWebSocketSessionController
{
    private readonly IBrowserClientNativeBrowserTransportController _nativeBrowserTransportController;
    private readonly IBrowserClientNativeBrowserRuntimeExecutionController _nativeBrowserRuntimeExecutionController;

    public BrowserClientNativeBrowserWebSocketSessionControllerService(
        IBrowserClientNativeBrowserTransportController nativeBrowserTransportController,
        IBrowserClientNativeBrowserRuntimeExecutionController nativeBrowserRuntimeExecutionController
    )
    {
        _nativeBrowserTransportController = nativeBrowserTransportController;
        _nativeBrowserRuntimeExecutionController = nativeBrowserRuntimeExecutionController;
    }

    public async ValueTask<BrowserClientNativeBrowserWebSocketSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserTransportControllerResult nativeBrowserTransportController = await _nativeBrowserTransportController.PrepareAsync(request, profileId);
        BrowserClientNativeBrowserRuntimeExecutionControllerResult nativeBrowserRuntimeExecutionController = await _nativeBrowserRuntimeExecutionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserWebSocketSessionControllerResult result = new()
        {
            ProfileId = nativeBrowserTransportController.ProfileId,
            AssetRootPath = nativeBrowserTransportController.AssetRootPath,
            ProfilesRootPath = nativeBrowserTransportController.ProfilesRootPath,
            CacheRootPath = nativeBrowserTransportController.CacheRootPath,
            ConfigRootPath = nativeBrowserTransportController.ConfigRootPath,
            SettingsFilePath = nativeBrowserTransportController.SettingsFilePath,
            StartupProfilePath = nativeBrowserTransportController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserTransportController.ReadyAssetCount,
            CacheHits = nativeBrowserTransportController.CacheHits,
            NativeBrowserTransportReady = nativeBrowserTransportController.IsReady,
            NativeBrowserTransportVersion = nativeBrowserTransportController.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserTransportController.Summary,
            NativeBrowserRuntimeExecutionReady = nativeBrowserRuntimeExecutionController.IsReady,
            NativeBrowserRuntimeExecutionVersion = nativeBrowserRuntimeExecutionController.NativeBrowserRuntimeExecutionVersion,
            NativeBrowserRuntimeExecutionSummary = nativeBrowserRuntimeExecutionController.Summary,
            TransportMode = nativeBrowserTransportController.TransportMode,
            TransportEndpoint = nativeBrowserTransportController.TransportEndpoint,
            RuntimeExecutionMode = nativeBrowserRuntimeExecutionController.RuntimeExecutionMode,
            LaunchMode = nativeBrowserTransportController.LaunchMode,
            ContractVersion = nativeBrowserTransportController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserTransportController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserTransportController.IsReady ? "native-browser-transport-ready" : "native-browser-transport-blocked",
            nativeBrowserRuntimeExecutionController.IsReady ? "native-browser-runtime-execution-ready" : "native-browser-runtime-execution-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));

        if (result.IsReady && Uri.TryCreate(nativeBrowserTransportController.TransportEndpoint, UriKind.Absolute, out Uri? websocketEndpoint) && websocketEndpoint is not null && (websocketEndpoint.Scheme == Uri.UriSchemeWs || websocketEndpoint.Scheme == Uri.UriSchemeWss))
        {
            result.WebSocketSessionEndpoint = websocketEndpoint.ToString();
            result.WebSocketSessionScheme = websocketEndpoint.Scheme;
            result.IsReady = true;
        }
        else
        {
            result.IsReady = false;
        }

        result.NativeBrowserWebSocketSessionVersion = "browser-native-browser-websocket-session-controller-v1";
        result.NativeBrowserWebSocketSessionStages =
        [
            "bind-native-browser-transport-controller",
            "bind-native-browser-runtime-execution-controller",
            "validate-browser-runtime-websocket-endpoint",
            "publish-browser-native-browser-websocket-session"
        ];
        result.WebSocketSessionProtocol = "browser-runtime-v1";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser websocket session controller ready for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketSessionStages.Length} stage(s)."
            : $"Browser-native browser websocket session controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserWebSocketSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserWebSocketSessionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserWebSocketSessionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserWebSocketSessionStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserTransportReady { get; set; }
    public string NativeBrowserTransportVersion { get; set; } = string.Empty;
    public string NativeBrowserTransportSummary { get; set; } = string.Empty;
    public bool NativeBrowserRuntimeExecutionReady { get; set; }
    public string NativeBrowserRuntimeExecutionVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeExecutionSummary { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
    public string WebSocketSessionProtocol { get; set; } = string.Empty;
    public string RuntimeExecutionMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
