namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserViewportSession
{
    ValueTask<BrowserClientRuntimeBrowserViewportSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserViewportSessionService : IBrowserClientRuntimeBrowserViewportSession
{
    private readonly IBrowserClientRuntimeBrowserDisplayReadyState _runtimeBrowserDisplayReadyState;

    public BrowserClientRuntimeBrowserViewportSessionService(IBrowserClientRuntimeBrowserDisplayReadyState runtimeBrowserDisplayReadyState)
    {
        _runtimeBrowserDisplayReadyState = runtimeBrowserDisplayReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserViewportSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDisplayReadyStateResult displayReadyState = await _runtimeBrowserDisplayReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserViewportSessionResult result = new()
        {
            ProfileId = displayReadyState.ProfileId,
            SessionId = displayReadyState.SessionId,
            SessionPath = displayReadyState.SessionPath,
            BrowserDisplayReadyStateVersion = displayReadyState.BrowserDisplayReadyStateVersion,
            BrowserDisplaySessionVersion = displayReadyState.BrowserDisplaySessionVersion,
            LaunchMode = displayReadyState.LaunchMode,
            AssetRootPath = displayReadyState.AssetRootPath,
            ProfilesRootPath = displayReadyState.ProfilesRootPath,
            CacheRootPath = displayReadyState.CacheRootPath,
            ConfigRootPath = displayReadyState.ConfigRootPath,
            SettingsFilePath = displayReadyState.SettingsFilePath,
            StartupProfilePath = displayReadyState.StartupProfilePath,
            RequiredAssets = displayReadyState.RequiredAssets,
            ReadyAssetCount = displayReadyState.ReadyAssetCount,
            CompletedSteps = displayReadyState.CompletedSteps,
            TotalSteps = displayReadyState.TotalSteps,
            Exists = displayReadyState.Exists,
            ReadSucceeded = displayReadyState.ReadSucceeded,
            BrowserDisplayReadyState = displayReadyState
        };

        if (!displayReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser viewport session blocked for profile '{displayReadyState.ProfileId}'.";
            result.Error = displayReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserViewportSessionVersion = "runtime-browser-viewport-session-v1";
        result.BrowserViewportStages =
        [
            "open-browser-viewport-session",
            "bind-browser-display-ready-state",
            "publish-browser-viewport-ready"
        ];
        result.BrowserViewportSummary = $"Runtime browser viewport session prepared {result.BrowserViewportStages.Length} viewport stage(s) for profile '{displayReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser viewport session ready for profile '{displayReadyState.ProfileId}' with {result.BrowserViewportStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserViewportSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserViewportSessionVersion { get; set; } = string.Empty;
    public string BrowserDisplayReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDisplaySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserViewportStages { get; set; } = Array.Empty<string>();
    public string BrowserViewportSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserDisplayReadyStateResult BrowserDisplayReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
