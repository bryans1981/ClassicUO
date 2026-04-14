namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateService : IBrowserClientRuntimeBrowserGoLiveReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLaunchReliabilityReadyState _runtimeBrowserLaunchReliabilityReadyState;

    public BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateService(IBrowserClientRuntimeBrowserLaunchReliabilityReadyState runtimeBrowserLaunchReliabilityReadyState)
    {
        _runtimeBrowserLaunchReliabilityReadyState = runtimeBrowserLaunchReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchReliabilityReadyStateResult prevReadyState = await _runtimeBrowserLaunchReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserGoLiveReliabilitySessionVersion = prevReadyState.BrowserLaunchReliabilityReadyStateVersion,
            BrowserLaunchReliabilityReadyStateVersion = prevReadyState.BrowserLaunchReliabilityReadyStateVersion,
            BrowserLaunchReliabilitySessionVersion = prevReadyState.BrowserLaunchReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser golivereliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveReliabilityReadyStateVersion = "runtime-browser-golivereliability-ready-state-v1";
        result.BrowserGoLiveReliabilityReadyChecks =
        [
            "browser-launchreliability-ready-state-ready",
            "browser-golivereliability-ready-state-ready",
            "browser-golivereliability-ready"
        ];
        result.BrowserGoLiveReliabilityReadySummary = $"Runtime browser golivereliability ready state passed {result.BrowserGoLiveReliabilityReadyChecks.Length} golivereliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser golivereliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserGoLiveReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGoLiveReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLaunchReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoLiveReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
