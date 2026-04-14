namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeploymentResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDeploymentResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeploymentResilienceReadyStateService : IBrowserClientRuntimeBrowserDeploymentResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserDeploymentResilienceSession _runtimeBrowserDeploymentResilienceSession;

    public BrowserClientRuntimeBrowserDeploymentResilienceReadyStateService(IBrowserClientRuntimeBrowserDeploymentResilienceSession runtimeBrowserDeploymentResilienceSession)
    {
        _runtimeBrowserDeploymentResilienceSession = runtimeBrowserDeploymentResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeploymentResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentResilienceSessionResult deploymentresilienceSession = await _runtimeBrowserDeploymentResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDeploymentResilienceReadyStateResult result = new()
        {
            ProfileId = deploymentresilienceSession.ProfileId,
            SessionId = deploymentresilienceSession.SessionId,
            SessionPath = deploymentresilienceSession.SessionPath,
            BrowserDeploymentResilienceSessionVersion = deploymentresilienceSession.BrowserDeploymentResilienceSessionVersion,
            BrowserOperationalResilienceReadyStateVersion = deploymentresilienceSession.BrowserOperationalResilienceReadyStateVersion,
            BrowserOperationalResilienceSessionVersion = deploymentresilienceSession.BrowserOperationalResilienceSessionVersion,
            LaunchMode = deploymentresilienceSession.LaunchMode,
            AssetRootPath = deploymentresilienceSession.AssetRootPath,
            ProfilesRootPath = deploymentresilienceSession.ProfilesRootPath,
            CacheRootPath = deploymentresilienceSession.CacheRootPath,
            ConfigRootPath = deploymentresilienceSession.ConfigRootPath,
            SettingsFilePath = deploymentresilienceSession.SettingsFilePath,
            StartupProfilePath = deploymentresilienceSession.StartupProfilePath,
            RequiredAssets = deploymentresilienceSession.RequiredAssets,
            ReadyAssetCount = deploymentresilienceSession.ReadyAssetCount,
            CompletedSteps = deploymentresilienceSession.CompletedSteps,
            TotalSteps = deploymentresilienceSession.TotalSteps,
            Exists = deploymentresilienceSession.Exists,
            ReadSucceeded = deploymentresilienceSession.ReadSucceeded
        };

        if (!deploymentresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser deploymentresilience ready state blocked for profile '{deploymentresilienceSession.ProfileId}'.";
            result.Error = deploymentresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeploymentResilienceReadyStateVersion = "runtime-browser-deploymentresilience-ready-state-v1";
        result.BrowserDeploymentResilienceReadyChecks =
        [
            "browser-operationalresilience-ready-state-ready",
            "browser-deploymentresilience-session-ready",
            "browser-deploymentresilience-ready"
        ];
        result.BrowserDeploymentResilienceReadySummary = $"Runtime browser deploymentresilience ready state passed {result.BrowserDeploymentResilienceReadyChecks.Length} deploymentresilience readiness check(s) for profile '{deploymentresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deploymentresilience ready state ready for profile '{deploymentresilienceSession.ProfileId}' with {result.BrowserDeploymentResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeploymentResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDeploymentResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserOperationalResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeploymentResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDeploymentResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
