namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTelemetryReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTelemetryReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTelemetryReadyStateService : IBrowserClientRuntimeBrowserTelemetryReadyState
{
    private readonly IBrowserClientRuntimeBrowserTelemetrySession _runtimeBrowserTelemetrySession;

    public BrowserClientRuntimeBrowserTelemetryReadyStateService(IBrowserClientRuntimeBrowserTelemetrySession runtimeBrowserTelemetrySession)
    {
        _runtimeBrowserTelemetrySession = runtimeBrowserTelemetrySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTelemetryReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTelemetrySessionResult telemetrySession = await _runtimeBrowserTelemetrySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTelemetryReadyStateResult result = new()
        {
            ProfileId = telemetrySession.ProfileId,
            SessionId = telemetrySession.SessionId,
            SessionPath = telemetrySession.SessionPath,
            BrowserTelemetrySessionVersion = telemetrySession.BrowserTelemetrySessionVersion,
            BrowserArchiveReadyStateVersion = telemetrySession.BrowserArchiveReadyStateVersion,
            BrowserArchiveSessionVersion = telemetrySession.BrowserArchiveSessionVersion,
            LaunchMode = telemetrySession.LaunchMode,
            AssetRootPath = telemetrySession.AssetRootPath,
            ProfilesRootPath = telemetrySession.ProfilesRootPath,
            CacheRootPath = telemetrySession.CacheRootPath,
            ConfigRootPath = telemetrySession.ConfigRootPath,
            SettingsFilePath = telemetrySession.SettingsFilePath,
            StartupProfilePath = telemetrySession.StartupProfilePath,
            RequiredAssets = telemetrySession.RequiredAssets,
            ReadyAssetCount = telemetrySession.ReadyAssetCount,
            CompletedSteps = telemetrySession.CompletedSteps,
            TotalSteps = telemetrySession.TotalSteps,
            Exists = telemetrySession.Exists,
            ReadSucceeded = telemetrySession.ReadSucceeded,
            BrowserTelemetrySession = telemetrySession
        };

        if (!telemetrySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser telemetry ready state blocked for profile '{telemetrySession.ProfileId}'.";
            result.Error = telemetrySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTelemetryReadyStateVersion = "runtime-browser-telemetry-ready-state-v1";
        result.BrowserTelemetryReadyChecks =
        [
            "browser-archive-ready-state-ready",
            "browser-telemetry-session-ready",
            "browser-telemetry-ready"
        ];
        result.BrowserTelemetryReadySummary = $"Runtime browser telemetry ready state passed {result.BrowserTelemetryReadyChecks.Length} telemetry readiness check(s) for profile '{telemetrySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser telemetry ready state ready for profile '{telemetrySession.ProfileId}' with {result.BrowserTelemetryReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTelemetryReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTelemetryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTelemetrySessionVersion { get; set; } = string.Empty;
    public string BrowserArchiveReadyStateVersion { get; set; } = string.Empty;
    public string BrowserArchiveSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTelemetryReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTelemetryReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserTelemetrySessionResult BrowserTelemetrySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
