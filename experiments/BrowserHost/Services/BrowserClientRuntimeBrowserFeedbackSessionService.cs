namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFeedbackSession
{
    ValueTask<BrowserClientRuntimeBrowserFeedbackSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFeedbackSessionService : IBrowserClientRuntimeBrowserFeedbackSession
{
    private readonly IBrowserClientRuntimeBrowserResponsivenessReadyState _runtimeBrowserResponsivenessReadyState;

    public BrowserClientRuntimeBrowserFeedbackSessionService(IBrowserClientRuntimeBrowserResponsivenessReadyState runtimeBrowserResponsivenessReadyState)
    {
        _runtimeBrowserResponsivenessReadyState = runtimeBrowserResponsivenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFeedbackSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResponsivenessReadyStateResult responsivenessReadyState = await _runtimeBrowserResponsivenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFeedbackSessionResult result = new()
        {
            ProfileId = responsivenessReadyState.ProfileId,
            SessionId = responsivenessReadyState.SessionId,
            SessionPath = responsivenessReadyState.SessionPath,
            BrowserResponsivenessReadyStateVersion = responsivenessReadyState.BrowserResponsivenessReadyStateVersion,
            BrowserResponsivenessSessionVersion = responsivenessReadyState.BrowserResponsivenessSessionVersion,
            LaunchMode = responsivenessReadyState.LaunchMode,
            AssetRootPath = responsivenessReadyState.AssetRootPath,
            ProfilesRootPath = responsivenessReadyState.ProfilesRootPath,
            CacheRootPath = responsivenessReadyState.CacheRootPath,
            ConfigRootPath = responsivenessReadyState.ConfigRootPath,
            SettingsFilePath = responsivenessReadyState.SettingsFilePath,
            StartupProfilePath = responsivenessReadyState.StartupProfilePath,
            RequiredAssets = responsivenessReadyState.RequiredAssets,
            ReadyAssetCount = responsivenessReadyState.ReadyAssetCount,
            CompletedSteps = responsivenessReadyState.CompletedSteps,
            TotalSteps = responsivenessReadyState.TotalSteps,
            Exists = responsivenessReadyState.Exists,
            ReadSucceeded = responsivenessReadyState.ReadSucceeded
        };

        if (!responsivenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser feedback session blocked for profile '{responsivenessReadyState.ProfileId}'.";
            result.Error = responsivenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFeedbackSessionVersion = "runtime-browser-feedback-session-v1";
        result.BrowserFeedbackStages =
        [
            "open-browser-feedback-session",
            "bind-browser-responsiveness-ready-state",
            "publish-browser-feedback-ready"
        ];
        result.BrowserFeedbackSummary = $"Runtime browser feedback session prepared {result.BrowserFeedbackStages.Length} feedback stage(s) for profile '{responsivenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser feedback session ready for profile '{responsivenessReadyState.ProfileId}' with {result.BrowserFeedbackStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFeedbackSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFeedbackSessionVersion { get; set; } = string.Empty;
    public string BrowserResponsivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResponsivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFeedbackStages { get; set; } = Array.Empty<string>();
    public string BrowserFeedbackSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
