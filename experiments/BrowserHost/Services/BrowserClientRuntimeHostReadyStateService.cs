namespace BrowserHost.Services;

public interface IBrowserClientRuntimeHostReadyState
{
    ValueTask<BrowserClientRuntimeHostReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeHostReadyStateService : IBrowserClientRuntimeHostReadyState
{
    private readonly IBrowserClientRuntimeHostLoop _runtimeHostLoop;

    public BrowserClientRuntimeHostReadyStateService(IBrowserClientRuntimeHostLoop runtimeHostLoop)
    {
        _runtimeHostLoop = runtimeHostLoop;
    }

    public async ValueTask<BrowserClientRuntimeHostReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeHostLoopResult hostLoop = await _runtimeHostLoop.RunAsync(profileId);

        BrowserClientRuntimeHostReadyStateResult result = new()
        {
            ProfileId = hostLoop.ProfileId,
            SessionId = hostLoop.SessionId,
            SessionPath = hostLoop.SessionPath,
            HostLoopVersion = hostLoop.HostLoopVersion,
            HostSessionVersion = hostLoop.HostSessionVersion,
            ClientLoopStateVersion = hostLoop.ClientLoopStateVersion,
            ClientRunStateVersion = hostLoop.ClientRunStateVersion,
            ClientActivationVersion = hostLoop.ClientActivationVersion,
            ClientLaunchSessionVersion = hostLoop.ClientLaunchSessionVersion,
            ClientReadyStateVersion = hostLoop.ClientReadyStateVersion,
            LaunchControllerVersion = hostLoop.LaunchControllerVersion,
            ReadySignalVersion = hostLoop.ReadySignalVersion,
            ReadinessGateVersion = hostLoop.ReadinessGateVersion,
            ClientBootstrapVersion = hostLoop.ClientBootstrapVersion,
            RuntimeSessionVersion = hostLoop.RuntimeSessionVersion,
            ConsumerVersion = hostLoop.ConsumerVersion,
            HandoffVersion = hostLoop.HandoffVersion,
            BootSessionVersion = hostLoop.BootSessionVersion,
            BootFlowVersion = hostLoop.BootFlowVersion,
            OrchestratorVersion = hostLoop.OrchestratorVersion,
            CoordinatorVersion = hostLoop.CoordinatorVersion,
            ControllerVersion = hostLoop.ControllerVersion,
            DispatcherVersion = hostLoop.DispatcherVersion,
            DriverVersion = hostLoop.DriverVersion,
            StateMachineVersion = hostLoop.StateMachineVersion,
            StateVersion = hostLoop.StateVersion,
            CycleVersion = hostLoop.CycleVersion,
            InvocationVersion = hostLoop.InvocationVersion,
            LoopVersion = hostLoop.LoopVersion,
            RunnerVersion = hostLoop.RunnerVersion,
            ExecutionVersion = hostLoop.ExecutionVersion,
            HandshakeVersion = hostLoop.HandshakeVersion,
            PacketVersion = hostLoop.PacketVersion,
            ContractVersion = hostLoop.ContractVersion,
            Exists = hostLoop.Exists,
            ReadSucceeded = hostLoop.ReadSucceeded
        };

        if (!hostLoop.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime host ready state blocked for profile '{hostLoop.ProfileId}'.";
            result.Error = hostLoop.Error;
            return result;
        }

        result.IsReady = true;
        result.HostReadyStateVersion = "runtime-host-ready-state-v1";
        result.LaunchMode = hostLoop.LaunchMode;
        result.AssetRootPath = hostLoop.AssetRootPath;
        result.ProfilesRootPath = hostLoop.ProfilesRootPath;
        result.CacheRootPath = hostLoop.CacheRootPath;
        result.ConfigRootPath = hostLoop.ConfigRootPath;
        result.SettingsFilePath = hostLoop.SettingsFilePath;
        result.StartupProfilePath = hostLoop.StartupProfilePath;
        result.RequiredAssets = hostLoop.RequiredAssets;
        result.ReadyAssetCount = hostLoop.ReadyAssetCount;
        result.CompletedSteps = hostLoop.CompletedSteps;
        result.TotalSteps = hostLoop.TotalSteps;
        result.Phases = hostLoop.Phases;
        result.CurrentState = hostLoop.CurrentState;
        result.States = hostLoop.States;
        result.Transitions = hostLoop.Transitions;
        result.DispatchTargets = hostLoop.DispatchTargets;
        result.ControlActions = hostLoop.ControlActions;
        result.CoordinatorSteps = hostLoop.CoordinatorSteps;
        result.OrchestrationStages = hostLoop.OrchestrationStages;
        result.BootFlowStages = hostLoop.BootFlowStages;
        result.BootSessionStages = hostLoop.BootSessionStages;
        result.HandoffArtifacts = hostLoop.HandoffArtifacts;
        result.ConsumedArtifacts = hostLoop.ConsumedArtifacts;
        result.RuntimeSessionStages = hostLoop.RuntimeSessionStages;
        result.ClientBootstrapActions = hostLoop.ClientBootstrapActions;
        result.ReadinessChecks = hostLoop.ReadinessChecks;
        result.ReadySignals = hostLoop.ReadySignals;
        result.LaunchControlActions = hostLoop.LaunchControlActions;
        result.ReadyStates = hostLoop.ReadyStates;
        result.ClientLaunchStages = hostLoop.ClientLaunchStages;
        result.ActivationSteps = hostLoop.ActivationSteps;
        result.RunStateStages = hostLoop.RunStateStages;
        result.ClientLoopStages = hostLoop.ClientLoopStages;
        result.HostSessionStages = hostLoop.HostSessionStages;
        result.HostLoopStages = hostLoop.HostLoopStages;
        result.HostReadyChecks =
        [
            "host-session-ready",
            "host-loop-ready",
            "host-ready"
        ];
        result.HostReadySummary = $"Runtime host ready state passed {result.HostReadyChecks.Length} host readiness check(s) for profile '{hostLoop.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime host ready state ready for profile '{hostLoop.ProfileId}' with {result.HostReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeHostReadyStateResult
{
    public bool IsReady { get; set; }
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
    public string HostReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
