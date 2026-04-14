namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionResilienceReadyStateService : IBrowserClientRuntimeBrowserProductionResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionResilienceSession _runtimeBrowserProductionResilienceSession;

    public BrowserClientRuntimeBrowserProductionResilienceReadyStateService(IBrowserClientRuntimeBrowserProductionResilienceSession runtimeBrowserProductionResilienceSession)
    {
        _runtimeBrowserProductionResilienceSession = runtimeBrowserProductionResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionResilienceSessionResult productionresilienceSession = await _runtimeBrowserProductionResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserProductionResilienceReadyStateResult result = new()
        {
            ProfileId = productionresilienceSession.ProfileId,
            SessionId = productionresilienceSession.SessionId,
            SessionPath = productionresilienceSession.SessionPath,
            BrowserProductionResilienceSessionVersion = productionresilienceSession.BrowserProductionResilienceSessionVersion,
            BrowserServiceDurabilityReadyStateVersion = productionresilienceSession.BrowserServiceDurabilityReadyStateVersion,
            BrowserServiceDurabilitySessionVersion = productionresilienceSession.BrowserServiceDurabilitySessionVersion,
            LaunchMode = productionresilienceSession.LaunchMode,
            AssetRootPath = productionresilienceSession.AssetRootPath,
            ProfilesRootPath = productionresilienceSession.ProfilesRootPath,
            CacheRootPath = productionresilienceSession.CacheRootPath,
            ConfigRootPath = productionresilienceSession.ConfigRootPath,
            SettingsFilePath = productionresilienceSession.SettingsFilePath,
            StartupProfilePath = productionresilienceSession.StartupProfilePath,
            RequiredAssets = productionresilienceSession.RequiredAssets,
            ReadyAssetCount = productionresilienceSession.ReadyAssetCount,
            CompletedSteps = productionresilienceSession.CompletedSteps,
            TotalSteps = productionresilienceSession.TotalSteps,
            Exists = productionresilienceSession.Exists,
            ReadSucceeded = productionresilienceSession.ReadSucceeded
        };

        if (!productionresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionresilience ready state blocked for profile '{productionresilienceSession.ProfileId}'.";
            result.Error = productionresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionResilienceReadyStateVersion = "runtime-browser-productionresilience-ready-state-v1";
        result.BrowserProductionResilienceReadyChecks =
        [
            "browser-servicedurability-ready-state-ready",
            "browser-productionresilience-session-ready",
            "browser-productionresilience-ready"
        ];
        result.BrowserProductionResilienceReadySummary = $"Runtime browser productionresilience ready state passed {result.BrowserProductionResilienceReadyChecks.Length} productionresilience readiness check(s) for profile '{productionresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionresilience ready state ready for profile '{productionresilienceSession.ProfileId}' with {result.BrowserProductionResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionResilienceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
