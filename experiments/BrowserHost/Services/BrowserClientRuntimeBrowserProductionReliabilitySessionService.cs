namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserProductionReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionReliabilitySessionService : IBrowserClientRuntimeBrowserProductionReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserProductionReadinessReadyState _runtimeBrowserProductionReadinessReadyState;

    public BrowserClientRuntimeBrowserProductionReliabilitySessionService(IBrowserClientRuntimeBrowserProductionReadinessReadyState runtimeBrowserProductionReadinessReadyState)
    {
        _runtimeBrowserProductionReadinessReadyState = runtimeBrowserProductionReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionReadinessReadyStateResult prevReadyState = await _runtimeBrowserProductionReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserServiceContinuityReadyStateVersion = prevReadyState.BrowserProductionReadinessReadyStateVersion,
            BrowserServiceContinuitySessionVersion = prevReadyState.BrowserProductionReadinessSessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionReliabilitySessionVersion = "runtime-browser-productionreliability-session-v1";
        result.BrowserProductionReliabilityStages =
        [
            "open-browser-productionreliability-session",
            "bind-browser-productionreadiness-ready-state",
            "publish-browser-productionreliability-ready"
        ];
        result.BrowserProductionReliabilitySummary = $"Runtime browser productionreliability session prepared {result.BrowserProductionReliabilityStages.Length} productionreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserProductionReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserProductionReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
