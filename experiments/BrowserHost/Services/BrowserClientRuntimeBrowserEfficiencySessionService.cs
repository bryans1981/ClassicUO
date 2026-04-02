namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEfficiencySession
{
    ValueTask<BrowserClientRuntimeBrowserEfficiencySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEfficiencySessionService : IBrowserClientRuntimeBrowserEfficiencySession
{
    private readonly IBrowserClientRuntimeBrowserFeedbackReadyState _runtimeBrowserFeedbackReadyState;

    public BrowserClientRuntimeBrowserEfficiencySessionService(IBrowserClientRuntimeBrowserFeedbackReadyState runtimeBrowserFeedbackReadyState)
    {
        _runtimeBrowserFeedbackReadyState = runtimeBrowserFeedbackReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEfficiencySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFeedbackReadyStateResult feedbackReadyState = await _runtimeBrowserFeedbackReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEfficiencySessionResult result = new()
        {
            ProfileId = feedbackReadyState.ProfileId,
            SessionId = feedbackReadyState.SessionId,
            SessionPath = feedbackReadyState.SessionPath,
            BrowserFeedbackReadyStateVersion = feedbackReadyState.BrowserFeedbackReadyStateVersion,
            BrowserFeedbackSessionVersion = feedbackReadyState.BrowserFeedbackSessionVersion,
            LaunchMode = feedbackReadyState.LaunchMode,
            AssetRootPath = feedbackReadyState.AssetRootPath,
            ProfilesRootPath = feedbackReadyState.ProfilesRootPath,
            CacheRootPath = feedbackReadyState.CacheRootPath,
            ConfigRootPath = feedbackReadyState.ConfigRootPath,
            SettingsFilePath = feedbackReadyState.SettingsFilePath,
            StartupProfilePath = feedbackReadyState.StartupProfilePath,
            RequiredAssets = feedbackReadyState.RequiredAssets,
            ReadyAssetCount = feedbackReadyState.ReadyAssetCount,
            CompletedSteps = feedbackReadyState.CompletedSteps,
            TotalSteps = feedbackReadyState.TotalSteps,
            Exists = feedbackReadyState.Exists,
            ReadSucceeded = feedbackReadyState.ReadSucceeded
        };

        if (!feedbackReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser efficiency session blocked for profile '{feedbackReadyState.ProfileId}'.";
            result.Error = feedbackReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEfficiencySessionVersion = "runtime-browser-efficiency-session-v1";
        result.BrowserEfficiencyStages =
        [
            "open-browser-efficiency-session",
            "bind-browser-feedback-ready-state",
            "publish-browser-efficiency-ready"
        ];
        result.BrowserEfficiencySummary = $"Runtime browser efficiency session prepared {result.BrowserEfficiencyStages.Length} efficiency stage(s) for profile '{feedbackReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser efficiency session ready for profile '{feedbackReadyState.ProfileId}' with {result.BrowserEfficiencyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEfficiencySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserEfficiencySessionVersion { get; set; } = string.Empty;
    public string BrowserFeedbackReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFeedbackSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEfficiencyStages { get; set; } = Array.Empty<string>();
    public string BrowserEfficiencySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
