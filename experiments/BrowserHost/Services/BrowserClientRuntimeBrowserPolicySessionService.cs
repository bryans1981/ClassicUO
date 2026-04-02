namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPolicySession
{
    ValueTask<BrowserClientRuntimeBrowserPolicySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPolicySessionService : IBrowserClientRuntimeBrowserPolicySession
{
    private readonly IBrowserClientRuntimeBrowserAlertingReadyState _runtimeBrowserAlertingReadyState;

    public BrowserClientRuntimeBrowserPolicySessionService(IBrowserClientRuntimeBrowserAlertingReadyState runtimeBrowserAlertingReadyState)
    {
        _runtimeBrowserAlertingReadyState = runtimeBrowserAlertingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPolicySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAlertingReadyStateResult alertingReadyState = await _runtimeBrowserAlertingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPolicySessionResult result = new()
        {
            ProfileId = alertingReadyState.ProfileId,
            SessionId = alertingReadyState.SessionId,
            SessionPath = alertingReadyState.SessionPath,
            BrowserAlertingReadyStateVersion = alertingReadyState.BrowserAlertingReadyStateVersion,
            BrowserAlertingSessionVersion = alertingReadyState.BrowserAlertingSessionVersion,
            LaunchMode = alertingReadyState.LaunchMode,
            AssetRootPath = alertingReadyState.AssetRootPath,
            ProfilesRootPath = alertingReadyState.ProfilesRootPath,
            CacheRootPath = alertingReadyState.CacheRootPath,
            ConfigRootPath = alertingReadyState.ConfigRootPath,
            SettingsFilePath = alertingReadyState.SettingsFilePath,
            StartupProfilePath = alertingReadyState.StartupProfilePath,
            RequiredAssets = alertingReadyState.RequiredAssets,
            ReadyAssetCount = alertingReadyState.ReadyAssetCount,
            CompletedSteps = alertingReadyState.CompletedSteps,
            TotalSteps = alertingReadyState.TotalSteps,
            Exists = alertingReadyState.Exists,
            ReadSucceeded = alertingReadyState.ReadSucceeded,
            BrowserAlertingReadyState = alertingReadyState
        };

        if (!alertingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser policy session blocked for profile '{alertingReadyState.ProfileId}'.";
            result.Error = alertingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPolicySessionVersion = "runtime-browser-policy-session-v1";
        result.BrowserPolicyStages =
        [
            "open-browser-policy-session",
            "bind-browser-alerting-ready-state",
            "publish-browser-policy-ready"
        ];
        result.BrowserPolicySummary = $"Runtime browser policy session prepared {result.BrowserPolicyStages.Length} policy stage(s) for profile '{alertingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser policy session ready for profile '{alertingReadyState.ProfileId}' with {result.BrowserPolicyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPolicySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPolicySessionVersion { get; set; } = string.Empty;
    public string BrowserAlertingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAlertingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPolicyStages { get; set; } = Array.Empty<string>();
    public string BrowserPolicySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAlertingReadyStateResult BrowserAlertingReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
