namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserSessionStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionStabilitySessionService : IBrowserClientRuntimeBrowserSessionStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeStabilityReadyState _runtimeBrowserRuntimeStabilityReadyState;

    public BrowserClientRuntimeBrowserSessionStabilitySessionService(IBrowserClientRuntimeBrowserRuntimeStabilityReadyState runtimeBrowserRuntimeStabilityReadyState)
    {
        _runtimeBrowserRuntimeStabilityReadyState = runtimeBrowserRuntimeStabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeStabilityReadyStateResult runtimestabilityReadyState = await _runtimeBrowserRuntimeStabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSessionStabilitySessionResult result = new()
        {
            ProfileId = runtimestabilityReadyState.ProfileId,
            SessionId = runtimestabilityReadyState.SessionId,
            SessionPath = runtimestabilityReadyState.SessionPath,
            BrowserRuntimeStabilityReadyStateVersion = runtimestabilityReadyState.BrowserRuntimeStabilityReadyStateVersion,
            BrowserRuntimeStabilitySessionVersion = runtimestabilityReadyState.BrowserRuntimeStabilitySessionVersion,
            LaunchMode = runtimestabilityReadyState.LaunchMode,
            AssetRootPath = runtimestabilityReadyState.AssetRootPath,
            ProfilesRootPath = runtimestabilityReadyState.ProfilesRootPath,
            CacheRootPath = runtimestabilityReadyState.CacheRootPath,
            ConfigRootPath = runtimestabilityReadyState.ConfigRootPath,
            SettingsFilePath = runtimestabilityReadyState.SettingsFilePath,
            StartupProfilePath = runtimestabilityReadyState.StartupProfilePath,
            RequiredAssets = runtimestabilityReadyState.RequiredAssets,
            ReadyAssetCount = runtimestabilityReadyState.ReadyAssetCount,
            CompletedSteps = runtimestabilityReadyState.CompletedSteps,
            TotalSteps = runtimestabilityReadyState.TotalSteps,
            Exists = runtimestabilityReadyState.Exists,
            ReadSucceeded = runtimestabilityReadyState.ReadSucceeded
        };

        if (!runtimestabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sessionstability session blocked for profile '{runtimestabilityReadyState.ProfileId}'.";
            result.Error = runtimestabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionStabilitySessionVersion = "runtime-browser-sessionstability-session-v1";
        result.BrowserSessionStabilityStages =
        [
            "open-browser-sessionstability-session",
            "bind-browser-runtimestability-ready-state",
            "publish-browser-sessionstability-ready"
        ];
        result.BrowserSessionStabilitySummary = $"Runtime browser sessionstability session prepared {result.BrowserSessionStabilityStages.Length} sessionstability stage(s) for profile '{runtimestabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessionstability session ready for profile '{runtimestabilityReadyState.ProfileId}' with {result.BrowserSessionStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserSessionStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
