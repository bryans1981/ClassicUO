namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDiagnosticsReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDiagnosticsReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDiagnosticsReadyStateService : IBrowserClientRuntimeBrowserDiagnosticsReadyState
{
    private readonly IBrowserClientRuntimeBrowserDiagnosticsSession _runtimeBrowserDiagnosticsSession;

    public BrowserClientRuntimeBrowserDiagnosticsReadyStateService(IBrowserClientRuntimeBrowserDiagnosticsSession runtimeBrowserDiagnosticsSession)
    {
        _runtimeBrowserDiagnosticsSession = runtimeBrowserDiagnosticsSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDiagnosticsReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDiagnosticsSessionResult diagnosticsSession = await _runtimeBrowserDiagnosticsSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDiagnosticsReadyStateResult result = new()
        {
            ProfileId = diagnosticsSession.ProfileId,
            SessionId = diagnosticsSession.SessionId,
            SessionPath = diagnosticsSession.SessionPath,
            BrowserDiagnosticsSessionVersion = diagnosticsSession.BrowserDiagnosticsSessionVersion,
            BrowserTelemetryReadyStateVersion = diagnosticsSession.BrowserTelemetryReadyStateVersion,
            BrowserTelemetrySessionVersion = diagnosticsSession.BrowserTelemetrySessionVersion,
            LaunchMode = diagnosticsSession.LaunchMode,
            AssetRootPath = diagnosticsSession.AssetRootPath,
            ProfilesRootPath = diagnosticsSession.ProfilesRootPath,
            CacheRootPath = diagnosticsSession.CacheRootPath,
            ConfigRootPath = diagnosticsSession.ConfigRootPath,
            SettingsFilePath = diagnosticsSession.SettingsFilePath,
            StartupProfilePath = diagnosticsSession.StartupProfilePath,
            RequiredAssets = diagnosticsSession.RequiredAssets,
            ReadyAssetCount = diagnosticsSession.ReadyAssetCount,
            CompletedSteps = diagnosticsSession.CompletedSteps,
            TotalSteps = diagnosticsSession.TotalSteps,
            Exists = diagnosticsSession.Exists,
            ReadSucceeded = diagnosticsSession.ReadSucceeded,
            BrowserDiagnosticsSession = diagnosticsSession
        };

        if (!diagnosticsSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser diagnostics ready state blocked for profile '{diagnosticsSession.ProfileId}'.";
            result.Error = diagnosticsSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDiagnosticsReadyStateVersion = "runtime-browser-diagnostics-ready-state-v1";
        result.BrowserDiagnosticsReadyChecks =
        [
            "browser-telemetry-ready-state-ready",
            "browser-diagnostics-session-ready",
            "browser-diagnostics-ready"
        ];
        result.BrowserDiagnosticsReadySummary = $"Runtime browser diagnostics ready state passed {result.BrowserDiagnosticsReadyChecks.Length} diagnostics readiness check(s) for profile '{diagnosticsSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser diagnostics ready state ready for profile '{diagnosticsSession.ProfileId}' with {result.BrowserDiagnosticsReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDiagnosticsReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDiagnosticsReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDiagnosticsSessionVersion { get; set; } = string.Empty;
    public string BrowserTelemetryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTelemetrySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDiagnosticsReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDiagnosticsReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserDiagnosticsSessionResult BrowserDiagnosticsSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
