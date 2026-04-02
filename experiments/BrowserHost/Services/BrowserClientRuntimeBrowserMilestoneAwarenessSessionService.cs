namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMilestoneAwarenessSession
{
    ValueTask<BrowserClientRuntimeBrowserMilestoneAwarenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMilestoneAwarenessSessionService : IBrowserClientRuntimeBrowserMilestoneAwarenessSession
{
    private readonly IBrowserClientRuntimeBrowserPathfindingReadyState _runtimeBrowserPathfindingReadyState;

    public BrowserClientRuntimeBrowserMilestoneAwarenessSessionService(IBrowserClientRuntimeBrowserPathfindingReadyState runtimeBrowserPathfindingReadyState)
    {
        _runtimeBrowserPathfindingReadyState = runtimeBrowserPathfindingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMilestoneAwarenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPathfindingReadyStateResult pathfindingReadyState = await _runtimeBrowserPathfindingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserMilestoneAwarenessSessionResult result = new()
        {
            ProfileId = pathfindingReadyState.ProfileId,
            SessionId = pathfindingReadyState.SessionId,
            SessionPath = pathfindingReadyState.SessionPath,
            BrowserPathfindingReadyStateVersion = pathfindingReadyState.BrowserPathfindingReadyStateVersion,
            BrowserPathfindingSessionVersion = pathfindingReadyState.BrowserPathfindingSessionVersion,
            LaunchMode = pathfindingReadyState.LaunchMode,
            AssetRootPath = pathfindingReadyState.AssetRootPath,
            ProfilesRootPath = pathfindingReadyState.ProfilesRootPath,
            CacheRootPath = pathfindingReadyState.CacheRootPath,
            ConfigRootPath = pathfindingReadyState.ConfigRootPath,
            SettingsFilePath = pathfindingReadyState.SettingsFilePath,
            StartupProfilePath = pathfindingReadyState.StartupProfilePath,
            RequiredAssets = pathfindingReadyState.RequiredAssets,
            ReadyAssetCount = pathfindingReadyState.ReadyAssetCount,
            CompletedSteps = pathfindingReadyState.CompletedSteps,
            TotalSteps = pathfindingReadyState.TotalSteps,
            Exists = pathfindingReadyState.Exists,
            ReadSucceeded = pathfindingReadyState.ReadSucceeded
        };

        if (!pathfindingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser milestoneawareness session blocked for profile '{pathfindingReadyState.ProfileId}'.";
            result.Error = pathfindingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMilestoneAwarenessSessionVersion = "runtime-browser-milestoneawareness-session-v1";
        result.BrowserMilestoneAwarenessStages =
        [
            "open-browser-milestoneawareness-session",
            "bind-browser-pathfinding-ready-state",
            "publish-browser-milestoneawareness-ready"
        ];
        result.BrowserMilestoneAwarenessSummary = $"Runtime browser milestoneawareness session prepared {result.BrowserMilestoneAwarenessStages.Length} milestoneawareness stage(s) for profile '{pathfindingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser milestoneawareness session ready for profile '{pathfindingReadyState.ProfileId}' with {result.BrowserMilestoneAwarenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMilestoneAwarenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserMilestoneAwarenessSessionVersion { get; set; } = string.Empty;
    public string BrowserPathfindingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPathfindingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMilestoneAwarenessStages { get; set; } = Array.Empty<string>();
    public string BrowserMilestoneAwarenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
