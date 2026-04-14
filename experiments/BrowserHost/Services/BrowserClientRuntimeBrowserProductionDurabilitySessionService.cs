namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionDurabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserProductionDurabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionDurabilitySessionService : IBrowserClientRuntimeBrowserProductionDurabilitySession
{
    private readonly IBrowserClientRuntimeBrowserProductionResilienceReadyState _runtimeBrowserProductionResilienceReadyState;

    public BrowserClientRuntimeBrowserProductionDurabilitySessionService(IBrowserClientRuntimeBrowserProductionResilienceReadyState runtimeBrowserProductionResilienceReadyState)
    {
        _runtimeBrowserProductionResilienceReadyState = runtimeBrowserProductionResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionDurabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionResilienceReadyStateResult prevReadyState = await _runtimeBrowserProductionResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionDurabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserProductionResilienceReadyStateVersion = prevReadyState.BrowserProductionResilienceReadyStateVersion,
            BrowserProductionResilienceSessionVersion = prevReadyState.BrowserProductionResilienceSessionVersion,
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
            result.Summary = $"Runtime browser productiondurability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionDurabilitySessionVersion = "runtime-browser-productiondurability-session-v1";
        result.BrowserProductionDurabilityStages =
        [
            "open-browser-productiondurability-session",
            "bind-browser-productionresilience-ready-state",
            "publish-browser-productiondurability-ready"
        ];
        result.BrowserProductionDurabilitySummary = $"Runtime browser productiondurability session prepared {result.BrowserProductionDurabilityStages.Length} productiondurability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productiondurability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserProductionDurabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionDurabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserProductionResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionDurabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserProductionDurabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
