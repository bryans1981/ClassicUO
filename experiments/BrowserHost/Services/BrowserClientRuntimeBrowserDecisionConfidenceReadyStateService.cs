namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDecisionConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDecisionConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDecisionConfidenceReadyStateService : IBrowserClientRuntimeBrowserDecisionConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserDecisionConfidenceSession _runtimeBrowserDecisionConfidenceSession;

    public BrowserClientRuntimeBrowserDecisionConfidenceReadyStateService(IBrowserClientRuntimeBrowserDecisionConfidenceSession runtimeBrowserDecisionConfidenceSession)
    {
        _runtimeBrowserDecisionConfidenceSession = runtimeBrowserDecisionConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDecisionConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDecisionConfidenceSessionResult decisionconfidenceSession = await _runtimeBrowserDecisionConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDecisionConfidenceReadyStateResult result = new()
        {
            ProfileId = decisionconfidenceSession.ProfileId,
            SessionId = decisionconfidenceSession.SessionId,
            SessionPath = decisionconfidenceSession.SessionPath,
            BrowserDecisionConfidenceSessionVersion = decisionconfidenceSession.BrowserDecisionConfidenceSessionVersion,
            BrowserTaskConfidenceReadyStateVersion = decisionconfidenceSession.BrowserTaskConfidenceReadyStateVersion,
            BrowserTaskConfidenceSessionVersion = decisionconfidenceSession.BrowserTaskConfidenceSessionVersion,
            LaunchMode = decisionconfidenceSession.LaunchMode,
            AssetRootPath = decisionconfidenceSession.AssetRootPath,
            ProfilesRootPath = decisionconfidenceSession.ProfilesRootPath,
            CacheRootPath = decisionconfidenceSession.CacheRootPath,
            ConfigRootPath = decisionconfidenceSession.ConfigRootPath,
            SettingsFilePath = decisionconfidenceSession.SettingsFilePath,
            StartupProfilePath = decisionconfidenceSession.StartupProfilePath,
            RequiredAssets = decisionconfidenceSession.RequiredAssets,
            ReadyAssetCount = decisionconfidenceSession.ReadyAssetCount,
            CompletedSteps = decisionconfidenceSession.CompletedSteps,
            TotalSteps = decisionconfidenceSession.TotalSteps,
            Exists = decisionconfidenceSession.Exists,
            ReadSucceeded = decisionconfidenceSession.ReadSucceeded
        };

        if (!decisionconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser decisionconfidence ready state blocked for profile '{decisionconfidenceSession.ProfileId}'.";
            result.Error = decisionconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDecisionConfidenceReadyStateVersion = "runtime-browser-decisionconfidence-ready-state-v1";
        result.BrowserDecisionConfidenceReadyChecks =
        [
            "browser-taskconfidence-ready-state-ready",
            "browser-decisionconfidence-session-ready",
            "browser-decisionconfidence-ready"
        ];
        result.BrowserDecisionConfidenceReadySummary = $"Runtime browser decisionconfidence ready state passed {result.BrowserDecisionConfidenceReadyChecks.Length} decisionconfidence readiness check(s) for profile '{decisionconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser decisionconfidence ready state ready for profile '{decisionconfidenceSession.ProfileId}' with {result.BrowserDecisionConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDecisionConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDecisionConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDecisionConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserTaskConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDecisionConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDecisionConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
