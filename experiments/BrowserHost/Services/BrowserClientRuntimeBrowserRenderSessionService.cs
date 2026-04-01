namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRenderSession
{
    ValueTask<BrowserClientRuntimeBrowserRenderSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRenderSessionService : IBrowserClientRuntimeBrowserRenderSession
{
    private readonly IBrowserClientRuntimeBrowserCanvasReadyState _runtimeBrowserCanvasReadyState;

    public BrowserClientRuntimeBrowserRenderSessionService(IBrowserClientRuntimeBrowserCanvasReadyState runtimeBrowserCanvasReadyState)
    {
        _runtimeBrowserCanvasReadyState = runtimeBrowserCanvasReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRenderSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCanvasReadyStateResult browserCanvasReadyState = await _runtimeBrowserCanvasReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRenderSessionResult result = new()
        {
            ProfileId = browserCanvasReadyState.ProfileId,
            SessionId = browserCanvasReadyState.SessionId,
            SessionPath = browserCanvasReadyState.SessionPath,
            BrowserCanvasReadyStateVersion = browserCanvasReadyState.BrowserCanvasReadyStateVersion,
            BrowserCanvasSessionVersion = browserCanvasReadyState.BrowserCanvasSessionVersion,
            BrowserFrameReadyStateVersion = browserCanvasReadyState.BrowserFrameReadyStateVersion,
            BrowserFrameSessionVersion = browserCanvasReadyState.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserCanvasReadyState.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserCanvasReadyState.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserCanvasReadyState.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserCanvasReadyState.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserCanvasReadyState.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserCanvasReadyState.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserCanvasReadyState.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserCanvasReadyState.PlatformLoopVersion,
            PlatformReadyStateVersion = browserCanvasReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = browserCanvasReadyState.PlatformSessionVersion,
            HostReadyStateVersion = browserCanvasReadyState.HostReadyStateVersion,
            HostLoopVersion = browserCanvasReadyState.HostLoopVersion,
            HostSessionVersion = browserCanvasReadyState.HostSessionVersion,
            ClientLoopStateVersion = browserCanvasReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = browserCanvasReadyState.ClientRunStateVersion,
            ClientActivationVersion = browserCanvasReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = browserCanvasReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserCanvasReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = browserCanvasReadyState.LaunchControllerVersion,
            ReadySignalVersion = browserCanvasReadyState.ReadySignalVersion,
            ReadinessGateVersion = browserCanvasReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = browserCanvasReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = browserCanvasReadyState.RuntimeSessionVersion,
            ConsumerVersion = browserCanvasReadyState.ConsumerVersion,
            HandoffVersion = browserCanvasReadyState.HandoffVersion,
            BootSessionVersion = browserCanvasReadyState.BootSessionVersion,
            BootFlowVersion = browserCanvasReadyState.BootFlowVersion,
            OrchestratorVersion = browserCanvasReadyState.OrchestratorVersion,
            CoordinatorVersion = browserCanvasReadyState.CoordinatorVersion,
            ControllerVersion = browserCanvasReadyState.ControllerVersion,
            DispatcherVersion = browserCanvasReadyState.DispatcherVersion,
            DriverVersion = browserCanvasReadyState.DriverVersion,
            StateMachineVersion = browserCanvasReadyState.StateMachineVersion,
            StateVersion = browserCanvasReadyState.StateVersion,
            CycleVersion = browserCanvasReadyState.CycleVersion,
            InvocationVersion = browserCanvasReadyState.InvocationVersion,
            LoopVersion = browserCanvasReadyState.LoopVersion,
            RunnerVersion = browserCanvasReadyState.RunnerVersion,
            ExecutionVersion = browserCanvasReadyState.ExecutionVersion,
            HandshakeVersion = browserCanvasReadyState.HandshakeVersion,
            PacketVersion = browserCanvasReadyState.PacketVersion,
            ContractVersion = browserCanvasReadyState.ContractVersion,
            Exists = browserCanvasReadyState.Exists,
            ReadSucceeded = browserCanvasReadyState.ReadSucceeded
        };

        if (!browserCanvasReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser render session blocked for profile '{browserCanvasReadyState.ProfileId}'.";
            result.Error = browserCanvasReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRenderSessionVersion = "runtime-browser-render-session-v1";
        result.LaunchMode = browserCanvasReadyState.LaunchMode;
        result.AssetRootPath = browserCanvasReadyState.AssetRootPath;
        result.ProfilesRootPath = browserCanvasReadyState.ProfilesRootPath;
        result.CacheRootPath = browserCanvasReadyState.CacheRootPath;
        result.ConfigRootPath = browserCanvasReadyState.ConfigRootPath;
        result.SettingsFilePath = browserCanvasReadyState.SettingsFilePath;
        result.StartupProfilePath = browserCanvasReadyState.StartupProfilePath;
        result.RequiredAssets = browserCanvasReadyState.RequiredAssets;
        result.ReadyAssetCount = browserCanvasReadyState.ReadyAssetCount;
        result.CompletedSteps = browserCanvasReadyState.CompletedSteps;
        result.TotalSteps = browserCanvasReadyState.TotalSteps;
        result.Phases = browserCanvasReadyState.Phases;
        result.CurrentState = browserCanvasReadyState.CurrentState;
        result.States = browserCanvasReadyState.States;
        result.Transitions = browserCanvasReadyState.Transitions;
        result.DispatchTargets = browserCanvasReadyState.DispatchTargets;
        result.ControlActions = browserCanvasReadyState.ControlActions;
        result.CoordinatorSteps = browserCanvasReadyState.CoordinatorSteps;
        result.OrchestrationStages = browserCanvasReadyState.OrchestrationStages;
        result.BootFlowStages = browserCanvasReadyState.BootFlowStages;
        result.BootSessionStages = browserCanvasReadyState.BootSessionStages;
        result.HandoffArtifacts = browserCanvasReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = browserCanvasReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = browserCanvasReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = browserCanvasReadyState.ClientBootstrapActions;
        result.ReadinessChecks = browserCanvasReadyState.ReadinessChecks;
        result.ReadySignals = browserCanvasReadyState.ReadySignals;
        result.LaunchControlActions = browserCanvasReadyState.LaunchControlActions;
        result.ReadyStates = browserCanvasReadyState.ReadyStates;
        result.ClientLaunchStages = browserCanvasReadyState.ClientLaunchStages;
        result.ActivationSteps = browserCanvasReadyState.ActivationSteps;
        result.RunStateStages = browserCanvasReadyState.RunStateStages;
        result.ClientLoopStages = browserCanvasReadyState.ClientLoopStages;
        result.HostSessionStages = browserCanvasReadyState.HostSessionStages;
        result.HostLoopStages = browserCanvasReadyState.HostLoopStages;
        result.HostReadyChecks = browserCanvasReadyState.HostReadyChecks;
        result.PlatformSessionStages = browserCanvasReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = browserCanvasReadyState.PlatformReadyChecks;
        result.PlatformLoopStages = browserCanvasReadyState.PlatformLoopStages;
        result.LaunchGateChecks = browserCanvasReadyState.LaunchGateChecks;
        result.BrowserShellStages = browserCanvasReadyState.BrowserShellStages;
        result.BrowserShellReadyChecks = browserCanvasReadyState.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserCanvasReadyState.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserCanvasReadyState.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserCanvasReadyState.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserCanvasReadyState.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserCanvasReadyState.BrowserFrameStages;
        result.BrowserFrameReadyChecks = browserCanvasReadyState.BrowserFrameReadyChecks;
        result.BrowserCanvasStages = browserCanvasReadyState.BrowserCanvasStages;
        result.BrowserCanvasReadyChecks = browserCanvasReadyState.BrowserCanvasReadyChecks;
        result.BrowserRenderStages =
        [
            "open-browser-render-session",
            "bind-browser-canvas-ready-state",
            "publish-browser-render-ready"
        ];
        result.BrowserRenderSummary = $"Runtime browser render session prepared {result.BrowserRenderStages.Length} browser-render stage(s) for profile '{browserCanvasReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser render session ready for profile '{browserCanvasReadyState.ProfileId}' with {result.BrowserRenderStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRenderSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserRenderSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
