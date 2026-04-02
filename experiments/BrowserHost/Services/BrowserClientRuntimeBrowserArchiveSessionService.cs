namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserArchiveSession
{
    ValueTask<BrowserClientRuntimeBrowserArchiveSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserArchiveSessionService : IBrowserClientRuntimeBrowserArchiveSession
{
    private readonly IBrowserClientRuntimeBrowserSnapshotReadyState _runtimeBrowserSnapshotReadyState;

    public BrowserClientRuntimeBrowserArchiveSessionService(IBrowserClientRuntimeBrowserSnapshotReadyState runtimeBrowserSnapshotReadyState)
    {
        _runtimeBrowserSnapshotReadyState = runtimeBrowserSnapshotReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserArchiveSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSnapshotReadyStateResult snapshotReadyState = await _runtimeBrowserSnapshotReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserArchiveSessionResult result = new()
        {
            ProfileId = snapshotReadyState.ProfileId,
            SessionId = snapshotReadyState.SessionId,
            SessionPath = snapshotReadyState.SessionPath,
            BrowserSnapshotReadyStateVersion = snapshotReadyState.BrowserSnapshotReadyStateVersion,
            BrowserSnapshotSessionVersion = snapshotReadyState.BrowserSnapshotSessionVersion,
            LaunchMode = snapshotReadyState.LaunchMode,
            AssetRootPath = snapshotReadyState.AssetRootPath,
            ProfilesRootPath = snapshotReadyState.ProfilesRootPath,
            CacheRootPath = snapshotReadyState.CacheRootPath,
            ConfigRootPath = snapshotReadyState.ConfigRootPath,
            SettingsFilePath = snapshotReadyState.SettingsFilePath,
            StartupProfilePath = snapshotReadyState.StartupProfilePath,
            RequiredAssets = snapshotReadyState.RequiredAssets,
            ReadyAssetCount = snapshotReadyState.ReadyAssetCount,
            CompletedSteps = snapshotReadyState.CompletedSteps,
            TotalSteps = snapshotReadyState.TotalSteps,
            Exists = snapshotReadyState.Exists,
            ReadSucceeded = snapshotReadyState.ReadSucceeded,
            BrowserSnapshotReadyState = snapshotReadyState
        };

        if (!snapshotReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser archive session blocked for profile '{snapshotReadyState.ProfileId}'.";
            result.Error = snapshotReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserArchiveSessionVersion = "runtime-browser-archive-session-v1";
        result.BrowserArchiveStages =
        [
            "open-browser-archive-session",
            "bind-browser-snapshot-ready-state",
            "publish-browser-archive-ready"
        ];
        result.BrowserArchiveSummary = $"Runtime browser archive session prepared {result.BrowserArchiveStages.Length} archive stage(s) for profile '{snapshotReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser archive session ready for profile '{snapshotReadyState.ProfileId}' with {result.BrowserArchiveStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserArchiveSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserArchiveStages { get; set; } = Array.Empty<string>();
    public string BrowserArchiveSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserSnapshotReadyStateResult BrowserSnapshotReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
