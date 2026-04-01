namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInputReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInputReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInputReadyStateService : IBrowserClientRuntimeBrowserInputReadyState
{
    private readonly IBrowserClientRuntimeBrowserInputSession _runtimeBrowserInputSession;

    public BrowserClientRuntimeBrowserInputReadyStateService(IBrowserClientRuntimeBrowserInputSession runtimeBrowserInputSession)
    {
        _runtimeBrowserInputSession = runtimeBrowserInputSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInputReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInputSessionResult inputSession = await _runtimeBrowserInputSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserInputReadyStateResult result = new()
        {
            ProfileId = inputSession.ProfileId,
            SessionId = inputSession.SessionId,
            SessionPath = inputSession.SessionPath,
            BrowserInputSessionVersion = inputSession.BrowserInputSessionVersion,
            BrowserSceneReadyStateVersion = inputSession.BrowserSceneReadyStateVersion,
            BrowserSceneSessionVersion = inputSession.BrowserSceneSessionVersion,
            LaunchMode = inputSession.LaunchMode,
            AssetRootPath = inputSession.AssetRootPath,
            ProfilesRootPath = inputSession.ProfilesRootPath,
            CacheRootPath = inputSession.CacheRootPath,
            ConfigRootPath = inputSession.ConfigRootPath,
            SettingsFilePath = inputSession.SettingsFilePath,
            StartupProfilePath = inputSession.StartupProfilePath,
            RequiredAssets = inputSession.RequiredAssets,
            ReadyAssetCount = inputSession.ReadyAssetCount,
            CompletedSteps = inputSession.CompletedSteps,
            TotalSteps = inputSession.TotalSteps,
            Exists = inputSession.Exists,
            ReadSucceeded = inputSession.ReadSucceeded,
            BrowserInputSession = inputSession
        };

        if (!inputSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser input ready state blocked for profile '{inputSession.ProfileId}'.";
            result.Error = inputSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInputReadyStateVersion = "runtime-browser-input-ready-state-v1";
        result.BrowserInputReadyChecks =
        [
            "browser-scene-ready-state-ready",
            "browser-input-session-ready",
            "browser-input-ready"
        ];
        result.BrowserInputReadySummary = $"Runtime browser input ready state passed {result.BrowserInputReadyChecks.Length} input readiness check(s) for profile '{inputSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser input ready state ready for profile '{inputSession.ProfileId}' with {result.BrowserInputReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInputReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInputReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInputSessionVersion { get; set; } = string.Empty;
    public string BrowserSceneReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSceneSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInputReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInputReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserInputSessionResult BrowserInputSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
