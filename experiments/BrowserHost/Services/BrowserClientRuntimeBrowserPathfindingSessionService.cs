namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPathfindingSession
{
    ValueTask<BrowserClientRuntimeBrowserPathfindingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPathfindingSessionService : IBrowserClientRuntimeBrowserPathfindingSession
{
    private readonly IBrowserClientRuntimeBrowserOrientationConfidenceReadyState _runtimeBrowserOrientationConfidenceReadyState;

    public BrowserClientRuntimeBrowserPathfindingSessionService(IBrowserClientRuntimeBrowserOrientationConfidenceReadyState runtimeBrowserOrientationConfidenceReadyState)
    {
        _runtimeBrowserOrientationConfidenceReadyState = runtimeBrowserOrientationConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPathfindingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOrientationConfidenceReadyStateResult orientationconfidenceReadyState = await _runtimeBrowserOrientationConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPathfindingSessionResult result = new()
        {
            ProfileId = orientationconfidenceReadyState.ProfileId,
            SessionId = orientationconfidenceReadyState.SessionId,
            SessionPath = orientationconfidenceReadyState.SessionPath,
            BrowserOrientationConfidenceReadyStateVersion = orientationconfidenceReadyState.BrowserOrientationConfidenceReadyStateVersion,
            BrowserOrientationConfidenceSessionVersion = orientationconfidenceReadyState.BrowserOrientationConfidenceSessionVersion,
            LaunchMode = orientationconfidenceReadyState.LaunchMode,
            AssetRootPath = orientationconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = orientationconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = orientationconfidenceReadyState.CacheRootPath,
            ConfigRootPath = orientationconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = orientationconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = orientationconfidenceReadyState.StartupProfilePath,
            RequiredAssets = orientationconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = orientationconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = orientationconfidenceReadyState.CompletedSteps,
            TotalSteps = orientationconfidenceReadyState.TotalSteps,
            Exists = orientationconfidenceReadyState.Exists,
            ReadSucceeded = orientationconfidenceReadyState.ReadSucceeded
        };

        if (!orientationconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser pathfinding session blocked for profile '{orientationconfidenceReadyState.ProfileId}'.";
            result.Error = orientationconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPathfindingSessionVersion = "runtime-browser-pathfinding-session-v1";
        result.BrowserPathfindingStages =
        [
            "open-browser-pathfinding-session",
            "bind-browser-orientationconfidence-ready-state",
            "publish-browser-pathfinding-ready"
        ];
        result.BrowserPathfindingSummary = $"Runtime browser pathfinding session prepared {result.BrowserPathfindingStages.Length} pathfinding stage(s) for profile '{orientationconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser pathfinding session ready for profile '{orientationconfidenceReadyState.ProfileId}' with {result.BrowserPathfindingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPathfindingSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPathfindingSessionVersion { get; set; } = string.Empty;
    public string BrowserOrientationConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOrientationConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPathfindingStages { get; set; } = Array.Empty<string>();
    public string BrowserPathfindingSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
