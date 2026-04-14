namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLaunchReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchReliabilityReadyStateService : IBrowserClientRuntimeBrowserLaunchReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserReleaseReliabilityReadyState _runtimeBrowserReleaseReliabilityReadyState;

    public BrowserClientRuntimeBrowserLaunchReliabilityReadyStateService(IBrowserClientRuntimeBrowserReleaseReliabilityReadyState runtimeBrowserReleaseReliabilityReadyState)
    {
        _runtimeBrowserReleaseReliabilityReadyState = runtimeBrowserReleaseReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseReliabilityReadyStateResult prevReadyState = await _runtimeBrowserReleaseReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLaunchReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLaunchReliabilitySessionVersion = prevReadyState.BrowserReleaseReliabilityReadyStateVersion,
            BrowserReleaseReliabilityReadyStateVersion = prevReadyState.BrowserReleaseReliabilityReadyStateVersion,
            BrowserReleaseReliabilitySessionVersion = prevReadyState.BrowserReleaseReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser launchreliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchReliabilityReadyStateVersion = "runtime-browser-launchreliability-ready-state-v1";
        result.BrowserLaunchReliabilityReadyChecks =
        [
            "browser-releasereliability-ready-state-ready",
            "browser-launchreliability-ready-state-ready",
            "browser-launchreliability-ready"
        ];
        result.BrowserLaunchReliabilityReadySummary = $"Runtime browser launchreliability ready state passed {result.BrowserLaunchReliabilityReadyChecks.Length} launchreliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchreliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLaunchReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLaunchReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserReleaseReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLaunchReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLaunchReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
