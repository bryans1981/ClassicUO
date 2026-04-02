namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProgressAwarenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProgressAwarenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProgressAwarenessReadyStateService : IBrowserClientRuntimeBrowserProgressAwarenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserProgressAwarenessSession _runtimeBrowserProgressAwarenessSession;

    public BrowserClientRuntimeBrowserProgressAwarenessReadyStateService(IBrowserClientRuntimeBrowserProgressAwarenessSession runtimeBrowserProgressAwarenessSession)
    {
        _runtimeBrowserProgressAwarenessSession = runtimeBrowserProgressAwarenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProgressAwarenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProgressAwarenessSessionResult progressawarenessSession = await _runtimeBrowserProgressAwarenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserProgressAwarenessReadyStateResult result = new()
        {
            ProfileId = progressawarenessSession.ProfileId,
            SessionId = progressawarenessSession.SessionId,
            SessionPath = progressawarenessSession.SessionPath,
            BrowserProgressAwarenessSessionVersion = progressawarenessSession.BrowserProgressAwarenessSessionVersion,
            BrowserWayfindingReadyStateVersion = progressawarenessSession.BrowserWayfindingReadyStateVersion,
            BrowserWayfindingSessionVersion = progressawarenessSession.BrowserWayfindingSessionVersion,
            LaunchMode = progressawarenessSession.LaunchMode,
            AssetRootPath = progressawarenessSession.AssetRootPath,
            ProfilesRootPath = progressawarenessSession.ProfilesRootPath,
            CacheRootPath = progressawarenessSession.CacheRootPath,
            ConfigRootPath = progressawarenessSession.ConfigRootPath,
            SettingsFilePath = progressawarenessSession.SettingsFilePath,
            StartupProfilePath = progressawarenessSession.StartupProfilePath,
            RequiredAssets = progressawarenessSession.RequiredAssets,
            ReadyAssetCount = progressawarenessSession.ReadyAssetCount,
            CompletedSteps = progressawarenessSession.CompletedSteps,
            TotalSteps = progressawarenessSession.TotalSteps,
            Exists = progressawarenessSession.Exists,
            ReadSucceeded = progressawarenessSession.ReadSucceeded
        };

        if (!progressawarenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser progressawareness ready state blocked for profile '{progressawarenessSession.ProfileId}'.";
            result.Error = progressawarenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProgressAwarenessReadyStateVersion = "runtime-browser-progressawareness-ready-state-v1";
        result.BrowserProgressAwarenessReadyChecks =
        [
            "browser-wayfinding-ready-state-ready",
            "browser-progressawareness-session-ready",
            "browser-progressawareness-ready"
        ];
        result.BrowserProgressAwarenessReadySummary = $"Runtime browser progressawareness ready state passed {result.BrowserProgressAwarenessReadyChecks.Length} progressawareness readiness check(s) for profile '{progressawarenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser progressawareness ready state ready for profile '{progressawarenessSession.ProfileId}' with {result.BrowserProgressAwarenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProgressAwarenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProgressAwarenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProgressAwarenessSessionVersion { get; set; } = string.Empty;
    public string BrowserWayfindingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserWayfindingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProgressAwarenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProgressAwarenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
