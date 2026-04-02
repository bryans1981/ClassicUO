namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTenacitySession
{
    ValueTask<BrowserClientRuntimeBrowserTenacitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTenacitySessionService : IBrowserClientRuntimeBrowserTenacitySession
{
    private readonly IBrowserClientRuntimeBrowserPersistenceReadyState _runtimeBrowserPersistenceReadyState;

    public BrowserClientRuntimeBrowserTenacitySessionService(IBrowserClientRuntimeBrowserPersistenceReadyState runtimeBrowserPersistenceReadyState)
    {
        _runtimeBrowserPersistenceReadyState = runtimeBrowserPersistenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTenacitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPersistenceReadyStateResult persistenceReadyState = await _runtimeBrowserPersistenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTenacitySessionResult result = new()
        {
            ProfileId = persistenceReadyState.ProfileId,
            SessionId = persistenceReadyState.SessionId,
            SessionPath = persistenceReadyState.SessionPath,
            BrowserPersistenceReadyStateVersion = persistenceReadyState.BrowserPersistenceReadyStateVersion,
            BrowserPersistenceSessionVersion = persistenceReadyState.BrowserPersistenceSessionVersion,
            LaunchMode = persistenceReadyState.LaunchMode,
            AssetRootPath = persistenceReadyState.AssetRootPath,
            ProfilesRootPath = persistenceReadyState.ProfilesRootPath,
            CacheRootPath = persistenceReadyState.CacheRootPath,
            ConfigRootPath = persistenceReadyState.ConfigRootPath,
            SettingsFilePath = persistenceReadyState.SettingsFilePath,
            StartupProfilePath = persistenceReadyState.StartupProfilePath,
            RequiredAssets = persistenceReadyState.RequiredAssets,
            ReadyAssetCount = persistenceReadyState.ReadyAssetCount,
            CompletedSteps = persistenceReadyState.CompletedSteps,
            TotalSteps = persistenceReadyState.TotalSteps,
            Exists = persistenceReadyState.Exists,
            ReadSucceeded = persistenceReadyState.ReadSucceeded
        };

        if (!persistenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser tenacity session blocked for profile '{persistenceReadyState.ProfileId}'.";
            result.Error = persistenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTenacitySessionVersion = "runtime-browser-tenacity-session-v1";
        result.BrowserTenacityStages =
        [
            "open-browser-tenacity-session",
            "bind-browser-persistence-ready-state",
            "publish-browser-tenacity-ready"
        ];
        result.BrowserTenacitySummary = $"Runtime browser tenacity session prepared {result.BrowserTenacityStages.Length} tenacity stage(s) for profile '{persistenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser tenacity session ready for profile '{persistenceReadyState.ProfileId}' with {result.BrowserTenacityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTenacitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTenacitySessionVersion { get; set; } = string.Empty;
    public string BrowserPersistenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPersistenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTenacityStages { get; set; } = Array.Empty<string>();
    public string BrowserTenacitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
