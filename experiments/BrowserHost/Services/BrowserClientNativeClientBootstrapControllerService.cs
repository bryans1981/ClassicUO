using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeClientBootstrapController
{
    ValueTask<BrowserClientNativeClientBootstrapControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeClientBootstrapControllerService : IBrowserClientNativeClientBootstrapController
{
    private readonly IBrowserClientNativeLaunchController _nativeLaunchController;
    private readonly IBrowserClientRuntimeClientBootstrapController _runtimeClientBootstrapController;

    public BrowserClientNativeClientBootstrapControllerService(
        IBrowserClientNativeLaunchController nativeLaunchController,
        IBrowserClientRuntimeClientBootstrapController runtimeClientBootstrapController
    )
    {
        _nativeLaunchController = nativeLaunchController;
        _runtimeClientBootstrapController = runtimeClientBootstrapController;
    }

    public async ValueTask<BrowserClientNativeClientBootstrapControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeLaunchControllerResult nativeLaunchController = await _nativeLaunchController.PrepareAsync(request, profileId);
        BrowserClientRuntimeClientBootstrapControllerResult runtimeClientBootstrapController = await _runtimeClientBootstrapController.ControlAsync(profileId);

        BrowserClientNativeClientBootstrapControllerResult result = new()
        {
            ProfileId = nativeLaunchController.ProfileId,
            AssetRootPath = nativeLaunchController.AssetRootPath,
            ProfilesRootPath = nativeLaunchController.ProfilesRootPath,
            CacheRootPath = nativeLaunchController.CacheRootPath,
            ConfigRootPath = nativeLaunchController.ConfigRootPath,
            SettingsFilePath = nativeLaunchController.SettingsFilePath,
            StartupProfilePath = nativeLaunchController.StartupProfilePath,
            ReadyAssetCount = nativeLaunchController.ReadyAssetCount,
            CacheHits = nativeLaunchController.CacheHits,
            NativeLaunchControllerReady = nativeLaunchController.IsReady,
            NativeLaunchControllerVersion = nativeLaunchController.NativeLaunchControllerVersion,
            NativeLaunchControllerSummary = nativeLaunchController.Summary,
            RuntimeClientBootstrapReady = runtimeClientBootstrapController.IsReady,
            RuntimeClientBootstrapVersion = runtimeClientBootstrapController.ClientBootstrapVersion,
            RuntimeClientBootstrapSummary = runtimeClientBootstrapController.Summary
        };

        result.RequiredAssets = nativeLaunchController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeLaunchController.IsReady ? "native-launch-ready" : "native-launch-blocked",
            runtimeClientBootstrapController.IsReady ? "runtime-client-bootstrap-ready" : "runtime-client-bootstrap-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeClientBootstrapVersion = "browser-native-client-bootstrap-controller-v1";
        result.NativeClientBootstrapStages =
        [
            "bind-native-launch-controller",
            "bind-runtime-client-bootstrap",
            "publish-browser-native-client-bootstrap"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native client bootstrap controller ready for profile '{result.ProfileId}' with {result.NativeClientBootstrapStages.Length} stage(s)."
            : $"Browser-native client bootstrap controller blocked for profile '{result.ProfileId}' with {result.NativeClientBootstrapStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeClientBootstrapControllerResult
{
    public bool IsReady { get; set; }
    public string NativeClientBootstrapVersion { get; set; } = string.Empty;
    public string[] NativeClientBootstrapStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeLaunchControllerReady { get; set; }
    public string NativeLaunchControllerVersion { get; set; } = string.Empty;
    public string NativeLaunchControllerSummary { get; set; } = string.Empty;
    public bool RuntimeClientBootstrapReady { get; set; }
    public string RuntimeClientBootstrapVersion { get; set; } = string.Empty;
    public string RuntimeClientBootstrapSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
