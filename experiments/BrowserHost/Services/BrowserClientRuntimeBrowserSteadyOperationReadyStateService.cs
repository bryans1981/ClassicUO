namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyOperationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSteadyOperationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationReadyStateService : IBrowserClientRuntimeBrowserSteadyOperationReadyState
{
    private readonly IBrowserClientRuntimeBrowserSteadyOperationSession _runtimeBrowserSteadyOperationSession;

    public BrowserClientRuntimeBrowserSteadyOperationReadyStateService(IBrowserClientRuntimeBrowserSteadyOperationSession runtimeBrowserSteadyOperationSession)
    {
        _runtimeBrowserSteadyOperationSession = runtimeBrowserSteadyOperationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyOperationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyOperationSessionResult steadyoperationSession = await _runtimeBrowserSteadyOperationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSteadyOperationReadyStateResult result = new()
        {
            ProfileId = steadyoperationSession.ProfileId,
            SessionId = steadyoperationSession.SessionId,
            SessionPath = steadyoperationSession.SessionPath,
            BrowserSteadyOperationSessionVersion = steadyoperationSession.BrowserSteadyOperationSessionVersion,
            BrowserServiceActivationReadyStateVersion = steadyoperationSession.BrowserServiceActivationReadyStateVersion,
            BrowserServiceActivationSessionVersion = steadyoperationSession.BrowserServiceActivationSessionVersion,
            LaunchMode = steadyoperationSession.LaunchMode,
            AssetRootPath = steadyoperationSession.AssetRootPath,
            ProfilesRootPath = steadyoperationSession.ProfilesRootPath,
            CacheRootPath = steadyoperationSession.CacheRootPath,
            ConfigRootPath = steadyoperationSession.ConfigRootPath,
            SettingsFilePath = steadyoperationSession.SettingsFilePath,
            StartupProfilePath = steadyoperationSession.StartupProfilePath,
            RequiredAssets = steadyoperationSession.RequiredAssets,
            ReadyAssetCount = steadyoperationSession.ReadyAssetCount,
            CompletedSteps = steadyoperationSession.CompletedSteps,
            TotalSteps = steadyoperationSession.TotalSteps,
            Exists = steadyoperationSession.Exists,
            ReadSucceeded = steadyoperationSession.ReadSucceeded
        };

        if (!steadyoperationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadyoperation ready state blocked for profile '{steadyoperationSession.ProfileId}'.";
            result.Error = steadyoperationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyOperationReadyStateVersion = "runtime-browser-steadyoperation-ready-state-v1";
        result.BrowserSteadyOperationReadyChecks =
        [
            "browser-serviceactivation-ready-state-ready",
            "browser-steadyoperation-session-ready",
            "browser-steadyoperation-ready"
        ];
        result.BrowserSteadyOperationReadySummary = $"Runtime browser steadyoperation ready state passed {result.BrowserSteadyOperationReadyChecks.Length} steadyoperation readiness check(s) for profile '{steadyoperationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadyoperation ready state ready for profile '{steadyoperationSession.ProfileId}' with {result.BrowserSteadyOperationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyOperationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyOperationSessionVersion { get; set; } = string.Empty;
    public string BrowserServiceActivationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceActivationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyOperationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSteadyOperationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
