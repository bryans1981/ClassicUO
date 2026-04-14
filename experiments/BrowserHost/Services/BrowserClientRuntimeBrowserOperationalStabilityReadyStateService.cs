namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOperationalStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalStabilityReadyStateService : IBrowserClientRuntimeBrowserOperationalStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserOperationalStabilitySession _runtimeBrowserOperationalStabilitySession;

    public BrowserClientRuntimeBrowserOperationalStabilityReadyStateService(IBrowserClientRuntimeBrowserOperationalStabilitySession runtimeBrowserOperationalStabilitySession)
    {
        _runtimeBrowserOperationalStabilitySession = runtimeBrowserOperationalStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperationalStabilitySessionResult operationalstabilitySession = await _runtimeBrowserOperationalStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOperationalStabilityReadyStateResult result = new()
        {
            ProfileId = operationalstabilitySession.ProfileId,
            SessionId = operationalstabilitySession.SessionId,
            SessionPath = operationalstabilitySession.SessionPath,
            BrowserOperationalStabilitySessionVersion = operationalstabilitySession.BrowserOperationalStabilitySessionVersion,
            BrowserSteadyStateReadinessReadyStateVersion = operationalstabilitySession.BrowserSteadyStateReadinessReadyStateVersion,
            BrowserSteadyStateReadinessSessionVersion = operationalstabilitySession.BrowserSteadyStateReadinessSessionVersion,
            LaunchMode = operationalstabilitySession.LaunchMode,
            AssetRootPath = operationalstabilitySession.AssetRootPath,
            ProfilesRootPath = operationalstabilitySession.ProfilesRootPath,
            CacheRootPath = operationalstabilitySession.CacheRootPath,
            ConfigRootPath = operationalstabilitySession.ConfigRootPath,
            SettingsFilePath = operationalstabilitySession.SettingsFilePath,
            StartupProfilePath = operationalstabilitySession.StartupProfilePath,
            RequiredAssets = operationalstabilitySession.RequiredAssets,
            ReadyAssetCount = operationalstabilitySession.ReadyAssetCount,
            CompletedSteps = operationalstabilitySession.CompletedSteps,
            TotalSteps = operationalstabilitySession.TotalSteps,
            Exists = operationalstabilitySession.Exists,
            ReadSucceeded = operationalstabilitySession.ReadSucceeded
        };

        if (!operationalstabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operationalstability ready state blocked for profile '{operationalstabilitySession.ProfileId}'.";
            result.Error = operationalstabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalStabilityReadyStateVersion = "runtime-browser-operationalstability-ready-state-v1";
        result.BrowserOperationalStabilityReadyChecks =
        [
            "browser-steadystatereadiness-ready-state-ready",
            "browser-operationalstability-session-ready",
            "browser-operationalstability-ready"
        ];
        result.BrowserOperationalStabilityReadySummary = $"Runtime browser operationalstability ready state passed {result.BrowserOperationalStabilityReadyChecks.Length} operationalstability readiness check(s) for profile '{operationalstabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalstability ready state ready for profile '{operationalstabilitySession.ProfileId}' with {result.BrowserOperationalStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOperationalStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSteadyStateReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyStateReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperationalStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOperationalStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
