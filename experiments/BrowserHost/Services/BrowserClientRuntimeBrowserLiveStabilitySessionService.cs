namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLiveStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveStabilitySessionService : IBrowserClientRuntimeBrowserLiveStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserProductionAssuranceReadyState _runtimeBrowserProductionAssuranceReadyState;

    public BrowserClientRuntimeBrowserLiveStabilitySessionService(IBrowserClientRuntimeBrowserProductionAssuranceReadyState runtimeBrowserProductionAssuranceReadyState)
    {
        _runtimeBrowserProductionAssuranceReadyState = runtimeBrowserProductionAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionAssuranceReadyStateResult productionassuranceReadyState = await _runtimeBrowserProductionAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveStabilitySessionResult result = new()
        {
            ProfileId = productionassuranceReadyState.ProfileId,
            SessionId = productionassuranceReadyState.SessionId,
            SessionPath = productionassuranceReadyState.SessionPath,
            BrowserProductionAssuranceReadyStateVersion = productionassuranceReadyState.BrowserProductionAssuranceReadyStateVersion,
            BrowserProductionAssuranceSessionVersion = productionassuranceReadyState.BrowserProductionAssuranceSessionVersion,
            LaunchMode = productionassuranceReadyState.LaunchMode,
            AssetRootPath = productionassuranceReadyState.AssetRootPath,
            ProfilesRootPath = productionassuranceReadyState.ProfilesRootPath,
            CacheRootPath = productionassuranceReadyState.CacheRootPath,
            ConfigRootPath = productionassuranceReadyState.ConfigRootPath,
            SettingsFilePath = productionassuranceReadyState.SettingsFilePath,
            StartupProfilePath = productionassuranceReadyState.StartupProfilePath,
            RequiredAssets = productionassuranceReadyState.RequiredAssets,
            ReadyAssetCount = productionassuranceReadyState.ReadyAssetCount,
            CompletedSteps = productionassuranceReadyState.CompletedSteps,
            TotalSteps = productionassuranceReadyState.TotalSteps,
            Exists = productionassuranceReadyState.Exists,
            ReadSucceeded = productionassuranceReadyState.ReadSucceeded
        };

        if (!productionassuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser livestability session blocked for profile '{productionassuranceReadyState.ProfileId}'.";
            result.Error = productionassuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveStabilitySessionVersion = "runtime-browser-livestability-session-v1";
        result.BrowserLiveStabilityStages =
        [
            "open-browser-livestability-session",
            "bind-browser-productionassurance-ready-state",
            "publish-browser-livestability-ready"
        ];
        result.BrowserLiveStabilitySummary = $"Runtime browser livestability session prepared {result.BrowserLiveStabilityStages.Length} livestability stage(s) for profile '{productionassuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livestability session ready for profile '{productionassuranceReadyState.ProfileId}' with {result.BrowserLiveStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserProductionAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
