namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLaunchStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchStabilityReadyStateService : IBrowserClientRuntimeBrowserLaunchStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLaunchStabilitySession _runtimeBrowserLaunchStabilitySession;

    public BrowserClientRuntimeBrowserLaunchStabilityReadyStateService(IBrowserClientRuntimeBrowserLaunchStabilitySession runtimeBrowserLaunchStabilitySession)
    {
        _runtimeBrowserLaunchStabilitySession = runtimeBrowserLaunchStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchStabilitySessionResult launchstabilitySession = await _runtimeBrowserLaunchStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLaunchStabilityReadyStateResult result = new()
        {
            ProfileId = launchstabilitySession.ProfileId,
            SessionId = launchstabilitySession.SessionId,
            SessionPath = launchstabilitySession.SessionPath,
            BrowserLaunchStabilitySessionVersion = launchstabilitySession.BrowserLaunchStabilitySessionVersion,
            BrowserReleaseStabilityReadyStateVersion = launchstabilitySession.BrowserReleaseStabilityReadyStateVersion,
            BrowserReleaseStabilitySessionVersion = launchstabilitySession.BrowserReleaseStabilitySessionVersion,
            LaunchMode = launchstabilitySession.LaunchMode,
            AssetRootPath = launchstabilitySession.AssetRootPath,
            ProfilesRootPath = launchstabilitySession.ProfilesRootPath,
            CacheRootPath = launchstabilitySession.CacheRootPath,
            ConfigRootPath = launchstabilitySession.ConfigRootPath,
            SettingsFilePath = launchstabilitySession.SettingsFilePath,
            StartupProfilePath = launchstabilitySession.StartupProfilePath,
            RequiredAssets = launchstabilitySession.RequiredAssets,
            ReadyAssetCount = launchstabilitySession.ReadyAssetCount,
            CompletedSteps = launchstabilitySession.CompletedSteps,
            TotalSteps = launchstabilitySession.TotalSteps,
            Exists = launchstabilitySession.Exists,
            ReadSucceeded = launchstabilitySession.ReadSucceeded
        };

        if (!launchstabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser launchstability ready state blocked for profile '{launchstabilitySession.ProfileId}'.";
            result.Error = launchstabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchStabilityReadyStateVersion = "runtime-browser-launchstability-ready-state-v1";
        result.BrowserLaunchStabilityReadyChecks =
        [
            "browser-releasestability-ready-state-ready",
            "browser-launchstability-session-ready",
            "browser-launchstability-ready"
        ];
        result.BrowserLaunchStabilityReadySummary = $"Runtime browser launchstability ready state passed {result.BrowserLaunchStabilityReadyChecks.Length} launchstability readiness check(s) for profile '{launchstabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchstability ready state ready for profile '{launchstabilitySession.ProfileId}' with {result.BrowserLaunchStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLaunchStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserReleaseStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLaunchStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLaunchStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
