namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRecoveryReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRecoveryReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRecoveryReadyStateService : IBrowserClientRuntimeBrowserRecoveryReadyState
{
    private readonly IBrowserClientRuntimeBrowserRecoverySession _runtimeBrowserRecoverySession;

    public BrowserClientRuntimeBrowserRecoveryReadyStateService(IBrowserClientRuntimeBrowserRecoverySession runtimeBrowserRecoverySession)
    {
        _runtimeBrowserRecoverySession = runtimeBrowserRecoverySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRecoveryReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRecoverySessionResult recoverySession = await _runtimeBrowserRecoverySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRecoveryReadyStateResult result = new()
        {
            ProfileId = recoverySession.ProfileId,
            SessionId = recoverySession.SessionId,
            SessionPath = recoverySession.SessionPath,
            BrowserRecoverySessionVersion = recoverySession.BrowserRecoverySessionVersion,
            BrowserHistoryReadyStateVersion = recoverySession.BrowserHistoryReadyStateVersion,
            BrowserHistorySessionVersion = recoverySession.BrowserHistorySessionVersion,
            LaunchMode = recoverySession.LaunchMode,
            AssetRootPath = recoverySession.AssetRootPath,
            ProfilesRootPath = recoverySession.ProfilesRootPath,
            CacheRootPath = recoverySession.CacheRootPath,
            ConfigRootPath = recoverySession.ConfigRootPath,
            SettingsFilePath = recoverySession.SettingsFilePath,
            StartupProfilePath = recoverySession.StartupProfilePath,
            RequiredAssets = recoverySession.RequiredAssets,
            ReadyAssetCount = recoverySession.ReadyAssetCount,
            CompletedSteps = recoverySession.CompletedSteps,
            TotalSteps = recoverySession.TotalSteps,
            Exists = recoverySession.Exists,
            ReadSucceeded = recoverySession.ReadSucceeded,
            BrowserRecoverySession = recoverySession
        };

        if (!recoverySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser recovery ready state blocked for profile '{recoverySession.ProfileId}'.";
            result.Error = recoverySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRecoveryReadyStateVersion = "runtime-browser-recovery-ready-state-v1";
        result.BrowserRecoveryReadyChecks =
        [
            "browser-history-ready-state-ready",
            "browser-recovery-session-ready",
            "browser-recovery-ready"
        ];
        result.BrowserRecoveryReadySummary = $"Runtime browser recovery ready state passed {result.BrowserRecoveryReadyChecks.Length} recovery readiness check(s) for profile '{recoverySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser recovery ready state ready for profile '{recoverySession.ProfileId}' with {result.BrowserRecoveryReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRecoveryReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRecoveryReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserRecoveryReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRecoveryReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRecoverySessionResult BrowserRecoverySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
