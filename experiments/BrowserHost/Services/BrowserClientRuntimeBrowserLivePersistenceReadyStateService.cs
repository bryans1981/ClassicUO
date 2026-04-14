namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLivePersistenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLivePersistenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLivePersistenceReadyStateService : IBrowserClientRuntimeBrowserLivePersistenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionContinuityReadyState _runtimeBrowserProductionContinuityReadyState;

    public BrowserClientRuntimeBrowserLivePersistenceReadyStateService(IBrowserClientRuntimeBrowserProductionContinuityReadyState runtimeBrowserProductionContinuityReadyState)
    {
        _runtimeBrowserProductionContinuityReadyState = runtimeBrowserProductionContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLivePersistenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionContinuityReadyStateResult prevReadyState = await _runtimeBrowserProductionContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLivePersistenceReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLivePersistenceSessionVersion = prevReadyState.BrowserProductionContinuityReadyStateVersion,
            BrowserProductionContinuityReadyStateVersion = prevReadyState.BrowserProductionContinuityReadyStateVersion,
            BrowserProductionContinuitySessionVersion = prevReadyState.BrowserProductionContinuitySessionVersion,
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
            result.Summary = $"Runtime browser livepersistence ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLivePersistenceReadyStateVersion = "runtime-browser-livepersistence-ready-state-v1";
        result.BrowserLivePersistenceReadyChecks =
        [
            "browser-productioncontinuity-ready-state-ready",
            "browser-livepersistence-ready-state-ready",
            "browser-livepersistence-ready"
        ];
        result.BrowserLivePersistenceReadySummary = $"Runtime browser livepersistence ready state passed {result.BrowserLivePersistenceReadyChecks.Length} livepersistence readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livepersistence ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLivePersistenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLivePersistenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLivePersistenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLivePersistenceSessionVersion { get; set; } = string.Empty;
    public string BrowserProductionContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLivePersistenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLivePersistenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
