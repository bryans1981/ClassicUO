namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserClosureConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserClosureConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserClosureConfidenceSessionService : IBrowserClientRuntimeBrowserClosureConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserMilestoneAwarenessReadyState _runtimeBrowserMilestoneAwarenessReadyState;

    public BrowserClientRuntimeBrowserClosureConfidenceSessionService(IBrowserClientRuntimeBrowserMilestoneAwarenessReadyState runtimeBrowserMilestoneAwarenessReadyState)
    {
        _runtimeBrowserMilestoneAwarenessReadyState = runtimeBrowserMilestoneAwarenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserClosureConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMilestoneAwarenessReadyStateResult milestoneawarenessReadyState = await _runtimeBrowserMilestoneAwarenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserClosureConfidenceSessionResult result = new()
        {
            ProfileId = milestoneawarenessReadyState.ProfileId,
            SessionId = milestoneawarenessReadyState.SessionId,
            SessionPath = milestoneawarenessReadyState.SessionPath,
            BrowserMilestoneAwarenessReadyStateVersion = milestoneawarenessReadyState.BrowserMilestoneAwarenessReadyStateVersion,
            BrowserMilestoneAwarenessSessionVersion = milestoneawarenessReadyState.BrowserMilestoneAwarenessSessionVersion,
            LaunchMode = milestoneawarenessReadyState.LaunchMode,
            AssetRootPath = milestoneawarenessReadyState.AssetRootPath,
            ProfilesRootPath = milestoneawarenessReadyState.ProfilesRootPath,
            CacheRootPath = milestoneawarenessReadyState.CacheRootPath,
            ConfigRootPath = milestoneawarenessReadyState.ConfigRootPath,
            SettingsFilePath = milestoneawarenessReadyState.SettingsFilePath,
            StartupProfilePath = milestoneawarenessReadyState.StartupProfilePath,
            RequiredAssets = milestoneawarenessReadyState.RequiredAssets,
            ReadyAssetCount = milestoneawarenessReadyState.ReadyAssetCount,
            CompletedSteps = milestoneawarenessReadyState.CompletedSteps,
            TotalSteps = milestoneawarenessReadyState.TotalSteps,
            Exists = milestoneawarenessReadyState.Exists,
            ReadSucceeded = milestoneawarenessReadyState.ReadSucceeded
        };

        if (!milestoneawarenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser closureconfidence session blocked for profile '{milestoneawarenessReadyState.ProfileId}'.";
            result.Error = milestoneawarenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserClosureConfidenceSessionVersion = "runtime-browser-closureconfidence-session-v1";
        result.BrowserClosureConfidenceStages =
        [
            "open-browser-closureconfidence-session",
            "bind-browser-milestoneawareness-ready-state",
            "publish-browser-closureconfidence-ready"
        ];
        result.BrowserClosureConfidenceSummary = $"Runtime browser closureconfidence session prepared {result.BrowserClosureConfidenceStages.Length} closureconfidence stage(s) for profile '{milestoneawarenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser closureconfidence session ready for profile '{milestoneawarenessReadyState.ProfileId}' with {result.BrowserClosureConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserClosureConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserClosureConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserMilestoneAwarenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMilestoneAwarenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserClosureConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserClosureConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
