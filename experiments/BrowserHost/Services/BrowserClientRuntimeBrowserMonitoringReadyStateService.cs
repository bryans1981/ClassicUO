namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMonitoringReadyState
{
    ValueTask<BrowserClientRuntimeBrowserMonitoringReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMonitoringReadyStateService : IBrowserClientRuntimeBrowserMonitoringReadyState
{
    private readonly IBrowserClientRuntimeBrowserMonitoringSession _runtimeBrowserMonitoringSession;

    public BrowserClientRuntimeBrowserMonitoringReadyStateService(IBrowserClientRuntimeBrowserMonitoringSession runtimeBrowserMonitoringSession)
    {
        _runtimeBrowserMonitoringSession = runtimeBrowserMonitoringSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMonitoringReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMonitoringSessionResult monitoringSession = await _runtimeBrowserMonitoringSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserMonitoringReadyStateResult result = new()
        {
            ProfileId = monitoringSession.ProfileId,
            SessionId = monitoringSession.SessionId,
            SessionPath = monitoringSession.SessionPath,
            BrowserMonitoringSessionVersion = monitoringSession.BrowserMonitoringSessionVersion,
            BrowserDiagnosticsReadyStateVersion = monitoringSession.BrowserDiagnosticsReadyStateVersion,
            BrowserDiagnosticsSessionVersion = monitoringSession.BrowserDiagnosticsSessionVersion,
            LaunchMode = monitoringSession.LaunchMode,
            AssetRootPath = monitoringSession.AssetRootPath,
            ProfilesRootPath = monitoringSession.ProfilesRootPath,
            CacheRootPath = monitoringSession.CacheRootPath,
            ConfigRootPath = monitoringSession.ConfigRootPath,
            SettingsFilePath = monitoringSession.SettingsFilePath,
            StartupProfilePath = monitoringSession.StartupProfilePath,
            RequiredAssets = monitoringSession.RequiredAssets,
            ReadyAssetCount = monitoringSession.ReadyAssetCount,
            CompletedSteps = monitoringSession.CompletedSteps,
            TotalSteps = monitoringSession.TotalSteps,
            Exists = monitoringSession.Exists,
            ReadSucceeded = monitoringSession.ReadSucceeded,
            BrowserMonitoringSession = monitoringSession
        };

        if (!monitoringSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser monitoring ready state blocked for profile '{monitoringSession.ProfileId}'.";
            result.Error = monitoringSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMonitoringReadyStateVersion = "runtime-browser-monitoring-ready-state-v1";
        result.BrowserMonitoringReadyChecks =
        [
            "browser-diagnostics-ready-state-ready",
            "browser-monitoring-session-ready",
            "browser-monitoring-ready"
        ];
        result.BrowserMonitoringReadySummary = $"Runtime browser monitoring ready state passed {result.BrowserMonitoringReadyChecks.Length} monitoring readiness check(s) for profile '{monitoringSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser monitoring ready state ready for profile '{monitoringSession.ProfileId}' with {result.BrowserMonitoringReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMonitoringReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserMonitoringReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMonitoringSessionVersion { get; set; } = string.Empty;
    public string BrowserDiagnosticsReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDiagnosticsSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMonitoringReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserMonitoringReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserMonitoringSessionResult BrowserMonitoringSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
