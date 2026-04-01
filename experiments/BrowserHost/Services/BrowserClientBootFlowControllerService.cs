namespace BrowserHost.Services;

public interface IBrowserClientBootFlowController
{
    ValueTask<BrowserClientBootFlowControllerResult> ControlAsync(string profileId = "default");
}

public sealed class BrowserClientBootFlowControllerService : IBrowserClientBootFlowController
{
    private readonly IBrowserClientRuntimeStartupOrchestrator _runtimeStartupOrchestrator;

    public BrowserClientBootFlowControllerService(IBrowserClientRuntimeStartupOrchestrator runtimeStartupOrchestrator)
    {
        _runtimeStartupOrchestrator = runtimeStartupOrchestrator;
    }

    public async ValueTask<BrowserClientBootFlowControllerResult> ControlAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupOrchestratorResult orchestrator = await _runtimeStartupOrchestrator.OrchestrateAsync(profileId);

        BrowserClientBootFlowControllerResult result = new()
        {
            ProfileId = orchestrator.ProfileId,
            SessionId = orchestrator.SessionId,
            SessionPath = orchestrator.SessionPath,
            OrchestratorVersion = orchestrator.OrchestratorVersion,
            CoordinatorVersion = orchestrator.CoordinatorVersion,
            ControllerVersion = orchestrator.ControllerVersion,
            DispatcherVersion = orchestrator.DispatcherVersion,
            DriverVersion = orchestrator.DriverVersion,
            StateMachineVersion = orchestrator.StateMachineVersion,
            StateVersion = orchestrator.StateVersion,
            CycleVersion = orchestrator.CycleVersion,
            InvocationVersion = orchestrator.InvocationVersion,
            LoopVersion = orchestrator.LoopVersion,
            RunnerVersion = orchestrator.RunnerVersion,
            ExecutionVersion = orchestrator.ExecutionVersion,
            HandshakeVersion = orchestrator.HandshakeVersion,
            PacketVersion = orchestrator.PacketVersion,
            ContractVersion = orchestrator.ContractVersion,
            Exists = orchestrator.Exists,
            ReadSucceeded = orchestrator.ReadSucceeded
        };

        if (!orchestrator.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser boot flow controller blocked for profile '{orchestrator.ProfileId}'.";
            result.Error = orchestrator.Error;
            return result;
        }

        result.IsReady = true;
        result.BootFlowVersion = "browser-boot-flow-controller-v1";
        result.LaunchMode = orchestrator.LaunchMode;
        result.AssetRootPath = orchestrator.AssetRootPath;
        result.ProfilesRootPath = orchestrator.ProfilesRootPath;
        result.CacheRootPath = orchestrator.CacheRootPath;
        result.ConfigRootPath = orchestrator.ConfigRootPath;
        result.SettingsFilePath = orchestrator.SettingsFilePath;
        result.StartupProfilePath = orchestrator.StartupProfilePath;
        result.RequiredAssets = orchestrator.RequiredAssets;
        result.ReadyAssetCount = orchestrator.ReadyAssetCount;
        result.CompletedSteps = orchestrator.CompletedSteps;
        result.TotalSteps = orchestrator.TotalSteps;
        result.Phases = orchestrator.Phases;
        result.CurrentState = orchestrator.CurrentState;
        result.States = orchestrator.States;
        result.Transitions = orchestrator.Transitions;
        result.DispatchTargets = orchestrator.DispatchTargets;
        result.ControlActions = orchestrator.ControlActions;
        result.CoordinatorSteps = orchestrator.CoordinatorSteps;
        result.OrchestrationStages = orchestrator.OrchestrationStages;
        result.BootFlowStages =
        [
            "browser-bootstrap",
            "runtime-bootstrap",
            "client-ready"
        ];
        result.BootFlowSummary = $"Browser boot flow controller prepared {result.BootFlowStages.Length} boot-flow stage(s) for profile '{orchestrator.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Browser boot flow controller ready for profile '{orchestrator.ProfileId}' with {result.BootFlowStages.Length} boot-flow stage(s).";

        return result;
    }
}

public sealed class BrowserClientBootFlowControllerResult
{
    public bool IsReady { get; set; }
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
    public string BootFlowSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
