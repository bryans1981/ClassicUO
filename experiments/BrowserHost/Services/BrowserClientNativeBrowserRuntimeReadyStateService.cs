using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeReadyState
{
    ValueTask<BrowserClientNativeBrowserRuntimeReadyStateResult> BuildAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeReadyStateService : IBrowserClientNativeBrowserRuntimeReadyState
{
    private readonly IBrowserClientNativeBrowserRuntime _nativeBrowserRuntime;

    public BrowserClientNativeBrowserRuntimeReadyStateService(IBrowserClientNativeBrowserRuntime nativeBrowserRuntime)
    {
        _nativeBrowserRuntime = nativeBrowserRuntime;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeReadyStateResult> BuildAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeResult nativeBrowserRuntime = await _nativeBrowserRuntime.PrepareAsync(request, profileId);

        BrowserClientNativeBrowserRuntimeReadyStateResult result = new()
        {
            ProfileId = nativeBrowserRuntime.ProfileId,
            AssetRootPath = nativeBrowserRuntime.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntime.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntime.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntime.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntime.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntime.StartupProfilePath,
            ReadyAssetCount = nativeBrowserRuntime.ReadyAssetCount,
            CacheHits = nativeBrowserRuntime.CacheHits,
            NativeBrowserRuntimeReady = nativeBrowserRuntime.IsReady,
            NativeBrowserRuntimeVersion = nativeBrowserRuntime.NativeBrowserRuntimeVersion,
            NativeBrowserRuntimeSummary = nativeBrowserRuntime.Summary,
            BrowserRenderReady = nativeBrowserRuntime.BrowserRenderReady,
            BrowserRenderReadyVersion = nativeBrowserRuntime.BrowserRenderReadyVersion,
            BrowserRenderSummary = nativeBrowserRuntime.BrowserRenderSummary,
            BrowserInputReady = nativeBrowserRuntime.BrowserInputReady,
            BrowserInputReadyVersion = nativeBrowserRuntime.BrowserInputReadyVersion,
            BrowserInputSummary = nativeBrowserRuntime.BrowserInputSummary,
            LaunchReady = nativeBrowserRuntime.LaunchReady,
            LaunchControllerVersion = nativeBrowserRuntime.LaunchControllerVersion,
            LaunchSummary = nativeBrowserRuntime.LaunchSummary,
            RequiredAssets = nativeBrowserRuntime.RequiredAssets
        };

        result.ReadinessChecks =
        [
            nativeBrowserRuntime.IsReady ? "native-browser-runtime-ready" : "native-browser-runtime-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeReadyStateVersion = "browser-native-browser-runtime-ready-state-v1";
        result.NativeBrowserRuntimeReadyStateStages =
        [
            "bind-native-browser-runtime",
            "publish-browser-native-browser-runtime-ready-state"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime ready state ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeReadyStateStages.Length} stage(s)."
            : $"Browser-native browser runtime ready state blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeReadyStateStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeReadyStateResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeReadyStateVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeReadyStateStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool NativeBrowserRuntimeReady { get; set; }
    public string NativeBrowserRuntimeVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeSummary { get; set; } = string.Empty;
    public bool BrowserRenderReady { get; set; }
    public string BrowserRenderReadyVersion { get; set; } = string.Empty;
    public string BrowserRenderSummary { get; set; } = string.Empty;
    public bool BrowserInputReady { get; set; }
    public string BrowserInputReadyVersion { get; set; } = string.Empty;
    public string BrowserInputSummary { get; set; } = string.Empty;
    public bool LaunchReady { get; set; }
    public string LaunchControllerVersion { get; set; } = string.Empty;
    public string LaunchSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
