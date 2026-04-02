namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComfortReadyState
{
    ValueTask<BrowserClientRuntimeBrowserComfortReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComfortReadyStateService : IBrowserClientRuntimeBrowserComfortReadyState
{
    private readonly IBrowserClientRuntimeBrowserComfortSession _runtimeBrowserComfortSession;

    public BrowserClientRuntimeBrowserComfortReadyStateService(IBrowserClientRuntimeBrowserComfortSession runtimeBrowserComfortSession)
    {
        _runtimeBrowserComfortSession = runtimeBrowserComfortSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComfortReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComfortSessionResult comfortSession = await _runtimeBrowserComfortSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserComfortReadyStateResult result = new()
        {
            ProfileId = comfortSession.ProfileId,
            SessionId = comfortSession.SessionId,
            SessionPath = comfortSession.SessionPath,
            BrowserComfortSessionVersion = comfortSession.BrowserComfortSessionVersion,
            BrowserConfidenceReadyStateVersion = comfortSession.BrowserConfidenceReadyStateVersion,
            BrowserConfidenceSessionVersion = comfortSession.BrowserConfidenceSessionVersion,
            LaunchMode = comfortSession.LaunchMode,
            AssetRootPath = comfortSession.AssetRootPath,
            ProfilesRootPath = comfortSession.ProfilesRootPath,
            CacheRootPath = comfortSession.CacheRootPath,
            ConfigRootPath = comfortSession.ConfigRootPath,
            SettingsFilePath = comfortSession.SettingsFilePath,
            StartupProfilePath = comfortSession.StartupProfilePath,
            RequiredAssets = comfortSession.RequiredAssets,
            ReadyAssetCount = comfortSession.ReadyAssetCount,
            CompletedSteps = comfortSession.CompletedSteps,
            TotalSteps = comfortSession.TotalSteps,
            Exists = comfortSession.Exists,
            ReadSucceeded = comfortSession.ReadSucceeded
        };

        if (!comfortSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser comfort ready state blocked for profile '{comfortSession.ProfileId}'.";
            result.Error = comfortSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComfortReadyStateVersion = "runtime-browser-comfort-ready-state-v1";
        result.BrowserComfortReadyChecks =
        [
            "browser-confidence-ready-state-ready",
            "browser-comfort-session-ready",
            "browser-comfort-ready"
        ];
        result.BrowserComfortReadySummary = $"Runtime browser comfort ready state passed {result.BrowserComfortReadyChecks.Length} comfort readiness check(s) for profile '{comfortSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser comfort ready state ready for profile '{comfortSession.ProfileId}' with {result.BrowserComfortReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComfortReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserComfortReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComfortSessionVersion { get; set; } = string.Empty;
    public string BrowserConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComfortReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserComfortReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
