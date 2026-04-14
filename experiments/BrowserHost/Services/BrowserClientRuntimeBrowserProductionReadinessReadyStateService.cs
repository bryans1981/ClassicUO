namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionReadinessReadyStateService : IBrowserClientRuntimeBrowserProductionReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionReadinessSession _runtimeBrowserProductionReadinessSession;

    public BrowserClientRuntimeBrowserProductionReadinessReadyStateService(IBrowserClientRuntimeBrowserProductionReadinessSession runtimeBrowserProductionReadinessSession)
    {
        _runtimeBrowserProductionReadinessSession = runtimeBrowserProductionReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionReadinessSessionResult productionreadinessSession = await _runtimeBrowserProductionReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserProductionReadinessReadyStateResult result = new()
        {
            ProfileId = productionreadinessSession.ProfileId,
            SessionId = productionreadinessSession.SessionId,
            SessionPath = productionreadinessSession.SessionPath,
            BrowserProductionReadinessSessionVersion = productionreadinessSession.BrowserProductionReadinessSessionVersion,
            BrowserServiceAssuranceReadyStateVersion = productionreadinessSession.BrowserServiceAssuranceReadyStateVersion,
            BrowserServiceAssuranceSessionVersion = productionreadinessSession.BrowserServiceAssuranceSessionVersion,
            LaunchMode = productionreadinessSession.LaunchMode,
            AssetRootPath = productionreadinessSession.AssetRootPath,
            ProfilesRootPath = productionreadinessSession.ProfilesRootPath,
            CacheRootPath = productionreadinessSession.CacheRootPath,
            ConfigRootPath = productionreadinessSession.ConfigRootPath,
            SettingsFilePath = productionreadinessSession.SettingsFilePath,
            StartupProfilePath = productionreadinessSession.StartupProfilePath,
            RequiredAssets = productionreadinessSession.RequiredAssets,
            ReadyAssetCount = productionreadinessSession.ReadyAssetCount,
            CompletedSteps = productionreadinessSession.CompletedSteps,
            TotalSteps = productionreadinessSession.TotalSteps,
            Exists = productionreadinessSession.Exists,
            ReadSucceeded = productionreadinessSession.ReadSucceeded
        };

        if (!productionreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionreadiness ready state blocked for profile '{productionreadinessSession.ProfileId}'.";
            result.Error = productionreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionReadinessReadyStateVersion = "runtime-browser-productionreadiness-ready-state-v1";
        result.BrowserProductionReadinessReadyChecks =
        [
            "browser-serviceassurance-ready-state-ready",
            "browser-productionreadiness-session-ready",
            "browser-productionreadiness-ready"
        ];
        result.BrowserProductionReadinessReadySummary = $"Runtime browser productionreadiness ready state passed {result.BrowserProductionReadinessReadyChecks.Length} productionreadiness readiness check(s) for profile '{productionreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionreadiness ready state ready for profile '{productionreadinessSession.ProfileId}' with {result.BrowserProductionReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserServiceAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
