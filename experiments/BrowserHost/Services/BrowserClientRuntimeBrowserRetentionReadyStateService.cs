namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRetentionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRetentionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRetentionReadyStateService : IBrowserClientRuntimeBrowserRetentionReadyState
{
    private readonly IBrowserClientRuntimeBrowserRetentionSession _runtimeBrowserRetentionSession;

    public BrowserClientRuntimeBrowserRetentionReadyStateService(IBrowserClientRuntimeBrowserRetentionSession runtimeBrowserRetentionSession)
    {
        _runtimeBrowserRetentionSession = runtimeBrowserRetentionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRetentionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRetentionSessionResult retentionSession = await _runtimeBrowserRetentionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRetentionReadyStateResult result = new()
        {
            ProfileId = retentionSession.ProfileId,
            SessionId = retentionSession.SessionId,
            SessionPath = retentionSession.SessionPath,
            BrowserRetentionSessionVersion = retentionSession.BrowserRetentionSessionVersion,
            BrowserMemorabilityReadyStateVersion = retentionSession.BrowserMemorabilityReadyStateVersion,
            BrowserMemorabilitySessionVersion = retentionSession.BrowserMemorabilitySessionVersion,
            LaunchMode = retentionSession.LaunchMode,
            AssetRootPath = retentionSession.AssetRootPath,
            ProfilesRootPath = retentionSession.ProfilesRootPath,
            CacheRootPath = retentionSession.CacheRootPath,
            ConfigRootPath = retentionSession.ConfigRootPath,
            SettingsFilePath = retentionSession.SettingsFilePath,
            StartupProfilePath = retentionSession.StartupProfilePath,
            RequiredAssets = retentionSession.RequiredAssets,
            ReadyAssetCount = retentionSession.ReadyAssetCount,
            CompletedSteps = retentionSession.CompletedSteps,
            TotalSteps = retentionSession.TotalSteps,
            Exists = retentionSession.Exists,
            ReadSucceeded = retentionSession.ReadSucceeded
        };

        if (!retentionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser retention ready state blocked for profile '{retentionSession.ProfileId}'.";
            result.Error = retentionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRetentionReadyStateVersion = "runtime-browser-retention-ready-state-v1";
        result.BrowserRetentionReadyChecks =
        [
            "browser-memorability-ready-state-ready",
            "browser-retention-session-ready",
            "browser-retention-ready"
        ];
        result.BrowserRetentionReadySummary = $"Runtime browser retention ready state passed {result.BrowserRetentionReadyChecks.Length} retention readiness check(s) for profile '{retentionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser retention ready state ready for profile '{retentionSession.ProfileId}' with {result.BrowserRetentionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRetentionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRetentionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRetentionSessionVersion { get; set; } = string.Empty;
    public string BrowserMemorabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMemorabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRetentionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRetentionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
