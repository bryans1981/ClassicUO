namespace BrowserHost.Services;

public interface IBrowserClientRuntimeReadySignal
{
    ValueTask<BrowserClientRuntimeReadySignalResult> SignalAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeReadySignalService : IBrowserClientRuntimeReadySignal
{
    private readonly IBrowserClientRuntimeStartupReadinessGate _runtimeStartupReadinessGate;

    public BrowserClientRuntimeReadySignalService(IBrowserClientRuntimeStartupReadinessGate runtimeStartupReadinessGate)
    {
        _runtimeStartupReadinessGate = runtimeStartupReadinessGate;
    }

    public async ValueTask<BrowserClientRuntimeReadySignalResult> SignalAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupReadinessGateResult readinessGate = await _runtimeStartupReadinessGate.EvaluateAsync(profileId);

        BrowserClientRuntimeReadySignalResult result = new()
        {
            ProfileId = readinessGate.ProfileId,
            SessionId = readinessGate.SessionId,
            SessionPath = readinessGate.SessionPath,
            ReadinessGateVersion = readinessGate.ReadinessGateVersion,
            ClientBootstrapVersion = readinessGate.ClientBootstrapVersion,
            RuntimeSessionVersion = readinessGate.RuntimeSessionVersion,
            ConsumerVersion = readinessGate.ConsumerVersion,
            HandoffVersion = readinessGate.HandoffVersion,
            BootSessionVersion = readinessGate.BootSessionVersion,
            BootFlowVersion = readinessGate.BootFlowVersion,
            OrchestratorVersion = readinessGate.OrchestratorVersion,
            CoordinatorVersion = readinessGate.CoordinatorVersion,
            ControllerVersion = readinessGate.ControllerVersion,
            DispatcherVersion = readinessGate.DispatcherVersion,
            DriverVersion = readinessGate.DriverVersion,
            StateMachineVersion = readinessGate.StateMachineVersion,
            StateVersion = readinessGate.StateVersion,
            CycleVersion = readinessGate.CycleVersion,
            InvocationVersion = readinessGate.InvocationVersion,
            LoopVersion = readinessGate.LoopVersion,
            RunnerVersion = readinessGate.RunnerVersion,
            ExecutionVersion = readinessGate.ExecutionVersion,
            HandshakeVersion = readinessGate.HandshakeVersion,
            PacketVersion = readinessGate.PacketVersion,
            ContractVersion = readinessGate.ContractVersion,
            Exists = readinessGate.Exists,
            ReadSucceeded = readinessGate.ReadSucceeded
        };

        if (!readinessGate.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime ready signal blocked for profile '{readinessGate.ProfileId}'.";
            result.Error = readinessGate.Error;
            return result;
        }

        result.IsReady = true;
        result.ReadySignalVersion = "runtime-ready-signal-v1";
        result.LaunchMode = readinessGate.LaunchMode;
        result.AssetRootPath = readinessGate.AssetRootPath;
        result.ProfilesRootPath = readinessGate.ProfilesRootPath;
        result.CacheRootPath = readinessGate.CacheRootPath;
        result.ConfigRootPath = readinessGate.ConfigRootPath;
        result.SettingsFilePath = readinessGate.SettingsFilePath;
        result.StartupProfilePath = readinessGate.StartupProfilePath;
        result.RequiredAssets = readinessGate.RequiredAssets;
        result.ReadyAssetCount = readinessGate.ReadyAssetCount;
        result.CompletedSteps = readinessGate.CompletedSteps;
        result.TotalSteps = readinessGate.TotalSteps;
        result.Phases = readinessGate.Phases;
        result.CurrentState = readinessGate.CurrentState;
        result.States = readinessGate.States;
        result.Transitions = readinessGate.Transitions;
        result.DispatchTargets = readinessGate.DispatchTargets;
        result.ControlActions = readinessGate.ControlActions;
        result.CoordinatorSteps = readinessGate.CoordinatorSteps;
        result.OrchestrationStages = readinessGate.OrchestrationStages;
        result.BootFlowStages = readinessGate.BootFlowStages;
        result.BootSessionStages = readinessGate.BootSessionStages;
        result.HandoffArtifacts = readinessGate.HandoffArtifacts;
        result.ConsumedArtifacts = readinessGate.ConsumedArtifacts;
        result.RuntimeSessionStages = readinessGate.RuntimeSessionStages;
        result.ClientBootstrapActions = readinessGate.ClientBootstrapActions;
        result.ReadinessChecks = readinessGate.ReadinessChecks;
        result.ReadySignals =
        [
            "browser-runtime-bound",
            "startup-gate-passed",
            "client-runtime-ready"
        ];
        result.ReadySignalSummary = $"Runtime ready signal published {result.ReadySignals.Length} readiness signal(s) for profile '{readinessGate.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime ready signal ready for profile '{readinessGate.ProfileId}' with {result.ReadySignals.Length} signal(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeReadySignalResult
{
    public bool IsReady { get; set; }
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
    public string ReadySignalSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
