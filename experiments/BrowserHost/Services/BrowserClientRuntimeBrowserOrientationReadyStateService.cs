namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOrientationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOrientationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOrientationReadyStateService : IBrowserClientRuntimeBrowserOrientationReadyState
{
    private readonly IBrowserClientRuntimeBrowserOrientationSession _runtimeBrowserOrientationSession;

    public BrowserClientRuntimeBrowserOrientationReadyStateService(IBrowserClientRuntimeBrowserOrientationSession runtimeBrowserOrientationSession)
    {
        _runtimeBrowserOrientationSession = runtimeBrowserOrientationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOrientationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOrientationSessionResult orientationSession = await _runtimeBrowserOrientationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOrientationReadyStateResult result = new()
        {
            ProfileId = orientationSession.ProfileId,
            SessionId = orientationSession.SessionId,
            SessionPath = orientationSession.SessionPath,
            BrowserOrientationSessionVersion = orientationSession.BrowserOrientationSessionVersion,
            BrowserSignpostingReadyStateVersion = orientationSession.BrowserSignpostingReadyStateVersion,
            BrowserSignpostingSessionVersion = orientationSession.BrowserSignpostingSessionVersion,
            LaunchMode = orientationSession.LaunchMode,
            AssetRootPath = orientationSession.AssetRootPath,
            ProfilesRootPath = orientationSession.ProfilesRootPath,
            CacheRootPath = orientationSession.CacheRootPath,
            ConfigRootPath = orientationSession.ConfigRootPath,
            SettingsFilePath = orientationSession.SettingsFilePath,
            StartupProfilePath = orientationSession.StartupProfilePath,
            RequiredAssets = orientationSession.RequiredAssets,
            ReadyAssetCount = orientationSession.ReadyAssetCount,
            CompletedSteps = orientationSession.CompletedSteps,
            TotalSteps = orientationSession.TotalSteps,
            Exists = orientationSession.Exists,
            ReadSucceeded = orientationSession.ReadSucceeded
        };

        if (!orientationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser orientation ready state blocked for profile '{orientationSession.ProfileId}'.";
            result.Error = orientationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOrientationReadyStateVersion = "runtime-browser-orientation-ready-state-v1";
        result.BrowserOrientationReadyChecks =
        [
            "browser-signposting-ready-state-ready",
            "browser-orientation-session-ready",
            "browser-orientation-ready"
        ];
        result.BrowserOrientationReadySummary = $"Runtime browser orientation ready state passed {result.BrowserOrientationReadyChecks.Length} orientation readiness check(s) for profile '{orientationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser orientation ready state ready for profile '{orientationSession.ProfileId}' with {result.BrowserOrientationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOrientationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOrientationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOrientationSessionVersion { get; set; } = string.Empty;
    public string BrowserSignpostingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSignpostingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOrientationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOrientationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
