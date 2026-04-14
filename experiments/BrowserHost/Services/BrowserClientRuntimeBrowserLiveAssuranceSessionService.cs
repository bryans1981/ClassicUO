namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserLiveAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveAssuranceSessionService : IBrowserClientRuntimeBrowserLiveAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserProductionContinuityReadyState _runtimeBrowserProductionContinuityReadyState;

    public BrowserClientRuntimeBrowserLiveAssuranceSessionService(IBrowserClientRuntimeBrowserProductionContinuityReadyState runtimeBrowserProductionContinuityReadyState)
    {
        _runtimeBrowserProductionContinuityReadyState = runtimeBrowserProductionContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionContinuityReadyStateResult prevReadyState = await _runtimeBrowserProductionContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveAssuranceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserProductionContinuityReadyStateVersion = prevReadyState.BrowserProductionContinuityReadyStateVersion,
            BrowserProductionContinuitySessionVersion = prevReadyState.BrowserProductionContinuitySessionVersion,
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
            result.Summary = $"Runtime browser liveassurance session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveAssuranceSessionVersion = "runtime-browser-liveassurance-session-v1";
        result.BrowserLiveAssuranceStages =
        [
            "open-browser-liveassurance-session",
            "bind-browser-productioncontinuity-ready-state",
            "publish-browser-liveassurance-ready"
        ];
        result.BrowserLiveAssuranceSummary = $"Runtime browser liveassurance session prepared {result.BrowserLiveAssuranceStages.Length} liveassurance stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser liveassurance session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLiveAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveAssuranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserProductionContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
