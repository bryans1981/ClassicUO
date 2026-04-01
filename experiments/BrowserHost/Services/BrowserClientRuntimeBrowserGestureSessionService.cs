namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGestureSession
{
    ValueTask<BrowserClientRuntimeBrowserGestureSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGestureSessionService : IBrowserClientRuntimeBrowserGestureSession
{
    private readonly IBrowserClientRuntimeBrowserCommandReadyState _runtimeBrowserCommandReadyState;

    public BrowserClientRuntimeBrowserGestureSessionService(IBrowserClientRuntimeBrowserCommandReadyState runtimeBrowserCommandReadyState)
    {
        _runtimeBrowserCommandReadyState = runtimeBrowserCommandReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGestureSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCommandReadyStateResult commandReadyState = await _runtimeBrowserCommandReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGestureSessionResult result = new()
        {
            ProfileId = commandReadyState.ProfileId,
            SessionId = commandReadyState.SessionId,
            SessionPath = commandReadyState.SessionPath,
            BrowserCommandReadyStateVersion = commandReadyState.BrowserCommandReadyStateVersion,
            BrowserCommandSessionVersion = commandReadyState.BrowserCommandSessionVersion,
            LaunchMode = commandReadyState.LaunchMode,
            AssetRootPath = commandReadyState.AssetRootPath,
            ProfilesRootPath = commandReadyState.ProfilesRootPath,
            CacheRootPath = commandReadyState.CacheRootPath,
            ConfigRootPath = commandReadyState.ConfigRootPath,
            SettingsFilePath = commandReadyState.SettingsFilePath,
            StartupProfilePath = commandReadyState.StartupProfilePath,
            RequiredAssets = commandReadyState.RequiredAssets,
            ReadyAssetCount = commandReadyState.ReadyAssetCount,
            CompletedSteps = commandReadyState.CompletedSteps,
            TotalSteps = commandReadyState.TotalSteps,
            Exists = commandReadyState.Exists,
            ReadSucceeded = commandReadyState.ReadSucceeded,
            BrowserCommandReadyState = commandReadyState
        };

        if (!commandReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser gesture session blocked for profile '{commandReadyState.ProfileId}'.";
            result.Error = commandReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGestureSessionVersion = "runtime-browser-gesture-session-v1";
        result.BrowserGestureStages =
        [
            "open-browser-gesture-session",
            "bind-browser-command-ready-state",
            "publish-browser-gesture-ready"
        ];
        result.BrowserGestureSummary = $"Runtime browser gesture session prepared {result.BrowserGestureStages.Length} gesture stage(s) for profile '{commandReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser gesture session ready for profile '{commandReadyState.ProfileId}' with {result.BrowserGestureStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGestureSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserGestureStages { get; set; } = Array.Empty<string>();
    public string BrowserGestureSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserCommandReadyStateResult BrowserCommandReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
