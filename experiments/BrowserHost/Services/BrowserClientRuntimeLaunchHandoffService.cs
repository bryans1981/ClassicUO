namespace BrowserHost.Services;

public interface IBrowserClientRuntimeLaunchHandoff
{
    ValueTask<BrowserClientRuntimeLaunchHandoffResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeLaunchHandoffService : IBrowserClientRuntimeLaunchHandoff
{
    private readonly IBrowserClientBootSession _bootSession;

    public BrowserClientRuntimeLaunchHandoffService(IBrowserClientBootSession bootSession)
    {
        _bootSession = bootSession;
    }

    public async ValueTask<BrowserClientRuntimeLaunchHandoffResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientBootSessionResult bootSession = await _bootSession.CreateAsync(profileId);

        BrowserClientRuntimeLaunchHandoffResult result = new()
        {
            ProfileId = bootSession.ProfileId,
            SessionId = bootSession.SessionId,
            SessionPath = bootSession.SessionPath,
            BootSessionVersion = bootSession.BootSessionVersion,
            BootFlowVersion = bootSession.BootFlowVersion,
            OrchestratorVersion = bootSession.OrchestratorVersion,
            CoordinatorVersion = bootSession.CoordinatorVersion,
            ControllerVersion = bootSession.ControllerVersion,
            DispatcherVersion = bootSession.DispatcherVersion,
            DriverVersion = bootSession.DriverVersion,
            StateMachineVersion = bootSession.StateMachineVersion,
            StateVersion = bootSession.StateVersion,
            CycleVersion = bootSession.CycleVersion,
            InvocationVersion = bootSession.InvocationVersion,
            LoopVersion = bootSession.LoopVersion,
            RunnerVersion = bootSession.RunnerVersion,
            ExecutionVersion = bootSession.ExecutionVersion,
            HandshakeVersion = bootSession.HandshakeVersion,
            PacketVersion = bootSession.PacketVersion,
            ContractVersion = bootSession.ContractVersion,
            Exists = bootSession.Exists,
            ReadSucceeded = bootSession.ReadSucceeded
        };

        if (!bootSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime launch handoff blocked for profile '{bootSession.ProfileId}'.";
            result.Error = bootSession.Error;
            return result;
        }

        result.IsReady = true;
        result.HandoffVersion = "runtime-launch-handoff-v1";
        result.LaunchMode = bootSession.LaunchMode;
        result.AssetRootPath = bootSession.AssetRootPath;
        result.ProfilesRootPath = bootSession.ProfilesRootPath;
        result.CacheRootPath = bootSession.CacheRootPath;
        result.ConfigRootPath = bootSession.ConfigRootPath;
        result.SettingsFilePath = bootSession.SettingsFilePath;
        result.StartupProfilePath = bootSession.StartupProfilePath;
        result.RequiredAssets = bootSession.RequiredAssets;
        result.ReadyAssetCount = bootSession.ReadyAssetCount;
        result.CompletedSteps = bootSession.CompletedSteps;
        result.TotalSteps = bootSession.TotalSteps;
        result.Phases = bootSession.Phases;
        result.CurrentState = bootSession.CurrentState;
        result.States = bootSession.States;
        result.Transitions = bootSession.Transitions;
        result.DispatchTargets = bootSession.DispatchTargets;
        result.ControlActions = bootSession.ControlActions;
        result.CoordinatorSteps = bootSession.CoordinatorSteps;
        result.OrchestrationStages = bootSession.OrchestrationStages;
        result.BootFlowStages = bootSession.BootFlowStages;
        result.BootSessionStages = bootSession.BootSessionStages;
        result.HandoffArtifacts =
        [
            "/cache/startup/default/launch-plan.json",
            "/cache/startup/default/launch-session.json"
        ];
        result.HandoffSummary = $"Runtime launch handoff prepared {result.HandoffArtifacts.Length} handoff artifact(s) for profile '{bootSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime launch handoff ready for profile '{bootSession.ProfileId}' with {result.HandoffArtifacts.Length} artifact(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeLaunchHandoffResult
{
    public bool IsReady { get; set; }
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
    public string HandoffSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
