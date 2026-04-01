namespace BrowserHost.Services;

public interface IBrowserClientBootSession
{
    ValueTask<BrowserClientBootSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientBootSessionService : IBrowserClientBootSession
{
    private readonly IBrowserClientBootFlowController _bootFlowController;

    public BrowserClientBootSessionService(IBrowserClientBootFlowController bootFlowController)
    {
        _bootFlowController = bootFlowController;
    }

    public async ValueTask<BrowserClientBootSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientBootFlowControllerResult bootFlow = await _bootFlowController.ControlAsync(profileId);

        BrowserClientBootSessionResult result = new()
        {
            ProfileId = bootFlow.ProfileId,
            SessionId = bootFlow.SessionId,
            SessionPath = bootFlow.SessionPath,
            BootFlowVersion = bootFlow.BootFlowVersion,
            OrchestratorVersion = bootFlow.OrchestratorVersion,
            CoordinatorVersion = bootFlow.CoordinatorVersion,
            ControllerVersion = bootFlow.ControllerVersion,
            DispatcherVersion = bootFlow.DispatcherVersion,
            DriverVersion = bootFlow.DriverVersion,
            StateMachineVersion = bootFlow.StateMachineVersion,
            StateVersion = bootFlow.StateVersion,
            CycleVersion = bootFlow.CycleVersion,
            InvocationVersion = bootFlow.InvocationVersion,
            LoopVersion = bootFlow.LoopVersion,
            RunnerVersion = bootFlow.RunnerVersion,
            ExecutionVersion = bootFlow.ExecutionVersion,
            HandshakeVersion = bootFlow.HandshakeVersion,
            PacketVersion = bootFlow.PacketVersion,
            ContractVersion = bootFlow.ContractVersion,
            Exists = bootFlow.Exists,
            ReadSucceeded = bootFlow.ReadSucceeded
        };

        if (!bootFlow.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser boot session blocked for profile '{bootFlow.ProfileId}'.";
            result.Error = bootFlow.Error;
            return result;
        }

        result.IsReady = true;
        result.BootSessionVersion = "browser-boot-session-v1";
        result.LaunchMode = bootFlow.LaunchMode;
        result.AssetRootPath = bootFlow.AssetRootPath;
        result.ProfilesRootPath = bootFlow.ProfilesRootPath;
        result.CacheRootPath = bootFlow.CacheRootPath;
        result.ConfigRootPath = bootFlow.ConfigRootPath;
        result.SettingsFilePath = bootFlow.SettingsFilePath;
        result.StartupProfilePath = bootFlow.StartupProfilePath;
        result.RequiredAssets = bootFlow.RequiredAssets;
        result.ReadyAssetCount = bootFlow.ReadyAssetCount;
        result.CompletedSteps = bootFlow.CompletedSteps;
        result.TotalSteps = bootFlow.TotalSteps;
        result.Phases = bootFlow.Phases;
        result.CurrentState = bootFlow.CurrentState;
        result.States = bootFlow.States;
        result.Transitions = bootFlow.Transitions;
        result.DispatchTargets = bootFlow.DispatchTargets;
        result.ControlActions = bootFlow.ControlActions;
        result.CoordinatorSteps = bootFlow.CoordinatorSteps;
        result.OrchestrationStages = bootFlow.OrchestrationStages;
        result.BootFlowStages = bootFlow.BootFlowStages;
        result.BootSessionStages =
        [
            "initialize-browser-client",
            "bind-runtime-handoff",
            "open-boot-session"
        ];
        result.BootSessionSummary = $"Browser boot session prepared {result.BootSessionStages.Length} session stage(s) for profile '{bootFlow.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Browser boot session ready for profile '{bootFlow.ProfileId}' with {result.BootSessionStages.Length} session stage(s).";

        return result;
    }
}

public sealed class BrowserClientBootSessionResult
{
    public bool IsReady { get; set; }
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
    public string BootSessionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
