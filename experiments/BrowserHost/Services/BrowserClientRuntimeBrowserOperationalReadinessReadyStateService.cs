namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOperationalReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalReadinessReadyStateService : IBrowserClientRuntimeBrowserOperationalReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserOperationalReadinessSession _runtimeBrowserOperationalReadinessSession;

    public BrowserClientRuntimeBrowserOperationalReadinessReadyStateService(IBrowserClientRuntimeBrowserOperationalReadinessSession runtimeBrowserOperationalReadinessSession)
    {
        _runtimeBrowserOperationalReadinessSession = runtimeBrowserOperationalReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperationalReadinessSessionResult operationalreadinessSession = await _runtimeBrowserOperationalReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOperationalReadinessReadyStateResult result = new()
        {
            ProfileId = operationalreadinessSession.ProfileId,
            SessionId = operationalreadinessSession.SessionId,
            SessionPath = operationalreadinessSession.SessionPath,
            BrowserOperationalReadinessSessionVersion = operationalreadinessSession.BrowserOperationalReadinessSessionVersion,
            BrowserCompletionReadinessReadyStateVersion = operationalreadinessSession.BrowserCompletionReadinessReadyStateVersion,
            BrowserCompletionReadinessSessionVersion = operationalreadinessSession.BrowserCompletionReadinessSessionVersion,
            LaunchMode = operationalreadinessSession.LaunchMode,
            AssetRootPath = operationalreadinessSession.AssetRootPath,
            ProfilesRootPath = operationalreadinessSession.ProfilesRootPath,
            CacheRootPath = operationalreadinessSession.CacheRootPath,
            ConfigRootPath = operationalreadinessSession.ConfigRootPath,
            SettingsFilePath = operationalreadinessSession.SettingsFilePath,
            StartupProfilePath = operationalreadinessSession.StartupProfilePath,
            RequiredAssets = operationalreadinessSession.RequiredAssets,
            ReadyAssetCount = operationalreadinessSession.ReadyAssetCount,
            CompletedSteps = operationalreadinessSession.CompletedSteps,
            TotalSteps = operationalreadinessSession.TotalSteps,
            Exists = operationalreadinessSession.Exists,
            ReadSucceeded = operationalreadinessSession.ReadSucceeded
        };

        if (!operationalreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operationalreadiness ready state blocked for profile '{operationalreadinessSession.ProfileId}'.";
            result.Error = operationalreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalReadinessReadyStateVersion = "runtime-browser-operationalreadiness-ready-state-v1";
        result.BrowserOperationalReadinessReadyChecks =
        [
            "browser-completionreadiness-ready-state-ready",
            "browser-operationalreadiness-session-ready",
            "browser-operationalreadiness-ready"
        ];
        result.BrowserOperationalReadinessReadySummary = $"Runtime browser operationalreadiness ready state passed {result.BrowserOperationalReadinessReadyChecks.Length} operationalreadiness readiness check(s) for profile '{operationalreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalreadiness ready state ready for profile '{operationalreadinessSession.ProfileId}' with {result.BrowserOperationalReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOperationalReadinessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperationalReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOperationalReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
