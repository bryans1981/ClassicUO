namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLivePersistenceSession
{
    ValueTask<BrowserClientRuntimeBrowserLivePersistenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLivePersistenceSessionService : IBrowserClientRuntimeBrowserLivePersistenceSession
{
    private readonly IBrowserClientRuntimeBrowserProductionContinuityReadyState _runtimeBrowserProductionContinuityReadyState;

    public BrowserClientRuntimeBrowserLivePersistenceSessionService(IBrowserClientRuntimeBrowserProductionContinuityReadyState runtimeBrowserProductionContinuityReadyState)
    {
        _runtimeBrowserProductionContinuityReadyState = runtimeBrowserProductionContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLivePersistenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionContinuityReadyStateResult prevReadyState = await _runtimeBrowserProductionContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLivePersistenceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
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
            result.Summary = $"Runtime browser livepersistence session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLivePersistenceSessionVersion = "runtime-browser-livepersistence-session-v1";
        result.BrowserLivePersistenceStages =
        [
            "open-browser-livepersistence-session",
            "bind-browser-productioncontinuity-ready-state",
            "publish-browser-livepersistence-ready"
        ];
        result.BrowserLivePersistenceSummary = $"Runtime browser livepersistence session prepared {result.BrowserLivePersistenceStages.Length} livepersistence stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livepersistence session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLivePersistenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLivePersistenceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserLivePersistenceStages { get; set; } = Array.Empty<string>();
    public string BrowserLivePersistenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
