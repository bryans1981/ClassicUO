namespace BrowserHost.Services;

public interface IBrowserClientStartupConsumer
{
    ValueTask<BrowserClientStartupConsumerResult> ConsumeAsync(string profileId = "default");
}

public sealed class BrowserClientStartupConsumerService : IBrowserClientStartupConsumer
{
    private readonly IBrowserClientStartupPacket _startupPacket;

    public BrowserClientStartupConsumerService(IBrowserClientStartupPacket startupPacket)
    {
        _startupPacket = startupPacket;
    }

    public async ValueTask<BrowserClientStartupConsumerResult> ConsumeAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientStartupPacketResult packet = await _startupPacket.BuildAsync(profileId);

        BrowserClientStartupConsumerResult result = new()
        {
            ProfileId = packet.ProfileId,
            SessionId = packet.SessionId,
            SessionPath = packet.SessionPath,
            PacketVersion = packet.PacketVersion,
            ContractVersion = packet.ContractVersion,
            Exists = packet.Exists,
            ReadSucceeded = packet.ReadSucceeded
        };

        if (!packet.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Startup consumer blocked for profile '{packet.ProfileId}'.";
            result.Error = packet.Error;
            return result;
        }

        result.IsReady = true;
        result.HandshakeVersion = "browser-bootstrap-v1";
        result.LaunchMode = packet.LaunchMode;
        result.AssetRootPath = packet.AssetRootPath;
        result.ProfilesRootPath = packet.ProfilesRootPath;
        result.CacheRootPath = packet.CacheRootPath;
        result.ConfigRootPath = packet.ConfigRootPath;
        result.SettingsFilePath = packet.SettingsFilePath;
        result.StartupProfilePath = packet.StartupProfilePath;
        result.RequiredAssets = packet.RequiredAssets;
        result.ReadyAssetCount = packet.ReadyAssetCount;
        result.CompletedSteps = packet.CompletedSteps;
        result.TotalSteps = packet.TotalSteps;
        result.HandshakeSummary = $"Bootstrap handshake prepared for profile '{packet.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Startup consumer ready for profile '{packet.ProfileId}' with {packet.RequiredAssets.Length} required asset(s).";

        return result;
    }
}

public sealed class BrowserClientStartupConsumerResult
{
    public bool IsReady { get; set; }
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
    public string HandshakeSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
