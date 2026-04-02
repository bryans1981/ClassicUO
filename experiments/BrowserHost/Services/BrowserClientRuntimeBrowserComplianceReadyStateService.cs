namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComplianceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserComplianceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComplianceReadyStateService : IBrowserClientRuntimeBrowserComplianceReadyState
{
    private readonly IBrowserClientRuntimeBrowserComplianceSession _runtimeBrowserComplianceSession;

    public BrowserClientRuntimeBrowserComplianceReadyStateService(IBrowserClientRuntimeBrowserComplianceSession runtimeBrowserComplianceSession)
    {
        _runtimeBrowserComplianceSession = runtimeBrowserComplianceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComplianceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComplianceSessionResult complianceSession = await _runtimeBrowserComplianceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserComplianceReadyStateResult result = new()
        {
            ProfileId = complianceSession.ProfileId,
            SessionId = complianceSession.SessionId,
            SessionPath = complianceSession.SessionPath,
            BrowserComplianceSessionVersion = complianceSession.BrowserComplianceSessionVersion,
            BrowserSecurityReadyStateVersion = complianceSession.BrowserSecurityReadyStateVersion,
            BrowserSecuritySessionVersion = complianceSession.BrowserSecuritySessionVersion,
            LaunchMode = complianceSession.LaunchMode,
            AssetRootPath = complianceSession.AssetRootPath,
            ProfilesRootPath = complianceSession.ProfilesRootPath,
            CacheRootPath = complianceSession.CacheRootPath,
            ConfigRootPath = complianceSession.ConfigRootPath,
            SettingsFilePath = complianceSession.SettingsFilePath,
            StartupProfilePath = complianceSession.StartupProfilePath,
            RequiredAssets = complianceSession.RequiredAssets,
            ReadyAssetCount = complianceSession.ReadyAssetCount,
            CompletedSteps = complianceSession.CompletedSteps,
            TotalSteps = complianceSession.TotalSteps,
            Exists = complianceSession.Exists,
            ReadSucceeded = complianceSession.ReadSucceeded,
            BrowserComplianceSession = complianceSession
        };

        if (!complianceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser compliance ready state blocked for profile '{complianceSession.ProfileId}'.";
            result.Error = complianceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComplianceReadyStateVersion = "runtime-browser-compliance-ready-state-v1";
        result.BrowserComplianceReadyChecks =
        [
            "browser-security-ready-state-ready",
            "browser-compliance-session-ready",
            "browser-compliance-ready"
        ];
        result.BrowserComplianceReadySummary = $"Runtime browser compliance ready state passed {result.BrowserComplianceReadyChecks.Length} compliance readiness check(s) for profile '{complianceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser compliance ready state ready for profile '{complianceSession.ProfileId}' with {result.BrowserComplianceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComplianceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserComplianceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComplianceSessionVersion { get; set; } = string.Empty;
    public string BrowserSecurityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSecuritySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComplianceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserComplianceReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserComplianceSessionResult BrowserComplianceSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
