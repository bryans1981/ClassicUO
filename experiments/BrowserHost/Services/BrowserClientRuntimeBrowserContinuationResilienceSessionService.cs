namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserContinuationResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationResilienceSessionService : IBrowserClientRuntimeBrowserContinuationResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserFlowResilienceReadyState _runtimeBrowserFlowResilienceReadyState;

    public BrowserClientRuntimeBrowserContinuationResilienceSessionService(IBrowserClientRuntimeBrowserFlowResilienceReadyState runtimeBrowserFlowResilienceReadyState)
    {
        _runtimeBrowserFlowResilienceReadyState = runtimeBrowserFlowResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowResilienceReadyStateResult prevReadyState = await _runtimeBrowserFlowResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserContinuationResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserFlowResilienceReadyStateVersion = prevReadyState.BrowserFlowResilienceReadyStateVersion,
            BrowserFlowResilienceSessionVersion = prevReadyState.BrowserFlowResilienceSessionVersion,
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
            result.Summary = $"Runtime browser continuationresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationResilienceSessionVersion = "runtime-browser-continuationresilience-session-v1";
        result.BrowserContinuationResilienceStages =
        [
            "open-browser-continuationresilience-session",
            "bind-browser-flowresilience-ready-state",
            "publish-browser-continuationresilience-ready"
        ];
        result.BrowserContinuationResilienceSummary = $"Runtime browser continuationresilience session prepared {result.BrowserContinuationResilienceStages.Length} continuationresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserContinuationResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserFlowResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserContinuationResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
