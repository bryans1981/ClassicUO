namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserOperationalReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalReadinessSessionService : IBrowserClientRuntimeBrowserOperationalReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserCompletionReadinessReadyState _runtimeBrowserCompletionReadinessReadyState;

    public BrowserClientRuntimeBrowserOperationalReadinessSessionService(IBrowserClientRuntimeBrowserCompletionReadinessReadyState runtimeBrowserCompletionReadinessReadyState)
    {
        _runtimeBrowserCompletionReadinessReadyState = runtimeBrowserCompletionReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCompletionReadinessReadyStateResult completionreadinessReadyState = await _runtimeBrowserCompletionReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOperationalReadinessSessionResult result = new()
        {
            ProfileId = completionreadinessReadyState.ProfileId,
            SessionId = completionreadinessReadyState.SessionId,
            SessionPath = completionreadinessReadyState.SessionPath,
            BrowserCompletionReadinessReadyStateVersion = completionreadinessReadyState.BrowserCompletionReadinessReadyStateVersion,
            BrowserCompletionReadinessSessionVersion = completionreadinessReadyState.BrowserCompletionReadinessSessionVersion,
            LaunchMode = completionreadinessReadyState.LaunchMode,
            AssetRootPath = completionreadinessReadyState.AssetRootPath,
            ProfilesRootPath = completionreadinessReadyState.ProfilesRootPath,
            CacheRootPath = completionreadinessReadyState.CacheRootPath,
            ConfigRootPath = completionreadinessReadyState.ConfigRootPath,
            SettingsFilePath = completionreadinessReadyState.SettingsFilePath,
            StartupProfilePath = completionreadinessReadyState.StartupProfilePath,
            RequiredAssets = completionreadinessReadyState.RequiredAssets,
            ReadyAssetCount = completionreadinessReadyState.ReadyAssetCount,
            CompletedSteps = completionreadinessReadyState.CompletedSteps,
            TotalSteps = completionreadinessReadyState.TotalSteps,
            Exists = completionreadinessReadyState.Exists,
            ReadSucceeded = completionreadinessReadyState.ReadSucceeded
        };

        if (!completionreadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operationalreadiness session blocked for profile '{completionreadinessReadyState.ProfileId}'.";
            result.Error = completionreadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalReadinessSessionVersion = "runtime-browser-operationalreadiness-session-v1";
        result.BrowserOperationalReadinessStages =
        [
            "open-browser-operationalreadiness-session",
            "bind-browser-completionreadiness-ready-state",
            "publish-browser-operationalreadiness-ready"
        ];
        result.BrowserOperationalReadinessSummary = $"Runtime browser operationalreadiness session prepared {result.BrowserOperationalReadinessStages.Length} operationalreadiness stage(s) for profile '{completionreadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalreadiness session ready for profile '{completionreadinessReadyState.ProfileId}' with {result.BrowserOperationalReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserOperationalReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserCompletionReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCompletionReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperationalReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserOperationalReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
