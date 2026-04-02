namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOrientationConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserOrientationConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOrientationConfidenceSessionService : IBrowserClientRuntimeBrowserOrientationConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserLandmarkingReadyState _runtimeBrowserLandmarkingReadyState;

    public BrowserClientRuntimeBrowserOrientationConfidenceSessionService(IBrowserClientRuntimeBrowserLandmarkingReadyState runtimeBrowserLandmarkingReadyState)
    {
        _runtimeBrowserLandmarkingReadyState = runtimeBrowserLandmarkingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOrientationConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLandmarkingReadyStateResult landmarkingReadyState = await _runtimeBrowserLandmarkingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOrientationConfidenceSessionResult result = new()
        {
            ProfileId = landmarkingReadyState.ProfileId,
            SessionId = landmarkingReadyState.SessionId,
            SessionPath = landmarkingReadyState.SessionPath,
            BrowserLandmarkingReadyStateVersion = landmarkingReadyState.BrowserLandmarkingReadyStateVersion,
            BrowserLandmarkingSessionVersion = landmarkingReadyState.BrowserLandmarkingSessionVersion,
            LaunchMode = landmarkingReadyState.LaunchMode,
            AssetRootPath = landmarkingReadyState.AssetRootPath,
            ProfilesRootPath = landmarkingReadyState.ProfilesRootPath,
            CacheRootPath = landmarkingReadyState.CacheRootPath,
            ConfigRootPath = landmarkingReadyState.ConfigRootPath,
            SettingsFilePath = landmarkingReadyState.SettingsFilePath,
            StartupProfilePath = landmarkingReadyState.StartupProfilePath,
            RequiredAssets = landmarkingReadyState.RequiredAssets,
            ReadyAssetCount = landmarkingReadyState.ReadyAssetCount,
            CompletedSteps = landmarkingReadyState.CompletedSteps,
            TotalSteps = landmarkingReadyState.TotalSteps,
            Exists = landmarkingReadyState.Exists,
            ReadSucceeded = landmarkingReadyState.ReadSucceeded
        };

        if (!landmarkingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser orientationconfidence session blocked for profile '{landmarkingReadyState.ProfileId}'.";
            result.Error = landmarkingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOrientationConfidenceSessionVersion = "runtime-browser-orientationconfidence-session-v1";
        result.BrowserOrientationConfidenceStages =
        [
            "open-browser-orientationconfidence-session",
            "bind-browser-landmarking-ready-state",
            "publish-browser-orientationconfidence-ready"
        ];
        result.BrowserOrientationConfidenceSummary = $"Runtime browser orientationconfidence session prepared {result.BrowserOrientationConfidenceStages.Length} orientationconfidence stage(s) for profile '{landmarkingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser orientationconfidence session ready for profile '{landmarkingReadyState.ProfileId}' with {result.BrowserOrientationConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOrientationConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserOrientationConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserLandmarkingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLandmarkingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOrientationConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserOrientationConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
