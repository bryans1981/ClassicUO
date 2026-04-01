namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupReadinessGate
{
    ValueTask<BrowserClientRuntimeStartupReadinessGateResult> EvaluateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupReadinessGateService : IBrowserClientRuntimeStartupReadinessGate
{
    private readonly IBrowserClientRuntimeClientBootstrapController _runtimeClientBootstrapController;

    public BrowserClientRuntimeStartupReadinessGateService(IBrowserClientRuntimeClientBootstrapController runtimeClientBootstrapController)
    {
        _runtimeClientBootstrapController = runtimeClientBootstrapController;
    }

    public async ValueTask<BrowserClientRuntimeStartupReadinessGateResult> EvaluateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeClientBootstrapControllerResult clientBootstrap = await _runtimeClientBootstrapController.ControlAsync(profileId);

        BrowserClientRuntimeStartupReadinessGateResult result = new()
        {
            ProfileId = clientBootstrap.ProfileId,
            SessionId = clientBootstrap.SessionId,
            SessionPath = clientBootstrap.SessionPath,
            ClientBootstrapVersion = clientBootstrap.ClientBootstrapVersion,
            RuntimeSessionVersion = clientBootstrap.RuntimeSessionVersion,
            ConsumerVersion = clientBootstrap.ConsumerVersion,
            HandoffVersion = clientBootstrap.HandoffVersion,
            BootSessionVersion = clientBootstrap.BootSessionVersion,
            BootFlowVersion = clientBootstrap.BootFlowVersion,
            OrchestratorVersion = clientBootstrap.OrchestratorVersion,
            CoordinatorVersion = clientBootstrap.CoordinatorVersion,
            ControllerVersion = clientBootstrap.ControllerVersion,
            DispatcherVersion = clientBootstrap.DispatcherVersion,
            DriverVersion = clientBootstrap.DriverVersion,
            StateMachineVersion = clientBootstrap.StateMachineVersion,
            StateVersion = clientBootstrap.StateVersion,
            CycleVersion = clientBootstrap.CycleVersion,
            InvocationVersion = clientBootstrap.InvocationVersion,
            LoopVersion = clientBootstrap.LoopVersion,
            RunnerVersion = clientBootstrap.RunnerVersion,
            ExecutionVersion = clientBootstrap.ExecutionVersion,
            HandshakeVersion = clientBootstrap.HandshakeVersion,
            PacketVersion = clientBootstrap.PacketVersion,
            ContractVersion = clientBootstrap.ContractVersion,
            Exists = clientBootstrap.Exists,
            ReadSucceeded = clientBootstrap.ReadSucceeded
        };

        if (!clientBootstrap.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup readiness gate blocked for profile '{clientBootstrap.ProfileId}'.";
            result.Error = clientBootstrap.Error;
            return result;
        }

        result.IsReady = true;
        result.ReadinessGateVersion = "runtime-startup-readiness-gate-v1";
        result.LaunchMode = clientBootstrap.LaunchMode;
        result.AssetRootPath = clientBootstrap.AssetRootPath;
        result.ProfilesRootPath = clientBootstrap.ProfilesRootPath;
        result.CacheRootPath = clientBootstrap.CacheRootPath;
        result.ConfigRootPath = clientBootstrap.ConfigRootPath;
        result.SettingsFilePath = clientBootstrap.SettingsFilePath;
        result.StartupProfilePath = clientBootstrap.StartupProfilePath;
        result.RequiredAssets = clientBootstrap.RequiredAssets;
        result.ReadyAssetCount = clientBootstrap.ReadyAssetCount;
        result.CompletedSteps = clientBootstrap.CompletedSteps;
        result.TotalSteps = clientBootstrap.TotalSteps;
        result.Phases = clientBootstrap.Phases;
        result.CurrentState = clientBootstrap.CurrentState;
        result.States = clientBootstrap.States;
        result.Transitions = clientBootstrap.Transitions;
        result.DispatchTargets = clientBootstrap.DispatchTargets;
        result.ControlActions = clientBootstrap.ControlActions;
        result.CoordinatorSteps = clientBootstrap.CoordinatorSteps;
        result.OrchestrationStages = clientBootstrap.OrchestrationStages;
        result.BootFlowStages = clientBootstrap.BootFlowStages;
        result.BootSessionStages = clientBootstrap.BootSessionStages;
        result.HandoffArtifacts = clientBootstrap.HandoffArtifacts;
        result.ConsumedArtifacts = clientBootstrap.ConsumedArtifacts;
        result.RuntimeSessionStages = clientBootstrap.RuntimeSessionStages;
        result.ClientBootstrapActions = clientBootstrap.ClientBootstrapActions;
        result.ReadinessChecks =
        [
            "assets-ready",
            "handoff-consumed",
            "runtime-session-ready",
            "client-bootstrap-ready"
        ];
        result.ReadinessSummary = $"Runtime startup readiness gate passed {result.ReadinessChecks.Length} readiness check(s) for profile '{clientBootstrap.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup readiness gate ready for profile '{clientBootstrap.ProfileId}' with {result.ReadinessChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupReadinessGateResult
{
    public bool IsReady { get; set; }
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
    public string ReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
