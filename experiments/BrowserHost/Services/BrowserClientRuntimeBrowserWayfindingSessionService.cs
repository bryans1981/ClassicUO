namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserWayfindingSession
{
    ValueTask<BrowserClientRuntimeBrowserWayfindingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserWayfindingSessionService : IBrowserClientRuntimeBrowserWayfindingSession
{
    private readonly IBrowserClientRuntimeBrowserOrientationReadyState _runtimeBrowserOrientationReadyState;

    public BrowserClientRuntimeBrowserWayfindingSessionService(IBrowserClientRuntimeBrowserOrientationReadyState runtimeBrowserOrientationReadyState)
    {
        _runtimeBrowserOrientationReadyState = runtimeBrowserOrientationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserWayfindingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOrientationReadyStateResult orientationReadyState = await _runtimeBrowserOrientationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserWayfindingSessionResult result = new()
        {
            ProfileId = orientationReadyState.ProfileId,
            SessionId = orientationReadyState.SessionId,
            SessionPath = orientationReadyState.SessionPath,
            BrowserOrientationReadyStateVersion = orientationReadyState.BrowserOrientationReadyStateVersion,
            BrowserOrientationSessionVersion = orientationReadyState.BrowserOrientationSessionVersion,
            LaunchMode = orientationReadyState.LaunchMode,
            AssetRootPath = orientationReadyState.AssetRootPath,
            ProfilesRootPath = orientationReadyState.ProfilesRootPath,
            CacheRootPath = orientationReadyState.CacheRootPath,
            ConfigRootPath = orientationReadyState.ConfigRootPath,
            SettingsFilePath = orientationReadyState.SettingsFilePath,
            StartupProfilePath = orientationReadyState.StartupProfilePath,
            RequiredAssets = orientationReadyState.RequiredAssets,
            ReadyAssetCount = orientationReadyState.ReadyAssetCount,
            CompletedSteps = orientationReadyState.CompletedSteps,
            TotalSteps = orientationReadyState.TotalSteps,
            Exists = orientationReadyState.Exists,
            ReadSucceeded = orientationReadyState.ReadSucceeded
        };

        if (!orientationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser wayfinding session blocked for profile '{orientationReadyState.ProfileId}'.";
            result.Error = orientationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserWayfindingSessionVersion = "runtime-browser-wayfinding-session-v1";
        result.BrowserWayfindingStages =
        [
            "open-browser-wayfinding-session",
            "bind-browser-orientation-ready-state",
            "publish-browser-wayfinding-ready"
        ];
        result.BrowserWayfindingSummary = $"Runtime browser wayfinding session prepared {result.BrowserWayfindingStages.Length} wayfinding stage(s) for profile '{orientationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser wayfinding session ready for profile '{orientationReadyState.ProfileId}' with {result.BrowserWayfindingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserWayfindingSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserWayfindingSessionVersion { get; set; } = string.Empty;
    public string BrowserOrientationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOrientationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserWayfindingStages { get; set; } = Array.Empty<string>();
    public string BrowserWayfindingSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
