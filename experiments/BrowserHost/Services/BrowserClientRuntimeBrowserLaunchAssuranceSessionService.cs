namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserLaunchAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchAssuranceSessionService : IBrowserClientRuntimeBrowserLaunchAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserReleaseReadinessReadyState _runtimeBrowserReleaseReadinessReadyState;

    public BrowserClientRuntimeBrowserLaunchAssuranceSessionService(IBrowserClientRuntimeBrowserReleaseReadinessReadyState runtimeBrowserReleaseReadinessReadyState)
    {
        _runtimeBrowserReleaseReadinessReadyState = runtimeBrowserReleaseReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseReadinessReadyStateResult releasereadinessReadyState = await _runtimeBrowserReleaseReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLaunchAssuranceSessionResult result = new()
        {
            ProfileId = releasereadinessReadyState.ProfileId,
            SessionId = releasereadinessReadyState.SessionId,
            SessionPath = releasereadinessReadyState.SessionPath,
            BrowserReleaseReadinessReadyStateVersion = releasereadinessReadyState.BrowserReleaseReadinessReadyStateVersion,
            BrowserReleaseReadinessSessionVersion = releasereadinessReadyState.BrowserReleaseReadinessSessionVersion,
            LaunchMode = releasereadinessReadyState.LaunchMode,
            AssetRootPath = releasereadinessReadyState.AssetRootPath,
            ProfilesRootPath = releasereadinessReadyState.ProfilesRootPath,
            CacheRootPath = releasereadinessReadyState.CacheRootPath,
            ConfigRootPath = releasereadinessReadyState.ConfigRootPath,
            SettingsFilePath = releasereadinessReadyState.SettingsFilePath,
            StartupProfilePath = releasereadinessReadyState.StartupProfilePath,
            RequiredAssets = releasereadinessReadyState.RequiredAssets,
            ReadyAssetCount = releasereadinessReadyState.ReadyAssetCount,
            CompletedSteps = releasereadinessReadyState.CompletedSteps,
            TotalSteps = releasereadinessReadyState.TotalSteps,
            Exists = releasereadinessReadyState.Exists,
            ReadSucceeded = releasereadinessReadyState.ReadSucceeded
        };

        if (!releasereadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser launchassurance session blocked for profile '{releasereadinessReadyState.ProfileId}'.";
            result.Error = releasereadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchAssuranceSessionVersion = "runtime-browser-launchassurance-session-v1";
        result.BrowserLaunchAssuranceStages =
        [
            "open-browser-launchassurance-session",
            "bind-browser-releasereadiness-ready-state",
            "publish-browser-launchassurance-ready"
        ];
        result.BrowserLaunchAssuranceSummary = $"Runtime browser launchassurance session prepared {result.BrowserLaunchAssuranceStages.Length} launchassurance stage(s) for profile '{releasereadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchassurance session ready for profile '{releasereadinessReadyState.ProfileId}' with {result.BrowserLaunchAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchAssuranceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserLaunchAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserLaunchAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
