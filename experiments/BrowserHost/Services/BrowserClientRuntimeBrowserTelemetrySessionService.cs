namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTelemetrySession
{
    ValueTask<BrowserClientRuntimeBrowserTelemetrySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTelemetrySessionService : IBrowserClientRuntimeBrowserTelemetrySession
{
    private readonly IBrowserClientRuntimeBrowserArchiveReadyState _runtimeBrowserArchiveReadyState;

    public BrowserClientRuntimeBrowserTelemetrySessionService(IBrowserClientRuntimeBrowserArchiveReadyState runtimeBrowserArchiveReadyState)
    {
        _runtimeBrowserArchiveReadyState = runtimeBrowserArchiveReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTelemetrySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserArchiveReadyStateResult archiveReadyState = await _runtimeBrowserArchiveReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTelemetrySessionResult result = new()
        {
            ProfileId = archiveReadyState.ProfileId,
            SessionId = archiveReadyState.SessionId,
            SessionPath = archiveReadyState.SessionPath,
            BrowserArchiveReadyStateVersion = archiveReadyState.BrowserArchiveReadyStateVersion,
            BrowserArchiveSessionVersion = archiveReadyState.BrowserArchiveSessionVersion,
            LaunchMode = archiveReadyState.LaunchMode,
            AssetRootPath = archiveReadyState.AssetRootPath,
            ProfilesRootPath = archiveReadyState.ProfilesRootPath,
            CacheRootPath = archiveReadyState.CacheRootPath,
            ConfigRootPath = archiveReadyState.ConfigRootPath,
            SettingsFilePath = archiveReadyState.SettingsFilePath,
            StartupProfilePath = archiveReadyState.StartupProfilePath,
            RequiredAssets = archiveReadyState.RequiredAssets,
            ReadyAssetCount = archiveReadyState.ReadyAssetCount,
            CompletedSteps = archiveReadyState.CompletedSteps,
            TotalSteps = archiveReadyState.TotalSteps,
            Exists = archiveReadyState.Exists,
            ReadSucceeded = archiveReadyState.ReadSucceeded,
            BrowserArchiveReadyState = archiveReadyState
        };

        if (!archiveReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser telemetry session blocked for profile '{archiveReadyState.ProfileId}'.";
            result.Error = archiveReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTelemetrySessionVersion = "runtime-browser-telemetry-session-v1";
        result.BrowserTelemetryStages =
        [
            "open-browser-telemetry-session",
            "bind-browser-archive-ready-state",
            "publish-browser-telemetry-ready"
        ];
        result.BrowserTelemetrySummary = $"Runtime browser telemetry session prepared {result.BrowserTelemetryStages.Length} telemetry stage(s) for profile '{archiveReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser telemetry session ready for profile '{archiveReadyState.ProfileId}' with {result.BrowserTelemetryStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTelemetrySessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserTelemetryStages { get; set; } = Array.Empty<string>();
    public string BrowserTelemetrySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserArchiveReadyStateResult BrowserArchiveReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
