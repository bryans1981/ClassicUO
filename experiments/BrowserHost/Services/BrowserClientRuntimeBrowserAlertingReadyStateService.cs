namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAlertingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAlertingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAlertingReadyStateService : IBrowserClientRuntimeBrowserAlertingReadyState
{
    private readonly IBrowserClientRuntimeBrowserAlertingSession _runtimeBrowserAlertingSession;

    public BrowserClientRuntimeBrowserAlertingReadyStateService(IBrowserClientRuntimeBrowserAlertingSession runtimeBrowserAlertingSession)
    {
        _runtimeBrowserAlertingSession = runtimeBrowserAlertingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAlertingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAlertingSessionResult alertingSession = await _runtimeBrowserAlertingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAlertingReadyStateResult result = new()
        {
            ProfileId = alertingSession.ProfileId,
            SessionId = alertingSession.SessionId,
            SessionPath = alertingSession.SessionPath,
            BrowserAlertingSessionVersion = alertingSession.BrowserAlertingSessionVersion,
            BrowserHealthReadyStateVersion = alertingSession.BrowserHealthReadyStateVersion,
            BrowserHealthSessionVersion = alertingSession.BrowserHealthSessionVersion,
            LaunchMode = alertingSession.LaunchMode,
            AssetRootPath = alertingSession.AssetRootPath,
            ProfilesRootPath = alertingSession.ProfilesRootPath,
            CacheRootPath = alertingSession.CacheRootPath,
            ConfigRootPath = alertingSession.ConfigRootPath,
            SettingsFilePath = alertingSession.SettingsFilePath,
            StartupProfilePath = alertingSession.StartupProfilePath,
            RequiredAssets = alertingSession.RequiredAssets,
            ReadyAssetCount = alertingSession.ReadyAssetCount,
            CompletedSteps = alertingSession.CompletedSteps,
            TotalSteps = alertingSession.TotalSteps,
            Exists = alertingSession.Exists,
            ReadSucceeded = alertingSession.ReadSucceeded,
            BrowserAlertingSession = alertingSession
        };

        if (!alertingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser alerting ready state blocked for profile '{alertingSession.ProfileId}'.";
            result.Error = alertingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAlertingReadyStateVersion = "runtime-browser-alerting-ready-state-v1";
        result.BrowserAlertingReadyChecks =
        [
            "browser-health-ready-state-ready",
            "browser-alerting-session-ready",
            "browser-alerting-ready"
        ];
        result.BrowserAlertingReadySummary = $"Runtime browser alerting ready state passed {result.BrowserAlertingReadyChecks.Length} alerting readiness check(s) for profile '{alertingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser alerting ready state ready for profile '{alertingSession.ProfileId}' with {result.BrowserAlertingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAlertingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAlertingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAlertingSessionVersion { get; set; } = string.Empty;
    public string BrowserHealthReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHealthSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAlertingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAlertingReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAlertingSessionResult BrowserAlertingSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
