namespace BrowserHost.Services;

public interface IBrowserClientRuntimeClientRunState
{
    ValueTask<BrowserClientRuntimeClientRunStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeClientRunStateService : IBrowserClientRuntimeClientRunState
{
    private readonly IBrowserClientRuntimeClientActivation _runtimeClientActivation;

    public BrowserClientRuntimeClientRunStateService(IBrowserClientRuntimeClientActivation runtimeClientActivation)
    {
        _runtimeClientActivation = runtimeClientActivation;
    }

    public async ValueTask<BrowserClientRuntimeClientRunStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeClientActivationResult activation = await _runtimeClientActivation.ActivateAsync(profileId);

        BrowserClientRuntimeClientRunStateResult result = new()
        {
            ProfileId = activation.ProfileId,
            SessionId = activation.SessionId,
            SessionPath = activation.SessionPath,
            ClientActivationVersion = activation.ClientActivationVersion,
            ClientLaunchSessionVersion = activation.ClientLaunchSessionVersion,
            ClientReadyStateVersion = activation.ClientReadyStateVersion,
            LaunchControllerVersion = activation.LaunchControllerVersion,
            ReadySignalVersion = activation.ReadySignalVersion,
            ReadinessGateVersion = activation.ReadinessGateVersion,
            ClientBootstrapVersion = activation.ClientBootstrapVersion,
            RuntimeSessionVersion = activation.RuntimeSessionVersion,
            ConsumerVersion = activation.ConsumerVersion,
            HandoffVersion = activation.HandoffVersion,
            BootSessionVersion = activation.BootSessionVersion,
            BootFlowVersion = activation.BootFlowVersion,
            OrchestratorVersion = activation.OrchestratorVersion,
            CoordinatorVersion = activation.CoordinatorVersion,
            ControllerVersion = activation.ControllerVersion,
            DispatcherVersion = activation.DispatcherVersion,
            DriverVersion = activation.DriverVersion,
            StateMachineVersion = activation.StateMachineVersion,
            StateVersion = activation.StateVersion,
            CycleVersion = activation.CycleVersion,
            InvocationVersion = activation.InvocationVersion,
            LoopVersion = activation.LoopVersion,
            RunnerVersion = activation.RunnerVersion,
            ExecutionVersion = activation.ExecutionVersion,
            HandshakeVersion = activation.HandshakeVersion,
            PacketVersion = activation.PacketVersion,
            ContractVersion = activation.ContractVersion,
            Exists = activation.Exists,
            ReadSucceeded = activation.ReadSucceeded
        };

        if (!activation.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime client run state blocked for profile '{activation.ProfileId}'.";
            result.Error = activation.Error;
            return result;
        }

        result.IsReady = true;
        result.ClientRunStateVersion = "runtime-client-run-state-v1";
        result.LaunchMode = activation.LaunchMode;
        result.AssetRootPath = activation.AssetRootPath;
        result.ProfilesRootPath = activation.ProfilesRootPath;
        result.CacheRootPath = activation.CacheRootPath;
        result.ConfigRootPath = activation.ConfigRootPath;
        result.SettingsFilePath = activation.SettingsFilePath;
        result.StartupProfilePath = activation.StartupProfilePath;
        result.RequiredAssets = activation.RequiredAssets;
        result.ReadyAssetCount = activation.ReadyAssetCount;
        result.CompletedSteps = activation.CompletedSteps;
        result.TotalSteps = activation.TotalSteps;
        result.Phases = activation.Phases;
        result.CurrentState = activation.CurrentState;
        result.States = activation.States;
        result.Transitions = activation.Transitions;
        result.DispatchTargets = activation.DispatchTargets;
        result.ControlActions = activation.ControlActions;
        result.CoordinatorSteps = activation.CoordinatorSteps;
        result.OrchestrationStages = activation.OrchestrationStages;
        result.BootFlowStages = activation.BootFlowStages;
        result.BootSessionStages = activation.BootSessionStages;
        result.HandoffArtifacts = activation.HandoffArtifacts;
        result.ConsumedArtifacts = activation.ConsumedArtifacts;
        result.RuntimeSessionStages = activation.RuntimeSessionStages;
        result.ClientBootstrapActions = activation.ClientBootstrapActions;
        result.ReadinessChecks = activation.ReadinessChecks;
        result.ReadySignals = activation.ReadySignals;
        result.LaunchControlActions = activation.LaunchControlActions;
        result.ReadyStates = activation.ReadyStates;
        result.ClientLaunchStages = activation.ClientLaunchStages;
        result.ActivationSteps = activation.ActivationSteps;
        result.RunStateStages =
        [
            "bind-client-activation",
            "prepare-client-run-state",
            "publish-client-running"
        ];
        result.RunStateSummary = $"Runtime client run state prepared {result.RunStateStages.Length} run-state stage(s) for profile '{activation.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime client run state ready for profile '{activation.ProfileId}' with {result.RunStateStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeClientRunStateResult
{
    public bool IsReady { get; set; }
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
    public string RunStateSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
