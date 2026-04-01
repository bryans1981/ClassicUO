namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRestoreSession
{
    ValueTask<BrowserClientRuntimeBrowserRestoreSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRestoreSessionService : IBrowserClientRuntimeBrowserRestoreSession
{
    private readonly IBrowserClientRuntimeBrowserStateSyncReadyState _runtimeBrowserStateSyncReadyState;

    public BrowserClientRuntimeBrowserRestoreSessionService(IBrowserClientRuntimeBrowserStateSyncReadyState runtimeBrowserStateSyncReadyState)
    {
        _runtimeBrowserStateSyncReadyState = runtimeBrowserStateSyncReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRestoreSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateSyncReadyStateResult stateSyncReadyState = await _runtimeBrowserStateSyncReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRestoreSessionResult result = new()
        {
            ProfileId = stateSyncReadyState.ProfileId,
            SessionId = stateSyncReadyState.SessionId,
            SessionPath = stateSyncReadyState.SessionPath,
            BrowserStateSyncReadyStateVersion = stateSyncReadyState.BrowserStateSyncReadyStateVersion,
            BrowserStateSyncSessionVersion = stateSyncReadyState.BrowserStateSyncSessionVersion,
            LaunchMode = stateSyncReadyState.LaunchMode,
            AssetRootPath = stateSyncReadyState.AssetRootPath,
            ProfilesRootPath = stateSyncReadyState.ProfilesRootPath,
            CacheRootPath = stateSyncReadyState.CacheRootPath,
            ConfigRootPath = stateSyncReadyState.ConfigRootPath,
            SettingsFilePath = stateSyncReadyState.SettingsFilePath,
            StartupProfilePath = stateSyncReadyState.StartupProfilePath,
            RequiredAssets = stateSyncReadyState.RequiredAssets,
            ReadyAssetCount = stateSyncReadyState.ReadyAssetCount,
            CompletedSteps = stateSyncReadyState.CompletedSteps,
            TotalSteps = stateSyncReadyState.TotalSteps,
            Exists = stateSyncReadyState.Exists,
            ReadSucceeded = stateSyncReadyState.ReadSucceeded,
            BrowserStateSyncReadyState = stateSyncReadyState
        };

        if (!stateSyncReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser restore session blocked for profile '{stateSyncReadyState.ProfileId}'.";
            result.Error = stateSyncReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRestoreSessionVersion = "runtime-browser-restore-session-v1";
        result.BrowserRestoreStages =
        [
            "open-browser-restore-session",
            "bind-browser-state-sync-ready-state",
            "publish-browser-restore-ready"
        ];
        result.BrowserRestoreSummary = $"Runtime browser restore session prepared {result.BrowserRestoreStages.Length} restore stage(s) for profile '{stateSyncReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser restore session ready for profile '{stateSyncReadyState.ProfileId}' with {result.BrowserRestoreStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRestoreSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserRestoreStages { get; set; } = Array.Empty<string>();
    public string BrowserRestoreSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserStateSyncReadyStateResult BrowserStateSyncReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
