namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionDurabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionDurabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionDurabilityReadyStateService : IBrowserClientRuntimeBrowserProductionDurabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionDurabilitySession _runtimeBrowserProductionDurabilitySession;

    public BrowserClientRuntimeBrowserProductionDurabilityReadyStateService(IBrowserClientRuntimeBrowserProductionDurabilitySession runtimeBrowserProductionDurabilitySession)
    {
        _runtimeBrowserProductionDurabilitySession = runtimeBrowserProductionDurabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionDurabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionDurabilitySessionResult productiondurabilitySession = await _runtimeBrowserProductionDurabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserProductionDurabilityReadyStateResult result = new()
        {
            ProfileId = productiondurabilitySession.ProfileId,
            SessionId = productiondurabilitySession.SessionId,
            SessionPath = productiondurabilitySession.SessionPath,
            BrowserProductionDurabilitySessionVersion = productiondurabilitySession.BrowserProductionDurabilitySessionVersion,
            BrowserProductionResilienceReadyStateVersion = productiondurabilitySession.BrowserProductionResilienceReadyStateVersion,
            BrowserProductionResilienceSessionVersion = productiondurabilitySession.BrowserProductionResilienceSessionVersion,
            LaunchMode = productiondurabilitySession.LaunchMode,
            AssetRootPath = productiondurabilitySession.AssetRootPath,
            ProfilesRootPath = productiondurabilitySession.ProfilesRootPath,
            CacheRootPath = productiondurabilitySession.CacheRootPath,
            ConfigRootPath = productiondurabilitySession.ConfigRootPath,
            SettingsFilePath = productiondurabilitySession.SettingsFilePath,
            StartupProfilePath = productiondurabilitySession.StartupProfilePath,
            RequiredAssets = productiondurabilitySession.RequiredAssets,
            ReadyAssetCount = productiondurabilitySession.ReadyAssetCount,
            CompletedSteps = productiondurabilitySession.CompletedSteps,
            TotalSteps = productiondurabilitySession.TotalSteps,
            Exists = productiondurabilitySession.Exists,
            ReadSucceeded = productiondurabilitySession.ReadSucceeded
        };

        if (!productiondurabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productiondurability ready state blocked for profile '{productiondurabilitySession.ProfileId}'.";
            result.Error = productiondurabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionDurabilityReadyStateVersion = "runtime-browser-productiondurability-ready-state-v1";
        result.BrowserProductionDurabilityReadyChecks =
        [
            "browser-productionresilience-ready-state-ready",
            "browser-productiondurability-session-ready",
            "browser-productiondurability-ready"
        ];
        result.BrowserProductionDurabilityReadySummary = $"Runtime browser productiondurability ready state passed {result.BrowserProductionDurabilityReadyChecks.Length} productiondurability readiness check(s) for profile '{productiondurabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productiondurability ready state ready for profile '{productiondurabilitySession.ProfileId}' with {result.BrowserProductionDurabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionDurabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionDurabilityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionDurabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionDurabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
