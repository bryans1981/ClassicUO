namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSecurityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSecurityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSecurityReadyStateService : IBrowserClientRuntimeBrowserSecurityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSecuritySession _runtimeBrowserSecuritySession;

    public BrowserClientRuntimeBrowserSecurityReadyStateService(IBrowserClientRuntimeBrowserSecuritySession runtimeBrowserSecuritySession)
    {
        _runtimeBrowserSecuritySession = runtimeBrowserSecuritySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSecurityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSecuritySessionResult securitySession = await _runtimeBrowserSecuritySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSecurityReadyStateResult result = new()
        {
            ProfileId = securitySession.ProfileId,
            SessionId = securitySession.SessionId,
            SessionPath = securitySession.SessionPath,
            BrowserSecuritySessionVersion = securitySession.BrowserSecuritySessionVersion,
            BrowserAuditReadyStateVersion = securitySession.BrowserAuditReadyStateVersion,
            BrowserAuditSessionVersion = securitySession.BrowserAuditSessionVersion,
            LaunchMode = securitySession.LaunchMode,
            AssetRootPath = securitySession.AssetRootPath,
            ProfilesRootPath = securitySession.ProfilesRootPath,
            CacheRootPath = securitySession.CacheRootPath,
            ConfigRootPath = securitySession.ConfigRootPath,
            SettingsFilePath = securitySession.SettingsFilePath,
            StartupProfilePath = securitySession.StartupProfilePath,
            RequiredAssets = securitySession.RequiredAssets,
            ReadyAssetCount = securitySession.ReadyAssetCount,
            CompletedSteps = securitySession.CompletedSteps,
            TotalSteps = securitySession.TotalSteps,
            Exists = securitySession.Exists,
            ReadSucceeded = securitySession.ReadSucceeded,
            BrowserSecuritySession = securitySession
        };

        if (!securitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser security ready state blocked for profile '{securitySession.ProfileId}'.";
            result.Error = securitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSecurityReadyStateVersion = "runtime-browser-security-ready-state-v1";
        result.BrowserSecurityReadyChecks =
        [
            "browser-audit-ready-state-ready",
            "browser-security-session-ready",
            "browser-security-ready"
        ];
        result.BrowserSecurityReadySummary = $"Runtime browser security ready state passed {result.BrowserSecurityReadyChecks.Length} security readiness check(s) for profile '{securitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser security ready state ready for profile '{securitySession.ProfileId}' with {result.BrowserSecurityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSecurityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSecurityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserSecurityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSecurityReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserSecuritySessionResult BrowserSecuritySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
