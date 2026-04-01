namespace BrowserHost.Services;

public interface IBrowserClientRuntimePlatformLaunchGate
{
    ValueTask<BrowserClientRuntimePlatformLaunchGateResult> EvaluateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimePlatformLaunchGateService : IBrowserClientRuntimePlatformLaunchGate
{
    private readonly IBrowserClientRuntimePlatformLoop _runtimePlatformLoop;

    public BrowserClientRuntimePlatformLaunchGateService(IBrowserClientRuntimePlatformLoop runtimePlatformLoop)
    {
        _runtimePlatformLoop = runtimePlatformLoop;
    }

    public async ValueTask<BrowserClientRuntimePlatformLaunchGateResult> EvaluateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimePlatformLoopResult platformLoop = await _runtimePlatformLoop.RunAsync(profileId);

        BrowserClientRuntimePlatformLaunchGateResult result = new()
        {
            ProfileId = platformLoop.ProfileId,
            SessionId = platformLoop.SessionId,
            SessionPath = platformLoop.SessionPath,
            PlatformLoopVersion = platformLoop.PlatformLoopVersion,
            PlatformReadyStateVersion = platformLoop.PlatformReadyStateVersion,
            PlatformSessionVersion = platformLoop.PlatformSessionVersion,
            HostReadyStateVersion = platformLoop.HostReadyStateVersion,
            HostLoopVersion = platformLoop.HostLoopVersion,
            HostSessionVersion = platformLoop.HostSessionVersion,
            ClientLoopStateVersion = platformLoop.ClientLoopStateVersion,
            ClientRunStateVersion = platformLoop.ClientRunStateVersion,
            ClientActivationVersion = platformLoop.ClientActivationVersion,
            ClientLaunchSessionVersion = platformLoop.ClientLaunchSessionVersion,
            ClientReadyStateVersion = platformLoop.ClientReadyStateVersion,
            LaunchControllerVersion = platformLoop.LaunchControllerVersion,
            ReadySignalVersion = platformLoop.ReadySignalVersion,
            ReadinessGateVersion = platformLoop.ReadinessGateVersion,
            ClientBootstrapVersion = platformLoop.ClientBootstrapVersion,
            RuntimeSessionVersion = platformLoop.RuntimeSessionVersion,
            ConsumerVersion = platformLoop.ConsumerVersion,
            HandoffVersion = platformLoop.HandoffVersion,
            BootSessionVersion = platformLoop.BootSessionVersion,
            BootFlowVersion = platformLoop.BootFlowVersion,
            OrchestratorVersion = platformLoop.OrchestratorVersion,
            CoordinatorVersion = platformLoop.CoordinatorVersion,
            ControllerVersion = platformLoop.ControllerVersion,
            DispatcherVersion = platformLoop.DispatcherVersion,
            DriverVersion = platformLoop.DriverVersion,
            StateMachineVersion = platformLoop.StateMachineVersion,
            StateVersion = platformLoop.StateVersion,
            CycleVersion = platformLoop.CycleVersion,
            InvocationVersion = platformLoop.InvocationVersion,
            LoopVersion = platformLoop.LoopVersion,
            RunnerVersion = platformLoop.RunnerVersion,
            ExecutionVersion = platformLoop.ExecutionVersion,
            HandshakeVersion = platformLoop.HandshakeVersion,
            PacketVersion = platformLoop.PacketVersion,
            ContractVersion = platformLoop.ContractVersion,
            Exists = platformLoop.Exists,
            ReadSucceeded = platformLoop.ReadSucceeded
        };

        if (!platformLoop.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime platform launch gate blocked for profile '{platformLoop.ProfileId}'.";
            result.Error = platformLoop.Error;
            return result;
        }

        result.IsReady = true;
        result.PlatformLaunchGateVersion = "runtime-platform-launch-gate-v1";
        result.LaunchMode = platformLoop.LaunchMode;
        result.AssetRootPath = platformLoop.AssetRootPath;
        result.ProfilesRootPath = platformLoop.ProfilesRootPath;
        result.CacheRootPath = platformLoop.CacheRootPath;
        result.ConfigRootPath = platformLoop.ConfigRootPath;
        result.SettingsFilePath = platformLoop.SettingsFilePath;
        result.StartupProfilePath = platformLoop.StartupProfilePath;
        result.RequiredAssets = platformLoop.RequiredAssets;
        result.ReadyAssetCount = platformLoop.ReadyAssetCount;
        result.CompletedSteps = platformLoop.CompletedSteps;
        result.TotalSteps = platformLoop.TotalSteps;
        result.Phases = platformLoop.Phases;
        result.CurrentState = platformLoop.CurrentState;
        result.States = platformLoop.States;
        result.Transitions = platformLoop.Transitions;
        result.DispatchTargets = platformLoop.DispatchTargets;
        result.ControlActions = platformLoop.ControlActions;
        result.CoordinatorSteps = platformLoop.CoordinatorSteps;
        result.OrchestrationStages = platformLoop.OrchestrationStages;
        result.BootFlowStages = platformLoop.BootFlowStages;
        result.BootSessionStages = platformLoop.BootSessionStages;
        result.HandoffArtifacts = platformLoop.HandoffArtifacts;
        result.ConsumedArtifacts = platformLoop.ConsumedArtifacts;
        result.RuntimeSessionStages = platformLoop.RuntimeSessionStages;
        result.ClientBootstrapActions = platformLoop.ClientBootstrapActions;
        result.ReadinessChecks = platformLoop.ReadinessChecks;
        result.ReadySignals = platformLoop.ReadySignals;
        result.LaunchControlActions = platformLoop.LaunchControlActions;
        result.ReadyStates = platformLoop.ReadyStates;
        result.ClientLaunchStages = platformLoop.ClientLaunchStages;
        result.ActivationSteps = platformLoop.ActivationSteps;
        result.RunStateStages = platformLoop.RunStateStages;
        result.ClientLoopStages = platformLoop.ClientLoopStages;
        result.HostSessionStages = platformLoop.HostSessionStages;
        result.HostLoopStages = platformLoop.HostLoopStages;
        result.HostReadyChecks = platformLoop.HostReadyChecks;
        result.PlatformSessionStages = platformLoop.PlatformSessionStages;
        result.PlatformReadyChecks = platformLoop.PlatformReadyChecks;
        result.PlatformLoopStages = platformLoop.PlatformLoopStages;
        result.LaunchGateChecks =
        [
            "platform-ready",
            "platform-loop-ready",
            "launch-gate-open"
        ];
        result.LaunchGateSummary = $"Runtime platform launch gate passed {result.LaunchGateChecks.Length} launch check(s) for profile '{platformLoop.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime platform launch gate ready for profile '{platformLoop.ProfileId}' with {result.LaunchGateChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimePlatformLaunchGateResult
{
    public bool IsReady { get; set; }
    public string PlatformLaunchGateVersion { get; set; } = string.Empty;
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
    public string[] LaunchGateChecks { get; set; } = Array.Empty<string>();
    public string LaunchGateSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
