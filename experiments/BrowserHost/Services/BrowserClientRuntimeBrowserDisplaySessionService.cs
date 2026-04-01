namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDisplaySession
{
    ValueTask<BrowserClientRuntimeBrowserDisplaySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDisplaySessionService : IBrowserClientRuntimeBrowserDisplaySession
{
    private readonly IBrowserClientRuntimeBrowserPresentReadyState _runtimeBrowserPresentReadyState;

    public BrowserClientRuntimeBrowserDisplaySessionService(IBrowserClientRuntimeBrowserPresentReadyState runtimeBrowserPresentReadyState)
    {
        _runtimeBrowserPresentReadyState = runtimeBrowserPresentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDisplaySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPresentReadyStateResult presentReadyState = await _runtimeBrowserPresentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDisplaySessionResult result = new()
        {
            ProfileId = presentReadyState.ProfileId,
            SessionId = presentReadyState.SessionId,
            SessionPath = presentReadyState.SessionPath,
            BrowserPresentReadyStateVersion = presentReadyState.BrowserPresentReadyStateVersion,
            BrowserPresentSessionVersion = presentReadyState.BrowserPresentSessionVersion,
            LaunchMode = presentReadyState.LaunchMode,
            AssetRootPath = presentReadyState.AssetRootPath,
            ProfilesRootPath = presentReadyState.ProfilesRootPath,
            CacheRootPath = presentReadyState.CacheRootPath,
            ConfigRootPath = presentReadyState.ConfigRootPath,
            SettingsFilePath = presentReadyState.SettingsFilePath,
            StartupProfilePath = presentReadyState.StartupProfilePath,
            RequiredAssets = presentReadyState.RequiredAssets,
            ReadyAssetCount = presentReadyState.ReadyAssetCount,
            CompletedSteps = presentReadyState.CompletedSteps,
            TotalSteps = presentReadyState.TotalSteps,
            Exists = presentReadyState.Exists,
            ReadSucceeded = presentReadyState.ReadSucceeded,
            BrowserPresentReadyState = presentReadyState
        };

        if (!presentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser display session blocked for profile '{presentReadyState.ProfileId}'.";
            result.Error = presentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDisplaySessionVersion = "runtime-browser-display-session-v1";
        result.BrowserDisplayStages =
        [
            "open-browser-display-session",
            "bind-browser-present-ready-state",
            "publish-browser-display-ready"
        ];
        result.BrowserDisplaySummary = $"Runtime browser display session prepared {result.BrowserDisplayStages.Length} display stage(s) for profile '{presentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser display session ready for profile '{presentReadyState.ProfileId}' with {result.BrowserDisplayStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDisplaySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDisplaySessionVersion { get; set; } = string.Empty;
    public string BrowserPresentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPresentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDisplayStages { get; set; } = Array.Empty<string>();
    public string BrowserDisplaySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPresentReadyStateResult BrowserPresentReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
