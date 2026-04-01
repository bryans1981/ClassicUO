namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFocusReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFocusReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFocusReadyStateService : IBrowserClientRuntimeBrowserFocusReadyState
{
    private readonly IBrowserClientRuntimeBrowserFocusSession _runtimeBrowserFocusSession;

    public BrowserClientRuntimeBrowserFocusReadyStateService(IBrowserClientRuntimeBrowserFocusSession runtimeBrowserFocusSession)
    {
        _runtimeBrowserFocusSession = runtimeBrowserFocusSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFocusReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFocusSessionResult focusSession = await _runtimeBrowserFocusSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFocusReadyStateResult result = new()
        {
            ProfileId = focusSession.ProfileId,
            SessionId = focusSession.SessionId,
            SessionPath = focusSession.SessionPath,
            BrowserFocusSessionVersion = focusSession.BrowserFocusSessionVersion,
            BrowserInteractionReadyStateVersion = focusSession.BrowserInteractionReadyStateVersion,
            BrowserInteractionSessionVersion = focusSession.BrowserInteractionSessionVersion,
            LaunchMode = focusSession.LaunchMode,
            AssetRootPath = focusSession.AssetRootPath,
            ProfilesRootPath = focusSession.ProfilesRootPath,
            CacheRootPath = focusSession.CacheRootPath,
            ConfigRootPath = focusSession.ConfigRootPath,
            SettingsFilePath = focusSession.SettingsFilePath,
            StartupProfilePath = focusSession.StartupProfilePath,
            RequiredAssets = focusSession.RequiredAssets,
            ReadyAssetCount = focusSession.ReadyAssetCount,
            CompletedSteps = focusSession.CompletedSteps,
            TotalSteps = focusSession.TotalSteps,
            Exists = focusSession.Exists,
            ReadSucceeded = focusSession.ReadSucceeded,
            BrowserFocusSession = focusSession
        };

        if (!focusSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser focus ready state blocked for profile '{focusSession.ProfileId}'.";
            result.Error = focusSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFocusReadyStateVersion = "runtime-browser-focus-ready-state-v1";
        result.BrowserFocusReadyChecks =
        [
            "browser-interaction-ready-state-ready",
            "browser-focus-session-ready",
            "browser-focus-ready"
        ];
        result.BrowserFocusReadySummary = $"Runtime browser focus ready state passed {result.BrowserFocusReadyChecks.Length} focus readiness check(s) for profile '{focusSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser focus ready state ready for profile '{focusSession.ProfileId}' with {result.BrowserFocusReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFocusReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFocusReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFocusSessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFocusReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFocusReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserFocusSessionResult BrowserFocusSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
