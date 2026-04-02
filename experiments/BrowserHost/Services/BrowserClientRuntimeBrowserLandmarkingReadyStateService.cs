namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLandmarkingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLandmarkingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLandmarkingReadyStateService : IBrowserClientRuntimeBrowserLandmarkingReadyState
{
    private readonly IBrowserClientRuntimeBrowserLandmarkingSession _runtimeBrowserLandmarkingSession;

    public BrowserClientRuntimeBrowserLandmarkingReadyStateService(IBrowserClientRuntimeBrowserLandmarkingSession runtimeBrowserLandmarkingSession)
    {
        _runtimeBrowserLandmarkingSession = runtimeBrowserLandmarkingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLandmarkingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLandmarkingSessionResult landmarkingSession = await _runtimeBrowserLandmarkingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLandmarkingReadyStateResult result = new()
        {
            ProfileId = landmarkingSession.ProfileId,
            SessionId = landmarkingSession.SessionId,
            SessionPath = landmarkingSession.SessionPath,
            BrowserLandmarkingSessionVersion = landmarkingSession.BrowserLandmarkingSessionVersion,
            BrowserCueingReadyStateVersion = landmarkingSession.BrowserCueingReadyStateVersion,
            BrowserCueingSessionVersion = landmarkingSession.BrowserCueingSessionVersion,
            LaunchMode = landmarkingSession.LaunchMode,
            AssetRootPath = landmarkingSession.AssetRootPath,
            ProfilesRootPath = landmarkingSession.ProfilesRootPath,
            CacheRootPath = landmarkingSession.CacheRootPath,
            ConfigRootPath = landmarkingSession.ConfigRootPath,
            SettingsFilePath = landmarkingSession.SettingsFilePath,
            StartupProfilePath = landmarkingSession.StartupProfilePath,
            RequiredAssets = landmarkingSession.RequiredAssets,
            ReadyAssetCount = landmarkingSession.ReadyAssetCount,
            CompletedSteps = landmarkingSession.CompletedSteps,
            TotalSteps = landmarkingSession.TotalSteps,
            Exists = landmarkingSession.Exists,
            ReadSucceeded = landmarkingSession.ReadSucceeded
        };

        if (!landmarkingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser landmarking ready state blocked for profile '{landmarkingSession.ProfileId}'.";
            result.Error = landmarkingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLandmarkingReadyStateVersion = "runtime-browser-landmarking-ready-state-v1";
        result.BrowserLandmarkingReadyChecks =
        [
            "browser-cueing-ready-state-ready",
            "browser-landmarking-session-ready",
            "browser-landmarking-ready"
        ];
        result.BrowserLandmarkingReadySummary = $"Runtime browser landmarking ready state passed {result.BrowserLandmarkingReadyChecks.Length} landmarking readiness check(s) for profile '{landmarkingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser landmarking ready state ready for profile '{landmarkingSession.ProfileId}' with {result.BrowserLandmarkingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLandmarkingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLandmarkingReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserLandmarkingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLandmarkingReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
