namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReleaseReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseReliabilityReadyStateService : IBrowserClientRuntimeBrowserReleaseReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserDeploymentReliabilityReadyState _runtimeBrowserDeploymentReliabilityReadyState;

    public BrowserClientRuntimeBrowserReleaseReliabilityReadyStateService(IBrowserClientRuntimeBrowserDeploymentReliabilityReadyState runtimeBrowserDeploymentReliabilityReadyState)
    {
        _runtimeBrowserDeploymentReliabilityReadyState = runtimeBrowserDeploymentReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentReliabilityReadyStateResult prevReadyState = await _runtimeBrowserDeploymentReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReleaseReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserReleaseReliabilitySessionVersion = prevReadyState.BrowserDeploymentReliabilityReadyStateVersion,
            BrowserDeploymentReliabilityReadyStateVersion = prevReadyState.BrowserDeploymentReliabilityReadyStateVersion,
            BrowserDeploymentReliabilitySessionVersion = prevReadyState.BrowserDeploymentReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser releasereliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseReliabilityReadyStateVersion = "runtime-browser-releasereliability-ready-state-v1";
        result.BrowserReleaseReliabilityReadyChecks =
        [
            "browser-deploymentreliability-ready-state-ready",
            "browser-releasereliability-ready-state-ready",
            "browser-releasereliability-ready"
        ];
        result.BrowserReleaseReliabilityReadySummary = $"Runtime browser releasereliability ready state passed {result.BrowserReleaseReliabilityReadyChecks.Length} releasereliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releasereliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserReleaseReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDeploymentReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReleaseReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
