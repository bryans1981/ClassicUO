namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLaunchAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchAssuranceReadyStateService : IBrowserClientRuntimeBrowserLaunchAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserLaunchAssuranceSession _runtimeBrowserLaunchAssuranceSession;

    public BrowserClientRuntimeBrowserLaunchAssuranceReadyStateService(IBrowserClientRuntimeBrowserLaunchAssuranceSession runtimeBrowserLaunchAssuranceSession)
    {
        _runtimeBrowserLaunchAssuranceSession = runtimeBrowserLaunchAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchAssuranceSessionResult launchassuranceSession = await _runtimeBrowserLaunchAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLaunchAssuranceReadyStateResult result = new()
        {
            ProfileId = launchassuranceSession.ProfileId,
            SessionId = launchassuranceSession.SessionId,
            SessionPath = launchassuranceSession.SessionPath,
            BrowserLaunchAssuranceSessionVersion = launchassuranceSession.BrowserLaunchAssuranceSessionVersion,
            BrowserReleaseReadinessReadyStateVersion = launchassuranceSession.BrowserReleaseReadinessReadyStateVersion,
            BrowserReleaseReadinessSessionVersion = launchassuranceSession.BrowserReleaseReadinessSessionVersion,
            LaunchMode = launchassuranceSession.LaunchMode,
            AssetRootPath = launchassuranceSession.AssetRootPath,
            ProfilesRootPath = launchassuranceSession.ProfilesRootPath,
            CacheRootPath = launchassuranceSession.CacheRootPath,
            ConfigRootPath = launchassuranceSession.ConfigRootPath,
            SettingsFilePath = launchassuranceSession.SettingsFilePath,
            StartupProfilePath = launchassuranceSession.StartupProfilePath,
            RequiredAssets = launchassuranceSession.RequiredAssets,
            ReadyAssetCount = launchassuranceSession.ReadyAssetCount,
            CompletedSteps = launchassuranceSession.CompletedSteps,
            TotalSteps = launchassuranceSession.TotalSteps,
            Exists = launchassuranceSession.Exists,
            ReadSucceeded = launchassuranceSession.ReadSucceeded
        };

        if (!launchassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser launchassurance ready state blocked for profile '{launchassuranceSession.ProfileId}'.";
            result.Error = launchassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchAssuranceReadyStateVersion = "runtime-browser-launchassurance-ready-state-v1";
        result.BrowserLaunchAssuranceReadyChecks =
        [
            "browser-releasereadiness-ready-state-ready",
            "browser-launchassurance-session-ready",
            "browser-launchassurance-ready"
        ];
        result.BrowserLaunchAssuranceReadySummary = $"Runtime browser launchassurance ready state passed {result.BrowserLaunchAssuranceReadyChecks.Length} launchassurance readiness check(s) for profile '{launchassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchassurance ready state ready for profile '{launchassuranceSession.ProfileId}' with {result.BrowserLaunchAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLaunchAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserReleaseReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLaunchAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLaunchAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
