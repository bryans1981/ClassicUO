namespace BrowserHost.Services;

public interface IBrowserClientStartupSessionExecutor
{
    ValueTask<BrowserClientStartupSessionExecutorResult> ExecuteAsync(string profileId = "default");
}

public sealed class BrowserClientStartupSessionExecutorService : IBrowserClientStartupSessionExecutor
{
    private readonly IBrowserClientStartupConsumer _startupConsumer;

    public BrowserClientStartupSessionExecutorService(IBrowserClientStartupConsumer startupConsumer)
    {
        _startupConsumer = startupConsumer;
    }

    public async ValueTask<BrowserClientStartupSessionExecutorResult> ExecuteAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientStartupConsumerResult consumer = await _startupConsumer.ConsumeAsync(profileId);

        BrowserClientStartupSessionExecutorResult result = new()
        {
            ProfileId = consumer.ProfileId,
            SessionId = consumer.SessionId,
            SessionPath = consumer.SessionPath,
            HandshakeVersion = consumer.HandshakeVersion,
            PacketVersion = consumer.PacketVersion,
            ContractVersion = consumer.ContractVersion,
            Exists = consumer.Exists,
            ReadSucceeded = consumer.ReadSucceeded
        };

        if (!consumer.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Startup session executor blocked for profile '{consumer.ProfileId}'.";
            result.Error = consumer.Error;
            return result;
        }

        result.IsReady = true;
        result.ExecutionVersion = "startup-executor-v1";
        result.LaunchMode = consumer.LaunchMode;
        result.AssetRootPath = consumer.AssetRootPath;
        result.ProfilesRootPath = consumer.ProfilesRootPath;
        result.CacheRootPath = consumer.CacheRootPath;
        result.ConfigRootPath = consumer.ConfigRootPath;
        result.SettingsFilePath = consumer.SettingsFilePath;
        result.StartupProfilePath = consumer.StartupProfilePath;
        result.RequiredAssets = consumer.RequiredAssets;
        result.ReadyAssetCount = consumer.ReadyAssetCount;
        result.CompletedSteps = consumer.CompletedSteps;
        result.TotalSteps = consumer.TotalSteps;
        result.ExecutionSummary = $"Startup session executor prepared runtime state for profile '{consumer.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Startup session executor ready for profile '{consumer.ProfileId}' with {consumer.RequiredAssets.Length} required asset(s).";

        return result;
    }
}

public sealed class BrowserClientStartupSessionExecutorResult
{
    public bool IsReady { get; set; }
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
    public string ExecutionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
