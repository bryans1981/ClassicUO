namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeploymentStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDeploymentStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeploymentStabilityReadyStateService : IBrowserClientRuntimeBrowserDeploymentStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserDeploymentStabilitySession _runtimeBrowserDeploymentStabilitySession;

    public BrowserClientRuntimeBrowserDeploymentStabilityReadyStateService(IBrowserClientRuntimeBrowserDeploymentStabilitySession runtimeBrowserDeploymentStabilitySession)
    {
        _runtimeBrowserDeploymentStabilitySession = runtimeBrowserDeploymentStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeploymentStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentStabilitySessionResult deploymentstabilitySession = await _runtimeBrowserDeploymentStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDeploymentStabilityReadyStateResult result = new()
        {
            ProfileId = deploymentstabilitySession.ProfileId,
            SessionId = deploymentstabilitySession.SessionId,
            SessionPath = deploymentstabilitySession.SessionPath,
            BrowserDeploymentStabilitySessionVersion = deploymentstabilitySession.BrowserDeploymentStabilitySessionVersion,
            BrowserOperationalStabilityReadyStateVersion = deploymentstabilitySession.BrowserOperationalStabilityReadyStateVersion,
            BrowserOperationalStabilitySessionVersion = deploymentstabilitySession.BrowserOperationalStabilitySessionVersion,
            LaunchMode = deploymentstabilitySession.LaunchMode,
            AssetRootPath = deploymentstabilitySession.AssetRootPath,
            ProfilesRootPath = deploymentstabilitySession.ProfilesRootPath,
            CacheRootPath = deploymentstabilitySession.CacheRootPath,
            ConfigRootPath = deploymentstabilitySession.ConfigRootPath,
            SettingsFilePath = deploymentstabilitySession.SettingsFilePath,
            StartupProfilePath = deploymentstabilitySession.StartupProfilePath,
            RequiredAssets = deploymentstabilitySession.RequiredAssets,
            ReadyAssetCount = deploymentstabilitySession.ReadyAssetCount,
            CompletedSteps = deploymentstabilitySession.CompletedSteps,
            TotalSteps = deploymentstabilitySession.TotalSteps,
            Exists = deploymentstabilitySession.Exists,
            ReadSucceeded = deploymentstabilitySession.ReadSucceeded
        };

        if (!deploymentstabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser deploymentstability ready state blocked for profile '{deploymentstabilitySession.ProfileId}'.";
            result.Error = deploymentstabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeploymentStabilityReadyStateVersion = "runtime-browser-deploymentstability-ready-state-v1";
        result.BrowserDeploymentStabilityReadyChecks =
        [
            "browser-operationalstability-ready-state-ready",
            "browser-deploymentstability-session-ready",
            "browser-deploymentstability-ready"
        ];
        result.BrowserDeploymentStabilityReadySummary = $"Runtime browser deploymentstability ready state passed {result.BrowserDeploymentStabilityReadyChecks.Length} deploymentstability readiness check(s) for profile '{deploymentstabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deploymentstability ready state ready for profile '{deploymentstabilitySession.ProfileId}' with {result.BrowserDeploymentStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeploymentStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDeploymentStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserOperationalStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeploymentStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDeploymentStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
