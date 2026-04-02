namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserArchiveReadyState
{
    ValueTask<BrowserClientRuntimeBrowserArchiveReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserArchiveReadyStateService : IBrowserClientRuntimeBrowserArchiveReadyState
{
    private readonly IBrowserClientRuntimeBrowserArchiveSession _runtimeBrowserArchiveSession;

    public BrowserClientRuntimeBrowserArchiveReadyStateService(IBrowserClientRuntimeBrowserArchiveSession runtimeBrowserArchiveSession)
    {
        _runtimeBrowserArchiveSession = runtimeBrowserArchiveSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserArchiveReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserArchiveSessionResult archiveSession = await _runtimeBrowserArchiveSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserArchiveReadyStateResult result = new()
        {
            ProfileId = archiveSession.ProfileId,
            SessionId = archiveSession.SessionId,
            SessionPath = archiveSession.SessionPath,
            BrowserArchiveSessionVersion = archiveSession.BrowserArchiveSessionVersion,
            BrowserSnapshotReadyStateVersion = archiveSession.BrowserSnapshotReadyStateVersion,
            BrowserSnapshotSessionVersion = archiveSession.BrowserSnapshotSessionVersion,
            LaunchMode = archiveSession.LaunchMode,
            AssetRootPath = archiveSession.AssetRootPath,
            ProfilesRootPath = archiveSession.ProfilesRootPath,
            CacheRootPath = archiveSession.CacheRootPath,
            ConfigRootPath = archiveSession.ConfigRootPath,
            SettingsFilePath = archiveSession.SettingsFilePath,
            StartupProfilePath = archiveSession.StartupProfilePath,
            RequiredAssets = archiveSession.RequiredAssets,
            ReadyAssetCount = archiveSession.ReadyAssetCount,
            CompletedSteps = archiveSession.CompletedSteps,
            TotalSteps = archiveSession.TotalSteps,
            Exists = archiveSession.Exists,
            ReadSucceeded = archiveSession.ReadSucceeded,
            BrowserArchiveSession = archiveSession
        };

        if (!archiveSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser archive ready state blocked for profile '{archiveSession.ProfileId}'.";
            result.Error = archiveSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserArchiveReadyStateVersion = "runtime-browser-archive-ready-state-v1";
        result.BrowserArchiveReadyChecks =
        [
            "browser-snapshot-ready-state-ready",
            "browser-archive-session-ready",
            "browser-archive-ready"
        ];
        result.BrowserArchiveReadySummary = $"Runtime browser archive ready state passed {result.BrowserArchiveReadyChecks.Length} archive readiness check(s) for profile '{archiveSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser archive ready state ready for profile '{archiveSession.ProfileId}' with {result.BrowserArchiveReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserArchiveReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserArchiveReadyStateVersion { get; set; } = string.Empty;
    public string BrowserArchiveSessionVersion { get; set; } = string.Empty;
    public string BrowserSnapshotReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSnapshotSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserArchiveReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserArchiveReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserArchiveSessionResult BrowserArchiveSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
