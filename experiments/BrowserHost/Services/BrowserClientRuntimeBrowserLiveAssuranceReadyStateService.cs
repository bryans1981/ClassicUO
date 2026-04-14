namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLiveAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveAssuranceReadyStateService : IBrowserClientRuntimeBrowserLiveAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserLiveAssuranceSession _runtimeBrowserLiveAssuranceSession;

    public BrowserClientRuntimeBrowserLiveAssuranceReadyStateService(IBrowserClientRuntimeBrowserLiveAssuranceSession runtimeBrowserLiveAssuranceSession)
    {
        _runtimeBrowserLiveAssuranceSession = runtimeBrowserLiveAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveAssuranceSessionResult liveassuranceSession = await _runtimeBrowserLiveAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLiveAssuranceReadyStateResult result = new()
        {
            ProfileId = liveassuranceSession.ProfileId,
            SessionId = liveassuranceSession.SessionId,
            SessionPath = liveassuranceSession.SessionPath,
            BrowserLiveAssuranceSessionVersion = liveassuranceSession.BrowserLiveAssuranceSessionVersion,
            BrowserProductionContinuityReadyStateVersion = liveassuranceSession.BrowserProductionContinuityReadyStateVersion,
            BrowserProductionContinuitySessionVersion = liveassuranceSession.BrowserProductionContinuitySessionVersion,
            LaunchMode = liveassuranceSession.LaunchMode,
            AssetRootPath = liveassuranceSession.AssetRootPath,
            ProfilesRootPath = liveassuranceSession.ProfilesRootPath,
            CacheRootPath = liveassuranceSession.CacheRootPath,
            ConfigRootPath = liveassuranceSession.ConfigRootPath,
            SettingsFilePath = liveassuranceSession.SettingsFilePath,
            StartupProfilePath = liveassuranceSession.StartupProfilePath,
            RequiredAssets = liveassuranceSession.RequiredAssets,
            ReadyAssetCount = liveassuranceSession.ReadyAssetCount,
            CompletedSteps = liveassuranceSession.CompletedSteps,
            TotalSteps = liveassuranceSession.TotalSteps,
            Exists = liveassuranceSession.Exists,
            ReadSucceeded = liveassuranceSession.ReadSucceeded
        };

        if (!liveassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser liveassurance ready state blocked for profile '{liveassuranceSession.ProfileId}'.";
            result.Error = liveassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveAssuranceReadyStateVersion = "runtime-browser-liveassurance-ready-state-v1";
        result.BrowserLiveAssuranceReadyChecks =
        [
            "browser-productioncontinuity-ready-state-ready",
            "browser-liveassurance-session-ready",
            "browser-liveassurance-ready"
        ];
        result.BrowserLiveAssuranceReadySummary = $"Runtime browser liveassurance ready state passed {result.BrowserLiveAssuranceReadyChecks.Length} liveassurance readiness check(s) for profile '{liveassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser liveassurance ready state ready for profile '{liveassuranceSession.ProfileId}' with {result.BrowserLiveAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveAssuranceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLiveAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
