namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserHistoryReadyState
{
    ValueTask<BrowserClientRuntimeBrowserHistoryReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserHistoryReadyStateService : IBrowserClientRuntimeBrowserHistoryReadyState
{
    private readonly IBrowserClientRuntimeBrowserHistorySession _runtimeBrowserHistorySession;

    public BrowserClientRuntimeBrowserHistoryReadyStateService(IBrowserClientRuntimeBrowserHistorySession runtimeBrowserHistorySession)
    {
        _runtimeBrowserHistorySession = runtimeBrowserHistorySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserHistoryReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserHistorySessionResult historySession = await _runtimeBrowserHistorySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserHistoryReadyStateResult result = new()
        {
            ProfileId = historySession.ProfileId,
            SessionId = historySession.SessionId,
            SessionPath = historySession.SessionPath,
            BrowserHistorySessionVersion = historySession.BrowserHistorySessionVersion,
            BrowserPersistenceReadyStateVersion = historySession.BrowserPersistenceReadyStateVersion,
            BrowserPersistenceSessionVersion = historySession.BrowserPersistenceSessionVersion,
            LaunchMode = historySession.LaunchMode,
            AssetRootPath = historySession.AssetRootPath,
            ProfilesRootPath = historySession.ProfilesRootPath,
            CacheRootPath = historySession.CacheRootPath,
            ConfigRootPath = historySession.ConfigRootPath,
            SettingsFilePath = historySession.SettingsFilePath,
            StartupProfilePath = historySession.StartupProfilePath,
            RequiredAssets = historySession.RequiredAssets,
            ReadyAssetCount = historySession.ReadyAssetCount,
            CompletedSteps = historySession.CompletedSteps,
            TotalSteps = historySession.TotalSteps,
            Exists = historySession.Exists,
            ReadSucceeded = historySession.ReadSucceeded,
            BrowserHistorySession = historySession
        };

        if (!historySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser history ready state blocked for profile '{historySession.ProfileId}'.";
            result.Error = historySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserHistoryReadyStateVersion = "runtime-browser-history-ready-state-v1";
        result.BrowserHistoryReadyChecks =
        [
            "browser-persistence-ready-state-ready",
            "browser-history-session-ready",
            "browser-history-ready"
        ];
        result.BrowserHistoryReadySummary = $"Runtime browser history ready state passed {result.BrowserHistoryReadyChecks.Length} history readiness check(s) for profile '{historySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser history ready state ready for profile '{historySession.ProfileId}' with {result.BrowserHistoryReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserHistoryReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserHistoryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHistorySessionVersion { get; set; } = string.Empty;
    public string BrowserPersistenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPersistenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserHistoryReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserHistoryReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserHistorySessionResult BrowserHistorySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
