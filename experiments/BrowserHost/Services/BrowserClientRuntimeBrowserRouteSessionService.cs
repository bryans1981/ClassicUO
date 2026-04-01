namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRouteSession
{
    ValueTask<BrowserClientRuntimeBrowserRouteSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRouteSessionService : IBrowserClientRuntimeBrowserRouteSession
{
    private readonly IBrowserClientRuntimeBrowserLifecycleReadyState _runtimeBrowserLifecycleReadyState;

    public BrowserClientRuntimeBrowserRouteSessionService(IBrowserClientRuntimeBrowserLifecycleReadyState runtimeBrowserLifecycleReadyState)
    {
        _runtimeBrowserLifecycleReadyState = runtimeBrowserLifecycleReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRouteSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLifecycleReadyStateResult lifecycleReadyState = await _runtimeBrowserLifecycleReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRouteSessionResult result = new()
        {
            ProfileId = lifecycleReadyState.ProfileId,
            SessionId = lifecycleReadyState.SessionId,
            SessionPath = lifecycleReadyState.SessionPath,
            BrowserLifecycleReadyStateVersion = lifecycleReadyState.BrowserLifecycleReadyStateVersion,
            BrowserLifecycleSessionVersion = lifecycleReadyState.BrowserLifecycleSessionVersion,
            LaunchMode = lifecycleReadyState.LaunchMode,
            AssetRootPath = lifecycleReadyState.AssetRootPath,
            ProfilesRootPath = lifecycleReadyState.ProfilesRootPath,
            CacheRootPath = lifecycleReadyState.CacheRootPath,
            ConfigRootPath = lifecycleReadyState.ConfigRootPath,
            SettingsFilePath = lifecycleReadyState.SettingsFilePath,
            StartupProfilePath = lifecycleReadyState.StartupProfilePath,
            RequiredAssets = lifecycleReadyState.RequiredAssets,
            ReadyAssetCount = lifecycleReadyState.ReadyAssetCount,
            CompletedSteps = lifecycleReadyState.CompletedSteps,
            TotalSteps = lifecycleReadyState.TotalSteps,
            Exists = lifecycleReadyState.Exists,
            ReadSucceeded = lifecycleReadyState.ReadSucceeded,
            BrowserLifecycleReadyState = lifecycleReadyState
        };

        if (!lifecycleReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser route session blocked for profile '{lifecycleReadyState.ProfileId}'.";
            result.Error = lifecycleReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRouteSessionVersion = "runtime-browser-route-session-v1";
        result.BrowserRouteStages =
        [
            "open-browser-route-session",
            "bind-browser-lifecycle-ready-state",
            "publish-browser-route-ready"
        ];
        result.BrowserRouteSummary = $"Runtime browser route session prepared {result.BrowserRouteStages.Length} route stage(s) for profile '{lifecycleReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser route session ready for profile '{lifecycleReadyState.ProfileId}' with {result.BrowserRouteStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRouteSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserRouteStages { get; set; } = Array.Empty<string>();
    public string BrowserRouteSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserLifecycleReadyStateResult BrowserLifecycleReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
