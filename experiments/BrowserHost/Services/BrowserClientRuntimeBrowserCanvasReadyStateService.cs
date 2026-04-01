namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCanvasReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCanvasReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCanvasReadyStateService : IBrowserClientRuntimeBrowserCanvasReadyState
{
    private readonly IBrowserClientRuntimeBrowserCanvasSession _runtimeBrowserCanvasSession;

    public BrowserClientRuntimeBrowserCanvasReadyStateService(IBrowserClientRuntimeBrowserCanvasSession runtimeBrowserCanvasSession)
    {
        _runtimeBrowserCanvasSession = runtimeBrowserCanvasSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCanvasReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCanvasSessionResult browserCanvasSession = await _runtimeBrowserCanvasSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCanvasReadyStateResult result = new()
        {
            ProfileId = browserCanvasSession.ProfileId,
            SessionId = browserCanvasSession.SessionId,
            SessionPath = browserCanvasSession.SessionPath,
            BrowserCanvasSessionVersion = browserCanvasSession.BrowserCanvasSessionVersion,
            BrowserFrameReadyStateVersion = browserCanvasSession.BrowserFrameReadyStateVersion,
            BrowserFrameSessionVersion = browserCanvasSession.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserCanvasSession.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserCanvasSession.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserCanvasSession.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserCanvasSession.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserCanvasSession.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserCanvasSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserCanvasSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserCanvasSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserCanvasSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserCanvasSession.PlatformSessionVersion,
            HostReadyStateVersion = browserCanvasSession.HostReadyStateVersion,
            HostLoopVersion = browserCanvasSession.HostLoopVersion,
            HostSessionVersion = browserCanvasSession.HostSessionVersion,
            ClientLoopStateVersion = browserCanvasSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserCanvasSession.ClientRunStateVersion,
            ClientActivationVersion = browserCanvasSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserCanvasSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserCanvasSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserCanvasSession.LaunchControllerVersion,
            ReadySignalVersion = browserCanvasSession.ReadySignalVersion,
            ReadinessGateVersion = browserCanvasSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserCanvasSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserCanvasSession.RuntimeSessionVersion,
            ConsumerVersion = browserCanvasSession.ConsumerVersion,
            HandoffVersion = browserCanvasSession.HandoffVersion,
            BootSessionVersion = browserCanvasSession.BootSessionVersion,
            BootFlowVersion = browserCanvasSession.BootFlowVersion,
            OrchestratorVersion = browserCanvasSession.OrchestratorVersion,
            CoordinatorVersion = browserCanvasSession.CoordinatorVersion,
            ControllerVersion = browserCanvasSession.ControllerVersion,
            DispatcherVersion = browserCanvasSession.DispatcherVersion,
            DriverVersion = browserCanvasSession.DriverVersion,
            StateMachineVersion = browserCanvasSession.StateMachineVersion,
            StateVersion = browserCanvasSession.StateVersion,
            CycleVersion = browserCanvasSession.CycleVersion,
            InvocationVersion = browserCanvasSession.InvocationVersion,
            LoopVersion = browserCanvasSession.LoopVersion,
            RunnerVersion = browserCanvasSession.RunnerVersion,
            ExecutionVersion = browserCanvasSession.ExecutionVersion,
            HandshakeVersion = browserCanvasSession.HandshakeVersion,
            PacketVersion = browserCanvasSession.PacketVersion,
            ContractVersion = browserCanvasSession.ContractVersion,
            Exists = browserCanvasSession.Exists,
            ReadSucceeded = browserCanvasSession.ReadSucceeded
        };

        if (!browserCanvasSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser canvas ready state blocked for profile '{browserCanvasSession.ProfileId}'.";
            result.Error = browserCanvasSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCanvasReadyStateVersion = "runtime-browser-canvas-ready-state-v1";
        result.LaunchMode = browserCanvasSession.LaunchMode;
        result.AssetRootPath = browserCanvasSession.AssetRootPath;
        result.ProfilesRootPath = browserCanvasSession.ProfilesRootPath;
        result.CacheRootPath = browserCanvasSession.CacheRootPath;
        result.ConfigRootPath = browserCanvasSession.ConfigRootPath;
        result.SettingsFilePath = browserCanvasSession.SettingsFilePath;
        result.StartupProfilePath = browserCanvasSession.StartupProfilePath;
        result.RequiredAssets = browserCanvasSession.RequiredAssets;
        result.ReadyAssetCount = browserCanvasSession.ReadyAssetCount;
        result.CompletedSteps = browserCanvasSession.CompletedSteps;
        result.TotalSteps = browserCanvasSession.TotalSteps;
        result.Phases = browserCanvasSession.Phases;
        result.CurrentState = browserCanvasSession.CurrentState;
        result.States = browserCanvasSession.States;
        result.Transitions = browserCanvasSession.Transitions;
        result.DispatchTargets = browserCanvasSession.DispatchTargets;
        result.ControlActions = browserCanvasSession.ControlActions;
        result.CoordinatorSteps = browserCanvasSession.CoordinatorSteps;
        result.OrchestrationStages = browserCanvasSession.OrchestrationStages;
        result.BootFlowStages = browserCanvasSession.BootFlowStages;
        result.BootSessionStages = browserCanvasSession.BootSessionStages;
        result.HandoffArtifacts = browserCanvasSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserCanvasSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserCanvasSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserCanvasSession.ClientBootstrapActions;
        result.ReadinessChecks = browserCanvasSession.ReadinessChecks;
        result.ReadySignals = browserCanvasSession.ReadySignals;
        result.LaunchControlActions = browserCanvasSession.LaunchControlActions;
        result.ReadyStates = browserCanvasSession.ReadyStates;
        result.ClientLaunchStages = browserCanvasSession.ClientLaunchStages;
        result.ActivationSteps = browserCanvasSession.ActivationSteps;
        result.RunStateStages = browserCanvasSession.RunStateStages;
        result.ClientLoopStages = browserCanvasSession.ClientLoopStages;
        result.HostSessionStages = browserCanvasSession.HostSessionStages;
        result.HostLoopStages = browserCanvasSession.HostLoopStages;
        result.HostReadyChecks = browserCanvasSession.HostReadyChecks;
        result.PlatformSessionStages = browserCanvasSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserCanvasSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserCanvasSession.PlatformLoopStages;
        result.LaunchGateChecks = browserCanvasSession.LaunchGateChecks;
        result.BrowserShellStages = browserCanvasSession.BrowserShellStages;
        result.BrowserShellReadyChecks = browserCanvasSession.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserCanvasSession.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserCanvasSession.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserCanvasSession.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserCanvasSession.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserCanvasSession.BrowserFrameStages;
        result.BrowserFrameReadyChecks = browserCanvasSession.BrowserFrameReadyChecks;
        result.BrowserCanvasStages = browserCanvasSession.BrowserCanvasStages;
        result.BrowserCanvasReadyChecks =
        [
            "browser-frame-ready-state-ready",
            "browser-canvas-session-ready",
            "browser-canvas-ready"
        ];
        result.BrowserCanvasReadySummary = $"Runtime browser canvas ready state passed {result.BrowserCanvasReadyChecks.Length} canvas readiness check(s) for profile '{browserCanvasSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser canvas ready state ready for profile '{browserCanvasSession.ProfileId}' with {result.BrowserCanvasReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCanvasReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCanvasReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCanvasSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCanvasStages { get; set; } = Array.Empty<string>();
    public string[] BrowserCanvasReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCanvasReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
