namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupStateMachine
{
    ValueTask<BrowserClientRuntimeStartupStateMachineResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupStateMachineService : IBrowserClientRuntimeStartupStateMachine
{
    private readonly IBrowserClientRuntimeStartupState _runtimeStartupState;

    public BrowserClientRuntimeStartupStateMachineService(IBrowserClientRuntimeStartupState runtimeStartupState)
    {
        _runtimeStartupState = runtimeStartupState;
    }

    public async ValueTask<BrowserClientRuntimeStartupStateMachineResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupStateResult state = await _runtimeStartupState.BuildAsync(profileId);

        BrowserClientRuntimeStartupStateMachineResult result = new()
        {
            ProfileId = state.ProfileId,
            SessionId = state.SessionId,
            SessionPath = state.SessionPath,
            StateVersion = state.StateVersion,
            CycleVersion = state.CycleVersion,
            InvocationVersion = state.InvocationVersion,
            LoopVersion = state.LoopVersion,
            RunnerVersion = state.RunnerVersion,
            ExecutionVersion = state.ExecutionVersion,
            HandshakeVersion = state.HandshakeVersion,
            PacketVersion = state.PacketVersion,
            ContractVersion = state.ContractVersion,
            Exists = state.Exists,
            ReadSucceeded = state.ReadSucceeded
        };

        if (!state.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup state machine blocked for profile '{state.ProfileId}'.";
            result.Error = state.Error;
            return result;
        }

        result.IsReady = true;
        result.StateMachineVersion = "runtime-startup-state-machine-v1";
        result.LaunchMode = state.LaunchMode;
        result.AssetRootPath = state.AssetRootPath;
        result.ProfilesRootPath = state.ProfilesRootPath;
        result.CacheRootPath = state.CacheRootPath;
        result.ConfigRootPath = state.ConfigRootPath;
        result.SettingsFilePath = state.SettingsFilePath;
        result.StartupProfilePath = state.StartupProfilePath;
        result.RequiredAssets = state.RequiredAssets;
        result.ReadyAssetCount = state.ReadyAssetCount;
        result.CompletedSteps = state.CompletedSteps;
        result.TotalSteps = state.TotalSteps;
        result.Phases = state.Phases;
        result.CurrentState = state.State;
        result.States =
        [
            "bootstrap",
            "profile",
            "runtime-ready"
        ];
        result.StateMachineSummary = $"Runtime startup state machine prepared for profile '{state.ProfileId}' in state '{state.State}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup state machine ready for profile '{state.ProfileId}' with {result.States.Length} state(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupStateMachineResult
{
    public bool IsReady { get; set; }
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
    public string StateMachineSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
