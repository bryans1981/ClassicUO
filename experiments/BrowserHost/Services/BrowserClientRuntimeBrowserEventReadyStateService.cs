namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEventReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEventReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEventReadyStateService : IBrowserClientRuntimeBrowserEventReadyState
{
    private readonly IBrowserClientRuntimeBrowserEventSession _runtimeBrowserEventSession;

    public BrowserClientRuntimeBrowserEventReadyStateService(IBrowserClientRuntimeBrowserEventSession runtimeBrowserEventSession)
    {
        _runtimeBrowserEventSession = runtimeBrowserEventSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEventReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEventSessionResult eventSession = await _runtimeBrowserEventSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEventReadyStateResult result = new()
        {
            ProfileId = eventSession.ProfileId,
            SessionId = eventSession.SessionId,
            SessionPath = eventSession.SessionPath,
            BrowserEventSessionVersion = eventSession.BrowserEventSessionVersion,
            BrowserInputReadyStateVersion = eventSession.BrowserInputReadyStateVersion,
            BrowserInputSessionVersion = eventSession.BrowserInputSessionVersion,
            LaunchMode = eventSession.LaunchMode,
            AssetRootPath = eventSession.AssetRootPath,
            ProfilesRootPath = eventSession.ProfilesRootPath,
            CacheRootPath = eventSession.CacheRootPath,
            ConfigRootPath = eventSession.ConfigRootPath,
            SettingsFilePath = eventSession.SettingsFilePath,
            StartupProfilePath = eventSession.StartupProfilePath,
            RequiredAssets = eventSession.RequiredAssets,
            ReadyAssetCount = eventSession.ReadyAssetCount,
            CompletedSteps = eventSession.CompletedSteps,
            TotalSteps = eventSession.TotalSteps,
            Exists = eventSession.Exists,
            ReadSucceeded = eventSession.ReadSucceeded,
            BrowserEventSession = eventSession
        };

        if (!eventSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser event ready state blocked for profile '{eventSession.ProfileId}'.";
            result.Error = eventSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEventReadyStateVersion = "runtime-browser-event-ready-state-v1";
        result.BrowserEventReadyChecks =
        [
            "browser-input-ready-state-ready",
            "browser-event-session-ready",
            "browser-event-ready"
        ];
        result.BrowserEventReadySummary = $"Runtime browser event ready state passed {result.BrowserEventReadyChecks.Length} event readiness check(s) for profile '{eventSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser event ready state ready for profile '{eventSession.ProfileId}' with {result.BrowserEventReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEventReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEventReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserEventReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEventReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserEventSessionResult BrowserEventSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
