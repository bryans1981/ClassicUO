namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCompletionReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCompletionReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCompletionReadinessReadyStateService : IBrowserClientRuntimeBrowserCompletionReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserCompletionReadinessSession _runtimeBrowserCompletionReadinessSession;

    public BrowserClientRuntimeBrowserCompletionReadinessReadyStateService(IBrowserClientRuntimeBrowserCompletionReadinessSession runtimeBrowserCompletionReadinessSession)
    {
        _runtimeBrowserCompletionReadinessSession = runtimeBrowserCompletionReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCompletionReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCompletionReadinessSessionResult completionreadinessSession = await _runtimeBrowserCompletionReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCompletionReadinessReadyStateResult result = new()
        {
            ProfileId = completionreadinessSession.ProfileId,
            SessionId = completionreadinessSession.SessionId,
            SessionPath = completionreadinessSession.SessionPath,
            BrowserCompletionReadinessSessionVersion = completionreadinessSession.BrowserCompletionReadinessSessionVersion,
            BrowserAccomplishmentReadyStateVersion = completionreadinessSession.BrowserAccomplishmentReadyStateVersion,
            BrowserAccomplishmentSessionVersion = completionreadinessSession.BrowserAccomplishmentSessionVersion,
            LaunchMode = completionreadinessSession.LaunchMode,
            AssetRootPath = completionreadinessSession.AssetRootPath,
            ProfilesRootPath = completionreadinessSession.ProfilesRootPath,
            CacheRootPath = completionreadinessSession.CacheRootPath,
            ConfigRootPath = completionreadinessSession.ConfigRootPath,
            SettingsFilePath = completionreadinessSession.SettingsFilePath,
            StartupProfilePath = completionreadinessSession.StartupProfilePath,
            RequiredAssets = completionreadinessSession.RequiredAssets,
            ReadyAssetCount = completionreadinessSession.ReadyAssetCount,
            CompletedSteps = completionreadinessSession.CompletedSteps,
            TotalSteps = completionreadinessSession.TotalSteps,
            Exists = completionreadinessSession.Exists,
            ReadSucceeded = completionreadinessSession.ReadSucceeded
        };

        if (!completionreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser completionreadiness ready state blocked for profile '{completionreadinessSession.ProfileId}'.";
            result.Error = completionreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCompletionReadinessReadyStateVersion = "runtime-browser-completionreadiness-ready-state-v1";
        result.BrowserCompletionReadinessReadyChecks =
        [
            "browser-accomplishment-ready-state-ready",
            "browser-completionreadiness-session-ready",
            "browser-completionreadiness-ready"
        ];
        result.BrowserCompletionReadinessReadySummary = $"Runtime browser completionreadiness ready state passed {result.BrowserCompletionReadinessReadyChecks.Length} completionreadiness readiness check(s) for profile '{completionreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser completionreadiness ready state ready for profile '{completionreadinessSession.ProfileId}' with {result.BrowserCompletionReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCompletionReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCompletionReadinessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserCompletionReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCompletionReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
