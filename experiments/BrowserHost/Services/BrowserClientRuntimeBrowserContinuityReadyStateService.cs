namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserContinuityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuityReadyStateService : IBrowserClientRuntimeBrowserContinuityReadyState
{
    private readonly IBrowserClientRuntimeBrowserContinuitySession _runtimeBrowserContinuitySession;

    public BrowserClientRuntimeBrowserContinuityReadyStateService(IBrowserClientRuntimeBrowserContinuitySession runtimeBrowserContinuitySession)
    {
        _runtimeBrowserContinuitySession = runtimeBrowserContinuitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuitySessionResult continuitySession = await _runtimeBrowserContinuitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserContinuityReadyStateResult result = new()
        {
            ProfileId = continuitySession.ProfileId,
            SessionId = continuitySession.SessionId,
            SessionPath = continuitySession.SessionPath,
            BrowserContinuitySessionVersion = continuitySession.BrowserContinuitySessionVersion,
            BrowserAvailabilityReadyStateVersion = continuitySession.BrowserAvailabilityReadyStateVersion,
            BrowserAvailabilitySessionVersion = continuitySession.BrowserAvailabilitySessionVersion,
            LaunchMode = continuitySession.LaunchMode,
            AssetRootPath = continuitySession.AssetRootPath,
            ProfilesRootPath = continuitySession.ProfilesRootPath,
            CacheRootPath = continuitySession.CacheRootPath,
            ConfigRootPath = continuitySession.ConfigRootPath,
            SettingsFilePath = continuitySession.SettingsFilePath,
            StartupProfilePath = continuitySession.StartupProfilePath,
            RequiredAssets = continuitySession.RequiredAssets,
            ReadyAssetCount = continuitySession.ReadyAssetCount,
            CompletedSteps = continuitySession.CompletedSteps,
            TotalSteps = continuitySession.TotalSteps,
            Exists = continuitySession.Exists,
            ReadSucceeded = continuitySession.ReadSucceeded
        };

        if (!continuitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser continuity ready state blocked for profile '{continuitySession.ProfileId}'.";
            result.Error = continuitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuityReadyStateVersion = "runtime-browser-continuity-ready-state-v1";
        result.BrowserContinuityReadyChecks =
        [
            "browser-availability-ready-state-ready",
            "browser-continuity-session-ready",
            "browser-continuity-ready"
        ];
        result.BrowserContinuityReadySummary = $"Runtime browser continuity ready state passed {result.BrowserContinuityReadyChecks.Length} continuity readiness check(s) for profile '{continuitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuity ready state ready for profile '{continuitySession.ProfileId}' with {result.BrowserContinuityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserAvailabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAvailabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserContinuityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
