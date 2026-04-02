namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRelianceSession
{
    ValueTask<BrowserClientRuntimeBrowserRelianceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRelianceSessionService : IBrowserClientRuntimeBrowserRelianceSession
{
    private readonly IBrowserClientRuntimeBrowserTrustInUseReadyState _runtimeBrowserTrustInUseReadyState;

    public BrowserClientRuntimeBrowserRelianceSessionService(IBrowserClientRuntimeBrowserTrustInUseReadyState runtimeBrowserTrustInUseReadyState)
    {
        _runtimeBrowserTrustInUseReadyState = runtimeBrowserTrustInUseReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRelianceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTrustInUseReadyStateResult trustinuseReadyState = await _runtimeBrowserTrustInUseReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRelianceSessionResult result = new()
        {
            ProfileId = trustinuseReadyState.ProfileId,
            SessionId = trustinuseReadyState.SessionId,
            SessionPath = trustinuseReadyState.SessionPath,
            BrowserTrustInUseReadyStateVersion = trustinuseReadyState.BrowserTrustInUseReadyStateVersion,
            BrowserTrustInUseSessionVersion = trustinuseReadyState.BrowserTrustInUseSessionVersion,
            LaunchMode = trustinuseReadyState.LaunchMode,
            AssetRootPath = trustinuseReadyState.AssetRootPath,
            ProfilesRootPath = trustinuseReadyState.ProfilesRootPath,
            CacheRootPath = trustinuseReadyState.CacheRootPath,
            ConfigRootPath = trustinuseReadyState.ConfigRootPath,
            SettingsFilePath = trustinuseReadyState.SettingsFilePath,
            StartupProfilePath = trustinuseReadyState.StartupProfilePath,
            RequiredAssets = trustinuseReadyState.RequiredAssets,
            ReadyAssetCount = trustinuseReadyState.ReadyAssetCount,
            CompletedSteps = trustinuseReadyState.CompletedSteps,
            TotalSteps = trustinuseReadyState.TotalSteps,
            Exists = trustinuseReadyState.Exists,
            ReadSucceeded = trustinuseReadyState.ReadSucceeded
        };

        if (!trustinuseReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser reliance session blocked for profile '{trustinuseReadyState.ProfileId}'.";
            result.Error = trustinuseReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRelianceSessionVersion = "runtime-browser-reliance-session-v1";
        result.BrowserRelianceStages =
        [
            "open-browser-reliance-session",
            "bind-browser-trustinuse-ready-state",
            "publish-browser-reliance-ready"
        ];
        result.BrowserRelianceSummary = $"Runtime browser reliance session prepared {result.BrowserRelianceStages.Length} reliance stage(s) for profile '{trustinuseReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser reliance session ready for profile '{trustinuseReadyState.ProfileId}' with {result.BrowserRelianceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRelianceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRelianceSessionVersion { get; set; } = string.Empty;
    public string BrowserTrustInUseReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustInUseSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRelianceStages { get; set; } = Array.Empty<string>();
    public string BrowserRelianceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
