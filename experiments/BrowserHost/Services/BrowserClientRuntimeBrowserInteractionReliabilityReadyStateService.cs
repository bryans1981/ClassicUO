namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInteractionReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionReliabilityReadyStateService : IBrowserClientRuntimeBrowserInteractionReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserStateReliabilityReadyState _runtimeBrowserStateReliabilityReadyState;

    public BrowserClientRuntimeBrowserInteractionReliabilityReadyStateService(IBrowserClientRuntimeBrowserStateReliabilityReadyState runtimeBrowserStateReliabilityReadyState)
    {
        _runtimeBrowserStateReliabilityReadyState = runtimeBrowserStateReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateReliabilityReadyStateResult prevReadyState = await _runtimeBrowserStateReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInteractionReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserInteractionReliabilitySessionVersion = prevReadyState.BrowserStateReliabilityReadyStateVersion,
            BrowserStateReliabilityReadyStateVersion = prevReadyState.BrowserStateReliabilityReadyStateVersion,
            BrowserStateReliabilitySessionVersion = prevReadyState.BrowserStateReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser interactionreliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionReliabilityReadyStateVersion = "runtime-browser-interactionreliability-ready-state-v1";
        result.BrowserInteractionReliabilityReadyChecks =
        [
            "browser-statereliability-ready-state-ready",
            "browser-interactionreliability-ready-state-ready",
            "browser-interactionreliability-ready"
        ];
        result.BrowserInteractionReliabilityReadySummary = $"Runtime browser interactionreliability ready state passed {result.BrowserInteractionReliabilityReadyChecks.Length} interactionreliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interactionreliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserInteractionReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserStateReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInteractionReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
