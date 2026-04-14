namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyContinuityReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateService : IBrowserClientRuntimeBrowserSteadyContinuityReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserSteadyContinuityReadinessSession _runtimeBrowserSteadyContinuityReadinessSession;

    public BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateService(IBrowserClientRuntimeBrowserSteadyContinuityReadinessSession runtimeBrowserSteadyContinuityReadinessSession)
    {
        _runtimeBrowserSteadyContinuityReadinessSession = runtimeBrowserSteadyContinuityReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionResult steadycontinuityreadinessSession = await _runtimeBrowserSteadyContinuityReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateResult result = new()
        {
            ProfileId = steadycontinuityreadinessSession.ProfileId,
            SessionId = steadycontinuityreadinessSession.SessionId,
            SessionPath = steadycontinuityreadinessSession.SessionPath,
            BrowserSteadyContinuityReadinessSessionVersion = steadycontinuityreadinessSession.BrowserSteadyContinuityReadinessSessionVersion,
            BrowserLiveContinuityReadyStateVersion = steadycontinuityreadinessSession.BrowserLiveContinuityReadyStateVersion,
            BrowserLiveContinuitySessionVersion = steadycontinuityreadinessSession.BrowserLiveContinuitySessionVersion,
            LaunchMode = steadycontinuityreadinessSession.LaunchMode,
            AssetRootPath = steadycontinuityreadinessSession.AssetRootPath,
            ProfilesRootPath = steadycontinuityreadinessSession.ProfilesRootPath,
            CacheRootPath = steadycontinuityreadinessSession.CacheRootPath,
            ConfigRootPath = steadycontinuityreadinessSession.ConfigRootPath,
            SettingsFilePath = steadycontinuityreadinessSession.SettingsFilePath,
            StartupProfilePath = steadycontinuityreadinessSession.StartupProfilePath,
            RequiredAssets = steadycontinuityreadinessSession.RequiredAssets,
            ReadyAssetCount = steadycontinuityreadinessSession.ReadyAssetCount,
            CompletedSteps = steadycontinuityreadinessSession.CompletedSteps,
            TotalSteps = steadycontinuityreadinessSession.TotalSteps,
            Exists = steadycontinuityreadinessSession.Exists,
            ReadSucceeded = steadycontinuityreadinessSession.ReadSucceeded
        };

        if (!steadycontinuityreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadycontinuityreadiness ready state blocked for profile '{steadycontinuityreadinessSession.ProfileId}'.";
            result.Error = steadycontinuityreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyContinuityReadinessReadyStateVersion = "runtime-browser-steadycontinuityreadiness-ready-state-v1";
        result.BrowserSteadyContinuityReadinessReadyChecks =
        [
            "browser-livecontinuity-ready-state-ready",
            "browser-steadycontinuityreadiness-session-ready",
            "browser-steadycontinuityreadiness-ready"
        ];
        result.BrowserSteadyContinuityReadinessReadySummary = $"Runtime browser steadycontinuityreadiness ready state passed {result.BrowserSteadyContinuityReadinessReadyChecks.Length} steadycontinuityreadiness readiness check(s) for profile '{steadycontinuityreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadycontinuityreadiness ready state ready for profile '{steadycontinuityreadinessSession.ProfileId}' with {result.BrowserSteadyContinuityReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyContinuityReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyContinuityReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyContinuityReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLiveContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyContinuityReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSteadyContinuityReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
