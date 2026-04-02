namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRecoverySession
{
    ValueTask<BrowserClientRuntimeBrowserRecoverySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRecoverySessionService : IBrowserClientRuntimeBrowserRecoverySession
{
    private readonly IBrowserClientRuntimeBrowserHistoryReadyState _runtimeBrowserHistoryReadyState;

    public BrowserClientRuntimeBrowserRecoverySessionService(IBrowserClientRuntimeBrowserHistoryReadyState runtimeBrowserHistoryReadyState)
    {
        _runtimeBrowserHistoryReadyState = runtimeBrowserHistoryReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRecoverySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserHistoryReadyStateResult historyReadyState = await _runtimeBrowserHistoryReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRecoverySessionResult result = new()
        {
            ProfileId = historyReadyState.ProfileId,
            SessionId = historyReadyState.SessionId,
            SessionPath = historyReadyState.SessionPath,
            BrowserHistoryReadyStateVersion = historyReadyState.BrowserHistoryReadyStateVersion,
            BrowserHistorySessionVersion = historyReadyState.BrowserHistorySessionVersion,
            LaunchMode = historyReadyState.LaunchMode,
            AssetRootPath = historyReadyState.AssetRootPath,
            ProfilesRootPath = historyReadyState.ProfilesRootPath,
            CacheRootPath = historyReadyState.CacheRootPath,
            ConfigRootPath = historyReadyState.ConfigRootPath,
            SettingsFilePath = historyReadyState.SettingsFilePath,
            StartupProfilePath = historyReadyState.StartupProfilePath,
            RequiredAssets = historyReadyState.RequiredAssets,
            ReadyAssetCount = historyReadyState.ReadyAssetCount,
            CompletedSteps = historyReadyState.CompletedSteps,
            TotalSteps = historyReadyState.TotalSteps,
            Exists = historyReadyState.Exists,
            ReadSucceeded = historyReadyState.ReadSucceeded,
            BrowserHistoryReadyState = historyReadyState
        };

        if (!historyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser recovery session blocked for profile '{historyReadyState.ProfileId}'.";
            result.Error = historyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRecoverySessionVersion = "runtime-browser-recovery-session-v1";
        result.BrowserRecoveryStages =
        [
            "open-browser-recovery-session",
            "bind-browser-history-ready-state",
            "publish-browser-recovery-ready"
        ];
        result.BrowserRecoverySummary = $"Runtime browser recovery session prepared {result.BrowserRecoveryStages.Length} recovery stage(s) for profile '{historyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser recovery session ready for profile '{historyReadyState.ProfileId}' with {result.BrowserRecoveryStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRecoverySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRecoverySessionVersion { get; set; } = string.Empty;
    public string BrowserHistoryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHistorySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRecoveryStages { get; set; } = Array.Empty<string>();
    public string BrowserRecoverySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserHistoryReadyStateResult BrowserHistoryReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
