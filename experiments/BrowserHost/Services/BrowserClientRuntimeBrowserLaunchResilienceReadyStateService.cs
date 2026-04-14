namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLaunchResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchResilienceReadyStateService : IBrowserClientRuntimeBrowserLaunchResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserLaunchResilienceSession _runtimeBrowserLaunchResilienceSession;

    public BrowserClientRuntimeBrowserLaunchResilienceReadyStateService(IBrowserClientRuntimeBrowserLaunchResilienceSession runtimeBrowserLaunchResilienceSession)
    {
        _runtimeBrowserLaunchResilienceSession = runtimeBrowserLaunchResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchResilienceSessionResult launchresilienceSession = await _runtimeBrowserLaunchResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLaunchResilienceReadyStateResult result = new()
        {
            ProfileId = launchresilienceSession.ProfileId,
            SessionId = launchresilienceSession.SessionId,
            SessionPath = launchresilienceSession.SessionPath,
            BrowserLaunchResilienceSessionVersion = launchresilienceSession.BrowserLaunchResilienceSessionVersion,
            BrowserReleaseResilienceReadyStateVersion = launchresilienceSession.BrowserReleaseResilienceReadyStateVersion,
            BrowserReleaseResilienceSessionVersion = launchresilienceSession.BrowserReleaseResilienceSessionVersion,
            LaunchMode = launchresilienceSession.LaunchMode,
            AssetRootPath = launchresilienceSession.AssetRootPath,
            ProfilesRootPath = launchresilienceSession.ProfilesRootPath,
            CacheRootPath = launchresilienceSession.CacheRootPath,
            ConfigRootPath = launchresilienceSession.ConfigRootPath,
            SettingsFilePath = launchresilienceSession.SettingsFilePath,
            StartupProfilePath = launchresilienceSession.StartupProfilePath,
            RequiredAssets = launchresilienceSession.RequiredAssets,
            ReadyAssetCount = launchresilienceSession.ReadyAssetCount,
            CompletedSteps = launchresilienceSession.CompletedSteps,
            TotalSteps = launchresilienceSession.TotalSteps,
            Exists = launchresilienceSession.Exists,
            ReadSucceeded = launchresilienceSession.ReadSucceeded
        };

        if (!launchresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser launchresilience ready state blocked for profile '{launchresilienceSession.ProfileId}'.";
            result.Error = launchresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchResilienceReadyStateVersion = "runtime-browser-launchresilience-ready-state-v1";
        result.BrowserLaunchResilienceReadyChecks =
        [
            "browser-releaseresilience-ready-state-ready",
            "browser-launchresilience-session-ready",
            "browser-launchresilience-ready"
        ];
        result.BrowserLaunchResilienceReadySummary = $"Runtime browser launchresilience ready state passed {result.BrowserLaunchResilienceReadyChecks.Length} launchresilience readiness check(s) for profile '{launchresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchresilience ready state ready for profile '{launchresilienceSession.ProfileId}' with {result.BrowserLaunchResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLaunchResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserReleaseResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLaunchResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLaunchResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
