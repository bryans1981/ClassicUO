namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupTransitionDriver
{
    ValueTask<BrowserClientRuntimeStartupTransitionDriverResult> DriveAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupTransitionDriverService : IBrowserClientRuntimeStartupTransitionDriver
{
    private readonly IBrowserClientRuntimeStartupStateMachine _runtimeStartupStateMachine;

    public BrowserClientRuntimeStartupTransitionDriverService(IBrowserClientRuntimeStartupStateMachine runtimeStartupStateMachine)
    {
        _runtimeStartupStateMachine = runtimeStartupStateMachine;
    }

    public async ValueTask<BrowserClientRuntimeStartupTransitionDriverResult> DriveAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupStateMachineResult stateMachine = await _runtimeStartupStateMachine.BuildAsync(profileId);

        BrowserClientRuntimeStartupTransitionDriverResult result = new()
        {
            ProfileId = stateMachine.ProfileId,
            SessionId = stateMachine.SessionId,
            SessionPath = stateMachine.SessionPath,
            StateMachineVersion = stateMachine.StateMachineVersion,
            StateVersion = stateMachine.StateVersion,
            CycleVersion = stateMachine.CycleVersion,
            InvocationVersion = stateMachine.InvocationVersion,
            LoopVersion = stateMachine.LoopVersion,
            RunnerVersion = stateMachine.RunnerVersion,
            ExecutionVersion = stateMachine.ExecutionVersion,
            HandshakeVersion = stateMachine.HandshakeVersion,
            PacketVersion = stateMachine.PacketVersion,
            ContractVersion = stateMachine.ContractVersion,
            Exists = stateMachine.Exists,
            ReadSucceeded = stateMachine.ReadSucceeded
        };

        if (!stateMachine.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup transition driver blocked for profile '{stateMachine.ProfileId}'.";
            result.Error = stateMachine.Error;
            return result;
        }

        result.IsReady = true;
        result.DriverVersion = "runtime-startup-transition-driver-v1";
        result.LaunchMode = stateMachine.LaunchMode;
        result.AssetRootPath = stateMachine.AssetRootPath;
        result.ProfilesRootPath = stateMachine.ProfilesRootPath;
        result.CacheRootPath = stateMachine.CacheRootPath;
        result.ConfigRootPath = stateMachine.ConfigRootPath;
        result.SettingsFilePath = stateMachine.SettingsFilePath;
        result.StartupProfilePath = stateMachine.StartupProfilePath;
        result.RequiredAssets = stateMachine.RequiredAssets;
        result.ReadyAssetCount = stateMachine.ReadyAssetCount;
        result.CompletedSteps = stateMachine.CompletedSteps;
        result.TotalSteps = stateMachine.TotalSteps;
        result.Phases = stateMachine.Phases;
        result.CurrentState = stateMachine.CurrentState;
        result.States = stateMachine.States;
        result.Transitions =
        [
            "bootstrap->profile",
            "profile->runtime-ready"
        ];
        result.DriverSummary = $"Runtime startup transition driver advanced profile '{stateMachine.ProfileId}' through {result.Transitions.Length} transition(s).";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup transition driver ready for profile '{stateMachine.ProfileId}' with {result.Transitions.Length} transition(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupTransitionDriverResult
{
    public bool IsReady { get; set; }
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
    public string DriverSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
