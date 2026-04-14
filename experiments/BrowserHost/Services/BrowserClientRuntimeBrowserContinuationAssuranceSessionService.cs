namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserContinuationAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationAssuranceSessionService : IBrowserClientRuntimeBrowserContinuationAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserInteractionStabilityReadyState _runtimeBrowserInteractionStabilityReadyState;

    public BrowserClientRuntimeBrowserContinuationAssuranceSessionService(IBrowserClientRuntimeBrowserInteractionStabilityReadyState runtimeBrowserInteractionStabilityReadyState)
    {
        _runtimeBrowserInteractionStabilityReadyState = runtimeBrowserInteractionStabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionStabilityReadyStateResult interactionstabilityReadyState = await _runtimeBrowserInteractionStabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserContinuationAssuranceSessionResult result = new()
        {
            ProfileId = interactionstabilityReadyState.ProfileId,
            SessionId = interactionstabilityReadyState.SessionId,
            SessionPath = interactionstabilityReadyState.SessionPath,
            BrowserInteractionStabilityReadyStateVersion = interactionstabilityReadyState.BrowserInteractionStabilityReadyStateVersion,
            BrowserInteractionStabilitySessionVersion = interactionstabilityReadyState.BrowserInteractionStabilitySessionVersion,
            LaunchMode = interactionstabilityReadyState.LaunchMode,
            AssetRootPath = interactionstabilityReadyState.AssetRootPath,
            ProfilesRootPath = interactionstabilityReadyState.ProfilesRootPath,
            CacheRootPath = interactionstabilityReadyState.CacheRootPath,
            ConfigRootPath = interactionstabilityReadyState.ConfigRootPath,
            SettingsFilePath = interactionstabilityReadyState.SettingsFilePath,
            StartupProfilePath = interactionstabilityReadyState.StartupProfilePath,
            RequiredAssets = interactionstabilityReadyState.RequiredAssets,
            ReadyAssetCount = interactionstabilityReadyState.ReadyAssetCount,
            CompletedSteps = interactionstabilityReadyState.CompletedSteps,
            TotalSteps = interactionstabilityReadyState.TotalSteps,
            Exists = interactionstabilityReadyState.Exists,
            ReadSucceeded = interactionstabilityReadyState.ReadSucceeded
        };

        if (!interactionstabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser continuationassurance session blocked for profile '{interactionstabilityReadyState.ProfileId}'.";
            result.Error = interactionstabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationAssuranceSessionVersion = "runtime-browser-continuationassurance-session-v1";
        result.BrowserContinuationAssuranceStages =
        [
            "open-browser-continuationassurance-session",
            "bind-browser-interactionstability-ready-state",
            "publish-browser-continuationassurance-ready"
        ];
        result.BrowserContinuationAssuranceSummary = $"Runtime browser continuationassurance session prepared {result.BrowserContinuationAssuranceStages.Length} continuationassurance stage(s) for profile '{interactionstabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationassurance session ready for profile '{interactionstabilityReadyState.ProfileId}' with {result.BrowserContinuationAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationAssuranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserContinuationAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
