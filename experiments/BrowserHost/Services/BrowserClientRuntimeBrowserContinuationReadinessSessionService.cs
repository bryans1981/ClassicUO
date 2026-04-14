namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserContinuationReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationReadinessSessionService : IBrowserClientRuntimeBrowserContinuationReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserFlowContinuityReadyState _runtimeBrowserFlowContinuityReadyState;

    public BrowserClientRuntimeBrowserContinuationReadinessSessionService(IBrowserClientRuntimeBrowserFlowContinuityReadyState runtimeBrowserFlowContinuityReadyState)
    {
        _runtimeBrowserFlowContinuityReadyState = runtimeBrowserFlowContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowContinuityReadyStateResult prevReadyState = await _runtimeBrowserFlowContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserContinuationReadinessSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserFlowContinuityReadyStateVersion = prevReadyState.BrowserFlowContinuityReadyStateVersion,
            BrowserFlowContinuitySessionVersion = prevReadyState.BrowserFlowContinuitySessionVersion,
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
            result.Summary = $"Runtime browser continuationreadiness session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationReadinessSessionVersion = "runtime-browser-continuationreadiness-session-v1";
        result.BrowserContinuationReadinessStages =
        [
            "open-browser-continuationreadiness-session",
            "bind-browser-flowcontinuity-ready-state",
            "publish-browser-continuationreadiness-ready"
        ];
        result.BrowserContinuationReadinessSummary = $"Runtime browser continuationreadiness session prepared {result.BrowserContinuationReadinessStages.Length} continuationreadiness stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationreadiness session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserContinuationReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserFlowContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserContinuationReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
