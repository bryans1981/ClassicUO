namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeContinuitySession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeContinuitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeContinuitySessionService : IBrowserClientRuntimeBrowserRuntimeContinuitySession
{
    private readonly IBrowserClientRuntimeBrowserLiveReliabilityReadyState _runtimeBrowserLiveReliabilityReadyState;

    public BrowserClientRuntimeBrowserRuntimeContinuitySessionService(IBrowserClientRuntimeBrowserLiveReliabilityReadyState runtimeBrowserLiveReliabilityReadyState)
    {
        _runtimeBrowserLiveReliabilityReadyState = runtimeBrowserLiveReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeContinuitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveReliabilityReadyStateResult prevReadyState = await _runtimeBrowserLiveReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeContinuitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLiveReliabilityReadyStateVersion = prevReadyState.BrowserLiveReliabilityReadyStateVersion,
            BrowserLiveReliabilitySessionVersion = prevReadyState.BrowserLiveReliabilitySessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimecontinuity session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeContinuitySessionVersion = "runtime-browser-runtimecontinuity-session-v1";
        result.BrowserRuntimeContinuityStages =
        [
            "open-browser-runtimecontinuity-session",
            "bind-browser-livereliability-ready-state",
            "publish-browser-runtimecontinuity-ready"
        ];
        result.BrowserRuntimeContinuitySummary = $"Runtime browser runtimecontinuity session prepared {result.BrowserRuntimeContinuityStages.Length} runtimecontinuity stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimecontinuity session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserRuntimeContinuityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeContinuitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeContinuityStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeContinuitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
