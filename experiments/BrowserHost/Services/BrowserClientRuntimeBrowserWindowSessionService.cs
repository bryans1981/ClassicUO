namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserWindowSession
{
    ValueTask<BrowserClientRuntimeBrowserWindowSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserWindowSessionService : IBrowserClientRuntimeBrowserWindowSession
{
    private readonly IBrowserClientRuntimeBrowserSurfaceReadyState _runtimeBrowserSurfaceReadyState;

    public BrowserClientRuntimeBrowserWindowSessionService(IBrowserClientRuntimeBrowserSurfaceReadyState runtimeBrowserSurfaceReadyState)
    {
        _runtimeBrowserSurfaceReadyState = runtimeBrowserSurfaceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserWindowSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSurfaceReadyStateResult browserSurfaceReadyState = await _runtimeBrowserSurfaceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserWindowSessionResult result = new()
        {
            ProfileId = browserSurfaceReadyState.ProfileId,
            SessionId = browserSurfaceReadyState.SessionId,
            SessionPath = browserSurfaceReadyState.SessionPath,
            BrowserSurfaceReadyStateVersion = browserSurfaceReadyState.BrowserSurfaceReadyStateVersion,
            BrowserSurfaceSessionVersion = browserSurfaceReadyState.BrowserSurfaceSessionVersion,
            BrowserShellReadyStateVersion = browserSurfaceReadyState.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserSurfaceReadyState.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserSurfaceReadyState.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserSurfaceReadyState.PlatformLoopVersion,
            PlatformReadyStateVersion = browserSurfaceReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = browserSurfaceReadyState.PlatformSessionVersion,
            HostReadyStateVersion = browserSurfaceReadyState.HostReadyStateVersion,
            HostLoopVersion = browserSurfaceReadyState.HostLoopVersion,
            HostSessionVersion = browserSurfaceReadyState.HostSessionVersion,
            ClientLoopStateVersion = browserSurfaceReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = browserSurfaceReadyState.ClientRunStateVersion,
            ClientActivationVersion = browserSurfaceReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = browserSurfaceReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserSurfaceReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = browserSurfaceReadyState.LaunchControllerVersion,
            ReadySignalVersion = browserSurfaceReadyState.ReadySignalVersion,
            ReadinessGateVersion = browserSurfaceReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = browserSurfaceReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = browserSurfaceReadyState.RuntimeSessionVersion,
            ConsumerVersion = browserSurfaceReadyState.ConsumerVersion,
            HandoffVersion = browserSurfaceReadyState.HandoffVersion,
            BootSessionVersion = browserSurfaceReadyState.BootSessionVersion,
            BootFlowVersion = browserSurfaceReadyState.BootFlowVersion,
            OrchestratorVersion = browserSurfaceReadyState.OrchestratorVersion,
            CoordinatorVersion = browserSurfaceReadyState.CoordinatorVersion,
            ControllerVersion = browserSurfaceReadyState.ControllerVersion,
            DispatcherVersion = browserSurfaceReadyState.DispatcherVersion,
            DriverVersion = browserSurfaceReadyState.DriverVersion,
            StateMachineVersion = browserSurfaceReadyState.StateMachineVersion,
            StateVersion = browserSurfaceReadyState.StateVersion,
            CycleVersion = browserSurfaceReadyState.CycleVersion,
            InvocationVersion = browserSurfaceReadyState.InvocationVersion,
            LoopVersion = browserSurfaceReadyState.LoopVersion,
            RunnerVersion = browserSurfaceReadyState.RunnerVersion,
            ExecutionVersion = browserSurfaceReadyState.ExecutionVersion,
            HandshakeVersion = browserSurfaceReadyState.HandshakeVersion,
            PacketVersion = browserSurfaceReadyState.PacketVersion,
            ContractVersion = browserSurfaceReadyState.ContractVersion,
            Exists = browserSurfaceReadyState.Exists,
            ReadSucceeded = browserSurfaceReadyState.ReadSucceeded
        };

        if (!browserSurfaceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser window session blocked for profile '{browserSurfaceReadyState.ProfileId}'.";
            result.Error = browserSurfaceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserWindowSessionVersion = "runtime-browser-window-session-v1";
        result.LaunchMode = browserSurfaceReadyState.LaunchMode;
        result.AssetRootPath = browserSurfaceReadyState.AssetRootPath;
        result.ProfilesRootPath = browserSurfaceReadyState.ProfilesRootPath;
        result.CacheRootPath = browserSurfaceReadyState.CacheRootPath;
        result.ConfigRootPath = browserSurfaceReadyState.ConfigRootPath;
        result.SettingsFilePath = browserSurfaceReadyState.SettingsFilePath;
        result.StartupProfilePath = browserSurfaceReadyState.StartupProfilePath;
        result.RequiredAssets = browserSurfaceReadyState.RequiredAssets;
        result.ReadyAssetCount = browserSurfaceReadyState.ReadyAssetCount;
        result.CompletedSteps = browserSurfaceReadyState.CompletedSteps;
        result.TotalSteps = browserSurfaceReadyState.TotalSteps;
        result.Phases = browserSurfaceReadyState.Phases;
        result.CurrentState = browserSurfaceReadyState.CurrentState;
        result.States = browserSurfaceReadyState.States;
        result.Transitions = browserSurfaceReadyState.Transitions;
        result.DispatchTargets = browserSurfaceReadyState.DispatchTargets;
        result.ControlActions = browserSurfaceReadyState.ControlActions;
        result.CoordinatorSteps = browserSurfaceReadyState.CoordinatorSteps;
        result.OrchestrationStages = browserSurfaceReadyState.OrchestrationStages;
        result.BootFlowStages = browserSurfaceReadyState.BootFlowStages;
        result.BootSessionStages = browserSurfaceReadyState.BootSessionStages;
        result.HandoffArtifacts = browserSurfaceReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = browserSurfaceReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = browserSurfaceReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = browserSurfaceReadyState.ClientBootstrapActions;
        result.ReadinessChecks = browserSurfaceReadyState.ReadinessChecks;
        result.ReadySignals = browserSurfaceReadyState.ReadySignals;
        result.LaunchControlActions = browserSurfaceReadyState.LaunchControlActions;
        result.ReadyStates = browserSurfaceReadyState.ReadyStates;
        result.ClientLaunchStages = browserSurfaceReadyState.ClientLaunchStages;
        result.ActivationSteps = browserSurfaceReadyState.ActivationSteps;
        result.RunStateStages = browserSurfaceReadyState.RunStateStages;
        result.ClientLoopStages = browserSurfaceReadyState.ClientLoopStages;
        result.HostSessionStages = browserSurfaceReadyState.HostSessionStages;
        result.HostLoopStages = browserSurfaceReadyState.HostLoopStages;
        result.HostReadyChecks = browserSurfaceReadyState.HostReadyChecks;
        result.PlatformSessionStages = browserSurfaceReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = browserSurfaceReadyState.PlatformReadyChecks;
        result.PlatformLoopStages = browserSurfaceReadyState.PlatformLoopStages;
        result.LaunchGateChecks = browserSurfaceReadyState.LaunchGateChecks;
        result.BrowserShellStages = browserSurfaceReadyState.BrowserShellStages;
        result.BrowserShellReadyChecks = browserSurfaceReadyState.BrowserShellReadyChecks;
        result.BrowserSurfaceStages = browserSurfaceReadyState.BrowserSurfaceStages;
        result.BrowserSurfaceReadyChecks = browserSurfaceReadyState.BrowserSurfaceReadyChecks;
        result.BrowserWindowStages =
        [
            "open-browser-window-session",
            "bind-browser-surface-ready-state",
            "publish-browser-window-ready"
        ];
        result.BrowserWindowSummary = $"Runtime browser window session prepared {result.BrowserWindowStages.Length} browser-window stage(s) for profile '{browserSurfaceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser window session ready for profile '{browserSurfaceReadyState.ProfileId}' with {result.BrowserWindowStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserWindowSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserWindowSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
