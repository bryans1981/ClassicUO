using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserNetworkController
{
    ValueTask<BrowserClientNativeBrowserNetworkControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserNetworkControllerService : IBrowserClientNativeBrowserNetworkController
{
    private readonly IBrowserClientNativeBrowserInputController _nativeBrowserInputController;
    private readonly IBrowserClientRuntimeLaunchContract _runtimeLaunchContract;

    public BrowserClientNativeBrowserNetworkControllerService(
        IBrowserClientNativeBrowserInputController nativeBrowserInputController,
        IBrowserClientRuntimeLaunchContract runtimeLaunchContract
    )
    {
        _nativeBrowserInputController = nativeBrowserInputController;
        _runtimeLaunchContract = runtimeLaunchContract;
    }

    public async ValueTask<BrowserClientNativeBrowserNetworkControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserInputControllerResult nativeBrowserInputController = await _nativeBrowserInputController.PrepareAsync(request, profileId);
        BrowserClientRuntimeLaunchContractResult runtimeLaunchContract = await _runtimeLaunchContract.BuildAsync(profileId);

        BrowserClientNativeBrowserNetworkControllerResult result = new()
        {
            ProfileId = nativeBrowserInputController.ProfileId,
            AssetRootPath = nativeBrowserInputController.AssetRootPath,
            ProfilesRootPath = nativeBrowserInputController.ProfilesRootPath,
            CacheRootPath = nativeBrowserInputController.CacheRootPath,
            ConfigRootPath = nativeBrowserInputController.ConfigRootPath,
            SettingsFilePath = nativeBrowserInputController.SettingsFilePath,
            StartupProfilePath = nativeBrowserInputController.StartupProfilePath,
            ReadyAssetCount = nativeBrowserInputController.ReadyAssetCount,
            CacheHits = nativeBrowserInputController.CacheHits,
            NativeBrowserInputReady = nativeBrowserInputController.IsReady,
            NativeBrowserInputVersion = nativeBrowserInputController.NativeBrowserInputVersion,
            NativeBrowserInputSummary = nativeBrowserInputController.Summary,
            RuntimeLaunchContractReady = runtimeLaunchContract.IsReady,
            RuntimeLaunchContractVersion = runtimeLaunchContract.ContractVersion,
            RuntimeLaunchContractSummary = runtimeLaunchContract.Summary,
            TransportMode = runtimeLaunchContract.IsReady ? "websocket-session" : "browser-network-blocked",
            LaunchMode = runtimeLaunchContract.LaunchMode,
            ContractVersion = runtimeLaunchContract.ContractVersion
        };

        result.RequiredAssets = nativeBrowserInputController.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeBrowserInputController.IsReady ? "native-browser-input-ready" : "native-browser-input-blocked",
            runtimeLaunchContract.IsReady ? "runtime-launch-contract-ready" : "runtime-launch-contract-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserNetworkVersion = "browser-native-browser-network-controller-v1";
        result.NativeBrowserNetworkStages =
        [
            "bind-native-browser-input-controller",
            "bind-runtime-launch-contract",
            "publish-browser-native-browser-network"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser network controller ready for profile '{result.ProfileId}' with {result.NativeBrowserNetworkStages.Length} stage(s)."
            : $"Browser-native browser network controller blocked for profile '{result.ProfileId}' with {result.NativeBrowserNetworkStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserNetworkControllerResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserNetworkVersion { get; set; } = string.Empty;
    public string[] NativeBrowserNetworkStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserInputReady { get; set; }
    public string NativeBrowserInputVersion { get; set; } = string.Empty;
    public string NativeBrowserInputSummary { get; set; } = string.Empty;
    public bool RuntimeLaunchContractReady { get; set; }
    public string RuntimeLaunchContractVersion { get; set; } = string.Empty;
    public string RuntimeLaunchContractSummary { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
