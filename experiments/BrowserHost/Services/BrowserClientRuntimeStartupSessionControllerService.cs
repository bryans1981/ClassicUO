namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupSessionController
{
    ValueTask<BrowserClientRuntimeStartupSessionControllerResult> ControlAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupSessionControllerService : IBrowserClientRuntimeStartupSessionController
{
    private readonly IBrowserClientRuntimeStartupDispatcher _runtimeStartupDispatcher;

    public BrowserClientRuntimeStartupSessionControllerService(IBrowserClientRuntimeStartupDispatcher runtimeStartupDispatcher)
    {
        _runtimeStartupDispatcher = runtimeStartupDispatcher;
    }

    public async ValueTask<BrowserClientRuntimeStartupSessionControllerResult> ControlAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupDispatcherResult dispatcher = await _runtimeStartupDispatcher.DispatchAsync(profileId);

        BrowserClientRuntimeStartupSessionControllerResult result = new()
        {
            ProfileId = dispatcher.ProfileId,
            SessionId = dispatcher.SessionId,
            SessionPath = dispatcher.SessionPath,
            DispatcherVersion = dispatcher.DispatcherVersion,
            DriverVersion = dispatcher.DriverVersion,
            StateMachineVersion = dispatcher.StateMachineVersion,
            StateVersion = dispatcher.StateVersion,
            CycleVersion = dispatcher.CycleVersion,
            InvocationVersion = dispatcher.InvocationVersion,
            LoopVersion = dispatcher.LoopVersion,
            RunnerVersion = dispatcher.RunnerVersion,
            ExecutionVersion = dispatcher.ExecutionVersion,
            HandshakeVersion = dispatcher.HandshakeVersion,
            PacketVersion = dispatcher.PacketVersion,
            ContractVersion = dispatcher.ContractVersion,
            Exists = dispatcher.Exists,
            ReadSucceeded = dispatcher.ReadSucceeded
        };

        if (!dispatcher.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup session controller blocked for profile '{dispatcher.ProfileId}'.";
            result.Error = dispatcher.Error;
            return result;
        }

        result.IsReady = true;
        result.ControllerVersion = "runtime-startup-session-controller-v1";
        result.LaunchMode = dispatcher.LaunchMode;
        result.AssetRootPath = dispatcher.AssetRootPath;
        result.ProfilesRootPath = dispatcher.ProfilesRootPath;
        result.CacheRootPath = dispatcher.CacheRootPath;
        result.ConfigRootPath = dispatcher.ConfigRootPath;
        result.SettingsFilePath = dispatcher.SettingsFilePath;
        result.StartupProfilePath = dispatcher.StartupProfilePath;
        result.RequiredAssets = dispatcher.RequiredAssets;
        result.ReadyAssetCount = dispatcher.ReadyAssetCount;
        result.CompletedSteps = dispatcher.CompletedSteps;
        result.TotalSteps = dispatcher.TotalSteps;
        result.Phases = dispatcher.Phases;
        result.CurrentState = dispatcher.CurrentState;
        result.States = dispatcher.States;
        result.Transitions = dispatcher.Transitions;
        result.DispatchTargets = dispatcher.DispatchTargets;
        result.ControlActions =
        [
            "validate-session",
            "bind-dispatch-targets",
            "commit-runtime-ready"
        ];
        result.ControllerSummary = $"Runtime startup session controller prepared {result.ControlActions.Length} control action(s) for profile '{dispatcher.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup session controller ready for profile '{dispatcher.ProfileId}' with {result.ControlActions.Length} control action(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupSessionControllerResult
{
    public bool IsReady { get; set; }
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
    public string ControllerSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
