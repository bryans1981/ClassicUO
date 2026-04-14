namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserLaunchResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchResilienceSessionService : IBrowserClientRuntimeBrowserLaunchResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserReleaseResilienceReadyState _runtimeBrowserReleaseResilienceReadyState;

    public BrowserClientRuntimeBrowserLaunchResilienceSessionService(IBrowserClientRuntimeBrowserReleaseResilienceReadyState runtimeBrowserReleaseResilienceReadyState)
    {
        _runtimeBrowserReleaseResilienceReadyState = runtimeBrowserReleaseResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseResilienceReadyStateResult prevReadyState = await _runtimeBrowserReleaseResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLaunchResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserReleaseResilienceReadyStateVersion = prevReadyState.BrowserReleaseResilienceReadyStateVersion,
            BrowserReleaseResilienceSessionVersion = prevReadyState.BrowserReleaseResilienceSessionVersion,
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
            result.Summary = $"Runtime browser launchresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchResilienceSessionVersion = "runtime-browser-launchresilience-session-v1";
        result.BrowserLaunchResilienceStages =
        [
            "open-browser-launchresilience-session",
            "bind-browser-releaseresilience-ready-state",
            "publish-browser-launchresilience-ready"
        ];
        result.BrowserLaunchResilienceSummary = $"Runtime browser launchresilience session prepared {result.BrowserLaunchResilienceStages.Length} launchresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLaunchResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchResilienceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserLaunchResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserLaunchResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
