namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeStabilitySessionService : IBrowserClientRuntimeBrowserRuntimeStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSteadyOperationReadyState _runtimeBrowserSteadyOperationReadyState;

    public BrowserClientRuntimeBrowserRuntimeStabilitySessionService(IBrowserClientRuntimeBrowserSteadyOperationReadyState runtimeBrowserSteadyOperationReadyState)
    {
        _runtimeBrowserSteadyOperationReadyState = runtimeBrowserSteadyOperationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyOperationReadyStateResult steadyoperationReadyState = await _runtimeBrowserSteadyOperationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeStabilitySessionResult result = new()
        {
            ProfileId = steadyoperationReadyState.ProfileId,
            SessionId = steadyoperationReadyState.SessionId,
            SessionPath = steadyoperationReadyState.SessionPath,
            BrowserSteadyOperationReadyStateVersion = steadyoperationReadyState.BrowserSteadyOperationReadyStateVersion,
            BrowserSteadyOperationSessionVersion = steadyoperationReadyState.BrowserSteadyOperationSessionVersion,
            LaunchMode = steadyoperationReadyState.LaunchMode,
            AssetRootPath = steadyoperationReadyState.AssetRootPath,
            ProfilesRootPath = steadyoperationReadyState.ProfilesRootPath,
            CacheRootPath = steadyoperationReadyState.CacheRootPath,
            ConfigRootPath = steadyoperationReadyState.ConfigRootPath,
            SettingsFilePath = steadyoperationReadyState.SettingsFilePath,
            StartupProfilePath = steadyoperationReadyState.StartupProfilePath,
            RequiredAssets = steadyoperationReadyState.RequiredAssets,
            ReadyAssetCount = steadyoperationReadyState.ReadyAssetCount,
            CompletedSteps = steadyoperationReadyState.CompletedSteps,
            TotalSteps = steadyoperationReadyState.TotalSteps,
            Exists = steadyoperationReadyState.Exists,
            ReadSucceeded = steadyoperationReadyState.ReadSucceeded
        };

        if (!steadyoperationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimestability session blocked for profile '{steadyoperationReadyState.ProfileId}'.";
            result.Error = steadyoperationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeStabilitySessionVersion = "runtime-browser-runtimestability-session-v1";
        result.BrowserRuntimeStabilityStages =
        [
            "open-browser-runtimestability-session",
            "bind-browser-steadyoperation-ready-state",
            "publish-browser-runtimestability-ready"
        ];
        result.BrowserRuntimeStabilitySummary = $"Runtime browser runtimestability session prepared {result.BrowserRuntimeStabilityStages.Length} runtimestability stage(s) for profile '{steadyoperationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimestability session ready for profile '{steadyoperationReadyState.ProfileId}' with {result.BrowserRuntimeStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSteadyOperationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyOperationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
