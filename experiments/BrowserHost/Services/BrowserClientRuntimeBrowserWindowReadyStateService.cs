namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserWindowReadyState
{
    ValueTask<BrowserClientRuntimeBrowserWindowReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserWindowReadyStateService : IBrowserClientRuntimeBrowserWindowReadyState
{
    private readonly IBrowserClientRuntimeBrowserWindowSession _runtimeBrowserWindowSession;

    public BrowserClientRuntimeBrowserWindowReadyStateService(IBrowserClientRuntimeBrowserWindowSession runtimeBrowserWindowSession)
    {
        _runtimeBrowserWindowSession = runtimeBrowserWindowSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserWindowReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserWindowSessionResult browserWindowSession = await _runtimeBrowserWindowSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserWindowReadyStateResult result = new()
        {
            ProfileId = browserWindowSession.ProfileId,
            SessionId = browserWindowSession.SessionId,
            SessionPath = browserWindowSession.SessionPath,
            BrowserWindowSessionVersion = browserWindowSession.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserWindowSession.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserWindowSession.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserWindowSession.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserWindowSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserWindowSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserWindowSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserWindowSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserWindowSession.PlatformSessionVersion,
            HostReadyStateVersion = browserWindowSession.HostReadyStateVersion,
            HostLoopVersion = browserWindowSession.HostLoopVersion,
            HostSessionVersion = browserWindowSession.HostSessionVersion,
            ClientLoopStateVersion = browserWindowSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserWindowSession.ClientRunStateVersion,
            ClientActivationVersion = browserWindowSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserWindowSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserWindowSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserWindowSession.LaunchControllerVersion,
            ReadySignalVersion = browserWindowSession.ReadySignalVersion,
            ReadinessGateVersion = browserWindowSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserWindowSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserWindowSession.RuntimeSessionVersion,
            ConsumerVersion = browserWindowSession.ConsumerVersion,
            HandoffVersion = browserWindowSession.HandoffVersion,
            BootSessionVersion = browserWindowSession.BootSessionVersion,
            BootFlowVersion = browserWindowSession.BootFlowVersion,
            OrchestratorVersion = browserWindowSession.OrchestratorVersion,
            CoordinatorVersion = browserWindowSession.CoordinatorVersion,
            ControllerVersion = browserWindowSession.ControllerVersion,
            DispatcherVersion = browserWindowSession.DispatcherVersion,
            DriverVersion = browserWindowSession.DriverVersion,
            StateMachineVersion = browserWindowSession.StateMachineVersion,
            StateVersion = browserWindowSession.StateVersion,
            CycleVersion = browserWindowSession.CycleVersion,
            InvocationVersion = browserWindowSession.InvocationVersion,
            LoopVersion = browserWindowSession.LoopVersion,
            RunnerVersion = browserWindowSession.RunnerVersion,
            ExecutionVersion = browserWindowSession.ExecutionVersion,
            HandshakeVersion = browserWindowSession.HandshakeVersion,
            PacketVersion = browserWindowSession.PacketVersion,
            ContractVersion = browserWindowSession.ContractVersion,
            Exists = browserWindowSession.Exists,
            ReadSucceeded = browserWindowSession.ReadSucceeded
        };

        if (!browserWindowSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser window ready state blocked for profile '{browserWindowSession.ProfileId}'.";
            result.Error = browserWindowSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserWindowReadyStateVersion = "runtime-browser-window-ready-state-v1";
        result.LaunchMode = browserWindowSession.LaunchMode;
        result.AssetRootPath = browserWindowSession.AssetRootPath;
        result.ProfilesRootPath = browserWindowSession.ProfilesRootPath;
        result.CacheRootPath = browserWindowSession.CacheRootPath;
        result.ConfigRootPath = browserWindowSession.ConfigRootPath;
        result.SettingsFilePath = browserWindowSession.SettingsFilePath;
        result.StartupProfilePath = browserWindowSession.StartupProfilePath;
        result.RequiredAssets = browserWindowSession.RequiredAssets;
        result.ReadyAssetCount = browserWindowSession.ReadyAssetCount;
        result.CompletedSteps = browserWindowSession.CompletedSteps;
        result.TotalSteps = browserWindowSession.TotalSteps;
        result.Phases = browserWindowSession.Phases;
        result.CurrentState = browserWindowSession.CurrentState;
        result.States = browserWindowSession.States;
        result.Transitions = browserWindowSession.Transitions;
        result.DispatchTargets = browserWindowSession.DispatchTargets;
        result.ControlActions = browserWindowSession.ControlActions;
        result.CoordinatorSteps = browserWindowSession.CoordinatorSteps;
        result.OrchestrationStages = browserWindowSession.OrchestrationStages;
        result.BootFlowStages = browserWindowSession.BootFlowStages;
        result.BootSessionStages = browserWindowSession.BootSessionStages;
        result.HandoffArtifacts = browserWindowSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserWindowSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserWindowSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserWindowSession.ClientBootstrapActions;
        result.ReadinessChecks = browserWindowSession.ReadinessChecks;
        result.ReadySignals = browserWindowSession.ReadySignals;
        result.LaunchControlActions = browserWindowSession.LaunchControlActions;
        result.ReadyStates = browserWindowSession.ReadyStates;
        result.ClientLaunchStages = browserWindowSession.ClientLaunchStages;
        result.ActivationSteps = browserWindowSession.ActivationSteps;
        result.RunStateStages = browserWindowSession.RunStateStages;
        result.ClientLoopStages = browserWindowSession.ClientLoopStages;
        result.HostSessionStages = browserWindowSession.HostSessionStages;
        result.HostLoopStages = browserWindowSession.HostLoopStages;
        result.HostReadyChecks = browserWindowSession.HostReadyChecks;
        result.PlatformSessionStages = browserWindowSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserWindowSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserWindowSession.PlatformLoopStages;
        result.LaunchGateChecks = browserWindowSession.LaunchGateChecks;
        result.BrowserShellStages = browserWindowSession.BrowserShellStages;
        result.BrowserShellReadyChecks = browserWindowSession.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserWindowSession.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserWindowSession.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserWindowSession.BrowserWindowStages;
        result.BrowserWindowReadyChecks =
        [
            "browser-surface-ready-state-ready",
            "browser-window-session-ready",
            "browser-window-ready"
        ];
        result.BrowserWindowReadySummary = $"Runtime browser window ready state passed {result.BrowserWindowReadyChecks.Length} window readiness check(s) for profile '{browserWindowSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser window ready state ready for profile '{browserWindowSession.ProfileId}' with {result.BrowserWindowReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserWindowReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserWindowReadyStateVersion { get; set; } = string.Empty;
    public string BrowserWindowSessionVersion { get; set; } = string.Empty;
    public string BrowserSurfaceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSurfaceSessionVersion { get; set; } = string.Empty;
    public string BrowserShellReadyStateVersion { get; set; } = string.Empty;
    public string BrowserShellSessionVersion { get; set; } = string.Empty;
    public string PlatformLaunchGateVersion { get; set; } = string.Empty;
    public string PlatformLoopVersion { get; set; } = string.Empty;
    public string PlatformReadyStateVersion { get; set; } = string.Empty;
    public string PlatformSessionVersion { get; set; } = string.Empty;
    public string HostReadyStateVersion { get; set; } = string.Empty;
    public string HostLoopVersion { get; set; } = string.Empty;
    public string HostSessionVersion { get; set; } = string.Empty;
    public string ClientLoopStateVersion { get; set; } = string.Empty;
    public string ClientRunStateVersion { get; set; } = string.Empty;
    public string ClientActivationVersion { get; set; } = string.Empty;
    public string ClientLaunchSessionVersion { get; set; } = string.Empty;
    public string ClientReadyStateVersion { get; set; } = string.Empty;
    public string LaunchControllerVersion { get; set; } = string.Empty;
    public string ReadySignalVersion { get; set; } = string.Empty;
    public string ReadinessGateVersion { get; set; } = string.Empty;
    public string ClientBootstrapVersion { get; set; } = string.Empty;
    public string RuntimeSessionVersion { get; set; } = string.Empty;
    public string ConsumerVersion { get; set; } = string.Empty;
    public string HandoffVersion { get; set; } = string.Empty;
    public string BootSessionVersion { get; set; } = string.Empty;
    public string BootFlowVersion { get; set; } = string.Empty;
    public string OrchestratorVersion { get; set; } = string.Empty;
    public string CoordinatorVersion { get; set; } = string.Empty;
    public string ControllerVersion { get; set; } = string.Empty;
    public string DispatcherVersion { get; set; } = string.Empty;
    public string DriverVersion { get; set; } = string.Empty;
    public string StateMachineVersion { get; set; } = string.Empty;
    public string StateVersion { get; set; } = string.Empty;
    public string CycleVersion { get; set; } = string.Empty;
    public string InvocationVersion { get; set; } = string.Empty;
    public string LoopVersion { get; set; } = string.Empty;
    public string RunnerVersion { get; set; } = string.Empty;
    public string ExecutionVersion { get; set; } = string.Empty;
    public string HandshakeVersion { get; set; } = string.Empty;
    public string PacketVersion { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string SessionId { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public string AssetRootPath { get; set; } = string.Empty;
    public string ProfilesRootPath { get; set; } = string.Empty;
    public string CacheRootPath { get; set; } = string.Empty;
    public string ConfigRootPath { get; set; } = string.Empty;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public int ReadyAssetCount { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public BrowserClientRuntimeBootstrapPhase[] Phases { get; set; } = Array.Empty<BrowserClientRuntimeBootstrapPhase>();
    public string CurrentState { get; set; } = string.Empty;
    public string[] States { get; set; } = Array.Empty<string>();
    public string[] Transitions { get; set; } = Array.Empty<string>();
    public string[] DispatchTargets { get; set; } = Array.Empty<string>();
    public string[] ControlActions { get; set; } = Array.Empty<string>();
    public string[] CoordinatorSteps { get; set; } = Array.Empty<string>();
    public string[] OrchestrationStages { get; set; } = Array.Empty<string>();
    public string[] BootFlowStages { get; set; } = Array.Empty<string>();
    public string[] BootSessionStages { get; set; } = Array.Empty<string>();
    public string[] HandoffArtifacts { get; set; } = Array.Empty<string>();
    public string[] ConsumedArtifacts { get; set; } = Array.Empty<string>();
    public string[] RuntimeSessionStages { get; set; } = Array.Empty<string>();
    public string[] ClientBootstrapActions { get; set; } = Array.Empty<string>();
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public string[] ReadySignals { get; set; } = Array.Empty<string>();
    public string[] LaunchControlActions { get; set; } = Array.Empty<string>();
    public string[] ReadyStates { get; set; } = Array.Empty<string>();
    public string[] ClientLaunchStages { get; set; } = Array.Empty<string>();
    public string[] ActivationSteps { get; set; } = Array.Empty<string>();
    public string[] RunStateStages { get; set; } = Array.Empty<string>();
    public string[] ClientLoopStages { get; set; } = Array.Empty<string>();
    public string[] HostSessionStages { get; set; } = Array.Empty<string>();
    public string[] HostLoopStages { get; set; } = Array.Empty<string>();
    public string[] HostReadyChecks { get; set; } = Array.Empty<string>();
    public string[] PlatformSessionStages { get; set; } = Array.Empty<string>();
    public string[] PlatformReadyChecks { get; set; } = Array.Empty<string>();
    public string[] PlatformLoopStages { get; set; } = Array.Empty<string>();
    public string[] LaunchGateChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserShellStages { get; set; } = Array.Empty<string>();
    public string[] BrowserShellReadyChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserSurfaceStages { get; set; } = Array.Empty<string>();
    public string[] BrowserSurfaceReadyChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserWindowStages { get; set; } = Array.Empty<string>();
    public string[] BrowserWindowReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserWindowReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
