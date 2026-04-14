namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReleaseReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseReadinessReadyStateService : IBrowserClientRuntimeBrowserReleaseReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserReleaseReadinessSession _runtimeBrowserReleaseReadinessSession;

    public BrowserClientRuntimeBrowserReleaseReadinessReadyStateService(IBrowserClientRuntimeBrowserReleaseReadinessSession runtimeBrowserReleaseReadinessSession)
    {
        _runtimeBrowserReleaseReadinessSession = runtimeBrowserReleaseReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseReadinessSessionResult releasereadinessSession = await _runtimeBrowserReleaseReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserReleaseReadinessReadyStateResult result = new()
        {
            ProfileId = releasereadinessSession.ProfileId,
            SessionId = releasereadinessSession.SessionId,
            SessionPath = releasereadinessSession.SessionPath,
            BrowserReleaseReadinessSessionVersion = releasereadinessSession.BrowserReleaseReadinessSessionVersion,
            BrowserDeploymentReadinessReadyStateVersion = releasereadinessSession.BrowserDeploymentReadinessReadyStateVersion,
            BrowserDeploymentReadinessSessionVersion = releasereadinessSession.BrowserDeploymentReadinessSessionVersion,
            LaunchMode = releasereadinessSession.LaunchMode,
            AssetRootPath = releasereadinessSession.AssetRootPath,
            ProfilesRootPath = releasereadinessSession.ProfilesRootPath,
            CacheRootPath = releasereadinessSession.CacheRootPath,
            ConfigRootPath = releasereadinessSession.ConfigRootPath,
            SettingsFilePath = releasereadinessSession.SettingsFilePath,
            StartupProfilePath = releasereadinessSession.StartupProfilePath,
            RequiredAssets = releasereadinessSession.RequiredAssets,
            ReadyAssetCount = releasereadinessSession.ReadyAssetCount,
            CompletedSteps = releasereadinessSession.CompletedSteps,
            TotalSteps = releasereadinessSession.TotalSteps,
            Exists = releasereadinessSession.Exists,
            ReadSucceeded = releasereadinessSession.ReadSucceeded
        };

        if (!releasereadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser releasereadiness ready state blocked for profile '{releasereadinessSession.ProfileId}'.";
            result.Error = releasereadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseReadinessReadyStateVersion = "runtime-browser-releasereadiness-ready-state-v1";
        result.BrowserReleaseReadinessReadyChecks =
        [
            "browser-deploymentreadiness-ready-state-ready",
            "browser-releasereadiness-session-ready",
            "browser-releasereadiness-ready"
        ];
        result.BrowserReleaseReadinessReadySummary = $"Runtime browser releasereadiness ready state passed {result.BrowserReleaseReadinessReadyChecks.Length} releasereadiness readiness check(s) for profile '{releasereadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releasereadiness ready state ready for profile '{releasereadinessSession.ProfileId}' with {result.BrowserReleaseReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseReadinessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReleaseReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
