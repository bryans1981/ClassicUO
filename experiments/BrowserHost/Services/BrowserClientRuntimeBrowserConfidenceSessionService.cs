namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConfidenceSessionService : IBrowserClientRuntimeBrowserConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserRetentionReadyState _runtimeBrowserRetentionReadyState;

    public BrowserClientRuntimeBrowserConfidenceSessionService(IBrowserClientRuntimeBrowserRetentionReadyState runtimeBrowserRetentionReadyState)
    {
        _runtimeBrowserRetentionReadyState = runtimeBrowserRetentionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRetentionReadyStateResult retentionReadyState = await _runtimeBrowserRetentionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserConfidenceSessionResult result = new()
        {
            ProfileId = retentionReadyState.ProfileId,
            SessionId = retentionReadyState.SessionId,
            SessionPath = retentionReadyState.SessionPath,
            BrowserRetentionReadyStateVersion = retentionReadyState.BrowserRetentionReadyStateVersion,
            BrowserRetentionSessionVersion = retentionReadyState.BrowserRetentionSessionVersion,
            LaunchMode = retentionReadyState.LaunchMode,
            AssetRootPath = retentionReadyState.AssetRootPath,
            ProfilesRootPath = retentionReadyState.ProfilesRootPath,
            CacheRootPath = retentionReadyState.CacheRootPath,
            ConfigRootPath = retentionReadyState.ConfigRootPath,
            SettingsFilePath = retentionReadyState.SettingsFilePath,
            StartupProfilePath = retentionReadyState.StartupProfilePath,
            RequiredAssets = retentionReadyState.RequiredAssets,
            ReadyAssetCount = retentionReadyState.ReadyAssetCount,
            CompletedSteps = retentionReadyState.CompletedSteps,
            TotalSteps = retentionReadyState.TotalSteps,
            Exists = retentionReadyState.Exists,
            ReadSucceeded = retentionReadyState.ReadSucceeded
        };

        if (!retentionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser confidence session blocked for profile '{retentionReadyState.ProfileId}'.";
            result.Error = retentionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConfidenceSessionVersion = "runtime-browser-confidence-session-v1";
        result.BrowserConfidenceStages =
        [
            "open-browser-confidence-session",
            "bind-browser-retention-ready-state",
            "publish-browser-confidence-ready"
        ];
        result.BrowserConfidenceSummary = $"Runtime browser confidence session prepared {result.BrowserConfidenceStages.Length} confidence stage(s) for profile '{retentionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser confidence session ready for profile '{retentionReadyState.ProfileId}' with {result.BrowserConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserRetentionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRetentionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
