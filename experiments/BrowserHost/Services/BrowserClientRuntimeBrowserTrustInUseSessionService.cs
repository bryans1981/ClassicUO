namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTrustInUseSession
{
    ValueTask<BrowserClientRuntimeBrowserTrustInUseSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTrustInUseSessionService : IBrowserClientRuntimeBrowserTrustInUseSession
{
    private readonly IBrowserClientRuntimeBrowserSuccessConfidenceReadyState _runtimeBrowserSuccessConfidenceReadyState;

    public BrowserClientRuntimeBrowserTrustInUseSessionService(IBrowserClientRuntimeBrowserSuccessConfidenceReadyState runtimeBrowserSuccessConfidenceReadyState)
    {
        _runtimeBrowserSuccessConfidenceReadyState = runtimeBrowserSuccessConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTrustInUseSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSuccessConfidenceReadyStateResult successconfidenceReadyState = await _runtimeBrowserSuccessConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTrustInUseSessionResult result = new()
        {
            ProfileId = successconfidenceReadyState.ProfileId,
            SessionId = successconfidenceReadyState.SessionId,
            SessionPath = successconfidenceReadyState.SessionPath,
            BrowserSuccessConfidenceReadyStateVersion = successconfidenceReadyState.BrowserSuccessConfidenceReadyStateVersion,
            BrowserSuccessConfidenceSessionVersion = successconfidenceReadyState.BrowserSuccessConfidenceSessionVersion,
            LaunchMode = successconfidenceReadyState.LaunchMode,
            AssetRootPath = successconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = successconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = successconfidenceReadyState.CacheRootPath,
            ConfigRootPath = successconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = successconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = successconfidenceReadyState.StartupProfilePath,
            RequiredAssets = successconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = successconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = successconfidenceReadyState.CompletedSteps,
            TotalSteps = successconfidenceReadyState.TotalSteps,
            Exists = successconfidenceReadyState.Exists,
            ReadSucceeded = successconfidenceReadyState.ReadSucceeded
        };

        if (!successconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser trustinuse session blocked for profile '{successconfidenceReadyState.ProfileId}'.";
            result.Error = successconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTrustInUseSessionVersion = "runtime-browser-trustinuse-session-v1";
        result.BrowserTrustInUseStages =
        [
            "open-browser-trustinuse-session",
            "bind-browser-successconfidence-ready-state",
            "publish-browser-trustinuse-ready"
        ];
        result.BrowserTrustInUseSummary = $"Runtime browser trustinuse session prepared {result.BrowserTrustInUseStages.Length} trustinuse stage(s) for profile '{successconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser trustinuse session ready for profile '{successconfidenceReadyState.ProfileId}' with {result.BrowserTrustInUseStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTrustInUseSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTrustInUseSessionVersion { get; set; } = string.Empty;
    public string BrowserSuccessConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSuccessConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTrustInUseStages { get; set; } = Array.Empty<string>();
    public string BrowserTrustInUseSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
