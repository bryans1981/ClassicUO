namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyOperationReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserSteadyOperationReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationReadinessSessionService : IBrowserClientRuntimeBrowserSteadyOperationReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserLiveAssuranceReadyState _runtimeBrowserLiveAssuranceReadyState;

    public BrowserClientRuntimeBrowserSteadyOperationReadinessSessionService(IBrowserClientRuntimeBrowserLiveAssuranceReadyState runtimeBrowserLiveAssuranceReadyState)
    {
        _runtimeBrowserLiveAssuranceReadyState = runtimeBrowserLiveAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyOperationReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveAssuranceReadyStateResult prevReadyState = await _runtimeBrowserLiveAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadyOperationReadinessSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLiveAssuranceReadyStateVersion = prevReadyState.BrowserLiveAssuranceReadyStateVersion,
            BrowserLiveAssuranceSessionVersion = prevReadyState.BrowserLiveAssuranceSessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadyoperationreadiness session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyOperationReadinessSessionVersion = "runtime-browser-steadyoperationreadiness-session-v1";
        result.BrowserSteadyOperationReadinessStages =
        [
            "open-browser-steadyoperationreadiness-session",
            "bind-browser-liveassurance-ready-state",
            "publish-browser-steadyoperationreadiness-ready"
        ];
        result.BrowserSteadyOperationReadinessSummary = $"Runtime browser steadyoperationreadiness session prepared {result.BrowserSteadyOperationReadinessStages.Length} steadyoperationreadiness stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadyoperationreadiness session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSteadyOperationReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyOperationReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLiveAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyOperationReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserSteadyOperationReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
