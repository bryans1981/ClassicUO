namespace BrowserHost.Services;

public interface IBrowserClientRuntimeInvocation
{
    ValueTask<BrowserClientRuntimeInvocationResult> InvokeAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeInvocationService : IBrowserClientRuntimeInvocation
{
    private readonly IBrowserClientRuntimeBootstrapLoop _runtimeBootstrapLoop;

    public BrowserClientRuntimeInvocationService(IBrowserClientRuntimeBootstrapLoop runtimeBootstrapLoop)
    {
        _runtimeBootstrapLoop = runtimeBootstrapLoop;
    }

    public async ValueTask<BrowserClientRuntimeInvocationResult> InvokeAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBootstrapLoopResult loop = await _runtimeBootstrapLoop.RunAsync(profileId);

        BrowserClientRuntimeInvocationResult result = new()
        {
            ProfileId = loop.ProfileId,
            SessionId = loop.SessionId,
            SessionPath = loop.SessionPath,
            LoopVersion = loop.LoopVersion,
            RunnerVersion = loop.RunnerVersion,
            ExecutionVersion = loop.ExecutionVersion,
            HandshakeVersion = loop.HandshakeVersion,
            PacketVersion = loop.PacketVersion,
            ContractVersion = loop.ContractVersion,
            Exists = loop.Exists,
            ReadSucceeded = loop.ReadSucceeded
        };

        if (!loop.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime invocation blocked for profile '{loop.ProfileId}'.";
            result.Error = loop.Error;
            return result;
        }

        result.IsReady = true;
        result.InvocationVersion = "runtime-invocation-v1";
        result.LaunchMode = loop.LaunchMode;
        result.AssetRootPath = loop.AssetRootPath;
        result.ProfilesRootPath = loop.ProfilesRootPath;
        result.CacheRootPath = loop.CacheRootPath;
        result.ConfigRootPath = loop.ConfigRootPath;
        result.SettingsFilePath = loop.SettingsFilePath;
        result.StartupProfilePath = loop.StartupProfilePath;
        result.RequiredAssets = loop.RequiredAssets;
        result.ReadyAssetCount = loop.ReadyAssetCount;
        result.CompletedSteps = loop.CompletedSteps;
        result.TotalSteps = loop.TotalSteps;
        result.Phases = loop.Phases;
        result.InvocationSummary = $"Runtime invocation prepared for profile '{loop.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime invocation ready for profile '{loop.ProfileId}' with {loop.Phases.Length} bootstrap phase(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeInvocationResult
{
    public bool IsReady { get; set; }
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
    public string InvocationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
