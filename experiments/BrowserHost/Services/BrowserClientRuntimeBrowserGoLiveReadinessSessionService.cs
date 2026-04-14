namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveReadinessSessionService : IBrowserClientRuntimeBrowserGoLiveReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserLaunchAssuranceReadyState _runtimeBrowserLaunchAssuranceReadyState;

    public BrowserClientRuntimeBrowserGoLiveReadinessSessionService(IBrowserClientRuntimeBrowserLaunchAssuranceReadyState runtimeBrowserLaunchAssuranceReadyState)
    {
        _runtimeBrowserLaunchAssuranceReadyState = runtimeBrowserLaunchAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchAssuranceReadyStateResult launchassuranceReadyState = await _runtimeBrowserLaunchAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveReadinessSessionResult result = new()
        {
            ProfileId = launchassuranceReadyState.ProfileId,
            SessionId = launchassuranceReadyState.SessionId,
            SessionPath = launchassuranceReadyState.SessionPath,
            BrowserLaunchAssuranceReadyStateVersion = launchassuranceReadyState.BrowserLaunchAssuranceReadyStateVersion,
            BrowserLaunchAssuranceSessionVersion = launchassuranceReadyState.BrowserLaunchAssuranceSessionVersion,
            LaunchMode = launchassuranceReadyState.LaunchMode,
            AssetRootPath = launchassuranceReadyState.AssetRootPath,
            ProfilesRootPath = launchassuranceReadyState.ProfilesRootPath,
            CacheRootPath = launchassuranceReadyState.CacheRootPath,
            ConfigRootPath = launchassuranceReadyState.ConfigRootPath,
            SettingsFilePath = launchassuranceReadyState.SettingsFilePath,
            StartupProfilePath = launchassuranceReadyState.StartupProfilePath,
            RequiredAssets = launchassuranceReadyState.RequiredAssets,
            ReadyAssetCount = launchassuranceReadyState.ReadyAssetCount,
            CompletedSteps = launchassuranceReadyState.CompletedSteps,
            TotalSteps = launchassuranceReadyState.TotalSteps,
            Exists = launchassuranceReadyState.Exists,
            ReadSucceeded = launchassuranceReadyState.ReadSucceeded
        };

        if (!launchassuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser golivereadiness session blocked for profile '{launchassuranceReadyState.ProfileId}'.";
            result.Error = launchassuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveReadinessSessionVersion = "runtime-browser-golivereadiness-session-v1";
        result.BrowserGoLiveReadinessStages =
        [
            "open-browser-golivereadiness-session",
            "bind-browser-launchassurance-ready-state",
            "publish-browser-golivereadiness-ready"
        ];
        result.BrowserGoLiveReadinessSummary = $"Runtime browser golivereadiness session prepared {result.BrowserGoLiveReadinessStages.Length} golivereadiness stage(s) for profile '{launchassuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser golivereadiness session ready for profile '{launchassuranceReadyState.ProfileId}' with {result.BrowserGoLiveReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveReadinessSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserGoLiveReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
