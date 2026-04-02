namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFeedbackReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFeedbackReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFeedbackReadyStateService : IBrowserClientRuntimeBrowserFeedbackReadyState
{
    private readonly IBrowserClientRuntimeBrowserFeedbackSession _runtimeBrowserFeedbackSession;

    public BrowserClientRuntimeBrowserFeedbackReadyStateService(IBrowserClientRuntimeBrowserFeedbackSession runtimeBrowserFeedbackSession)
    {
        _runtimeBrowserFeedbackSession = runtimeBrowserFeedbackSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFeedbackReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFeedbackSessionResult feedbackSession = await _runtimeBrowserFeedbackSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFeedbackReadyStateResult result = new()
        {
            ProfileId = feedbackSession.ProfileId,
            SessionId = feedbackSession.SessionId,
            SessionPath = feedbackSession.SessionPath,
            BrowserFeedbackSessionVersion = feedbackSession.BrowserFeedbackSessionVersion,
            BrowserResponsivenessReadyStateVersion = feedbackSession.BrowserResponsivenessReadyStateVersion,
            BrowserResponsivenessSessionVersion = feedbackSession.BrowserResponsivenessSessionVersion,
            LaunchMode = feedbackSession.LaunchMode,
            AssetRootPath = feedbackSession.AssetRootPath,
            ProfilesRootPath = feedbackSession.ProfilesRootPath,
            CacheRootPath = feedbackSession.CacheRootPath,
            ConfigRootPath = feedbackSession.ConfigRootPath,
            SettingsFilePath = feedbackSession.SettingsFilePath,
            StartupProfilePath = feedbackSession.StartupProfilePath,
            RequiredAssets = feedbackSession.RequiredAssets,
            ReadyAssetCount = feedbackSession.ReadyAssetCount,
            CompletedSteps = feedbackSession.CompletedSteps,
            TotalSteps = feedbackSession.TotalSteps,
            Exists = feedbackSession.Exists,
            ReadSucceeded = feedbackSession.ReadSucceeded
        };

        if (!feedbackSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser feedback ready state blocked for profile '{feedbackSession.ProfileId}'.";
            result.Error = feedbackSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFeedbackReadyStateVersion = "runtime-browser-feedback-ready-state-v1";
        result.BrowserFeedbackReadyChecks =
        [
            "browser-responsiveness-ready-state-ready",
            "browser-feedback-session-ready",
            "browser-feedback-ready"
        ];
        result.BrowserFeedbackReadySummary = $"Runtime browser feedback ready state passed {result.BrowserFeedbackReadyChecks.Length} feedback readiness check(s) for profile '{feedbackSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser feedback ready state ready for profile '{feedbackSession.ProfileId}' with {result.BrowserFeedbackReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFeedbackReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFeedbackReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserFeedbackReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFeedbackReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
