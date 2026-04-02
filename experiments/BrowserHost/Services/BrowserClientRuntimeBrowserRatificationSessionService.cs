namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRatificationSession
{
    ValueTask<BrowserClientRuntimeBrowserRatificationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRatificationSessionService : IBrowserClientRuntimeBrowserRatificationSession
{
    private readonly IBrowserClientRuntimeBrowserAffirmationReadyState _runtimeBrowserAffirmationReadyState;

    public BrowserClientRuntimeBrowserRatificationSessionService(IBrowserClientRuntimeBrowserAffirmationReadyState runtimeBrowserAffirmationReadyState)
    {
        _runtimeBrowserAffirmationReadyState = runtimeBrowserAffirmationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRatificationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAffirmationReadyStateResult affirmationReadyState = await _runtimeBrowserAffirmationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRatificationSessionResult result = new()
        {
            ProfileId = affirmationReadyState.ProfileId,
            SessionId = affirmationReadyState.SessionId,
            SessionPath = affirmationReadyState.SessionPath,
            BrowserAffirmationReadyStateVersion = affirmationReadyState.BrowserAffirmationReadyStateVersion,
            BrowserAffirmationSessionVersion = affirmationReadyState.BrowserAffirmationSessionVersion,
            LaunchMode = affirmationReadyState.LaunchMode,
            AssetRootPath = affirmationReadyState.AssetRootPath,
            ProfilesRootPath = affirmationReadyState.ProfilesRootPath,
            CacheRootPath = affirmationReadyState.CacheRootPath,
            ConfigRootPath = affirmationReadyState.ConfigRootPath,
            SettingsFilePath = affirmationReadyState.SettingsFilePath,
            StartupProfilePath = affirmationReadyState.StartupProfilePath,
            RequiredAssets = affirmationReadyState.RequiredAssets,
            ReadyAssetCount = affirmationReadyState.ReadyAssetCount,
            CompletedSteps = affirmationReadyState.CompletedSteps,
            TotalSteps = affirmationReadyState.TotalSteps,
            Exists = affirmationReadyState.Exists,
            ReadSucceeded = affirmationReadyState.ReadSucceeded
        };

        if (!affirmationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser ratification session blocked for profile '{affirmationReadyState.ProfileId}'.";
            result.Error = affirmationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRatificationSessionVersion = "runtime-browser-ratification-session-v1";
        result.BrowserRatificationStages =
        [
            "open-browser-ratification-session",
            "bind-browser-affirmation-ready-state",
            "publish-browser-ratification-ready"
        ];
        result.BrowserRatificationSummary = $"Runtime browser ratification session prepared {result.BrowserRatificationStages.Length} ratification stage(s) for profile '{affirmationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser ratification session ready for profile '{affirmationReadyState.ProfileId}' with {result.BrowserRatificationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRatificationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRatificationSessionVersion { get; set; } = string.Empty;
    public string BrowserAffirmationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAffirmationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRatificationStages { get; set; } = Array.Empty<string>();
    public string BrowserRatificationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
