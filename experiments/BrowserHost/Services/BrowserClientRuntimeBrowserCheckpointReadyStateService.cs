namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCheckpointReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCheckpointReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCheckpointReadyStateService : IBrowserClientRuntimeBrowserCheckpointReadyState
{
    private readonly IBrowserClientRuntimeBrowserCheckpointSession _runtimeBrowserCheckpointSession;

    public BrowserClientRuntimeBrowserCheckpointReadyStateService(IBrowserClientRuntimeBrowserCheckpointSession runtimeBrowserCheckpointSession)
    {
        _runtimeBrowserCheckpointSession = runtimeBrowserCheckpointSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCheckpointReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCheckpointSessionResult checkpointSession = await _runtimeBrowserCheckpointSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCheckpointReadyStateResult result = new()
        {
            ProfileId = checkpointSession.ProfileId,
            SessionId = checkpointSession.SessionId,
            SessionPath = checkpointSession.SessionPath,
            BrowserCheckpointSessionVersion = checkpointSession.BrowserCheckpointSessionVersion,
            BrowserResumeReadyStateVersion = checkpointSession.BrowserResumeReadyStateVersion,
            BrowserResumeSessionVersion = checkpointSession.BrowserResumeSessionVersion,
            LaunchMode = checkpointSession.LaunchMode,
            AssetRootPath = checkpointSession.AssetRootPath,
            ProfilesRootPath = checkpointSession.ProfilesRootPath,
            CacheRootPath = checkpointSession.CacheRootPath,
            ConfigRootPath = checkpointSession.ConfigRootPath,
            SettingsFilePath = checkpointSession.SettingsFilePath,
            StartupProfilePath = checkpointSession.StartupProfilePath,
            RequiredAssets = checkpointSession.RequiredAssets,
            ReadyAssetCount = checkpointSession.ReadyAssetCount,
            CompletedSteps = checkpointSession.CompletedSteps,
            TotalSteps = checkpointSession.TotalSteps,
            Exists = checkpointSession.Exists,
            ReadSucceeded = checkpointSession.ReadSucceeded,
            BrowserCheckpointSession = checkpointSession
        };

        if (!checkpointSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser checkpoint ready state blocked for profile '{checkpointSession.ProfileId}'.";
            result.Error = checkpointSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCheckpointReadyStateVersion = "runtime-browser-checkpoint-ready-state-v1";
        result.BrowserCheckpointReadyChecks =
        [
            "browser-resume-ready-state-ready",
            "browser-checkpoint-session-ready",
            "browser-checkpoint-ready"
        ];
        result.BrowserCheckpointReadySummary = $"Runtime browser checkpoint ready state passed {result.BrowserCheckpointReadyChecks.Length} checkpoint readiness check(s) for profile '{checkpointSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser checkpoint ready state ready for profile '{checkpointSession.ProfileId}' with {result.BrowserCheckpointReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCheckpointReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCheckpointReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserCheckpointReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCheckpointReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserCheckpointSessionResult BrowserCheckpointSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
