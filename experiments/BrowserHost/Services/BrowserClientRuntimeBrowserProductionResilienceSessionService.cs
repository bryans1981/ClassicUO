namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserProductionResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionResilienceSessionService : IBrowserClientRuntimeBrowserProductionResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserServiceDurabilityReadyState _runtimeBrowserServiceDurabilityReadyState;

    public BrowserClientRuntimeBrowserProductionResilienceSessionService(IBrowserClientRuntimeBrowserServiceDurabilityReadyState runtimeBrowserServiceDurabilityReadyState)
    {
        _runtimeBrowserServiceDurabilityReadyState = runtimeBrowserServiceDurabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceDurabilityReadyStateResult prevReadyState = await _runtimeBrowserServiceDurabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserServiceDurabilityReadyStateVersion = prevReadyState.BrowserServiceDurabilityReadyStateVersion,
            BrowserServiceDurabilitySessionVersion = prevReadyState.BrowserServiceDurabilitySessionVersion,
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
            result.Summary = $"Runtime browser productionresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionResilienceSessionVersion = "runtime-browser-productionresilience-session-v1";
        result.BrowserProductionResilienceStages =
        [
            "open-browser-productionresilience-session",
            "bind-browser-servicedurability-ready-state",
            "publish-browser-productionresilience-ready"
        ];
        result.BrowserProductionResilienceSummary = $"Runtime browser productionresilience session prepared {result.BrowserProductionResilienceStages.Length} productionresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserProductionResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserServiceDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserProductionResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
