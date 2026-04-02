namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPolicyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPolicyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPolicyReadyStateService : IBrowserClientRuntimeBrowserPolicyReadyState
{
    private readonly IBrowserClientRuntimeBrowserPolicySession _runtimeBrowserPolicySession;

    public BrowserClientRuntimeBrowserPolicyReadyStateService(IBrowserClientRuntimeBrowserPolicySession runtimeBrowserPolicySession)
    {
        _runtimeBrowserPolicySession = runtimeBrowserPolicySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPolicyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPolicySessionResult policySession = await _runtimeBrowserPolicySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPolicyReadyStateResult result = new()
        {
            ProfileId = policySession.ProfileId,
            SessionId = policySession.SessionId,
            SessionPath = policySession.SessionPath,
            BrowserPolicySessionVersion = policySession.BrowserPolicySessionVersion,
            BrowserAlertingReadyStateVersion = policySession.BrowserAlertingReadyStateVersion,
            BrowserAlertingSessionVersion = policySession.BrowserAlertingSessionVersion,
            LaunchMode = policySession.LaunchMode,
            AssetRootPath = policySession.AssetRootPath,
            ProfilesRootPath = policySession.ProfilesRootPath,
            CacheRootPath = policySession.CacheRootPath,
            ConfigRootPath = policySession.ConfigRootPath,
            SettingsFilePath = policySession.SettingsFilePath,
            StartupProfilePath = policySession.StartupProfilePath,
            RequiredAssets = policySession.RequiredAssets,
            ReadyAssetCount = policySession.ReadyAssetCount,
            CompletedSteps = policySession.CompletedSteps,
            TotalSteps = policySession.TotalSteps,
            Exists = policySession.Exists,
            ReadSucceeded = policySession.ReadSucceeded,
            BrowserPolicySession = policySession
        };

        if (!policySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser policy ready state blocked for profile '{policySession.ProfileId}'.";
            result.Error = policySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPolicyReadyStateVersion = "runtime-browser-policy-ready-state-v1";
        result.BrowserPolicyReadyChecks =
        [
            "browser-alerting-ready-state-ready",
            "browser-policy-session-ready",
            "browser-policy-ready"
        ];
        result.BrowserPolicyReadySummary = $"Runtime browser policy ready state passed {result.BrowserPolicyReadyChecks.Length} policy readiness check(s) for profile '{policySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser policy ready state ready for profile '{policySession.ProfileId}' with {result.BrowserPolicyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPolicyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPolicyReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserPolicyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPolicyReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPolicySessionResult BrowserPolicySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
