namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRenderReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRenderReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRenderReadyStateService : IBrowserClientRuntimeBrowserRenderReadyState
{
    private readonly IBrowserClientRuntimeBrowserRenderSession _runtimeBrowserRenderSession;

    public BrowserClientRuntimeBrowserRenderReadyStateService(IBrowserClientRuntimeBrowserRenderSession runtimeBrowserRenderSession)
    {
        _runtimeBrowserRenderSession = runtimeBrowserRenderSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRenderReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRenderSessionResult browserRenderSession = await _runtimeBrowserRenderSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRenderReadyStateResult result = new()
        {
            ProfileId = browserRenderSession.ProfileId,
            SessionId = browserRenderSession.SessionId,
            SessionPath = browserRenderSession.SessionPath,
            BrowserRenderSessionVersion = browserRenderSession.BrowserRenderSessionVersion,
            BrowserCanvasReadyStateVersion = browserRenderSession.BrowserCanvasReadyStateVersion,
            BrowserCanvasSessionVersion = browserRenderSession.BrowserCanvasSessionVersion,
            BrowserFrameReadyStateVersion = browserRenderSession.BrowserFrameReadyStateVersion,
            BrowserFrameSessionVersion = browserRenderSession.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserRenderSession.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserRenderSession.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserRenderSession.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserRenderSession.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserRenderSession.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserRenderSession.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserRenderSession.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserRenderSession.PlatformLoopVersion,
            PlatformReadyStateVersion = browserRenderSession.PlatformReadyStateVersion,
            PlatformSessionVersion = browserRenderSession.PlatformSessionVersion,
            HostReadyStateVersion = browserRenderSession.HostReadyStateVersion,
            HostLoopVersion = browserRenderSession.HostLoopVersion,
            HostSessionVersion = browserRenderSession.HostSessionVersion,
            ClientLoopStateVersion = browserRenderSession.ClientLoopStateVersion,
            ClientRunStateVersion = browserRenderSession.ClientRunStateVersion,
            ClientActivationVersion = browserRenderSession.ClientActivationVersion,
            ClientLaunchSessionVersion = browserRenderSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserRenderSession.ClientReadyStateVersion,
            LaunchControllerVersion = browserRenderSession.LaunchControllerVersion,
            ReadySignalVersion = browserRenderSession.ReadySignalVersion,
            ReadinessGateVersion = browserRenderSession.ReadinessGateVersion,
            ClientBootstrapVersion = browserRenderSession.ClientBootstrapVersion,
            RuntimeSessionVersion = browserRenderSession.RuntimeSessionVersion,
            ConsumerVersion = browserRenderSession.ConsumerVersion,
            HandoffVersion = browserRenderSession.HandoffVersion,
            BootSessionVersion = browserRenderSession.BootSessionVersion,
            BootFlowVersion = browserRenderSession.BootFlowVersion,
            OrchestratorVersion = browserRenderSession.OrchestratorVersion,
            CoordinatorVersion = browserRenderSession.CoordinatorVersion,
            ControllerVersion = browserRenderSession.ControllerVersion,
            DispatcherVersion = browserRenderSession.DispatcherVersion,
            DriverVersion = browserRenderSession.DriverVersion,
            StateMachineVersion = browserRenderSession.StateMachineVersion,
            StateVersion = browserRenderSession.StateVersion,
            CycleVersion = browserRenderSession.CycleVersion,
            InvocationVersion = browserRenderSession.InvocationVersion,
            LoopVersion = browserRenderSession.LoopVersion,
            RunnerVersion = browserRenderSession.RunnerVersion,
            ExecutionVersion = browserRenderSession.ExecutionVersion,
            HandshakeVersion = browserRenderSession.HandshakeVersion,
            PacketVersion = browserRenderSession.PacketVersion,
            ContractVersion = browserRenderSession.ContractVersion,
            Exists = browserRenderSession.Exists,
            ReadSucceeded = browserRenderSession.ReadSucceeded
        };

        if (!browserRenderSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser render ready state blocked for profile '{browserRenderSession.ProfileId}'.";
            result.Error = browserRenderSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRenderReadyStateVersion = "runtime-browser-render-ready-state-v1";
        result.LaunchMode = browserRenderSession.LaunchMode;
        result.AssetRootPath = browserRenderSession.AssetRootPath;
        result.ProfilesRootPath = browserRenderSession.ProfilesRootPath;
        result.CacheRootPath = browserRenderSession.CacheRootPath;
        result.ConfigRootPath = browserRenderSession.ConfigRootPath;
        result.SettingsFilePath = browserRenderSession.SettingsFilePath;
        result.StartupProfilePath = browserRenderSession.StartupProfilePath;
        result.RequiredAssets = browserRenderSession.RequiredAssets;
        result.ReadyAssetCount = browserRenderSession.ReadyAssetCount;
        result.CompletedSteps = browserRenderSession.CompletedSteps;
        result.TotalSteps = browserRenderSession.TotalSteps;
        result.Phases = browserRenderSession.Phases;
        result.CurrentState = browserRenderSession.CurrentState;
        result.States = browserRenderSession.States;
        result.Transitions = browserRenderSession.Transitions;
        result.DispatchTargets = browserRenderSession.DispatchTargets;
        result.ControlActions = browserRenderSession.ControlActions;
        result.CoordinatorSteps = browserRenderSession.CoordinatorSteps;
        result.OrchestrationStages = browserRenderSession.OrchestrationStages;
        result.BootFlowStages = browserRenderSession.BootFlowStages;
        result.BootSessionStages = browserRenderSession.BootSessionStages;
        result.HandoffArtifacts = browserRenderSession.HandoffArtifacts;
        result.ConsumedArtifacts = browserRenderSession.ConsumedArtifacts;
        result.RuntimeSessionStages = browserRenderSession.RuntimeSessionStages;
        result.ClientBootstrapActions = browserRenderSession.ClientBootstrapActions;
        result.ReadinessChecks = browserRenderSession.ReadinessChecks;
        result.ReadySignals = browserRenderSession.ReadySignals;
        result.LaunchControlActions = browserRenderSession.LaunchControlActions;
        result.ReadyStates = browserRenderSession.ReadyStates;
        result.ClientLaunchStages = browserRenderSession.ClientLaunchStages;
        result.ActivationSteps = browserRenderSession.ActivationSteps;
        result.RunStateStages = browserRenderSession.RunStateStages;
        result.ClientLoopStages = browserRenderSession.ClientLoopStages;
        result.HostSessionStages = browserRenderSession.HostSessionStages;
        result.HostLoopStages = browserRenderSession.HostLoopStages;
        result.HostReadyChecks = browserRenderSession.HostReadyChecks;
        result.PlatformSessionStages = browserRenderSession.PlatformSessionStages;
        result.PlatformReadyChecks = browserRenderSession.PlatformReadyChecks;
        result.PlatformLoopStages = browserRenderSession.PlatformLoopStages;
        result.LaunchGateChecks = browserRenderSession.LaunchGateChecks;
        result.BrowserShellStages = browserRenderSession.BrowserShellStages;
        result.BrowserShellReadyChecks = browserRenderSession.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserRenderSession.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserRenderSession.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserRenderSession.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserRenderSession.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserRenderSession.BrowserFrameStages;
        result.BrowserFrameReadyChecks = browserRenderSession.BrowserFrameReadyChecks;
        result.BrowserCanvasStages = browserRenderSession.BrowserCanvasStages;
        result.BrowserCanvasReadyChecks = browserRenderSession.BrowserCanvasReadyChecks;
        result.BrowserRenderStages = browserRenderSession.BrowserRenderStages;
        result.BrowserRenderReadyChecks =
        [
            "browser-canvas-ready-state-ready",
            "browser-render-session-ready",
            "browser-render-ready"
        ];
        result.BrowserRenderReadySummary = $"Runtime browser render ready state passed {result.BrowserRenderReadyChecks.Length} render readiness check(s) for profile '{browserRenderSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser render ready state ready for profile '{browserRenderSession.ProfileId}' with {result.BrowserRenderReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRenderReadyStateResult
{
    public bool IsReady { get; set; }
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
    public string BrowserRenderReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
