namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSnapshotSession
{
    ValueTask<BrowserClientRuntimeBrowserSnapshotSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSnapshotSessionService : IBrowserClientRuntimeBrowserSnapshotSession
{
    private readonly IBrowserClientRuntimeBrowserRecoveryReadyState _runtimeBrowserRecoveryReadyState;

    public BrowserClientRuntimeBrowserSnapshotSessionService(IBrowserClientRuntimeBrowserRecoveryReadyState runtimeBrowserRecoveryReadyState)
    {
        _runtimeBrowserRecoveryReadyState = runtimeBrowserRecoveryReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSnapshotSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRecoveryReadyStateResult recoveryReadyState = await _runtimeBrowserRecoveryReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSnapshotSessionResult result = new()
        {
            ProfileId = recoveryReadyState.ProfileId,
            SessionId = recoveryReadyState.SessionId,
            SessionPath = recoveryReadyState.SessionPath,
            BrowserRecoveryReadyStateVersion = recoveryReadyState.BrowserRecoveryReadyStateVersion,
            BrowserRecoverySessionVersion = recoveryReadyState.BrowserRecoverySessionVersion,
            LaunchMode = recoveryReadyState.LaunchMode,
            AssetRootPath = recoveryReadyState.AssetRootPath,
            ProfilesRootPath = recoveryReadyState.ProfilesRootPath,
            CacheRootPath = recoveryReadyState.CacheRootPath,
            ConfigRootPath = recoveryReadyState.ConfigRootPath,
            SettingsFilePath = recoveryReadyState.SettingsFilePath,
            StartupProfilePath = recoveryReadyState.StartupProfilePath,
            RequiredAssets = recoveryReadyState.RequiredAssets,
            ReadyAssetCount = recoveryReadyState.ReadyAssetCount,
            CompletedSteps = recoveryReadyState.CompletedSteps,
            TotalSteps = recoveryReadyState.TotalSteps,
            Exists = recoveryReadyState.Exists,
            ReadSucceeded = recoveryReadyState.ReadSucceeded,
            BrowserRecoveryReadyState = recoveryReadyState
        };

        if (!recoveryReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser snapshot session blocked for profile '{recoveryReadyState.ProfileId}'.";
            result.Error = recoveryReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSnapshotSessionVersion = "runtime-browser-snapshot-session-v1";
        result.BrowserSnapshotStages =
        [
            "open-browser-snapshot-session",
            "bind-browser-recovery-ready-state",
            "publish-browser-snapshot-ready"
        ];
        result.BrowserSnapshotSummary = $"Runtime browser snapshot session prepared {result.BrowserSnapshotStages.Length} snapshot stage(s) for profile '{recoveryReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser snapshot session ready for profile '{recoveryReadyState.ProfileId}' with {result.BrowserSnapshotStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSnapshotSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserSnapshotStages { get; set; } = Array.Empty<string>();
    public string BrowserSnapshotSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRecoveryReadyStateResult BrowserRecoveryReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
