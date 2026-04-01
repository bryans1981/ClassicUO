namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLifecycleReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLifecycleReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLifecycleReadyStateService : IBrowserClientRuntimeBrowserLifecycleReadyState
{
    private readonly IBrowserClientRuntimeBrowserLifecycleSession _runtimeBrowserLifecycleSession;

    public BrowserClientRuntimeBrowserLifecycleReadyStateService(IBrowserClientRuntimeBrowserLifecycleSession runtimeBrowserLifecycleSession)
    {
        _runtimeBrowserLifecycleSession = runtimeBrowserLifecycleSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLifecycleReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLifecycleSessionResult lifecycleSession = await _runtimeBrowserLifecycleSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLifecycleReadyStateResult result = new()
        {
            ProfileId = lifecycleSession.ProfileId,
            SessionId = lifecycleSession.SessionId,
            SessionPath = lifecycleSession.SessionPath,
            BrowserLifecycleSessionVersion = lifecycleSession.BrowserLifecycleSessionVersion,
            BrowserGestureReadyStateVersion = lifecycleSession.BrowserGestureReadyStateVersion,
            BrowserGestureSessionVersion = lifecycleSession.BrowserGestureSessionVersion,
            LaunchMode = lifecycleSession.LaunchMode,
            AssetRootPath = lifecycleSession.AssetRootPath,
            ProfilesRootPath = lifecycleSession.ProfilesRootPath,
            CacheRootPath = lifecycleSession.CacheRootPath,
            ConfigRootPath = lifecycleSession.ConfigRootPath,
            SettingsFilePath = lifecycleSession.SettingsFilePath,
            StartupProfilePath = lifecycleSession.StartupProfilePath,
            RequiredAssets = lifecycleSession.RequiredAssets,
            ReadyAssetCount = lifecycleSession.ReadyAssetCount,
            CompletedSteps = lifecycleSession.CompletedSteps,
            TotalSteps = lifecycleSession.TotalSteps,
            Exists = lifecycleSession.Exists,
            ReadSucceeded = lifecycleSession.ReadSucceeded,
            BrowserLifecycleSession = lifecycleSession
        };

        if (!lifecycleSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser lifecycle ready state blocked for profile '{lifecycleSession.ProfileId}'.";
            result.Error = lifecycleSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLifecycleReadyStateVersion = "runtime-browser-lifecycle-ready-state-v1";
        result.BrowserLifecycleReadyChecks =
        [
            "browser-gesture-ready-state-ready",
            "browser-lifecycle-session-ready",
            "browser-lifecycle-ready"
        ];
        result.BrowserLifecycleReadySummary = $"Runtime browser lifecycle ready state passed {result.BrowserLifecycleReadyChecks.Length} lifecycle readiness check(s) for profile '{lifecycleSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser lifecycle ready state ready for profile '{lifecycleSession.ProfileId}' with {result.BrowserLifecycleReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLifecycleReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLifecycleReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLifecycleSessionVersion { get; set; } = string.Empty;
    public string BrowserGestureReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGestureSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLifecycleReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLifecycleReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserLifecycleSessionResult BrowserLifecycleSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
