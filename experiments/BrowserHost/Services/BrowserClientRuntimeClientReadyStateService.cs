namespace BrowserHost.Services;

public interface IBrowserClientRuntimeClientReadyState
{
    ValueTask<BrowserClientRuntimeClientReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeClientReadyStateService : IBrowserClientRuntimeClientReadyState
{
    private readonly IBrowserClientRuntimeLaunchController _runtimeLaunchController;

    public BrowserClientRuntimeClientReadyStateService(IBrowserClientRuntimeLaunchController runtimeLaunchController)
    {
        _runtimeLaunchController = runtimeLaunchController;
    }

    public async ValueTask<BrowserClientRuntimeClientReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeLaunchControllerResult launchController = await _runtimeLaunchController.ControlAsync(profileId);

        BrowserClientRuntimeClientReadyStateResult result = new()
        {
            ProfileId = launchController.ProfileId,
            SessionId = launchController.SessionId,
            SessionPath = launchController.SessionPath,
            LaunchControllerVersion = launchController.LaunchControllerVersion,
            ReadySignalVersion = launchController.ReadySignalVersion,
            ReadinessGateVersion = launchController.ReadinessGateVersion,
            ClientBootstrapVersion = launchController.ClientBootstrapVersion,
            RuntimeSessionVersion = launchController.RuntimeSessionVersion,
            ConsumerVersion = launchController.ConsumerVersion,
            HandoffVersion = launchController.HandoffVersion,
            BootSessionVersion = launchController.BootSessionVersion,
            BootFlowVersion = launchController.BootFlowVersion,
            OrchestratorVersion = launchController.OrchestratorVersion,
            CoordinatorVersion = launchController.CoordinatorVersion,
            ControllerVersion = launchController.ControllerVersion,
            DispatcherVersion = launchController.DispatcherVersion,
            DriverVersion = launchController.DriverVersion,
            StateMachineVersion = launchController.StateMachineVersion,
            StateVersion = launchController.StateVersion,
            CycleVersion = launchController.CycleVersion,
            InvocationVersion = launchController.InvocationVersion,
            LoopVersion = launchController.LoopVersion,
            RunnerVersion = launchController.RunnerVersion,
            ExecutionVersion = launchController.ExecutionVersion,
            HandshakeVersion = launchController.HandshakeVersion,
            PacketVersion = launchController.PacketVersion,
            ContractVersion = launchController.ContractVersion,
            Exists = launchController.Exists,
            ReadSucceeded = launchController.ReadSucceeded
        };

        if (!launchController.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime client ready state blocked for profile '{launchController.ProfileId}'.";
            result.Error = launchController.Error;
            return result;
        }

        result.IsReady = true;
        result.ClientReadyStateVersion = "runtime-client-ready-state-v1";
        result.LaunchMode = launchController.LaunchMode;
        result.AssetRootPath = launchController.AssetRootPath;
        result.ProfilesRootPath = launchController.ProfilesRootPath;
        result.CacheRootPath = launchController.CacheRootPath;
        result.ConfigRootPath = launchController.ConfigRootPath;
        result.SettingsFilePath = launchController.SettingsFilePath;
        result.StartupProfilePath = launchController.StartupProfilePath;
        result.RequiredAssets = launchController.RequiredAssets;
        result.ReadyAssetCount = launchController.ReadyAssetCount;
        result.CompletedSteps = launchController.CompletedSteps;
        result.TotalSteps = launchController.TotalSteps;
        result.Phases = launchController.Phases;
        result.CurrentState = launchController.CurrentState;
        result.States = launchController.States;
        result.Transitions = launchController.Transitions;
        result.DispatchTargets = launchController.DispatchTargets;
        result.ControlActions = launchController.ControlActions;
        result.CoordinatorSteps = launchController.CoordinatorSteps;
        result.OrchestrationStages = launchController.OrchestrationStages;
        result.BootFlowStages = launchController.BootFlowStages;
        result.BootSessionStages = launchController.BootSessionStages;
        result.HandoffArtifacts = launchController.HandoffArtifacts;
        result.ConsumedArtifacts = launchController.ConsumedArtifacts;
        result.RuntimeSessionStages = launchController.RuntimeSessionStages;
        result.ClientBootstrapActions = launchController.ClientBootstrapActions;
        result.ReadinessChecks = launchController.ReadinessChecks;
        result.ReadySignals = launchController.ReadySignals;
        result.LaunchControlActions = launchController.LaunchControlActions;
        result.ReadyStates =
        [
            "assets-bound",
            "browser-session-bound",
            "client-ready"
        ];
        result.ReadyStateSummary = $"Runtime client ready state prepared {result.ReadyStates.Length} ready-state marker(s) for profile '{launchController.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime client ready state ready for profile '{launchController.ProfileId}' with {result.ReadyStates.Length} marker(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeClientReadyStateResult
{
    public bool IsReady { get; set; }
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
    public string ReadyStateSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
