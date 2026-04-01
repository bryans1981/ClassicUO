namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPointerReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPointerReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPointerReadyStateService : IBrowserClientRuntimeBrowserPointerReadyState
{
    private readonly IBrowserClientRuntimeBrowserPointerSession _runtimeBrowserPointerSession;

    public BrowserClientRuntimeBrowserPointerReadyStateService(IBrowserClientRuntimeBrowserPointerSession runtimeBrowserPointerSession)
    {
        _runtimeBrowserPointerSession = runtimeBrowserPointerSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPointerReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPointerSessionResult pointerSession = await _runtimeBrowserPointerSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPointerReadyStateResult result = new()
        {
            ProfileId = pointerSession.ProfileId,
            SessionId = pointerSession.SessionId,
            SessionPath = pointerSession.SessionPath,
            BrowserPointerSessionVersion = pointerSession.BrowserPointerSessionVersion,
            BrowserShortcutReadyStateVersion = pointerSession.BrowserShortcutReadyStateVersion,
            BrowserShortcutSessionVersion = pointerSession.BrowserShortcutSessionVersion,
            LaunchMode = pointerSession.LaunchMode,
            AssetRootPath = pointerSession.AssetRootPath,
            ProfilesRootPath = pointerSession.ProfilesRootPath,
            CacheRootPath = pointerSession.CacheRootPath,
            ConfigRootPath = pointerSession.ConfigRootPath,
            SettingsFilePath = pointerSession.SettingsFilePath,
            StartupProfilePath = pointerSession.StartupProfilePath,
            RequiredAssets = pointerSession.RequiredAssets,
            ReadyAssetCount = pointerSession.ReadyAssetCount,
            CompletedSteps = pointerSession.CompletedSteps,
            TotalSteps = pointerSession.TotalSteps,
            Exists = pointerSession.Exists,
            ReadSucceeded = pointerSession.ReadSucceeded,
            BrowserPointerSession = pointerSession
        };

        if (!pointerSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser pointer ready state blocked for profile '{pointerSession.ProfileId}'.";
            result.Error = pointerSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPointerReadyStateVersion = "runtime-browser-pointer-ready-state-v1";
        result.BrowserPointerReadyChecks =
        [
            "browser-shortcut-ready-state-ready",
            "browser-pointer-session-ready",
            "browser-pointer-ready"
        ];
        result.BrowserPointerReadySummary = $"Runtime browser pointer ready state passed {result.BrowserPointerReadyChecks.Length} pointer readiness check(s) for profile '{pointerSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser pointer ready state ready for profile '{pointerSession.ProfileId}' with {result.BrowserPointerReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPointerReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPointerReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserPointerReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPointerReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPointerSessionResult BrowserPointerSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
