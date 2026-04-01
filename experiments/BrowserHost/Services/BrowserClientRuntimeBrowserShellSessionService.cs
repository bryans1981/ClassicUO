namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserShellSession
{
    ValueTask<BrowserClientRuntimeBrowserShellSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserShellSessionService : IBrowserClientRuntimeBrowserShellSession
{
    private readonly IBrowserClientRuntimePlatformLaunchGate _runtimePlatformLaunchGate;

    public BrowserClientRuntimeBrowserShellSessionService(IBrowserClientRuntimePlatformLaunchGate runtimePlatformLaunchGate)
    {
        _runtimePlatformLaunchGate = runtimePlatformLaunchGate;
    }

    public async ValueTask<BrowserClientRuntimeBrowserShellSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimePlatformLaunchGateResult launchGate = await _runtimePlatformLaunchGate.EvaluateAsync(profileId);

        BrowserClientRuntimeBrowserShellSessionResult result = new()
        {
            ProfileId = launchGate.ProfileId,
            SessionId = launchGate.SessionId,
            SessionPath = launchGate.SessionPath,
            PlatformLaunchGateVersion = launchGate.PlatformLaunchGateVersion,
            PlatformLoopVersion = launchGate.PlatformLoopVersion,
            PlatformReadyStateVersion = launchGate.PlatformReadyStateVersion,
            PlatformSessionVersion = launchGate.PlatformSessionVersion,
            HostReadyStateVersion = launchGate.HostReadyStateVersion,
            HostLoopVersion = launchGate.HostLoopVersion,
            HostSessionVersion = launchGate.HostSessionVersion,
            ClientLoopStateVersion = launchGate.ClientLoopStateVersion,
            ClientRunStateVersion = launchGate.ClientRunStateVersion,
            ClientActivationVersion = launchGate.ClientActivationVersion,
            ClientLaunchSessionVersion = launchGate.ClientLaunchSessionVersion,
            ClientReadyStateVersion = launchGate.ClientReadyStateVersion,
            LaunchControllerVersion = launchGate.LaunchControllerVersion,
            ReadySignalVersion = launchGate.ReadySignalVersion,
            ReadinessGateVersion = launchGate.ReadinessGateVersion,
            ClientBootstrapVersion = launchGate.ClientBootstrapVersion,
            RuntimeSessionVersion = launchGate.RuntimeSessionVersion,
            ConsumerVersion = launchGate.ConsumerVersion,
            HandoffVersion = launchGate.HandoffVersion,
            BootSessionVersion = launchGate.BootSessionVersion,
            BootFlowVersion = launchGate.BootFlowVersion,
            OrchestratorVersion = launchGate.OrchestratorVersion,
            CoordinatorVersion = launchGate.CoordinatorVersion,
            ControllerVersion = launchGate.ControllerVersion,
            DispatcherVersion = launchGate.DispatcherVersion,
            DriverVersion = launchGate.DriverVersion,
            StateMachineVersion = launchGate.StateMachineVersion,
            StateVersion = launchGate.StateVersion,
            CycleVersion = launchGate.CycleVersion,
            InvocationVersion = launchGate.InvocationVersion,
            LoopVersion = launchGate.LoopVersion,
            RunnerVersion = launchGate.RunnerVersion,
            ExecutionVersion = launchGate.ExecutionVersion,
            HandshakeVersion = launchGate.HandshakeVersion,
            PacketVersion = launchGate.PacketVersion,
            ContractVersion = launchGate.ContractVersion,
            Exists = launchGate.Exists,
            ReadSucceeded = launchGate.ReadSucceeded
        };

        if (!launchGate.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser shell session blocked for profile '{launchGate.ProfileId}'.";
            result.Error = launchGate.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserShellSessionVersion = "runtime-browser-shell-session-v1";
        result.LaunchMode = launchGate.LaunchMode;
        result.AssetRootPath = launchGate.AssetRootPath;
        result.ProfilesRootPath = launchGate.ProfilesRootPath;
        result.CacheRootPath = launchGate.CacheRootPath;
        result.ConfigRootPath = launchGate.ConfigRootPath;
        result.SettingsFilePath = launchGate.SettingsFilePath;
        result.StartupProfilePath = launchGate.StartupProfilePath;
        result.RequiredAssets = launchGate.RequiredAssets;
        result.ReadyAssetCount = launchGate.ReadyAssetCount;
        result.CompletedSteps = launchGate.CompletedSteps;
        result.TotalSteps = launchGate.TotalSteps;
        result.Phases = launchGate.Phases;
        result.CurrentState = launchGate.CurrentState;
        result.States = launchGate.States;
        result.Transitions = launchGate.Transitions;
        result.DispatchTargets = launchGate.DispatchTargets;
        result.ControlActions = launchGate.ControlActions;
        result.CoordinatorSteps = launchGate.CoordinatorSteps;
        result.OrchestrationStages = launchGate.OrchestrationStages;
        result.BootFlowStages = launchGate.BootFlowStages;
        result.BootSessionStages = launchGate.BootSessionStages;
        result.HandoffArtifacts = launchGate.HandoffArtifacts;
        result.ConsumedArtifacts = launchGate.ConsumedArtifacts;
        result.RuntimeSessionStages = launchGate.RuntimeSessionStages;
        result.ClientBootstrapActions = launchGate.ClientBootstrapActions;
        result.ReadinessChecks = launchGate.ReadinessChecks;
        result.ReadySignals = launchGate.ReadySignals;
        result.LaunchControlActions = launchGate.LaunchControlActions;
        result.ReadyStates = launchGate.ReadyStates;
        result.ClientLaunchStages = launchGate.ClientLaunchStages;
        result.ActivationSteps = launchGate.ActivationSteps;
        result.RunStateStages = launchGate.RunStateStages;
        result.ClientLoopStages = launchGate.ClientLoopStages;
        result.HostSessionStages = launchGate.HostSessionStages;
        result.HostLoopStages = launchGate.HostLoopStages;
        result.HostReadyChecks = launchGate.HostReadyChecks;
        result.PlatformSessionStages = launchGate.PlatformSessionStages;
        result.PlatformReadyChecks = launchGate.PlatformReadyChecks;
        result.PlatformLoopStages = launchGate.PlatformLoopStages;
        result.LaunchGateChecks = launchGate.LaunchGateChecks;
        result.BrowserShellStages =
        [
            "open-browser-shell-session",
            "bind-platform-launch-gate",
            "publish-browser-shell-ready"
        ];
        result.BrowserShellSummary = $"Runtime browser shell session prepared {result.BrowserShellStages.Length} browser-shell stage(s) for profile '{launchGate.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser shell session ready for profile '{launchGate.ProfileId}' with {result.BrowserShellStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserShellSessionResult
{
    public bool IsReady { get; set; }
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
    public string BrowserShellSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
