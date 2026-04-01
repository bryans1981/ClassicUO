namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCanvasSession
{
    ValueTask<BrowserClientRuntimeBrowserCanvasSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCanvasSessionService : IBrowserClientRuntimeBrowserCanvasSession
{
    private readonly IBrowserClientRuntimeBrowserFrameReadyState _runtimeBrowserFrameReadyState;

    public BrowserClientRuntimeBrowserCanvasSessionService(IBrowserClientRuntimeBrowserFrameReadyState runtimeBrowserFrameReadyState)
    {
        _runtimeBrowserFrameReadyState = runtimeBrowserFrameReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCanvasSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFrameReadyStateResult browserFrameReadyState = await _runtimeBrowserFrameReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCanvasSessionResult result = new()
        {
            ProfileId = browserFrameReadyState.ProfileId,
            SessionId = browserFrameReadyState.SessionId,
            SessionPath = browserFrameReadyState.SessionPath,
            BrowserFrameReadyStateVersion = browserFrameReadyState.BrowserFrameReadyStateVersion,
            BrowserFrameSessionVersion = browserFrameReadyState.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserFrameReadyState.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserFrameReadyState.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserFrameReadyState.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserFrameReadyState.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserFrameReadyState.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserFrameReadyState.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserFrameReadyState.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserFrameReadyState.PlatformLoopVersion,
            PlatformReadyStateVersion = browserFrameReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = browserFrameReadyState.PlatformSessionVersion,
            HostReadyStateVersion = browserFrameReadyState.HostReadyStateVersion,
            HostLoopVersion = browserFrameReadyState.HostLoopVersion,
            HostSessionVersion = browserFrameReadyState.HostSessionVersion,
            ClientLoopStateVersion = browserFrameReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = browserFrameReadyState.ClientRunStateVersion,
            ClientActivationVersion = browserFrameReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = browserFrameReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserFrameReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = browserFrameReadyState.LaunchControllerVersion,
            ReadySignalVersion = browserFrameReadyState.ReadySignalVersion,
            ReadinessGateVersion = browserFrameReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = browserFrameReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = browserFrameReadyState.RuntimeSessionVersion,
            ConsumerVersion = browserFrameReadyState.ConsumerVersion,
            HandoffVersion = browserFrameReadyState.HandoffVersion,
            BootSessionVersion = browserFrameReadyState.BootSessionVersion,
            BootFlowVersion = browserFrameReadyState.BootFlowVersion,
            OrchestratorVersion = browserFrameReadyState.OrchestratorVersion,
            CoordinatorVersion = browserFrameReadyState.CoordinatorVersion,
            ControllerVersion = browserFrameReadyState.ControllerVersion,
            DispatcherVersion = browserFrameReadyState.DispatcherVersion,
            DriverVersion = browserFrameReadyState.DriverVersion,
            StateMachineVersion = browserFrameReadyState.StateMachineVersion,
            StateVersion = browserFrameReadyState.StateVersion,
            CycleVersion = browserFrameReadyState.CycleVersion,
            InvocationVersion = browserFrameReadyState.InvocationVersion,
            LoopVersion = browserFrameReadyState.LoopVersion,
            RunnerVersion = browserFrameReadyState.RunnerVersion,
            ExecutionVersion = browserFrameReadyState.ExecutionVersion,
            HandshakeVersion = browserFrameReadyState.HandshakeVersion,
            PacketVersion = browserFrameReadyState.PacketVersion,
            ContractVersion = browserFrameReadyState.ContractVersion,
            Exists = browserFrameReadyState.Exists,
            ReadSucceeded = browserFrameReadyState.ReadSucceeded
        };

        if (!browserFrameReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser canvas session blocked for profile '{browserFrameReadyState.ProfileId}'.";
            result.Error = browserFrameReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCanvasSessionVersion = "runtime-browser-canvas-session-v1";
        result.LaunchMode = browserFrameReadyState.LaunchMode;
        result.AssetRootPath = browserFrameReadyState.AssetRootPath;
        result.ProfilesRootPath = browserFrameReadyState.ProfilesRootPath;
        result.CacheRootPath = browserFrameReadyState.CacheRootPath;
        result.ConfigRootPath = browserFrameReadyState.ConfigRootPath;
        result.SettingsFilePath = browserFrameReadyState.SettingsFilePath;
        result.StartupProfilePath = browserFrameReadyState.StartupProfilePath;
        result.RequiredAssets = browserFrameReadyState.RequiredAssets;
        result.ReadyAssetCount = browserFrameReadyState.ReadyAssetCount;
        result.CompletedSteps = browserFrameReadyState.CompletedSteps;
        result.TotalSteps = browserFrameReadyState.TotalSteps;
        result.Phases = browserFrameReadyState.Phases;
        result.CurrentState = browserFrameReadyState.CurrentState;
        result.States = browserFrameReadyState.States;
        result.Transitions = browserFrameReadyState.Transitions;
        result.DispatchTargets = browserFrameReadyState.DispatchTargets;
        result.ControlActions = browserFrameReadyState.ControlActions;
        result.CoordinatorSteps = browserFrameReadyState.CoordinatorSteps;
        result.OrchestrationStages = browserFrameReadyState.OrchestrationStages;
        result.BootFlowStages = browserFrameReadyState.BootFlowStages;
        result.BootSessionStages = browserFrameReadyState.BootSessionStages;
        result.HandoffArtifacts = browserFrameReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = browserFrameReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = browserFrameReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = browserFrameReadyState.ClientBootstrapActions;
        result.ReadinessChecks = browserFrameReadyState.ReadinessChecks;
        result.ReadySignals = browserFrameReadyState.ReadySignals;
        result.LaunchControlActions = browserFrameReadyState.LaunchControlActions;
        result.ReadyStates = browserFrameReadyState.ReadyStates;
        result.ClientLaunchStages = browserFrameReadyState.ClientLaunchStages;
        result.ActivationSteps = browserFrameReadyState.ActivationSteps;
        result.RunStateStages = browserFrameReadyState.RunStateStages;
        result.ClientLoopStages = browserFrameReadyState.ClientLoopStages;
        result.HostSessionStages = browserFrameReadyState.HostSessionStages;
        result.HostLoopStages = browserFrameReadyState.HostLoopStages;
        result.HostReadyChecks = browserFrameReadyState.HostReadyChecks;
        result.PlatformSessionStages = browserFrameReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = browserFrameReadyState.PlatformReadyChecks;
        result.PlatformLoopStages = browserFrameReadyState.PlatformLoopStages;
        result.LaunchGateChecks = browserFrameReadyState.LaunchGateChecks;
        result.BrowserShellStages = browserFrameReadyState.BrowserShellStages;
        result.BrowserShellReadyChecks = browserFrameReadyState.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserFrameReadyState.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserFrameReadyState.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserFrameReadyState.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserFrameReadyState.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserFrameReadyState.BrowserFrameStages;
        result.BrowserFrameReadyChecks = browserFrameReadyState.BrowserFrameReadyChecks;
        result.BrowserCanvasStages =
        [
            "open-browser-canvas-session",
            "bind-browser-frame-ready-state",
            "publish-browser-canvas-ready"
        ];
        result.BrowserCanvasSummary = $"Runtime browser canvas session prepared {result.BrowserCanvasStages.Length} browser-canvas stage(s) for profile '{browserFrameReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser canvas session ready for profile '{browserFrameReadyState.ProfileId}' with {result.BrowserCanvasStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCanvasSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserCanvasSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
