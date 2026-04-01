namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSceneReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSceneReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSceneReadyStateService : IBrowserClientRuntimeBrowserSceneReadyState
{
    private readonly IBrowserClientRuntimeBrowserSceneSession _runtimeBrowserSceneSession;

    public BrowserClientRuntimeBrowserSceneReadyStateService(IBrowserClientRuntimeBrowserSceneSession runtimeBrowserSceneSession)
    {
        _runtimeBrowserSceneSession = runtimeBrowserSceneSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSceneReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSceneSessionResult sceneSession = await _runtimeBrowserSceneSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSceneReadyStateResult result = new()
        {
            ProfileId = sceneSession.ProfileId,
            SessionId = sceneSession.SessionId,
            SessionPath = sceneSession.SessionPath,
            BrowserSceneSessionVersion = sceneSession.BrowserSceneSessionVersion,
            BrowserViewportReadyStateVersion = sceneSession.BrowserViewportReadyStateVersion,
            BrowserViewportSessionVersion = sceneSession.BrowserViewportSessionVersion,
            LaunchMode = sceneSession.LaunchMode,
            AssetRootPath = sceneSession.AssetRootPath,
            ProfilesRootPath = sceneSession.ProfilesRootPath,
            CacheRootPath = sceneSession.CacheRootPath,
            ConfigRootPath = sceneSession.ConfigRootPath,
            SettingsFilePath = sceneSession.SettingsFilePath,
            StartupProfilePath = sceneSession.StartupProfilePath,
            RequiredAssets = sceneSession.RequiredAssets,
            ReadyAssetCount = sceneSession.ReadyAssetCount,
            CompletedSteps = sceneSession.CompletedSteps,
            TotalSteps = sceneSession.TotalSteps,
            Exists = sceneSession.Exists,
            ReadSucceeded = sceneSession.ReadSucceeded,
            BrowserSceneSession = sceneSession
        };

        if (!sceneSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser scene ready state blocked for profile '{sceneSession.ProfileId}'.";
            result.Error = sceneSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSceneReadyStateVersion = "runtime-browser-scene-ready-state-v1";
        result.BrowserSceneReadyChecks =
        [
            "browser-viewport-ready-state-ready",
            "browser-scene-session-ready",
            "browser-scene-ready"
        ];
        result.BrowserSceneReadySummary = $"Runtime browser scene ready state passed {result.BrowserSceneReadyChecks.Length} scene readiness check(s) for profile '{sceneSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser scene ready state ready for profile '{sceneSession.ProfileId}' with {result.BrowserSceneReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSceneReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSceneReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserSceneReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSceneReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserSceneSessionResult BrowserSceneSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
