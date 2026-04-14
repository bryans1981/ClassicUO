namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserProductionAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionAssuranceSessionService : IBrowserClientRuntimeBrowserProductionAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserProductionReadinessReadyState _runtimeBrowserProductionReadinessReadyState;

    public BrowserClientRuntimeBrowserProductionAssuranceSessionService(IBrowserClientRuntimeBrowserProductionReadinessReadyState runtimeBrowserProductionReadinessReadyState)
    {
        _runtimeBrowserProductionReadinessReadyState = runtimeBrowserProductionReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionReadinessReadyStateResult productionreadinessReadyState = await _runtimeBrowserProductionReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionAssuranceSessionResult result = new()
        {
            ProfileId = productionreadinessReadyState.ProfileId,
            SessionId = productionreadinessReadyState.SessionId,
            SessionPath = productionreadinessReadyState.SessionPath,
            BrowserProductionReadinessReadyStateVersion = productionreadinessReadyState.BrowserProductionReadinessReadyStateVersion,
            BrowserProductionReadinessSessionVersion = productionreadinessReadyState.BrowserProductionReadinessSessionVersion,
            LaunchMode = productionreadinessReadyState.LaunchMode,
            AssetRootPath = productionreadinessReadyState.AssetRootPath,
            ProfilesRootPath = productionreadinessReadyState.ProfilesRootPath,
            CacheRootPath = productionreadinessReadyState.CacheRootPath,
            ConfigRootPath = productionreadinessReadyState.ConfigRootPath,
            SettingsFilePath = productionreadinessReadyState.SettingsFilePath,
            StartupProfilePath = productionreadinessReadyState.StartupProfilePath,
            RequiredAssets = productionreadinessReadyState.RequiredAssets,
            ReadyAssetCount = productionreadinessReadyState.ReadyAssetCount,
            CompletedSteps = productionreadinessReadyState.CompletedSteps,
            TotalSteps = productionreadinessReadyState.TotalSteps,
            Exists = productionreadinessReadyState.Exists,
            ReadSucceeded = productionreadinessReadyState.ReadSucceeded
        };

        if (!productionreadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionassurance session blocked for profile '{productionreadinessReadyState.ProfileId}'.";
            result.Error = productionreadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionAssuranceSessionVersion = "runtime-browser-productionassurance-session-v1";
        result.BrowserProductionAssuranceStages =
        [
            "open-browser-productionassurance-session",
            "bind-browser-productionreadiness-ready-state",
            "publish-browser-productionassurance-ready"
        ];
        result.BrowserProductionAssuranceSummary = $"Runtime browser productionassurance session prepared {result.BrowserProductionAssuranceStages.Length} productionassurance stage(s) for profile '{productionreadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionassurance session ready for profile '{productionreadinessReadyState.ProfileId}' with {result.BrowserProductionAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionAssuranceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserProductionAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserProductionAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
