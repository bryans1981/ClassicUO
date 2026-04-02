namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFamiliaritySession
{
    ValueTask<BrowserClientRuntimeBrowserFamiliaritySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFamiliaritySessionService : IBrowserClientRuntimeBrowserFamiliaritySession
{
    private readonly IBrowserClientRuntimeBrowserPredictabilityReadyState _runtimeBrowserPredictabilityReadyState;

    public BrowserClientRuntimeBrowserFamiliaritySessionService(IBrowserClientRuntimeBrowserPredictabilityReadyState runtimeBrowserPredictabilityReadyState)
    {
        _runtimeBrowserPredictabilityReadyState = runtimeBrowserPredictabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFamiliaritySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPredictabilityReadyStateResult predictabilityReadyState = await _runtimeBrowserPredictabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFamiliaritySessionResult result = new()
        {
            ProfileId = predictabilityReadyState.ProfileId,
            SessionId = predictabilityReadyState.SessionId,
            SessionPath = predictabilityReadyState.SessionPath,
            BrowserPredictabilityReadyStateVersion = predictabilityReadyState.BrowserPredictabilityReadyStateVersion,
            BrowserPredictabilitySessionVersion = predictabilityReadyState.BrowserPredictabilitySessionVersion,
            LaunchMode = predictabilityReadyState.LaunchMode,
            AssetRootPath = predictabilityReadyState.AssetRootPath,
            ProfilesRootPath = predictabilityReadyState.ProfilesRootPath,
            CacheRootPath = predictabilityReadyState.CacheRootPath,
            ConfigRootPath = predictabilityReadyState.ConfigRootPath,
            SettingsFilePath = predictabilityReadyState.SettingsFilePath,
            StartupProfilePath = predictabilityReadyState.StartupProfilePath,
            RequiredAssets = predictabilityReadyState.RequiredAssets,
            ReadyAssetCount = predictabilityReadyState.ReadyAssetCount,
            CompletedSteps = predictabilityReadyState.CompletedSteps,
            TotalSteps = predictabilityReadyState.TotalSteps,
            Exists = predictabilityReadyState.Exists,
            ReadSucceeded = predictabilityReadyState.ReadSucceeded
        };

        if (!predictabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser familiarity session blocked for profile '{predictabilityReadyState.ProfileId}'.";
            result.Error = predictabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFamiliaritySessionVersion = "runtime-browser-familiarity-session-v1";
        result.BrowserFamiliarityStages =
        [
            "open-browser-familiarity-session",
            "bind-browser-predictability-ready-state",
            "publish-browser-familiarity-ready"
        ];
        result.BrowserFamiliaritySummary = $"Runtime browser familiarity session prepared {result.BrowserFamiliarityStages.Length} familiarity stage(s) for profile '{predictabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser familiarity session ready for profile '{predictabilityReadyState.ProfileId}' with {result.BrowserFamiliarityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFamiliaritySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFamiliaritySessionVersion { get; set; } = string.Empty;
    public string BrowserPredictabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPredictabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFamiliarityStages { get; set; } = Array.Empty<string>();
    public string BrowserFamiliaritySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

