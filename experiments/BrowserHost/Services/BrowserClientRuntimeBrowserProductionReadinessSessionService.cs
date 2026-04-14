namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserProductionReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionReadinessSessionService : IBrowserClientRuntimeBrowserProductionReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserServiceAssuranceReadyState _runtimeBrowserServiceAssuranceReadyState;

    public BrowserClientRuntimeBrowserProductionReadinessSessionService(IBrowserClientRuntimeBrowserServiceAssuranceReadyState runtimeBrowserServiceAssuranceReadyState)
    {
        _runtimeBrowserServiceAssuranceReadyState = runtimeBrowserServiceAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceAssuranceReadyStateResult serviceassuranceReadyState = await _runtimeBrowserServiceAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionReadinessSessionResult result = new()
        {
            ProfileId = serviceassuranceReadyState.ProfileId,
            SessionId = serviceassuranceReadyState.SessionId,
            SessionPath = serviceassuranceReadyState.SessionPath,
            BrowserServiceAssuranceReadyStateVersion = serviceassuranceReadyState.BrowserServiceAssuranceReadyStateVersion,
            BrowserServiceAssuranceSessionVersion = serviceassuranceReadyState.BrowserServiceAssuranceSessionVersion,
            LaunchMode = serviceassuranceReadyState.LaunchMode,
            AssetRootPath = serviceassuranceReadyState.AssetRootPath,
            ProfilesRootPath = serviceassuranceReadyState.ProfilesRootPath,
            CacheRootPath = serviceassuranceReadyState.CacheRootPath,
            ConfigRootPath = serviceassuranceReadyState.ConfigRootPath,
            SettingsFilePath = serviceassuranceReadyState.SettingsFilePath,
            StartupProfilePath = serviceassuranceReadyState.StartupProfilePath,
            RequiredAssets = serviceassuranceReadyState.RequiredAssets,
            ReadyAssetCount = serviceassuranceReadyState.ReadyAssetCount,
            CompletedSteps = serviceassuranceReadyState.CompletedSteps,
            TotalSteps = serviceassuranceReadyState.TotalSteps,
            Exists = serviceassuranceReadyState.Exists,
            ReadSucceeded = serviceassuranceReadyState.ReadSucceeded
        };

        if (!serviceassuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionreadiness session blocked for profile '{serviceassuranceReadyState.ProfileId}'.";
            result.Error = serviceassuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionReadinessSessionVersion = "runtime-browser-productionreadiness-session-v1";
        result.BrowserProductionReadinessStages =
        [
            "open-browser-productionreadiness-session",
            "bind-browser-serviceassurance-ready-state",
            "publish-browser-productionreadiness-ready"
        ];
        result.BrowserProductionReadinessSummary = $"Runtime browser productionreadiness session prepared {result.BrowserProductionReadinessStages.Length} productionreadiness stage(s) for profile '{serviceassuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionreadiness session ready for profile '{serviceassuranceReadyState.ProfileId}' with {result.BrowserProductionReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionReadinessSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserProductionReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserProductionReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
