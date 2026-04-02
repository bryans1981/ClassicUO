namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserHealthSession
{
    ValueTask<BrowserClientRuntimeBrowserHealthSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserHealthSessionService : IBrowserClientRuntimeBrowserHealthSession
{
    private readonly IBrowserClientRuntimeBrowserWatchdogReadyState _runtimeBrowserWatchdogReadyState;

    public BrowserClientRuntimeBrowserHealthSessionService(IBrowserClientRuntimeBrowserWatchdogReadyState runtimeBrowserWatchdogReadyState)
    {
        _runtimeBrowserWatchdogReadyState = runtimeBrowserWatchdogReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserHealthSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserWatchdogReadyStateResult watchdogReadyState = await _runtimeBrowserWatchdogReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserHealthSessionResult result = new()
        {
            ProfileId = watchdogReadyState.ProfileId,
            SessionId = watchdogReadyState.SessionId,
            SessionPath = watchdogReadyState.SessionPath,
            BrowserWatchdogReadyStateVersion = watchdogReadyState.BrowserWatchdogReadyStateVersion,
            BrowserWatchdogSessionVersion = watchdogReadyState.BrowserWatchdogSessionVersion,
            LaunchMode = watchdogReadyState.LaunchMode,
            AssetRootPath = watchdogReadyState.AssetRootPath,
            ProfilesRootPath = watchdogReadyState.ProfilesRootPath,
            CacheRootPath = watchdogReadyState.CacheRootPath,
            ConfigRootPath = watchdogReadyState.ConfigRootPath,
            SettingsFilePath = watchdogReadyState.SettingsFilePath,
            StartupProfilePath = watchdogReadyState.StartupProfilePath,
            RequiredAssets = watchdogReadyState.RequiredAssets,
            ReadyAssetCount = watchdogReadyState.ReadyAssetCount,
            CompletedSteps = watchdogReadyState.CompletedSteps,
            TotalSteps = watchdogReadyState.TotalSteps,
            Exists = watchdogReadyState.Exists,
            ReadSucceeded = watchdogReadyState.ReadSucceeded,
            BrowserWatchdogReadyState = watchdogReadyState
        };

        if (!watchdogReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser health session blocked for profile '{watchdogReadyState.ProfileId}'.";
            result.Error = watchdogReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserHealthSessionVersion = "runtime-browser-health-session-v1";
        result.BrowserHealthStages =
        [
            "open-browser-health-session",
            "bind-browser-watchdog-ready-state",
            "publish-browser-health-ready"
        ];
        result.BrowserHealthSummary = $"Runtime browser health session prepared {result.BrowserHealthStages.Length} health stage(s) for profile '{watchdogReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser health session ready for profile '{watchdogReadyState.ProfileId}' with {result.BrowserHealthStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserHealthSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserHealthStages { get; set; } = Array.Empty<string>();
    public string BrowserHealthSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserWatchdogReadyStateResult BrowserWatchdogReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
