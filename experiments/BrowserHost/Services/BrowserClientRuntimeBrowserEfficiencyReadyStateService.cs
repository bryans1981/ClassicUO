namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEfficiencyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEfficiencyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEfficiencyReadyStateService : IBrowserClientRuntimeBrowserEfficiencyReadyState
{
    private readonly IBrowserClientRuntimeBrowserEfficiencySession _runtimeBrowserEfficiencySession;

    public BrowserClientRuntimeBrowserEfficiencyReadyStateService(IBrowserClientRuntimeBrowserEfficiencySession runtimeBrowserEfficiencySession)
    {
        _runtimeBrowserEfficiencySession = runtimeBrowserEfficiencySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEfficiencyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEfficiencySessionResult efficiencySession = await _runtimeBrowserEfficiencySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEfficiencyReadyStateResult result = new()
        {
            ProfileId = efficiencySession.ProfileId,
            SessionId = efficiencySession.SessionId,
            SessionPath = efficiencySession.SessionPath,
            BrowserEfficiencySessionVersion = efficiencySession.BrowserEfficiencySessionVersion,
            BrowserFeedbackReadyStateVersion = efficiencySession.BrowserFeedbackReadyStateVersion,
            BrowserFeedbackSessionVersion = efficiencySession.BrowserFeedbackSessionVersion,
            LaunchMode = efficiencySession.LaunchMode,
            AssetRootPath = efficiencySession.AssetRootPath,
            ProfilesRootPath = efficiencySession.ProfilesRootPath,
            CacheRootPath = efficiencySession.CacheRootPath,
            ConfigRootPath = efficiencySession.ConfigRootPath,
            SettingsFilePath = efficiencySession.SettingsFilePath,
            StartupProfilePath = efficiencySession.StartupProfilePath,
            RequiredAssets = efficiencySession.RequiredAssets,
            ReadyAssetCount = efficiencySession.ReadyAssetCount,
            CompletedSteps = efficiencySession.CompletedSteps,
            TotalSteps = efficiencySession.TotalSteps,
            Exists = efficiencySession.Exists,
            ReadSucceeded = efficiencySession.ReadSucceeded
        };

        if (!efficiencySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser efficiency ready state blocked for profile '{efficiencySession.ProfileId}'.";
            result.Error = efficiencySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEfficiencyReadyStateVersion = "runtime-browser-efficiency-ready-state-v1";
        result.BrowserEfficiencyReadyChecks =
        [
            "browser-feedback-ready-state-ready",
            "browser-efficiency-session-ready",
            "browser-efficiency-ready"
        ];
        result.BrowserEfficiencyReadySummary = $"Runtime browser efficiency ready state passed {result.BrowserEfficiencyReadyChecks.Length} efficiency readiness check(s) for profile '{efficiencySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser efficiency ready state ready for profile '{efficiencySession.ProfileId}' with {result.BrowserEfficiencyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEfficiencyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEfficiencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEfficiencySessionVersion { get; set; } = string.Empty;
    public string BrowserFeedbackReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFeedbackSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEfficiencyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEfficiencyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
