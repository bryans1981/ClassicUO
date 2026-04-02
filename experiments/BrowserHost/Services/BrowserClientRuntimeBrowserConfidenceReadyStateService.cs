namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConfidenceReadyStateService : IBrowserClientRuntimeBrowserConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserConfidenceSession _runtimeBrowserConfidenceSession;

    public BrowserClientRuntimeBrowserConfidenceReadyStateService(IBrowserClientRuntimeBrowserConfidenceSession runtimeBrowserConfidenceSession)
    {
        _runtimeBrowserConfidenceSession = runtimeBrowserConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConfidenceSessionResult confidenceSession = await _runtimeBrowserConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserConfidenceReadyStateResult result = new()
        {
            ProfileId = confidenceSession.ProfileId,
            SessionId = confidenceSession.SessionId,
            SessionPath = confidenceSession.SessionPath,
            BrowserConfidenceSessionVersion = confidenceSession.BrowserConfidenceSessionVersion,
            BrowserRetentionReadyStateVersion = confidenceSession.BrowserRetentionReadyStateVersion,
            BrowserRetentionSessionVersion = confidenceSession.BrowserRetentionSessionVersion,
            LaunchMode = confidenceSession.LaunchMode,
            AssetRootPath = confidenceSession.AssetRootPath,
            ProfilesRootPath = confidenceSession.ProfilesRootPath,
            CacheRootPath = confidenceSession.CacheRootPath,
            ConfigRootPath = confidenceSession.ConfigRootPath,
            SettingsFilePath = confidenceSession.SettingsFilePath,
            StartupProfilePath = confidenceSession.StartupProfilePath,
            RequiredAssets = confidenceSession.RequiredAssets,
            ReadyAssetCount = confidenceSession.ReadyAssetCount,
            CompletedSteps = confidenceSession.CompletedSteps,
            TotalSteps = confidenceSession.TotalSteps,
            Exists = confidenceSession.Exists,
            ReadSucceeded = confidenceSession.ReadSucceeded
        };

        if (!confidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser confidence ready state blocked for profile '{confidenceSession.ProfileId}'.";
            result.Error = confidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConfidenceReadyStateVersion = "runtime-browser-confidence-ready-state-v1";
        result.BrowserConfidenceReadyChecks =
        [
            "browser-retention-ready-state-ready",
            "browser-confidence-session-ready",
            "browser-confidence-ready"
        ];
        result.BrowserConfidenceReadySummary = $"Runtime browser confidence ready state passed {result.BrowserConfidenceReadyChecks.Length} confidence readiness check(s) for profile '{confidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser confidence ready state ready for profile '{confidenceSession.ProfileId}' with {result.BrowserConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserRetentionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRetentionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
