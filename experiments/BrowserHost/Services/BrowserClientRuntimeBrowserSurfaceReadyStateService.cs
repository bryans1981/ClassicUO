namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSurfaceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSurfaceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSurfaceReadyStateService : IBrowserClientRuntimeBrowserSurfaceReadyState
{
    private readonly IBrowserClientRuntimeBrowserSurfaceSession _runtimeBrowserSurfaceSession;

    public BrowserClientRuntimeBrowserSurfaceReadyStateService(IBrowserClientRuntimeBrowserSurfaceSession runtimeBrowserSurfaceSession)
    {
        _runtimeBrowserSurfaceSession = runtimeBrowserSurfaceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSurfaceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSurfaceSessionResult browserSurfaceSession = await _runtimeBrowserSurfaceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSurfaceReadyStateResult result = new()
        {
            ProfileId = browserSurfaceSession.ProfileId,
            SessionId = browserSurfaceSession.SessionId,
            SessionPath = browserSurfaceSession.SessionPath,
            BrowserSurfaceSessionVersion = browserSurfaceSession.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserSurfaceSession.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserSurfaceSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserSurfaceSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserSurfaceSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserSurfaceSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserSurfaceSession.PlatformSessionVersion,
            HostReadyStateVersion = browserSurfaceSession.HostReadyStateVersion,
            HostLoopVersion = browserSurfaceSession.HostLoopVersion,
            HostSessionVersion = browserSurfaceSession.HostSessionVersion,
            ClientLoopStateVersion = browserSurfaceSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserSurfaceSession.ClientRunStateVersion,
            ClientActivationVersion = browserSurfaceSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserSurfaceSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserSurfaceSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserSurfaceSession.LaunchControllerVersion,
            ReadySignalVersion = browserSurfaceSession.ReadySignalVersion,
            ReadinessGateVersion = browserSurfaceSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserSurfaceSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserSurfaceSession.RuntimeSessionVersion,
            ConsumerVersion = browserSurfaceSession.ConsumerVersion,
            HandoffVersion = browserSurfaceSession.HandoffVersion,
            BootSessionVersion = browserSurfaceSession.BootSessionVersion,
            BootFlowVersion = browserSurfaceSession.BootFlowVersion,
            OrchestratorVersion = browserSurfaceSession.OrchestratorVersion,
            CoordinatorVersion = browserSurfaceSession.CoordinatorVersion,
            ControllerVersion = browserSurfaceSession.ControllerVersion,
            DispatcherVersion = browserSurfaceSession.DispatcherVersion,
            DriverVersion = browserSurfaceSession.DriverVersion,
            StateMachineVersion = browserSurfaceSession.StateMachineVersion,
            StateVersion = browserSurfaceSession.StateVersion,
            CycleVersion = browserSurfaceSession.CycleVersion,
            InvocationVersion = browserSurfaceSession.InvocationVersion,
            LoopVersion = browserSurfaceSession.LoopVersion,
            RunnerVersion = browserSurfaceSession.RunnerVersion,
            ExecutionVersion = browserSurfaceSession.ExecutionVersion,
            HandshakeVersion = browserSurfaceSession.HandshakeVersion,
            PacketVersion = browserSurfaceSession.PacketVersion,
            ContractVersion = browserSurfaceSession.ContractVersion,
            Exists = browserSurfaceSession.Exists,
            ReadSucceeded = browserSurfaceSession.ReadSucceeded
        };

        if (!browserSurfaceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser surface ready state blocked for profile '{browserSurfaceSession.ProfileId}'.";
            result.Error = browserSurfaceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSurfaceReadyStateVersion = "runtime-browser-surface-ready-state-v1";
        result.LaunchMode = browserSurfaceSession.LaunchMode;
        result.AssetRootPath = browserSurfaceSession.AssetRootPath;
        result.ProfilesRootPath = browserSurfaceSession.ProfilesRootPath;
        result.CacheRootPath = browserSurfaceSession.CacheRootPath;
        result.ConfigRootPath = browserSurfaceSession.ConfigRootPath;
        result.SettingsFilePath = browserSurfaceSession.SettingsFilePath;
        result.StartupProfilePath = browserSurfaceSession.StartupProfilePath;
        result.RequiredAssets = browserSurfaceSession.RequiredAssets;
        result.ReadyAssetCount = browserSurfaceSession.ReadyAssetCount;
        result.CompletedSteps = browserSurfaceSession.CompletedSteps;
        result.TotalSteps = browserSurfaceSession.TotalSteps;
        result.Phases = browserSurfaceSession.Phases;
        result.CurrentState = browserSurfaceSession.CurrentState;
        result.States = browserSurfaceSession.States;
        result.Transitions = browserSurfaceSession.Transitions;
        result.DispatchTargets = browserSurfaceSession.DispatchTargets;
        result.ControlActions = browserSurfaceSession.ControlActions;
        result.CoordinatorSteps = browserSurfaceSession.CoordinatorSteps;
        result.OrchestrationStages = browserSurfaceSession.OrchestrationStages;
        result.BootFlowStages = browserSurfaceSession.BootFlowStages;
        result.BootSessionStages = browserSurfaceSession.BootSessionStages;
        result.HandoffArtifacts = browserSurfaceSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserSurfaceSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserSurfaceSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserSurfaceSession.ClientBootstrapActions;
        result.ReadinessChecks = browserSurfaceSession.ReadinessChecks;
        result.ReadySignals = browserSurfaceSession.ReadySignals;
        result.LaunchControlActions = browserSurfaceSession.LaunchControlActions;
        result.ReadyStates = browserSurfaceSession.ReadyStates;
        result.ClientLaunchStages = browserSurfaceSession.ClientLaunchStages;
        result.ActivationSteps = browserSurfaceSession.ActivationSteps;
        result.RunStateStages = browserSurfaceSession.RunStateStages;
        result.ClientLoopStages = browserSurfaceSession.ClientLoopStages;
        result.HostSessionStages = browserSurfaceSession.HostSessionStages;
        result.HostLoopStages = browserSurfaceSession.HostLoopStages;
        result.HostReadyChecks = browserSurfaceSession.HostReadyChecks;
        result.PlatformSessionStages = browserSurfaceSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserSurfaceSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserSurfaceSession.PlatformLoopStages;
        result.LaunchGateChecks = browserSurfaceSession.LaunchGateChecks;
        result.BrowserShellStages = browserSurfaceSession.BrowserShellStages;
        result.BrowserShellReadyChecks = browserSurfaceSession.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserSurfaceSession.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks =
        [
            "browser-shell-ready-state-ready",
            "browser-surface-session-ready",
            "browser-surface-ready"
        ];
        result.BrowserSurfaceReadySummary = $"Runtime browser surface ready state passed {result.BrowserSurfaceReadyChecks.Length} surface readiness check(s) for profile '{browserSurfaceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser surface ready state ready for profile '{browserSurfaceSession.ProfileId}' with {result.BrowserSurfaceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSurfaceReadyStateResult
{
    public bool IsReady { get; set; }
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
    public string BrowserSurfaceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
