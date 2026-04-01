namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInputSession
{
    ValueTask<BrowserClientRuntimeBrowserInputSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInputSessionService : IBrowserClientRuntimeBrowserInputSession
{
    private readonly IBrowserClientRuntimeBrowserSceneReadyState _runtimeBrowserSceneReadyState;

    public BrowserClientRuntimeBrowserInputSessionService(IBrowserClientRuntimeBrowserSceneReadyState runtimeBrowserSceneReadyState)
    {
        _runtimeBrowserSceneReadyState = runtimeBrowserSceneReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInputSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSceneReadyStateResult sceneReadyState = await _runtimeBrowserSceneReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInputSessionResult result = new()
        {
            ProfileId = sceneReadyState.ProfileId,
            SessionId = sceneReadyState.SessionId,
            SessionPath = sceneReadyState.SessionPath,
            BrowserSceneReadyStateVersion = sceneReadyState.BrowserSceneReadyStateVersion,
            BrowserSceneSessionVersion = sceneReadyState.BrowserSceneSessionVersion,
            LaunchMode = sceneReadyState.LaunchMode,
            AssetRootPath = sceneReadyState.AssetRootPath,
            ProfilesRootPath = sceneReadyState.ProfilesRootPath,
            CacheRootPath = sceneReadyState.CacheRootPath,
            ConfigRootPath = sceneReadyState.ConfigRootPath,
            SettingsFilePath = sceneReadyState.SettingsFilePath,
            StartupProfilePath = sceneReadyState.StartupProfilePath,
            RequiredAssets = sceneReadyState.RequiredAssets,
            ReadyAssetCount = sceneReadyState.ReadyAssetCount,
            CompletedSteps = sceneReadyState.CompletedSteps,
            TotalSteps = sceneReadyState.TotalSteps,
            Exists = sceneReadyState.Exists,
            ReadSucceeded = sceneReadyState.ReadSucceeded,
            BrowserSceneReadyState = sceneReadyState
        };

        if (!sceneReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser input session blocked for profile '{sceneReadyState.ProfileId}'.";
            result.Error = sceneReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInputSessionVersion = "runtime-browser-input-session-v1";
        result.BrowserInputStages =
        [
            "open-browser-input-session",
            "bind-browser-scene-ready-state",
            "publish-browser-input-ready"
        ];
        result.BrowserInputSummary = $"Runtime browser input session prepared {result.BrowserInputStages.Length} input stage(s) for profile '{sceneReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser input session ready for profile '{sceneReadyState.ProfileId}' with {result.BrowserInputStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInputSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInputSessionVersion { get; set; } = string.Empty;
    public string BrowserSceneReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSceneSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInputStages { get; set; } = Array.Empty<string>();
    public string BrowserInputSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserSceneReadyStateResult BrowserSceneReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
