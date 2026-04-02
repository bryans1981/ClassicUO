namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMonitoringSession
{
    ValueTask<BrowserClientRuntimeBrowserMonitoringSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMonitoringSessionService : IBrowserClientRuntimeBrowserMonitoringSession
{
    private readonly IBrowserClientRuntimeBrowserDiagnosticsReadyState _runtimeBrowserDiagnosticsReadyState;

    public BrowserClientRuntimeBrowserMonitoringSessionService(IBrowserClientRuntimeBrowserDiagnosticsReadyState runtimeBrowserDiagnosticsReadyState)
    {
        _runtimeBrowserDiagnosticsReadyState = runtimeBrowserDiagnosticsReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMonitoringSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDiagnosticsReadyStateResult diagnosticsReadyState = await _runtimeBrowserDiagnosticsReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserMonitoringSessionResult result = new()
        {
            ProfileId = diagnosticsReadyState.ProfileId,
            SessionId = diagnosticsReadyState.SessionId,
            SessionPath = diagnosticsReadyState.SessionPath,
            BrowserDiagnosticsReadyStateVersion = diagnosticsReadyState.BrowserDiagnosticsReadyStateVersion,
            BrowserDiagnosticsSessionVersion = diagnosticsReadyState.BrowserDiagnosticsSessionVersion,
            LaunchMode = diagnosticsReadyState.LaunchMode,
            AssetRootPath = diagnosticsReadyState.AssetRootPath,
            ProfilesRootPath = diagnosticsReadyState.ProfilesRootPath,
            CacheRootPath = diagnosticsReadyState.CacheRootPath,
            ConfigRootPath = diagnosticsReadyState.ConfigRootPath,
            SettingsFilePath = diagnosticsReadyState.SettingsFilePath,
            StartupProfilePath = diagnosticsReadyState.StartupProfilePath,
            RequiredAssets = diagnosticsReadyState.RequiredAssets,
            ReadyAssetCount = diagnosticsReadyState.ReadyAssetCount,
            CompletedSteps = diagnosticsReadyState.CompletedSteps,
            TotalSteps = diagnosticsReadyState.TotalSteps,
            Exists = diagnosticsReadyState.Exists,
            ReadSucceeded = diagnosticsReadyState.ReadSucceeded,
            BrowserDiagnosticsReadyState = diagnosticsReadyState
        };

        if (!diagnosticsReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser monitoring session blocked for profile '{diagnosticsReadyState.ProfileId}'.";
            result.Error = diagnosticsReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMonitoringSessionVersion = "runtime-browser-monitoring-session-v1";
        result.BrowserMonitoringStages =
        [
            "open-browser-monitoring-session",
            "bind-browser-diagnostics-ready-state",
            "publish-browser-monitoring-ready"
        ];
        result.BrowserMonitoringSummary = $"Runtime browser monitoring session prepared {result.BrowserMonitoringStages.Length} monitoring stage(s) for profile '{diagnosticsReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser monitoring session ready for profile '{diagnosticsReadyState.ProfileId}' with {result.BrowserMonitoringStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMonitoringSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserMonitoringStages { get; set; } = Array.Empty<string>();
    public string BrowserMonitoringSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserDiagnosticsReadyStateResult BrowserDiagnosticsReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
