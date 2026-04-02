namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserWatchdogSession
{
    ValueTask<BrowserClientRuntimeBrowserWatchdogSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserWatchdogSessionService : IBrowserClientRuntimeBrowserWatchdogSession
{
    private readonly IBrowserClientRuntimeBrowserMonitoringReadyState _runtimeBrowserMonitoringReadyState;

    public BrowserClientRuntimeBrowserWatchdogSessionService(IBrowserClientRuntimeBrowserMonitoringReadyState runtimeBrowserMonitoringReadyState)
    {
        _runtimeBrowserMonitoringReadyState = runtimeBrowserMonitoringReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserWatchdogSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMonitoringReadyStateResult monitoringReadyState = await _runtimeBrowserMonitoringReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserWatchdogSessionResult result = new()
        {
            ProfileId = monitoringReadyState.ProfileId,
            SessionId = monitoringReadyState.SessionId,
            SessionPath = monitoringReadyState.SessionPath,
            BrowserMonitoringReadyStateVersion = monitoringReadyState.BrowserMonitoringReadyStateVersion,
            BrowserMonitoringSessionVersion = monitoringReadyState.BrowserMonitoringSessionVersion,
            LaunchMode = monitoringReadyState.LaunchMode,
            AssetRootPath = monitoringReadyState.AssetRootPath,
            ProfilesRootPath = monitoringReadyState.ProfilesRootPath,
            CacheRootPath = monitoringReadyState.CacheRootPath,
            ConfigRootPath = monitoringReadyState.ConfigRootPath,
            SettingsFilePath = monitoringReadyState.SettingsFilePath,
            StartupProfilePath = monitoringReadyState.StartupProfilePath,
            RequiredAssets = monitoringReadyState.RequiredAssets,
            ReadyAssetCount = monitoringReadyState.ReadyAssetCount,
            CompletedSteps = monitoringReadyState.CompletedSteps,
            TotalSteps = monitoringReadyState.TotalSteps,
            Exists = monitoringReadyState.Exists,
            ReadSucceeded = monitoringReadyState.ReadSucceeded,
            BrowserMonitoringReadyState = monitoringReadyState
        };

        if (!monitoringReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser watchdog session blocked for profile '{monitoringReadyState.ProfileId}'.";
            result.Error = monitoringReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserWatchdogSessionVersion = "runtime-browser-watchdog-session-v1";
        result.BrowserWatchdogStages =
        [
            "open-browser-watchdog-session",
            "bind-browser-monitoring-ready-state",
            "publish-browser-watchdog-ready"
        ];
        result.BrowserWatchdogSummary = $"Runtime browser watchdog session prepared {result.BrowserWatchdogStages.Length} watchdog stage(s) for profile '{monitoringReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser watchdog session ready for profile '{monitoringReadyState.ProfileId}' with {result.BrowserWatchdogStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserWatchdogSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserWatchdogStages { get; set; } = Array.Empty<string>();
    public string BrowserWatchdogSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserMonitoringReadyStateResult BrowserMonitoringReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
