namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPresentSession
{
    ValueTask<BrowserClientRuntimeBrowserPresentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPresentSessionService : IBrowserClientRuntimeBrowserPresentSession
{
    private readonly IBrowserClientRuntimeBrowserRenderReadyState _runtimeBrowserRenderReadyState;

    public BrowserClientRuntimeBrowserPresentSessionService(IBrowserClientRuntimeBrowserRenderReadyState runtimeBrowserRenderReadyState)
    {
        _runtimeBrowserRenderReadyState = runtimeBrowserRenderReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPresentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRenderReadyStateResult browserRenderReadyState = await _runtimeBrowserRenderReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPresentSessionResult result = new()
        {
            ProfileId = browserRenderReadyState.ProfileId,
            SessionId = browserRenderReadyState.SessionId,
            SessionPath = browserRenderReadyState.SessionPath,
            BrowserRenderReadyStateVersion = browserRenderReadyState.BrowserRenderReadyStateVersion,
            BrowserRenderSessionVersion = browserRenderReadyState.BrowserRenderSessionVersion,
            BrowserCanvasReadyStateVersion = browserRenderReadyState.BrowserCanvasReadyStateVersion,
            BrowserCanvasSessionVersion = browserRenderReadyState.BrowserCanvasSessionVersion,
            BrowserFrameReadyStateVersion = browserRenderReadyState.BrowserFrameReadyStateVersion,
            BrowserFrameSessionVersion = browserRenderReadyState.BrowserFrameSessionVersion,
            BrowserWindowReadyStateVersion = browserRenderReadyState.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserRenderReadyState.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserRenderReadyState.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserRenderReadyState.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserRenderReadyState.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserRenderReadyState.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserRenderReadyState.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserRenderReadyState.PlatformLoopVersion,
            PlatformReadyStateVersion = browserRenderReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = browserRenderReadyState.PlatformSessionVersion,
            HostReadyStateVersion = browserRenderReadyState.HostReadyStateVersion,
            HostLoopVersion = browserRenderReadyState.HostLoopVersion,
            HostSessionVersion = browserRenderReadyState.HostSessionVersion,
            ClientLoopStateVersion = browserRenderReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = browserRenderReadyState.ClientRunStateVersion,
            ClientActivationVersion = browserRenderReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = browserRenderReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserRenderReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = browserRenderReadyState.LaunchControllerVersion,
            ReadySignalVersion = browserRenderReadyState.ReadySignalVersion,
            ReadinessGateVersion = browserRenderReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = browserRenderReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = browserRenderReadyState.RuntimeSessionVersion,
            ConsumerVersion = browserRenderReadyState.ConsumerVersion,
            HandoffVersion = browserRenderReadyState.HandoffVersion,
            BootSessionVersion = browserRenderReadyState.BootSessionVersion,
            BootFlowVersion = browserRenderReadyState.BootFlowVersion,
            OrchestratorVersion = browserRenderReadyState.OrchestratorVersion,
            CoordinatorVersion = browserRenderReadyState.CoordinatorVersion,
            ControllerVersion = browserRenderReadyState.ControllerVersion,
            DispatcherVersion = browserRenderReadyState.DispatcherVersion,
            DriverVersion = browserRenderReadyState.DriverVersion,
            StateMachineVersion = browserRenderReadyState.StateMachineVersion,
            StateVersion = browserRenderReadyState.StateVersion,
            CycleVersion = browserRenderReadyState.CycleVersion,
            InvocationVersion = browserRenderReadyState.InvocationVersion,
            LoopVersion = browserRenderReadyState.LoopVersion,
            RunnerVersion = browserRenderReadyState.RunnerVersion,
            ExecutionVersion = browserRenderReadyState.ExecutionVersion,
            HandshakeVersion = browserRenderReadyState.HandshakeVersion,
            PacketVersion = browserRenderReadyState.PacketVersion,
            ContractVersion = browserRenderReadyState.ContractVersion,
            Exists = browserRenderReadyState.Exists,
            ReadSucceeded = browserRenderReadyState.ReadSucceeded
        };

        if (!browserRenderReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser present session blocked for profile '{browserRenderReadyState.ProfileId}'.";
            result.Error = browserRenderReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPresentSessionVersion = "runtime-browser-present-session-v1";
        result.LaunchMode = browserRenderReadyState.LaunchMode;
        result.AssetRootPath = browserRenderReadyState.AssetRootPath;
        result.ProfilesRootPath = browserRenderReadyState.ProfilesRootPath;
        result.CacheRootPath = browserRenderReadyState.CacheRootPath;
        result.ConfigRootPath = browserRenderReadyState.ConfigRootPath;
        result.SettingsFilePath = browserRenderReadyState.SettingsFilePath;
        result.StartupProfilePath = browserRenderReadyState.StartupProfilePath;
        result.RequiredAssets = browserRenderReadyState.RequiredAssets;
        result.ReadyAssetCount = browserRenderReadyState.ReadyAssetCount;
        result.CompletedSteps = browserRenderReadyState.CompletedSteps;
        result.TotalSteps = browserRenderReadyState.TotalSteps;
        result.Phases = browserRenderReadyState.Phases;
        result.CurrentState = browserRenderReadyState.CurrentState;
        result.States = browserRenderReadyState.States;
        result.Transitions = browserRenderReadyState.Transitions;
        result.DispatchTargets = browserRenderReadyState.DispatchTargets;
        result.ControlActions = browserRenderReadyState.ControlActions;
        result.CoordinatorSteps = browserRenderReadyState.CoordinatorSteps;
        result.OrchestrationStages = browserRenderReadyState.OrchestrationStages;
        result.BootFlowStages = browserRenderReadyState.BootFlowStages;
        result.BootSessionStages = browserRenderReadyState.BootSessionStages;
        result.HandoffArtifacts = browserRenderReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = browserRenderReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = browserRenderReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = browserRenderReadyState.ClientBootstrapActions;
        result.ReadinessChecks = browserRenderReadyState.ReadinessChecks;
        result.ReadySignals = browserRenderReadyState.ReadySignals;
        result.LaunchControlActions = browserRenderReadyState.LaunchControlActions;
        result.ReadyStates = browserRenderReadyState.ReadyStates;
        result.ClientLaunchStages = browserRenderReadyState.ClientLaunchStages;
        result.ActivationSteps = browserRenderReadyState.ActivationSteps;
        result.RunStateStages = browserRenderReadyState.RunStateStages;
        result.ClientLoopStages = browserRenderReadyState.ClientLoopStages;
        result.HostSessionStages = browserRenderReadyState.HostSessionStages;
        result.HostLoopStages = browserRenderReadyState.HostLoopStages;
        result.HostReadyChecks = browserRenderReadyState.HostReadyChecks;
        result.PlatformSessionStages = browserRenderReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = browserRenderReadyState.PlatformReadyChecks;
        result.PlatformLoopStages = browserRenderReadyState.PlatformLoopStages;
        result.LaunchGateChecks = browserRenderReadyState.LaunchGateChecks;
        result.BrowserShellStages = browserRenderReadyState.BrowserShellStages;
        result.BrowserShellReadyChecks = browserRenderReadyState.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserRenderReadyState.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserRenderReadyState.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserRenderReadyState.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserRenderReadyState.BrowserWindowReadyChecks;
        result.BrowserFrameStages = browserRenderReadyState.BrowserFrameStages;
        result.BrowserFrameReadyChecks = browserRenderReadyState.BrowserFrameReadyChecks;
        result.BrowserCanvasStages = browserRenderReadyState.BrowserCanvasStages;
        result.BrowserCanvasReadyChecks = browserRenderReadyState.BrowserCanvasReadyChecks;
        result.BrowserRenderStages = browserRenderReadyState.BrowserRenderStages;
        result.BrowserRenderReadyChecks = browserRenderReadyState.BrowserRenderReadyChecks;
        result.BrowserPresentStages =
        [
            "open-browser-present-session",
            "bind-browser-render-ready-state",
            "publish-browser-present-ready"
        ];
        result.BrowserPresentSummary = $"Runtime browser present session prepared {result.BrowserPresentStages.Length} browser-present stage(s) for profile '{browserRenderReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser present session ready for profile '{browserRenderReadyState.ProfileId}' with {result.BrowserPresentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPresentSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserPresentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
