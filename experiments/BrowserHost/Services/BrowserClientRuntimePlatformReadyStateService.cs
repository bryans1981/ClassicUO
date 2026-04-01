namespace BrowserHost.Services;

public interface IBrowserClientRuntimePlatformReadyState
{
    ValueTask<BrowserClientRuntimePlatformReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimePlatformReadyStateService : IBrowserClientRuntimePlatformReadyState
{
    private readonly IBrowserClientRuntimePlatformSession _runtimePlatformSession;

    public BrowserClientRuntimePlatformReadyStateService(IBrowserClientRuntimePlatformSession runtimePlatformSession)
    {
        _runtimePlatformSession = runtimePlatformSession;
    }

    public async ValueTask<BrowserClientRuntimePlatformReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimePlatformSessionResult platformSession = await _runtimePlatformSession.CreateAsync(profileId);

        BrowserClientRuntimePlatformReadyStateResult result = new()
        {
            ProfileId = platformSession.ProfileId,
            SessionId = platformSession.SessionId,
            SessionPath = platformSession.SessionPath,
            PlatformSessionVersion = platformSession.PlatformSessionVersion,
            HostReadyStateVersion = platformSession.HostReadyStateVersion,
            HostLoopVersion = platformSession.HostLoopVersion,
            HostSessionVersion = platformSession.HostSessionVersion,
            ClientLoopStateVersion = platformSession.ClientLoopStateVersion,
            ClientRunStateVersion = platformSession.ClientRunStateVersion,
            ClientActivationVersion = platformSession.ClientActivationVersion,
            ClientLaunchSessionVersion = platformSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = platformSession.ClientReadyStateVersion,
            LaunchControllerVersion = platformSession.LaunchControllerVersion,
            ReadySignalVersion = platformSession.ReadySignalVersion,
            ReadinessGateVersion = platformSession.ReadinessGateVersion,
            ClientBootstrapVersion = platformSession.ClientBootstrapVersion,
            RuntimeSessionVersion = platformSession.RuntimeSessionVersion,
            ConsumerVersion = platformSession.ConsumerVersion,
            HandoffVersion = platformSession.HandoffVersion,
            BootSessionVersion = platformSession.BootSessionVersion,
            BootFlowVersion = platformSession.BootFlowVersion,
            OrchestratorVersion = platformSession.OrchestratorVersion,
            CoordinatorVersion = platformSession.CoordinatorVersion,
            ControllerVersion = platformSession.ControllerVersion,
            DispatcherVersion = platformSession.DispatcherVersion,
            DriverVersion = platformSession.DriverVersion,
            StateMachineVersion = platformSession.StateMachineVersion,
            StateVersion = platformSession.StateVersion,
            CycleVersion = platformSession.CycleVersion,
            InvocationVersion = platformSession.InvocationVersion,
            LoopVersion = platformSession.LoopVersion,
            RunnerVersion = platformSession.RunnerVersion,
            ExecutionVersion = platformSession.ExecutionVersion,
            HandshakeVersion = platformSession.HandshakeVersion,
            PacketVersion = platformSession.PacketVersion,
            ContractVersion = platformSession.ContractVersion,
            Exists = platformSession.Exists,
            ReadSucceeded = platformSession.ReadSucceeded
        };

        if (!platformSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime platform ready state blocked for profile '{platformSession.ProfileId}'.";
            result.Error = platformSession.Error;
            return result;
        }

        result.IsReady = true;
        result.PlatformReadyStateVersion = "runtime-platform-ready-state-v1";
        result.LaunchMode = platformSession.LaunchMode;
        result.AssetRootPath = platformSession.AssetRootPath;
        result.ProfilesRootPath = platformSession.ProfilesRootPath;
        result.CacheRootPath = platformSession.CacheRootPath;
        result.ConfigRootPath = platformSession.ConfigRootPath;
        result.SettingsFilePath = platformSession.SettingsFilePath;
        result.StartupProfilePath = platformSession.StartupProfilePath;
        result.RequiredAssets = platformSession.RequiredAssets;
        result.ReadyAssetCount = platformSession.ReadyAssetCount;
        result.CompletedSteps = platformSession.CompletedSteps;
        result.TotalSteps = platformSession.TotalSteps;
        result.Phases = platformSession.Phases;
        result.CurrentState = platformSession.CurrentState;
        result.States = platformSession.States;
        result.Transitions = platformSession.Transitions;
        result.DispatchTargets = platformSession.DispatchTargets;
        result.ControlActions = platformSession.ControlActions;
        result.CoordinatorSteps = platformSession.CoordinatorSteps;
        result.OrchestrationStages = platformSession.OrchestrationStages;
        result.BootFlowStages = platformSession.BootFlowStages;
        result.BootSessionStages = platformSession.BootSessionStages;
        result.HandoffArtifacts = platformSession.HandoffArtifacts;
        result.ConsumedArtifacts = platformSession.ConsumedArtifacts;
        result.RuntimeSessionStages = platformSession.RuntimeSessionStages;
        result.ClientBootstrapActions = platformSession.ClientBootstrapActions;
        result.ReadinessChecks = platformSession.ReadinessChecks;
        result.ReadySignals = platformSession.ReadySignals;
        result.LaunchControlActions = platformSession.LaunchControlActions;
        result.ReadyStates = platformSession.ReadyStates;
        result.ClientLaunchStages = platformSession.ClientLaunchStages;
        result.ActivationSteps = platformSession.ActivationSteps;
        result.RunStateStages = platformSession.RunStateStages;
        result.ClientLoopStages = platformSession.ClientLoopStages;
        result.HostSessionStages = platformSession.HostSessionStages;
        result.HostLoopStages = platformSession.HostLoopStages;
        result.HostReadyChecks = platformSession.HostReadyChecks;
        result.PlatformSessionStages = platformSession.PlatformSessionStages;
        result.PlatformReadyChecks =
        [
            "platform-session-ready",
            "host-ready",
            "platform-ready"
        ];
        result.PlatformReadySummary = $"Runtime platform ready state passed {result.PlatformReadyChecks.Length} platform readiness check(s) for profile '{platformSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime platform ready state ready for profile '{platformSession.ProfileId}' with {result.PlatformReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimePlatformReadyStateResult
{
    public bool IsReady { get; set; }
    public string PlatformReadyStateVersion { get; set; } = string.Empty;
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
    public string[] PlatformReadyChecks { get; set; } = Array.Empty<string>();
    public string PlatformReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
