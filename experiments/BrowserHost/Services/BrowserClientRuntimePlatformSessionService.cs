namespace BrowserHost.Services;

public interface IBrowserClientRuntimePlatformSession
{
    ValueTask<BrowserClientRuntimePlatformSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimePlatformSessionService : IBrowserClientRuntimePlatformSession
{
    private readonly IBrowserClientRuntimeHostReadyState _runtimeHostReadyState;

    public BrowserClientRuntimePlatformSessionService(IBrowserClientRuntimeHostReadyState runtimeHostReadyState)
    {
        _runtimeHostReadyState = runtimeHostReadyState;
    }

    public async ValueTask<BrowserClientRuntimePlatformSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeHostReadyStateResult hostReadyState = await _runtimeHostReadyState.BuildAsync(profileId);

        BrowserClientRuntimePlatformSessionResult result = new()
        {
            ProfileId = hostReadyState.ProfileId,
            SessionId = hostReadyState.SessionId,
            SessionPath = hostReadyState.SessionPath,
            HostReadyStateVersion = hostReadyState.HostReadyStateVersion,
            HostLoopVersion = hostReadyState.HostLoopVersion,
            HostSessionVersion = hostReadyState.HostSessionVersion,
            ClientLoopStateVersion = hostReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = hostReadyState.ClientRunStateVersion,
            ClientActivationVersion = hostReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = hostReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = hostReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = hostReadyState.LaunchControllerVersion,
            ReadySignalVersion = hostReadyState.ReadySignalVersion,
            ReadinessGateVersion = hostReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = hostReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = hostReadyState.RuntimeSessionVersion,
            ConsumerVersion = hostReadyState.ConsumerVersion,
            HandoffVersion = hostReadyState.HandoffVersion,
            BootSessionVersion = hostReadyState.BootSessionVersion,
            BootFlowVersion = hostReadyState.BootFlowVersion,
            OrchestratorVersion = hostReadyState.OrchestratorVersion,
            CoordinatorVersion = hostReadyState.CoordinatorVersion,
            ControllerVersion = hostReadyState.ControllerVersion,
            DispatcherVersion = hostReadyState.DispatcherVersion,
            DriverVersion = hostReadyState.DriverVersion,
            StateMachineVersion = hostReadyState.StateMachineVersion,
            StateVersion = hostReadyState.StateVersion,
            CycleVersion = hostReadyState.CycleVersion,
            InvocationVersion = hostReadyState.InvocationVersion,
            LoopVersion = hostReadyState.LoopVersion,
            RunnerVersion = hostReadyState.RunnerVersion,
            ExecutionVersion = hostReadyState.ExecutionVersion,
            HandshakeVersion = hostReadyState.HandshakeVersion,
            PacketVersion = hostReadyState.PacketVersion,
            ContractVersion = hostReadyState.ContractVersion,
            Exists = hostReadyState.Exists,
            ReadSucceeded = hostReadyState.ReadSucceeded
        };

        if (!hostReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime platform session blocked for profile '{hostReadyState.ProfileId}'.";
            result.Error = hostReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.PlatformSessionVersion = "runtime-platform-session-v1";
        result.LaunchMode = hostReadyState.LaunchMode;
        result.AssetRootPath = hostReadyState.AssetRootPath;
        result.ProfilesRootPath = hostReadyState.ProfilesRootPath;
        result.CacheRootPath = hostReadyState.CacheRootPath;
        result.ConfigRootPath = hostReadyState.ConfigRootPath;
        result.SettingsFilePath = hostReadyState.SettingsFilePath;
        result.StartupProfilePath = hostReadyState.StartupProfilePath;
        result.RequiredAssets = hostReadyState.RequiredAssets;
        result.ReadyAssetCount = hostReadyState.ReadyAssetCount;
        result.CompletedSteps = hostReadyState.CompletedSteps;
        result.TotalSteps = hostReadyState.TotalSteps;
        result.Phases = hostReadyState.Phases;
        result.CurrentState = hostReadyState.CurrentState;
        result.States = hostReadyState.States;
        result.Transitions = hostReadyState.Transitions;
        result.DispatchTargets = hostReadyState.DispatchTargets;
        result.ControlActions = hostReadyState.ControlActions;
        result.CoordinatorSteps = hostReadyState.CoordinatorSteps;
        result.OrchestrationStages = hostReadyState.OrchestrationStages;
        result.BootFlowStages = hostReadyState.BootFlowStages;
        result.BootSessionStages = hostReadyState.BootSessionStages;
        result.HandoffArtifacts = hostReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = hostReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = hostReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = hostReadyState.ClientBootstrapActions;
        result.ReadinessChecks = hostReadyState.ReadinessChecks;
        result.ReadySignals = hostReadyState.ReadySignals;
        result.LaunchControlActions = hostReadyState.LaunchControlActions;
        result.ReadyStates = hostReadyState.ReadyStates;
        result.ClientLaunchStages = hostReadyState.ClientLaunchStages;
        result.ActivationSteps = hostReadyState.ActivationSteps;
        result.RunStateStages = hostReadyState.RunStateStages;
        result.ClientLoopStages = hostReadyState.ClientLoopStages;
        result.HostSessionStages = hostReadyState.HostSessionStages;
        result.HostLoopStages = hostReadyState.HostLoopStages;
        result.HostReadyChecks = hostReadyState.HostReadyChecks;
        result.PlatformSessionStages =
        [
            "open-platform-session",
            "bind-host-ready-state",
            "publish-platform-session-ready"
        ];
        result.PlatformSessionSummary = $"Runtime platform session prepared {result.PlatformSessionStages.Length} platform-session stage(s) for profile '{hostReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime platform session ready for profile '{hostReadyState.ProfileId}' with {result.PlatformSessionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimePlatformSessionResult
{
    public bool IsReady { get; set; }
    public string PlatformSessionVersion { get; set; } = string.Empty;
    public string HostReadyStateVersion { get; set; } = string.Empty;
    public string HostLoopVersion { get; set; } = string.Empty;
    public string HostSessionVersion { get; set; } = string.Empty;
    public string ClientLoopStateVersion { get; set; } = string.Empty;
    public string ClientRunStateVersion { get; set; } = string.Empty;
    public string ClientActivationVersion { get; set; } = string.Empty;
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
    public string[] ActivationSteps { get; set; } = Array.Empty<string>();
    public string[] RunStateStages { get; set; } = Array.Empty<string>();
    public string[] ClientLoopStages { get; set; } = Array.Empty<string>();
    public string[] HostSessionStages { get; set; } = Array.Empty<string>();
    public string[] HostLoopStages { get; set; } = Array.Empty<string>();
    public string[] HostReadyChecks { get; set; } = Array.Empty<string>();
    public string[] PlatformSessionStages { get; set; } = Array.Empty<string>();
    public string PlatformSessionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
