namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLandmarkingSession
{
    ValueTask<BrowserClientRuntimeBrowserLandmarkingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLandmarkingSessionService : IBrowserClientRuntimeBrowserLandmarkingSession
{
    private readonly IBrowserClientRuntimeBrowserCueingReadyState _runtimeBrowserCueingReadyState;

    public BrowserClientRuntimeBrowserLandmarkingSessionService(IBrowserClientRuntimeBrowserCueingReadyState runtimeBrowserCueingReadyState)
    {
        _runtimeBrowserCueingReadyState = runtimeBrowserCueingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLandmarkingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCueingReadyStateResult cueingReadyState = await _runtimeBrowserCueingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLandmarkingSessionResult result = new()
        {
            ProfileId = cueingReadyState.ProfileId,
            SessionId = cueingReadyState.SessionId,
            SessionPath = cueingReadyState.SessionPath,
            BrowserCueingReadyStateVersion = cueingReadyState.BrowserCueingReadyStateVersion,
            BrowserCueingSessionVersion = cueingReadyState.BrowserCueingSessionVersion,
            LaunchMode = cueingReadyState.LaunchMode,
            AssetRootPath = cueingReadyState.AssetRootPath,
            ProfilesRootPath = cueingReadyState.ProfilesRootPath,
            CacheRootPath = cueingReadyState.CacheRootPath,
            ConfigRootPath = cueingReadyState.ConfigRootPath,
            SettingsFilePath = cueingReadyState.SettingsFilePath,
            StartupProfilePath = cueingReadyState.StartupProfilePath,
            RequiredAssets = cueingReadyState.RequiredAssets,
            ReadyAssetCount = cueingReadyState.ReadyAssetCount,
            CompletedSteps = cueingReadyState.CompletedSteps,
            TotalSteps = cueingReadyState.TotalSteps,
            Exists = cueingReadyState.Exists,
            ReadSucceeded = cueingReadyState.ReadSucceeded
        };

        if (!cueingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser landmarking session blocked for profile '{cueingReadyState.ProfileId}'.";
            result.Error = cueingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLandmarkingSessionVersion = "runtime-browser-landmarking-session-v1";
        result.BrowserLandmarkingStages =
        [
            "open-browser-landmarking-session",
            "bind-browser-cueing-ready-state",
            "publish-browser-landmarking-ready"
        ];
        result.BrowserLandmarkingSummary = $"Runtime browser landmarking session prepared {result.BrowserLandmarkingStages.Length} landmarking stage(s) for profile '{cueingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser landmarking session ready for profile '{cueingReadyState.ProfileId}' with {result.BrowserLandmarkingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLandmarkingSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLandmarkingSessionVersion { get; set; } = string.Empty;
    public string BrowserCueingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCueingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLandmarkingStages { get; set; } = Array.Empty<string>();
    public string BrowserLandmarkingSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
