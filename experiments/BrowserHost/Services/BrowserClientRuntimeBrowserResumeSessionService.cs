namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResumeSession
{
    ValueTask<BrowserClientRuntimeBrowserResumeSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResumeSessionService : IBrowserClientRuntimeBrowserResumeSession
{
    private readonly IBrowserClientRuntimeBrowserRestoreReadyState _runtimeBrowserRestoreReadyState;

    public BrowserClientRuntimeBrowserResumeSessionService(IBrowserClientRuntimeBrowserRestoreReadyState runtimeBrowserRestoreReadyState)
    {
        _runtimeBrowserRestoreReadyState = runtimeBrowserRestoreReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResumeSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRestoreReadyStateResult restoreReadyState = await _runtimeBrowserRestoreReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserResumeSessionResult result = new()
        {
            ProfileId = restoreReadyState.ProfileId,
            SessionId = restoreReadyState.SessionId,
            SessionPath = restoreReadyState.SessionPath,
            BrowserRestoreReadyStateVersion = restoreReadyState.BrowserRestoreReadyStateVersion,
            BrowserRestoreSessionVersion = restoreReadyState.BrowserRestoreSessionVersion,
            LaunchMode = restoreReadyState.LaunchMode,
            AssetRootPath = restoreReadyState.AssetRootPath,
            ProfilesRootPath = restoreReadyState.ProfilesRootPath,
            CacheRootPath = restoreReadyState.CacheRootPath,
            ConfigRootPath = restoreReadyState.ConfigRootPath,
            SettingsFilePath = restoreReadyState.SettingsFilePath,
            StartupProfilePath = restoreReadyState.StartupProfilePath,
            RequiredAssets = restoreReadyState.RequiredAssets,
            ReadyAssetCount = restoreReadyState.ReadyAssetCount,
            CompletedSteps = restoreReadyState.CompletedSteps,
            TotalSteps = restoreReadyState.TotalSteps,
            Exists = restoreReadyState.Exists,
            ReadSucceeded = restoreReadyState.ReadSucceeded,
            BrowserRestoreReadyState = restoreReadyState
        };

        if (!restoreReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resume session blocked for profile '{restoreReadyState.ProfileId}'.";
            result.Error = restoreReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResumeSessionVersion = "runtime-browser-resume-session-v1";
        result.BrowserResumeStages =
        [
            "open-browser-resume-session",
            "bind-browser-restore-ready-state",
            "publish-browser-resume-ready"
        ];
        result.BrowserResumeSummary = $"Runtime browser resume session prepared {result.BrowserResumeStages.Length} resume stage(s) for profile '{restoreReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resume session ready for profile '{restoreReadyState.ProfileId}' with {result.BrowserResumeStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResumeSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserResumeSessionVersion { get; set; } = string.Empty;
    public string BrowserRestoreReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRestoreSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResumeStages { get; set; } = Array.Empty<string>();
    public string BrowserResumeSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRestoreReadyStateResult BrowserRestoreReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
