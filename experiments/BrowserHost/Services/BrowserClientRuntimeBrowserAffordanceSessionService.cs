namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAffordanceSession
{
    ValueTask<BrowserClientRuntimeBrowserAffordanceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAffordanceSessionService : IBrowserClientRuntimeBrowserAffordanceSession
{
    private readonly IBrowserClientRuntimeBrowserImmersionReadyState _runtimeBrowserImmersionReadyState;

    public BrowserClientRuntimeBrowserAffordanceSessionService(IBrowserClientRuntimeBrowserImmersionReadyState runtimeBrowserImmersionReadyState)
    {
        _runtimeBrowserImmersionReadyState = runtimeBrowserImmersionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAffordanceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserImmersionReadyStateResult immersionReadyState = await _runtimeBrowserImmersionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAffordanceSessionResult result = new()
        {
            ProfileId = immersionReadyState.ProfileId,
            SessionId = immersionReadyState.SessionId,
            SessionPath = immersionReadyState.SessionPath,
            BrowserImmersionReadyStateVersion = immersionReadyState.BrowserImmersionReadyStateVersion,
            BrowserImmersionSessionVersion = immersionReadyState.BrowserImmersionSessionVersion,
            LaunchMode = immersionReadyState.LaunchMode,
            AssetRootPath = immersionReadyState.AssetRootPath,
            ProfilesRootPath = immersionReadyState.ProfilesRootPath,
            CacheRootPath = immersionReadyState.CacheRootPath,
            ConfigRootPath = immersionReadyState.ConfigRootPath,
            SettingsFilePath = immersionReadyState.SettingsFilePath,
            StartupProfilePath = immersionReadyState.StartupProfilePath,
            RequiredAssets = immersionReadyState.RequiredAssets,
            ReadyAssetCount = immersionReadyState.ReadyAssetCount,
            CompletedSteps = immersionReadyState.CompletedSteps,
            TotalSteps = immersionReadyState.TotalSteps,
            Exists = immersionReadyState.Exists,
            ReadSucceeded = immersionReadyState.ReadSucceeded
        };

        if (!immersionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser affordance session blocked for profile '{immersionReadyState.ProfileId}'.";
            result.Error = immersionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAffordanceSessionVersion = "runtime-browser-affordance-session-v1";
        result.BrowserAffordanceStages =
        [
            "open-browser-affordance-session",
            "bind-browser-immersion-ready-state",
            "publish-browser-affordance-ready"
        ];
        result.BrowserAffordanceSummary = $"Runtime browser affordance session prepared {result.BrowserAffordanceStages.Length} affordance stage(s) for profile '{immersionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser affordance session ready for profile '{immersionReadyState.ProfileId}' with {result.BrowserAffordanceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAffordanceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAffordanceSessionVersion { get; set; } = string.Empty;
    public string BrowserImmersionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserImmersionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAffordanceStages { get; set; } = Array.Empty<string>();
    public string BrowserAffordanceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
