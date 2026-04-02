namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOutcomeConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateService : IBrowserClientRuntimeBrowserOutcomeConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserOutcomeConfidenceSession _runtimeBrowserOutcomeConfidenceSession;

    public BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateService(IBrowserClientRuntimeBrowserOutcomeConfidenceSession runtimeBrowserOutcomeConfidenceSession)
    {
        _runtimeBrowserOutcomeConfidenceSession = runtimeBrowserOutcomeConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOutcomeConfidenceSessionResult outcomeconfidenceSession = await _runtimeBrowserOutcomeConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateResult result = new()
        {
            ProfileId = outcomeconfidenceSession.ProfileId,
            SessionId = outcomeconfidenceSession.SessionId,
            SessionPath = outcomeconfidenceSession.SessionPath,
            BrowserOutcomeConfidenceSessionVersion = outcomeconfidenceSession.BrowserOutcomeConfidenceSessionVersion,
            BrowserDecisionConfidenceReadyStateVersion = outcomeconfidenceSession.BrowserDecisionConfidenceReadyStateVersion,
            BrowserDecisionConfidenceSessionVersion = outcomeconfidenceSession.BrowserDecisionConfidenceSessionVersion,
            LaunchMode = outcomeconfidenceSession.LaunchMode,
            AssetRootPath = outcomeconfidenceSession.AssetRootPath,
            ProfilesRootPath = outcomeconfidenceSession.ProfilesRootPath,
            CacheRootPath = outcomeconfidenceSession.CacheRootPath,
            ConfigRootPath = outcomeconfidenceSession.ConfigRootPath,
            SettingsFilePath = outcomeconfidenceSession.SettingsFilePath,
            StartupProfilePath = outcomeconfidenceSession.StartupProfilePath,
            RequiredAssets = outcomeconfidenceSession.RequiredAssets,
            ReadyAssetCount = outcomeconfidenceSession.ReadyAssetCount,
            CompletedSteps = outcomeconfidenceSession.CompletedSteps,
            TotalSteps = outcomeconfidenceSession.TotalSteps,
            Exists = outcomeconfidenceSession.Exists,
            ReadSucceeded = outcomeconfidenceSession.ReadSucceeded
        };

        if (!outcomeconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser outcomeconfidence ready state blocked for profile '{outcomeconfidenceSession.ProfileId}'.";
            result.Error = outcomeconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOutcomeConfidenceReadyStateVersion = "runtime-browser-outcomeconfidence-ready-state-v1";
        result.BrowserOutcomeConfidenceReadyChecks =
        [
            "browser-decisionconfidence-ready-state-ready",
            "browser-outcomeconfidence-session-ready",
            "browser-outcomeconfidence-ready"
        ];
        result.BrowserOutcomeConfidenceReadySummary = $"Runtime browser outcomeconfidence ready state passed {result.BrowserOutcomeConfidenceReadyChecks.Length} outcomeconfidence readiness check(s) for profile '{outcomeconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser outcomeconfidence ready state ready for profile '{outcomeconfidenceSession.ProfileId}' with {result.BrowserOutcomeConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOutcomeConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOutcomeConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOutcomeConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserDecisionConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDecisionConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOutcomeConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOutcomeConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
