namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveResilienceReadyStateService : IBrowserClientRuntimeBrowserGoLiveResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserGoLiveResilienceSession _runtimeBrowserGoLiveResilienceSession;

    public BrowserClientRuntimeBrowserGoLiveResilienceReadyStateService(IBrowserClientRuntimeBrowserGoLiveResilienceSession runtimeBrowserGoLiveResilienceSession)
    {
        _runtimeBrowserGoLiveResilienceSession = runtimeBrowserGoLiveResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoLiveResilienceSessionResult goliveresilienceSession = await _runtimeBrowserGoLiveResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveResilienceReadyStateResult result = new()
        {
            ProfileId = goliveresilienceSession.ProfileId,
            SessionId = goliveresilienceSession.SessionId,
            SessionPath = goliveresilienceSession.SessionPath,
            BrowserGoLiveResilienceSessionVersion = goliveresilienceSession.BrowserGoLiveResilienceSessionVersion,
            BrowserLaunchResilienceReadyStateVersion = goliveresilienceSession.BrowserLaunchResilienceReadyStateVersion,
            BrowserLaunchResilienceSessionVersion = goliveresilienceSession.BrowserLaunchResilienceSessionVersion,
            LaunchMode = goliveresilienceSession.LaunchMode,
            AssetRootPath = goliveresilienceSession.AssetRootPath,
            ProfilesRootPath = goliveresilienceSession.ProfilesRootPath,
            CacheRootPath = goliveresilienceSession.CacheRootPath,
            ConfigRootPath = goliveresilienceSession.ConfigRootPath,
            SettingsFilePath = goliveresilienceSession.SettingsFilePath,
            StartupProfilePath = goliveresilienceSession.StartupProfilePath,
            RequiredAssets = goliveresilienceSession.RequiredAssets,
            ReadyAssetCount = goliveresilienceSession.ReadyAssetCount,
            CompletedSteps = goliveresilienceSession.CompletedSteps,
            TotalSteps = goliveresilienceSession.TotalSteps,
            Exists = goliveresilienceSession.Exists,
            ReadSucceeded = goliveresilienceSession.ReadSucceeded
        };

        if (!goliveresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser goliveresilience ready state blocked for profile '{goliveresilienceSession.ProfileId}'.";
            result.Error = goliveresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveResilienceReadyStateVersion = "runtime-browser-goliveresilience-ready-state-v1";
        result.BrowserGoLiveResilienceReadyChecks =
        [
            "browser-launchresilience-ready-state-ready",
            "browser-goliveresilience-session-ready",
            "browser-goliveresilience-ready"
        ];
        result.BrowserGoLiveResilienceReadySummary = $"Runtime browser goliveresilience ready state passed {result.BrowserGoLiveResilienceReadyChecks.Length} goliveresilience readiness check(s) for profile '{goliveresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser goliveresilience ready state ready for profile '{goliveresilienceSession.ProfileId}' with {result.BrowserGoLiveResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGoLiveResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserLaunchResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoLiveResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
