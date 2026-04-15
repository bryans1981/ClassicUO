namespace BrowserHost.Services;

public interface IBrowserClientNativeRuntimeShell
{
    ValueTask<BrowserClientNativeRuntimeShellResult> PrepareAsync(string profileId = "default");
}

public sealed class BrowserClientNativeRuntimeShellService : IBrowserClientNativeRuntimeShell
{
    private readonly IBrowserClientNativeExecutionPlan _nativeExecutionPlan;
    private readonly IBrowserClientRuntimeBrowserRenderReadyState _browserRenderReadyState;
    private readonly IBrowserClientRuntimeBrowserInputReadyState _browserInputReadyState;
    private readonly IBrowserClientRuntimeLaunchController _runtimeLaunchController;

    public BrowserClientNativeRuntimeShellService(
        IBrowserClientNativeExecutionPlan nativeExecutionPlan,
        IBrowserClientRuntimeBrowserRenderReadyState browserRenderReadyState,
        IBrowserClientRuntimeBrowserInputReadyState browserInputReadyState,
        IBrowserClientRuntimeLaunchController runtimeLaunchController
    )
    {
        _nativeExecutionPlan = nativeExecutionPlan;
        _browserRenderReadyState = browserRenderReadyState;
        _browserInputReadyState = browserInputReadyState;
        _runtimeLaunchController = runtimeLaunchController;
    }

    public async ValueTask<BrowserClientNativeRuntimeShellResult> PrepareAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeExecutionPlanResult nativeExecutionPlan = await _nativeExecutionPlan.PrepareAsync(profileId);
        BrowserClientRuntimeBrowserRenderReadyStateResult browserRenderReadyState = await _browserRenderReadyState.BuildAsync(profileId);
        BrowserClientRuntimeBrowserInputReadyStateResult browserInputReadyState = await _browserInputReadyState.BuildAsync(profileId);
        BrowserClientRuntimeLaunchControllerResult runtimeLaunchController = await _runtimeLaunchController.ControlAsync(profileId);

        BrowserClientNativeRuntimeShellResult result = new()
        {
            ProfileId = nativeExecutionPlan.ProfileId,
            PackageVersion = nativeExecutionPlan.PackageVersion,
            BootstrapPackagePath = nativeExecutionPlan.BootstrapPackagePath,
            LaunchSessionPath = nativeExecutionPlan.LaunchSessionPath,
            ExecutionMode = nativeExecutionPlan.ExecutionMode,
            RenderingMode = nativeExecutionPlan.RenderingMode,
            InputMode = nativeExecutionPlan.InputMode,
            TransportMode = nativeExecutionPlan.TransportMode,
            AssetMode = nativeExecutionPlan.AssetMode,
            EnvironmentMode = nativeExecutionPlan.EnvironmentMode,
            TargetRuntime = nativeExecutionPlan.TargetRuntime,
            NativeExecutionPlanVersion = nativeExecutionPlan.BrowserNativeRuntimeShellVersion,
            NativeExecutionPlanReady = nativeExecutionPlan.IsReady,
            RenderReady = browserRenderReadyState.IsReady,
            InputReady = browserInputReadyState.IsReady,
            LaunchReady = runtimeLaunchController.IsReady,
            RenderSummary = browserRenderReadyState.Summary,
            InputSummary = browserInputReadyState.Summary,
            LaunchSummary = runtimeLaunchController.Summary
        };

        result.RequiredAssets = nativeExecutionPlan.RequiredAssets;
        result.ReadinessChecks =
        [
            nativeExecutionPlan.IsReady ? "native-execution-plan-ready" : "native-execution-plan-blocked",
            browserRenderReadyState.IsReady ? "browser-render-ready" : "browser-render-blocked",
            browserInputReadyState.IsReady ? "browser-input-ready" : "browser-input-blocked",
            runtimeLaunchController.IsReady ? "runtime-launch-ready" : "runtime-launch-blocked"
        ];
        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.BrowserNativeRuntimeShellVersion = "browser-native-runtime-shell-v2";
        result.BrowserNativeRuntimeShellStages =
        [
            "bind-browser-render-ready-state",
            "bind-browser-input-ready-state",
            "bind-runtime-launch-controller",
            "publish-browser-native-runtime-shell"
        ];
        result.BrowserNativeRuntimeShellReady = result.IsReady;
        result.BrowserNativeRuntimeShellSummary = result.IsReady
            ? $"Browser native runtime shell ready for profile '{result.ProfileId}' with {result.BrowserNativeRuntimeShellStages.Length} stage(s)."
            : $"Browser native runtime shell blocked for profile '{result.ProfileId}' with {result.BrowserNativeRuntimeShellStages.Length} stage(s).";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser native runtime shell ready for profile '{result.ProfileId}' with {result.ReadinessChecks.Length} check(s)."
            : $"Browser native runtime shell blocked for profile '{result.ProfileId}' with {result.ReadinessChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientNativeRuntimeShellResult
{
    public bool IsReady { get; set; }
    public string BrowserNativeRuntimeShellVersion { get; set; } = string.Empty;
    public string[] BrowserNativeRuntimeShellStages { get; set; } = Array.Empty<string>();
    public bool BrowserNativeRuntimeShellReady { get; set; }
    public string BrowserNativeRuntimeShellSummary { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string PackageVersion { get; set; } = string.Empty;
    public string BootstrapPackagePath { get; set; } = string.Empty;
    public string LaunchSessionPath { get; set; } = string.Empty;
    public string ExecutionMode { get; set; } = string.Empty;
    public string RenderingMode { get; set; } = string.Empty;
    public string InputMode { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string AssetMode { get; set; } = string.Empty;
    public string EnvironmentMode { get; set; } = string.Empty;
    public string TargetRuntime { get; set; } = string.Empty;
    public string NativeExecutionPlanVersion { get; set; } = string.Empty;
    public bool NativeExecutionPlanReady { get; set; }
    public bool RenderReady { get; set; }
    public bool InputReady { get; set; }
    public bool LaunchReady { get; set; }
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public string RenderSummary { get; set; } = string.Empty;
    public string InputSummary { get; set; } = string.Empty;
    public string LaunchSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
