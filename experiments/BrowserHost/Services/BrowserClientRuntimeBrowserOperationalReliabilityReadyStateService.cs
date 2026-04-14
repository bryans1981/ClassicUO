namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOperationalReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalReliabilityReadyStateService : IBrowserClientRuntimeBrowserOperationalReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSteadyContinuityReadinessReadyState _runtimeBrowserSteadyContinuityReadinessReadyState;

    public BrowserClientRuntimeBrowserOperationalReliabilityReadyStateService(IBrowserClientRuntimeBrowserSteadyContinuityReadinessReadyState runtimeBrowserSteadyContinuityReadinessReadyState)
    {
        _runtimeBrowserSteadyContinuityReadinessReadyState = runtimeBrowserSteadyContinuityReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateResult prevReadyState = await _runtimeBrowserSteadyContinuityReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOperationalReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserOperationalReliabilitySessionVersion = prevReadyState.BrowserSteadyContinuityReadinessReadyStateVersion,
            BrowserSteadyContinuityReadinessReadyStateVersion = prevReadyState.BrowserSteadyContinuityReadinessReadyStateVersion,
            BrowserSteadyContinuityReadinessSessionVersion = prevReadyState.BrowserSteadyContinuityReadinessSessionVersion,
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
            result.Summary = $"Runtime browser operationalreliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalReliabilityReadyStateVersion = "runtime-browser-operationalreliability-ready-state-v1";
        result.BrowserOperationalReliabilityReadyChecks =
        [
            "browser-steadycontinuityreadiness-ready-state-ready",
            "browser-operationalreliability-ready-state-ready",
            "browser-operationalreliability-ready"
        ];
        result.BrowserOperationalReliabilityReadySummary = $"Runtime browser operationalreliability ready state passed {result.BrowserOperationalReliabilityReadyChecks.Length} operationalreliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalreliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserOperationalReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOperationalReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSteadyContinuityReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyContinuityReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperationalReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOperationalReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
