namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserContinuationResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationResilienceReadyStateService : IBrowserClientRuntimeBrowserContinuationResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserContinuationResilienceSession _runtimeBrowserContinuationResilienceSession;

    public BrowserClientRuntimeBrowserContinuationResilienceReadyStateService(IBrowserClientRuntimeBrowserContinuationResilienceSession runtimeBrowserContinuationResilienceSession)
    {
        _runtimeBrowserContinuationResilienceSession = runtimeBrowserContinuationResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationResilienceSessionResult continuationresilienceSession = await _runtimeBrowserContinuationResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserContinuationResilienceReadyStateResult result = new()
        {
            ProfileId = continuationresilienceSession.ProfileId,
            SessionId = continuationresilienceSession.SessionId,
            SessionPath = continuationresilienceSession.SessionPath,
            BrowserContinuationResilienceSessionVersion = continuationresilienceSession.BrowserContinuationResilienceSessionVersion,
            BrowserFlowResilienceReadyStateVersion = continuationresilienceSession.BrowserFlowResilienceReadyStateVersion,
            BrowserFlowResilienceSessionVersion = continuationresilienceSession.BrowserFlowResilienceSessionVersion,
            LaunchMode = continuationresilienceSession.LaunchMode,
            AssetRootPath = continuationresilienceSession.AssetRootPath,
            ProfilesRootPath = continuationresilienceSession.ProfilesRootPath,
            CacheRootPath = continuationresilienceSession.CacheRootPath,
            ConfigRootPath = continuationresilienceSession.ConfigRootPath,
            SettingsFilePath = continuationresilienceSession.SettingsFilePath,
            StartupProfilePath = continuationresilienceSession.StartupProfilePath,
            RequiredAssets = continuationresilienceSession.RequiredAssets,
            ReadyAssetCount = continuationresilienceSession.ReadyAssetCount,
            CompletedSteps = continuationresilienceSession.CompletedSteps,
            TotalSteps = continuationresilienceSession.TotalSteps,
            Exists = continuationresilienceSession.Exists,
            ReadSucceeded = continuationresilienceSession.ReadSucceeded
        };

        if (!continuationresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser continuationresilience ready state blocked for profile '{continuationresilienceSession.ProfileId}'.";
            result.Error = continuationresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationResilienceReadyStateVersion = "runtime-browser-continuationresilience-ready-state-v1";
        result.BrowserContinuationResilienceReadyChecks =
        [
            "browser-flowresilience-ready-state-ready",
            "browser-continuationresilience-session-ready",
            "browser-continuationresilience-ready"
        ];
        result.BrowserContinuationResilienceReadySummary = $"Runtime browser continuationresilience ready state passed {result.BrowserContinuationResilienceReadyChecks.Length} continuationresilience readiness check(s) for profile '{continuationresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationresilience ready state ready for profile '{continuationresilienceSession.ProfileId}' with {result.BrowserContinuationResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserFlowResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserContinuationResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
