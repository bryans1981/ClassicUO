namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFrameSession
{
    ValueTask<BrowserClientRuntimeBrowserFrameSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFrameSessionService : IBrowserClientRuntimeBrowserFrameSession
{
    private readonly IBrowserClientRuntimeBrowserWindowReadyState _runtimeBrowserWindowReadyState;

    public BrowserClientRuntimeBrowserFrameSessionService(IBrowserClientRuntimeBrowserWindowReadyState runtimeBrowserWindowReadyState)
    {
        _runtimeBrowserWindowReadyState = runtimeBrowserWindowReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFrameSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserWindowReadyStateResult browserWindowReadyState = await _runtimeBrowserWindowReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFrameSessionResult result = new()
        {
            ProfileId = browserWindowReadyState.ProfileId,
            SessionId = browserWindowReadyState.SessionId,
            SessionPath = browserWindowReadyState.SessionPath,
            BrowserWindowReadyStateVersion = browserWindowReadyState.BrowserWindowReadyStateVersion,
            BrowserWindowSessionVersion = browserWindowReadyState.BrowserWindowSessionVersion,
            BrowserSurfaceReadyStateVersion = browserWindowReadyState.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserWindowReadyState.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserWindowReadyState.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserWindowReadyState.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserWindowReadyState.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserWindowReadyState.PlatformLoopVersion,
            PlatformReadyStateVersion = browserWindowReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = browserWindowReadyState.PlatformSessionVersion,
            HostReadyStateVersion = browserWindowReadyState.HostReadyStateVersion,
            HostLoopVersion = browserWindowReadyState.HostLoopVersion,
            HostSessionVersion = browserWindowReadyState.HostSessionVersion,
            ClientLoopStateVersion = browserWindowReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = browserWindowReadyState.ClientRunStateVersion,
            ClientActivationVersion = browserWindowReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = browserWindowReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserWindowReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = browserWindowReadyState.LaunchControllerVersion,
            ReadySignalVersion = browserWindowReadyState.ReadySignalVersion,
            ReadinessGateVersion = browserWindowReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = browserWindowReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = browserWindowReadyState.RuntimeSessionVersion,
            ConsumerVersion = browserWindowReadyState.ConsumerVersion,
            HandoffVersion = browserWindowReadyState.HandoffVersion,
            BootSessionVersion = browserWindowReadyState.BootSessionVersion,
            BootFlowVersion = browserWindowReadyState.BootFlowVersion,
            OrchestratorVersion = browserWindowReadyState.OrchestratorVersion,
            CoordinatorVersion = browserWindowReadyState.CoordinatorVersion,
            ControllerVersion = browserWindowReadyState.ControllerVersion,
            DispatcherVersion = browserWindowReadyState.DispatcherVersion,
            DriverVersion = browserWindowReadyState.DriverVersion,
            StateMachineVersion = browserWindowReadyState.StateMachineVersion,
            StateVersion = browserWindowReadyState.StateVersion,
            CycleVersion = browserWindowReadyState.CycleVersion,
            InvocationVersion = browserWindowReadyState.InvocationVersion,
            LoopVersion = browserWindowReadyState.LoopVersion,
            RunnerVersion = browserWindowReadyState.RunnerVersion,
            ExecutionVersion = browserWindowReadyState.ExecutionVersion,
            HandshakeVersion = browserWindowReadyState.HandshakeVersion,
            PacketVersion = browserWindowReadyState.PacketVersion,
            ContractVersion = browserWindowReadyState.ContractVersion,
            Exists = browserWindowReadyState.Exists,
            ReadSucceeded = browserWindowReadyState.ReadSucceeded
        };

        if (!browserWindowReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser frame session blocked for profile '{browserWindowReadyState.ProfileId}'.";
            result.Error = browserWindowReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFrameSessionVersion = "runtime-browser-frame-session-v1";
        result.LaunchMode = browserWindowReadyState.LaunchMode;
        result.AssetRootPath = browserWindowReadyState.AssetRootPath;
        result.ProfilesRootPath = browserWindowReadyState.ProfilesRootPath;
        result.CacheRootPath = browserWindowReadyState.CacheRootPath;
        result.ConfigRootPath = browserWindowReadyState.ConfigRootPath;
        result.SettingsFilePath = browserWindowReadyState.SettingsFilePath;
        result.StartupProfilePath = browserWindowReadyState.StartupProfilePath;
        result.RequiredAssets = browserWindowReadyState.RequiredAssets;
        result.ReadyAssetCount = browserWindowReadyState.ReadyAssetCount;
        result.CompletedSteps = browserWindowReadyState.CompletedSteps;
        result.TotalSteps = browserWindowReadyState.TotalSteps;
        result.Phases = browserWindowReadyState.Phases;
        result.CurrentState = browserWindowReadyState.CurrentState;
        result.States = browserWindowReadyState.States;
        result.Transitions = browserWindowReadyState.Transitions;
        result.DispatchTargets = browserWindowReadyState.DispatchTargets;
        result.ControlActions = browserWindowReadyState.ControlActions;
        result.CoordinatorSteps = browserWindowReadyState.CoordinatorSteps;
        result.OrchestrationStages = browserWindowReadyState.OrchestrationStages;
        result.BootFlowStages = browserWindowReadyState.BootFlowStages;
        result.BootSessionStages = browserWindowReadyState.BootSessionStages;
        result.HandoffArtifacts = browserWindowReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = browserWindowReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = browserWindowReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = browserWindowReadyState.ClientBootstrapActions;
        result.ReadinessChecks = browserWindowReadyState.ReadinessChecks;
        result.ReadySignals = browserWindowReadyState.ReadySignals;
        result.LaunchControlActions = browserWindowReadyState.LaunchControlActions;
        result.ReadyStates = browserWindowReadyState.ReadyStates;
        result.ClientLaunchStages = browserWindowReadyState.ClientLaunchStages;
        result.ActivationSteps = browserWindowReadyState.ActivationSteps;
        result.RunStateStages = browserWindowReadyState.RunStateStages;
        result.ClientLoopStages = browserWindowReadyState.ClientLoopStages;
        result.HostSessionStages = browserWindowReadyState.HostSessionStages;
        result.HostLoopStages = browserWindowReadyState.HostLoopStages;
        result.HostReadyChecks = browserWindowReadyState.HostReadyChecks;
        result.PlatformSessionStages = browserWindowReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = browserWindowReadyState.PlatformReadyChecks;
        result.PlatformLoopStages = browserWindowReadyState.PlatformLoopStages;
        result.LaunchGateChecks = browserWindowReadyState.LaunchGateChecks;
        result.BrowserShellStages = browserWindowReadyState.BrowserShellStages;
        result.BrowserShellReadyChecks = browserWindowReadyState.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserWindowReadyState.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserWindowReadyState.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages = browserWindowReadyState.BrowserWindowStages;
        result.BrowserWindowReadyChecks = browserWindowReadyState.BrowserWindowReadyChecks;
        result.BrowserFrameStages =
        [
            "open-browser-frame-session",
            "bind-browser-window-ready-state",
            "publish-browser-frame-ready"
        ];
        result.BrowserFrameSummary = $"Runtime browser frame session prepared {result.BrowserFrameStages.Length} browser-frame stage(s) for profile '{browserWindowReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser frame session ready for profile '{browserWindowReadyState.ProfileId}' with {result.BrowserFrameStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFrameSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserFrameSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
