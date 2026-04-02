namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComplianceSession
{
    ValueTask<BrowserClientRuntimeBrowserComplianceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComplianceSessionService : IBrowserClientRuntimeBrowserComplianceSession
{
    private readonly IBrowserClientRuntimeBrowserSecurityReadyState _runtimeBrowserSecurityReadyState;

    public BrowserClientRuntimeBrowserComplianceSessionService(IBrowserClientRuntimeBrowserSecurityReadyState runtimeBrowserSecurityReadyState)
    {
        _runtimeBrowserSecurityReadyState = runtimeBrowserSecurityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComplianceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSecurityReadyStateResult securityReadyState = await _runtimeBrowserSecurityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserComplianceSessionResult result = new()
        {
            ProfileId = securityReadyState.ProfileId,
            SessionId = securityReadyState.SessionId,
            SessionPath = securityReadyState.SessionPath,
            BrowserSecurityReadyStateVersion = securityReadyState.BrowserSecurityReadyStateVersion,
            BrowserSecuritySessionVersion = securityReadyState.BrowserSecuritySessionVersion,
            LaunchMode = securityReadyState.LaunchMode,
            AssetRootPath = securityReadyState.AssetRootPath,
            ProfilesRootPath = securityReadyState.ProfilesRootPath,
            CacheRootPath = securityReadyState.CacheRootPath,
            ConfigRootPath = securityReadyState.ConfigRootPath,
            SettingsFilePath = securityReadyState.SettingsFilePath,
            StartupProfilePath = securityReadyState.StartupProfilePath,
            RequiredAssets = securityReadyState.RequiredAssets,
            ReadyAssetCount = securityReadyState.ReadyAssetCount,
            CompletedSteps = securityReadyState.CompletedSteps,
            TotalSteps = securityReadyState.TotalSteps,
            Exists = securityReadyState.Exists,
            ReadSucceeded = securityReadyState.ReadSucceeded,
            BrowserSecurityReadyState = securityReadyState
        };

        if (!securityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser compliance session blocked for profile '{securityReadyState.ProfileId}'.";
            result.Error = securityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComplianceSessionVersion = "runtime-browser-compliance-session-v1";
        result.BrowserComplianceStages =
        [
            "open-browser-compliance-session",
            "bind-browser-security-ready-state",
            "publish-browser-compliance-ready"
        ];
        result.BrowserComplianceSummary = $"Runtime browser compliance session prepared {result.BrowserComplianceStages.Length} compliance stage(s) for profile '{securityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser compliance session ready for profile '{securityReadyState.ProfileId}' with {result.BrowserComplianceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComplianceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserComplianceStages { get; set; } = Array.Empty<string>();
    public string BrowserComplianceSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserSecurityReadyStateResult BrowserSecurityReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
