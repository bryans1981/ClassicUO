namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAlertingSession
{
    ValueTask<BrowserClientRuntimeBrowserAlertingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAlertingSessionService : IBrowserClientRuntimeBrowserAlertingSession
{
    private readonly IBrowserClientRuntimeBrowserHealthReadyState _runtimeBrowserHealthReadyState;

    public BrowserClientRuntimeBrowserAlertingSessionService(IBrowserClientRuntimeBrowserHealthReadyState runtimeBrowserHealthReadyState)
    {
        _runtimeBrowserHealthReadyState = runtimeBrowserHealthReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAlertingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserHealthReadyStateResult healthReadyState = await _runtimeBrowserHealthReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAlertingSessionResult result = new()
        {
            ProfileId = healthReadyState.ProfileId,
            SessionId = healthReadyState.SessionId,
            SessionPath = healthReadyState.SessionPath,
            BrowserHealthReadyStateVersion = healthReadyState.BrowserHealthReadyStateVersion,
            BrowserHealthSessionVersion = healthReadyState.BrowserHealthSessionVersion,
            LaunchMode = healthReadyState.LaunchMode,
            AssetRootPath = healthReadyState.AssetRootPath,
            ProfilesRootPath = healthReadyState.ProfilesRootPath,
            CacheRootPath = healthReadyState.CacheRootPath,
            ConfigRootPath = healthReadyState.ConfigRootPath,
            SettingsFilePath = healthReadyState.SettingsFilePath,
            StartupProfilePath = healthReadyState.StartupProfilePath,
            RequiredAssets = healthReadyState.RequiredAssets,
            ReadyAssetCount = healthReadyState.ReadyAssetCount,
            CompletedSteps = healthReadyState.CompletedSteps,
            TotalSteps = healthReadyState.TotalSteps,
            Exists = healthReadyState.Exists,
            ReadSucceeded = healthReadyState.ReadSucceeded,
            BrowserHealthReadyState = healthReadyState
        };

        if (!healthReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser alerting session blocked for profile '{healthReadyState.ProfileId}'.";
            result.Error = healthReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAlertingSessionVersion = "runtime-browser-alerting-session-v1";
        result.BrowserAlertingStages =
        [
            "open-browser-alerting-session",
            "bind-browser-health-ready-state",
            "publish-browser-alerting-ready"
        ];
        result.BrowserAlertingSummary = $"Runtime browser alerting session prepared {result.BrowserAlertingStages.Length} alerting stage(s) for profile '{healthReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser alerting session ready for profile '{healthReadyState.ProfileId}' with {result.BrowserAlertingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAlertingSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAlertingSessionVersion { get; set; } = string.Empty;
    public string BrowserHealthReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHealthSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAlertingStages { get; set; } = Array.Empty<string>();
    public string BrowserAlertingSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserHealthReadyStateResult BrowserHealthReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
