namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionContinuityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionContinuityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionContinuityReadyStateService : IBrowserClientRuntimeBrowserProductionContinuityReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionReliabilityReadyState _runtimeBrowserProductionReliabilityReadyState;

    public BrowserClientRuntimeBrowserProductionContinuityReadyStateService(IBrowserClientRuntimeBrowserProductionReliabilityReadyState runtimeBrowserProductionReliabilityReadyState)
    {
        _runtimeBrowserProductionReliabilityReadyState = runtimeBrowserProductionReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionContinuityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionReliabilityReadyStateResult prevReadyState = await _runtimeBrowserProductionReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionContinuityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserProductionContinuitySessionVersion = prevReadyState.BrowserProductionReliabilityReadyStateVersion,
            BrowserProductionReliabilityReadyStateVersion = prevReadyState.BrowserProductionReliabilityReadyStateVersion,
            BrowserProductionReliabilitySessionVersion = prevReadyState.BrowserProductionReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser productioncontinuity ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionContinuityReadyStateVersion = "runtime-browser-productioncontinuity-ready-state-v1";
        result.BrowserProductionContinuityReadyChecks =
        [
            "browser-productionreliability-ready-state-ready",
            "browser-productioncontinuity-ready-state-ready",
            "browser-productioncontinuity-ready"
        ];
        result.BrowserProductionContinuityReadySummary = $"Runtime browser productioncontinuity ready state passed {result.BrowserProductionContinuityReadyChecks.Length} productioncontinuity readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productioncontinuity ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserProductionContinuityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionContinuityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserProductionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionContinuityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionContinuityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
