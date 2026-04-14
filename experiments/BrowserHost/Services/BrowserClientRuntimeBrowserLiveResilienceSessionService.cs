namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserLiveResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveResilienceSessionService : IBrowserClientRuntimeBrowserLiveResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserSustainmentResilienceReadyState _runtimeBrowserSustainmentResilienceReadyState;

    public BrowserClientRuntimeBrowserLiveResilienceSessionService(IBrowserClientRuntimeBrowserSustainmentResilienceReadyState runtimeBrowserSustainmentResilienceReadyState)
    {
        _runtimeBrowserSustainmentResilienceReadyState = runtimeBrowserSustainmentResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentResilienceReadyStateResult prevReadyState = await _runtimeBrowserSustainmentResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSustainmentResilienceReadyStateVersion = prevReadyState.BrowserSustainmentResilienceReadyStateVersion,
            BrowserSustainmentResilienceSessionVersion = prevReadyState.BrowserSustainmentResilienceSessionVersion,
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
            result.Summary = $"Runtime browser liveresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveResilienceSessionVersion = "runtime-browser-liveresilience-session-v1";
        result.BrowserLiveResilienceStages =
        [
            "open-browser-liveresilience-session",
            "bind-browser-sustainmentresilience-ready-state",
            "publish-browser-liveresilience-ready"
        ];
        result.BrowserLiveResilienceSummary = $"Runtime browser liveresilience session prepared {result.BrowserLiveResilienceStages.Length} liveresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser liveresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLiveResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveResilienceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserLiveResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
