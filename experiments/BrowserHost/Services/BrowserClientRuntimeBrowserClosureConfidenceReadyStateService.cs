namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserClosureConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserClosureConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserClosureConfidenceReadyStateService : IBrowserClientRuntimeBrowserClosureConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserClosureConfidenceSession _runtimeBrowserClosureConfidenceSession;

    public BrowserClientRuntimeBrowserClosureConfidenceReadyStateService(IBrowserClientRuntimeBrowserClosureConfidenceSession runtimeBrowserClosureConfidenceSession)
    {
        _runtimeBrowserClosureConfidenceSession = runtimeBrowserClosureConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserClosureConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserClosureConfidenceSessionResult closureconfidenceSession = await _runtimeBrowserClosureConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserClosureConfidenceReadyStateResult result = new()
        {
            ProfileId = closureconfidenceSession.ProfileId,
            SessionId = closureconfidenceSession.SessionId,
            SessionPath = closureconfidenceSession.SessionPath,
            BrowserClosureConfidenceSessionVersion = closureconfidenceSession.BrowserClosureConfidenceSessionVersion,
            BrowserMilestoneAwarenessReadyStateVersion = closureconfidenceSession.BrowserMilestoneAwarenessReadyStateVersion,
            BrowserMilestoneAwarenessSessionVersion = closureconfidenceSession.BrowserMilestoneAwarenessSessionVersion,
            LaunchMode = closureconfidenceSession.LaunchMode,
            AssetRootPath = closureconfidenceSession.AssetRootPath,
            ProfilesRootPath = closureconfidenceSession.ProfilesRootPath,
            CacheRootPath = closureconfidenceSession.CacheRootPath,
            ConfigRootPath = closureconfidenceSession.ConfigRootPath,
            SettingsFilePath = closureconfidenceSession.SettingsFilePath,
            StartupProfilePath = closureconfidenceSession.StartupProfilePath,
            RequiredAssets = closureconfidenceSession.RequiredAssets,
            ReadyAssetCount = closureconfidenceSession.ReadyAssetCount,
            CompletedSteps = closureconfidenceSession.CompletedSteps,
            TotalSteps = closureconfidenceSession.TotalSteps,
            Exists = closureconfidenceSession.Exists,
            ReadSucceeded = closureconfidenceSession.ReadSucceeded
        };

        if (!closureconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser closureconfidence ready state blocked for profile '{closureconfidenceSession.ProfileId}'.";
            result.Error = closureconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserClosureConfidenceReadyStateVersion = "runtime-browser-closureconfidence-ready-state-v1";
        result.BrowserClosureConfidenceReadyChecks =
        [
            "browser-milestoneawareness-ready-state-ready",
            "browser-closureconfidence-session-ready",
            "browser-closureconfidence-ready"
        ];
        result.BrowserClosureConfidenceReadySummary = $"Runtime browser closureconfidence ready state passed {result.BrowserClosureConfidenceReadyChecks.Length} closureconfidence readiness check(s) for profile '{closureconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser closureconfidence ready state ready for profile '{closureconfidenceSession.ProfileId}' with {result.BrowserClosureConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserClosureConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserClosureConfidenceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserClosureConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserClosureConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
