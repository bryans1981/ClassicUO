namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupCycle
{
    ValueTask<BrowserClientRuntimeStartupCycleResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupCycleService : IBrowserClientRuntimeStartupCycle
{
    private readonly IBrowserClientRuntimeInvocation _runtimeInvocation;

    public BrowserClientRuntimeStartupCycleService(IBrowserClientRuntimeInvocation runtimeInvocation)
    {
        _runtimeInvocation = runtimeInvocation;
    }

    public async ValueTask<BrowserClientRuntimeStartupCycleResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeInvocationResult invocation = await _runtimeInvocation.InvokeAsync(profileId);

        BrowserClientRuntimeStartupCycleResult result = new()
        {
            ProfileId = invocation.ProfileId,
            SessionId = invocation.SessionId,
            SessionPath = invocation.SessionPath,
            InvocationVersion = invocation.InvocationVersion,
            LoopVersion = invocation.LoopVersion,
            RunnerVersion = invocation.RunnerVersion,
            ExecutionVersion = invocation.ExecutionVersion,
            HandshakeVersion = invocation.HandshakeVersion,
            PacketVersion = invocation.PacketVersion,
            ContractVersion = invocation.ContractVersion,
            Exists = invocation.Exists,
            ReadSucceeded = invocation.ReadSucceeded
        };

        if (!invocation.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup cycle blocked for profile '{invocation.ProfileId}'.";
            result.Error = invocation.Error;
            return result;
        }

        result.IsReady = true;
        result.CycleVersion = "runtime-startup-cycle-v1";
        result.LaunchMode = invocation.LaunchMode;
        result.AssetRootPath = invocation.AssetRootPath;
        result.ProfilesRootPath = invocation.ProfilesRootPath;
        result.CacheRootPath = invocation.CacheRootPath;
        result.ConfigRootPath = invocation.ConfigRootPath;
        result.SettingsFilePath = invocation.SettingsFilePath;
        result.StartupProfilePath = invocation.StartupProfilePath;
        result.RequiredAssets = invocation.RequiredAssets;
        result.ReadyAssetCount = invocation.ReadyAssetCount;
        result.CompletedSteps = invocation.CompletedSteps;
        result.TotalSteps = invocation.TotalSteps;
        result.Phases = invocation.Phases;
        result.State = "ready";
        result.CycleSummary = $"Runtime startup cycle is ready for profile '{invocation.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup cycle ready for profile '{invocation.ProfileId}' with {invocation.Phases.Length} phase(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupCycleResult
{
    public bool IsReady { get; set; }
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
    public string CycleSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
