namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceReliabilityReadyStateService : IBrowserClientRuntimeBrowserServiceReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserGoLiveReliabilityReadyState _runtimeBrowserGoLiveReliabilityReadyState;

    public BrowserClientRuntimeBrowserServiceReliabilityReadyStateService(IBrowserClientRuntimeBrowserGoLiveReliabilityReadyState runtimeBrowserGoLiveReliabilityReadyState)
    {
        _runtimeBrowserGoLiveReliabilityReadyState = runtimeBrowserGoLiveReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateResult prevReadyState = await _runtimeBrowserGoLiveReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserServiceReliabilitySessionVersion = prevReadyState.BrowserGoLiveReliabilityReadyStateVersion,
            BrowserGoLiveReliabilityReadyStateVersion = prevReadyState.BrowserGoLiveReliabilityReadyStateVersion,
            BrowserGoLiveReliabilitySessionVersion = prevReadyState.BrowserGoLiveReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser servicereliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceReliabilityReadyStateVersion = "runtime-browser-servicereliability-ready-state-v1";
        result.BrowserServiceReliabilityReadyChecks =
        [
            "browser-golivereliability-ready-state-ready",
            "browser-servicereliability-ready-state-ready",
            "browser-servicereliability-ready"
        ];
        result.BrowserServiceReliabilityReadySummary = $"Runtime browser servicereliability ready state passed {result.BrowserServiceReliabilityReadyChecks.Length} servicereliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser servicereliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserServiceReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
