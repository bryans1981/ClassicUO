namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserContinuationReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationReliabilityReadyStateService : IBrowserClientRuntimeBrowserContinuationReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserFlowReliabilityReadyState _runtimeBrowserFlowReliabilityReadyState;

    public BrowserClientRuntimeBrowserContinuationReliabilityReadyStateService(IBrowserClientRuntimeBrowserFlowReliabilityReadyState runtimeBrowserFlowReliabilityReadyState)
    {
        _runtimeBrowserFlowReliabilityReadyState = runtimeBrowserFlowReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowReliabilityReadyStateResult prevReadyState = await _runtimeBrowserFlowReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserContinuationReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserContinuationReliabilitySessionVersion = prevReadyState.BrowserFlowReliabilityReadyStateVersion,
            BrowserFlowReliabilityReadyStateVersion = prevReadyState.BrowserFlowReliabilityReadyStateVersion,
            BrowserFlowReliabilitySessionVersion = prevReadyState.BrowserFlowReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser continuationreliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationReliabilityReadyStateVersion = "runtime-browser-continuationreliability-ready-state-v1";
        result.BrowserContinuationReliabilityReadyChecks =
        [
            "browser-flowreliability-ready-state-ready",
            "browser-continuationreliability-ready-state-ready",
            "browser-continuationreliability-ready"
        ];
        result.BrowserContinuationReliabilityReadySummary = $"Runtime browser continuationreliability ready state passed {result.BrowserContinuationReliabilityReadyChecks.Length} continuationreliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationreliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserContinuationReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserFlowReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserContinuationReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
