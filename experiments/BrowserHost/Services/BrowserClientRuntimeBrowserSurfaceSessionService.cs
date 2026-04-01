namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSurfaceSession
{
    ValueTask<BrowserClientRuntimeBrowserSurfaceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSurfaceSessionService : IBrowserClientRuntimeBrowserSurfaceSession
{
    private readonly IBrowserClientRuntimeBrowserShellReadyState _runtimeBrowserShellReadyState;

    public BrowserClientRuntimeBrowserSurfaceSessionService(IBrowserClientRuntimeBrowserShellReadyState runtimeBrowserShellReadyState)
    {
        _runtimeBrowserShellReadyState = runtimeBrowserShellReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSurfaceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserShellReadyStateResult browserShellReadyState = await _runtimeBrowserShellReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSurfaceSessionResult result = new()
        {
            ProfileId = browserShellReadyState.ProfileId,
            SessionId = browserShellReadyState.SessionId,
            SessionPath = browserShellReadyState.SessionPath,
            BrowserShellReadyStateVersion = browserShellReadyState.BrowserShellReadyStateVersion,
            BrowserShellSessionVersion = browserShellReadyState.BrowserShellSessionVersion,
            PlatformLaunchGateVersion = browserShellReadyState.PlatformLaunchGateVersion,
            PlatformLoopVersion = browserShellReadyState.PlatformLoopVersion,
            PlatformReadyStateVersion = browserShellReadyState.PlatformReadyStateVersion,
            PlatformSessionVersion = browserShellReadyState.PlatformSessionVersion,
            HostReadyStateVersion = browserShellReadyState.HostReadyStateVersion,
            HostLoopVersion = browserShellReadyState.HostLoopVersion,
            HostSessionVersion = browserShellReadyState.HostSessionVersion,
            ClientLoopStateVersion = browserShellReadyState.ClientLoopStateVersion,
            ClientRunStateVersion = browserShellReadyState.ClientRunStateVersion,
            ClientActivationVersion = browserShellReadyState.ClientActivationVersion,
            ClientLaunchSessionVersion = browserShellReadyState.ClientLaunchSessionVersion,
            ClientReadyStateVersion = browserShellReadyState.ClientReadyStateVersion,
            LaunchControllerVersion = browserShellReadyState.LaunchControllerVersion,
            ReadySignalVersion = browserShellReadyState.ReadySignalVersion,
            ReadinessGateVersion = browserShellReadyState.ReadinessGateVersion,
            ClientBootstrapVersion = browserShellReadyState.ClientBootstrapVersion,
            RuntimeSessionVersion = browserShellReadyState.RuntimeSessionVersion,
            ConsumerVersion = browserShellReadyState.ConsumerVersion,
            HandoffVersion = browserShellReadyState.HandoffVersion,
            BootSessionVersion = browserShellReadyState.BootSessionVersion,
            BootFlowVersion = browserShellReadyState.BootFlowVersion,
            OrchestratorVersion = browserShellReadyState.OrchestratorVersion,
            CoordinatorVersion = browserShellReadyState.CoordinatorVersion,
            ControllerVersion = browserShellReadyState.ControllerVersion,
            DispatcherVersion = browserShellReadyState.DispatcherVersion,
            DriverVersion = browserShellReadyState.DriverVersion,
            StateMachineVersion = browserShellReadyState.StateMachineVersion,
            StateVersion = browserShellReadyState.StateVersion,
            CycleVersion = browserShellReadyState.CycleVersion,
            InvocationVersion = browserShellReadyState.InvocationVersion,
            LoopVersion = browserShellReadyState.LoopVersion,
            RunnerVersion = browserShellReadyState.RunnerVersion,
            ExecutionVersion = browserShellReadyState.ExecutionVersion,
            HandshakeVersion = browserShellReadyState.HandshakeVersion,
            PacketVersion = browserShellReadyState.PacketVersion,
            ContractVersion = browserShellReadyState.ContractVersion,
            Exists = browserShellReadyState.Exists,
            ReadSucceeded = browserShellReadyState.ReadSucceeded
        };

        if (!browserShellReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser surface session blocked for profile '{browserShellReadyState.ProfileId}'.";
            result.Error = browserShellReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSurfaceSessionVersion = "runtime-browser-surface-session-v1";
        result.LaunchMode = browserShellReadyState.LaunchMode;
        result.AssetRootPath = browserShellReadyState.AssetRootPath;
        result.ProfilesRootPath = browserShellReadyState.ProfilesRootPath;
        result.CacheRootPath = browserShellReadyState.CacheRootPath;
        result.ConfigRootPath = browserShellReadyState.ConfigRootPath;
        result.SettingsFilePath = browserShellReadyState.SettingsFilePath;
        result.StartupProfilePath = browserShellReadyState.StartupProfilePath;
        result.RequiredAssets = browserShellReadyState.RequiredAssets;
        result.ReadyAssetCount = browserShellReadyState.ReadyAssetCount;
        result.CompletedSteps = browserShellReadyState.CompletedSteps;
        result.TotalSteps = browserShellReadyState.TotalSteps;
        result.Phases = browserShellReadyState.Phases;
        result.CurrentState = browserShellReadyState.CurrentState;
        result.States = browserShellReadyState.States;
        result.Transitions = browserShellReadyState.Transitions;
        result.DispatchTargets = browserShellReadyState.DispatchTargets;
        result.ControlActions = browserShellReadyState.ControlActions;
        result.CoordinatorSteps = browserShellReadyState.CoordinatorSteps;
        result.OrchestrationStages = browserShellReadyState.OrchestrationStages;
        result.BootFlowStages = browserShellReadyState.BootFlowStages;
        result.BootSessionStages = browserShellReadyState.BootSessionStages;
        result.HandoffArtifacts = browserShellReadyState.HandoffArtifacts;
        result.ConsumedArtifacts = browserShellReadyState.ConsumedArtifacts;
        result.RuntimeSessionStages = browserShellReadyState.RuntimeSessionStages;
        result.ClientBootstrapActions = browserShellReadyState.ClientBootstrapActions;
        result.ReadinessChecks = browserShellReadyState.ReadinessChecks;
        result.ReadySignals = browserShellReadyState.ReadySignals;
        result.LaunchControlActions = browserShellReadyState.LaunchControlActions;
        result.ReadyStates = browserShellReadyState.ReadyStates;
        result.ClientLaunchStages = browserShellReadyState.ClientLaunchStages;
        result.ActivationSteps = browserShellReadyState.ActivationSteps;
        result.RunStateStages = browserShellReadyState.RunStateStages;
        result.ClientLoopStages = browserShellReadyState.ClientLoopStages;
        result.HostSessionStages = browserShellReadyState.HostSessionStages;
        result.HostLoopStages = browserShellReadyState.HostLoopStages;
        result.HostReadyChecks = browserShellReadyState.HostReadyChecks;
        result.PlatformSessionStages = browserShellReadyState.PlatformSessionStages;
        result.PlatformReadyChecks = browserShellReadyState.PlatformReadyChecks;
        result.PlatformLoopStages = browserShellReadyState.PlatformLoopStages;
        result.LaunchGateChecks = browserShellReadyState.LaunchGateChecks;
        result.BrowserShellStages = browserShellReadyState.BrowserShellStages;
        result.BrowserShellReadyChecks = browserShellReadyState.BrowserShellReadyChecks;
        result.BrowserSurfaceStages =
        [
            "open-browser-surface",
            "bind-browser-shell-ready-state",
            "publish-browser-surface-ready"
        ];
        result.BrowserSurfaceSummary = $"Runtime browser surface session prepared {result.BrowserSurfaceStages.Length} browser-surface stage(s) for profile '{browserShellReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser surface session ready for profile '{browserShellReadyState.ProfileId}' with {result.BrowserSurfaceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSurfaceSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserSurfaceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
