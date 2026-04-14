namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserContinuationAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationAssuranceReadyStateService : IBrowserClientRuntimeBrowserContinuationAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserContinuationAssuranceSession _runtimeBrowserContinuationAssuranceSession;

    public BrowserClientRuntimeBrowserContinuationAssuranceReadyStateService(IBrowserClientRuntimeBrowserContinuationAssuranceSession runtimeBrowserContinuationAssuranceSession)
    {
        _runtimeBrowserContinuationAssuranceSession = runtimeBrowserContinuationAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationAssuranceSessionResult continuationassuranceSession = await _runtimeBrowserContinuationAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserContinuationAssuranceReadyStateResult result = new()
        {
            ProfileId = continuationassuranceSession.ProfileId,
            SessionId = continuationassuranceSession.SessionId,
            SessionPath = continuationassuranceSession.SessionPath,
            BrowserContinuationAssuranceSessionVersion = continuationassuranceSession.BrowserContinuationAssuranceSessionVersion,
            BrowserInteractionStabilityReadyStateVersion = continuationassuranceSession.BrowserInteractionStabilityReadyStateVersion,
            BrowserInteractionStabilitySessionVersion = continuationassuranceSession.BrowserInteractionStabilitySessionVersion,
            LaunchMode = continuationassuranceSession.LaunchMode,
            AssetRootPath = continuationassuranceSession.AssetRootPath,
            ProfilesRootPath = continuationassuranceSession.ProfilesRootPath,
            CacheRootPath = continuationassuranceSession.CacheRootPath,
            ConfigRootPath = continuationassuranceSession.ConfigRootPath,
            SettingsFilePath = continuationassuranceSession.SettingsFilePath,
            StartupProfilePath = continuationassuranceSession.StartupProfilePath,
            RequiredAssets = continuationassuranceSession.RequiredAssets,
            ReadyAssetCount = continuationassuranceSession.ReadyAssetCount,
            CompletedSteps = continuationassuranceSession.CompletedSteps,
            TotalSteps = continuationassuranceSession.TotalSteps,
            Exists = continuationassuranceSession.Exists,
            ReadSucceeded = continuationassuranceSession.ReadSucceeded
        };

        if (!continuationassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser continuationassurance ready state blocked for profile '{continuationassuranceSession.ProfileId}'.";
            result.Error = continuationassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationAssuranceReadyStateVersion = "runtime-browser-continuationassurance-ready-state-v1";
        result.BrowserContinuationAssuranceReadyChecks =
        [
            "browser-interactionstability-ready-state-ready",
            "browser-continuationassurance-session-ready",
            "browser-continuationassurance-ready"
        ];
        result.BrowserContinuationAssuranceReadySummary = $"Runtime browser continuationassurance ready state passed {result.BrowserContinuationAssuranceReadyChecks.Length} continuationassurance readiness check(s) for profile '{continuationassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationassurance ready state ready for profile '{continuationassuranceSession.ProfileId}' with {result.BrowserContinuationAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserContinuationAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
