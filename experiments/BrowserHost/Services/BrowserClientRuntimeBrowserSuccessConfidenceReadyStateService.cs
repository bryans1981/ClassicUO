namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSuccessConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSuccessConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSuccessConfidenceReadyStateService : IBrowserClientRuntimeBrowserSuccessConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserSuccessConfidenceSession _runtimeBrowserSuccessConfidenceSession;

    public BrowserClientRuntimeBrowserSuccessConfidenceReadyStateService(IBrowserClientRuntimeBrowserSuccessConfidenceSession runtimeBrowserSuccessConfidenceSession)
    {
        _runtimeBrowserSuccessConfidenceSession = runtimeBrowserSuccessConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSuccessConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSuccessConfidenceSessionResult successconfidenceSession = await _runtimeBrowserSuccessConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSuccessConfidenceReadyStateResult result = new()
        {
            ProfileId = successconfidenceSession.ProfileId,
            SessionId = successconfidenceSession.SessionId,
            SessionPath = successconfidenceSession.SessionPath,
            BrowserSuccessConfidenceSessionVersion = successconfidenceSession.BrowserSuccessConfidenceSessionVersion,
            BrowserOutcomeConfidenceReadyStateVersion = successconfidenceSession.BrowserOutcomeConfidenceReadyStateVersion,
            BrowserOutcomeConfidenceSessionVersion = successconfidenceSession.BrowserOutcomeConfidenceSessionVersion,
            LaunchMode = successconfidenceSession.LaunchMode,
            AssetRootPath = successconfidenceSession.AssetRootPath,
            ProfilesRootPath = successconfidenceSession.ProfilesRootPath,
            CacheRootPath = successconfidenceSession.CacheRootPath,
            ConfigRootPath = successconfidenceSession.ConfigRootPath,
            SettingsFilePath = successconfidenceSession.SettingsFilePath,
            StartupProfilePath = successconfidenceSession.StartupProfilePath,
            RequiredAssets = successconfidenceSession.RequiredAssets,
            ReadyAssetCount = successconfidenceSession.ReadyAssetCount,
            CompletedSteps = successconfidenceSession.CompletedSteps,
            TotalSteps = successconfidenceSession.TotalSteps,
            Exists = successconfidenceSession.Exists,
            ReadSucceeded = successconfidenceSession.ReadSucceeded
        };

        if (!successconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser successconfidence ready state blocked for profile '{successconfidenceSession.ProfileId}'.";
            result.Error = successconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSuccessConfidenceReadyStateVersion = "runtime-browser-successconfidence-ready-state-v1";
        result.BrowserSuccessConfidenceReadyChecks =
        [
            "browser-outcomeconfidence-ready-state-ready",
            "browser-successconfidence-session-ready",
            "browser-successconfidence-ready"
        ];
        result.BrowserSuccessConfidenceReadySummary = $"Runtime browser successconfidence ready state passed {result.BrowserSuccessConfidenceReadyChecks.Length} successconfidence readiness check(s) for profile '{successconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser successconfidence ready state ready for profile '{successconfidenceSession.ProfileId}' with {result.BrowserSuccessConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSuccessConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSuccessConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSuccessConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserOutcomeConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOutcomeConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSuccessConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSuccessConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
