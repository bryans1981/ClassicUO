namespace BrowserHost.Services;

public interface IBrowserClientRuntimeHostSession
{
    ValueTask<BrowserClientRuntimeHostSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeHostSessionService : IBrowserClientRuntimeHostSession
{
    private readonly IBrowserClientRuntimeClientLoopState _runtimeClientLoopState;

    public BrowserClientRuntimeHostSessionService(IBrowserClientRuntimeClientLoopState runtimeClientLoopState)
    {
        _runtimeClientLoopState = runtimeClientLoopState;
    }

    public async ValueTask<BrowserClientRuntimeHostSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeClientLoopStateResult loopState = await _runtimeClientLoopState.BuildAsync(profileId);

        BrowserClientRuntimeHostSessionResult result = new()
        {
            ProfileId = loopState.ProfileId,
            SessionId = loopState.SessionId,
            SessionPath = loopState.SessionPath,
            ClientLoopStateVersion = loopState.ClientLoopStateVersion,
            ClientRunStateVersion = loopState.ClientRunStateVersion,
            ClientActivationVersion = loopState.ClientActivationVersion,
            ClientLaunchSessionVersion = loopState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = loopState.ClientReadyStateVersion,
            LaunchControllerVersion = loopState.LaunchControllerVersion,
            ReadySignalVersion = loopState.ReadySignalVersion,
            ReadinessGateVersion = loopState.ReadinessGateVersion,
            ClientBootstrapVersion = loopState.ClientBootstrapVersion,
            RuntimeSessionVersion = loopState.RuntimeSessionVersion,
            ConsumerVersion = loopState.ConsumerVersion,
            HandoffVersion = loopState.HandoffVersion,
            BootSessionVersion = loopState.BootSessionVersion,
            BootFlowVersion = loopState.BootFlowVersion,
            OrchestratorVersion = loopState.OrchestratorVersion,
            CoordinatorVersion = loopState.CoordinatorVersion,
            ControllerVersion = loopState.ControllerVersion,
            DispatcherVersion = loopState.DispatcherVersion,
            DriverVersion = loopState.DriverVersion,
            StateMachineVersion = loopState.StateMachineVersion,
            StateVersion = loopState.StateVersion,
            CycleVersion = loopState.CycleVersion,
            InvocationVersion = loopState.InvocationVersion,
            LoopVersion = loopState.LoopVersion,
            RunnerVersion = loopState.RunnerVersion,
            ExecutionVersion = loopState.ExecutionVersion,
            HandshakeVersion = loopState.HandshakeVersion,
            PacketVersion = loopState.PacketVersion,
            ContractVersion = loopState.ContractVersion,
            Exists = loopState.Exists,
            ReadSucceeded = loopState.ReadSucceeded
        };

        if (!loopState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime host session blocked for profile '{loopState.ProfileId}'.";
            result.Error = loopState.Error;
            return result;
        }

        result.IsReady = true;
        result.HostSessionVersion = "runtime-host-session-v1";
        result.LaunchMode = loopState.LaunchMode;
        result.AssetRootPath = loopState.AssetRootPath;
        result.ProfilesRootPath = loopState.ProfilesRootPath;
        result.CacheRootPath = loopState.CacheRootPath;
        result.ConfigRootPath = loopState.ConfigRootPath;
        result.SettingsFilePath = loopState.SettingsFilePath;
        result.StartupProfilePath = loopState.StartupProfilePath;
        result.RequiredAssets = loopState.RequiredAssets;
        result.ReadyAssetCount = loopState.ReadyAssetCount;
        result.CompletedSteps = loopState.CompletedSteps;
        result.TotalSteps = loopState.TotalSteps;
        result.Phases = loopState.Phases;
        result.CurrentState = loopState.CurrentState;
        result.States = loopState.States;
        result.Transitions = loopState.Transitions;
        result.DispatchTargets = loopState.DispatchTargets;
        result.ControlActions = loopState.ControlActions;
        result.CoordinatorSteps = loopState.CoordinatorSteps;
        result.OrchestrationStages = loopState.OrchestrationStages;
        result.BootFlowStages = loopState.BootFlowStages;
        result.BootSessionStages = loopState.BootSessionStages;
        result.HandoffArtifacts = loopState.HandoffArtifacts;
        result.ConsumedArtifacts = loopState.ConsumedArtifacts;
        result.RuntimeSessionStages = loopState.RuntimeSessionStages;
        result.ClientBootstrapActions = loopState.ClientBootstrapActions;
        result.ReadinessChecks = loopState.ReadinessChecks;
        result.ReadySignals = loopState.ReadySignals;
        result.LaunchControlActions = loopState.LaunchControlActions;
        result.ReadyStates = loopState.ReadyStates;
        result.ClientLaunchStages = loopState.ClientLaunchStages;
        result.ActivationSteps = loopState.ActivationSteps;
        result.RunStateStages = loopState.RunStateStages;
        result.ClientLoopStages = loopState.ClientLoopStages;
        result.HostSessionStages =
        [
            "open-runtime-host-session",
            "bind-client-loop-state",
            "publish-host-session-ready"
        ];
        result.HostSessionSummary = $"Runtime host session prepared {result.HostSessionStages.Length} host-session stage(s) for profile '{loopState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime host session ready for profile '{loopState.ProfileId}' with {result.HostSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeHostSessionResult
{
    public bool IsReady { get; set; }
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
    public string HostSessionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
