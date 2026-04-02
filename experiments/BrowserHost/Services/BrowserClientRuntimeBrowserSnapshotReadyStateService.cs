namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSnapshotReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSnapshotReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSnapshotReadyStateService : IBrowserClientRuntimeBrowserSnapshotReadyState
{
    private readonly IBrowserClientRuntimeBrowserSnapshotSession _runtimeBrowserSnapshotSession;

    public BrowserClientRuntimeBrowserSnapshotReadyStateService(IBrowserClientRuntimeBrowserSnapshotSession runtimeBrowserSnapshotSession)
    {
        _runtimeBrowserSnapshotSession = runtimeBrowserSnapshotSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSnapshotReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSnapshotSessionResult snapshotSession = await _runtimeBrowserSnapshotSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSnapshotReadyStateResult result = new()
        {
            ProfileId = snapshotSession.ProfileId,
            SessionId = snapshotSession.SessionId,
            SessionPath = snapshotSession.SessionPath,
            BrowserSnapshotSessionVersion = snapshotSession.BrowserSnapshotSessionVersion,
            BrowserRecoveryReadyStateVersion = snapshotSession.BrowserRecoveryReadyStateVersion,
            BrowserRecoverySessionVersion = snapshotSession.BrowserRecoverySessionVersion,
            LaunchMode = snapshotSession.LaunchMode,
            AssetRootPath = snapshotSession.AssetRootPath,
            ProfilesRootPath = snapshotSession.ProfilesRootPath,
            CacheRootPath = snapshotSession.CacheRootPath,
            ConfigRootPath = snapshotSession.ConfigRootPath,
            SettingsFilePath = snapshotSession.SettingsFilePath,
            StartupProfilePath = snapshotSession.StartupProfilePath,
            RequiredAssets = snapshotSession.RequiredAssets,
            ReadyAssetCount = snapshotSession.ReadyAssetCount,
            CompletedSteps = snapshotSession.CompletedSteps,
            TotalSteps = snapshotSession.TotalSteps,
            Exists = snapshotSession.Exists,
            ReadSucceeded = snapshotSession.ReadSucceeded,
            BrowserSnapshotSession = snapshotSession
        };

        if (!snapshotSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser snapshot ready state blocked for profile '{snapshotSession.ProfileId}'.";
            result.Error = snapshotSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSnapshotReadyStateVersion = "runtime-browser-snapshot-ready-state-v1";
        result.BrowserSnapshotReadyChecks =
        [
            "browser-recovery-ready-state-ready",
            "browser-snapshot-session-ready",
            "browser-snapshot-ready"
        ];
        result.BrowserSnapshotReadySummary = $"Runtime browser snapshot ready state passed {result.BrowserSnapshotReadyChecks.Length} snapshot readiness check(s) for profile '{snapshotSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser snapshot ready state ready for profile '{snapshotSession.ProfileId}' with {result.BrowserSnapshotReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSnapshotReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSnapshotReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSnapshotSessionVersion { get; set; } = string.Empty;
    public string BrowserRecoveryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRecoverySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSnapshotReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSnapshotReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserSnapshotSessionResult BrowserSnapshotSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
