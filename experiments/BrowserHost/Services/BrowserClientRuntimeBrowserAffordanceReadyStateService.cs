namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAffordanceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAffordanceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAffordanceReadyStateService : IBrowserClientRuntimeBrowserAffordanceReadyState
{
    private readonly IBrowserClientRuntimeBrowserAffordanceSession _runtimeBrowserAffordanceSession;

    public BrowserClientRuntimeBrowserAffordanceReadyStateService(IBrowserClientRuntimeBrowserAffordanceSession runtimeBrowserAffordanceSession)
    {
        _runtimeBrowserAffordanceSession = runtimeBrowserAffordanceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAffordanceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAffordanceSessionResult affordanceSession = await _runtimeBrowserAffordanceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAffordanceReadyStateResult result = new()
        {
            ProfileId = affordanceSession.ProfileId,
            SessionId = affordanceSession.SessionId,
            SessionPath = affordanceSession.SessionPath,
            BrowserAffordanceSessionVersion = affordanceSession.BrowserAffordanceSessionVersion,
            BrowserImmersionReadyStateVersion = affordanceSession.BrowserImmersionReadyStateVersion,
            BrowserImmersionSessionVersion = affordanceSession.BrowserImmersionSessionVersion,
            LaunchMode = affordanceSession.LaunchMode,
            AssetRootPath = affordanceSession.AssetRootPath,
            ProfilesRootPath = affordanceSession.ProfilesRootPath,
            CacheRootPath = affordanceSession.CacheRootPath,
            ConfigRootPath = affordanceSession.ConfigRootPath,
            SettingsFilePath = affordanceSession.SettingsFilePath,
            StartupProfilePath = affordanceSession.StartupProfilePath,
            RequiredAssets = affordanceSession.RequiredAssets,
            ReadyAssetCount = affordanceSession.ReadyAssetCount,
            CompletedSteps = affordanceSession.CompletedSteps,
            TotalSteps = affordanceSession.TotalSteps,
            Exists = affordanceSession.Exists,
            ReadSucceeded = affordanceSession.ReadSucceeded
        };

        if (!affordanceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser affordance ready state blocked for profile '{affordanceSession.ProfileId}'.";
            result.Error = affordanceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAffordanceReadyStateVersion = "runtime-browser-affordance-ready-state-v1";
        result.BrowserAffordanceReadyChecks =
        [
            "browser-immersion-ready-state-ready",
            "browser-affordance-session-ready",
            "browser-affordance-ready"
        ];
        result.BrowserAffordanceReadySummary = $"Runtime browser affordance ready state passed {result.BrowserAffordanceReadyChecks.Length} affordance readiness check(s) for profile '{affordanceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser affordance ready state ready for profile '{affordanceSession.ProfileId}' with {result.BrowserAffordanceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAffordanceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAffordanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAffordanceSessionVersion { get; set; } = string.Empty;
    public string BrowserImmersionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserImmersionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAffordanceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAffordanceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
