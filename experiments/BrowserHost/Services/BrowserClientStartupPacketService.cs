namespace BrowserHost.Services;

public interface IBrowserClientStartupPacket
{
    ValueTask<BrowserClientStartupPacketResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientStartupPacketService : IBrowserClientStartupPacket
{
    private readonly IBrowserClientRuntimeLaunchContract _runtimeLaunchContract;

    public BrowserClientStartupPacketService(IBrowserClientRuntimeLaunchContract runtimeLaunchContract)
    {
        _runtimeLaunchContract = runtimeLaunchContract;
    }

    public async ValueTask<BrowserClientStartupPacketResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeLaunchContractResult contract = await _runtimeLaunchContract.BuildAsync(profileId);

        BrowserClientStartupPacketResult result = new()
        {
            ProfileId = contract.ProfileId,
            SessionId = contract.SessionId,
            SessionPath = contract.SessionPath,
            Exists = contract.Exists,
            ReadSucceeded = contract.ReadSucceeded,
            ContractVersion = contract.ContractVersion
        };

        if (!contract.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Startup packet blocked for profile '{contract.ProfileId}'.";
            result.Error = contract.Error;
            return result;
        }

        result.IsReady = true;
        result.PacketVersion = "startup-packet-v1";
        result.LaunchMode = contract.LaunchMode;
        result.ArtifactPath = contract.ArtifactPath;
        result.AssetRootPath = contract.AssetRootPath;
        result.ProfilesRootPath = contract.ProfilesRootPath;
        result.CacheRootPath = contract.CacheRootPath;
        result.ConfigRootPath = contract.ConfigRootPath;
        result.SettingsFilePath = contract.SettingsFilePath;
        result.StartupProfilePath = contract.StartupProfilePath;
        result.RequiredAssets = contract.RequiredAssets;
        result.ReadyAssetCount = contract.ReadyAssetCount;
        result.CompletedSteps = contract.CompletedSteps;
        result.TotalSteps = contract.TotalSteps;
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Startup packet ready for profile '{contract.ProfileId}' with {contract.RequiredAssets.Length} asset path(s).";

        return result;
    }
}

public sealed class BrowserClientStartupPacketResult
{
    public bool IsReady { get; set; }
    public string PacketVersion { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string SessionId { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;
    public string ArtifactPath { get; set; } = string.Empty;
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
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
