namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCompletionReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserCompletionReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCompletionReadinessSessionService : IBrowserClientRuntimeBrowserCompletionReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserAccomplishmentReadyState _runtimeBrowserAccomplishmentReadyState;

    public BrowserClientRuntimeBrowserCompletionReadinessSessionService(IBrowserClientRuntimeBrowserAccomplishmentReadyState runtimeBrowserAccomplishmentReadyState)
    {
        _runtimeBrowserAccomplishmentReadyState = runtimeBrowserAccomplishmentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCompletionReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAccomplishmentReadyStateResult accomplishmentReadyState = await _runtimeBrowserAccomplishmentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCompletionReadinessSessionResult result = new()
        {
            ProfileId = accomplishmentReadyState.ProfileId,
            SessionId = accomplishmentReadyState.SessionId,
            SessionPath = accomplishmentReadyState.SessionPath,
            BrowserAccomplishmentReadyStateVersion = accomplishmentReadyState.BrowserAccomplishmentReadyStateVersion,
            BrowserAccomplishmentSessionVersion = accomplishmentReadyState.BrowserAccomplishmentSessionVersion,
            LaunchMode = accomplishmentReadyState.LaunchMode,
            AssetRootPath = accomplishmentReadyState.AssetRootPath,
            ProfilesRootPath = accomplishmentReadyState.ProfilesRootPath,
            CacheRootPath = accomplishmentReadyState.CacheRootPath,
            ConfigRootPath = accomplishmentReadyState.ConfigRootPath,
            SettingsFilePath = accomplishmentReadyState.SettingsFilePath,
            StartupProfilePath = accomplishmentReadyState.StartupProfilePath,
            RequiredAssets = accomplishmentReadyState.RequiredAssets,
            ReadyAssetCount = accomplishmentReadyState.ReadyAssetCount,
            CompletedSteps = accomplishmentReadyState.CompletedSteps,
            TotalSteps = accomplishmentReadyState.TotalSteps,
            Exists = accomplishmentReadyState.Exists,
            ReadSucceeded = accomplishmentReadyState.ReadSucceeded
        };

        if (!accomplishmentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser completionreadiness session blocked for profile '{accomplishmentReadyState.ProfileId}'.";
            result.Error = accomplishmentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCompletionReadinessSessionVersion = "runtime-browser-completionreadiness-session-v1";
        result.BrowserCompletionReadinessStages =
        [
            "open-browser-completionreadiness-session",
            "bind-browser-accomplishment-ready-state",
            "publish-browser-completionreadiness-ready"
        ];
        result.BrowserCompletionReadinessSummary = $"Runtime browser completionreadiness session prepared {result.BrowserCompletionReadinessStages.Length} completionreadiness stage(s) for profile '{accomplishmentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser completionreadiness session ready for profile '{accomplishmentReadyState.ProfileId}' with {result.BrowserCompletionReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCompletionReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCompletionReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserAccomplishmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAccomplishmentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCompletionReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserCompletionReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
