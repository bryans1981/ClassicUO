namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPredictabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserPredictabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPredictabilitySessionService : IBrowserClientRuntimeBrowserPredictabilitySession
{
    private readonly IBrowserClientRuntimeBrowserCohesivenessReadyState _runtimeBrowserCohesivenessReadyState;

    public BrowserClientRuntimeBrowserPredictabilitySessionService(IBrowserClientRuntimeBrowserCohesivenessReadyState runtimeBrowserCohesivenessReadyState)
    {
        _runtimeBrowserCohesivenessReadyState = runtimeBrowserCohesivenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPredictabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCohesivenessReadyStateResult cohesivenessReadyState = await _runtimeBrowserCohesivenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPredictabilitySessionResult result = new()
        {
            ProfileId = cohesivenessReadyState.ProfileId,
            SessionId = cohesivenessReadyState.SessionId,
            SessionPath = cohesivenessReadyState.SessionPath,
            BrowserCohesivenessReadyStateVersion = cohesivenessReadyState.BrowserCohesivenessReadyStateVersion,
            BrowserCohesivenessSessionVersion = cohesivenessReadyState.BrowserCohesivenessSessionVersion,
            LaunchMode = cohesivenessReadyState.LaunchMode,
            AssetRootPath = cohesivenessReadyState.AssetRootPath,
            ProfilesRootPath = cohesivenessReadyState.ProfilesRootPath,
            CacheRootPath = cohesivenessReadyState.CacheRootPath,
            ConfigRootPath = cohesivenessReadyState.ConfigRootPath,
            SettingsFilePath = cohesivenessReadyState.SettingsFilePath,
            StartupProfilePath = cohesivenessReadyState.StartupProfilePath,
            RequiredAssets = cohesivenessReadyState.RequiredAssets,
            ReadyAssetCount = cohesivenessReadyState.ReadyAssetCount,
            CompletedSteps = cohesivenessReadyState.CompletedSteps,
            TotalSteps = cohesivenessReadyState.TotalSteps,
            Exists = cohesivenessReadyState.Exists,
            ReadSucceeded = cohesivenessReadyState.ReadSucceeded
        };

        if (!cohesivenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser predictability session blocked for profile '{cohesivenessReadyState.ProfileId}'.";
            result.Error = cohesivenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPredictabilitySessionVersion = "runtime-browser-predictability-session-v1";
        result.BrowserPredictabilityStages =
        [
            "open-browser-predictability-session",
            "bind-browser-cohesiveness-ready-state",
            "publish-browser-predictability-ready"
        ];
        result.BrowserPredictabilitySummary = $"Runtime browser predictability session prepared {result.BrowserPredictabilityStages.Length} predictability stage(s) for profile '{cohesivenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser predictability session ready for profile '{cohesivenessReadyState.ProfileId}' with {result.BrowserPredictabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPredictabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPredictabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCohesivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCohesivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPredictabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserPredictabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

