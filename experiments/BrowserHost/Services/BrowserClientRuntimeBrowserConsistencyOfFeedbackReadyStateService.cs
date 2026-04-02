namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConsistencyOfFeedbackReadyState
{
    ValueTask<BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateService : IBrowserClientRuntimeBrowserConsistencyOfFeedbackReadyState
{
    private readonly IBrowserClientRuntimeBrowserConsistencyOfFeedbackSession _runtimeBrowserConsistencyOfFeedbackSession;

    public BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateService(IBrowserClientRuntimeBrowserConsistencyOfFeedbackSession runtimeBrowserConsistencyOfFeedbackSession)
    {
        _runtimeBrowserConsistencyOfFeedbackSession = runtimeBrowserConsistencyOfFeedbackSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionResult consistencyoffeedbackSession = await _runtimeBrowserConsistencyOfFeedbackSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateResult result = new()
        {
            ProfileId = consistencyoffeedbackSession.ProfileId,
            SessionId = consistencyoffeedbackSession.SessionId,
            SessionPath = consistencyoffeedbackSession.SessionPath,
            BrowserConsistencyOfFeedbackSessionVersion = consistencyoffeedbackSession.BrowserConsistencyOfFeedbackSessionVersion,
            BrowserIntentionalityReadyStateVersion = consistencyoffeedbackSession.BrowserIntentionalityReadyStateVersion,
            BrowserIntentionalitySessionVersion = consistencyoffeedbackSession.BrowserIntentionalitySessionVersion,
            LaunchMode = consistencyoffeedbackSession.LaunchMode,
            AssetRootPath = consistencyoffeedbackSession.AssetRootPath,
            ProfilesRootPath = consistencyoffeedbackSession.ProfilesRootPath,
            CacheRootPath = consistencyoffeedbackSession.CacheRootPath,
            ConfigRootPath = consistencyoffeedbackSession.ConfigRootPath,
            SettingsFilePath = consistencyoffeedbackSession.SettingsFilePath,
            StartupProfilePath = consistencyoffeedbackSession.StartupProfilePath,
            RequiredAssets = consistencyoffeedbackSession.RequiredAssets,
            ReadyAssetCount = consistencyoffeedbackSession.ReadyAssetCount,
            CompletedSteps = consistencyoffeedbackSession.CompletedSteps,
            TotalSteps = consistencyoffeedbackSession.TotalSteps,
            Exists = consistencyoffeedbackSession.Exists,
            ReadSucceeded = consistencyoffeedbackSession.ReadSucceeded
        };

        if (!consistencyoffeedbackSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser consistencyoffeedback ready state blocked for profile '{consistencyoffeedbackSession.ProfileId}'.";
            result.Error = consistencyoffeedbackSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConsistencyOfFeedbackReadyStateVersion = "runtime-browser-consistencyoffeedback-ready-state-v1";
        result.BrowserConsistencyOfFeedbackReadyChecks =
        [
            "browser-intentionality-ready-state-ready",
            "browser-consistencyoffeedback-session-ready",
            "browser-consistencyoffeedback-ready"
        ];
        result.BrowserConsistencyOfFeedbackReadySummary = $"Runtime browser consistencyoffeedback ready state passed {result.BrowserConsistencyOfFeedbackReadyChecks.Length} consistencyoffeedback readiness check(s) for profile '{consistencyoffeedbackSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser consistencyoffeedback ready state ready for profile '{consistencyoffeedbackSession.ProfileId}' with {result.BrowserConsistencyOfFeedbackReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserConsistencyOfFeedbackReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConsistencyOfFeedbackSessionVersion { get; set; } = string.Empty;
    public string BrowserIntentionalityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserIntentionalitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConsistencyOfFeedbackReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserConsistencyOfFeedbackReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
