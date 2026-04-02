namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEngagementReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEngagementReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEngagementReadyStateService : IBrowserClientRuntimeBrowserEngagementReadyState
{
    private readonly IBrowserClientRuntimeBrowserEngagementSession _runtimeBrowserEngagementSession;

    public BrowserClientRuntimeBrowserEngagementReadyStateService(IBrowserClientRuntimeBrowserEngagementSession runtimeBrowserEngagementSession)
    {
        _runtimeBrowserEngagementSession = runtimeBrowserEngagementSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEngagementReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEngagementSessionResult engagementSession = await _runtimeBrowserEngagementSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEngagementReadyStateResult result = new()
        {
            ProfileId = engagementSession.ProfileId,
            SessionId = engagementSession.SessionId,
            SessionPath = engagementSession.SessionPath,
            BrowserEngagementSessionVersion = engagementSession.BrowserEngagementSessionVersion,
            BrowserFlowStateReadyStateVersion = engagementSession.BrowserFlowStateReadyStateVersion,
            BrowserFlowStateSessionVersion = engagementSession.BrowserFlowStateSessionVersion,
            LaunchMode = engagementSession.LaunchMode,
            AssetRootPath = engagementSession.AssetRootPath,
            ProfilesRootPath = engagementSession.ProfilesRootPath,
            CacheRootPath = engagementSession.CacheRootPath,
            ConfigRootPath = engagementSession.ConfigRootPath,
            SettingsFilePath = engagementSession.SettingsFilePath,
            StartupProfilePath = engagementSession.StartupProfilePath,
            RequiredAssets = engagementSession.RequiredAssets,
            ReadyAssetCount = engagementSession.ReadyAssetCount,
            CompletedSteps = engagementSession.CompletedSteps,
            TotalSteps = engagementSession.TotalSteps,
            Exists = engagementSession.Exists,
            ReadSucceeded = engagementSession.ReadSucceeded
        };

        if (!engagementSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser engagement ready state blocked for profile '{engagementSession.ProfileId}'.";
            result.Error = engagementSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEngagementReadyStateVersion = "runtime-browser-engagement-ready-state-v1";
        result.BrowserEngagementReadyChecks =
        [
            "browser-flowstate-ready-state-ready",
            "browser-engagement-session-ready",
            "browser-engagement-ready"
        ];
        result.BrowserEngagementReadySummary = $"Runtime browser engagement ready state passed {result.BrowserEngagementReadyChecks.Length} engagement readiness check(s) for profile '{engagementSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser engagement ready state ready for profile '{engagementSession.ProfileId}' with {result.BrowserEngagementReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEngagementReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEngagementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEngagementSessionVersion { get; set; } = string.Empty;
    public string BrowserFlowStateReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowStateSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEngagementReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEngagementReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
