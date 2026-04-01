namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPresentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPresentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPresentReadyStateService : IBrowserClientRuntimeBrowserPresentReadyState
{
    private readonly IBrowserClientRuntimeBrowserPresentSession _runtimeBrowserPresentSession;

    public BrowserClientRuntimeBrowserPresentReadyStateService(IBrowserClientRuntimeBrowserPresentSession runtimeBrowserPresentSession)
    {
        _runtimeBrowserPresentSession = runtimeBrowserPresentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPresentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPresentSessionResult browserPresentSession = await _runtimeBrowserPresentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPresentReadyStateResult result = new()
        {
            ProfileId = browserPresentSession.ProfileId,
            SessionId = browserPresentSession.SessionId,
            SessionPath = browserPresentSession.SessionPath,
            BrowserPresentSessionVersion = browserPresentSession.BrowserPresentSessionVersion,
            BrowserRenderReadyStateVersion = browserPresentSession.BrowserRenderReadyStateVersion,
            BrowserRenderSessionVersion = browserPresentSession.BrowserRenderSessionVersion,
            BrowserCanvasReadyStateVersion = browserPresentSession.BrowserCanvasReadyStateVersion,
            BrowserCanvasSessionVersion = browserPresentSession.BrowserCanvasSessionVersion,
            BrowserFrameReadyStateVersion = browserPresentSession.BrowserFrameReadyStateVersion,
            BrowserFrameSessionVersion = browserPresentSession.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserPresentSession.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserPresentSession.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserPresentSession.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserPresentSession.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserPresentSession.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserPresentSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserPresentSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserPresentSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserPresentSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserPresentSession.PlatformSessionVersion,
            HostReadyStateVersion = browserPresentSession.HostReadyStateVersion,
            HostLoopVersion = browserPresentSession.HostLoopVersion,
            HostSessionVersion = browserPresentSession.HostSessionVersion,
            ClientLoopStateVersion = browserPresentSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserPresentSession.ClientRunStateVersion,
            ClientActivationVersion = browserPresentSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserPresentSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserPresentSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserPresentSession.LaunchControllerVersion,
            ReadySignalVersion = browserPresentSession.ReadySignalVersion,
            ReadinessGateVersion = browserPresentSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserPresentSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserPresentSession.RuntimeSessionVersion,
            ConsumerVersion = browserPresentSession.ConsumerVersion,
            HandoffVersion = browserPresentSession.HandoffVersion,
            BootSessionVersion = browserPresentSession.BootSessionVersion,
            BootFlowVersion = browserPresentSession.BootFlowVersion,
            OrchestratorVersion = browserPresentSession.OrchestratorVersion,
            CoordinatorVersion = browserPresentSession.CoordinatorVersion,
            ControllerVersion = browserPresentSession.ControllerVersion,
            DispatcherVersion = browserPresentSession.DispatcherVersion,
            DriverVersion = browserPresentSession.DriverVersion,
            StateMachineVersion = browserPresentSession.StateMachineVersion,
            StateVersion = browserPresentSession.StateVersion,
            CycleVersion = browserPresentSession.CycleVersion,
            InvocationVersion = browserPresentSession.InvocationVersion,
            LoopVersion = browserPresentSession.LoopVersion,
            RunnerVersion = browserPresentSession.RunnerVersion,
            ExecutionVersion = browserPresentSession.ExecutionVersion,
            HandshakeVersion = browserPresentSession.HandshakeVersion,
            PacketVersion = browserPresentSession.PacketVersion,
            ContractVersion = browserPresentSession.ContractVersion,
            Exists = browserPresentSession.Exists,
            ReadSucceeded = browserPresentSession.ReadSucceeded
        };

        if (!browserPresentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser present ready state blocked for profile '{browserPresentSession.ProfileId}'.";
            result.Error = browserPresentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPresentReadyStateVersion = "runtime-browser-present-ready-state-v1";
        result.LaunchMode = browserPresentSession.LaunchMode;
        result.AssetRootPath = browserPresentSession.AssetRootPath;
        result.ProfilesRootPath = browserPresentSession.ProfilesRootPath;
        result.CacheRootPath = browserPresentSession.CacheRootPath;
        result.ConfigRootPath = browserPresentSession.ConfigRootPath;
        result.SettingsFilePath = browserPresentSession.SettingsFilePath;
        result.StartupProfilePath = browserPresentSession.StartupProfilePath;
        result.RequiredAssets = browserPresentSession.RequiredAssets;
        result.ReadyAssetCount = browserPresentSession.ReadyAssetCount;
        result.CompletedSteps = browserPresentSession.CompletedSteps;
        result.TotalSteps = browserPresentSession.TotalSteps;
        result.Phases = browserPresentSession.Phases;
        result.CurrentState = browserPresentSession.CurrentState;
        result.States = browserPresentSession.States;
        result.Transitions = browserPresentSession.Transitions;
        result.DispatchTargets = browserPresentSession.DispatchTargets;
        result.ControlActions = browserPresentSession.ControlActions;
        result.CoordinatorSteps = browserPresentSession.CoordinatorSteps;
        result.OrchestrationStages = browserPresentSession.OrchestrationStages;
        result.BootFlowStages = browserPresentSession.BootFlowStages;
        result.BootSessionStages = browserPresentSession.BootSessionStages;
        result.HandoffArtifacts = browserPresentSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserPresentSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserPresentSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserPresentSession.ClientBootstrapActions;
        result.ReadinessChecks = browserPresentSession.ReadinessChecks;
        result.ReadySignals = browserPresentSession.ReadySignals;
        result.LaunchControlActions = browserPresentSession.LaunchControlActions;
        result.ReadyStates = browserPresentSession.ReadyStates;
        result.ClientLaunchStages = browserPresentSession.ClientLaunchStages;
        result.ActivationSteps = browserPresentSession.ActivationSteps;
        result.RunStateStages = browserPresentSession.RunStateStages;
        result.ClientLoopStages = browserPresentSession.ClientLoopStages;
        result.HostSessionStages = browserPresentSession.HostSessionStages;
        result.HostLoopStages = browserPresentSession.HostLoopStages;
        result.HostReadyChecks = browserPresentSession.HostReadyChecks;
        result.PlatformSessionStages = browserPresentSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserPresentSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserPresentSession.PlatformLoopStages;
        result.LaunchGateChecks = browserPresentSession.LaunchGateChecks;
        result.BrowserShellStages = browserPresentSession.BrowserShellStages;
        result.BrowserShellReadyChecks = browserPresentSession.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserPresentSession.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserPresentSession.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserPresentSession.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserPresentSession.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserPresentSession.BrowserFrameStages;
        result.BrowserFrameReadyChecks = browserPresentSession.BrowserFrameReadyChecks;
        result.BrowserCanvasStages = browserPresentSession.BrowserCanvasStages;
        result.BrowserCanvasReadyChecks = browserPresentSession.BrowserCanvasReadyChecks;
        result.BrowserRenderStages = browserPresentSession.BrowserRenderStages;
        result.BrowserRenderReadyChecks = browserPresentSession.BrowserRenderReadyChecks;
        result.BrowserPresentStages = browserPresentSession.BrowserPresentStages;
        result.BrowserPresentReadyChecks =
        [
            "browser-render-ready-state-ready",
            "browser-present-session-ready",
            "browser-present-ready"
        ];
        result.BrowserPresentReadySummary = $"Runtime browser present ready state passed {result.BrowserPresentReadyChecks.Length} present readiness check(s) for profile '{browserPresentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser present ready state ready for profile '{browserPresentSession.ProfileId}' with {result.BrowserPresentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPresentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPresentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPresentSessionVersion { get; set; } = string.Empty;
    public string BrowserRenderReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRenderSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRenderStages { get; set; } = Array.Empty<string>();
    public string[] BrowserRenderReadyChecks { get; set; } = Array.Empty<string>();
    public string[] BrowserPresentStages { get; set; } = Array.Empty<string>();
    public string[] BrowserPresentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPresentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
