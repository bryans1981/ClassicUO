namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCompletionAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserCompletionAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCompletionAssuranceSessionService : IBrowserClientRuntimeBrowserCompletionAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserResolutionReadyState _runtimeBrowserResolutionReadyState;

    public BrowserClientRuntimeBrowserCompletionAssuranceSessionService(IBrowserClientRuntimeBrowserResolutionReadyState runtimeBrowserResolutionReadyState)
    {
        _runtimeBrowserResolutionReadyState = runtimeBrowserResolutionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCompletionAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResolutionReadyStateResult resolutionReadyState = await _runtimeBrowserResolutionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCompletionAssuranceSessionResult result = new()
        {
            ProfileId = resolutionReadyState.ProfileId,
            SessionId = resolutionReadyState.SessionId,
            SessionPath = resolutionReadyState.SessionPath,
            BrowserResolutionReadyStateVersion = resolutionReadyState.BrowserResolutionReadyStateVersion,
            BrowserResolutionSessionVersion = resolutionReadyState.BrowserResolutionSessionVersion,
            LaunchMode = resolutionReadyState.LaunchMode,
            AssetRootPath = resolutionReadyState.AssetRootPath,
            ProfilesRootPath = resolutionReadyState.ProfilesRootPath,
            CacheRootPath = resolutionReadyState.CacheRootPath,
            ConfigRootPath = resolutionReadyState.ConfigRootPath,
            SettingsFilePath = resolutionReadyState.SettingsFilePath,
            StartupProfilePath = resolutionReadyState.StartupProfilePath,
            RequiredAssets = resolutionReadyState.RequiredAssets,
            ReadyAssetCount = resolutionReadyState.ReadyAssetCount,
            CompletedSteps = resolutionReadyState.CompletedSteps,
            TotalSteps = resolutionReadyState.TotalSteps,
            Exists = resolutionReadyState.Exists,
            ReadSucceeded = resolutionReadyState.ReadSucceeded
        };

        if (!resolutionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser completionassurance session blocked for profile '{resolutionReadyState.ProfileId}'.";
            result.Error = resolutionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCompletionAssuranceSessionVersion = "runtime-browser-completionassurance-session-v1";
        result.BrowserCompletionAssuranceStages =
        [
            "open-browser-completionassurance-session",
            "bind-browser-resolution-ready-state",
            "publish-browser-completionassurance-ready"
        ];
        result.BrowserCompletionAssuranceSummary = $"Runtime browser completionassurance session prepared {result.BrowserCompletionAssuranceStages.Length} completionassurance stage(s) for profile '{resolutionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser completionassurance session ready for profile '{resolutionReadyState.ProfileId}' with {result.BrowserCompletionAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCompletionAssuranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCompletionAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserResolutionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResolutionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCompletionAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserCompletionAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
