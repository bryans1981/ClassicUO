namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserReleaseReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseReadinessSessionService : IBrowserClientRuntimeBrowserReleaseReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserDeploymentReadinessReadyState _runtimeBrowserDeploymentReadinessReadyState;

    public BrowserClientRuntimeBrowserReleaseReadinessSessionService(IBrowserClientRuntimeBrowserDeploymentReadinessReadyState runtimeBrowserDeploymentReadinessReadyState)
    {
        _runtimeBrowserDeploymentReadinessReadyState = runtimeBrowserDeploymentReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentReadinessReadyStateResult deploymentreadinessReadyState = await _runtimeBrowserDeploymentReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReleaseReadinessSessionResult result = new()
        {
            ProfileId = deploymentreadinessReadyState.ProfileId,
            SessionId = deploymentreadinessReadyState.SessionId,
            SessionPath = deploymentreadinessReadyState.SessionPath,
            BrowserDeploymentReadinessReadyStateVersion = deploymentreadinessReadyState.BrowserDeploymentReadinessReadyStateVersion,
            BrowserDeploymentReadinessSessionVersion = deploymentreadinessReadyState.BrowserDeploymentReadinessSessionVersion,
            LaunchMode = deploymentreadinessReadyState.LaunchMode,
            AssetRootPath = deploymentreadinessReadyState.AssetRootPath,
            ProfilesRootPath = deploymentreadinessReadyState.ProfilesRootPath,
            CacheRootPath = deploymentreadinessReadyState.CacheRootPath,
            ConfigRootPath = deploymentreadinessReadyState.ConfigRootPath,
            SettingsFilePath = deploymentreadinessReadyState.SettingsFilePath,
            StartupProfilePath = deploymentreadinessReadyState.StartupProfilePath,
            RequiredAssets = deploymentreadinessReadyState.RequiredAssets,
            ReadyAssetCount = deploymentreadinessReadyState.ReadyAssetCount,
            CompletedSteps = deploymentreadinessReadyState.CompletedSteps,
            TotalSteps = deploymentreadinessReadyState.TotalSteps,
            Exists = deploymentreadinessReadyState.Exists,
            ReadSucceeded = deploymentreadinessReadyState.ReadSucceeded
        };

        if (!deploymentreadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser releasereadiness session blocked for profile '{deploymentreadinessReadyState.ProfileId}'.";
            result.Error = deploymentreadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseReadinessSessionVersion = "runtime-browser-releasereadiness-session-v1";
        result.BrowserReleaseReadinessStages =
        [
            "open-browser-releasereadiness-session",
            "bind-browser-deploymentreadiness-ready-state",
            "publish-browser-releasereadiness-ready"
        ];
        result.BrowserReleaseReadinessSummary = $"Runtime browser releasereadiness session prepared {result.BrowserReleaseReadinessStages.Length} releasereadiness stage(s) for profile '{deploymentreadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releasereadiness session ready for profile '{deploymentreadinessReadyState.ProfileId}' with {result.BrowserReleaseReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserDeploymentReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserReleaseReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
