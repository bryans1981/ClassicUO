namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReleaseResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserReleaseResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReleaseResilienceSessionService : IBrowserClientRuntimeBrowserReleaseResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserDeploymentResilienceReadyState _runtimeBrowserDeploymentResilienceReadyState;

    public BrowserClientRuntimeBrowserReleaseResilienceSessionService(IBrowserClientRuntimeBrowserDeploymentResilienceReadyState runtimeBrowserDeploymentResilienceReadyState)
    {
        _runtimeBrowserDeploymentResilienceReadyState = runtimeBrowserDeploymentResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReleaseResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeploymentResilienceReadyStateResult prevReadyState = await _runtimeBrowserDeploymentResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReleaseResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserDeploymentResilienceReadyStateVersion = prevReadyState.BrowserDeploymentResilienceReadyStateVersion,
            BrowserDeploymentResilienceSessionVersion = prevReadyState.BrowserDeploymentResilienceSessionVersion,
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
            result.Summary = $"Runtime browser releaseresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReleaseResilienceSessionVersion = "runtime-browser-releaseresilience-session-v1";
        result.BrowserReleaseResilienceStages =
        [
            "open-browser-releaseresilience-session",
            "bind-browser-deploymentresilience-ready-state",
            "publish-browser-releaseresilience-ready"
        ];
        result.BrowserReleaseResilienceSummary = $"Runtime browser releaseresilience session prepared {result.BrowserReleaseResilienceStages.Length} releaseresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser releaseresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserReleaseResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReleaseResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserReleaseResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserDeploymentResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeploymentResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReleaseResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserReleaseResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
