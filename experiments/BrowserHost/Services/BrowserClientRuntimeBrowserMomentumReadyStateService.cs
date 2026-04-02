namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMomentumReadyState
{
    ValueTask<BrowserClientRuntimeBrowserMomentumReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMomentumReadyStateService : IBrowserClientRuntimeBrowserMomentumReadyState
{
    private readonly IBrowserClientRuntimeBrowserMomentumSession _runtimeBrowserMomentumSession;

    public BrowserClientRuntimeBrowserMomentumReadyStateService(IBrowserClientRuntimeBrowserMomentumSession runtimeBrowserMomentumSession)
    {
        _runtimeBrowserMomentumSession = runtimeBrowserMomentumSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMomentumReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMomentumSessionResult momentumSession = await _runtimeBrowserMomentumSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserMomentumReadyStateResult result = new()
        {
            ProfileId = momentumSession.ProfileId,
            SessionId = momentumSession.SessionId,
            SessionPath = momentumSession.SessionPath,
            BrowserMomentumSessionVersion = momentumSession.BrowserMomentumSessionVersion,
            BrowserAimfulnessReadyStateVersion = momentumSession.BrowserAimfulnessReadyStateVersion,
            BrowserAimfulnessSessionVersion = momentumSession.BrowserAimfulnessSessionVersion,
            LaunchMode = momentumSession.LaunchMode,
            AssetRootPath = momentumSession.AssetRootPath,
            ProfilesRootPath = momentumSession.ProfilesRootPath,
            CacheRootPath = momentumSession.CacheRootPath,
            ConfigRootPath = momentumSession.ConfigRootPath,
            SettingsFilePath = momentumSession.SettingsFilePath,
            StartupProfilePath = momentumSession.StartupProfilePath,
            RequiredAssets = momentumSession.RequiredAssets,
            ReadyAssetCount = momentumSession.ReadyAssetCount,
            CompletedSteps = momentumSession.CompletedSteps,
            TotalSteps = momentumSession.TotalSteps,
            Exists = momentumSession.Exists,
            ReadSucceeded = momentumSession.ReadSucceeded
        };

        if (!momentumSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser momentum ready state blocked for profile '{momentumSession.ProfileId}'.";
            result.Error = momentumSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMomentumReadyStateVersion = "runtime-browser-momentum-ready-state-v1";
        result.BrowserMomentumReadyChecks =
        [
            "browser-aimfulness-ready-state-ready",
            "browser-momentum-session-ready",
            "browser-momentum-ready"
        ];
        result.BrowserMomentumReadySummary = $"Runtime browser momentum ready state passed {result.BrowserMomentumReadyChecks.Length} momentum readiness check(s) for profile '{momentumSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser momentum ready state ready for profile '{momentumSession.ProfileId}' with {result.BrowserMomentumReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMomentumReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserMomentumReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMomentumSessionVersion { get; set; } = string.Empty;
    public string BrowserAimfulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAimfulnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMomentumReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserMomentumReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
