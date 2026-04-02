namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOrientationConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOrientationConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOrientationConfidenceReadyStateService : IBrowserClientRuntimeBrowserOrientationConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserOrientationConfidenceSession _runtimeBrowserOrientationConfidenceSession;

    public BrowserClientRuntimeBrowserOrientationConfidenceReadyStateService(IBrowserClientRuntimeBrowserOrientationConfidenceSession runtimeBrowserOrientationConfidenceSession)
    {
        _runtimeBrowserOrientationConfidenceSession = runtimeBrowserOrientationConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOrientationConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOrientationConfidenceSessionResult orientationconfidenceSession = await _runtimeBrowserOrientationConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOrientationConfidenceReadyStateResult result = new()
        {
            ProfileId = orientationconfidenceSession.ProfileId,
            SessionId = orientationconfidenceSession.SessionId,
            SessionPath = orientationconfidenceSession.SessionPath,
            BrowserOrientationConfidenceSessionVersion = orientationconfidenceSession.BrowserOrientationConfidenceSessionVersion,
            BrowserLandmarkingReadyStateVersion = orientationconfidenceSession.BrowserLandmarkingReadyStateVersion,
            BrowserLandmarkingSessionVersion = orientationconfidenceSession.BrowserLandmarkingSessionVersion,
            LaunchMode = orientationconfidenceSession.LaunchMode,
            AssetRootPath = orientationconfidenceSession.AssetRootPath,
            ProfilesRootPath = orientationconfidenceSession.ProfilesRootPath,
            CacheRootPath = orientationconfidenceSession.CacheRootPath,
            ConfigRootPath = orientationconfidenceSession.ConfigRootPath,
            SettingsFilePath = orientationconfidenceSession.SettingsFilePath,
            StartupProfilePath = orientationconfidenceSession.StartupProfilePath,
            RequiredAssets = orientationconfidenceSession.RequiredAssets,
            ReadyAssetCount = orientationconfidenceSession.ReadyAssetCount,
            CompletedSteps = orientationconfidenceSession.CompletedSteps,
            TotalSteps = orientationconfidenceSession.TotalSteps,
            Exists = orientationconfidenceSession.Exists,
            ReadSucceeded = orientationconfidenceSession.ReadSucceeded
        };

        if (!orientationconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser orientationconfidence ready state blocked for profile '{orientationconfidenceSession.ProfileId}'.";
            result.Error = orientationconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOrientationConfidenceReadyStateVersion = "runtime-browser-orientationconfidence-ready-state-v1";
        result.BrowserOrientationConfidenceReadyChecks =
        [
            "browser-landmarking-ready-state-ready",
            "browser-orientationconfidence-session-ready",
            "browser-orientationconfidence-ready"
        ];
        result.BrowserOrientationConfidenceReadySummary = $"Runtime browser orientationconfidence ready state passed {result.BrowserOrientationConfidenceReadyChecks.Length} orientationconfidence readiness check(s) for profile '{orientationconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser orientationconfidence ready state ready for profile '{orientationconfidenceSession.ProfileId}' with {result.BrowserOrientationConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOrientationConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOrientationConfidenceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserOrientationConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOrientationConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
