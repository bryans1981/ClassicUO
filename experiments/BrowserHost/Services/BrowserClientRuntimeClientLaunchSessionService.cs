namespace BrowserHost.Services;

public interface IBrowserClientRuntimeClientLaunchSession
{
    ValueTask<BrowserClientRuntimeClientLaunchSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeClientLaunchSessionService : IBrowserClientRuntimeClientLaunchSession
{
    private readonly IBrowserClientRuntimeClientReadyState _runtimeClientReadyState;

    public BrowserClientRuntimeClientLaunchSessionService(IBrowserClientRuntimeClientReadyState runtimeClientReadyState)
    {
        _runtimeClientReadyState = runtimeClientReadyState;
    }

    public async ValueTask<BrowserClientRuntimeClientLaunchSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeClientReadyStateResult readyState = await _runtimeClientReadyState.BuildAsync(profileId);

        BrowserClientRuntimeClientLaunchSessionResult result = new()
        {
            ProfileId = readyState.ProfileId,
            SessionId = readyState.SessionId,
            SessionPath = readyState.SessionPath,
            ClientReadyStateVersion = readyState.ClientReadyStateVersion,
            LaunchControllerVersion = readyState.LaunchControllerVersion,
            ReadySignalVersion = readyState.ReadySignalVersion,
            ReadinessGateVersion = readyState.ReadinessGateVersion,
            ClientBootstrapVersion = readyState.ClientBootstrapVersion,
            RuntimeSessionVersion = readyState.RuntimeSessionVersion,
            ConsumerVersion = readyState.ConsumerVersion,
            HandoffVersion = readyState.HandoffVersion,
            BootSessionVersion = readyState.BootSessionVersion,
            BootFlowVersion = readyState.BootFlowVersion,
            OrchestratorVersion = readyState.OrchestratorVersion,
            CoordinatorVersion = readyState.CoordinatorVersion,
            ControllerVersion = readyState.ControllerVersion,
            DispatcherVersion = readyState.DispatcherVersion,
            DriverVersion = readyState.DriverVersion,
            StateMachineVersion = readyState.StateMachineVersion,
            StateVersion = readyState.StateVersion,
            CycleVersion = readyState.CycleVersion,
            InvocationVersion = readyState.InvocationVersion,
            LoopVersion = readyState.LoopVersion,
            RunnerVersion = readyState.RunnerVersion,
            ExecutionVersion = readyState.ExecutionVersion,
            HandshakeVersion = readyState.HandshakeVersion,
            PacketVersion = readyState.PacketVersion,
            ContractVersion = readyState.ContractVersion,
            Exists = readyState.Exists,
            ReadSucceeded = readyState.ReadSucceeded
        };

        if (!readyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime client launch session blocked for profile '{readyState.ProfileId}'.";
            result.Error = readyState.Error;
            return result;
        }

        result.IsReady = true;
        result.ClientLaunchSessionVersion = "runtime-client-launch-session-v1";
        result.LaunchMode = readyState.LaunchMode;
        result.AssetRootPath = readyState.AssetRootPath;
        result.ProfilesRootPath = readyState.ProfilesRootPath;
        result.CacheRootPath = readyState.CacheRootPath;
        result.ConfigRootPath = readyState.ConfigRootPath;
        result.SettingsFilePath = readyState.SettingsFilePath;
        result.StartupProfilePath = readyState.StartupProfilePath;
        result.RequiredAssets = readyState.RequiredAssets;
        result.ReadyAssetCount = readyState.ReadyAssetCount;
        result.CompletedSteps = readyState.CompletedSteps;
        result.TotalSteps = readyState.TotalSteps;
        result.Phases = readyState.Phases;
        result.CurrentState = readyState.CurrentState;
        result.States = readyState.States;
        result.Transitions = readyState.Transitions;
        result.DispatchTargets = readyState.DispatchTargets;
        result.ControlActions = readyState.ControlActions;
        result.CoordinatorSteps = readyState.CoordinatorSteps;
        result.OrchestrationStages = readyState.OrchestrationStages;
        result.BootFlowStages = readyState.BootFlowStages;
        result.BootSessionStages = readyState.BootSessionStages;
        result.HandoffArtifacts = readyState.HandoffArtifacts;
        result.ConsumedArtifacts = readyState.ConsumedArtifacts;
        result.RuntimeSessionStages = readyState.RuntimeSessionStages;
        result.ClientBootstrapActions = readyState.ClientBootstrapActions;
        result.ReadinessChecks = readyState.ReadinessChecks;
        result.ReadySignals = readyState.ReadySignals;
        result.LaunchControlActions = readyState.LaunchControlActions;
        result.ReadyStates = readyState.ReadyStates;
        result.ClientLaunchStages =
        [
            "open-client-launch-session",
            "bind-client-ready-state",
            "publish-client-launch-session-ready"
        ];
        result.ClientLaunchSummary = $"Runtime client launch session prepared {result.ClientLaunchStages.Length} launch session stage(s) for profile '{readyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime client launch session ready for profile '{readyState.ProfileId}' with {result.ClientLaunchStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeClientLaunchSessionResult
{
    public bool IsReady { get; set; }
    public string ClientLaunchSessionVersion { get; set; } = string.Empty;
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
    public string[] ClientLaunchStages { get; set; } = Array.Empty<string>();
    public string ClientLaunchSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
