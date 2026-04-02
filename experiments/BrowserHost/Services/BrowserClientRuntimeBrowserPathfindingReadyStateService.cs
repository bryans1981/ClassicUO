namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPathfindingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPathfindingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPathfindingReadyStateService : IBrowserClientRuntimeBrowserPathfindingReadyState
{
    private readonly IBrowserClientRuntimeBrowserPathfindingSession _runtimeBrowserPathfindingSession;

    public BrowserClientRuntimeBrowserPathfindingReadyStateService(IBrowserClientRuntimeBrowserPathfindingSession runtimeBrowserPathfindingSession)
    {
        _runtimeBrowserPathfindingSession = runtimeBrowserPathfindingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPathfindingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPathfindingSessionResult pathfindingSession = await _runtimeBrowserPathfindingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPathfindingReadyStateResult result = new()
        {
            ProfileId = pathfindingSession.ProfileId,
            SessionId = pathfindingSession.SessionId,
            SessionPath = pathfindingSession.SessionPath,
            BrowserPathfindingSessionVersion = pathfindingSession.BrowserPathfindingSessionVersion,
            BrowserOrientationConfidenceReadyStateVersion = pathfindingSession.BrowserOrientationConfidenceReadyStateVersion,
            BrowserOrientationConfidenceSessionVersion = pathfindingSession.BrowserOrientationConfidenceSessionVersion,
            LaunchMode = pathfindingSession.LaunchMode,
            AssetRootPath = pathfindingSession.AssetRootPath,
            ProfilesRootPath = pathfindingSession.ProfilesRootPath,
            CacheRootPath = pathfindingSession.CacheRootPath,
            ConfigRootPath = pathfindingSession.ConfigRootPath,
            SettingsFilePath = pathfindingSession.SettingsFilePath,
            StartupProfilePath = pathfindingSession.StartupProfilePath,
            RequiredAssets = pathfindingSession.RequiredAssets,
            ReadyAssetCount = pathfindingSession.ReadyAssetCount,
            CompletedSteps = pathfindingSession.CompletedSteps,
            TotalSteps = pathfindingSession.TotalSteps,
            Exists = pathfindingSession.Exists,
            ReadSucceeded = pathfindingSession.ReadSucceeded
        };

        if (!pathfindingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser pathfinding ready state blocked for profile '{pathfindingSession.ProfileId}'.";
            result.Error = pathfindingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPathfindingReadyStateVersion = "runtime-browser-pathfinding-ready-state-v1";
        result.BrowserPathfindingReadyChecks =
        [
            "browser-orientationconfidence-ready-state-ready",
            "browser-pathfinding-session-ready",
            "browser-pathfinding-ready"
        ];
        result.BrowserPathfindingReadySummary = $"Runtime browser pathfinding ready state passed {result.BrowserPathfindingReadyChecks.Length} pathfinding readiness check(s) for profile '{pathfindingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser pathfinding ready state ready for profile '{pathfindingSession.ProfileId}' with {result.BrowserPathfindingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPathfindingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPathfindingReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserPathfindingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPathfindingReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
