namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserContinuationReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationReadinessReadyStateService : IBrowserClientRuntimeBrowserContinuationReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserContinuationReadinessSession _runtimeBrowserContinuationReadinessSession;

    public BrowserClientRuntimeBrowserContinuationReadinessReadyStateService(IBrowserClientRuntimeBrowserContinuationReadinessSession runtimeBrowserContinuationReadinessSession)
    {
        _runtimeBrowserContinuationReadinessSession = runtimeBrowserContinuationReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationReadinessSessionResult continuationreadinessSession = await _runtimeBrowserContinuationReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserContinuationReadinessReadyStateResult result = new()
        {
            ProfileId = continuationreadinessSession.ProfileId,
            SessionId = continuationreadinessSession.SessionId,
            SessionPath = continuationreadinessSession.SessionPath,
            BrowserContinuationReadinessSessionVersion = continuationreadinessSession.BrowserContinuationReadinessSessionVersion,
            BrowserFlowContinuityReadyStateVersion = continuationreadinessSession.BrowserFlowContinuityReadyStateVersion,
            BrowserFlowContinuitySessionVersion = continuationreadinessSession.BrowserFlowContinuitySessionVersion,
            LaunchMode = continuationreadinessSession.LaunchMode,
            AssetRootPath = continuationreadinessSession.AssetRootPath,
            ProfilesRootPath = continuationreadinessSession.ProfilesRootPath,
            CacheRootPath = continuationreadinessSession.CacheRootPath,
            ConfigRootPath = continuationreadinessSession.ConfigRootPath,
            SettingsFilePath = continuationreadinessSession.SettingsFilePath,
            StartupProfilePath = continuationreadinessSession.StartupProfilePath,
            RequiredAssets = continuationreadinessSession.RequiredAssets,
            ReadyAssetCount = continuationreadinessSession.ReadyAssetCount,
            CompletedSteps = continuationreadinessSession.CompletedSteps,
            TotalSteps = continuationreadinessSession.TotalSteps,
            Exists = continuationreadinessSession.Exists,
            ReadSucceeded = continuationreadinessSession.ReadSucceeded
        };

        if (!continuationreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser continuationreadiness ready state blocked for profile '{continuationreadinessSession.ProfileId}'.";
            result.Error = continuationreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationReadinessReadyStateVersion = "runtime-browser-continuationreadiness-ready-state-v1";
        result.BrowserContinuationReadinessReadyChecks =
        [
            "browser-flowcontinuity-ready-state-ready",
            "browser-continuationreadiness-session-ready",
            "browser-continuationreadiness-ready"
        ];
        result.BrowserContinuationReadinessReadySummary = $"Runtime browser continuationreadiness ready state passed {result.BrowserContinuationReadinessReadyChecks.Length} continuationreadiness readiness check(s) for profile '{continuationreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationreadiness ready state ready for profile '{continuationreadinessSession.ProfileId}' with {result.BrowserContinuationReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserFlowContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserContinuationReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
