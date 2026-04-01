namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFrameReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFrameReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFrameReadyStateService : IBrowserClientRuntimeBrowserFrameReadyState
{
    private readonly IBrowserClientRuntimeBrowserFrameSession _runtimeBrowserFrameSession;

    public BrowserClientRuntimeBrowserFrameReadyStateService(IBrowserClientRuntimeBrowserFrameSession runtimeBrowserFrameSession)
    {
        _runtimeBrowserFrameSession = runtimeBrowserFrameSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFrameReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFrameSessionResult browserFrameSession = await _runtimeBrowserFrameSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFrameReadyStateResult result = new()
        {
            ProfileId = browserFrameSession.ProfileId,
            SessionId = browserFrameSession.SessionId,
            SessionPath = browserFrameSession.SessionPath,
            BrowserFrameSessionVersion = browserFrameSession.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserFrameSession.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserFrameSession.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserFrameSession.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserFrameSession.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserFrameSession.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserFrameSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserFrameSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserFrameSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserFrameSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserFrameSession.PlatformSessionVersion,
            HostReadyStateVersion = browserFrameSession.HostReadyStateVersion,
            HostLoopVersion = browserFrameSession.HostLoopVersion,
            HostSessionVersion = browserFrameSession.HostSessionVersion,
            ClientLoopStateVersion = browserFrameSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserFrameSession.ClientRunStateVersion,
            ClientActivationVersion = browserFrameSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserFrameSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserFrameSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserFrameSession.LaunchControllerVersion,
            ReadySignalVersion = browserFrameSession.ReadySignalVersion,
            ReadinessGateVersion = browserFrameSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserFrameSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserFrameSession.RuntimeSessionVersion,
            ConsumerVersion = browserFrameSession.ConsumerVersion,
            HandoffVersion = browserFrameSession.HandoffVersion,
            BootSessionVersion = browserFrameSession.BootSessionVersion,
            BootFlowVersion = browserFrameSession.BootFlowVersion,
            OrchestratorVersion = browserFrameSession.OrchestratorVersion,
            CoordinatorVersion = browserFrameSession.CoordinatorVersion,
            ControllerVersion = browserFrameSession.ControllerVersion,
            DispatcherVersion = browserFrameSession.DispatcherVersion,
            DriverVersion = browserFrameSession.DriverVersion,
            StateMachineVersion = browserFrameSession.StateMachineVersion,
            StateVersion = browserFrameSession.StateVersion,
            CycleVersion = browserFrameSession.CycleVersion,
            InvocationVersion = browserFrameSession.InvocationVersion,
            LoopVersion = browserFrameSession.LoopVersion,
            RunnerVersion = browserFrameSession.RunnerVersion,
            ExecutionVersion = browserFrameSession.ExecutionVersion,
            HandshakeVersion = browserFrameSession.HandshakeVersion,
            PacketVersion = browserFrameSession.PacketVersion,
            ContractVersion = browserFrameSession.ContractVersion,
            Exists = browserFrameSession.Exists,
            ReadSucceeded = browserFrameSession.ReadSucceeded
        };

        if (!browserFrameSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser frame ready state blocked for profile '{browserFrameSession.ProfileId}'.";
            result.Error = browserFrameSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFrameReadyStateVersion = "runtime-browser-frame-ready-state-v1";
        result.LaunchMode = browserFrameSession.LaunchMode;
        result.AssetRootPath = browserFrameSession.AssetRootPath;
        result.ProfilesRootPath = browserFrameSession.ProfilesRootPath;
        result.CacheRootPath = browserFrameSession.CacheRootPath;
        result.ConfigRootPath = browserFrameSession.ConfigRootPath;
        result.SettingsFilePath = browserFrameSession.SettingsFilePath;
        result.StartupProfilePath = browserFrameSession.StartupProfilePath;
        result.RequiredAssets = browserFrameSession.RequiredAssets;
        result.ReadyAssetCount = browserFrameSession.ReadyAssetCount;
        result.CompletedSteps = browserFrameSession.CompletedSteps;
        result.TotalSteps = browserFrameSession.TotalSteps;
        result.Phases = browserFrameSession.Phases;
        result.CurrentState = browserFrameSession.CurrentState;
        result.States = browserFrameSession.States;
        result.Transitions = browserFrameSession.Transitions;
        result.DispatchTargets = browserFrameSession.DispatchTargets;
        result.ControlActions = browserFrameSession.ControlActions;
        result.CoordinatorSteps = browserFrameSession.CoordinatorSteps;
        result.OrchestrationStages = browserFrameSession.OrchestrationStages;
        result.BootFlowStages = browserFrameSession.BootFlowStages;
        result.BootSessionStages = browserFrameSession.BootSessionStages;
        result.HandoffArtifacts = browserFrameSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserFrameSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserFrameSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserFrameSession.ClientBootstrapActions;
        result.ReadinessChecks = browserFrameSession.ReadinessChecks;
        result.ReadySignals = browserFrameSession.ReadySignals;
        result.LaunchControlActions = browserFrameSession.LaunchControlActions;
        result.ReadyStates = browserFrameSession.ReadyStates;
        result.ClientLaunchStages = browserFrameSession.ClientLaunchStages;
        result.ActivationSteps = browserFrameSession.ActivationSteps;
        result.RunStateStages = browserFrameSession.RunStateStages;
        result.ClientLoopStages = browserFrameSession.ClientLoopStages;
        result.HostSessionStages = browserFrameSession.HostSessionStages;
        result.HostLoopStages = browserFrameSession.HostLoopStages;
        result.HostReadyChecks = browserFrameSession.HostReadyChecks;
        result.PlatformSessionStages = browserFrameSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserFrameSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserFrameSession.PlatformLoopStages;
        result.LaunchGateChecks = browserFrameSession.LaunchGateChecks;
        result.BrowserShellStages = browserFrameSession.BrowserShellStages;
        result.BrowserShellReadyChecks = browserFrameSession.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserFrameSession.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserFrameSession.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserFrameSession.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserFrameSession.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserFrameSession.BrowserFrameStages;
        result.BrowserFrameReadyChecks =
        [
            "browser-window-ready-state-ready",
            "browser-frame-session-ready",
            "browser-frame-ready"
        ];
        result.BrowserFrameReadySummary = $"Runtime browser frame ready state passed {result.BrowserFrameReadyChecks.Length} frame readiness check(s) for profile '{browserFrameSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser frame ready state ready for profile '{browserFrameSession.ProfileId}' with {result.BrowserFrameReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFrameReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFrameReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFrameSessionVersion { get; set; } = string.Empty;
    public string BrowserWindowReadyStateVersion { get; set; } = string.Empty;
    public string BrowserWindowSessionVersion { get; set; } = string.Empty;
    public string BrowserSurfaceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSurfaceSessionVersion { get; set; } = string.Empty;
    public string BrowserShellReadyStateVersion { get; set; } = string.Empty;
    public string BrowserShellSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserShellStages { get; set; } = Array.Empty<string>();
    public string[] BrowserShellReadyChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserSurfaceStages { get; set; } = Array.Empty<string>();
    public string[] BrowserSurfaceReadyChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserWindowStages { get; set; } = Array.Empty<string>();
    public string[] BrowserWindowReadyChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserFrameStages { get; set; } = Array.Empty<string>();
    public string[] BrowserFrameReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFrameReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
