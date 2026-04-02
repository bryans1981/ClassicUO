namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPredictabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPredictabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPredictabilityReadyStateService : IBrowserClientRuntimeBrowserPredictabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserPredictabilitySession _runtimeBrowserPredictabilitySession;

    public BrowserClientRuntimeBrowserPredictabilityReadyStateService(IBrowserClientRuntimeBrowserPredictabilitySession runtimeBrowserPredictabilitySession)
    {
        _runtimeBrowserPredictabilitySession = runtimeBrowserPredictabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPredictabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPredictabilitySessionResult predictabilitySession = await _runtimeBrowserPredictabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPredictabilityReadyStateResult result = new()
        {
            ProfileId = predictabilitySession.ProfileId,
            SessionId = predictabilitySession.SessionId,
            SessionPath = predictabilitySession.SessionPath,
            BrowserPredictabilitySessionVersion = predictabilitySession.BrowserPredictabilitySessionVersion,
            BrowserCohesivenessReadyStateVersion = predictabilitySession.BrowserCohesivenessReadyStateVersion,
            BrowserCohesivenessSessionVersion = predictabilitySession.BrowserCohesivenessSessionVersion,
            LaunchMode = predictabilitySession.LaunchMode,
            AssetRootPath = predictabilitySession.AssetRootPath,
            ProfilesRootPath = predictabilitySession.ProfilesRootPath,
            CacheRootPath = predictabilitySession.CacheRootPath,
            ConfigRootPath = predictabilitySession.ConfigRootPath,
            SettingsFilePath = predictabilitySession.SettingsFilePath,
            StartupProfilePath = predictabilitySession.StartupProfilePath,
            RequiredAssets = predictabilitySession.RequiredAssets,
            ReadyAssetCount = predictabilitySession.ReadyAssetCount,
            CompletedSteps = predictabilitySession.CompletedSteps,
            TotalSteps = predictabilitySession.TotalSteps,
            Exists = predictabilitySession.Exists,
            ReadSucceeded = predictabilitySession.ReadSucceeded
        };

        if (!predictabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser predictability ready state blocked for profile '{predictabilitySession.ProfileId}'.";
            result.Error = predictabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPredictabilityReadyStateVersion = "runtime-browser-predictability-ready-state-v1";
        result.BrowserPredictabilityReadyChecks =
        [
            "browser-cohesiveness-ready-state-ready",
            "browser-predictability-session-ready",
            "browser-predictability-ready"
        ];
        result.BrowserPredictabilityReadySummary = $"Runtime browser predictability ready state passed {result.BrowserPredictabilityReadyChecks.Length} predictability readiness check(s) for profile '{predictabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser predictability ready state ready for profile '{predictabilitySession.ProfileId}' with {result.BrowserPredictabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPredictabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPredictabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPredictabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCohesivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCohesivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPredictabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPredictabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

