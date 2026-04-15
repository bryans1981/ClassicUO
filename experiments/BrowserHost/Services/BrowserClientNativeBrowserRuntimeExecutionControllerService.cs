using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeExecutionController
{
    ValueTask<BrowserClientNativeBrowserRuntimeExecutionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeExecutionControllerService : IBrowserClientNativeBrowserRuntimeExecutionController
{
    private readonly IBrowserClientNativeBrowserTransportController _nativeBrowserTransportController;

    public BrowserClientNativeBrowserRuntimeExecutionControllerService(
        IBrowserClientNativeBrowserTransportController nativeBrowserTransportController
    )
    {
        _nativeBrowserTransportController = nativeBrowserTransportController;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeExecutionControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserTransportControllerResult nativeBrowserTransportController = await _nativeBrowserTransportController.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeExecutionControllerResult result = new()
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
            TransportMode = nativeBrowserTransportController.TransportMode,
            TransportEndpoint = nativeBrowserTransportController.TransportEndpoint,
            LaunchMode = nativeBrowserTransportController.LaunchMode,
            ContractVersion = nativeBrowserTransportController.ContractVersion
        };

        result.RequiredAssets = nativeBrowserTransportController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserTransportController.IsReady ? "native-browser-transport-ready" : "native-browser-transport-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeExecutionVersion = "browser-native-browser-runtime-execution-controller-v1";
        result.NativeBrowserRuntimeExecutionStages =
        [
            "bind-native-browser-transport-controller",
            "publish-browser-native-browser-runtime-execution"
        ];
        result.RuntimeExecutionMode = nativeBrowserTransportController.IsReady ? "websocket-runtime-execution" : "browser-runtime-execution-blocked";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime execution controller ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeExecutionStages.Length} stage(s)."
            : $"Browser-native browser runtime execution controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeExecutionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeExecutionControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeExecutionVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeExecutionStages { get; set; } = Array.Empty<string>();
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
    public string TransportMode { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string RuntimeExecutionMode { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
