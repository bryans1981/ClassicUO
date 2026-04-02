namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRetentionSession
{
    ValueTask<BrowserClientRuntimeBrowserRetentionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRetentionSessionService : IBrowserClientRuntimeBrowserRetentionSession
{
    private readonly IBrowserClientRuntimeBrowserMemorabilityReadyState _runtimeBrowserMemorabilityReadyState;

    public BrowserClientRuntimeBrowserRetentionSessionService(IBrowserClientRuntimeBrowserMemorabilityReadyState runtimeBrowserMemorabilityReadyState)
    {
        _runtimeBrowserMemorabilityReadyState = runtimeBrowserMemorabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRetentionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMemorabilityReadyStateResult memorabilityReadyState = await _runtimeBrowserMemorabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRetentionSessionResult result = new()
        {
            ProfileId = memorabilityReadyState.ProfileId,
            SessionId = memorabilityReadyState.SessionId,
            SessionPath = memorabilityReadyState.SessionPath,
            BrowserMemorabilityReadyStateVersion = memorabilityReadyState.BrowserMemorabilityReadyStateVersion,
            BrowserMemorabilitySessionVersion = memorabilityReadyState.BrowserMemorabilitySessionVersion,
            LaunchMode = memorabilityReadyState.LaunchMode,
            AssetRootPath = memorabilityReadyState.AssetRootPath,
            ProfilesRootPath = memorabilityReadyState.ProfilesRootPath,
            CacheRootPath = memorabilityReadyState.CacheRootPath,
            ConfigRootPath = memorabilityReadyState.ConfigRootPath,
            SettingsFilePath = memorabilityReadyState.SettingsFilePath,
            StartupProfilePath = memorabilityReadyState.StartupProfilePath,
            RequiredAssets = memorabilityReadyState.RequiredAssets,
            ReadyAssetCount = memorabilityReadyState.ReadyAssetCount,
            CompletedSteps = memorabilityReadyState.CompletedSteps,
            TotalSteps = memorabilityReadyState.TotalSteps,
            Exists = memorabilityReadyState.Exists,
            ReadSucceeded = memorabilityReadyState.ReadSucceeded
        };

        if (!memorabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser retention session blocked for profile '{memorabilityReadyState.ProfileId}'.";
            result.Error = memorabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRetentionSessionVersion = "runtime-browser-retention-session-v1";
        result.BrowserRetentionStages =
        [
            "open-browser-retention-session",
            "bind-browser-memorability-ready-state",
            "publish-browser-retention-ready"
        ];
        result.BrowserRetentionSummary = $"Runtime browser retention session prepared {result.BrowserRetentionStages.Length} retention stage(s) for profile '{memorabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser retention session ready for profile '{memorabilityReadyState.ProfileId}' with {result.BrowserRetentionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRetentionSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserRetentionStages { get; set; } = Array.Empty<string>();
    public string BrowserRetentionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
