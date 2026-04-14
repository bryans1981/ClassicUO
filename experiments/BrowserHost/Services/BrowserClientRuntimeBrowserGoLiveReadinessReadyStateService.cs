namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveReadinessReadyStateService : IBrowserClientRuntimeBrowserGoLiveReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserGoLiveReadinessSession _runtimeBrowserGoLiveReadinessSession;

    public BrowserClientRuntimeBrowserGoLiveReadinessReadyStateService(IBrowserClientRuntimeBrowserGoLiveReadinessSession runtimeBrowserGoLiveReadinessSession)
    {
        _runtimeBrowserGoLiveReadinessSession = runtimeBrowserGoLiveReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoLiveReadinessSessionResult golivereadinessSession = await _runtimeBrowserGoLiveReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveReadinessReadyStateResult result = new()
        {
            ProfileId = golivereadinessSession.ProfileId,
            SessionId = golivereadinessSession.SessionId,
            SessionPath = golivereadinessSession.SessionPath,
            BrowserGoLiveReadinessSessionVersion = golivereadinessSession.BrowserGoLiveReadinessSessionVersion,
            BrowserLaunchAssuranceReadyStateVersion = golivereadinessSession.BrowserLaunchAssuranceReadyStateVersion,
            BrowserLaunchAssuranceSessionVersion = golivereadinessSession.BrowserLaunchAssuranceSessionVersion,
            LaunchMode = golivereadinessSession.LaunchMode,
            AssetRootPath = golivereadinessSession.AssetRootPath,
            ProfilesRootPath = golivereadinessSession.ProfilesRootPath,
            CacheRootPath = golivereadinessSession.CacheRootPath,
            ConfigRootPath = golivereadinessSession.ConfigRootPath,
            SettingsFilePath = golivereadinessSession.SettingsFilePath,
            StartupProfilePath = golivereadinessSession.StartupProfilePath,
            RequiredAssets = golivereadinessSession.RequiredAssets,
            ReadyAssetCount = golivereadinessSession.ReadyAssetCount,
            CompletedSteps = golivereadinessSession.CompletedSteps,
            TotalSteps = golivereadinessSession.TotalSteps,
            Exists = golivereadinessSession.Exists,
            ReadSucceeded = golivereadinessSession.ReadSucceeded
        };

        if (!golivereadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser golivereadiness ready state blocked for profile '{golivereadinessSession.ProfileId}'.";
            result.Error = golivereadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveReadinessReadyStateVersion = "runtime-browser-golivereadiness-ready-state-v1";
        result.BrowserGoLiveReadinessReadyChecks =
        [
            "browser-launchassurance-ready-state-ready",
            "browser-golivereadiness-session-ready",
            "browser-golivereadiness-ready"
        ];
        result.BrowserGoLiveReadinessReadySummary = $"Runtime browser golivereadiness ready state passed {result.BrowserGoLiveReadinessReadyChecks.Length} golivereadiness readiness check(s) for profile '{golivereadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser golivereadiness ready state ready for profile '{golivereadinessSession.ProfileId}' with {result.BrowserGoLiveReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGoLiveReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLaunchAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoLiveReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
