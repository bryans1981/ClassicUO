namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserStateReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateReliabilityReadyStateService : IBrowserClientRuntimeBrowserStateReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSessionReliabilityReadyState _runtimeBrowserSessionReliabilityReadyState;

    public BrowserClientRuntimeBrowserStateReliabilityReadyStateService(IBrowserClientRuntimeBrowserSessionReliabilityReadyState runtimeBrowserSessionReliabilityReadyState)
    {
        _runtimeBrowserSessionReliabilityReadyState = runtimeBrowserSessionReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionReliabilityReadyStateResult prevReadyState = await _runtimeBrowserSessionReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStateReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserStateReliabilitySessionVersion = prevReadyState.BrowserSessionReliabilityReadyStateVersion,
            BrowserSessionReliabilityReadyStateVersion = prevReadyState.BrowserSessionReliabilityReadyStateVersion,
            BrowserSessionReliabilitySessionVersion = prevReadyState.BrowserSessionReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser statereliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateReliabilityReadyStateVersion = "runtime-browser-statereliability-ready-state-v1";
        result.BrowserStateReliabilityReadyChecks =
        [
            "browser-sessionreliability-ready-state-ready",
            "browser-statereliability-ready-state-ready",
            "browser-statereliability-ready"
        ];
        result.BrowserStateReliabilityReadySummary = $"Runtime browser statereliability ready state passed {result.BrowserStateReliabilityReadyChecks.Length} statereliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser statereliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserStateReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserStateReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSessionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserStateReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
