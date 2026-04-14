namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyPersistenceReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserSteadyPersistenceReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyPersistenceReadinessSessionService : IBrowserClientRuntimeBrowserSteadyPersistenceReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserLivePersistenceReadyState _runtimeBrowserLivePersistenceReadyState;

    public BrowserClientRuntimeBrowserSteadyPersistenceReadinessSessionService(IBrowserClientRuntimeBrowserLivePersistenceReadyState runtimeBrowserLivePersistenceReadyState)
    {
        _runtimeBrowserLivePersistenceReadyState = runtimeBrowserLivePersistenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyPersistenceReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLivePersistenceReadyStateResult prevReadyState = await _runtimeBrowserLivePersistenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadyPersistenceReadinessSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLivePersistenceReadyStateVersion = prevReadyState.BrowserLivePersistenceReadyStateVersion,
            BrowserLivePersistenceSessionVersion = prevReadyState.BrowserLivePersistenceSessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadypersistencereadiness session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyPersistenceReadinessSessionVersion = "runtime-browser-steadypersistencereadiness-session-v1";
        result.BrowserSteadyPersistenceReadinessStages =
        [
            "open-browser-steadypersistencereadiness-session",
            "bind-browser-livepersistence-ready-state",
            "publish-browser-steadypersistencereadiness-ready"
        ];
        result.BrowserSteadyPersistenceReadinessSummary = $"Runtime browser steadypersistencereadiness session prepared {result.BrowserSteadyPersistenceReadinessStages.Length} steadypersistencereadiness stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadypersistencereadiness session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSteadyPersistenceReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyPersistenceReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyPersistenceReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLivePersistenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLivePersistenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyPersistenceReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserSteadyPersistenceReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
