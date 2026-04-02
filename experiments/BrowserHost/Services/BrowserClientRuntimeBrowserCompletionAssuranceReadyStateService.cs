namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCompletionAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCompletionAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCompletionAssuranceReadyStateService : IBrowserClientRuntimeBrowserCompletionAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserCompletionAssuranceSession _runtimeBrowserCompletionAssuranceSession;

    public BrowserClientRuntimeBrowserCompletionAssuranceReadyStateService(IBrowserClientRuntimeBrowserCompletionAssuranceSession runtimeBrowserCompletionAssuranceSession)
    {
        _runtimeBrowserCompletionAssuranceSession = runtimeBrowserCompletionAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCompletionAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCompletionAssuranceSessionResult completionassuranceSession = await _runtimeBrowserCompletionAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCompletionAssuranceReadyStateResult result = new()
        {
            ProfileId = completionassuranceSession.ProfileId,
            SessionId = completionassuranceSession.SessionId,
            SessionPath = completionassuranceSession.SessionPath,
            BrowserCompletionAssuranceSessionVersion = completionassuranceSession.BrowserCompletionAssuranceSessionVersion,
            BrowserResolutionReadyStateVersion = completionassuranceSession.BrowserResolutionReadyStateVersion,
            BrowserResolutionSessionVersion = completionassuranceSession.BrowserResolutionSessionVersion,
            LaunchMode = completionassuranceSession.LaunchMode,
            AssetRootPath = completionassuranceSession.AssetRootPath,
            ProfilesRootPath = completionassuranceSession.ProfilesRootPath,
            CacheRootPath = completionassuranceSession.CacheRootPath,
            ConfigRootPath = completionassuranceSession.ConfigRootPath,
            SettingsFilePath = completionassuranceSession.SettingsFilePath,
            StartupProfilePath = completionassuranceSession.StartupProfilePath,
            RequiredAssets = completionassuranceSession.RequiredAssets,
            ReadyAssetCount = completionassuranceSession.ReadyAssetCount,
            CompletedSteps = completionassuranceSession.CompletedSteps,
            TotalSteps = completionassuranceSession.TotalSteps,
            Exists = completionassuranceSession.Exists,
            ReadSucceeded = completionassuranceSession.ReadSucceeded
        };

        if (!completionassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser completionassurance ready state blocked for profile '{completionassuranceSession.ProfileId}'.";
            result.Error = completionassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCompletionAssuranceReadyStateVersion = "runtime-browser-completionassurance-ready-state-v1";
        result.BrowserCompletionAssuranceReadyChecks =
        [
            "browser-resolution-ready-state-ready",
            "browser-completionassurance-session-ready",
            "browser-completionassurance-ready"
        ];
        result.BrowserCompletionAssuranceReadySummary = $"Runtime browser completionassurance ready state passed {result.BrowserCompletionAssuranceReadyChecks.Length} completionassurance readiness check(s) for profile '{completionassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser completionassurance ready state ready for profile '{completionassuranceSession.ProfileId}' with {result.BrowserCompletionAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCompletionAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCompletionAssuranceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserCompletionAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCompletionAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
