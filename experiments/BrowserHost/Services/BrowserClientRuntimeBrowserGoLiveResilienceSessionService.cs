namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveResilienceSessionService : IBrowserClientRuntimeBrowserGoLiveResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserLaunchResilienceReadyState _runtimeBrowserLaunchResilienceReadyState;

    public BrowserClientRuntimeBrowserGoLiveResilienceSessionService(IBrowserClientRuntimeBrowserLaunchResilienceReadyState runtimeBrowserLaunchResilienceReadyState)
    {
        _runtimeBrowserLaunchResilienceReadyState = runtimeBrowserLaunchResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchResilienceReadyStateResult prevReadyState = await _runtimeBrowserLaunchResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLaunchResilienceReadyStateVersion = prevReadyState.BrowserLaunchResilienceReadyStateVersion,
            BrowserLaunchResilienceSessionVersion = prevReadyState.BrowserLaunchResilienceSessionVersion,
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
            result.Summary = $"Runtime browser goliveresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveResilienceSessionVersion = "runtime-browser-goliveresilience-session-v1";
        result.BrowserGoLiveResilienceStages =
        [
            "open-browser-goliveresilience-session",
            "bind-browser-launchresilience-ready-state",
            "publish-browser-goliveresilience-ready"
        ];
        result.BrowserGoLiveResilienceSummary = $"Runtime browser goliveresilience session prepared {result.BrowserGoLiveResilienceStages.Length} goliveresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser goliveresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserGoLiveResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveResilienceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserGoLiveResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
