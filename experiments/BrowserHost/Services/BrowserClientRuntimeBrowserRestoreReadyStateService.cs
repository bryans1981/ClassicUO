namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRestoreReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRestoreReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRestoreReadyStateService : IBrowserClientRuntimeBrowserRestoreReadyState
{
    private readonly IBrowserClientRuntimeBrowserRestoreSession _runtimeBrowserRestoreSession;

    public BrowserClientRuntimeBrowserRestoreReadyStateService(IBrowserClientRuntimeBrowserRestoreSession runtimeBrowserRestoreSession)
    {
        _runtimeBrowserRestoreSession = runtimeBrowserRestoreSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRestoreReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRestoreSessionResult restoreSession = await _runtimeBrowserRestoreSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRestoreReadyStateResult result = new()
        {
            ProfileId = restoreSession.ProfileId,
            SessionId = restoreSession.SessionId,
            SessionPath = restoreSession.SessionPath,
            BrowserRestoreSessionVersion = restoreSession.BrowserRestoreSessionVersion,
            BrowserStateSyncReadyStateVersion = restoreSession.BrowserStateSyncReadyStateVersion,
            BrowserStateSyncSessionVersion = restoreSession.BrowserStateSyncSessionVersion,
            LaunchMode = restoreSession.LaunchMode,
            AssetRootPath = restoreSession.AssetRootPath,
            ProfilesRootPath = restoreSession.ProfilesRootPath,
            CacheRootPath = restoreSession.CacheRootPath,
            ConfigRootPath = restoreSession.ConfigRootPath,
            SettingsFilePath = restoreSession.SettingsFilePath,
            StartupProfilePath = restoreSession.StartupProfilePath,
            RequiredAssets = restoreSession.RequiredAssets,
            ReadyAssetCount = restoreSession.ReadyAssetCount,
            CompletedSteps = restoreSession.CompletedSteps,
            TotalSteps = restoreSession.TotalSteps,
            Exists = restoreSession.Exists,
            ReadSucceeded = restoreSession.ReadSucceeded,
            BrowserRestoreSession = restoreSession
        };

        if (!restoreSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser restore ready state blocked for profile '{restoreSession.ProfileId}'.";
            result.Error = restoreSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRestoreReadyStateVersion = "runtime-browser-restore-ready-state-v1";
        result.BrowserRestoreReadyChecks =
        [
            "browser-state-sync-ready-state-ready",
            "browser-restore-session-ready",
            "browser-restore-ready"
        ];
        result.BrowserRestoreReadySummary = $"Runtime browser restore ready state passed {result.BrowserRestoreReadyChecks.Length} restore readiness check(s) for profile '{restoreSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser restore ready state ready for profile '{restoreSession.ProfileId}' with {result.BrowserRestoreReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRestoreReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRestoreReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRestoreSessionVersion { get; set; } = string.Empty;
    public string BrowserStateSyncReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateSyncSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRestoreReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRestoreReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRestoreSessionResult BrowserRestoreSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
