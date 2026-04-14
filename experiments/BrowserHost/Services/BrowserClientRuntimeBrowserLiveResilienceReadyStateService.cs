namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLiveResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveResilienceReadyStateService : IBrowserClientRuntimeBrowserLiveResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserLiveResilienceSession _runtimeBrowserLiveResilienceSession;

    public BrowserClientRuntimeBrowserLiveResilienceReadyStateService(IBrowserClientRuntimeBrowserLiveResilienceSession runtimeBrowserLiveResilienceSession)
    {
        _runtimeBrowserLiveResilienceSession = runtimeBrowserLiveResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveResilienceSessionResult liveresilienceSession = await _runtimeBrowserLiveResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLiveResilienceReadyStateResult result = new()
        {
            ProfileId = liveresilienceSession.ProfileId,
            SessionId = liveresilienceSession.SessionId,
            SessionPath = liveresilienceSession.SessionPath,
            BrowserLiveResilienceSessionVersion = liveresilienceSession.BrowserLiveResilienceSessionVersion,
            BrowserSustainmentResilienceReadyStateVersion = liveresilienceSession.BrowserSustainmentResilienceReadyStateVersion,
            BrowserSustainmentResilienceSessionVersion = liveresilienceSession.BrowserSustainmentResilienceSessionVersion,
            LaunchMode = liveresilienceSession.LaunchMode,
            AssetRootPath = liveresilienceSession.AssetRootPath,
            ProfilesRootPath = liveresilienceSession.ProfilesRootPath,
            CacheRootPath = liveresilienceSession.CacheRootPath,
            ConfigRootPath = liveresilienceSession.ConfigRootPath,
            SettingsFilePath = liveresilienceSession.SettingsFilePath,
            StartupProfilePath = liveresilienceSession.StartupProfilePath,
            RequiredAssets = liveresilienceSession.RequiredAssets,
            ReadyAssetCount = liveresilienceSession.ReadyAssetCount,
            CompletedSteps = liveresilienceSession.CompletedSteps,
            TotalSteps = liveresilienceSession.TotalSteps,
            Exists = liveresilienceSession.Exists,
            ReadSucceeded = liveresilienceSession.ReadSucceeded
        };

        if (!liveresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser liveresilience ready state blocked for profile '{liveresilienceSession.ProfileId}'.";
            result.Error = liveresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveResilienceReadyStateVersion = "runtime-browser-liveresilience-ready-state-v1";
        result.BrowserLiveResilienceReadyChecks =
        [
            "browser-sustainmentresilience-ready-state-ready",
            "browser-liveresilience-session-ready",
            "browser-liveresilience-ready"
        ];
        result.BrowserLiveResilienceReadySummary = $"Runtime browser liveresilience ready state passed {result.BrowserLiveResilienceReadyChecks.Length} liveresilience readiness check(s) for profile '{liveresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser liveresilience ready state ready for profile '{liveresilienceSession.ProfileId}' with {result.BrowserLiveResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserSustainmentResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLiveResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
