namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMilestoneAwarenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateService : IBrowserClientRuntimeBrowserMilestoneAwarenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserMilestoneAwarenessSession _runtimeBrowserMilestoneAwarenessSession;

    public BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateService(IBrowserClientRuntimeBrowserMilestoneAwarenessSession runtimeBrowserMilestoneAwarenessSession)
    {
        _runtimeBrowserMilestoneAwarenessSession = runtimeBrowserMilestoneAwarenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMilestoneAwarenessSessionResult milestoneawarenessSession = await _runtimeBrowserMilestoneAwarenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateResult result = new()
        {
            ProfileId = milestoneawarenessSession.ProfileId,
            SessionId = milestoneawarenessSession.SessionId,
            SessionPath = milestoneawarenessSession.SessionPath,
            BrowserMilestoneAwarenessSessionVersion = milestoneawarenessSession.BrowserMilestoneAwarenessSessionVersion,
            BrowserPathfindingReadyStateVersion = milestoneawarenessSession.BrowserPathfindingReadyStateVersion,
            BrowserPathfindingSessionVersion = milestoneawarenessSession.BrowserPathfindingSessionVersion,
            LaunchMode = milestoneawarenessSession.LaunchMode,
            AssetRootPath = milestoneawarenessSession.AssetRootPath,
            ProfilesRootPath = milestoneawarenessSession.ProfilesRootPath,
            CacheRootPath = milestoneawarenessSession.CacheRootPath,
            ConfigRootPath = milestoneawarenessSession.ConfigRootPath,
            SettingsFilePath = milestoneawarenessSession.SettingsFilePath,
            StartupProfilePath = milestoneawarenessSession.StartupProfilePath,
            RequiredAssets = milestoneawarenessSession.RequiredAssets,
            ReadyAssetCount = milestoneawarenessSession.ReadyAssetCount,
            CompletedSteps = milestoneawarenessSession.CompletedSteps,
            TotalSteps = milestoneawarenessSession.TotalSteps,
            Exists = milestoneawarenessSession.Exists,
            ReadSucceeded = milestoneawarenessSession.ReadSucceeded
        };

        if (!milestoneawarenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser milestoneawareness ready state blocked for profile '{milestoneawarenessSession.ProfileId}'.";
            result.Error = milestoneawarenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMilestoneAwarenessReadyStateVersion = "runtime-browser-milestoneawareness-ready-state-v1";
        result.BrowserMilestoneAwarenessReadyChecks =
        [
            "browser-pathfinding-ready-state-ready",
            "browser-milestoneawareness-session-ready",
            "browser-milestoneawareness-ready"
        ];
        result.BrowserMilestoneAwarenessReadySummary = $"Runtime browser milestoneawareness ready state passed {result.BrowserMilestoneAwarenessReadyChecks.Length} milestoneawareness readiness check(s) for profile '{milestoneawarenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser milestoneawareness ready state ready for profile '{milestoneawarenessSession.ProfileId}' with {result.BrowserMilestoneAwarenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserMilestoneAwarenessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserMilestoneAwarenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserMilestoneAwarenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
