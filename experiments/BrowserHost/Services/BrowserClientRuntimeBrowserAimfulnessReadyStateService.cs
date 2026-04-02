namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAimfulnessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAimfulnessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAimfulnessReadyStateService : IBrowserClientRuntimeBrowserAimfulnessReadyState
{
    private readonly IBrowserClientRuntimeBrowserAimfulnessSession _runtimeBrowserAimfulnessSession;

    public BrowserClientRuntimeBrowserAimfulnessReadyStateService(IBrowserClientRuntimeBrowserAimfulnessSession runtimeBrowserAimfulnessSession)
    {
        _runtimeBrowserAimfulnessSession = runtimeBrowserAimfulnessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAimfulnessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAimfulnessSessionResult aimfulnessSession = await _runtimeBrowserAimfulnessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAimfulnessReadyStateResult result = new()
        {
            ProfileId = aimfulnessSession.ProfileId,
            SessionId = aimfulnessSession.SessionId,
            SessionPath = aimfulnessSession.SessionPath,
            BrowserAimfulnessSessionVersion = aimfulnessSession.BrowserAimfulnessSessionVersion,
            BrowserTaskFitReadyStateVersion = aimfulnessSession.BrowserTaskFitReadyStateVersion,
            BrowserTaskFitSessionVersion = aimfulnessSession.BrowserTaskFitSessionVersion,
            LaunchMode = aimfulnessSession.LaunchMode,
            AssetRootPath = aimfulnessSession.AssetRootPath,
            ProfilesRootPath = aimfulnessSession.ProfilesRootPath,
            CacheRootPath = aimfulnessSession.CacheRootPath,
            ConfigRootPath = aimfulnessSession.ConfigRootPath,
            SettingsFilePath = aimfulnessSession.SettingsFilePath,
            StartupProfilePath = aimfulnessSession.StartupProfilePath,
            RequiredAssets = aimfulnessSession.RequiredAssets,
            ReadyAssetCount = aimfulnessSession.ReadyAssetCount,
            CompletedSteps = aimfulnessSession.CompletedSteps,
            TotalSteps = aimfulnessSession.TotalSteps,
            Exists = aimfulnessSession.Exists,
            ReadSucceeded = aimfulnessSession.ReadSucceeded
        };

        if (!aimfulnessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser aimfulness ready state blocked for profile '{aimfulnessSession.ProfileId}'.";
            result.Error = aimfulnessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAimfulnessReadyStateVersion = "runtime-browser-aimfulness-ready-state-v1";
        result.BrowserAimfulnessReadyChecks =
        [
            "browser-taskfit-ready-state-ready",
            "browser-aimfulness-session-ready",
            "browser-aimfulness-ready"
        ];
        result.BrowserAimfulnessReadySummary = $"Runtime browser aimfulness ready state passed {result.BrowserAimfulnessReadyChecks.Length} aimfulness readiness check(s) for profile '{aimfulnessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser aimfulness ready state ready for profile '{aimfulnessSession.ProfileId}' with {result.BrowserAimfulnessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAimfulnessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAimfulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAimfulnessSessionVersion { get; set; } = string.Empty;
    public string BrowserTaskFitReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskFitSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAimfulnessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAimfulnessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
