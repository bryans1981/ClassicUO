namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeploymentReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDeploymentReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeploymentReadinessReadyStateService : IBrowserClientRuntimeBrowserDeploymentReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserDeploymentReadinessSession _runtimeBrowserDeploymentReadinessSession;

    public BrowserClientRuntimeBrowserDeploymentReadinessReadyStateService(IBrowserClientRuntimeBrowserDeploymentReadinessSession runtimeBrowserDeploymentReadinessSession)
    {
        _runtimeBrowserDeploymentReadinessSession = runtimeBrowserDeploymentReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeploymentReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentReadinessSessionResult deploymentreadinessSession = await _runtimeBrowserDeploymentReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDeploymentReadinessReadyStateResult result = new()
        {
            ProfileId = deploymentreadinessSession.ProfileId,
            SessionId = deploymentreadinessSession.SessionId,
            SessionPath = deploymentreadinessSession.SessionPath,
            BrowserDeploymentReadinessSessionVersion = deploymentreadinessSession.BrowserDeploymentReadinessSessionVersion,
            BrowserOperationalReadinessReadyStateVersion = deploymentreadinessSession.BrowserOperationalReadinessReadyStateVersion,
            BrowserOperationalReadinessSessionVersion = deploymentreadinessSession.BrowserOperationalReadinessSessionVersion,
            LaunchMode = deploymentreadinessSession.LaunchMode,
            AssetRootPath = deploymentreadinessSession.AssetRootPath,
            ProfilesRootPath = deploymentreadinessSession.ProfilesRootPath,
            CacheRootPath = deploymentreadinessSession.CacheRootPath,
            ConfigRootPath = deploymentreadinessSession.ConfigRootPath,
            SettingsFilePath = deploymentreadinessSession.SettingsFilePath,
            StartupProfilePath = deploymentreadinessSession.StartupProfilePath,
            RequiredAssets = deploymentreadinessSession.RequiredAssets,
            ReadyAssetCount = deploymentreadinessSession.ReadyAssetCount,
            CompletedSteps = deploymentreadinessSession.CompletedSteps,
            TotalSteps = deploymentreadinessSession.TotalSteps,
            Exists = deploymentreadinessSession.Exists,
            ReadSucceeded = deploymentreadinessSession.ReadSucceeded
        };

        if (!deploymentreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser deploymentreadiness ready state blocked for profile '{deploymentreadinessSession.ProfileId}'.";
            result.Error = deploymentreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeploymentReadinessReadyStateVersion = "runtime-browser-deploymentreadiness-ready-state-v1";
        result.BrowserDeploymentReadinessReadyChecks =
        [
            "browser-operationalreadiness-ready-state-ready",
            "browser-deploymentreadiness-session-ready",
            "browser-deploymentreadiness-ready"
        ];
        result.BrowserDeploymentReadinessReadySummary = $"Runtime browser deploymentreadiness ready state passed {result.BrowserDeploymentReadinessReadyChecks.Length} deploymentreadiness readiness check(s) for profile '{deploymentreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deploymentreadiness ready state ready for profile '{deploymentreadinessSession.ProfileId}' with {result.BrowserDeploymentReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeploymentReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDeploymentReadinessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeploymentReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDeploymentReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
