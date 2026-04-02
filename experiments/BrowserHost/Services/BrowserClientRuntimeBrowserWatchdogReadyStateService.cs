namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserWatchdogReadyState
{
    ValueTask<BrowserClientRuntimeBrowserWatchdogReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserWatchdogReadyStateService : IBrowserClientRuntimeBrowserWatchdogReadyState
{
    private readonly IBrowserClientRuntimeBrowserWatchdogSession _runtimeBrowserWatchdogSession;

    public BrowserClientRuntimeBrowserWatchdogReadyStateService(IBrowserClientRuntimeBrowserWatchdogSession runtimeBrowserWatchdogSession)
    {
        _runtimeBrowserWatchdogSession = runtimeBrowserWatchdogSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserWatchdogReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserWatchdogSessionResult watchdogSession = await _runtimeBrowserWatchdogSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserWatchdogReadyStateResult result = new()
        {
            ProfileId = watchdogSession.ProfileId,
            SessionId = watchdogSession.SessionId,
            SessionPath = watchdogSession.SessionPath,
            BrowserWatchdogSessionVersion = watchdogSession.BrowserWatchdogSessionVersion,
            BrowserMonitoringReadyStateVersion = watchdogSession.BrowserMonitoringReadyStateVersion,
            BrowserMonitoringSessionVersion = watchdogSession.BrowserMonitoringSessionVersion,
            LaunchMode = watchdogSession.LaunchMode,
            AssetRootPath = watchdogSession.AssetRootPath,
            ProfilesRootPath = watchdogSession.ProfilesRootPath,
            CacheRootPath = watchdogSession.CacheRootPath,
            ConfigRootPath = watchdogSession.ConfigRootPath,
            SettingsFilePath = watchdogSession.SettingsFilePath,
            StartupProfilePath = watchdogSession.StartupProfilePath,
            RequiredAssets = watchdogSession.RequiredAssets,
            ReadyAssetCount = watchdogSession.ReadyAssetCount,
            CompletedSteps = watchdogSession.CompletedSteps,
            TotalSteps = watchdogSession.TotalSteps,
            Exists = watchdogSession.Exists,
            ReadSucceeded = watchdogSession.ReadSucceeded,
            BrowserWatchdogSession = watchdogSession
        };

        if (!watchdogSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser watchdog ready state blocked for profile '{watchdogSession.ProfileId}'.";
            result.Error = watchdogSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserWatchdogReadyStateVersion = "runtime-browser-watchdog-ready-state-v1";
        result.BrowserWatchdogReadyChecks =
        [
            "browser-monitoring-ready-state-ready",
            "browser-watchdog-session-ready",
            "browser-watchdog-ready"
        ];
        result.BrowserWatchdogReadySummary = $"Runtime browser watchdog ready state passed {result.BrowserWatchdogReadyChecks.Length} watchdog readiness check(s) for profile '{watchdogSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser watchdog ready state ready for profile '{watchdogSession.ProfileId}' with {result.BrowserWatchdogReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserWatchdogReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserWatchdogReadyStateVersion { get; set; } = string.Empty;
    public string BrowserWatchdogSessionVersion { get; set; } = string.Empty;
    public string BrowserMonitoringReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMonitoringSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserWatchdogReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserWatchdogReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserWatchdogSessionResult BrowserWatchdogSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
