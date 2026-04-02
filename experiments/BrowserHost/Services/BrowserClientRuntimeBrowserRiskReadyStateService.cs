namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRiskReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRiskReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRiskReadyStateService : IBrowserClientRuntimeBrowserRiskReadyState
{
    private readonly IBrowserClientRuntimeBrowserRiskSession _runtimeBrowserRiskSession;

    public BrowserClientRuntimeBrowserRiskReadyStateService(IBrowserClientRuntimeBrowserRiskSession runtimeBrowserRiskSession)
    {
        _runtimeBrowserRiskSession = runtimeBrowserRiskSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRiskReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRiskSessionResult riskSession = await _runtimeBrowserRiskSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRiskReadyStateResult result = new()
        {
            ProfileId = riskSession.ProfileId,
            SessionId = riskSession.SessionId,
            SessionPath = riskSession.SessionPath,
            BrowserRiskSessionVersion = riskSession.BrowserRiskSessionVersion,
            BrowserAssuranceReadyStateVersion = riskSession.BrowserAssuranceReadyStateVersion,
            BrowserAssuranceSessionVersion = riskSession.BrowserAssuranceSessionVersion,
            LaunchMode = riskSession.LaunchMode,
            AssetRootPath = riskSession.AssetRootPath,
            ProfilesRootPath = riskSession.ProfilesRootPath,
            CacheRootPath = riskSession.CacheRootPath,
            ConfigRootPath = riskSession.ConfigRootPath,
            SettingsFilePath = riskSession.SettingsFilePath,
            StartupProfilePath = riskSession.StartupProfilePath,
            RequiredAssets = riskSession.RequiredAssets,
            ReadyAssetCount = riskSession.ReadyAssetCount,
            CompletedSteps = riskSession.CompletedSteps,
            TotalSteps = riskSession.TotalSteps,
            Exists = riskSession.Exists,
            ReadSucceeded = riskSession.ReadSucceeded,
            BrowserRiskSession = riskSession
        };

        if (!riskSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser risk ready state blocked for profile '{riskSession.ProfileId}'.";
            result.Error = riskSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRiskReadyStateVersion = "runtime-browser-risk-ready-state-v1";
        result.BrowserRiskReadyChecks =
        [
            "browser-assurance-ready-state-ready",
            "browser-risk-session-ready",
            "browser-risk-ready"
        ];
        result.BrowserRiskReadySummary = $"Runtime browser risk ready state passed {result.BrowserRiskReadyChecks.Length} risk readiness check(s) for profile '{riskSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser risk ready state ready for profile '{riskSession.ProfileId}' with {result.BrowserRiskReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRiskReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRiskReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRiskSessionVersion { get; set; } = string.Empty;
    public string BrowserAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRiskReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRiskReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRiskSessionResult BrowserRiskSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
