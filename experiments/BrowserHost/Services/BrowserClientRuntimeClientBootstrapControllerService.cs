namespace BrowserHost.Services;

public interface IBrowserClientRuntimeClientBootstrapController
{
    ValueTask<BrowserClientRuntimeClientBootstrapControllerResult> ControlAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeClientBootstrapControllerService : IBrowserClientRuntimeClientBootstrapController
{
    private readonly IBrowserClientRuntimeBootstrapSession _runtimeBootstrapSession;

    public BrowserClientRuntimeClientBootstrapControllerService(IBrowserClientRuntimeBootstrapSession runtimeBootstrapSession)
    {
        _runtimeBootstrapSession = runtimeBootstrapSession;
    }

    public async ValueTask<BrowserClientRuntimeClientBootstrapControllerResult> ControlAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBootstrapSessionResult runtimeSession = await _runtimeBootstrapSession.CreateAsync(profileId);

        BrowserClientRuntimeClientBootstrapControllerResult result = new()
        {
            ProfileId = runtimeSession.ProfileId,
            SessionId = runtimeSession.SessionId,
            SessionPath = runtimeSession.SessionPath,
            RuntimeSessionVersion = runtimeSession.RuntimeSessionVersion,
            ConsumerVersion = runtimeSession.ConsumerVersion,
            HandoffVersion = runtimeSession.HandoffVersion,
            BootSessionVersion = runtimeSession.BootSessionVersion,
            BootFlowVersion = runtimeSession.BootFlowVersion,
            OrchestratorVersion = runtimeSession.OrchestratorVersion,
            CoordinatorVersion = runtimeSession.CoordinatorVersion,
            ControllerVersion = runtimeSession.ControllerVersion,
            DispatcherVersion = runtimeSession.DispatcherVersion,
            DriverVersion = runtimeSession.DriverVersion,
            StateMachineVersion = runtimeSession.StateMachineVersion,
            StateVersion = runtimeSession.StateVersion,
            CycleVersion = runtimeSession.CycleVersion,
            InvocationVersion = runtimeSession.InvocationVersion,
            LoopVersion = runtimeSession.LoopVersion,
            RunnerVersion = runtimeSession.RunnerVersion,
            ExecutionVersion = runtimeSession.ExecutionVersion,
            HandshakeVersion = runtimeSession.HandshakeVersion,
            PacketVersion = runtimeSession.PacketVersion,
            ContractVersion = runtimeSession.ContractVersion,
            Exists = runtimeSession.Exists,
            ReadSucceeded = runtimeSession.ReadSucceeded
        };

        if (!runtimeSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime client bootstrap controller blocked for profile '{runtimeSession.ProfileId}'.";
            result.Error = runtimeSession.Error;
            return result;
        }

        result.IsReady = true;
        result.ClientBootstrapVersion = "runtime-client-bootstrap-controller-v1";
        result.LaunchMode = runtimeSession.LaunchMode;
        result.AssetRootPath = runtimeSession.AssetRootPath;
        result.ProfilesRootPath = runtimeSession.ProfilesRootPath;
        result.CacheRootPath = runtimeSession.CacheRootPath;
        result.ConfigRootPath = runtimeSession.ConfigRootPath;
        result.SettingsFilePath = runtimeSession.SettingsFilePath;
        result.StartupProfilePath = runtimeSession.StartupProfilePath;
        result.RequiredAssets = runtimeSession.RequiredAssets;
        result.ReadyAssetCount = runtimeSession.ReadyAssetCount;
        result.CompletedSteps = runtimeSession.CompletedSteps;
        result.TotalSteps = runtimeSession.TotalSteps;
        result.Phases = runtimeSession.Phases;
        result.CurrentState = runtimeSession.CurrentState;
        result.States = runtimeSession.States;
        result.Transitions = runtimeSession.Transitions;
        result.DispatchTargets = runtimeSession.DispatchTargets;
        result.ControlActions = runtimeSession.ControlActions;
        result.CoordinatorSteps = runtimeSession.CoordinatorSteps;
        result.OrchestrationStages = runtimeSession.OrchestrationStages;
        result.BootFlowStages = runtimeSession.BootFlowStages;
        result.BootSessionStages = runtimeSession.BootSessionStages;
        result.HandoffArtifacts = runtimeSession.HandoffArtifacts;
        result.ConsumedArtifacts = runtimeSession.ConsumedArtifacts;
        result.RuntimeSessionStages = runtimeSession.RuntimeSessionStages;
        result.ClientBootstrapActions =
        [
            "bind-client-runtime-state",
            "attach-browser-session-context",
            "publish-client-bootstrap-ready"
        ];
        result.ClientBootstrapSummary = $"Runtime client bootstrap controller prepared {result.ClientBootstrapActions.Length} client bootstrap action(s) for profile '{runtimeSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime client bootstrap controller ready for profile '{runtimeSession.ProfileId}' with {result.ClientBootstrapActions.Length} action(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeClientBootstrapControllerResult
{
    public bool IsReady { get; set; }
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
    public string ClientBootstrapSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
