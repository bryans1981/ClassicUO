namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPersistenceSession
{
    ValueTask<BrowserClientRuntimeBrowserPersistenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPersistenceSessionService : IBrowserClientRuntimeBrowserPersistenceSession
{
    private readonly IBrowserClientRuntimeBrowserCheckpointReadyState _runtimeBrowserCheckpointReadyState;

    public BrowserClientRuntimeBrowserPersistenceSessionService(IBrowserClientRuntimeBrowserCheckpointReadyState runtimeBrowserCheckpointReadyState)
    {
        _runtimeBrowserCheckpointReadyState = runtimeBrowserCheckpointReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPersistenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCheckpointReadyStateResult checkpointReadyState = await _runtimeBrowserCheckpointReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPersistenceSessionResult result = new()
        {
            ProfileId = checkpointReadyState.ProfileId,
            SessionId = checkpointReadyState.SessionId,
            SessionPath = checkpointReadyState.SessionPath,
            BrowserCheckpointReadyStateVersion = checkpointReadyState.BrowserCheckpointReadyStateVersion,
            BrowserCheckpointSessionVersion = checkpointReadyState.BrowserCheckpointSessionVersion,
            LaunchMode = checkpointReadyState.LaunchMode,
            AssetRootPath = checkpointReadyState.AssetRootPath,
            ProfilesRootPath = checkpointReadyState.ProfilesRootPath,
            CacheRootPath = checkpointReadyState.CacheRootPath,
            ConfigRootPath = checkpointReadyState.ConfigRootPath,
            SettingsFilePath = checkpointReadyState.SettingsFilePath,
            StartupProfilePath = checkpointReadyState.StartupProfilePath,
            RequiredAssets = checkpointReadyState.RequiredAssets,
            ReadyAssetCount = checkpointReadyState.ReadyAssetCount,
            CompletedSteps = checkpointReadyState.CompletedSteps,
            TotalSteps = checkpointReadyState.TotalSteps,
            Exists = checkpointReadyState.Exists,
            ReadSucceeded = checkpointReadyState.ReadSucceeded
        };

        if (!checkpointReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser persistence session blocked for profile '{checkpointReadyState.ProfileId}'.";
            result.Error = checkpointReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPersistenceSessionVersion = "runtime-browser-persistence-session-v1";
        result.BrowserPersistenceStages =
        [
            "open-browser-persistence-session",
            "bind-browser-checkpoint-ready-state",
            "publish-browser-persistence-ready"
        ];
        result.BrowserPersistenceSummary = $"Runtime browser persistence session prepared {result.BrowserPersistenceStages.Length} persistence stage(s) for profile '{checkpointReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser persistence session ready for profile '{checkpointReadyState.ProfileId}' with {result.BrowserPersistenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPersistenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPersistenceSessionVersion { get; set; } = string.Empty;
    public string BrowserCheckpointReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCheckpointSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPersistenceStages { get; set; } = Array.Empty<string>();
    public string BrowserPersistenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
