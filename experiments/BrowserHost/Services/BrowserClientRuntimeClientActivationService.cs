namespace BrowserHost.Services;

public interface IBrowserClientRuntimeClientActivation
{
    ValueTask<BrowserClientRuntimeClientActivationResult> ActivateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeClientActivationService : IBrowserClientRuntimeClientActivation
{
    private readonly IBrowserClientRuntimeClientLaunchSession _runtimeClientLaunchSession;

    public BrowserClientRuntimeClientActivationService(IBrowserClientRuntimeClientLaunchSession runtimeClientLaunchSession)
    {
        _runtimeClientLaunchSession = runtimeClientLaunchSession;
    }

    public async ValueTask<BrowserClientRuntimeClientActivationResult> ActivateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeClientLaunchSessionResult launchSession = await _runtimeClientLaunchSession.CreateAsync(profileId);

        BrowserClientRuntimeClientActivationResult result = new()
        {
            ProfileId = launchSession.ProfileId,
            SessionId = launchSession.SessionId,
            SessionPath = launchSession.SessionPath,
            ClientLaunchSessionVersion = launchSession.ClientLaunchSessionVersion,
            ClientReadyStateVersion = launchSession.ClientReadyStateVersion,
            LaunchControllerVersion = launchSession.LaunchControllerVersion,
            ReadySignalVersion = launchSession.ReadySignalVersion,
            ReadinessGateVersion = launchSession.ReadinessGateVersion,
            ClientBootstrapVersion = launchSession.ClientBootstrapVersion,
            RuntimeSessionVersion = launchSession.RuntimeSessionVersion,
            ConsumerVersion = launchSession.ConsumerVersion,
            HandoffVersion = launchSession.HandoffVersion,
            BootSessionVersion = launchSession.BootSessionVersion,
            BootFlowVersion = launchSession.BootFlowVersion,
            OrchestratorVersion = launchSession.OrchestratorVersion,
            CoordinatorVersion = launchSession.CoordinatorVersion,
            ControllerVersion = launchSession.ControllerVersion,
            DispatcherVersion = launchSession.DispatcherVersion,
            DriverVersion = launchSession.DriverVersion,
            StateMachineVersion = launchSession.StateMachineVersion,
            StateVersion = launchSession.StateVersion,
            CycleVersion = launchSession.CycleVersion,
            InvocationVersion = launchSession.InvocationVersion,
            LoopVersion = launchSession.LoopVersion,
            RunnerVersion = launchSession.RunnerVersion,
            ExecutionVersion = launchSession.ExecutionVersion,
            HandshakeVersion = launchSession.HandshakeVersion,
            PacketVersion = launchSession.PacketVersion,
            ContractVersion = launchSession.ContractVersion,
            Exists = launchSession.Exists,
            ReadSucceeded = launchSession.ReadSucceeded
        };

        if (!launchSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime client activation blocked for profile '{launchSession.ProfileId}'.";
            result.Error = launchSession.Error;
            return result;
        }

        result.IsReady = true;
        result.ClientActivationVersion = "runtime-client-activation-v1";
        result.LaunchMode = launchSession.LaunchMode;
        result.AssetRootPath = launchSession.AssetRootPath;
        result.ProfilesRootPath = launchSession.ProfilesRootPath;
        result.CacheRootPath = launchSession.CacheRootPath;
        result.ConfigRootPath = launchSession.ConfigRootPath;
        result.SettingsFilePath = launchSession.SettingsFilePath;
        result.StartupProfilePath = launchSession.StartupProfilePath;
        result.RequiredAssets = launchSession.RequiredAssets;
        result.ReadyAssetCount = launchSession.ReadyAssetCount;
        result.CompletedSteps = launchSession.CompletedSteps;
        result.TotalSteps = launchSession.TotalSteps;
        result.Phases = launchSession.Phases;
        result.CurrentState = launchSession.CurrentState;
        result.States = launchSession.States;
        result.Transitions = launchSession.Transitions;
        result.DispatchTargets = launchSession.DispatchTargets;
        result.ControlActions = launchSession.ControlActions;
        result.CoordinatorSteps = launchSession.CoordinatorSteps;
        result.OrchestrationStages = launchSession.OrchestrationStages;
        result.BootFlowStages = launchSession.BootFlowStages;
        result.BootSessionStages = launchSession.BootSessionStages;
        result.HandoffArtifacts = launchSession.HandoffArtifacts;
        result.ConsumedArtifacts = launchSession.ConsumedArtifacts;
        result.RuntimeSessionStages = launchSession.RuntimeSessionStages;
        result.ClientBootstrapActions = launchSession.ClientBootstrapActions;
        result.ReadinessChecks = launchSession.ReadinessChecks;
        result.ReadySignals = launchSession.ReadySignals;
        result.LaunchControlActions = launchSession.LaunchControlActions;
        result.ReadyStates = launchSession.ReadyStates;
        result.ClientLaunchStages = launchSession.ClientLaunchStages;
        result.ActivationSteps =
        [
            "bind-launch-session",
            "activate-client-runtime",
            "publish-client-active"
        ];
        result.ActivationSummary = $"Runtime client activation prepared {result.ActivationSteps.Length} activation step(s) for profile '{launchSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime client activation ready for profile '{launchSession.ProfileId}' with {result.ActivationSteps.Length} step(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeClientActivationResult
{
    public bool IsReady { get; set; }
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
    public string ActivationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
