namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserWayfindingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserWayfindingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserWayfindingReadyStateService : IBrowserClientRuntimeBrowserWayfindingReadyState
{
    private readonly IBrowserClientRuntimeBrowserWayfindingSession _runtimeBrowserWayfindingSession;

    public BrowserClientRuntimeBrowserWayfindingReadyStateService(IBrowserClientRuntimeBrowserWayfindingSession runtimeBrowserWayfindingSession)
    {
        _runtimeBrowserWayfindingSession = runtimeBrowserWayfindingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserWayfindingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserWayfindingSessionResult wayfindingSession = await _runtimeBrowserWayfindingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserWayfindingReadyStateResult result = new()
        {
            ProfileId = wayfindingSession.ProfileId,
            SessionId = wayfindingSession.SessionId,
            SessionPath = wayfindingSession.SessionPath,
            BrowserWayfindingSessionVersion = wayfindingSession.BrowserWayfindingSessionVersion,
            BrowserOrientationReadyStateVersion = wayfindingSession.BrowserOrientationReadyStateVersion,
            BrowserOrientationSessionVersion = wayfindingSession.BrowserOrientationSessionVersion,
            LaunchMode = wayfindingSession.LaunchMode,
            AssetRootPath = wayfindingSession.AssetRootPath,
            ProfilesRootPath = wayfindingSession.ProfilesRootPath,
            CacheRootPath = wayfindingSession.CacheRootPath,
            ConfigRootPath = wayfindingSession.ConfigRootPath,
            SettingsFilePath = wayfindingSession.SettingsFilePath,
            StartupProfilePath = wayfindingSession.StartupProfilePath,
            RequiredAssets = wayfindingSession.RequiredAssets,
            ReadyAssetCount = wayfindingSession.ReadyAssetCount,
            CompletedSteps = wayfindingSession.CompletedSteps,
            TotalSteps = wayfindingSession.TotalSteps,
            Exists = wayfindingSession.Exists,
            ReadSucceeded = wayfindingSession.ReadSucceeded
        };

        if (!wayfindingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser wayfinding ready state blocked for profile '{wayfindingSession.ProfileId}'.";
            result.Error = wayfindingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserWayfindingReadyStateVersion = "runtime-browser-wayfinding-ready-state-v1";
        result.BrowserWayfindingReadyChecks =
        [
            "browser-orientation-ready-state-ready",
            "browser-wayfinding-session-ready",
            "browser-wayfinding-ready"
        ];
        result.BrowserWayfindingReadySummary = $"Runtime browser wayfinding ready state passed {result.BrowserWayfindingReadyChecks.Length} wayfinding readiness check(s) for profile '{wayfindingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser wayfinding ready state ready for profile '{wayfindingSession.ProfileId}' with {result.BrowserWayfindingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserWayfindingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserWayfindingReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserWayfindingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserWayfindingReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
