namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTaskFitSession
{
    ValueTask<BrowserClientRuntimeBrowserTaskFitSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTaskFitSessionService : IBrowserClientRuntimeBrowserTaskFitSession
{
    private readonly IBrowserClientRuntimeBrowserDirectionalityReadyState _runtimeBrowserDirectionalityReadyState;

    public BrowserClientRuntimeBrowserTaskFitSessionService(IBrowserClientRuntimeBrowserDirectionalityReadyState runtimeBrowserDirectionalityReadyState)
    {
        _runtimeBrowserDirectionalityReadyState = runtimeBrowserDirectionalityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTaskFitSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDirectionalityReadyStateResult directionalityReadyState = await _runtimeBrowserDirectionalityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTaskFitSessionResult result = new()
        {
            ProfileId = directionalityReadyState.ProfileId,
            SessionId = directionalityReadyState.SessionId,
            SessionPath = directionalityReadyState.SessionPath,
            BrowserDirectionalityReadyStateVersion = directionalityReadyState.BrowserDirectionalityReadyStateVersion,
            BrowserDirectionalitySessionVersion = directionalityReadyState.BrowserDirectionalitySessionVersion,
            LaunchMode = directionalityReadyState.LaunchMode,
            AssetRootPath = directionalityReadyState.AssetRootPath,
            ProfilesRootPath = directionalityReadyState.ProfilesRootPath,
            CacheRootPath = directionalityReadyState.CacheRootPath,
            ConfigRootPath = directionalityReadyState.ConfigRootPath,
            SettingsFilePath = directionalityReadyState.SettingsFilePath,
            StartupProfilePath = directionalityReadyState.StartupProfilePath,
            RequiredAssets = directionalityReadyState.RequiredAssets,
            ReadyAssetCount = directionalityReadyState.ReadyAssetCount,
            CompletedSteps = directionalityReadyState.CompletedSteps,
            TotalSteps = directionalityReadyState.TotalSteps,
            Exists = directionalityReadyState.Exists,
            ReadSucceeded = directionalityReadyState.ReadSucceeded
        };

        if (!directionalityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser taskfit session blocked for profile '{directionalityReadyState.ProfileId}'.";
            result.Error = directionalityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTaskFitSessionVersion = "runtime-browser-taskfit-session-v1";
        result.BrowserTaskFitStages =
        [
            "open-browser-taskfit-session",
            "bind-browser-directionality-ready-state",
            "publish-browser-taskfit-ready"
        ];
        result.BrowserTaskFitSummary = $"Runtime browser taskfit session prepared {result.BrowserTaskFitStages.Length} taskfit stage(s) for profile '{directionalityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser taskfit session ready for profile '{directionalityReadyState.ProfileId}' with {result.BrowserTaskFitStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTaskFitSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTaskFitSessionVersion { get; set; } = string.Empty;
    public string BrowserDirectionalityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDirectionalitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTaskFitStages { get; set; } = Array.Empty<string>();
    public string BrowserTaskFitSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
