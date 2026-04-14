namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyStateReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateService : IBrowserClientRuntimeBrowserSteadyStateReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserSteadyStateReadinessSession _runtimeBrowserSteadyStateReadinessSession;

    public BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateService(IBrowserClientRuntimeBrowserSteadyStateReadinessSession runtimeBrowserSteadyStateReadinessSession)
    {
        _runtimeBrowserSteadyStateReadinessSession = runtimeBrowserSteadyStateReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyStateReadinessSessionResult steadystatereadinessSession = await _runtimeBrowserSteadyStateReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateResult result = new()
        {
            ProfileId = steadystatereadinessSession.ProfileId,
            SessionId = steadystatereadinessSession.SessionId,
            SessionPath = steadystatereadinessSession.SessionPath,
            BrowserSteadyStateReadinessSessionVersion = steadystatereadinessSession.BrowserSteadyStateReadinessSessionVersion,
            BrowserLiveStabilityReadyStateVersion = steadystatereadinessSession.BrowserLiveStabilityReadyStateVersion,
            BrowserLiveStabilitySessionVersion = steadystatereadinessSession.BrowserLiveStabilitySessionVersion,
            LaunchMode = steadystatereadinessSession.LaunchMode,
            AssetRootPath = steadystatereadinessSession.AssetRootPath,
            ProfilesRootPath = steadystatereadinessSession.ProfilesRootPath,
            CacheRootPath = steadystatereadinessSession.CacheRootPath,
            ConfigRootPath = steadystatereadinessSession.ConfigRootPath,
            SettingsFilePath = steadystatereadinessSession.SettingsFilePath,
            StartupProfilePath = steadystatereadinessSession.StartupProfilePath,
            RequiredAssets = steadystatereadinessSession.RequiredAssets,
            ReadyAssetCount = steadystatereadinessSession.ReadyAssetCount,
            CompletedSteps = steadystatereadinessSession.CompletedSteps,
            TotalSteps = steadystatereadinessSession.TotalSteps,
            Exists = steadystatereadinessSession.Exists,
            ReadSucceeded = steadystatereadinessSession.ReadSucceeded
        };

        if (!steadystatereadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadystatereadiness ready state blocked for profile '{steadystatereadinessSession.ProfileId}'.";
            result.Error = steadystatereadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyStateReadinessReadyStateVersion = "runtime-browser-steadystatereadiness-ready-state-v1";
        result.BrowserSteadyStateReadinessReadyChecks =
        [
            "browser-livestability-ready-state-ready",
            "browser-steadystatereadiness-session-ready",
            "browser-steadystatereadiness-ready"
        ];
        result.BrowserSteadyStateReadinessReadySummary = $"Runtime browser steadystatereadiness ready state passed {result.BrowserSteadyStateReadinessReadyChecks.Length} steadystatereadiness readiness check(s) for profile '{steadystatereadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadystatereadiness ready state ready for profile '{steadystatereadinessSession.ProfileId}' with {result.BrowserSteadyStateReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyStateReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyStateReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLiveStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyStateReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSteadyStateReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
