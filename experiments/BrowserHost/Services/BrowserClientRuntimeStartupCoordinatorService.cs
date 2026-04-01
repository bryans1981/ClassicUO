namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupCoordinator
{
    ValueTask<BrowserClientRuntimeStartupCoordinatorResult> CoordinateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupCoordinatorService : IBrowserClientRuntimeStartupCoordinator
{
    private readonly IBrowserClientRuntimeStartupSessionController _runtimeStartupSessionController;

    public BrowserClientRuntimeStartupCoordinatorService(IBrowserClientRuntimeStartupSessionController runtimeStartupSessionController)
    {
        _runtimeStartupSessionController = runtimeStartupSessionController;
    }

    public async ValueTask<BrowserClientRuntimeStartupCoordinatorResult> CoordinateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupSessionControllerResult controller = await _runtimeStartupSessionController.ControlAsync(profileId);

        BrowserClientRuntimeStartupCoordinatorResult result = new()
        {
            ProfileId = controller.ProfileId,
            SessionId = controller.SessionId,
            SessionPath = controller.SessionPath,
            ControllerVersion = controller.ControllerVersion,
            DispatcherVersion = controller.DispatcherVersion,
            DriverVersion = controller.DriverVersion,
            StateMachineVersion = controller.StateMachineVersion,
            StateVersion = controller.StateVersion,
            CycleVersion = controller.CycleVersion,
            InvocationVersion = controller.InvocationVersion,
            LoopVersion = controller.LoopVersion,
            RunnerVersion = controller.RunnerVersion,
            ExecutionVersion = controller.ExecutionVersion,
            HandshakeVersion = controller.HandshakeVersion,
            PacketVersion = controller.PacketVersion,
            ContractVersion = controller.ContractVersion,
            Exists = controller.Exists,
            ReadSucceeded = controller.ReadSucceeded
        };

        if (!controller.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup coordinator blocked for profile '{controller.ProfileId}'.";
            result.Error = controller.Error;
            return result;
        }

        result.IsReady = true;
        result.CoordinatorVersion = "runtime-startup-coordinator-v1";
        result.LaunchMode = controller.LaunchMode;
        result.AssetRootPath = controller.AssetRootPath;
        result.ProfilesRootPath = controller.ProfilesRootPath;
        result.CacheRootPath = controller.CacheRootPath;
        result.ConfigRootPath = controller.ConfigRootPath;
        result.SettingsFilePath = controller.SettingsFilePath;
        result.StartupProfilePath = controller.StartupProfilePath;
        result.RequiredAssets = controller.RequiredAssets;
        result.ReadyAssetCount = controller.ReadyAssetCount;
        result.CompletedSteps = controller.CompletedSteps;
        result.TotalSteps = controller.TotalSteps;
        result.Phases = controller.Phases;
        result.CurrentState = controller.CurrentState;
        result.States = controller.States;
        result.Transitions = controller.Transitions;
        result.DispatchTargets = controller.DispatchTargets;
        result.ControlActions = controller.ControlActions;
        result.CoordinatorSteps =
        [
            "prepare-bootstrap-context",
            "coordinate-session-control",
            "confirm-runtime-handoff"
        ];
        result.CoordinatorSummary = $"Runtime startup coordinator prepared {result.CoordinatorSteps.Length} coordination step(s) for profile '{controller.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup coordinator ready for profile '{controller.ProfileId}' with {result.CoordinatorSteps.Length} coordination step(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupCoordinatorResult
{
    public bool IsReady { get; set; }
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
    public string CoordinatorSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
