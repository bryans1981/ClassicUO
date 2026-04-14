namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveContinuityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLiveContinuityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveContinuityReadyStateService : IBrowserClientRuntimeBrowserLiveContinuityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLiveContinuitySession _runtimeBrowserLiveContinuitySession;

    public BrowserClientRuntimeBrowserLiveContinuityReadyStateService(IBrowserClientRuntimeBrowserLiveContinuitySession runtimeBrowserLiveContinuitySession)
    {
        _runtimeBrowserLiveContinuitySession = runtimeBrowserLiveContinuitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveContinuityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveContinuitySessionResult livecontinuitySession = await _runtimeBrowserLiveContinuitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLiveContinuityReadyStateResult result = new()
        {
            ProfileId = livecontinuitySession.ProfileId,
            SessionId = livecontinuitySession.SessionId,
            SessionPath = livecontinuitySession.SessionPath,
            BrowserLiveContinuitySessionVersion = livecontinuitySession.BrowserLiveContinuitySessionVersion,
            BrowserProductionDurabilityReadyStateVersion = livecontinuitySession.BrowserProductionDurabilityReadyStateVersion,
            BrowserProductionDurabilitySessionVersion = livecontinuitySession.BrowserProductionDurabilitySessionVersion,
            LaunchMode = livecontinuitySession.LaunchMode,
            AssetRootPath = livecontinuitySession.AssetRootPath,
            ProfilesRootPath = livecontinuitySession.ProfilesRootPath,
            CacheRootPath = livecontinuitySession.CacheRootPath,
            ConfigRootPath = livecontinuitySession.ConfigRootPath,
            SettingsFilePath = livecontinuitySession.SettingsFilePath,
            StartupProfilePath = livecontinuitySession.StartupProfilePath,
            RequiredAssets = livecontinuitySession.RequiredAssets,
            ReadyAssetCount = livecontinuitySession.ReadyAssetCount,
            CompletedSteps = livecontinuitySession.CompletedSteps,
            TotalSteps = livecontinuitySession.TotalSteps,
            Exists = livecontinuitySession.Exists,
            ReadSucceeded = livecontinuitySession.ReadSucceeded
        };

        if (!livecontinuitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser livecontinuity ready state blocked for profile '{livecontinuitySession.ProfileId}'.";
            result.Error = livecontinuitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveContinuityReadyStateVersion = "runtime-browser-livecontinuity-ready-state-v1";
        result.BrowserLiveContinuityReadyChecks =
        [
            "browser-productiondurability-ready-state-ready",
            "browser-livecontinuity-session-ready",
            "browser-livecontinuity-ready"
        ];
        result.BrowserLiveContinuityReadySummary = $"Runtime browser livecontinuity ready state passed {result.BrowserLiveContinuityReadyChecks.Length} livecontinuity readiness check(s) for profile '{livecontinuitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livecontinuity ready state ready for profile '{livecontinuitySession.ProfileId}' with {result.BrowserLiveContinuityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveContinuityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserProductionDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveContinuityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLiveContinuityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
