namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPerceivabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserPerceivabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPerceivabilitySessionService : IBrowserClientRuntimeBrowserPerceivabilitySession
{
    private readonly IBrowserClientRuntimeBrowserConsistencyOfFeedbackReadyState _runtimeBrowserConsistencyOfFeedbackReadyState;

    public BrowserClientRuntimeBrowserPerceivabilitySessionService(IBrowserClientRuntimeBrowserConsistencyOfFeedbackReadyState runtimeBrowserConsistencyOfFeedbackReadyState)
    {
        _runtimeBrowserConsistencyOfFeedbackReadyState = runtimeBrowserConsistencyOfFeedbackReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPerceivabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConsistencyOfFeedbackReadyStateResult consistencyoffeedbackReadyState = await _runtimeBrowserConsistencyOfFeedbackReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPerceivabilitySessionResult result = new()
        {
            ProfileId = consistencyoffeedbackReadyState.ProfileId,
            SessionId = consistencyoffeedbackReadyState.SessionId,
            SessionPath = consistencyoffeedbackReadyState.SessionPath,
            BrowserConsistencyOfFeedbackReadyStateVersion = consistencyoffeedbackReadyState.BrowserConsistencyOfFeedbackReadyStateVersion,
            BrowserConsistencyOfFeedbackSessionVersion = consistencyoffeedbackReadyState.BrowserConsistencyOfFeedbackSessionVersion,
            LaunchMode = consistencyoffeedbackReadyState.LaunchMode,
            AssetRootPath = consistencyoffeedbackReadyState.AssetRootPath,
            ProfilesRootPath = consistencyoffeedbackReadyState.ProfilesRootPath,
            CacheRootPath = consistencyoffeedbackReadyState.CacheRootPath,
            ConfigRootPath = consistencyoffeedbackReadyState.ConfigRootPath,
            SettingsFilePath = consistencyoffeedbackReadyState.SettingsFilePath,
            StartupProfilePath = consistencyoffeedbackReadyState.StartupProfilePath,
            RequiredAssets = consistencyoffeedbackReadyState.RequiredAssets,
            ReadyAssetCount = consistencyoffeedbackReadyState.ReadyAssetCount,
            CompletedSteps = consistencyoffeedbackReadyState.CompletedSteps,
            TotalSteps = consistencyoffeedbackReadyState.TotalSteps,
            Exists = consistencyoffeedbackReadyState.Exists,
            ReadSucceeded = consistencyoffeedbackReadyState.ReadSucceeded
        };

        if (!consistencyoffeedbackReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser perceivability session blocked for profile '{consistencyoffeedbackReadyState.ProfileId}'.";
            result.Error = consistencyoffeedbackReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPerceivabilitySessionVersion = "runtime-browser-perceivability-session-v1";
        result.BrowserPerceivabilityStages =
        [
            "open-browser-perceivability-session",
            "bind-browser-consistencyoffeedback-ready-state",
            "publish-browser-perceivability-ready"
        ];
        result.BrowserPerceivabilitySummary = $"Runtime browser perceivability session prepared {result.BrowserPerceivabilityStages.Length} perceivability stage(s) for profile '{consistencyoffeedbackReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser perceivability session ready for profile '{consistencyoffeedbackReadyState.ProfileId}' with {result.BrowserPerceivabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPerceivabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPerceivabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserConsistencyOfFeedbackReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConsistencyOfFeedbackSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPerceivabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserPerceivabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
