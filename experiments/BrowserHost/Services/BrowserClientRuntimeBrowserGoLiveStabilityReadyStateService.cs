namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveStabilityReadyStateService : IBrowserClientRuntimeBrowserGoLiveStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserGoLiveStabilitySession _runtimeBrowserGoLiveStabilitySession;

    public BrowserClientRuntimeBrowserGoLiveStabilityReadyStateService(IBrowserClientRuntimeBrowserGoLiveStabilitySession runtimeBrowserGoLiveStabilitySession)
    {
        _runtimeBrowserGoLiveStabilitySession = runtimeBrowserGoLiveStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoLiveStabilitySessionResult golivestabilitySession = await _runtimeBrowserGoLiveStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveStabilityReadyStateResult result = new()
        {
            ProfileId = golivestabilitySession.ProfileId,
            SessionId = golivestabilitySession.SessionId,
            SessionPath = golivestabilitySession.SessionPath,
            BrowserGoLiveStabilitySessionVersion = golivestabilitySession.BrowserGoLiveStabilitySessionVersion,
            BrowserLaunchStabilityReadyStateVersion = golivestabilitySession.BrowserLaunchStabilityReadyStateVersion,
            BrowserLaunchStabilitySessionVersion = golivestabilitySession.BrowserLaunchStabilitySessionVersion,
            LaunchMode = golivestabilitySession.LaunchMode,
            AssetRootPath = golivestabilitySession.AssetRootPath,
            ProfilesRootPath = golivestabilitySession.ProfilesRootPath,
            CacheRootPath = golivestabilitySession.CacheRootPath,
            ConfigRootPath = golivestabilitySession.ConfigRootPath,
            SettingsFilePath = golivestabilitySession.SettingsFilePath,
            StartupProfilePath = golivestabilitySession.StartupProfilePath,
            RequiredAssets = golivestabilitySession.RequiredAssets,
            ReadyAssetCount = golivestabilitySession.ReadyAssetCount,
            CompletedSteps = golivestabilitySession.CompletedSteps,
            TotalSteps = golivestabilitySession.TotalSteps,
            Exists = golivestabilitySession.Exists,
            ReadSucceeded = golivestabilitySession.ReadSucceeded
        };

        if (!golivestabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser golivestability ready state blocked for profile '{golivestabilitySession.ProfileId}'.";
            result.Error = golivestabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveStabilityReadyStateVersion = "runtime-browser-golivestability-ready-state-v1";
        result.BrowserGoLiveStabilityReadyChecks =
        [
            "browser-launchstability-ready-state-ready",
            "browser-golivestability-session-ready",
            "browser-golivestability-ready"
        ];
        result.BrowserGoLiveStabilityReadySummary = $"Runtime browser golivestability ready state passed {result.BrowserGoLiveStabilityReadyChecks.Length} golivestability readiness check(s) for profile '{golivestabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser golivestability ready state ready for profile '{golivestabilitySession.ProfileId}' with {result.BrowserGoLiveStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGoLiveStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLaunchStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoLiveStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
