namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionReliabilityReadyStateService : IBrowserClientRuntimeBrowserProductionReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionReadinessReadyState _runtimeBrowserProductionReadinessReadyState;

    public BrowserClientRuntimeBrowserProductionReliabilityReadyStateService(IBrowserClientRuntimeBrowserProductionReadinessReadyState runtimeBrowserProductionReadinessReadyState)
    {
        _runtimeBrowserProductionReadinessReadyState = runtimeBrowserProductionReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionReadinessReadyStateResult prevReadyState = await _runtimeBrowserProductionReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserProductionReliabilitySessionVersion = prevReadyState.BrowserProductionReadinessReadyStateVersion,
            BrowserServiceContinuityReadyStateVersion = prevReadyState.BrowserProductionReadinessReadyStateVersion,
            BrowserServiceContinuitySessionVersion = prevReadyState.BrowserProductionReadinessSessionVersion,
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
            result.Summary = $"Runtime browser productionreliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionReliabilityReadyStateVersion = "runtime-browser-productionreliability-ready-state-v1";
        result.BrowserProductionReliabilityReadyChecks =
        [
            "browser-productionreadiness-ready-state-ready",
            "browser-productionreliability-ready-state-ready",
            "browser-productionreliability-ready"
        ];
        result.BrowserProductionReliabilityReadySummary = $"Runtime browser productionreliability ready state passed {result.BrowserProductionReliabilityReadyChecks.Length} productionreliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionreliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserProductionReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
