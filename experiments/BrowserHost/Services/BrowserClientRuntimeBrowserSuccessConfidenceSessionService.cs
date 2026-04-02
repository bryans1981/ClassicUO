namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSuccessConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserSuccessConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSuccessConfidenceSessionService : IBrowserClientRuntimeBrowserSuccessConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserOutcomeConfidenceReadyState _runtimeBrowserOutcomeConfidenceReadyState;

    public BrowserClientRuntimeBrowserSuccessConfidenceSessionService(IBrowserClientRuntimeBrowserOutcomeConfidenceReadyState runtimeBrowserOutcomeConfidenceReadyState)
    {
        _runtimeBrowserOutcomeConfidenceReadyState = runtimeBrowserOutcomeConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSuccessConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateResult outcomeconfidenceReadyState = await _runtimeBrowserOutcomeConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSuccessConfidenceSessionResult result = new()
        {
            ProfileId = outcomeconfidenceReadyState.ProfileId,
            SessionId = outcomeconfidenceReadyState.SessionId,
            SessionPath = outcomeconfidenceReadyState.SessionPath,
            BrowserOutcomeConfidenceReadyStateVersion = outcomeconfidenceReadyState.BrowserOutcomeConfidenceReadyStateVersion,
            BrowserOutcomeConfidenceSessionVersion = outcomeconfidenceReadyState.BrowserOutcomeConfidenceSessionVersion,
            LaunchMode = outcomeconfidenceReadyState.LaunchMode,
            AssetRootPath = outcomeconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = outcomeconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = outcomeconfidenceReadyState.CacheRootPath,
            ConfigRootPath = outcomeconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = outcomeconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = outcomeconfidenceReadyState.StartupProfilePath,
            RequiredAssets = outcomeconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = outcomeconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = outcomeconfidenceReadyState.CompletedSteps,
            TotalSteps = outcomeconfidenceReadyState.TotalSteps,
            Exists = outcomeconfidenceReadyState.Exists,
            ReadSucceeded = outcomeconfidenceReadyState.ReadSucceeded
        };

        if (!outcomeconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser successconfidence session blocked for profile '{outcomeconfidenceReadyState.ProfileId}'.";
            result.Error = outcomeconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSuccessConfidenceSessionVersion = "runtime-browser-successconfidence-session-v1";
        result.BrowserSuccessConfidenceStages =
        [
            "open-browser-successconfidence-session",
            "bind-browser-outcomeconfidence-ready-state",
            "publish-browser-successconfidence-ready"
        ];
        result.BrowserSuccessConfidenceSummary = $"Runtime browser successconfidence session prepared {result.BrowserSuccessConfidenceStages.Length} successconfidence stage(s) for profile '{outcomeconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser successconfidence session ready for profile '{outcomeconfidenceReadyState.ProfileId}' with {result.BrowserSuccessConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSuccessConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSuccessConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserOutcomeConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOutcomeConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSuccessConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserSuccessConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
