namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserShellReadyState
{
    ValueTask<BrowserClientRuntimeBrowserShellReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserShellReadyStateService : IBrowserClientRuntimeBrowserShellReadyState
{
    private readonly IBrowserClientRuntimeBrowserShellSession _runtimeBrowserShellSession;

    public BrowserClientRuntimeBrowserShellReadyStateService(IBrowserClientRuntimeBrowserShellSession runtimeBrowserShellSession)
    {
        _runtimeBrowserShellSession = runtimeBrowserShellSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserShellReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserShellSessionResult browserShellSession = await _runtimeBrowserShellSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserShellReadyStateResult result = new()
        {
            ProfileId = browserShellSession.ProfileId,
            SessionId = browserShellSession.SessionId,
            SessionPath = browserShellSession.SessionPath,
            BrowserShellSessionVersion = browserShellSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserShellSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserShellSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserShellSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserShellSession.PlatformSessionVersion,
            HostReadyStateVersion = browserShellSession.HostReadyStateVersion,
            HostLoopVersion = browserShellSession.HostLoopVersion,
            HostSessionVersion = browserShellSession.HostSessionVersion,
            ClientLoopStateVersion = browserShellSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserShellSession.ClientRunStateVersion,
            ClientActivationVersion = browserShellSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserShellSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserShellSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserShellSession.LaunchControllerVersion,
            ReadySignalVersion = browserShellSession.ReadySignalVersion,
            ReadinessGateVersion = browserShellSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserShellSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserShellSession.RuntimeSessionVersion,
            ConsumerVersion = browserShellSession.ConsumerVersion,
            HandoffVersion = browserShellSession.HandoffVersion,
            BootSessionVersion = browserShellSession.BootSessionVersion,
            BootFlowVersion = browserShellSession.BootFlowVersion,
            OrchestratorVersion = browserShellSession.OrchestratorVersion,
            CoordinatorVersion = browserShellSession.CoordinatorVersion,
            ControllerVersion = browserShellSession.ControllerVersion,
            DispatcherVersion = browserShellSession.DispatcherVersion,
            DriverVersion = browserShellSession.DriverVersion,
            StateMachineVersion = browserShellSession.StateMachineVersion,
            StateVersion = browserShellSession.StateVersion,
            CycleVersion = browserShellSession.CycleVersion,
            InvocationVersion = browserShellSession.InvocationVersion,
            LoopVersion = browserShellSession.LoopVersion,
            RunnerVersion = browserShellSession.RunnerVersion,
            ExecutionVersion = browserShellSession.ExecutionVersion,
            HandshakeVersion = browserShellSession.HandshakeVersion,
            PacketVersion = browserShellSession.PacketVersion,
            ContractVersion = browserShellSession.ContractVersion,
            Exists = browserShellSession.Exists,
            ReadSucceeded = browserShellSession.ReadSucceeded
        };

        if (!browserShellSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser shell ready state blocked for profile '{browserShellSession.ProfileId}'.";
            result.Error = browserShellSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserShellReadyStateVersion = "runtime-browser-shell-ready-state-v1";
        result.LaunchMode = browserShellSession.LaunchMode;
        result.AssetRootPath = browserShellSession.AssetRootPath;
        result.ProfilesRootPath = browserShellSession.ProfilesRootPath;
        result.CacheRootPath = browserShellSession.CacheRootPath;
        result.ConfigRootPath = browserShellSession.ConfigRootPath;
        result.SettingsFilePath = browserShellSession.SettingsFilePath;
        result.StartupProfilePath = browserShellSession.StartupProfilePath;
        result.RequiredAssets = browserShellSession.RequiredAssets;
        result.ReadyAssetCount = browserShellSession.ReadyAssetCount;
        result.CompletedSteps = browserShellSession.CompletedSteps;
        result.TotalSteps = browserShellSession.TotalSteps;
        result.Phases = browserShellSession.Phases;
        result.CurrentState = browserShellSession.CurrentState;
        result.States = browserShellSession.States;
        result.Transitions = browserShellSession.Transitions;
        result.DispatchTargets = browserShellSession.DispatchTargets;
        result.ControlActions = browserShellSession.ControlActions;
        result.CoordinatorSteps = browserShellSession.CoordinatorSteps;
        result.OrchestrationStages = browserShellSession.OrchestrationStages;
        result.BootFlowStages = browserShellSession.BootFlowStages;
        result.BootSessionStages = browserShellSession.BootSessionStages;
        result.HandoffArtifacts = browserShellSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserShellSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserShellSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserShellSession.ClientBootstrapActions;
        result.ReadinessChecks = browserShellSession.ReadinessChecks;
        result.ReadySignals = browserShellSession.ReadySignals;
        result.LaunchControlActions = browserShellSession.LaunchControlActions;
        result.ReadyStates = browserShellSession.ReadyStates;
        result.ClientLaunchStages = browserShellSession.ClientLaunchStages;
        result.ActivationSteps = browserShellSession.ActivationSteps;
        result.RunStateStages = browserShellSession.RunStateStages;
        result.ClientLoopStages = browserShellSession.ClientLoopStages;
        result.HostSessionStages = browserShellSession.HostSessionStages;
        result.HostLoopStages = browserShellSession.HostLoopStages;
        result.HostReadyChecks = browserShellSession.HostReadyChecks;
        result.PlatformSessionStages = browserShellSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserShellSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserShellSession.PlatformLoopStages;
        result.LaunchGateChecks = browserShellSession.LaunchGateChecks;
        result.BrowserShellStages = browserShellSession.BrowserShellStages;
        result.BrowserShellReadyChecks =
        [
            "browser-shell-session-ready",
            "platform-launch-gate-open",
            "browser-shell-ready"
        ];
        result.BrowserShellReadySummary = $"Runtime browser shell ready state passed {result.BrowserShellReadyChecks.Length} shell readiness check(s) for profile '{browserShellSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser shell ready state ready for profile '{browserShellSession.ProfileId}' with {result.BrowserShellReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserShellReadyStateResult
{
    public bool IsReady { get; set; }
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
    public string BrowserShellReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
