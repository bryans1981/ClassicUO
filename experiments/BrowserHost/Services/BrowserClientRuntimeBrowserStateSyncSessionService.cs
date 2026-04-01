namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateSyncSession
{
    ValueTask<BrowserClientRuntimeBrowserStateSyncSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateSyncSessionService : IBrowserClientRuntimeBrowserStateSyncSession
{
    private readonly IBrowserClientRuntimeBrowserRouteReadyState _runtimeBrowserRouteReadyState;

    public BrowserClientRuntimeBrowserStateSyncSessionService(IBrowserClientRuntimeBrowserRouteReadyState runtimeBrowserRouteReadyState)
    {
        _runtimeBrowserRouteReadyState = runtimeBrowserRouteReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateSyncSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRouteReadyStateResult routeReadyState = await _runtimeBrowserRouteReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStateSyncSessionResult result = new()
        {
            ProfileId = routeReadyState.ProfileId,
            SessionId = routeReadyState.SessionId,
            SessionPath = routeReadyState.SessionPath,
            BrowserRouteReadyStateVersion = routeReadyState.BrowserRouteReadyStateVersion,
            BrowserRouteSessionVersion = routeReadyState.BrowserRouteSessionVersion,
            LaunchMode = routeReadyState.LaunchMode,
            AssetRootPath = routeReadyState.AssetRootPath,
            ProfilesRootPath = routeReadyState.ProfilesRootPath,
            CacheRootPath = routeReadyState.CacheRootPath,
            ConfigRootPath = routeReadyState.ConfigRootPath,
            SettingsFilePath = routeReadyState.SettingsFilePath,
            StartupProfilePath = routeReadyState.StartupProfilePath,
            RequiredAssets = routeReadyState.RequiredAssets,
            ReadyAssetCount = routeReadyState.ReadyAssetCount,
            CompletedSteps = routeReadyState.CompletedSteps,
            TotalSteps = routeReadyState.TotalSteps,
            Exists = routeReadyState.Exists,
            ReadSucceeded = routeReadyState.ReadSucceeded,
            BrowserRouteReadyState = routeReadyState
        };

        if (!routeReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser state-sync session blocked for profile '{routeReadyState.ProfileId}'.";
            result.Error = routeReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateSyncSessionVersion = "runtime-browser-state-sync-session-v1";
        result.BrowserStateSyncStages =
        [
            "open-browser-state-sync-session",
            "bind-browser-route-ready-state",
            "publish-browser-state-sync-ready"
        ];
        result.BrowserStateSyncSummary = $"Runtime browser state-sync session prepared {result.BrowserStateSyncStages.Length} state-sync stage(s) for profile '{routeReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser state-sync session ready for profile '{routeReadyState.ProfileId}' with {result.BrowserStateSyncStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateSyncSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserStateSyncStages { get; set; } = Array.Empty<string>();
    public string BrowserStateSyncSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRouteReadyStateResult BrowserRouteReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
