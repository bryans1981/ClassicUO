namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTrustInUseReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTrustInUseReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTrustInUseReadyStateService : IBrowserClientRuntimeBrowserTrustInUseReadyState
{
    private readonly IBrowserClientRuntimeBrowserTrustInUseSession _runtimeBrowserTrustInUseSession;

    public BrowserClientRuntimeBrowserTrustInUseReadyStateService(IBrowserClientRuntimeBrowserTrustInUseSession runtimeBrowserTrustInUseSession)
    {
        _runtimeBrowserTrustInUseSession = runtimeBrowserTrustInUseSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTrustInUseReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTrustInUseSessionResult trustinuseSession = await _runtimeBrowserTrustInUseSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTrustInUseReadyStateResult result = new()
        {
            ProfileId = trustinuseSession.ProfileId,
            SessionId = trustinuseSession.SessionId,
            SessionPath = trustinuseSession.SessionPath,
            BrowserTrustInUseSessionVersion = trustinuseSession.BrowserTrustInUseSessionVersion,
            BrowserSuccessConfidenceReadyStateVersion = trustinuseSession.BrowserSuccessConfidenceReadyStateVersion,
            BrowserSuccessConfidenceSessionVersion = trustinuseSession.BrowserSuccessConfidenceSessionVersion,
            LaunchMode = trustinuseSession.LaunchMode,
            AssetRootPath = trustinuseSession.AssetRootPath,
            ProfilesRootPath = trustinuseSession.ProfilesRootPath,
            CacheRootPath = trustinuseSession.CacheRootPath,
            ConfigRootPath = trustinuseSession.ConfigRootPath,
            SettingsFilePath = trustinuseSession.SettingsFilePath,
            StartupProfilePath = trustinuseSession.StartupProfilePath,
            RequiredAssets = trustinuseSession.RequiredAssets,
            ReadyAssetCount = trustinuseSession.ReadyAssetCount,
            CompletedSteps = trustinuseSession.CompletedSteps,
            TotalSteps = trustinuseSession.TotalSteps,
            Exists = trustinuseSession.Exists,
            ReadSucceeded = trustinuseSession.ReadSucceeded
        };

        if (!trustinuseSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser trustinuse ready state blocked for profile '{trustinuseSession.ProfileId}'.";
            result.Error = trustinuseSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTrustInUseReadyStateVersion = "runtime-browser-trustinuse-ready-state-v1";
        result.BrowserTrustInUseReadyChecks =
        [
            "browser-successconfidence-ready-state-ready",
            "browser-trustinuse-session-ready",
            "browser-trustinuse-ready"
        ];
        result.BrowserTrustInUseReadySummary = $"Runtime browser trustinuse ready state passed {result.BrowserTrustInUseReadyChecks.Length} trustinuse readiness check(s) for profile '{trustinuseSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser trustinuse ready state ready for profile '{trustinuseSession.ProfileId}' with {result.BrowserTrustInUseReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTrustInUseReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTrustInUseReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustInUseSessionVersion { get; set; } = string.Empty;
    public string BrowserSuccessConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSuccessConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTrustInUseReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTrustInUseReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
