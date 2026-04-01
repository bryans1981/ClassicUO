namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEventSession
{
    ValueTask<BrowserClientRuntimeBrowserEventSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEventSessionService : IBrowserClientRuntimeBrowserEventSession
{
    private readonly IBrowserClientRuntimeBrowserInputReadyState _runtimeBrowserInputReadyState;

    public BrowserClientRuntimeBrowserEventSessionService(IBrowserClientRuntimeBrowserInputReadyState runtimeBrowserInputReadyState)
    {
        _runtimeBrowserInputReadyState = runtimeBrowserInputReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEventSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInputReadyStateResult inputReadyState = await _runtimeBrowserInputReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEventSessionResult result = new()
        {
            ProfileId = inputReadyState.ProfileId,
            SessionId = inputReadyState.SessionId,
            SessionPath = inputReadyState.SessionPath,
            BrowserInputReadyStateVersion = inputReadyState.BrowserInputReadyStateVersion,
            BrowserInputSessionVersion = inputReadyState.BrowserInputSessionVersion,
            LaunchMode = inputReadyState.LaunchMode,
            AssetRootPath = inputReadyState.AssetRootPath,
            ProfilesRootPath = inputReadyState.ProfilesRootPath,
            CacheRootPath = inputReadyState.CacheRootPath,
            ConfigRootPath = inputReadyState.ConfigRootPath,
            SettingsFilePath = inputReadyState.SettingsFilePath,
            StartupProfilePath = inputReadyState.StartupProfilePath,
            RequiredAssets = inputReadyState.RequiredAssets,
            ReadyAssetCount = inputReadyState.ReadyAssetCount,
            CompletedSteps = inputReadyState.CompletedSteps,
            TotalSteps = inputReadyState.TotalSteps,
            Exists = inputReadyState.Exists,
            ReadSucceeded = inputReadyState.ReadSucceeded,
            BrowserInputReadyState = inputReadyState
        };

        if (!inputReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser event session blocked for profile '{inputReadyState.ProfileId}'.";
            result.Error = inputReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEventSessionVersion = "runtime-browser-event-session-v1";
        result.BrowserEventStages =
        [
            "open-browser-event-session",
            "bind-browser-input-ready-state",
            "publish-browser-event-ready"
        ];
        result.BrowserEventSummary = $"Runtime browser event session prepared {result.BrowserEventStages.Length} event stage(s) for profile '{inputReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser event session ready for profile '{inputReadyState.ProfileId}' with {result.BrowserEventStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEventSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserEventSessionVersion { get; set; } = string.Empty;
    public string BrowserInputReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInputSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEventStages { get; set; } = Array.Empty<string>();
    public string BrowserEventSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserInputReadyStateResult BrowserInputReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
