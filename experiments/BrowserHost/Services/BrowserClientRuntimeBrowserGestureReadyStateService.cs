namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGestureReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGestureReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGestureReadyStateService : IBrowserClientRuntimeBrowserGestureReadyState
{
    private readonly IBrowserClientRuntimeBrowserGestureSession _runtimeBrowserGestureSession;

    public BrowserClientRuntimeBrowserGestureReadyStateService(IBrowserClientRuntimeBrowserGestureSession runtimeBrowserGestureSession)
    {
        _runtimeBrowserGestureSession = runtimeBrowserGestureSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGestureReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGestureSessionResult gestureSession = await _runtimeBrowserGestureSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGestureReadyStateResult result = new()
        {
            ProfileId = gestureSession.ProfileId,
            SessionId = gestureSession.SessionId,
            SessionPath = gestureSession.SessionPath,
            BrowserGestureSessionVersion = gestureSession.BrowserGestureSessionVersion,
            BrowserCommandReadyStateVersion = gestureSession.BrowserCommandReadyStateVersion,
            BrowserCommandSessionVersion = gestureSession.BrowserCommandSessionVersion,
            LaunchMode = gestureSession.LaunchMode,
            AssetRootPath = gestureSession.AssetRootPath,
            ProfilesRootPath = gestureSession.ProfilesRootPath,
            CacheRootPath = gestureSession.CacheRootPath,
            ConfigRootPath = gestureSession.ConfigRootPath,
            SettingsFilePath = gestureSession.SettingsFilePath,
            StartupProfilePath = gestureSession.StartupProfilePath,
            RequiredAssets = gestureSession.RequiredAssets,
            ReadyAssetCount = gestureSession.ReadyAssetCount,
            CompletedSteps = gestureSession.CompletedSteps,
            TotalSteps = gestureSession.TotalSteps,
            Exists = gestureSession.Exists,
            ReadSucceeded = gestureSession.ReadSucceeded,
            BrowserGestureSession = gestureSession
        };

        if (!gestureSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser gesture ready state blocked for profile '{gestureSession.ProfileId}'.";
            result.Error = gestureSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGestureReadyStateVersion = "runtime-browser-gesture-ready-state-v1";
        result.BrowserGestureReadyChecks =
        [
            "browser-command-ready-state-ready",
            "browser-gesture-session-ready",
            "browser-gesture-ready"
        ];
        result.BrowserGestureReadySummary = $"Runtime browser gesture ready state passed {result.BrowserGestureReadyChecks.Length} gesture readiness check(s) for profile '{gestureSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser gesture ready state ready for profile '{gestureSession.ProfileId}' with {result.BrowserGestureReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGestureReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGestureReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGestureSessionVersion { get; set; } = string.Empty;
    public string BrowserCommandReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCommandSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGestureReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGestureReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserGestureSessionResult BrowserGestureSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
