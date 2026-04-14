namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeStabilityReadyStateService : IBrowserClientRuntimeBrowserRuntimeStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserRuntimeStabilitySession _runtimeBrowserRuntimeStabilitySession;

    public BrowserClientRuntimeBrowserRuntimeStabilityReadyStateService(IBrowserClientRuntimeBrowserRuntimeStabilitySession runtimeBrowserRuntimeStabilitySession)
    {
        _runtimeBrowserRuntimeStabilitySession = runtimeBrowserRuntimeStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeStabilitySessionResult runtimestabilitySession = await _runtimeBrowserRuntimeStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeStabilityReadyStateResult result = new()
        {
            ProfileId = runtimestabilitySession.ProfileId,
            SessionId = runtimestabilitySession.SessionId,
            SessionPath = runtimestabilitySession.SessionPath,
            BrowserRuntimeStabilitySessionVersion = runtimestabilitySession.BrowserRuntimeStabilitySessionVersion,
            BrowserSteadyOperationReadyStateVersion = runtimestabilitySession.BrowserSteadyOperationReadyStateVersion,
            BrowserSteadyOperationSessionVersion = runtimestabilitySession.BrowserSteadyOperationSessionVersion,
            LaunchMode = runtimestabilitySession.LaunchMode,
            AssetRootPath = runtimestabilitySession.AssetRootPath,
            ProfilesRootPath = runtimestabilitySession.ProfilesRootPath,
            CacheRootPath = runtimestabilitySession.CacheRootPath,
            ConfigRootPath = runtimestabilitySession.ConfigRootPath,
            SettingsFilePath = runtimestabilitySession.SettingsFilePath,
            StartupProfilePath = runtimestabilitySession.StartupProfilePath,
            RequiredAssets = runtimestabilitySession.RequiredAssets,
            ReadyAssetCount = runtimestabilitySession.ReadyAssetCount,
            CompletedSteps = runtimestabilitySession.CompletedSteps,
            TotalSteps = runtimestabilitySession.TotalSteps,
            Exists = runtimestabilitySession.Exists,
            ReadSucceeded = runtimestabilitySession.ReadSucceeded
        };

        if (!runtimestabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimestability ready state blocked for profile '{runtimestabilitySession.ProfileId}'.";
            result.Error = runtimestabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeStabilityReadyStateVersion = "runtime-browser-runtimestability-ready-state-v1";
        result.BrowserRuntimeStabilityReadyChecks =
        [
            "browser-steadyoperation-ready-state-ready",
            "browser-runtimestability-session-ready",
            "browser-runtimestability-ready"
        ];
        result.BrowserRuntimeStabilityReadySummary = $"Runtime browser runtimestability ready state passed {result.BrowserRuntimeStabilityReadyChecks.Length} runtimestability readiness check(s) for profile '{runtimestabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimestability ready state ready for profile '{runtimestabilitySession.ProfileId}' with {result.BrowserRuntimeStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeStabilityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
