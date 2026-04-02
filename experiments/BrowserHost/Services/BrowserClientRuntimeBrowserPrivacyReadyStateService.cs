namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPrivacyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPrivacyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPrivacyReadyStateService : IBrowserClientRuntimeBrowserPrivacyReadyState
{
    private readonly IBrowserClientRuntimeBrowserPrivacySession _runtimeBrowserPrivacySession;

    public BrowserClientRuntimeBrowserPrivacyReadyStateService(IBrowserClientRuntimeBrowserPrivacySession runtimeBrowserPrivacySession)
    {
        _runtimeBrowserPrivacySession = runtimeBrowserPrivacySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPrivacyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPrivacySessionResult privacySession = await _runtimeBrowserPrivacySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPrivacyReadyStateResult result = new()
        {
            ProfileId = privacySession.ProfileId,
            SessionId = privacySession.SessionId,
            SessionPath = privacySession.SessionPath,
            BrowserPrivacySessionVersion = privacySession.BrowserPrivacySessionVersion,
            BrowserComplianceReadyStateVersion = privacySession.BrowserComplianceReadyStateVersion,
            BrowserComplianceSessionVersion = privacySession.BrowserComplianceSessionVersion,
            LaunchMode = privacySession.LaunchMode,
            AssetRootPath = privacySession.AssetRootPath,
            ProfilesRootPath = privacySession.ProfilesRootPath,
            CacheRootPath = privacySession.CacheRootPath,
            ConfigRootPath = privacySession.ConfigRootPath,
            SettingsFilePath = privacySession.SettingsFilePath,
            StartupProfilePath = privacySession.StartupProfilePath,
            RequiredAssets = privacySession.RequiredAssets,
            ReadyAssetCount = privacySession.ReadyAssetCount,
            CompletedSteps = privacySession.CompletedSteps,
            TotalSteps = privacySession.TotalSteps,
            Exists = privacySession.Exists,
            ReadSucceeded = privacySession.ReadSucceeded,
            BrowserPrivacySession = privacySession
        };

        if (!privacySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser privacy ready state blocked for profile '{privacySession.ProfileId}'.";
            result.Error = privacySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPrivacyReadyStateVersion = "runtime-browser-privacy-ready-state-v1";
        result.BrowserPrivacyReadyChecks =
        [
            "browser-compliance-ready-state-ready",
            "browser-privacy-session-ready",
            "browser-privacy-ready"
        ];
        result.BrowserPrivacyReadySummary = $"Runtime browser privacy ready state passed {result.BrowserPrivacyReadyChecks.Length} privacy readiness check(s) for profile '{privacySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser privacy ready state ready for profile '{privacySession.ProfileId}' with {result.BrowserPrivacyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPrivacyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPrivacyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPrivacySessionVersion { get; set; } = string.Empty;
    public string BrowserComplianceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComplianceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPrivacyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPrivacyReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPrivacySessionResult BrowserPrivacySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
