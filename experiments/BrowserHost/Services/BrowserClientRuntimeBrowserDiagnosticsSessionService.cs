namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDiagnosticsSession
{
    ValueTask<BrowserClientRuntimeBrowserDiagnosticsSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDiagnosticsSessionService : IBrowserClientRuntimeBrowserDiagnosticsSession
{
    private readonly IBrowserClientRuntimeBrowserTelemetryReadyState _runtimeBrowserTelemetryReadyState;

    public BrowserClientRuntimeBrowserDiagnosticsSessionService(IBrowserClientRuntimeBrowserTelemetryReadyState runtimeBrowserTelemetryReadyState)
    {
        _runtimeBrowserTelemetryReadyState = runtimeBrowserTelemetryReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDiagnosticsSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTelemetryReadyStateResult telemetryReadyState = await _runtimeBrowserTelemetryReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDiagnosticsSessionResult result = new()
        {
            ProfileId = telemetryReadyState.ProfileId,
            SessionId = telemetryReadyState.SessionId,
            SessionPath = telemetryReadyState.SessionPath,
            BrowserTelemetryReadyStateVersion = telemetryReadyState.BrowserTelemetryReadyStateVersion,
            BrowserTelemetrySessionVersion = telemetryReadyState.BrowserTelemetrySessionVersion,
            LaunchMode = telemetryReadyState.LaunchMode,
            AssetRootPath = telemetryReadyState.AssetRootPath,
            ProfilesRootPath = telemetryReadyState.ProfilesRootPath,
            CacheRootPath = telemetryReadyState.CacheRootPath,
            ConfigRootPath = telemetryReadyState.ConfigRootPath,
            SettingsFilePath = telemetryReadyState.SettingsFilePath,
            StartupProfilePath = telemetryReadyState.StartupProfilePath,
            RequiredAssets = telemetryReadyState.RequiredAssets,
            ReadyAssetCount = telemetryReadyState.ReadyAssetCount,
            CompletedSteps = telemetryReadyState.CompletedSteps,
            TotalSteps = telemetryReadyState.TotalSteps,
            Exists = telemetryReadyState.Exists,
            ReadSucceeded = telemetryReadyState.ReadSucceeded,
            BrowserTelemetryReadyState = telemetryReadyState
        };

        if (!telemetryReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser diagnostics session blocked for profile '{telemetryReadyState.ProfileId}'.";
            result.Error = telemetryReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDiagnosticsSessionVersion = "runtime-browser-diagnostics-session-v1";
        result.BrowserDiagnosticsStages =
        [
            "open-browser-diagnostics-session",
            "bind-browser-telemetry-ready-state",
            "publish-browser-diagnostics-ready"
        ];
        result.BrowserDiagnosticsSummary = $"Runtime browser diagnostics session prepared {result.BrowserDiagnosticsStages.Length} diagnostics stage(s) for profile '{telemetryReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser diagnostics session ready for profile '{telemetryReadyState.ProfileId}' with {result.BrowserDiagnosticsStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDiagnosticsSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserDiagnosticsStages { get; set; } = Array.Empty<string>();
    public string BrowserDiagnosticsSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserTelemetryReadyStateResult BrowserTelemetryReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
