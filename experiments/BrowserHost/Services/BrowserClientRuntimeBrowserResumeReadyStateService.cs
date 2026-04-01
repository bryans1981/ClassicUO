namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResumeReadyState
{
    ValueTask<BrowserClientRuntimeBrowserResumeReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResumeReadyStateService : IBrowserClientRuntimeBrowserResumeReadyState
{
    private readonly IBrowserClientRuntimeBrowserResumeSession _runtimeBrowserResumeSession;

    public BrowserClientRuntimeBrowserResumeReadyStateService(IBrowserClientRuntimeBrowserResumeSession runtimeBrowserResumeSession)
    {
        _runtimeBrowserResumeSession = runtimeBrowserResumeSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResumeReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResumeSessionResult resumeSession = await _runtimeBrowserResumeSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserResumeReadyStateResult result = new()
        {
            ProfileId = resumeSession.ProfileId,
            SessionId = resumeSession.SessionId,
            SessionPath = resumeSession.SessionPath,
            BrowserResumeSessionVersion = resumeSession.BrowserResumeSessionVersion,
            BrowserRestoreReadyStateVersion = resumeSession.BrowserRestoreReadyStateVersion,
            BrowserRestoreSessionVersion = resumeSession.BrowserRestoreSessionVersion,
            LaunchMode = resumeSession.LaunchMode,
            AssetRootPath = resumeSession.AssetRootPath,
            ProfilesRootPath = resumeSession.ProfilesRootPath,
            CacheRootPath = resumeSession.CacheRootPath,
            ConfigRootPath = resumeSession.ConfigRootPath,
            SettingsFilePath = resumeSession.SettingsFilePath,
            StartupProfilePath = resumeSession.StartupProfilePath,
            RequiredAssets = resumeSession.RequiredAssets,
            ReadyAssetCount = resumeSession.ReadyAssetCount,
            CompletedSteps = resumeSession.CompletedSteps,
            TotalSteps = resumeSession.TotalSteps,
            Exists = resumeSession.Exists,
            ReadSucceeded = resumeSession.ReadSucceeded,
            BrowserResumeSession = resumeSession
        };

        if (!resumeSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resume ready state blocked for profile '{resumeSession.ProfileId}'.";
            result.Error = resumeSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResumeReadyStateVersion = "runtime-browser-resume-ready-state-v1";
        result.BrowserResumeReadyChecks =
        [
            "browser-restore-ready-state-ready",
            "browser-resume-session-ready",
            "browser-resume-ready"
        ];
        result.BrowserResumeReadySummary = $"Runtime browser resume ready state passed {result.BrowserResumeReadyChecks.Length} resume readiness check(s) for profile '{resumeSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resume ready state ready for profile '{resumeSession.ProfileId}' with {result.BrowserResumeReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResumeReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserResumeReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserResumeReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserResumeReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserResumeSessionResult BrowserResumeSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
