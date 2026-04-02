namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAimfulnessSession
{
    ValueTask<BrowserClientRuntimeBrowserAimfulnessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAimfulnessSessionService : IBrowserClientRuntimeBrowserAimfulnessSession
{
    private readonly IBrowserClientRuntimeBrowserTaskFitReadyState _runtimeBrowserTaskFitReadyState;

    public BrowserClientRuntimeBrowserAimfulnessSessionService(IBrowserClientRuntimeBrowserTaskFitReadyState runtimeBrowserTaskFitReadyState)
    {
        _runtimeBrowserTaskFitReadyState = runtimeBrowserTaskFitReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAimfulnessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTaskFitReadyStateResult taskfitReadyState = await _runtimeBrowserTaskFitReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAimfulnessSessionResult result = new()
        {
            ProfileId = taskfitReadyState.ProfileId,
            SessionId = taskfitReadyState.SessionId,
            SessionPath = taskfitReadyState.SessionPath,
            BrowserTaskFitReadyStateVersion = taskfitReadyState.BrowserTaskFitReadyStateVersion,
            BrowserTaskFitSessionVersion = taskfitReadyState.BrowserTaskFitSessionVersion,
            LaunchMode = taskfitReadyState.LaunchMode,
            AssetRootPath = taskfitReadyState.AssetRootPath,
            ProfilesRootPath = taskfitReadyState.ProfilesRootPath,
            CacheRootPath = taskfitReadyState.CacheRootPath,
            ConfigRootPath = taskfitReadyState.ConfigRootPath,
            SettingsFilePath = taskfitReadyState.SettingsFilePath,
            StartupProfilePath = taskfitReadyState.StartupProfilePath,
            RequiredAssets = taskfitReadyState.RequiredAssets,
            ReadyAssetCount = taskfitReadyState.ReadyAssetCount,
            CompletedSteps = taskfitReadyState.CompletedSteps,
            TotalSteps = taskfitReadyState.TotalSteps,
            Exists = taskfitReadyState.Exists,
            ReadSucceeded = taskfitReadyState.ReadSucceeded
        };

        if (!taskfitReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser aimfulness session blocked for profile '{taskfitReadyState.ProfileId}'.";
            result.Error = taskfitReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAimfulnessSessionVersion = "runtime-browser-aimfulness-session-v1";
        result.BrowserAimfulnessStages =
        [
            "open-browser-aimfulness-session",
            "bind-browser-taskfit-ready-state",
            "publish-browser-aimfulness-ready"
        ];
        result.BrowserAimfulnessSummary = $"Runtime browser aimfulness session prepared {result.BrowserAimfulnessStages.Length} aimfulness stage(s) for profile '{taskfitReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser aimfulness session ready for profile '{taskfitReadyState.ProfileId}' with {result.BrowserAimfulnessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAimfulnessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAimfulnessSessionVersion { get; set; } = string.Empty;
    public string BrowserTaskFitReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskFitSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAimfulnessStages { get; set; } = Array.Empty<string>();
    public string BrowserAimfulnessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
