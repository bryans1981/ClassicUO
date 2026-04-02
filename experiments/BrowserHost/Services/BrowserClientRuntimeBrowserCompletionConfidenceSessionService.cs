namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCompletionConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserCompletionConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCompletionConfidenceSessionService : IBrowserClientRuntimeBrowserCompletionConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserProgressAwarenessReadyState _runtimeBrowserProgressAwarenessReadyState;

    public BrowserClientRuntimeBrowserCompletionConfidenceSessionService(IBrowserClientRuntimeBrowserProgressAwarenessReadyState runtimeBrowserProgressAwarenessReadyState)
    {
        _runtimeBrowserProgressAwarenessReadyState = runtimeBrowserProgressAwarenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCompletionConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProgressAwarenessReadyStateResult progressawarenessReadyState = await _runtimeBrowserProgressAwarenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCompletionConfidenceSessionResult result = new()
        {
            ProfileId = progressawarenessReadyState.ProfileId,
            SessionId = progressawarenessReadyState.SessionId,
            SessionPath = progressawarenessReadyState.SessionPath,
            BrowserProgressAwarenessReadyStateVersion = progressawarenessReadyState.BrowserProgressAwarenessReadyStateVersion,
            BrowserProgressAwarenessSessionVersion = progressawarenessReadyState.BrowserProgressAwarenessSessionVersion,
            LaunchMode = progressawarenessReadyState.LaunchMode,
            AssetRootPath = progressawarenessReadyState.AssetRootPath,
            ProfilesRootPath = progressawarenessReadyState.ProfilesRootPath,
            CacheRootPath = progressawarenessReadyState.CacheRootPath,
            ConfigRootPath = progressawarenessReadyState.ConfigRootPath,
            SettingsFilePath = progressawarenessReadyState.SettingsFilePath,
            StartupProfilePath = progressawarenessReadyState.StartupProfilePath,
            RequiredAssets = progressawarenessReadyState.RequiredAssets,
            ReadyAssetCount = progressawarenessReadyState.ReadyAssetCount,
            CompletedSteps = progressawarenessReadyState.CompletedSteps,
            TotalSteps = progressawarenessReadyState.TotalSteps,
            Exists = progressawarenessReadyState.Exists,
            ReadSucceeded = progressawarenessReadyState.ReadSucceeded
        };

        if (!progressawarenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser completionconfidence session blocked for profile '{progressawarenessReadyState.ProfileId}'.";
            result.Error = progressawarenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCompletionConfidenceSessionVersion = "runtime-browser-completionconfidence-session-v1";
        result.BrowserCompletionConfidenceStages =
        [
            "open-browser-completionconfidence-session",
            "bind-browser-progressawareness-ready-state",
            "publish-browser-completionconfidence-ready"
        ];
        result.BrowserCompletionConfidenceSummary = $"Runtime browser completionconfidence session prepared {result.BrowserCompletionConfidenceStages.Length} completionconfidence stage(s) for profile '{progressawarenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser completionconfidence session ready for profile '{progressawarenessReadyState.ProfileId}' with {result.BrowserCompletionConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCompletionConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCompletionConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserProgressAwarenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProgressAwarenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCompletionConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserCompletionConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
