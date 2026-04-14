namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLiveStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveStabilityReadyStateService : IBrowserClientRuntimeBrowserLiveStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLiveStabilitySession _runtimeBrowserLiveStabilitySession;

    public BrowserClientRuntimeBrowserLiveStabilityReadyStateService(IBrowserClientRuntimeBrowserLiveStabilitySession runtimeBrowserLiveStabilitySession)
    {
        _runtimeBrowserLiveStabilitySession = runtimeBrowserLiveStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveStabilitySessionResult livestabilitySession = await _runtimeBrowserLiveStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLiveStabilityReadyStateResult result = new()
        {
            ProfileId = livestabilitySession.ProfileId,
            SessionId = livestabilitySession.SessionId,
            SessionPath = livestabilitySession.SessionPath,
            BrowserLiveStabilitySessionVersion = livestabilitySession.BrowserLiveStabilitySessionVersion,
            BrowserProductionAssuranceReadyStateVersion = livestabilitySession.BrowserProductionAssuranceReadyStateVersion,
            BrowserProductionAssuranceSessionVersion = livestabilitySession.BrowserProductionAssuranceSessionVersion,
            LaunchMode = livestabilitySession.LaunchMode,
            AssetRootPath = livestabilitySession.AssetRootPath,
            ProfilesRootPath = livestabilitySession.ProfilesRootPath,
            CacheRootPath = livestabilitySession.CacheRootPath,
            ConfigRootPath = livestabilitySession.ConfigRootPath,
            SettingsFilePath = livestabilitySession.SettingsFilePath,
            StartupProfilePath = livestabilitySession.StartupProfilePath,
            RequiredAssets = livestabilitySession.RequiredAssets,
            ReadyAssetCount = livestabilitySession.ReadyAssetCount,
            CompletedSteps = livestabilitySession.CompletedSteps,
            TotalSteps = livestabilitySession.TotalSteps,
            Exists = livestabilitySession.Exists,
            ReadSucceeded = livestabilitySession.ReadSucceeded
        };

        if (!livestabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser livestability ready state blocked for profile '{livestabilitySession.ProfileId}'.";
            result.Error = livestabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveStabilityReadyStateVersion = "runtime-browser-livestability-ready-state-v1";
        result.BrowserLiveStabilityReadyChecks =
        [
            "browser-productionassurance-ready-state-ready",
            "browser-livestability-session-ready",
            "browser-livestability-ready"
        ];
        result.BrowserLiveStabilityReadySummary = $"Runtime browser livestability ready state passed {result.BrowserLiveStabilityReadyChecks.Length} livestability readiness check(s) for profile '{livestabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livestability ready state ready for profile '{livestabilitySession.ProfileId}' with {result.BrowserLiveStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserProductionAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLiveStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
