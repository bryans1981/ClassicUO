namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPersistenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPersistenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPersistenceReadyStateService : IBrowserClientRuntimeBrowserPersistenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserPersistenceSession _runtimeBrowserPersistenceSession;

    public BrowserClientRuntimeBrowserPersistenceReadyStateService(IBrowserClientRuntimeBrowserPersistenceSession runtimeBrowserPersistenceSession)
    {
        _runtimeBrowserPersistenceSession = runtimeBrowserPersistenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPersistenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPersistenceSessionResult persistenceSession = await _runtimeBrowserPersistenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPersistenceReadyStateResult result = new()
        {
            ProfileId = persistenceSession.ProfileId,
            SessionId = persistenceSession.SessionId,
            SessionPath = persistenceSession.SessionPath,
            BrowserPersistenceSessionVersion = persistenceSession.BrowserPersistenceSessionVersion,
            BrowserCheckpointReadyStateVersion = persistenceSession.BrowserCheckpointReadyStateVersion,
            BrowserCheckpointSessionVersion = persistenceSession.BrowserCheckpointSessionVersion,
            LaunchMode = persistenceSession.LaunchMode,
            AssetRootPath = persistenceSession.AssetRootPath,
            ProfilesRootPath = persistenceSession.ProfilesRootPath,
            CacheRootPath = persistenceSession.CacheRootPath,
            ConfigRootPath = persistenceSession.ConfigRootPath,
            SettingsFilePath = persistenceSession.SettingsFilePath,
            StartupProfilePath = persistenceSession.StartupProfilePath,
            RequiredAssets = persistenceSession.RequiredAssets,
            ReadyAssetCount = persistenceSession.ReadyAssetCount,
            CompletedSteps = persistenceSession.CompletedSteps,
            TotalSteps = persistenceSession.TotalSteps,
            Exists = persistenceSession.Exists,
            ReadSucceeded = persistenceSession.ReadSucceeded
        };

        if (!persistenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser persistence ready state blocked for profile '{persistenceSession.ProfileId}'.";
            result.Error = persistenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPersistenceReadyStateVersion = "runtime-browser-persistence-ready-state-v1";
        result.BrowserPersistenceReadyChecks =
        [
            "browser-checkpoint-ready-state-ready",
            "browser-persistence-session-ready",
            "browser-persistence-ready"
        ];
        result.BrowserPersistenceReadySummary = $"Runtime browser persistence ready state passed {result.BrowserPersistenceReadyChecks.Length} persistence readiness check(s) for profile '{persistenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser persistence ready state ready for profile '{persistenceSession.ProfileId}' with {result.BrowserPersistenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPersistenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPersistenceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserPersistenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPersistenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
