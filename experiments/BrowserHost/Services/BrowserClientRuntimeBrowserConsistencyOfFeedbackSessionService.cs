namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConsistencyOfFeedbackSession
{
    ValueTask<BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionService : IBrowserClientRuntimeBrowserConsistencyOfFeedbackSession
{
    private readonly IBrowserClientRuntimeBrowserIntentionalityReadyState _runtimeBrowserIntentionalityReadyState;

    public BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionService(IBrowserClientRuntimeBrowserIntentionalityReadyState runtimeBrowserIntentionalityReadyState)
    {
        _runtimeBrowserIntentionalityReadyState = runtimeBrowserIntentionalityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserIntentionalityReadyStateResult intentionalityReadyState = await _runtimeBrowserIntentionalityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionResult result = new()
        {
            ProfileId = intentionalityReadyState.ProfileId,
            SessionId = intentionalityReadyState.SessionId,
            SessionPath = intentionalityReadyState.SessionPath,
            BrowserIntentionalityReadyStateVersion = intentionalityReadyState.BrowserIntentionalityReadyStateVersion,
            BrowserIntentionalitySessionVersion = intentionalityReadyState.BrowserIntentionalitySessionVersion,
            LaunchMode = intentionalityReadyState.LaunchMode,
            AssetRootPath = intentionalityReadyState.AssetRootPath,
            ProfilesRootPath = intentionalityReadyState.ProfilesRootPath,
            CacheRootPath = intentionalityReadyState.CacheRootPath,
            ConfigRootPath = intentionalityReadyState.ConfigRootPath,
            SettingsFilePath = intentionalityReadyState.SettingsFilePath,
            StartupProfilePath = intentionalityReadyState.StartupProfilePath,
            RequiredAssets = intentionalityReadyState.RequiredAssets,
            ReadyAssetCount = intentionalityReadyState.ReadyAssetCount,
            CompletedSteps = intentionalityReadyState.CompletedSteps,
            TotalSteps = intentionalityReadyState.TotalSteps,
            Exists = intentionalityReadyState.Exists,
            ReadSucceeded = intentionalityReadyState.ReadSucceeded
        };

        if (!intentionalityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser consistencyoffeedback session blocked for profile '{intentionalityReadyState.ProfileId}'.";
            result.Error = intentionalityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConsistencyOfFeedbackSessionVersion = "runtime-browser-consistencyoffeedback-session-v1";
        result.BrowserConsistencyOfFeedbackStages =
        [
            "open-browser-consistencyoffeedback-session",
            "bind-browser-intentionality-ready-state",
            "publish-browser-consistencyoffeedback-ready"
        ];
        result.BrowserConsistencyOfFeedbackSummary = $"Runtime browser consistencyoffeedback session prepared {result.BrowserConsistencyOfFeedbackStages.Length} consistencyoffeedback stage(s) for profile '{intentionalityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser consistencyoffeedback session ready for profile '{intentionalityReadyState.ProfileId}' with {result.BrowserConsistencyOfFeedbackStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConsistencyOfFeedbackSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserConsistencyOfFeedbackStages { get; set; } = Array.Empty<string>();
    public string BrowserConsistencyOfFeedbackSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
