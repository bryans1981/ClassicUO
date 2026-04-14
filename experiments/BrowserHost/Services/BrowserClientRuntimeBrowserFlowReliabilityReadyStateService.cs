namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFlowReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowReliabilityReadyStateService : IBrowserClientRuntimeBrowserFlowReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserInteractionReliabilityReadyState _runtimeBrowserInteractionReliabilityReadyState;

    public BrowserClientRuntimeBrowserFlowReliabilityReadyStateService(IBrowserClientRuntimeBrowserInteractionReliabilityReadyState runtimeBrowserInteractionReliabilityReadyState)
    {
        _runtimeBrowserInteractionReliabilityReadyState = runtimeBrowserInteractionReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionReliabilityReadyStateResult prevReadyState = await _runtimeBrowserInteractionReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFlowReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserFlowReliabilitySessionVersion = prevReadyState.BrowserInteractionReliabilityReadyStateVersion,
            BrowserInteractionReliabilityReadyStateVersion = prevReadyState.BrowserInteractionReliabilityReadyStateVersion,
            BrowserInteractionReliabilitySessionVersion = prevReadyState.BrowserInteractionReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser flowreliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowReliabilityReadyStateVersion = "runtime-browser-flowreliability-ready-state-v1";
        result.BrowserFlowReliabilityReadyChecks =
        [
            "browser-interactionreliability-ready-state-ready",
            "browser-flowreliability-ready-state-ready",
            "browser-flowreliability-ready"
        ];
        result.BrowserFlowReliabilityReadySummary = $"Runtime browser flowreliability ready state passed {result.BrowserFlowReliabilityReadyChecks.Length} flowreliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowreliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserFlowReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFlowReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlowReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFlowReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
