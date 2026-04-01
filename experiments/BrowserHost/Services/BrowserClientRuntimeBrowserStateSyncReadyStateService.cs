namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateSyncReadyState
{
    ValueTask<BrowserClientRuntimeBrowserStateSyncReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateSyncReadyStateService : IBrowserClientRuntimeBrowserStateSyncReadyState
{
    private readonly IBrowserClientRuntimeBrowserStateSyncSession _runtimeBrowserStateSyncSession;

    public BrowserClientRuntimeBrowserStateSyncReadyStateService(IBrowserClientRuntimeBrowserStateSyncSession runtimeBrowserStateSyncSession)
    {
        _runtimeBrowserStateSyncSession = runtimeBrowserStateSyncSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateSyncReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateSyncSessionResult stateSyncSession = await _runtimeBrowserStateSyncSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserStateSyncReadyStateResult result = new()
        {
            ProfileId = stateSyncSession.ProfileId,
            SessionId = stateSyncSession.SessionId,
            SessionPath = stateSyncSession.SessionPath,
            BrowserStateSyncSessionVersion = stateSyncSession.BrowserStateSyncSessionVersion,
            BrowserRouteReadyStateVersion = stateSyncSession.BrowserRouteReadyStateVersion,
            BrowserRouteSessionVersion = stateSyncSession.BrowserRouteSessionVersion,
            LaunchMode = stateSyncSession.LaunchMode,
            AssetRootPath = stateSyncSession.AssetRootPath,
            ProfilesRootPath = stateSyncSession.ProfilesRootPath,
            CacheRootPath = stateSyncSession.CacheRootPath,
            ConfigRootPath = stateSyncSession.ConfigRootPath,
            SettingsFilePath = stateSyncSession.SettingsFilePath,
            StartupProfilePath = stateSyncSession.StartupProfilePath,
            RequiredAssets = stateSyncSession.RequiredAssets,
            ReadyAssetCount = stateSyncSession.ReadyAssetCount,
            CompletedSteps = stateSyncSession.CompletedSteps,
            TotalSteps = stateSyncSession.TotalSteps,
            Exists = stateSyncSession.Exists,
            ReadSucceeded = stateSyncSession.ReadSucceeded,
            BrowserStateSyncSession = stateSyncSession
        };

        if (!stateSyncSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser state-sync ready state blocked for profile '{stateSyncSession.ProfileId}'.";
            result.Error = stateSyncSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateSyncReadyStateVersion = "runtime-browser-state-sync-ready-state-v1";
        result.BrowserStateSyncReadyChecks =
        [
            "browser-route-ready-state-ready",
            "browser-state-sync-session-ready",
            "browser-state-sync-ready"
        ];
        result.BrowserStateSyncReadySummary = $"Runtime browser state-sync ready state passed {result.BrowserStateSyncReadyChecks.Length} state-sync readiness check(s) for profile '{stateSyncSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser state-sync ready state ready for profile '{stateSyncSession.ProfileId}' with {result.BrowserStateSyncReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateSyncReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserStateSyncReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateSyncSessionVersion { get; set; } = string.Empty;
    public string BrowserRouteReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRouteSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateSyncReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserStateSyncReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserStateSyncSessionResult BrowserStateSyncSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
