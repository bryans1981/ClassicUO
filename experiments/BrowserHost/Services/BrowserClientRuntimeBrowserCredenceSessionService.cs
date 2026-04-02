namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCredenceSession
{
    ValueTask<BrowserClientRuntimeBrowserCredenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCredenceSessionService : IBrowserClientRuntimeBrowserCredenceSession
{
    private readonly IBrowserClientRuntimeBrowserCompletionConfidenceReadyState _runtimeBrowserCompletionConfidenceReadyState;

    public BrowserClientRuntimeBrowserCredenceSessionService(IBrowserClientRuntimeBrowserCompletionConfidenceReadyState runtimeBrowserCompletionConfidenceReadyState)
    {
        _runtimeBrowserCompletionConfidenceReadyState = runtimeBrowserCompletionConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCredenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult completionconfidenceReadyState = await _runtimeBrowserCompletionConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCredenceSessionResult result = new()
        {
            ProfileId = completionconfidenceReadyState.ProfileId,
            SessionId = completionconfidenceReadyState.SessionId,
            SessionPath = completionconfidenceReadyState.SessionPath,
            BrowserCompletionConfidenceReadyStateVersion = completionconfidenceReadyState.BrowserCompletionConfidenceReadyStateVersion,
            BrowserCompletionConfidenceSessionVersion = completionconfidenceReadyState.BrowserCompletionConfidenceSessionVersion,
            LaunchMode = completionconfidenceReadyState.LaunchMode,
            AssetRootPath = completionconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = completionconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = completionconfidenceReadyState.CacheRootPath,
            ConfigRootPath = completionconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = completionconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = completionconfidenceReadyState.StartupProfilePath,
            RequiredAssets = completionconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = completionconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = completionconfidenceReadyState.CompletedSteps,
            TotalSteps = completionconfidenceReadyState.TotalSteps,
            Exists = completionconfidenceReadyState.Exists,
            ReadSucceeded = completionconfidenceReadyState.ReadSucceeded
        };

        if (!completionconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser credence session blocked for profile '{completionconfidenceReadyState.ProfileId}'.";
            result.Error = completionconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCredenceSessionVersion = "runtime-browser-credence-session-v1";
        result.BrowserCredenceStages =
        [
            "open-browser-credence-session",
            "bind-browser-completionconfidence-ready-state",
            "publish-browser-credence-ready"
        ];
        result.BrowserCredenceSummary = $"Runtime browser credence session prepared {result.BrowserCredenceStages.Length} credence stage(s) for profile '{completionconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser credence session ready for profile '{completionconfidenceReadyState.ProfileId}' with {result.BrowserCredenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCredenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCredenceSessionVersion { get; set; } = string.Empty;
    public string BrowserCompletionConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCompletionConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCredenceStages { get; set; } = Array.Empty<string>();
    public string BrowserCredenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
