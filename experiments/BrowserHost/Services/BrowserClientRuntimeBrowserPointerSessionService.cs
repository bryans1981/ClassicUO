namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPointerSession
{
    ValueTask<BrowserClientRuntimeBrowserPointerSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPointerSessionService : IBrowserClientRuntimeBrowserPointerSession
{
    private readonly IBrowserClientRuntimeBrowserShortcutReadyState _runtimeBrowserShortcutReadyState;

    public BrowserClientRuntimeBrowserPointerSessionService(IBrowserClientRuntimeBrowserShortcutReadyState runtimeBrowserShortcutReadyState)
    {
        _runtimeBrowserShortcutReadyState = runtimeBrowserShortcutReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPointerSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserShortcutReadyStateResult shortcutReadyState = await _runtimeBrowserShortcutReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPointerSessionResult result = new()
        {
            ProfileId = shortcutReadyState.ProfileId,
            SessionId = shortcutReadyState.SessionId,
            SessionPath = shortcutReadyState.SessionPath,
            BrowserShortcutReadyStateVersion = shortcutReadyState.BrowserShortcutReadyStateVersion,
            BrowserShortcutSessionVersion = shortcutReadyState.BrowserShortcutSessionVersion,
            LaunchMode = shortcutReadyState.LaunchMode,
            AssetRootPath = shortcutReadyState.AssetRootPath,
            ProfilesRootPath = shortcutReadyState.ProfilesRootPath,
            CacheRootPath = shortcutReadyState.CacheRootPath,
            ConfigRootPath = shortcutReadyState.ConfigRootPath,
            SettingsFilePath = shortcutReadyState.SettingsFilePath,
            StartupProfilePath = shortcutReadyState.StartupProfilePath,
            RequiredAssets = shortcutReadyState.RequiredAssets,
            ReadyAssetCount = shortcutReadyState.ReadyAssetCount,
            CompletedSteps = shortcutReadyState.CompletedSteps,
            TotalSteps = shortcutReadyState.TotalSteps,
            Exists = shortcutReadyState.Exists,
            ReadSucceeded = shortcutReadyState.ReadSucceeded,
            BrowserShortcutReadyState = shortcutReadyState
        };

        if (!shortcutReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser pointer session blocked for profile '{shortcutReadyState.ProfileId}'.";
            result.Error = shortcutReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPointerSessionVersion = "runtime-browser-pointer-session-v1";
        result.BrowserPointerStages =
        [
            "open-browser-pointer-session",
            "bind-browser-shortcut-ready-state",
            "publish-browser-pointer-ready"
        ];
        result.BrowserPointerSummary = $"Runtime browser pointer session prepared {result.BrowserPointerStages.Length} pointer stage(s) for profile '{shortcutReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser pointer session ready for profile '{shortcutReadyState.ProfileId}' with {result.BrowserPointerStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPointerSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPointerSessionVersion { get; set; } = string.Empty;
    public string BrowserShortcutReadyStateVersion { get; set; } = string.Empty;
    public string BrowserShortcutSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPointerStages { get; set; } = Array.Empty<string>();
    public string BrowserPointerSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserShortcutReadyStateResult BrowserShortcutReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
