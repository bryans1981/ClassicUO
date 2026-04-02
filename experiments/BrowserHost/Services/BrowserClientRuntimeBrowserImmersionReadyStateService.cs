namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserImmersionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserImmersionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserImmersionReadyStateService : IBrowserClientRuntimeBrowserImmersionReadyState
{
    private readonly IBrowserClientRuntimeBrowserImmersionSession _runtimeBrowserImmersionSession;

    public BrowserClientRuntimeBrowserImmersionReadyStateService(IBrowserClientRuntimeBrowserImmersionSession runtimeBrowserImmersionSession)
    {
        _runtimeBrowserImmersionSession = runtimeBrowserImmersionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserImmersionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserImmersionSessionResult immersionSession = await _runtimeBrowserImmersionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserImmersionReadyStateResult result = new()
        {
            ProfileId = immersionSession.ProfileId,
            SessionId = immersionSession.SessionId,
            SessionPath = immersionSession.SessionPath,
            BrowserImmersionSessionVersion = immersionSession.BrowserImmersionSessionVersion,
            BrowserEngagementReadyStateVersion = immersionSession.BrowserEngagementReadyStateVersion,
            BrowserEngagementSessionVersion = immersionSession.BrowserEngagementSessionVersion,
            LaunchMode = immersionSession.LaunchMode,
            AssetRootPath = immersionSession.AssetRootPath,
            ProfilesRootPath = immersionSession.ProfilesRootPath,
            CacheRootPath = immersionSession.CacheRootPath,
            ConfigRootPath = immersionSession.ConfigRootPath,
            SettingsFilePath = immersionSession.SettingsFilePath,
            StartupProfilePath = immersionSession.StartupProfilePath,
            RequiredAssets = immersionSession.RequiredAssets,
            ReadyAssetCount = immersionSession.ReadyAssetCount,
            CompletedSteps = immersionSession.CompletedSteps,
            TotalSteps = immersionSession.TotalSteps,
            Exists = immersionSession.Exists,
            ReadSucceeded = immersionSession.ReadSucceeded
        };

        if (!immersionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser immersion ready state blocked for profile '{immersionSession.ProfileId}'.";
            result.Error = immersionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserImmersionReadyStateVersion = "runtime-browser-immersion-ready-state-v1";
        result.BrowserImmersionReadyChecks =
        [
            "browser-engagement-ready-state-ready",
            "browser-immersion-session-ready",
            "browser-immersion-ready"
        ];
        result.BrowserImmersionReadySummary = $"Runtime browser immersion ready state passed {result.BrowserImmersionReadyChecks.Length} immersion readiness check(s) for profile '{immersionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser immersion ready state ready for profile '{immersionSession.ProfileId}' with {result.BrowserImmersionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserImmersionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserImmersionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserImmersionSessionVersion { get; set; } = string.Empty;
    public string BrowserEngagementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEngagementSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserImmersionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserImmersionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
