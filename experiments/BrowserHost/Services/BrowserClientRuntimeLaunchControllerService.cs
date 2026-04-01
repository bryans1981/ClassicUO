namespace BrowserHost.Services;

public interface IBrowserClientRuntimeLaunchController
{
    ValueTask<BrowserClientRuntimeLaunchControllerResult> ControlAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeLaunchControllerService : IBrowserClientRuntimeLaunchController
{
    private readonly IBrowserClientRuntimeReadySignal _runtimeReadySignal;

    public BrowserClientRuntimeLaunchControllerService(IBrowserClientRuntimeReadySignal runtimeReadySignal)
    {
        _runtimeReadySignal = runtimeReadySignal;
    }

    public async ValueTask<BrowserClientRuntimeLaunchControllerResult> ControlAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeReadySignalResult readySignal = await _runtimeReadySignal.SignalAsync(profileId);

        BrowserClientRuntimeLaunchControllerResult result = new()
        {
            ProfileId = readySignal.ProfileId,
            SessionId = readySignal.SessionId,
            SessionPath = readySignal.SessionPath,
            ReadySignalVersion = readySignal.ReadySignalVersion,
            ReadinessGateVersion = readySignal.ReadinessGateVersion,
            ClientBootstrapVersion = readySignal.ClientBootstrapVersion,
            RuntimeSessionVersion = readySignal.RuntimeSessionVersion,
            ConsumerVersion = readySignal.ConsumerVersion,
            HandoffVersion = readySignal.HandoffVersion,
            BootSessionVersion = readySignal.BootSessionVersion,
            BootFlowVersion = readySignal.BootFlowVersion,
            OrchestratorVersion = readySignal.OrchestratorVersion,
            CoordinatorVersion = readySignal.CoordinatorVersion,
            ControllerVersion = readySignal.ControllerVersion,
            DispatcherVersion = readySignal.DispatcherVersion,
            DriverVersion = readySignal.DriverVersion,
            StateMachineVersion = readySignal.StateMachineVersion,
            StateVersion = readySignal.StateVersion,
            CycleVersion = readySignal.CycleVersion,
            InvocationVersion = readySignal.InvocationVersion,
            LoopVersion = readySignal.LoopVersion,
            RunnerVersion = readySignal.RunnerVersion,
            ExecutionVersion = readySignal.ExecutionVersion,
            HandshakeVersion = readySignal.HandshakeVersion,
            PacketVersion = readySignal.PacketVersion,
            ContractVersion = readySignal.ContractVersion,
            Exists = readySignal.Exists,
            ReadSucceeded = readySignal.ReadSucceeded
        };

        if (!readySignal.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime launch controller blocked for profile '{readySignal.ProfileId}'.";
            result.Error = readySignal.Error;
            return result;
        }

        result.IsReady = true;
        result.LaunchControllerVersion = "runtime-launch-controller-v1";
        result.LaunchMode = readySignal.LaunchMode;
        result.AssetRootPath = readySignal.AssetRootPath;
        result.ProfilesRootPath = readySignal.ProfilesRootPath;
        result.CacheRootPath = readySignal.CacheRootPath;
        result.ConfigRootPath = readySignal.ConfigRootPath;
        result.SettingsFilePath = readySignal.SettingsFilePath;
        result.StartupProfilePath = readySignal.StartupProfilePath;
        result.RequiredAssets = readySignal.RequiredAssets;
        result.ReadyAssetCount = readySignal.ReadyAssetCount;
        result.CompletedSteps = readySignal.CompletedSteps;
        result.TotalSteps = readySignal.TotalSteps;
        result.Phases = readySignal.Phases;
        result.CurrentState = readySignal.CurrentState;
        result.States = readySignal.States;
        result.Transitions = readySignal.Transitions;
        result.DispatchTargets = readySignal.DispatchTargets;
        result.ControlActions = readySignal.ControlActions;
        result.CoordinatorSteps = readySignal.CoordinatorSteps;
        result.OrchestrationStages = readySignal.OrchestrationStages;
        result.BootFlowStages = readySignal.BootFlowStages;
        result.BootSessionStages = readySignal.BootSessionStages;
        result.HandoffArtifacts = readySignal.HandoffArtifacts;
        result.ConsumedArtifacts = readySignal.ConsumedArtifacts;
        result.RuntimeSessionStages = readySignal.RuntimeSessionStages;
        result.ClientBootstrapActions = readySignal.ClientBootstrapActions;
        result.ReadinessChecks = readySignal.ReadinessChecks;
        result.ReadySignals = readySignal.ReadySignals;
        result.LaunchControlActions =
        [
            "bind-runtime-ready-signal",
            "prepare-client-launch",
            "publish-launch-ready"
        ];
        result.LaunchControlSummary = $"Runtime launch controller prepared {result.LaunchControlActions.Length} launch control action(s) for profile '{readySignal.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime launch controller ready for profile '{readySignal.ProfileId}' with {result.LaunchControlActions.Length} action(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeLaunchControllerResult
{
    public bool IsReady { get; set; }
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
    public string LaunchControlSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
