namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserShortcutSession
{
    ValueTask<BrowserClientRuntimeBrowserShortcutSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserShortcutSessionService : IBrowserClientRuntimeBrowserShortcutSession
{
    private readonly IBrowserClientRuntimeBrowserFocusReadyState _runtimeBrowserFocusReadyState;

    public BrowserClientRuntimeBrowserShortcutSessionService(IBrowserClientRuntimeBrowserFocusReadyState runtimeBrowserFocusReadyState)
    {
        _runtimeBrowserFocusReadyState = runtimeBrowserFocusReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserShortcutSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFocusReadyStateResult focusReadyState = await _runtimeBrowserFocusReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserShortcutSessionResult result = new()
        {
            ProfileId = focusReadyState.ProfileId,
            SessionId = focusReadyState.SessionId,
            SessionPath = focusReadyState.SessionPath,
            BrowserFocusReadyStateVersion = focusReadyState.BrowserFocusReadyStateVersion,
            BrowserFocusSessionVersion = focusReadyState.BrowserFocusSessionVersion,
            LaunchMode = focusReadyState.LaunchMode,
            AssetRootPath = focusReadyState.AssetRootPath,
            ProfilesRootPath = focusReadyState.ProfilesRootPath,
            CacheRootPath = focusReadyState.CacheRootPath,
            ConfigRootPath = focusReadyState.ConfigRootPath,
            SettingsFilePath = focusReadyState.SettingsFilePath,
            StartupProfilePath = focusReadyState.StartupProfilePath,
            RequiredAssets = focusReadyState.RequiredAssets,
            ReadyAssetCount = focusReadyState.ReadyAssetCount,
            CompletedSteps = focusReadyState.CompletedSteps,
            TotalSteps = focusReadyState.TotalSteps,
            Exists = focusReadyState.Exists,
            ReadSucceeded = focusReadyState.ReadSucceeded,
            BrowserFocusReadyState = focusReadyState
        };

        if (!focusReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser shortcut session blocked for profile '{focusReadyState.ProfileId}'.";
            result.Error = focusReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserShortcutSessionVersion = "runtime-browser-shortcut-session-v1";
        result.BrowserShortcutStages =
        [
            "open-browser-shortcut-session",
            "bind-browser-focus-ready-state",
            "publish-browser-shortcut-ready"
        ];
        result.BrowserShortcutSummary = $"Runtime browser shortcut session prepared {result.BrowserShortcutStages.Length} shortcut stage(s) for profile '{focusReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser shortcut session ready for profile '{focusReadyState.ProfileId}' with {result.BrowserShortcutStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserShortcutSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserShortcutStages { get; set; } = Array.Empty<string>();
    public string BrowserShortcutSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserFocusReadyStateResult BrowserFocusReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
