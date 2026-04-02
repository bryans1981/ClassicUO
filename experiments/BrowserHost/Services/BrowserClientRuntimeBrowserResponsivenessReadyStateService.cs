namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResponsivenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserResponsivenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResponsivenessReadyStateService : IBrowserClientRuntimeBrowserResponsivenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserResponsivenessSession _runtimeBrowserResponsivenessSession;

    public BrowserClientRuntimeBrowserResponsivenessReadyStateService(IBrowserClientRuntimeBrowserResponsivenessSession runtimeBrowserResponsivenessSession)
    {
        _runtimeBrowserResponsivenessSession = runtimeBrowserResponsivenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResponsivenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResponsivenessSessionResult responsivenessSession = await _runtimeBrowserResponsivenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserResponsivenessReadyStateResult result = new()
        {
            ProfileId = responsivenessSession.ProfileId,
            SessionId = responsivenessSession.SessionId,
            SessionPath = responsivenessSession.SessionPath,
            BrowserResponsivenessSessionVersion = responsivenessSession.BrowserResponsivenessSessionVersion,
            BrowserVisibilityReadyStateVersion = responsivenessSession.BrowserVisibilityReadyStateVersion,
            BrowserVisibilitySessionVersion = responsivenessSession.BrowserVisibilitySessionVersion,
            LaunchMode = responsivenessSession.LaunchMode,
            AssetRootPath = responsivenessSession.AssetRootPath,
            ProfilesRootPath = responsivenessSession.ProfilesRootPath,
            CacheRootPath = responsivenessSession.CacheRootPath,
            ConfigRootPath = responsivenessSession.ConfigRootPath,
            SettingsFilePath = responsivenessSession.SettingsFilePath,
            StartupProfilePath = responsivenessSession.StartupProfilePath,
            RequiredAssets = responsivenessSession.RequiredAssets,
            ReadyAssetCount = responsivenessSession.ReadyAssetCount,
            CompletedSteps = responsivenessSession.CompletedSteps,
            TotalSteps = responsivenessSession.TotalSteps,
            Exists = responsivenessSession.Exists,
            ReadSucceeded = responsivenessSession.ReadSucceeded
        };

        if (!responsivenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser responsiveness ready state blocked for profile '{responsivenessSession.ProfileId}'.";
            result.Error = responsivenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResponsivenessReadyStateVersion = "runtime-browser-responsiveness-ready-state-v1";
        result.BrowserResponsivenessReadyChecks =
        [
            "browser-visibility-ready-state-ready",
            "browser-responsiveness-session-ready",
            "browser-responsiveness-ready"
        ];
        result.BrowserResponsivenessReadySummary = $"Runtime browser responsiveness ready state passed {result.BrowserResponsivenessReadyChecks.Length} responsiveness readiness check(s) for profile '{responsivenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser responsiveness ready state ready for profile '{responsivenessSession.ProfileId}' with {result.BrowserResponsivenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResponsivenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserResponsivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResponsivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserVisibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserVisibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResponsivenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserResponsivenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
