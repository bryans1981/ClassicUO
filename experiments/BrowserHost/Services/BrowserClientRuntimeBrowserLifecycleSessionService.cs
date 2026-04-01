namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLifecycleSession
{
    ValueTask<BrowserClientRuntimeBrowserLifecycleSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLifecycleSessionService : IBrowserClientRuntimeBrowserLifecycleSession
{
    private readonly IBrowserClientRuntimeBrowserGestureReadyState _runtimeBrowserGestureReadyState;

    public BrowserClientRuntimeBrowserLifecycleSessionService(IBrowserClientRuntimeBrowserGestureReadyState runtimeBrowserGestureReadyState)
    {
        _runtimeBrowserGestureReadyState = runtimeBrowserGestureReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLifecycleSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGestureReadyStateResult gestureReadyState = await _runtimeBrowserGestureReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLifecycleSessionResult result = new()
        {
            ProfileId = gestureReadyState.ProfileId,
            SessionId = gestureReadyState.SessionId,
            SessionPath = gestureReadyState.SessionPath,
            BrowserGestureReadyStateVersion = gestureReadyState.BrowserGestureReadyStateVersion,
            BrowserGestureSessionVersion = gestureReadyState.BrowserGestureSessionVersion,
            LaunchMode = gestureReadyState.LaunchMode,
            AssetRootPath = gestureReadyState.AssetRootPath,
            ProfilesRootPath = gestureReadyState.ProfilesRootPath,
            CacheRootPath = gestureReadyState.CacheRootPath,
            ConfigRootPath = gestureReadyState.ConfigRootPath,
            SettingsFilePath = gestureReadyState.SettingsFilePath,
            StartupProfilePath = gestureReadyState.StartupProfilePath,
            RequiredAssets = gestureReadyState.RequiredAssets,
            ReadyAssetCount = gestureReadyState.ReadyAssetCount,
            CompletedSteps = gestureReadyState.CompletedSteps,
            TotalSteps = gestureReadyState.TotalSteps,
            Exists = gestureReadyState.Exists,
            ReadSucceeded = gestureReadyState.ReadSucceeded,
            BrowserGestureReadyState = gestureReadyState
        };

        if (!gestureReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser lifecycle session blocked for profile '{gestureReadyState.ProfileId}'.";
            result.Error = gestureReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLifecycleSessionVersion = "runtime-browser-lifecycle-session-v1";
        result.BrowserLifecycleStages =
        [
            "open-browser-lifecycle-session",
            "bind-browser-gesture-ready-state",
            "publish-browser-lifecycle-ready"
        ];
        result.BrowserLifecycleSummary = $"Runtime browser lifecycle session prepared {result.BrowserLifecycleStages.Length} lifecycle stage(s) for profile '{gestureReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser lifecycle session ready for profile '{gestureReadyState.ProfileId}' with {result.BrowserLifecycleStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLifecycleSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLifecycleSessionVersion { get; set; } = string.Empty;
    public string BrowserGestureReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGestureSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLifecycleStages { get; set; } = Array.Empty<string>();
    public string BrowserLifecycleSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserGestureReadyStateResult BrowserGestureReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
