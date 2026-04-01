namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBootstrapConsumer
{
    ValueTask<BrowserClientRuntimeBootstrapConsumerResult> ConsumeAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBootstrapConsumerService : IBrowserClientRuntimeBootstrapConsumer
{
    private readonly IBrowserClientRuntimeLaunchHandoff _runtimeLaunchHandoff;

    public BrowserClientRuntimeBootstrapConsumerService(IBrowserClientRuntimeLaunchHandoff runtimeLaunchHandoff)
    {
        _runtimeLaunchHandoff = runtimeLaunchHandoff;
    }

    public async ValueTask<BrowserClientRuntimeBootstrapConsumerResult> ConsumeAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeLaunchHandoffResult handoff = await _runtimeLaunchHandoff.BuildAsync(profileId);

        BrowserClientRuntimeBootstrapConsumerResult result = new()
        {
            ProfileId = handoff.ProfileId,
            SessionId = handoff.SessionId,
            SessionPath = handoff.SessionPath,
            HandoffVersion = handoff.HandoffVersion,
            BootSessionVersion = handoff.BootSessionVersion,
            BootFlowVersion = handoff.BootFlowVersion,
            OrchestratorVersion = handoff.OrchestratorVersion,
            CoordinatorVersion = handoff.CoordinatorVersion,
            ControllerVersion = handoff.ControllerVersion,
            DispatcherVersion = handoff.DispatcherVersion,
            DriverVersion = handoff.DriverVersion,
            StateMachineVersion = handoff.StateMachineVersion,
            StateVersion = handoff.StateVersion,
            CycleVersion = handoff.CycleVersion,
            InvocationVersion = handoff.InvocationVersion,
            LoopVersion = handoff.LoopVersion,
            RunnerVersion = handoff.RunnerVersion,
            ExecutionVersion = handoff.ExecutionVersion,
            HandshakeVersion = handoff.HandshakeVersion,
            PacketVersion = handoff.PacketVersion,
            ContractVersion = handoff.ContractVersion,
            Exists = handoff.Exists,
            ReadSucceeded = handoff.ReadSucceeded
        };

        if (!handoff.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime bootstrap consumer blocked for profile '{handoff.ProfileId}'.";
            result.Error = handoff.Error;
            return result;
        }

        result.IsReady = true;
        result.ConsumerVersion = "runtime-bootstrap-consumer-v1";
        result.LaunchMode = handoff.LaunchMode;
        result.AssetRootPath = handoff.AssetRootPath;
        result.ProfilesRootPath = handoff.ProfilesRootPath;
        result.CacheRootPath = handoff.CacheRootPath;
        result.ConfigRootPath = handoff.ConfigRootPath;
        result.SettingsFilePath = handoff.SettingsFilePath;
        result.StartupProfilePath = handoff.StartupProfilePath;
        result.RequiredAssets = handoff.RequiredAssets;
        result.ReadyAssetCount = handoff.ReadyAssetCount;
        result.CompletedSteps = handoff.CompletedSteps;
        result.TotalSteps = handoff.TotalSteps;
        result.Phases = handoff.Phases;
        result.CurrentState = handoff.CurrentState;
        result.States = handoff.States;
        result.Transitions = handoff.Transitions;
        result.DispatchTargets = handoff.DispatchTargets;
        result.ControlActions = handoff.ControlActions;
        result.CoordinatorSteps = handoff.CoordinatorSteps;
        result.OrchestrationStages = handoff.OrchestrationStages;
        result.BootFlowStages = handoff.BootFlowStages;
        result.BootSessionStages = handoff.BootSessionStages;
        result.HandoffArtifacts = handoff.HandoffArtifacts;
        result.ConsumedArtifacts = handoff.HandoffArtifacts;
        result.ConsumerSummary = $"Runtime bootstrap consumer accepted {result.ConsumedArtifacts.Length} handoff artifact(s) for profile '{handoff.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime bootstrap consumer ready for profile '{handoff.ProfileId}' with {result.ConsumedArtifacts.Length} consumed artifact(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBootstrapConsumerResult
{
    public bool IsReady { get; set; }
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
    public string ConsumerSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
