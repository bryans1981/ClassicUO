namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeploymentReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserDeploymentReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeploymentReliabilitySessionService : IBrowserClientRuntimeBrowserDeploymentReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserOperationalReliabilityReadyState _runtimeBrowserOperationalReliabilityReadyState;

    public BrowserClientRuntimeBrowserDeploymentReliabilitySessionService(IBrowserClientRuntimeBrowserOperationalReliabilityReadyState runtimeBrowserOperationalReliabilityReadyState)
    {
        _runtimeBrowserOperationalReliabilityReadyState = runtimeBrowserOperationalReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeploymentReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperationalReliabilityReadyStateResult prevReadyState = await _runtimeBrowserOperationalReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDeploymentReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserOperationalReliabilityReadyStateVersion = prevReadyState.BrowserOperationalReliabilityReadyStateVersion,
            BrowserOperationalReliabilitySessionVersion = prevReadyState.BrowserOperationalReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser deploymentreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeploymentReliabilitySessionVersion = "runtime-browser-deploymentreliability-session-v1";
        result.BrowserDeploymentReliabilityStages =
        [
            "open-browser-deploymentreliability-session",
            "bind-browser-operationalreliability-ready-state",
            "publish-browser-deploymentreliability-ready"
        ];
        result.BrowserDeploymentReliabilitySummary = $"Runtime browser deploymentreliability session prepared {result.BrowserDeploymentReliabilityStages.Length} deploymentreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deploymentreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserDeploymentReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeploymentReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDeploymentReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserOperationalReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeploymentReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserDeploymentReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
