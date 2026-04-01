namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserShortcutReadyState
{
    ValueTask<BrowserClientRuntimeBrowserShortcutReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserShortcutReadyStateService : IBrowserClientRuntimeBrowserShortcutReadyState
{
    private readonly IBrowserClientRuntimeBrowserShortcutSession _runtimeBrowserShortcutSession;

    public BrowserClientRuntimeBrowserShortcutReadyStateService(IBrowserClientRuntimeBrowserShortcutSession runtimeBrowserShortcutSession)
    {
        _runtimeBrowserShortcutSession = runtimeBrowserShortcutSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserShortcutReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserShortcutSessionResult shortcutSession = await _runtimeBrowserShortcutSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserShortcutReadyStateResult result = new()
        {
            ProfileId = shortcutSession.ProfileId,
            SessionId = shortcutSession.SessionId,
            SessionPath = shortcutSession.SessionPath,
            BrowserShortcutSessionVersion = shortcutSession.BrowserShortcutSessionVersion,
            BrowserFocusReadyStateVersion = shortcutSession.BrowserFocusReadyStateVersion,
            BrowserFocusSessionVersion = shortcutSession.BrowserFocusSessionVersion,
            LaunchMode = shortcutSession.LaunchMode,
            AssetRootPath = shortcutSession.AssetRootPath,
            ProfilesRootPath = shortcutSession.ProfilesRootPath,
            CacheRootPath = shortcutSession.CacheRootPath,
            ConfigRootPath = shortcutSession.ConfigRootPath,
            SettingsFilePath = shortcutSession.SettingsFilePath,
            StartupProfilePath = shortcutSession.StartupProfilePath,
            RequiredAssets = shortcutSession.RequiredAssets,
            ReadyAssetCount = shortcutSession.ReadyAssetCount,
            CompletedSteps = shortcutSession.CompletedSteps,
            TotalSteps = shortcutSession.TotalSteps,
            Exists = shortcutSession.Exists,
            ReadSucceeded = shortcutSession.ReadSucceeded,
            BrowserShortcutSession = shortcutSession
        };

        if (!shortcutSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser shortcut ready state blocked for profile '{shortcutSession.ProfileId}'.";
            result.Error = shortcutSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserShortcutReadyStateVersion = "runtime-browser-shortcut-ready-state-v1";
        result.BrowserShortcutReadyChecks =
        [
            "browser-focus-ready-state-ready",
            "browser-shortcut-session-ready",
            "browser-shortcut-ready"
        ];
        result.BrowserShortcutReadySummary = $"Runtime browser shortcut ready state passed {result.BrowserShortcutReadyChecks.Length} shortcut readiness check(s) for profile '{shortcutSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser shortcut ready state ready for profile '{shortcutSession.ProfileId}' with {result.BrowserShortcutReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserShortcutReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserShortcutReadyStateVersion { get; set; } = string.Empty;
    public string BrowserShortcutSessionVersion { get; set; } = string.Empty;
    public string BrowserFocusReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFocusSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserShortcutReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserShortcutReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserShortcutSessionResult BrowserShortcutSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
