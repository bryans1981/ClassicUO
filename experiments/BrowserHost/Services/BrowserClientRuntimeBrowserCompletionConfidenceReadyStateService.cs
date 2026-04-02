namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCompletionConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCompletionConfidenceReadyStateService : IBrowserClientRuntimeBrowserCompletionConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserCompletionConfidenceSession _runtimeBrowserCompletionConfidenceSession;

    public BrowserClientRuntimeBrowserCompletionConfidenceReadyStateService(IBrowserClientRuntimeBrowserCompletionConfidenceSession runtimeBrowserCompletionConfidenceSession)
    {
        _runtimeBrowserCompletionConfidenceSession = runtimeBrowserCompletionConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCompletionConfidenceSessionResult completionconfidenceSession = await _runtimeBrowserCompletionConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult result = new()
        {
            ProfileId = completionconfidenceSession.ProfileId,
            SessionId = completionconfidenceSession.SessionId,
            SessionPath = completionconfidenceSession.SessionPath,
            BrowserCompletionConfidenceSessionVersion = completionconfidenceSession.BrowserCompletionConfidenceSessionVersion,
            BrowserProgressAwarenessReadyStateVersion = completionconfidenceSession.BrowserProgressAwarenessReadyStateVersion,
            BrowserProgressAwarenessSessionVersion = completionconfidenceSession.BrowserProgressAwarenessSessionVersion,
            LaunchMode = completionconfidenceSession.LaunchMode,
            AssetRootPath = completionconfidenceSession.AssetRootPath,
            ProfilesRootPath = completionconfidenceSession.ProfilesRootPath,
            CacheRootPath = completionconfidenceSession.CacheRootPath,
            ConfigRootPath = completionconfidenceSession.ConfigRootPath,
            SettingsFilePath = completionconfidenceSession.SettingsFilePath,
            StartupProfilePath = completionconfidenceSession.StartupProfilePath,
            RequiredAssets = completionconfidenceSession.RequiredAssets,
            ReadyAssetCount = completionconfidenceSession.ReadyAssetCount,
            CompletedSteps = completionconfidenceSession.CompletedSteps,
            TotalSteps = completionconfidenceSession.TotalSteps,
            Exists = completionconfidenceSession.Exists,
            ReadSucceeded = completionconfidenceSession.ReadSucceeded
        };

        if (!completionconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser completionconfidence ready state blocked for profile '{completionconfidenceSession.ProfileId}'.";
            result.Error = completionconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCompletionConfidenceReadyStateVersion = "runtime-browser-completionconfidence-ready-state-v1";
        result.BrowserCompletionConfidenceReadyChecks =
        [
            "browser-progressawareness-ready-state-ready",
            "browser-completionconfidence-session-ready",
            "browser-completionconfidence-ready"
        ];
        result.BrowserCompletionConfidenceReadySummary = $"Runtime browser completionconfidence ready state passed {result.BrowserCompletionConfidenceReadyChecks.Length} completionconfidence readiness check(s) for profile '{completionconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser completionconfidence ready state ready for profile '{completionconfidenceSession.ProfileId}' with {result.BrowserCompletionConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCompletionConfidenceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserCompletionConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCompletionConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
