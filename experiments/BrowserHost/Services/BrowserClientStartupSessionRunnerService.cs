namespace BrowserHost.Services;

public interface IBrowserClientStartupSessionRunner
{
    ValueTask<BrowserClientStartupSessionRunnerResult> RunAsync(string profileId = "default");
}

public sealed class BrowserClientStartupSessionRunnerService : IBrowserClientStartupSessionRunner
{
    private readonly IBrowserClientStartupSessionExecutor _startupSessionExecutor;

    public BrowserClientStartupSessionRunnerService(IBrowserClientStartupSessionExecutor startupSessionExecutor)
    {
        _startupSessionExecutor = startupSessionExecutor;
    }

    public async ValueTask<BrowserClientStartupSessionRunnerResult> RunAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientStartupSessionExecutorResult executor = await _startupSessionExecutor.ExecuteAsync(profileId);

        BrowserClientStartupSessionRunnerResult result = new()
        {
            ProfileId = executor.ProfileId,
            SessionId = executor.SessionId,
            SessionPath = executor.SessionPath,
            ExecutionVersion = executor.ExecutionVersion,
            HandshakeVersion = executor.HandshakeVersion,
            PacketVersion = executor.PacketVersion,
            ContractVersion = executor.ContractVersion,
            Exists = executor.Exists,
            ReadSucceeded = executor.ReadSucceeded
        };

        if (!executor.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Startup session runner blocked for profile '{executor.ProfileId}'.";
            result.Error = executor.Error;
            return result;
        }

        result.IsReady = true;
        result.RunnerVersion = "startup-runner-v1";
        result.LaunchMode = executor.LaunchMode;
        result.AssetRootPath = executor.AssetRootPath;
        result.ProfilesRootPath = executor.ProfilesRootPath;
        result.CacheRootPath = executor.CacheRootPath;
        result.ConfigRootPath = executor.ConfigRootPath;
        result.SettingsFilePath = executor.SettingsFilePath;
        result.StartupProfilePath = executor.StartupProfilePath;
        result.RequiredAssets = executor.RequiredAssets;
        result.ReadyAssetCount = executor.ReadyAssetCount;
        result.CompletedSteps = executor.CompletedSteps;
        result.TotalSteps = executor.TotalSteps;
        result.RunSummary = $"Startup session runner simulated a runtime pass for profile '{executor.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Startup session runner ready for profile '{executor.ProfileId}' with {executor.RequiredAssets.Length} required asset(s).";

        return result;
    }
}

public sealed class BrowserClientStartupSessionRunnerResult
{
    public bool IsReady { get; set; }
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
    public string RunSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
