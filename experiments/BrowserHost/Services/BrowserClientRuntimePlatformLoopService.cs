namespace BrowserHost.Services;

public interface IBrowserClientRuntimePlatformLoop
{
    ValueTask<BrowserClientRuntimePlatformLoopResult> RunAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimePlatformLoopService : IBrowserClientRuntimePlatformLoop
{
    private readonly IBrowserClientRuntimePlatformReadyState _runtimePlatformReadyState;

    public BrowserClientRuntimePlatformLoopService(IBrowserClientRuntimePlatformReadyState runtimePlatformReadyState)
    {
        _runtimePlatformReadyState = runtimePlatformReadyState;
    }

    public async ValueTask<BrowserClientRuntimePlatformLoopResult> RunAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimePlatformReadyStateResult platformReadyState = await _runtimePlatformReadyState.BuildAsync(profileId);

        BrowserClientRuntimePlatformLoopResult result = new()
        {
            ProfileId = platformReadyState.ProfileId,
            SessionId = platformReadyState.SessionId,
            SessionPath = platformReadyState.SessionPath,
            PlatformReadyStateVersion = platformReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = platformReadyState.PlatformSessionVersion,
            HostReadyStateVersion = platformReadyState.HostReadyStateVersion,
            HostLoopVersion = platformReadyState.HostLoopVersion,
            HostSessionVersion = platformReadyState.HostSessionVersion,
            ClientLoopStateVersion = platformReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = platformReadyState.ClientRunStateVersion,
            ClientActivationVersion = platformReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = platformReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = platformReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = platformReadyState.LaunchControllerVersion,
            ReadySignalVersion = platformReadyState.ReadySignalVersion,
            ReadinessGateVersion = platformReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = platformReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = platformReadyState.RuntimeSessionVersion,
            ConsumerVersion = platformReadyState.ConsumerVersion,
            HandoffVersion = platformReadyState.HandoffVersion,
            BootSessionVersion = platformReadyState.BootSessionVersion,
            BootFlowVersion = platformReadyState.BootFlowVersion,
            OrchestratorVersion = platformReadyState.OrchestratorVersion,
            CoordinatorVersion = platformReadyState.CoordinatorVersion,
            ControllerVersion = platformReadyState.ControllerVersion,
            DispatcherVersion = platformReadyState.DispatcherVersion,
            DriverVersion = platformReadyState.DriverVersion,
            StateMachineVersion = platformReadyState.StateMachineVersion,
            StateVersion = platformReadyState.StateVersion,
            CycleVersion = platformReadyState.CycleVersion,
            InvocationVersion = platformReadyState.InvocationVersion,
            LoopVersion = platformReadyState.LoopVersion,
            RunnerVersion = platformReadyState.RunnerVersion,
            ExecutionVersion = platformReadyState.ExecutionVersion,
            HandshakeVersion = platformReadyState.HandshakeVersion,
            PacketVersion = platformReadyState.PacketVersion,
            ContractVersion = platformReadyState.ContractVersion,
            Exists = platformReadyState.Exists,
            ReadSucceeded = platformReadyState.ReadSucceeded
        };

        if (!platformReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime platform loop blocked for profile '{platformReadyState.ProfileId}'.";
            result.Error = platformReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.PlatformLoopVersion = "runtime-platform-loop-v1";
        result.LaunchMode = platformReadyState.LaunchMode;
        result.AssetRootPath = platformReadyState.AssetRootPath;
        result.ProfilesRootPath = platformReadyState.ProfilesRootPath;
        result.CacheRootPath = platformReadyState.CacheRootPath;
        result.ConfigRootPath = platformReadyState.ConfigRootPath;
        result.SettingsFilePath = platformReadyState.SettingsFilePath;
        result.StartupProfilePath = platformReadyState.StartupProfilePath;
        result.RequiredAssets = platformReadyState.RequiredAssets;
        result.ReadyAssetCount = platformReadyState.ReadyAssetCount;
        result.CompletedSteps = platformReadyState.CompletedSteps;
        result.TotalSteps = platformReadyState.TotalSteps;
        result.Phases = platformReadyState.Phases;
        result.CurrentState = platformReadyState.CurrentState;
        result.States = platformReadyState.States;
        result.Transitions = platformReadyState.Transitions;
        result.DispatchTargets = platformReadyState.DispatchTargets;
        result.ControlActions = platformReadyState.ControlActions;
        result.CoordinatorSteps = platformReadyState.CoordinatorSteps;
        result.OrchestrationStages = platformReadyState.OrchestrationStages;
        result.BootFlowStages = platformReadyState.BootFlowStages;
        result.BootSessionStages = platformReadyState.BootSessionStages;
        result.HandoffArtifacts = platformReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = platformReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = platformReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = platformReadyState.ClientBootstrapActions;
        result.ReadinessChecks = platformReadyState.ReadinessChecks;
        result.ReadySignals = platformReadyState.ReadySignals;
        result.LaunchControlActions = platformReadyState.LaunchControlActions;
        result.ReadyStates = platformReadyState.ReadyStates;
        result.ClientLaunchStages = platformReadyState.ClientLaunchStages;
        result.ActivationSteps = platformReadyState.ActivationSteps;
        result.RunStateStages = platformReadyState.RunStateStages;
        result.ClientLoopStages = platformReadyState.ClientLoopStages;
        result.HostSessionStages = platformReadyState.HostSessionStages;
        result.HostLoopStages = platformReadyState.HostLoopStages;
        result.HostReadyChecks = platformReadyState.HostReadyChecks;
        result.PlatformSessionStages = platformReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = platformReadyState.PlatformReadyChecks;
        result.PlatformLoopStages =
        [
            "initialize-platform-loop",
            "bind-platform-ready-state",
            "publish-platform-loop-ready"
        ];
        result.PlatformLoopSummary = $"Runtime platform loop prepared {result.PlatformLoopStages.Length} platform-loop stage(s) for profile '{platformReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime platform loop ready for profile '{platformReadyState.ProfileId}' with {result.PlatformLoopStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimePlatformLoopResult
{
    public bool IsReady { get; set; }
    public string PlatformLoopVersion { get; set; } = string.Empty;
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
    public string[] PlatformLoopStages { get; set; } = Array.Empty<string>();
    public string PlatformLoopSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
