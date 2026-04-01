namespace BrowserHost.Services;

public interface IBrowserClientRuntimeHostLoop
{
    ValueTask<BrowserClientRuntimeHostLoopResult> RunAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeHostLoopService : IBrowserClientRuntimeHostLoop
{
    private readonly IBrowserClientRuntimeHostSession _runtimeHostSession;

    public BrowserClientRuntimeHostLoopService(IBrowserClientRuntimeHostSession runtimeHostSession)
    {
        _runtimeHostSession = runtimeHostSession;
    }

    public async ValueTask<BrowserClientRuntimeHostLoopResult> RunAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeHostSessionResult hostSession = await _runtimeHostSession.CreateAsync(profileId);

        BrowserClientRuntimeHostLoopResult result = new()
        {
            ProfileId = hostSession.ProfileId,
            SessionId = hostSession.SessionId,
            SessionPath = hostSession.SessionPath,
            HostSessionVersion = hostSession.HostSessionVersion,
            ClientLoopStateVersion = hostSession.ClientLoopStateVersion,
            ClientRunStateVersion = hostSession.ClientRunStateVersion,
            ClientActivationVersion = hostSession.ClientActivationVersion,
            ClientLaunchSessionVersion = hostSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = hostSession.ClientReadyStateVersion,
            LaunchControllerVersion = hostSession.LaunchControllerVersion,
            ReadySignalVersion = hostSession.ReadySignalVersion,
            ReadinessGateVersion = hostSession.ReadinessGateVersion,
            ClientBootstrapVersion = hostSession.ClientBootstrapVersion,
            RuntimeSessionVersion = hostSession.RuntimeSessionVersion,
            ConsumerVersion = hostSession.ConsumerVersion,
            HandoffVersion = hostSession.HandoffVersion,
            BootSessionVersion = hostSession.BootSessionVersion,
            BootFlowVersion = hostSession.BootFlowVersion,
            OrchestratorVersion = hostSession.OrchestratorVersion,
            CoordinatorVersion = hostSession.CoordinatorVersion,
            ControllerVersion = hostSession.ControllerVersion,
            DispatcherVersion = hostSession.DispatcherVersion,
            DriverVersion = hostSession.DriverVersion,
            StateMachineVersion = hostSession.StateMachineVersion,
            StateVersion = hostSession.StateVersion,
            CycleVersion = hostSession.CycleVersion,
            InvocationVersion = hostSession.InvocationVersion,
            LoopVersion = hostSession.LoopVersion,
            RunnerVersion = hostSession.RunnerVersion,
            ExecutionVersion = hostSession.ExecutionVersion,
            HandshakeVersion = hostSession.HandshakeVersion,
            PacketVersion = hostSession.PacketVersion,
            ContractVersion = hostSession.ContractVersion,
            Exists = hostSession.Exists,
            ReadSucceeded = hostSession.ReadSucceeded
        };

        if (!hostSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime host loop blocked for profile '{hostSession.ProfileId}'.";
            result.Error = hostSession.Error;
            return result;
        }

        result.IsReady = true;
        result.HostLoopVersion = "runtime-host-loop-v1";
        result.LaunchMode = hostSession.LaunchMode;
        result.AssetRootPath = hostSession.AssetRootPath;
        result.ProfilesRootPath = hostSession.ProfilesRootPath;
        result.CacheRootPath = hostSession.CacheRootPath;
        result.ConfigRootPath = hostSession.ConfigRootPath;
        result.SettingsFilePath = hostSession.SettingsFilePath;
        result.StartupProfilePath = hostSession.StartupProfilePath;
        result.RequiredAssets = hostSession.RequiredAssets;
        result.ReadyAssetCount = hostSession.ReadyAssetCount;
        result.CompletedSteps = hostSession.CompletedSteps;
        result.TotalSteps = hostSession.TotalSteps;
        result.Phases = hostSession.Phases;
        result.CurrentState = hostSession.CurrentState;
        result.States = hostSession.States;
        result.Transitions = hostSession.Transitions;
        result.DispatchTargets = hostSession.DispatchTargets;
        result.ControlActions = hostSession.ControlActions;
        result.CoordinatorSteps = hostSession.CoordinatorSteps;
        result.OrchestrationStages = hostSession.OrchestrationStages;
        result.BootFlowStages = hostSession.BootFlowStages;
        result.BootSessionStages = hostSession.BootSessionStages;
        result.HandoffArtifacts = hostSession.HandoffArtifacts;
        result.ConsumedArtifacts = hostSession.ConsumedArtifacts;
        result.RuntimeSessionStages = hostSession.RuntimeSessionStages;
        result.ClientBootstrapActions = hostSession.ClientBootstrapActions;
        result.ReadinessChecks = hostSession.ReadinessChecks;
        result.ReadySignals = hostSession.ReadySignals;
        result.LaunchControlActions = hostSession.LaunchControlActions;
        result.ReadyStates = hostSession.ReadyStates;
        result.ClientLaunchStages = hostSession.ClientLaunchStages;
        result.ActivationSteps = hostSession.ActivationSteps;
        result.RunStateStages = hostSession.RunStateStages;
        result.ClientLoopStages = hostSession.ClientLoopStages;
        result.HostSessionStages = hostSession.HostSessionStages;
        result.HostLoopStages =
        [
            "initialize-host-loop",
            "bind-host-session",
            "publish-host-loop-ready"
        ];
        result.HostLoopSummary = $"Runtime host loop prepared {result.HostLoopStages.Length} host-loop stage(s) for profile '{hostSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime host loop ready for profile '{hostSession.ProfileId}' with {result.HostLoopStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeHostLoopResult
{
    public bool IsReady { get; set; }
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
    public string HostLoopSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
