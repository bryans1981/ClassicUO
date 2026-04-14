namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeploymentReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserDeploymentReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeploymentReadinessSessionService : IBrowserClientRuntimeBrowserDeploymentReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserOperationalReadinessReadyState _runtimeBrowserOperationalReadinessReadyState;

    public BrowserClientRuntimeBrowserDeploymentReadinessSessionService(IBrowserClientRuntimeBrowserOperationalReadinessReadyState runtimeBrowserOperationalReadinessReadyState)
    {
        _runtimeBrowserOperationalReadinessReadyState = runtimeBrowserOperationalReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeploymentReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperationalReadinessReadyStateResult operationalreadinessReadyState = await _runtimeBrowserOperationalReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDeploymentReadinessSessionResult result = new()
        {
            ProfileId = operationalreadinessReadyState.ProfileId,
            SessionId = operationalreadinessReadyState.SessionId,
            SessionPath = operationalreadinessReadyState.SessionPath,
            BrowserOperationalReadinessReadyStateVersion = operationalreadinessReadyState.BrowserOperationalReadinessReadyStateVersion,
            BrowserOperationalReadinessSessionVersion = operationalreadinessReadyState.BrowserOperationalReadinessSessionVersion,
            LaunchMode = operationalreadinessReadyState.LaunchMode,
            AssetRootPath = operationalreadinessReadyState.AssetRootPath,
            ProfilesRootPath = operationalreadinessReadyState.ProfilesRootPath,
            CacheRootPath = operationalreadinessReadyState.CacheRootPath,
            ConfigRootPath = operationalreadinessReadyState.ConfigRootPath,
            SettingsFilePath = operationalreadinessReadyState.SettingsFilePath,
            StartupProfilePath = operationalreadinessReadyState.StartupProfilePath,
            RequiredAssets = operationalreadinessReadyState.RequiredAssets,
            ReadyAssetCount = operationalreadinessReadyState.ReadyAssetCount,
            CompletedSteps = operationalreadinessReadyState.CompletedSteps,
            TotalSteps = operationalreadinessReadyState.TotalSteps,
            Exists = operationalreadinessReadyState.Exists,
            ReadSucceeded = operationalreadinessReadyState.ReadSucceeded
        };

        if (!operationalreadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser deploymentreadiness session blocked for profile '{operationalreadinessReadyState.ProfileId}'.";
            result.Error = operationalreadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeploymentReadinessSessionVersion = "runtime-browser-deploymentreadiness-session-v1";
        result.BrowserDeploymentReadinessStages =
        [
            "open-browser-deploymentreadiness-session",
            "bind-browser-operationalreadiness-ready-state",
            "publish-browser-deploymentreadiness-ready"
        ];
        result.BrowserDeploymentReadinessSummary = $"Runtime browser deploymentreadiness session prepared {result.BrowserDeploymentReadinessStages.Length} deploymentreadiness stage(s) for profile '{operationalreadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deploymentreadiness session ready for profile '{operationalreadinessReadyState.ProfileId}' with {result.BrowserDeploymentReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeploymentReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDeploymentReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserOperationalReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeploymentReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserDeploymentReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
