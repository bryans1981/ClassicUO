namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReleaseStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseStabilityReadyStateService : IBrowserClientRuntimeBrowserReleaseStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserReleaseStabilitySession _runtimeBrowserReleaseStabilitySession;

    public BrowserClientRuntimeBrowserReleaseStabilityReadyStateService(IBrowserClientRuntimeBrowserReleaseStabilitySession runtimeBrowserReleaseStabilitySession)
    {
        _runtimeBrowserReleaseStabilitySession = runtimeBrowserReleaseStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseStabilitySessionResult releasestabilitySession = await _runtimeBrowserReleaseStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserReleaseStabilityReadyStateResult result = new()
        {
            ProfileId = releasestabilitySession.ProfileId,
            SessionId = releasestabilitySession.SessionId,
            SessionPath = releasestabilitySession.SessionPath,
            BrowserReleaseStabilitySessionVersion = releasestabilitySession.BrowserReleaseStabilitySessionVersion,
            BrowserDeploymentStabilityReadyStateVersion = releasestabilitySession.BrowserDeploymentStabilityReadyStateVersion,
            BrowserDeploymentStabilitySessionVersion = releasestabilitySession.BrowserDeploymentStabilitySessionVersion,
            LaunchMode = releasestabilitySession.LaunchMode,
            AssetRootPath = releasestabilitySession.AssetRootPath,
            ProfilesRootPath = releasestabilitySession.ProfilesRootPath,
            CacheRootPath = releasestabilitySession.CacheRootPath,
            ConfigRootPath = releasestabilitySession.ConfigRootPath,
            SettingsFilePath = releasestabilitySession.SettingsFilePath,
            StartupProfilePath = releasestabilitySession.StartupProfilePath,
            RequiredAssets = releasestabilitySession.RequiredAssets,
            ReadyAssetCount = releasestabilitySession.ReadyAssetCount,
            CompletedSteps = releasestabilitySession.CompletedSteps,
            TotalSteps = releasestabilitySession.TotalSteps,
            Exists = releasestabilitySession.Exists,
            ReadSucceeded = releasestabilitySession.ReadSucceeded
        };

        if (!releasestabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser releasestability ready state blocked for profile '{releasestabilitySession.ProfileId}'.";
            result.Error = releasestabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseStabilityReadyStateVersion = "runtime-browser-releasestability-ready-state-v1";
        result.BrowserReleaseStabilityReadyChecks =
        [
            "browser-deploymentstability-ready-state-ready",
            "browser-releasestability-session-ready",
            "browser-releasestability-ready"
        ];
        result.BrowserReleaseStabilityReadySummary = $"Runtime browser releasestability ready state passed {result.BrowserReleaseStabilityReadyChecks.Length} releasestability readiness check(s) for profile '{releasestabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releasestability ready state ready for profile '{releasestabilitySession.ProfileId}' with {result.BrowserReleaseStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDeploymentStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReleaseStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
