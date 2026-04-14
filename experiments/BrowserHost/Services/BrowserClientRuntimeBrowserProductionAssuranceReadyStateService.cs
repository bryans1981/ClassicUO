namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionAssuranceReadyStateService : IBrowserClientRuntimeBrowserProductionAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionAssuranceSession _runtimeBrowserProductionAssuranceSession;

    public BrowserClientRuntimeBrowserProductionAssuranceReadyStateService(IBrowserClientRuntimeBrowserProductionAssuranceSession runtimeBrowserProductionAssuranceSession)
    {
        _runtimeBrowserProductionAssuranceSession = runtimeBrowserProductionAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionAssuranceSessionResult productionassuranceSession = await _runtimeBrowserProductionAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserProductionAssuranceReadyStateResult result = new()
        {
            ProfileId = productionassuranceSession.ProfileId,
            SessionId = productionassuranceSession.SessionId,
            SessionPath = productionassuranceSession.SessionPath,
            BrowserProductionAssuranceSessionVersion = productionassuranceSession.BrowserProductionAssuranceSessionVersion,
            BrowserProductionReadinessReadyStateVersion = productionassuranceSession.BrowserProductionReadinessReadyStateVersion,
            BrowserProductionReadinessSessionVersion = productionassuranceSession.BrowserProductionReadinessSessionVersion,
            LaunchMode = productionassuranceSession.LaunchMode,
            AssetRootPath = productionassuranceSession.AssetRootPath,
            ProfilesRootPath = productionassuranceSession.ProfilesRootPath,
            CacheRootPath = productionassuranceSession.CacheRootPath,
            ConfigRootPath = productionassuranceSession.ConfigRootPath,
            SettingsFilePath = productionassuranceSession.SettingsFilePath,
            StartupProfilePath = productionassuranceSession.StartupProfilePath,
            RequiredAssets = productionassuranceSession.RequiredAssets,
            ReadyAssetCount = productionassuranceSession.ReadyAssetCount,
            CompletedSteps = productionassuranceSession.CompletedSteps,
            TotalSteps = productionassuranceSession.TotalSteps,
            Exists = productionassuranceSession.Exists,
            ReadSucceeded = productionassuranceSession.ReadSucceeded
        };

        if (!productionassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionassurance ready state blocked for profile '{productionassuranceSession.ProfileId}'.";
            result.Error = productionassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionAssuranceReadyStateVersion = "runtime-browser-productionassurance-ready-state-v1";
        result.BrowserProductionAssuranceReadyChecks =
        [
            "browser-productionreadiness-ready-state-ready",
            "browser-productionassurance-session-ready",
            "browser-productionassurance-ready"
        ];
        result.BrowserProductionAssuranceReadySummary = $"Runtime browser productionassurance ready state passed {result.BrowserProductionAssuranceReadyChecks.Length} productionassurance readiness check(s) for profile '{productionassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionassurance ready state ready for profile '{productionassuranceSession.ProfileId}' with {result.BrowserProductionAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserProductionReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
