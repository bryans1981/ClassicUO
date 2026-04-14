namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReleaseResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseResilienceReadyStateService : IBrowserClientRuntimeBrowserReleaseResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserReleaseResilienceSession _runtimeBrowserReleaseResilienceSession;

    public BrowserClientRuntimeBrowserReleaseResilienceReadyStateService(IBrowserClientRuntimeBrowserReleaseResilienceSession runtimeBrowserReleaseResilienceSession)
    {
        _runtimeBrowserReleaseResilienceSession = runtimeBrowserReleaseResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseResilienceSessionResult releaseresilienceSession = await _runtimeBrowserReleaseResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserReleaseResilienceReadyStateResult result = new()
        {
            ProfileId = releaseresilienceSession.ProfileId,
            SessionId = releaseresilienceSession.SessionId,
            SessionPath = releaseresilienceSession.SessionPath,
            BrowserReleaseResilienceSessionVersion = releaseresilienceSession.BrowserReleaseResilienceSessionVersion,
            BrowserDeploymentResilienceReadyStateVersion = releaseresilienceSession.BrowserDeploymentResilienceReadyStateVersion,
            BrowserDeploymentResilienceSessionVersion = releaseresilienceSession.BrowserDeploymentResilienceSessionVersion,
            LaunchMode = releaseresilienceSession.LaunchMode,
            AssetRootPath = releaseresilienceSession.AssetRootPath,
            ProfilesRootPath = releaseresilienceSession.ProfilesRootPath,
            CacheRootPath = releaseresilienceSession.CacheRootPath,
            ConfigRootPath = releaseresilienceSession.ConfigRootPath,
            SettingsFilePath = releaseresilienceSession.SettingsFilePath,
            StartupProfilePath = releaseresilienceSession.StartupProfilePath,
            RequiredAssets = releaseresilienceSession.RequiredAssets,
            ReadyAssetCount = releaseresilienceSession.ReadyAssetCount,
            CompletedSteps = releaseresilienceSession.CompletedSteps,
            TotalSteps = releaseresilienceSession.TotalSteps,
            Exists = releaseresilienceSession.Exists,
            ReadSucceeded = releaseresilienceSession.ReadSucceeded
        };

        if (!releaseresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser releaseresilience ready state blocked for profile '{releaseresilienceSession.ProfileId}'.";
            result.Error = releaseresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseResilienceReadyStateVersion = "runtime-browser-releaseresilience-ready-state-v1";
        result.BrowserReleaseResilienceReadyChecks =
        [
            "browser-deploymentresilience-ready-state-ready",
            "browser-releaseresilience-session-ready",
            "browser-releaseresilience-ready"
        ];
        result.BrowserReleaseResilienceReadySummary = $"Runtime browser releaseresilience ready state passed {result.BrowserReleaseResilienceReadyChecks.Length} releaseresilience readiness check(s) for profile '{releaseresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releaseresilience ready state ready for profile '{releaseresilienceSession.ProfileId}' with {result.BrowserReleaseResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserDeploymentResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReleaseResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
