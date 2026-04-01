namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBootstrapSession
{
    ValueTask<BrowserClientRuntimeBootstrapSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBootstrapSessionService : IBrowserClientRuntimeBootstrapSession
{
    private readonly IBrowserClientRuntimeBootstrapConsumer _runtimeBootstrapConsumer;

    public BrowserClientRuntimeBootstrapSessionService(IBrowserClientRuntimeBootstrapConsumer runtimeBootstrapConsumer)
    {
        _runtimeBootstrapConsumer = runtimeBootstrapConsumer;
    }

    public async ValueTask<BrowserClientRuntimeBootstrapSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBootstrapConsumerResult consumer = await _runtimeBootstrapConsumer.ConsumeAsync(profileId);

        BrowserClientRuntimeBootstrapSessionResult result = new()
        {
            ProfileId = consumer.ProfileId,
            SessionId = consumer.SessionId,
            SessionPath = consumer.SessionPath,
            ConsumerVersion = consumer.ConsumerVersion,
            HandoffVersion = consumer.HandoffVersion,
            BootSessionVersion = consumer.BootSessionVersion,
            BootFlowVersion = consumer.BootFlowVersion,
            OrchestratorVersion = consumer.OrchestratorVersion,
            CoordinatorVersion = consumer.CoordinatorVersion,
            ControllerVersion = consumer.ControllerVersion,
            DispatcherVersion = consumer.DispatcherVersion,
            DriverVersion = consumer.DriverVersion,
            StateMachineVersion = consumer.StateMachineVersion,
            StateVersion = consumer.StateVersion,
            CycleVersion = consumer.CycleVersion,
            InvocationVersion = consumer.InvocationVersion,
            LoopVersion = consumer.LoopVersion,
            RunnerVersion = consumer.RunnerVersion,
            ExecutionVersion = consumer.ExecutionVersion,
            HandshakeVersion = consumer.HandshakeVersion,
            PacketVersion = consumer.PacketVersion,
            ContractVersion = consumer.ContractVersion,
            Exists = consumer.Exists,
            ReadSucceeded = consumer.ReadSucceeded
        };

        if (!consumer.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime bootstrap session blocked for profile '{consumer.ProfileId}'.";
            result.Error = consumer.Error;
            return result;
        }

        result.IsReady = true;
        result.RuntimeSessionVersion = "runtime-bootstrap-session-v1";
        result.LaunchMode = consumer.LaunchMode;
        result.AssetRootPath = consumer.AssetRootPath;
        result.ProfilesRootPath = consumer.ProfilesRootPath;
        result.CacheRootPath = consumer.CacheRootPath;
        result.ConfigRootPath = consumer.ConfigRootPath;
        result.SettingsFilePath = consumer.SettingsFilePath;
        result.StartupProfilePath = consumer.StartupProfilePath;
        result.RequiredAssets = consumer.RequiredAssets;
        result.ReadyAssetCount = consumer.ReadyAssetCount;
        result.CompletedSteps = consumer.CompletedSteps;
        result.TotalSteps = consumer.TotalSteps;
        result.Phases = consumer.Phases;
        result.CurrentState = consumer.CurrentState;
        result.States = consumer.States;
        result.Transitions = consumer.Transitions;
        result.DispatchTargets = consumer.DispatchTargets;
        result.ControlActions = consumer.ControlActions;
        result.CoordinatorSteps = consumer.CoordinatorSteps;
        result.OrchestrationStages = consumer.OrchestrationStages;
        result.BootFlowStages = consumer.BootFlowStages;
        result.BootSessionStages = consumer.BootSessionStages;
        result.HandoffArtifacts = consumer.HandoffArtifacts;
        result.ConsumedArtifacts = consumer.ConsumedArtifacts;
        result.RuntimeSessionStages =
        [
            "consume-launch-handoff",
            "open-runtime-session",
            "mark-runtime-bootstrap-ready"
        ];
        result.RuntimeSessionSummary = $"Runtime bootstrap session prepared {result.RuntimeSessionStages.Length} runtime session stage(s) for profile '{consumer.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime bootstrap session ready for profile '{consumer.ProfileId}' with {result.RuntimeSessionStages.Length} runtime stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBootstrapSessionResult
{
    public bool IsReady { get; set; }
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
    public string RuntimeSessionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
