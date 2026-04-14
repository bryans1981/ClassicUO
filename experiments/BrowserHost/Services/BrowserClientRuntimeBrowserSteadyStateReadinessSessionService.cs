namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyStateReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserSteadyStateReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyStateReadinessSessionService : IBrowserClientRuntimeBrowserSteadyStateReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserLiveStabilityReadyState _runtimeBrowserLiveStabilityReadyState;

    public BrowserClientRuntimeBrowserSteadyStateReadinessSessionService(IBrowserClientRuntimeBrowserLiveStabilityReadyState runtimeBrowserLiveStabilityReadyState)
    {
        _runtimeBrowserLiveStabilityReadyState = runtimeBrowserLiveStabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyStateReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveStabilityReadyStateResult livestabilityReadyState = await _runtimeBrowserLiveStabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadyStateReadinessSessionResult result = new()
        {
            ProfileId = livestabilityReadyState.ProfileId,
            SessionId = livestabilityReadyState.SessionId,
            SessionPath = livestabilityReadyState.SessionPath,
            BrowserLiveStabilityReadyStateVersion = livestabilityReadyState.BrowserLiveStabilityReadyStateVersion,
            BrowserLiveStabilitySessionVersion = livestabilityReadyState.BrowserLiveStabilitySessionVersion,
            LaunchMode = livestabilityReadyState.LaunchMode,
            AssetRootPath = livestabilityReadyState.AssetRootPath,
            ProfilesRootPath = livestabilityReadyState.ProfilesRootPath,
            CacheRootPath = livestabilityReadyState.CacheRootPath,
            ConfigRootPath = livestabilityReadyState.ConfigRootPath,
            SettingsFilePath = livestabilityReadyState.SettingsFilePath,
            StartupProfilePath = livestabilityReadyState.StartupProfilePath,
            RequiredAssets = livestabilityReadyState.RequiredAssets,
            ReadyAssetCount = livestabilityReadyState.ReadyAssetCount,
            CompletedSteps = livestabilityReadyState.CompletedSteps,
            TotalSteps = livestabilityReadyState.TotalSteps,
            Exists = livestabilityReadyState.Exists,
            ReadSucceeded = livestabilityReadyState.ReadSucceeded
        };

        if (!livestabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadystatereadiness session blocked for profile '{livestabilityReadyState.ProfileId}'.";
            result.Error = livestabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyStateReadinessSessionVersion = "runtime-browser-steadystatereadiness-session-v1";
        result.BrowserSteadyStateReadinessStages =
        [
            "open-browser-steadystatereadiness-session",
            "bind-browser-livestability-ready-state",
            "publish-browser-steadystatereadiness-ready"
        ];
        result.BrowserSteadyStateReadinessSummary = $"Runtime browser steadystatereadiness session prepared {result.BrowserSteadyStateReadinessStages.Length} steadystatereadiness stage(s) for profile '{livestabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadystatereadiness session ready for profile '{livestabilityReadyState.ProfileId}' with {result.BrowserSteadyStateReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyStateReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyStateReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLiveStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyStateReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserSteadyStateReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
