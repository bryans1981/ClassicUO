namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupOrchestrator
{
    ValueTask<BrowserClientRuntimeStartupOrchestratorResult> OrchestrateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupOrchestratorService : IBrowserClientRuntimeStartupOrchestrator
{
    private readonly IBrowserClientRuntimeStartupCoordinator _runtimeStartupCoordinator;

    public BrowserClientRuntimeStartupOrchestratorService(IBrowserClientRuntimeStartupCoordinator runtimeStartupCoordinator)
    {
        _runtimeStartupCoordinator = runtimeStartupCoordinator;
    }

    public async ValueTask<BrowserClientRuntimeStartupOrchestratorResult> OrchestrateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupCoordinatorResult coordinator = await _runtimeStartupCoordinator.CoordinateAsync(profileId);

        BrowserClientRuntimeStartupOrchestratorResult result = new()
        {
            ProfileId = coordinator.ProfileId,
            SessionId = coordinator.SessionId,
            SessionPath = coordinator.SessionPath,
            CoordinatorVersion = coordinator.CoordinatorVersion,
            ControllerVersion = coordinator.ControllerVersion,
            DispatcherVersion = coordinator.DispatcherVersion,
            DriverVersion = coordinator.DriverVersion,
            StateMachineVersion = coordinator.StateMachineVersion,
            StateVersion = coordinator.StateVersion,
            CycleVersion = coordinator.CycleVersion,
            InvocationVersion = coordinator.InvocationVersion,
            LoopVersion = coordinator.LoopVersion,
            RunnerVersion = coordinator.RunnerVersion,
            ExecutionVersion = coordinator.ExecutionVersion,
            HandshakeVersion = coordinator.HandshakeVersion,
            PacketVersion = coordinator.PacketVersion,
            ContractVersion = coordinator.ContractVersion,
            Exists = coordinator.Exists,
            ReadSucceeded = coordinator.ReadSucceeded
        };

        if (!coordinator.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup orchestrator blocked for profile '{coordinator.ProfileId}'.";
            result.Error = coordinator.Error;
            return result;
        }

        result.IsReady = true;
        result.OrchestratorVersion = "runtime-startup-orchestrator-v1";
        result.LaunchMode = coordinator.LaunchMode;
        result.AssetRootPath = coordinator.AssetRootPath;
        result.ProfilesRootPath = coordinator.ProfilesRootPath;
        result.CacheRootPath = coordinator.CacheRootPath;
        result.ConfigRootPath = coordinator.ConfigRootPath;
        result.SettingsFilePath = coordinator.SettingsFilePath;
        result.StartupProfilePath = coordinator.StartupProfilePath;
        result.RequiredAssets = coordinator.RequiredAssets;
        result.ReadyAssetCount = coordinator.ReadyAssetCount;
        result.CompletedSteps = coordinator.CompletedSteps;
        result.TotalSteps = coordinator.TotalSteps;
        result.Phases = coordinator.Phases;
        result.CurrentState = coordinator.CurrentState;
        result.States = coordinator.States;
        result.Transitions = coordinator.Transitions;
        result.DispatchTargets = coordinator.DispatchTargets;
        result.ControlActions = coordinator.ControlActions;
        result.CoordinatorSteps = coordinator.CoordinatorSteps;
        result.OrchestrationStages =
        [
            "bootstrap-assets-ready",
            "startup-session-bound",
            "runtime-handoff-complete"
        ];
        result.OrchestratorSummary = $"Runtime startup orchestrator prepared {result.OrchestrationStages.Length} orchestration stage(s) for profile '{coordinator.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup orchestrator ready for profile '{coordinator.ProfileId}' with {result.OrchestrationStages.Length} orchestration stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupOrchestratorResult
{
    public bool IsReady { get; set; }
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
    public string OrchestratorSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
