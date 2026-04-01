namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupState
{
    ValueTask<BrowserClientRuntimeStartupStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupStateService : IBrowserClientRuntimeStartupState
{
    private readonly IBrowserClientRuntimeStartupCycle _runtimeStartupCycle;

    public BrowserClientRuntimeStartupStateService(IBrowserClientRuntimeStartupCycle runtimeStartupCycle)
    {
        _runtimeStartupCycle = runtimeStartupCycle;
    }

    public async ValueTask<BrowserClientRuntimeStartupStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupCycleResult cycle = await _runtimeStartupCycle.BuildAsync(profileId);

        BrowserClientRuntimeStartupStateResult result = new()
        {
            ProfileId = cycle.ProfileId,
            SessionId = cycle.SessionId,
            SessionPath = cycle.SessionPath,
            CycleVersion = cycle.CycleVersion,
            InvocationVersion = cycle.InvocationVersion,
            LoopVersion = cycle.LoopVersion,
            RunnerVersion = cycle.RunnerVersion,
            ExecutionVersion = cycle.ExecutionVersion,
            HandshakeVersion = cycle.HandshakeVersion,
            PacketVersion = cycle.PacketVersion,
            ContractVersion = cycle.ContractVersion,
            Exists = cycle.Exists,
            ReadSucceeded = cycle.ReadSucceeded
        };

        if (!cycle.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup state blocked for profile '{cycle.ProfileId}'.";
            result.Error = cycle.Error;
            return result;
        }

        result.IsReady = true;
        result.StateVersion = "runtime-startup-state-v1";
        result.LaunchMode = cycle.LaunchMode;
        result.AssetRootPath = cycle.AssetRootPath;
        result.ProfilesRootPath = cycle.ProfilesRootPath;
        result.CacheRootPath = cycle.CacheRootPath;
        result.ConfigRootPath = cycle.ConfigRootPath;
        result.SettingsFilePath = cycle.SettingsFilePath;
        result.StartupProfilePath = cycle.StartupProfilePath;
        result.RequiredAssets = cycle.RequiredAssets;
        result.ReadyAssetCount = cycle.ReadyAssetCount;
        result.CompletedSteps = cycle.CompletedSteps;
        result.TotalSteps = cycle.TotalSteps;
        result.Phases = cycle.Phases;
        result.State = cycle.State;
        result.StateSummary = $"Runtime startup state prepared for profile '{cycle.ProfileId}' in state '{cycle.State}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup state ready for profile '{cycle.ProfileId}' with {cycle.Phases.Length} phase(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupStateResult
{
    public bool IsReady { get; set; }
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
    public string State { get; set; } = string.Empty;
    public string StateSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
