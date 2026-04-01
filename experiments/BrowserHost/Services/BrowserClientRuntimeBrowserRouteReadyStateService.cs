namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRouteReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRouteReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRouteReadyStateService : IBrowserClientRuntimeBrowserRouteReadyState
{
    private readonly IBrowserClientRuntimeBrowserRouteSession _runtimeBrowserRouteSession;

    public BrowserClientRuntimeBrowserRouteReadyStateService(IBrowserClientRuntimeBrowserRouteSession runtimeBrowserRouteSession)
    {
        _runtimeBrowserRouteSession = runtimeBrowserRouteSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRouteReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRouteSessionResult routeSession = await _runtimeBrowserRouteSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRouteReadyStateResult result = new()
        {
            ProfileId = routeSession.ProfileId,
            SessionId = routeSession.SessionId,
            SessionPath = routeSession.SessionPath,
            BrowserRouteSessionVersion = routeSession.BrowserRouteSessionVersion,
            BrowserLifecycleReadyStateVersion = routeSession.BrowserLifecycleReadyStateVersion,
            BrowserLifecycleSessionVersion = routeSession.BrowserLifecycleSessionVersion,
            LaunchMode = routeSession.LaunchMode,
            AssetRootPath = routeSession.AssetRootPath,
            ProfilesRootPath = routeSession.ProfilesRootPath,
            CacheRootPath = routeSession.CacheRootPath,
            ConfigRootPath = routeSession.ConfigRootPath,
            SettingsFilePath = routeSession.SettingsFilePath,
            StartupProfilePath = routeSession.StartupProfilePath,
            RequiredAssets = routeSession.RequiredAssets,
            ReadyAssetCount = routeSession.ReadyAssetCount,
            CompletedSteps = routeSession.CompletedSteps,
            TotalSteps = routeSession.TotalSteps,
            Exists = routeSession.Exists,
            ReadSucceeded = routeSession.ReadSucceeded,
            BrowserRouteSession = routeSession
        };

        if (!routeSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser route ready state blocked for profile '{routeSession.ProfileId}'.";
            result.Error = routeSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRouteReadyStateVersion = "runtime-browser-route-ready-state-v1";
        result.BrowserRouteReadyChecks =
        [
            "browser-lifecycle-ready-state-ready",
            "browser-route-session-ready",
            "browser-route-ready"
        ];
        result.BrowserRouteReadySummary = $"Runtime browser route ready state passed {result.BrowserRouteReadyChecks.Length} route readiness check(s) for profile '{routeSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser route ready state ready for profile '{routeSession.ProfileId}' with {result.BrowserRouteReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRouteReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRouteReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRouteSessionVersion { get; set; } = string.Empty;
    public string BrowserLifecycleReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLifecycleSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRouteReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRouteReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRouteSessionResult BrowserRouteSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
