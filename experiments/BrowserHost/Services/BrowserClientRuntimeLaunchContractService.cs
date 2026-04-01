using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientRuntimeLaunchContract
{
    ValueTask<BrowserClientRuntimeLaunchContractResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeLaunchContractService : IBrowserClientRuntimeLaunchContract
{
    private readonly IBrowserClientLaunchSessionReader _launchSessionReader;

    public BrowserClientRuntimeLaunchContractService(IBrowserClientLaunchSessionReader launchSessionReader)
    {
        _launchSessionReader = launchSessionReader;
    }

    public async ValueTask<BrowserClientRuntimeLaunchContractResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientLaunchSessionRead sessionRead = await _launchSessionReader.ReadLaunchSessionAsync(profileId);

        BrowserClientRuntimeLaunchContractResult result = new()
        {
            ProfileId = sessionRead.ProfileId,
            SessionPath = sessionRead.SessionPath,
            Exists = sessionRead.Exists,
            ReadSucceeded = sessionRead.ReadSucceeded
        };

        if (!sessionRead.ReadSucceeded || !sessionRead.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime launch contract blocked for profile '{sessionRead.ProfileId}'.";
            result.Error = sessionRead.Error;
            return result;
        }

        BrowserClientLaunchSession session = sessionRead.Session;

        result.IsReady = true;
        result.SessionId = session.SessionId;
        result.ArtifactPath = session.ArtifactPath;
        result.AssetRootPath = session.AssetRootPath;
        result.ProfilesRootPath = session.ProfilesRootPath;
        result.CacheRootPath = session.CacheRootPath;
        result.ConfigRootPath = session.ConfigRootPath;
        result.SettingsFilePath = session.SettingsFilePath;
        result.StartupProfilePath = session.StartupProfilePath;
        result.RequiredAssets = session.RequiredAssets;
        result.ReadyAssetCount = session.ReadyAssetCount;
        result.CompletedSteps = session.CompletedSteps;
        result.TotalSteps = session.TotalSteps;
        result.LaunchMode = "browser-spike";
        result.ContractVersion = "v1";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime launch contract ready for profile '{session.ProfileId}' with {session.RequiredAssets.Length} required asset(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeLaunchContractResult
{
    public bool IsReady { get; set; }
    public string ContractVersion { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string SessionId { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;
    public string ArtifactPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
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
