namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSceneSession
{
    ValueTask<BrowserClientRuntimeBrowserSceneSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSceneSessionService : IBrowserClientRuntimeBrowserSceneSession
{
    private readonly IBrowserClientRuntimeBrowserViewportReadyState _runtimeBrowserViewportReadyState;

    public BrowserClientRuntimeBrowserSceneSessionService(IBrowserClientRuntimeBrowserViewportReadyState runtimeBrowserViewportReadyState)
    {
        _runtimeBrowserViewportReadyState = runtimeBrowserViewportReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSceneSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserViewportReadyStateResult viewportReadyState = await _runtimeBrowserViewportReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSceneSessionResult result = new()
        {
            ProfileId = viewportReadyState.ProfileId,
            SessionId = viewportReadyState.SessionId,
            SessionPath = viewportReadyState.SessionPath,
            BrowserViewportReadyStateVersion = viewportReadyState.BrowserViewportReadyStateVersion,
            BrowserViewportSessionVersion = viewportReadyState.BrowserViewportSessionVersion,
            LaunchMode = viewportReadyState.LaunchMode,
            AssetRootPath = viewportReadyState.AssetRootPath,
            ProfilesRootPath = viewportReadyState.ProfilesRootPath,
            CacheRootPath = viewportReadyState.CacheRootPath,
            ConfigRootPath = viewportReadyState.ConfigRootPath,
            SettingsFilePath = viewportReadyState.SettingsFilePath,
            StartupProfilePath = viewportReadyState.StartupProfilePath,
            RequiredAssets = viewportReadyState.RequiredAssets,
            ReadyAssetCount = viewportReadyState.ReadyAssetCount,
            CompletedSteps = viewportReadyState.CompletedSteps,
            TotalSteps = viewportReadyState.TotalSteps,
            Exists = viewportReadyState.Exists,
            ReadSucceeded = viewportReadyState.ReadSucceeded,
            BrowserViewportReadyState = viewportReadyState
        };

        if (!viewportReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser scene session blocked for profile '{viewportReadyState.ProfileId}'.";
            result.Error = viewportReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSceneSessionVersion = "runtime-browser-scene-session-v1";
        result.BrowserSceneStages =
        [
            "open-browser-scene-session",
            "bind-browser-viewport-ready-state",
            "publish-browser-scene-ready"
        ];
        result.BrowserSceneSummary = $"Runtime browser scene session prepared {result.BrowserSceneStages.Length} scene stage(s) for profile '{viewportReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser scene session ready for profile '{viewportReadyState.ProfileId}' with {result.BrowserSceneStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSceneSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSceneSessionVersion { get; set; } = string.Empty;
    public string BrowserViewportReadyStateVersion { get; set; } = string.Empty;
    public string BrowserViewportSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSceneStages { get; set; } = Array.Empty<string>();
    public string BrowserSceneSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserViewportReadyStateResult BrowserViewportReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
