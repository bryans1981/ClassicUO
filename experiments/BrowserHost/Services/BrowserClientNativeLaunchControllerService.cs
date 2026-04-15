using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeLaunchController
{
    ValueTask<BrowserClientNativeLaunchControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeLaunchControllerService : IBrowserClientNativeLaunchController
{
    private readonly IBrowserClientEntrypoint _browserClientEntrypoint;
    private readonly IBrowserClientNativeRuntimeShell _nativeRuntimeShell;

    public BrowserClientNativeLaunchControllerService(
        IBrowserClientEntrypoint browserClientEntrypoint,
        IBrowserClientNativeRuntimeShell nativeRuntimeShell
    )
    {
        _browserClientEntrypoint = browserClientEntrypoint;
        _nativeRuntimeShell = nativeRuntimeShell;
    }

    public async ValueTask<BrowserClientNativeLaunchControllerResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientLaunchPlan launchPlan = await _browserClientEntrypoint.PrepareLaunchAsync(request, profileId);
        BrowserClientNativeRuntimeShellResult nativeRuntimeShell = await _nativeRuntimeShell.PrepareAsync(profileId);

        BrowserClientNativeLaunchControllerResult result = new()
        {
            ProfileId = launchPlan.ProfileId,
            LaunchMode = launchPlan.IsReady ? "browser-native-launch" : "browser-native-launch-blocked",
            AssetRootPath = launchPlan.AssetRootPath,
            ProfilesRootPath = launchPlan.ProfilesRootPath,
            CacheRootPath = launchPlan.CacheRootPath,
            ConfigRootPath = launchPlan.ConfigRootPath,
            SettingsFilePath = launchPlan.SettingsFilePath,
            StartupProfilePath = launchPlan.StartupProfilePath,
            ReadyAssetCount = launchPlan.ReadyAssetCount,
            CacheHits = launchPlan.CacheHits,
            LaunchPlanReady = launchPlan.IsReady,
            LaunchPlanSummary = launchPlan.Summary,
            NativeRuntimeShellReady = nativeRuntimeShell.IsReady,
            NativeRuntimeShellVersion = nativeRuntimeShell.BrowserNativeRuntimeShellVersion,
            NativeRuntimeShellSummary = nativeRuntimeShell.BrowserNativeRuntimeShellSummary
        };

        result.RequiredAssets = launchPlan.Assets.Select(static asset => asset.Path).ToArray();
        result.ReadinessChecks =
        [
            launchPlan.IsReady ? "launch-plan-ready" : "launch-plan-blocked",
            nativeRuntimeShell.IsReady ? "native-shell-ready" : "native-shell-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeLaunchControllerVersion = "browser-native-launch-controller-v1";
        result.NativeLaunchControllerStages =
        [
            "bind-launch-plan",
            "bind-native-runtime-shell",
            "publish-native-launch-controller"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser native launch controller ready for profile '{result.ProfileId}' with {result.NativeLaunchControllerStages.Length} stage(s)."
            : $"Browser native launch controller blocked for profile '{result.ProfileId}' with {result.NativeLaunchControllerStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeLaunchControllerResult
{
    public bool IsReady { get; set; }
    public string NativeLaunchControllerVersion { get; set; } = string.Empty;
    public string[] NativeLaunchControllerStages { get; set; } = Array.Empty<string>();
    public string LaunchMode { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool LaunchPlanReady { get; set; }
    public string LaunchPlanSummary { get; set; } = string.Empty;
    public bool NativeRuntimeShellReady { get; set; }
    public string NativeRuntimeShellVersion { get; set; } = string.Empty;
    public string NativeRuntimeShellSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
