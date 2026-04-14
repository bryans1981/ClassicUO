namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeploymentResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserDeploymentResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeploymentResilienceSessionService : IBrowserClientRuntimeBrowserDeploymentResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserOperationalResilienceReadyState _runtimeBrowserOperationalResilienceReadyState;

    public BrowserClientRuntimeBrowserDeploymentResilienceSessionService(IBrowserClientRuntimeBrowserOperationalResilienceReadyState runtimeBrowserOperationalResilienceReadyState)
    {
        _runtimeBrowserOperationalResilienceReadyState = runtimeBrowserOperationalResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeploymentResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperationalResilienceReadyStateResult prevReadyState = await _runtimeBrowserOperationalResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDeploymentResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserOperationalResilienceReadyStateVersion = prevReadyState.BrowserOperationalResilienceReadyStateVersion,
            BrowserOperationalResilienceSessionVersion = prevReadyState.BrowserOperationalResilienceSessionVersion,
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
            result.Summary = $"Runtime browser deploymentresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeploymentResilienceSessionVersion = "runtime-browser-deploymentresilience-session-v1";
        result.BrowserDeploymentResilienceStages =
        [
            "open-browser-deploymentresilience-session",
            "bind-browser-operationalresilience-ready-state",
            "publish-browser-deploymentresilience-ready"
        ];
        result.BrowserDeploymentResilienceSummary = $"Runtime browser deploymentresilience session prepared {result.BrowserDeploymentResilienceStages.Length} deploymentresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deploymentresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserDeploymentResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeploymentResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDeploymentResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserOperationalResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeploymentResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserDeploymentResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
