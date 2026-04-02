namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSecuritySession
{
    ValueTask<BrowserClientRuntimeBrowserSecuritySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSecuritySessionService : IBrowserClientRuntimeBrowserSecuritySession
{
    private readonly IBrowserClientRuntimeBrowserAuditReadyState _runtimeBrowserAuditReadyState;

    public BrowserClientRuntimeBrowserSecuritySessionService(IBrowserClientRuntimeBrowserAuditReadyState runtimeBrowserAuditReadyState)
    {
        _runtimeBrowserAuditReadyState = runtimeBrowserAuditReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSecuritySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAuditReadyStateResult auditReadyState = await _runtimeBrowserAuditReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSecuritySessionResult result = new()
        {
            ProfileId = auditReadyState.ProfileId,
            SessionId = auditReadyState.SessionId,
            SessionPath = auditReadyState.SessionPath,
            BrowserAuditReadyStateVersion = auditReadyState.BrowserAuditReadyStateVersion,
            BrowserAuditSessionVersion = auditReadyState.BrowserAuditSessionVersion,
            LaunchMode = auditReadyState.LaunchMode,
            AssetRootPath = auditReadyState.AssetRootPath,
            ProfilesRootPath = auditReadyState.ProfilesRootPath,
            CacheRootPath = auditReadyState.CacheRootPath,
            ConfigRootPath = auditReadyState.ConfigRootPath,
            SettingsFilePath = auditReadyState.SettingsFilePath,
            StartupProfilePath = auditReadyState.StartupProfilePath,
            RequiredAssets = auditReadyState.RequiredAssets,
            ReadyAssetCount = auditReadyState.ReadyAssetCount,
            CompletedSteps = auditReadyState.CompletedSteps,
            TotalSteps = auditReadyState.TotalSteps,
            Exists = auditReadyState.Exists,
            ReadSucceeded = auditReadyState.ReadSucceeded,
            BrowserAuditReadyState = auditReadyState
        };

        if (!auditReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser security session blocked for profile '{auditReadyState.ProfileId}'.";
            result.Error = auditReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSecuritySessionVersion = "runtime-browser-security-session-v1";
        result.BrowserSecurityStages =
        [
            "open-browser-security-session",
            "bind-browser-audit-ready-state",
            "publish-browser-security-ready"
        ];
        result.BrowserSecuritySummary = $"Runtime browser security session prepared {result.BrowserSecurityStages.Length} security stage(s) for profile '{auditReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser security session ready for profile '{auditReadyState.ProfileId}' with {result.BrowserSecurityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSecuritySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSecuritySessionVersion { get; set; } = string.Empty;
    public string BrowserAuditReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAuditSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSecurityStages { get; set; } = Array.Empty<string>();
    public string BrowserSecuritySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAuditReadyStateResult BrowserAuditReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
