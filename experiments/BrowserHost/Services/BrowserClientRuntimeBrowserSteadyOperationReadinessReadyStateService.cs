namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyOperationReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateService : IBrowserClientRuntimeBrowserSteadyOperationReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserSteadyOperationReadinessSession _runtimeBrowserSteadyOperationReadinessSession;

    public BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateService(IBrowserClientRuntimeBrowserSteadyOperationReadinessSession runtimeBrowserSteadyOperationReadinessSession)
    {
        _runtimeBrowserSteadyOperationReadinessSession = runtimeBrowserSteadyOperationReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyOperationReadinessSessionResult steadyoperationreadinessSession = await _runtimeBrowserSteadyOperationReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateResult result = new()
        {
            ProfileId = steadyoperationreadinessSession.ProfileId,
            SessionId = steadyoperationreadinessSession.SessionId,
            SessionPath = steadyoperationreadinessSession.SessionPath,
            BrowserSteadyOperationReadinessSessionVersion = steadyoperationreadinessSession.BrowserSteadyOperationReadinessSessionVersion,
            BrowserLiveAssuranceReadyStateVersion = steadyoperationreadinessSession.BrowserLiveAssuranceReadyStateVersion,
            BrowserLiveAssuranceSessionVersion = steadyoperationreadinessSession.BrowserLiveAssuranceSessionVersion,
            LaunchMode = steadyoperationreadinessSession.LaunchMode,
            AssetRootPath = steadyoperationreadinessSession.AssetRootPath,
            ProfilesRootPath = steadyoperationreadinessSession.ProfilesRootPath,
            CacheRootPath = steadyoperationreadinessSession.CacheRootPath,
            ConfigRootPath = steadyoperationreadinessSession.ConfigRootPath,
            SettingsFilePath = steadyoperationreadinessSession.SettingsFilePath,
            StartupProfilePath = steadyoperationreadinessSession.StartupProfilePath,
            RequiredAssets = steadyoperationreadinessSession.RequiredAssets,
            ReadyAssetCount = steadyoperationreadinessSession.ReadyAssetCount,
            CompletedSteps = steadyoperationreadinessSession.CompletedSteps,
            TotalSteps = steadyoperationreadinessSession.TotalSteps,
            Exists = steadyoperationreadinessSession.Exists,
            ReadSucceeded = steadyoperationreadinessSession.ReadSucceeded
        };

        if (!steadyoperationreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadyoperationreadiness ready state blocked for profile '{steadyoperationreadinessSession.ProfileId}'.";
            result.Error = steadyoperationreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyOperationReadinessReadyStateVersion = "runtime-browser-steadyoperationreadiness-ready-state-v1";
        result.BrowserSteadyOperationReadinessReadyChecks =
        [
            "browser-liveassurance-ready-state-ready",
            "browser-steadyoperationreadiness-session-ready",
            "browser-steadyoperationreadiness-ready"
        ];
        result.BrowserSteadyOperationReadinessReadySummary = $"Runtime browser steadyoperationreadiness ready state passed {result.BrowserSteadyOperationReadinessReadyChecks.Length} steadyoperationreadiness readiness check(s) for profile '{steadyoperationreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadyoperationreadiness ready state ready for profile '{steadyoperationreadinessSession.ProfileId}' with {result.BrowserSteadyOperationReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyOperationReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyOperationReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLiveAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyOperationReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSteadyOperationReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
