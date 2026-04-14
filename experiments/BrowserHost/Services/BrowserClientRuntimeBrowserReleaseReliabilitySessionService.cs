namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserReleaseReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseReliabilitySessionService : IBrowserClientRuntimeBrowserReleaseReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserDeploymentReliabilityReadyState _runtimeBrowserDeploymentReliabilityReadyState;

    public BrowserClientRuntimeBrowserReleaseReliabilitySessionService(IBrowserClientRuntimeBrowserDeploymentReliabilityReadyState runtimeBrowserDeploymentReliabilityReadyState)
    {
        _runtimeBrowserDeploymentReliabilityReadyState = runtimeBrowserDeploymentReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentReliabilityReadyStateResult prevReadyState = await _runtimeBrowserDeploymentReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReleaseReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserDeploymentReliabilityReadyStateVersion = prevReadyState.BrowserDeploymentReliabilityReadyStateVersion,
            BrowserDeploymentReliabilitySessionVersion = prevReadyState.BrowserDeploymentReliabilitySessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser releasereliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseReliabilitySessionVersion = "runtime-browser-releasereliability-session-v1";
        result.BrowserReleaseReliabilityStages =
        [
            "open-browser-releasereliability-session",
            "bind-browser-deploymentreliability-ready-state",
            "publish-browser-releasereliability-ready"
        ];
        result.BrowserReleaseReliabilitySummary = $"Runtime browser releasereliability session prepared {result.BrowserReleaseReliabilityStages.Length} releasereliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releasereliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserReleaseReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDeploymentReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserReleaseReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
