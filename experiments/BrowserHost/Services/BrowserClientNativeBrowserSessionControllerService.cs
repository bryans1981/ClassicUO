using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserSessionController
{
    ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserSessionControllerService : IBrowserClientNativeBrowserSessionController
{
    private readonly IBrowserClientNativeBrowserRuntimeSessionController _nativeBrowserRuntimeSessionController;

    public BrowserClientNativeBrowserSessionControllerService(
        IBrowserClientNativeBrowserRuntimeSessionController nativeBrowserRuntimeSessionController
    )
    {
        _nativeBrowserRuntimeSessionController = nativeBrowserRuntimeSessionController;
    }

    public async ValueTask<BrowserClientNativeBrowserSessionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeSessionControllerResult nativeBrowserRuntimeSessionController = await _nativeBrowserRuntimeSessionController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserSessionControllerResult result = new()
        {
            ProfileId = nativeBrowserRuntimeSessionController.ProfileId,
            AssetRootPath = nativeBrowserRuntimeSessionController.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntimeSessionController.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntimeSessionController.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntimeSessionController.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntimeSessionController.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntimeSessionController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserRuntimeSessionController.ReadyAssetCount,
            CacheHits = nativeBrowserRuntimeSessionController.CacheHits,
            NativeBrowserRuntimeExecutionReady = nativeBrowserRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionReady,
            NativeBrowserRuntimeExecutionVersion = nativeBrowserRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionVersion,
            NativeBrowserRuntimeExecutionSummary = nativeBrowserRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionSummary,
            NativeBrowserWebSocketSessionReady = nativeBrowserRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionReady,
            NativeBrowserWebSocketSessionVersion = nativeBrowserRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionVersion,
            NativeBrowserWebSocketSessionSummary = nativeBrowserRuntimeSessionController.NativeBrowserWebSocketRuntimeSessionSummary,
            WebSocketSessionEndpoint = nativeBrowserRuntimeSessionController.WebSocketRuntimeExecutionEndpoint,
            WebSocketSessionScheme = nativeBrowserRuntimeSessionController.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserRuntimeSessionController.WebSocketRuntimeExecutionProtocol,
            BrowserSessionStabilityReady = nativeBrowserRuntimeSessionController.BrowserSessionStabilityReady,
            BrowserSessionStabilityReadyStateVersion = nativeBrowserRuntimeSessionController.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = nativeBrowserRuntimeSessionController.BrowserSessionStabilitySummary,
            TransportMode = nativeBrowserRuntimeSessionController.TransportMode,
            TransportEndpoint = nativeBrowserRuntimeSessionController.TransportEndpoint,
            RuntimeExecutionMode = nativeBrowserRuntimeSessionController.RuntimeExecutionMode,
            LaunchMode = nativeBrowserRuntimeSessionController.LaunchMode,
            ContractVersion = nativeBrowserRuntimeSessionController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserRuntimeSessionController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserRuntimeSessionController.IsReady ? "native-browser-runtime-session-ready" : "native-browser-runtime-session-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserSessionVersion = "browser-native-browser-session-controller-v5";
        result.NativeBrowserSessionStages =
        [
            "bind-native-browser-runtime-session-controller",
            "publish-browser-native-browser-session"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser session controller ready for profile '{result.ProfileId}' with {result.NativeBrowserSessionStages.Length} stage(s)."
            : $"Browser-native browser session controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserSessionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserSessionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserSessionStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserRuntimeExecutionReady { get; set; }
    public string NativeBrowserRuntimeExecutionVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeExecutionSummary { get; set; } = string.Empty;
    public bool NativeBrowserWebSocketSessionReady { get; set; }
    public string NativeBrowserWebSocketSessionVersion { get; set; } = string.Empty;
    public string NativeBrowserWebSocketSessionSummary { get; set; } = string.Empty;
    public string WebSocketSessionEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
    public string WebSocketSessionProtocol { get; set; } = string.Empty;
    public bool BrowserSessionStabilityReady { get; set; }
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySummary { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string RuntimeExecutionMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
