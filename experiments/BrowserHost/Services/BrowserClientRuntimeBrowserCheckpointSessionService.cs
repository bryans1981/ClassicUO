namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCheckpointSession
{
    ValueTask<BrowserClientRuntimeBrowserCheckpointSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCheckpointSessionService : IBrowserClientRuntimeBrowserCheckpointSession
{
    private readonly IBrowserClientRuntimeBrowserResumeReadyState _runtimeBrowserResumeReadyState;

    public BrowserClientRuntimeBrowserCheckpointSessionService(IBrowserClientRuntimeBrowserResumeReadyState runtimeBrowserResumeReadyState)
    {
        _runtimeBrowserResumeReadyState = runtimeBrowserResumeReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCheckpointSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResumeReadyStateResult resumeReadyState = await _runtimeBrowserResumeReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCheckpointSessionResult result = new()
        {
            ProfileId = resumeReadyState.ProfileId,
            SessionId = resumeReadyState.SessionId,
            SessionPath = resumeReadyState.SessionPath,
            BrowserResumeReadyStateVersion = resumeReadyState.BrowserResumeReadyStateVersion,
            BrowserResumeSessionVersion = resumeReadyState.BrowserResumeSessionVersion,
            LaunchMode = resumeReadyState.LaunchMode,
            AssetRootPath = resumeReadyState.AssetRootPath,
            ProfilesRootPath = resumeReadyState.ProfilesRootPath,
            CacheRootPath = resumeReadyState.CacheRootPath,
            ConfigRootPath = resumeReadyState.ConfigRootPath,
            SettingsFilePath = resumeReadyState.SettingsFilePath,
            StartupProfilePath = resumeReadyState.StartupProfilePath,
            RequiredAssets = resumeReadyState.RequiredAssets,
            ReadyAssetCount = resumeReadyState.ReadyAssetCount,
            CompletedSteps = resumeReadyState.CompletedSteps,
            TotalSteps = resumeReadyState.TotalSteps,
            Exists = resumeReadyState.Exists,
            ReadSucceeded = resumeReadyState.ReadSucceeded,
            BrowserResumeReadyState = resumeReadyState
        };

        if (!resumeReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser checkpoint session blocked for profile '{resumeReadyState.ProfileId}'.";
            result.Error = resumeReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCheckpointSessionVersion = "runtime-browser-checkpoint-session-v1";
        result.BrowserCheckpointStages =
        [
            "open-browser-checkpoint-session",
            "bind-browser-resume-ready-state",
            "publish-browser-checkpoint-ready"
        ];
        result.BrowserCheckpointSummary = $"Runtime browser checkpoint session prepared {result.BrowserCheckpointStages.Length} checkpoint stage(s) for profile '{resumeReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser checkpoint session ready for profile '{resumeReadyState.ProfileId}' with {result.BrowserCheckpointStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCheckpointSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCheckpointSessionVersion { get; set; } = string.Empty;
    public string BrowserResumeReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResumeSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCheckpointStages { get; set; } = Array.Empty<string>();
    public string BrowserCheckpointSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserResumeReadyStateResult BrowserResumeReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
