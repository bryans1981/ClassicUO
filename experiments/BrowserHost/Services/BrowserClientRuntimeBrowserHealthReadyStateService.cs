namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserHealthReadyState
{
    ValueTask<BrowserClientRuntimeBrowserHealthReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserHealthReadyStateService : IBrowserClientRuntimeBrowserHealthReadyState
{
    private readonly IBrowserClientRuntimeBrowserHealthSession _runtimeBrowserHealthSession;

    public BrowserClientRuntimeBrowserHealthReadyStateService(IBrowserClientRuntimeBrowserHealthSession runtimeBrowserHealthSession)
    {
        _runtimeBrowserHealthSession = runtimeBrowserHealthSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserHealthReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserHealthSessionResult healthSession = await _runtimeBrowserHealthSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserHealthReadyStateResult result = new()
        {
            ProfileId = healthSession.ProfileId,
            SessionId = healthSession.SessionId,
            SessionPath = healthSession.SessionPath,
            BrowserHealthSessionVersion = healthSession.BrowserHealthSessionVersion,
            BrowserWatchdogReadyStateVersion = healthSession.BrowserWatchdogReadyStateVersion,
            BrowserWatchdogSessionVersion = healthSession.BrowserWatchdogSessionVersion,
            LaunchMode = healthSession.LaunchMode,
            AssetRootPath = healthSession.AssetRootPath,
            ProfilesRootPath = healthSession.ProfilesRootPath,
            CacheRootPath = healthSession.CacheRootPath,
            ConfigRootPath = healthSession.ConfigRootPath,
            SettingsFilePath = healthSession.SettingsFilePath,
            StartupProfilePath = healthSession.StartupProfilePath,
            RequiredAssets = healthSession.RequiredAssets,
            ReadyAssetCount = healthSession.ReadyAssetCount,
            CompletedSteps = healthSession.CompletedSteps,
            TotalSteps = healthSession.TotalSteps,
            Exists = healthSession.Exists,
            ReadSucceeded = healthSession.ReadSucceeded,
            BrowserHealthSession = healthSession
        };

        if (!healthSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser health ready state blocked for profile '{healthSession.ProfileId}'.";
            result.Error = healthSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserHealthReadyStateVersion = "runtime-browser-health-ready-state-v1";
        result.BrowserHealthReadyChecks =
        [
            "browser-watchdog-ready-state-ready",
            "browser-health-session-ready",
            "browser-health-ready"
        ];
        result.BrowserHealthReadySummary = $"Runtime browser health ready state passed {result.BrowserHealthReadyChecks.Length} health readiness check(s) for profile '{healthSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser health ready state ready for profile '{healthSession.ProfileId}' with {result.BrowserHealthReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserHealthReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserHealthReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHealthSessionVersion { get; set; } = string.Empty;
    public string BrowserWatchdogReadyStateVersion { get; set; } = string.Empty;
    public string BrowserWatchdogSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserHealthReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserHealthReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserHealthSessionResult BrowserHealthSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
