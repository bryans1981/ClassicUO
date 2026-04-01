namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDisplayReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDisplayReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDisplayReadyStateService : IBrowserClientRuntimeBrowserDisplayReadyState
{
    private readonly IBrowserClientRuntimeBrowserDisplaySession _runtimeBrowserDisplaySession;

    public BrowserClientRuntimeBrowserDisplayReadyStateService(IBrowserClientRuntimeBrowserDisplaySession runtimeBrowserDisplaySession)
    {
        _runtimeBrowserDisplaySession = runtimeBrowserDisplaySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDisplayReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDisplaySessionResult displaySession = await _runtimeBrowserDisplaySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDisplayReadyStateResult result = new()
        {
            ProfileId = displaySession.ProfileId,
            SessionId = displaySession.SessionId,
            SessionPath = displaySession.SessionPath,
            BrowserDisplaySessionVersion = displaySession.BrowserDisplaySessionVersion,
            BrowserPresentReadyStateVersion = displaySession.BrowserPresentReadyStateVersion,
            BrowserPresentSessionVersion = displaySession.BrowserPresentSessionVersion,
            LaunchMode = displaySession.LaunchMode,
            AssetRootPath = displaySession.AssetRootPath,
            ProfilesRootPath = displaySession.ProfilesRootPath,
            CacheRootPath = displaySession.CacheRootPath,
            ConfigRootPath = displaySession.ConfigRootPath,
            SettingsFilePath = displaySession.SettingsFilePath,
            StartupProfilePath = displaySession.StartupProfilePath,
            RequiredAssets = displaySession.RequiredAssets,
            ReadyAssetCount = displaySession.ReadyAssetCount,
            CompletedSteps = displaySession.CompletedSteps,
            TotalSteps = displaySession.TotalSteps,
            Exists = displaySession.Exists,
            ReadSucceeded = displaySession.ReadSucceeded,
            BrowserDisplaySession = displaySession
        };

        if (!displaySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser display ready state blocked for profile '{displaySession.ProfileId}'.";
            result.Error = displaySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDisplayReadyStateVersion = "runtime-browser-display-ready-state-v1";
        result.BrowserDisplayReadyChecks =
        [
            "browser-present-ready-state-ready",
            "browser-display-session-ready",
            "browser-display-ready"
        ];
        result.BrowserDisplayReadySummary = $"Runtime browser display ready state passed {result.BrowserDisplayReadyChecks.Length} display readiness check(s) for profile '{displaySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser display ready state ready for profile '{displaySession.ProfileId}' with {result.BrowserDisplayReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDisplayReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDisplayReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDisplaySessionVersion { get; set; } = string.Empty;
    public string BrowserPresentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPresentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDisplayReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDisplayReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserDisplaySessionResult BrowserDisplaySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
