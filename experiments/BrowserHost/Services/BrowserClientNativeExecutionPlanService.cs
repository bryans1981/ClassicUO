using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeExecutionPlan
{
    ValueTask<BrowserClientNativeExecutionPlanResult> PrepareAsync(string profileId = "default");
}

public sealed class BrowserClientNativeExecutionPlanService : IBrowserClientNativeExecutionPlan
{
    private readonly IBrowserClientBootstrapPackageConsumer _bootstrapPackageConsumer;
    private readonly IBrowserClientLaunchSessionReader _launchSessionReader;

    public BrowserClientNativeExecutionPlanService(
        IBrowserClientBootstrapPackageConsumer bootstrapPackageConsumer,
        IBrowserClientLaunchSessionReader launchSessionReader
    )
    {
        _bootstrapPackageConsumer = bootstrapPackageConsumer;
        _launchSessionReader = launchSessionReader;
    }

    public async ValueTask<BrowserClientNativeExecutionPlanResult> PrepareAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientBootstrapPackageConsumerResult packageConsumer = await _bootstrapPackageConsumer.ConsumeAsync(profileId);
        BrowserClientLaunchSessionRead launchSessionRead = await _launchSessionReader.ReadLaunchSessionAsync(profileId);

        BrowserClientNativeExecutionPlanResult result = new()
        {
            ProfileId = packageConsumer.ProfileId,
            PackageVersion = packageConsumer.PackageVersion,
            BootstrapPackagePath = packageConsumer.ArtifactPath,
            LaunchSessionPath = launchSessionRead.SessionPath,
            PackageReady = packageConsumer.IsReady,
            LaunchSessionReady = launchSessionRead.IsReady,
            RequiredAssetCount = packageConsumer.RequiredAssetCount,
            ReadyAssetCount = packageConsumer.RequiredAssetCount,
            TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds
        };

        result.ExecutionMode = "browser-native-wasm-webgl";
        result.RenderingMode = "webgl-canvas";
        result.InputMode = "browser-pointer-keyboard";
        result.TransportMode = "websocket-session";
        result.AssetMode = "browser-filesystem-seam";
        result.EnvironmentMode = "browser-hosted";
        result.TargetRuntime = "classicuo-browser-client";
        result.RequiredAssets = packageConsumer.Assets.Select(static asset => asset.Path).ToArray();
        result.ReadinessChecks =
        [
            packageConsumer.IsReady ? "bootstrap-package-consumed" : "bootstrap-package-blocked",
            launchSessionRead.IsReady ? "launch-session-ready" : "launch-session-blocked",
            packageConsumer.RequiredAssetCount == 3 ? "required-assets-ready" : "required-assets-missing",
            launchSessionRead.Exists ? "launch-session-present" : "launch-session-missing"
        ];
        result.IsReady = packageConsumer.IsReady && launchSessionRead.IsReady;
        result.BrowserNativeRuntimeShellVersion = "browser-native-runtime-shell-v1";
        result.BrowserNativeRuntimeShellStages =
        [
            "bind-browser-render-surface",
            "bind-browser-input-surface",
            "bind-browser-transport-session",
            "publish-browser-native-runtime-shell"
        ];
        result.BrowserNativeRuntimeShellReady = result.IsReady;
        result.BrowserNativeRuntimeShellSummary = result.BrowserNativeRuntimeShellReady
            ? $"Browser native runtime shell ready for profile '{result.ProfileId}' with {result.BrowserNativeRuntimeShellStages.Length} stage(s)."
            : $"Browser native runtime shell blocked for profile '{result.ProfileId}' with {result.BrowserNativeRuntimeShellStages.Length} stage(s).";
        result.Summary = result.IsReady
            ? $"Browser-native execution plan ready for profile '{result.ProfileId}' with {result.ReadinessChecks.Length} check(s)."
            : $"Browser-native execution plan blocked for profile '{result.ProfileId}' with {result.ReadinessChecks.Length} check(s).";
        result.PackageSummary = packageConsumer.Summary;
        result.LaunchSessionSummary = launchSessionRead.Summary;

        return result;
    }
}

public sealed class BrowserClientNativeExecutionPlanResult
{
    public bool IsReady { get; set; }
    public string ProfileId { get; set; } = "default";
    public string PackageVersion { get; set; } = string.Empty;
    public string BootstrapPackagePath { get; set; } = string.Empty;
    public string LaunchSessionPath { get; set; } = string.Empty;
    public bool PackageReady { get; set; }
    public bool LaunchSessionReady { get; set; }
    public string ExecutionMode { get; set; } = string.Empty;
    public string RenderingMode { get; set; } = string.Empty;
    public string InputMode { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string AssetMode { get; set; } = string.Empty;
    public string EnvironmentMode { get; set; } = string.Empty;
    public string TargetRuntime { get; set; } = string.Empty;
    public bool BrowserNativeRuntimeShellReady { get; set; }
    public string BrowserNativeRuntimeShellVersion { get; set; } = string.Empty;
    public string[] BrowserNativeRuntimeShellStages { get; set; } = Array.Empty<string>();
    public string BrowserNativeRuntimeShellSummary { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public int RequiredAssetCount { get; set; }
    public int ReadyAssetCount { get; set; }
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public string PackageSummary { get; set; } = string.Empty;
    public string LaunchSessionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
