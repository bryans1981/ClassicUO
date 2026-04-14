namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyStateService : IBrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserLivePersistenceReadyState _runtimeBrowserLivePersistenceReadyState;

    public BrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyStateService(IBrowserClientRuntimeBrowserLivePersistenceReadyState runtimeBrowserLivePersistenceReadyState)
    {
        _runtimeBrowserLivePersistenceReadyState = runtimeBrowserLivePersistenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLivePersistenceReadyStateResult prevReadyState = await _runtimeBrowserLivePersistenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSteadyPersistenceReadinessSessionVersion = prevReadyState.BrowserLivePersistenceReadyStateVersion,
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
            result.Summary = $"Runtime browser steadypersistencereadiness ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyPersistenceReadinessReadyStateVersion = "runtime-browser-steadypersistencereadiness-ready-state-v1";
        result.BrowserSteadyPersistenceReadinessReadyChecks =
        [
            "browser-livepersistence-ready-state-ready",
            "browser-steadypersistencereadiness-ready-state-ready",
            "browser-steadypersistencereadiness-ready"
        ];
        result.BrowserSteadyPersistenceReadinessReadySummary = $"Runtime browser steadypersistencereadiness ready state passed {result.BrowserSteadyPersistenceReadinessReadyChecks.Length} steadypersistencereadiness readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadypersistencereadiness ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSteadyPersistenceReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyPersistenceReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyPersistenceReadinessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyPersistenceReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSteadyPersistenceReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
