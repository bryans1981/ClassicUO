namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAuditReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAuditReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAuditReadyStateService : IBrowserClientRuntimeBrowserAuditReadyState
{
    private readonly IBrowserClientRuntimeBrowserAuditSession _runtimeBrowserAuditSession;

    public BrowserClientRuntimeBrowserAuditReadyStateService(IBrowserClientRuntimeBrowserAuditSession runtimeBrowserAuditSession)
    {
        _runtimeBrowserAuditSession = runtimeBrowserAuditSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAuditReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAuditSessionResult auditSession = await _runtimeBrowserAuditSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAuditReadyStateResult result = new()
        {
            ProfileId = auditSession.ProfileId,
            SessionId = auditSession.SessionId,
            SessionPath = auditSession.SessionPath,
            BrowserAuditSessionVersion = auditSession.BrowserAuditSessionVersion,
            BrowserPolicyReadyStateVersion = auditSession.BrowserPolicyReadyStateVersion,
            BrowserPolicySessionVersion = auditSession.BrowserPolicySessionVersion,
            LaunchMode = auditSession.LaunchMode,
            AssetRootPath = auditSession.AssetRootPath,
            ProfilesRootPath = auditSession.ProfilesRootPath,
            CacheRootPath = auditSession.CacheRootPath,
            ConfigRootPath = auditSession.ConfigRootPath,
            SettingsFilePath = auditSession.SettingsFilePath,
            StartupProfilePath = auditSession.StartupProfilePath,
            RequiredAssets = auditSession.RequiredAssets,
            ReadyAssetCount = auditSession.ReadyAssetCount,
            CompletedSteps = auditSession.CompletedSteps,
            TotalSteps = auditSession.TotalSteps,
            Exists = auditSession.Exists,
            ReadSucceeded = auditSession.ReadSucceeded,
            BrowserAuditSession = auditSession
        };

        if (!auditSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser audit ready state blocked for profile '{auditSession.ProfileId}'.";
            result.Error = auditSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAuditReadyStateVersion = "runtime-browser-audit-ready-state-v1";
        result.BrowserAuditReadyChecks =
        [
            "browser-policy-ready-state-ready",
            "browser-audit-session-ready",
            "browser-audit-ready"
        ];
        result.BrowserAuditReadySummary = $"Runtime browser audit ready state passed {result.BrowserAuditReadyChecks.Length} audit readiness check(s) for profile '{auditSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser audit ready state ready for profile '{auditSession.ProfileId}' with {result.BrowserAuditReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAuditReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAuditReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAuditSessionVersion { get; set; } = string.Empty;
    public string BrowserPolicyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPolicySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAuditReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAuditReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAuditSessionResult BrowserAuditSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
