namespace BrowserHost.Services;

public interface IBrowserClientRuntimeClientLoopState
{
    ValueTask<BrowserClientRuntimeClientLoopStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeClientLoopStateService : IBrowserClientRuntimeClientLoopState
{
    private readonly IBrowserClientRuntimeClientRunState _runtimeClientRunState;

    public BrowserClientRuntimeClientLoopStateService(IBrowserClientRuntimeClientRunState runtimeClientRunState)
    {
        _runtimeClientRunState = runtimeClientRunState;
    }

    public async ValueTask<BrowserClientRuntimeClientLoopStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeClientRunStateResult runState = await _runtimeClientRunState.BuildAsync(profileId);

        BrowserClientRuntimeClientLoopStateResult result = new()
        {
            ProfileId = runState.ProfileId,
            SessionId = runState.SessionId,
            SessionPath = runState.SessionPath,
            ClientRunStateVersion = runState.ClientRunStateVersion,
            ClientActivationVersion = runState.ClientActivationVersion,
            ClientLaunchSessionVersion = runState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = runState.ClientReadyStateVersion,
            LaunchControllerVersion = runState.LaunchControllerVersion,
            ReadySignalVersion = runState.ReadySignalVersion,
            ReadinessGateVersion = runState.ReadinessGateVersion,
            ClientBootstrapVersion = runState.ClientBootstrapVersion,
            RuntimeSessionVersion = runState.RuntimeSessionVersion,
            ConsumerVersion = runState.ConsumerVersion,
            HandoffVersion = runState.HandoffVersion,
            BootSessionVersion = runState.BootSessionVersion,
            BootFlowVersion = runState.BootFlowVersion,
            OrchestratorVersion = runState.OrchestratorVersion,
            CoordinatorVersion = runState.CoordinatorVersion,
            ControllerVersion = runState.ControllerVersion,
            DispatcherVersion = runState.DispatcherVersion,
            DriverVersion = runState.DriverVersion,
            StateMachineVersion = runState.StateMachineVersion,
            StateVersion = runState.StateVersion,
            CycleVersion = runState.CycleVersion,
            InvocationVersion = runState.InvocationVersion,
            LoopVersion = runState.LoopVersion,
            RunnerVersion = runState.RunnerVersion,
            ExecutionVersion = runState.ExecutionVersion,
            HandshakeVersion = runState.HandshakeVersion,
            PacketVersion = runState.PacketVersion,
            ContractVersion = runState.ContractVersion,
            Exists = runState.Exists,
            ReadSucceeded = runState.ReadSucceeded
        };

        if (!runState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime client loop state blocked for profile '{runState.ProfileId}'.";
            result.Error = runState.Error;
            return result;
        }

        result.IsReady = true;
        result.ClientLoopStateVersion = "runtime-client-loop-state-v1";
        result.LaunchMode = runState.LaunchMode;
        result.AssetRootPath = runState.AssetRootPath;
        result.ProfilesRootPath = runState.ProfilesRootPath;
        result.CacheRootPath = runState.CacheRootPath;
        result.ConfigRootPath = runState.ConfigRootPath;
        result.SettingsFilePath = runState.SettingsFilePath;
        result.StartupProfilePath = runState.StartupProfilePath;
        result.RequiredAssets = runState.RequiredAssets;
        result.ReadyAssetCount = runState.ReadyAssetCount;
        result.CompletedSteps = runState.CompletedSteps;
        result.TotalSteps = runState.TotalSteps;
        result.Phases = runState.Phases;
        result.CurrentState = runState.CurrentState;
        result.States = runState.States;
        result.Transitions = runState.Transitions;
        result.DispatchTargets = runState.DispatchTargets;
        result.ControlActions = runState.ControlActions;
        result.CoordinatorSteps = runState.CoordinatorSteps;
        result.OrchestrationStages = runState.OrchestrationStages;
        result.BootFlowStages = runState.BootFlowStages;
        result.BootSessionStages = runState.BootSessionStages;
        result.HandoffArtifacts = runState.HandoffArtifacts;
        result.ConsumedArtifacts = runState.ConsumedArtifacts;
        result.RuntimeSessionStages = runState.RuntimeSessionStages;
        result.ClientBootstrapActions = runState.ClientBootstrapActions;
        result.ReadinessChecks = runState.ReadinessChecks;
        result.ReadySignals = runState.ReadySignals;
        result.LaunchControlActions = runState.LaunchControlActions;
        result.ReadyStates = runState.ReadyStates;
        result.ClientLaunchStages = runState.ClientLaunchStages;
        result.ActivationSteps = runState.ActivationSteps;
        result.RunStateStages = runState.RunStateStages;
        result.ClientLoopStages =
        [
            "initialize-client-loop",
            "bind-client-run-state",
            "publish-client-loop-ready"
        ];
        result.ClientLoopSummary = $"Runtime client loop state prepared {result.ClientLoopStages.Length} loop stage(s) for profile '{runState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime client loop state ready for profile '{runState.ProfileId}' with {result.ClientLoopStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeClientLoopStateResult
{
    public bool IsReady { get; set; }
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
    public string ClientLoopSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
