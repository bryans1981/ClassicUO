namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserViewportReadyState
{
    ValueTask<BrowserClientRuntimeBrowserViewportReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserViewportReadyStateService : IBrowserClientRuntimeBrowserViewportReadyState
{
    private readonly IBrowserClientRuntimeBrowserViewportSession _runtimeBrowserViewportSession;

    public BrowserClientRuntimeBrowserViewportReadyStateService(IBrowserClientRuntimeBrowserViewportSession runtimeBrowserViewportSession)
    {
        _runtimeBrowserViewportSession = runtimeBrowserViewportSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserViewportReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserViewportSessionResult viewportSession = await _runtimeBrowserViewportSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserViewportReadyStateResult result = new()
        {
            ProfileId = viewportSession.ProfileId,
            SessionId = viewportSession.SessionId,
            SessionPath = viewportSession.SessionPath,
            BrowserViewportSessionVersion = viewportSession.BrowserViewportSessionVersion,
            BrowserDisplayReadyStateVersion = viewportSession.BrowserDisplayReadyStateVersion,
            BrowserDisplaySessionVersion = viewportSession.BrowserDisplaySessionVersion,
            LaunchMode = viewportSession.LaunchMode,
            AssetRootPath = viewportSession.AssetRootPath,
            ProfilesRootPath = viewportSession.ProfilesRootPath,
            CacheRootPath = viewportSession.CacheRootPath,
            ConfigRootPath = viewportSession.ConfigRootPath,
            SettingsFilePath = viewportSession.SettingsFilePath,
            StartupProfilePath = viewportSession.StartupProfilePath,
            RequiredAssets = viewportSession.RequiredAssets,
            ReadyAssetCount = viewportSession.ReadyAssetCount,
            CompletedSteps = viewportSession.CompletedSteps,
            TotalSteps = viewportSession.TotalSteps,
            Exists = viewportSession.Exists,
            ReadSucceeded = viewportSession.ReadSucceeded,
            BrowserViewportSession = viewportSession
        };

        if (!viewportSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser viewport ready state blocked for profile '{viewportSession.ProfileId}'.";
            result.Error = viewportSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserViewportReadyStateVersion = "runtime-browser-viewport-ready-state-v1";
        result.BrowserViewportReadyChecks =
        [
            "browser-display-ready-state-ready",
            "browser-viewport-session-ready",
            "browser-viewport-ready"
        ];
        result.BrowserViewportReadySummary = $"Runtime browser viewport ready state passed {result.BrowserViewportReadyChecks.Length} viewport readiness check(s) for profile '{viewportSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser viewport ready state ready for profile '{viewportSession.ProfileId}' with {result.BrowserViewportReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserViewportReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserViewportReadyStateVersion { get; set; } = string.Empty;
    public string BrowserViewportSessionVersion { get; set; } = string.Empty;
    public string BrowserDisplayReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDisplaySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserViewportReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserViewportReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserViewportSessionResult BrowserViewportSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
