namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProgressAwarenessSession
{
    ValueTask<BrowserClientRuntimeBrowserProgressAwarenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProgressAwarenessSessionService : IBrowserClientRuntimeBrowserProgressAwarenessSession
{
    private readonly IBrowserClientRuntimeBrowserWayfindingReadyState _runtimeBrowserWayfindingReadyState;

    public BrowserClientRuntimeBrowserProgressAwarenessSessionService(IBrowserClientRuntimeBrowserWayfindingReadyState runtimeBrowserWayfindingReadyState)
    {
        _runtimeBrowserWayfindingReadyState = runtimeBrowserWayfindingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProgressAwarenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserWayfindingReadyStateResult wayfindingReadyState = await _runtimeBrowserWayfindingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProgressAwarenessSessionResult result = new()
        {
            ProfileId = wayfindingReadyState.ProfileId,
            SessionId = wayfindingReadyState.SessionId,
            SessionPath = wayfindingReadyState.SessionPath,
            BrowserWayfindingReadyStateVersion = wayfindingReadyState.BrowserWayfindingReadyStateVersion,
            BrowserWayfindingSessionVersion = wayfindingReadyState.BrowserWayfindingSessionVersion,
            LaunchMode = wayfindingReadyState.LaunchMode,
            AssetRootPath = wayfindingReadyState.AssetRootPath,
            ProfilesRootPath = wayfindingReadyState.ProfilesRootPath,
            CacheRootPath = wayfindingReadyState.CacheRootPath,
            ConfigRootPath = wayfindingReadyState.ConfigRootPath,
            SettingsFilePath = wayfindingReadyState.SettingsFilePath,
            StartupProfilePath = wayfindingReadyState.StartupProfilePath,
            RequiredAssets = wayfindingReadyState.RequiredAssets,
            ReadyAssetCount = wayfindingReadyState.ReadyAssetCount,
            CompletedSteps = wayfindingReadyState.CompletedSteps,
            TotalSteps = wayfindingReadyState.TotalSteps,
            Exists = wayfindingReadyState.Exists,
            ReadSucceeded = wayfindingReadyState.ReadSucceeded
        };

        if (!wayfindingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser progressawareness session blocked for profile '{wayfindingReadyState.ProfileId}'.";
            result.Error = wayfindingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProgressAwarenessSessionVersion = "runtime-browser-progressawareness-session-v1";
        result.BrowserProgressAwarenessStages =
        [
            "open-browser-progressawareness-session",
            "bind-browser-wayfinding-ready-state",
            "publish-browser-progressawareness-ready"
        ];
        result.BrowserProgressAwarenessSummary = $"Runtime browser progressawareness session prepared {result.BrowserProgressAwarenessStages.Length} progressawareness stage(s) for profile '{wayfindingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser progressawareness session ready for profile '{wayfindingReadyState.ProfileId}' with {result.BrowserProgressAwarenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProgressAwarenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserProgressAwarenessSessionVersion { get; set; } = string.Empty;
    public string BrowserWayfindingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserWayfindingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProgressAwarenessStages { get; set; } = Array.Empty<string>();
    public string BrowserProgressAwarenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
