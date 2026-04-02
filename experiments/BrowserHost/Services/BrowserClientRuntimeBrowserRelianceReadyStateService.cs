namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRelianceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRelianceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRelianceReadyStateService : IBrowserClientRuntimeBrowserRelianceReadyState
{
    private readonly IBrowserClientRuntimeBrowserRelianceSession _runtimeBrowserRelianceSession;

    public BrowserClientRuntimeBrowserRelianceReadyStateService(IBrowserClientRuntimeBrowserRelianceSession runtimeBrowserRelianceSession)
    {
        _runtimeBrowserRelianceSession = runtimeBrowserRelianceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRelianceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRelianceSessionResult relianceSession = await _runtimeBrowserRelianceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRelianceReadyStateResult result = new()
        {
            ProfileId = relianceSession.ProfileId,
            SessionId = relianceSession.SessionId,
            SessionPath = relianceSession.SessionPath,
            BrowserRelianceSessionVersion = relianceSession.BrowserRelianceSessionVersion,
            BrowserTrustInUseReadyStateVersion = relianceSession.BrowserTrustInUseReadyStateVersion,
            BrowserTrustInUseSessionVersion = relianceSession.BrowserTrustInUseSessionVersion,
            LaunchMode = relianceSession.LaunchMode,
            AssetRootPath = relianceSession.AssetRootPath,
            ProfilesRootPath = relianceSession.ProfilesRootPath,
            CacheRootPath = relianceSession.CacheRootPath,
            ConfigRootPath = relianceSession.ConfigRootPath,
            SettingsFilePath = relianceSession.SettingsFilePath,
            StartupProfilePath = relianceSession.StartupProfilePath,
            RequiredAssets = relianceSession.RequiredAssets,
            ReadyAssetCount = relianceSession.ReadyAssetCount,
            CompletedSteps = relianceSession.CompletedSteps,
            TotalSteps = relianceSession.TotalSteps,
            Exists = relianceSession.Exists,
            ReadSucceeded = relianceSession.ReadSucceeded
        };

        if (!relianceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser reliance ready state blocked for profile '{relianceSession.ProfileId}'.";
            result.Error = relianceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRelianceReadyStateVersion = "runtime-browser-reliance-ready-state-v1";
        result.BrowserRelianceReadyChecks =
        [
            "browser-trustinuse-ready-state-ready",
            "browser-reliance-session-ready",
            "browser-reliance-ready"
        ];
        result.BrowserRelianceReadySummary = $"Runtime browser reliance ready state passed {result.BrowserRelianceReadyChecks.Length} reliance readiness check(s) for profile '{relianceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser reliance ready state ready for profile '{relianceSession.ProfileId}' with {result.BrowserRelianceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRelianceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRelianceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRelianceSessionVersion { get; set; } = string.Empty;
    public string BrowserTrustInUseReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustInUseSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRelianceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRelianceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
