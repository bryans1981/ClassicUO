namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserInteractionStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionStabilitySessionService : IBrowserClientRuntimeBrowserInteractionStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserStateStabilityReadyState _runtimeBrowserStateStabilityReadyState;

    public BrowserClientRuntimeBrowserInteractionStabilitySessionService(IBrowserClientRuntimeBrowserStateStabilityReadyState runtimeBrowserStateStabilityReadyState)
    {
        _runtimeBrowserStateStabilityReadyState = runtimeBrowserStateStabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateStabilityReadyStateResult statestabilityReadyState = await _runtimeBrowserStateStabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInteractionStabilitySessionResult result = new()
        {
            ProfileId = statestabilityReadyState.ProfileId,
            SessionId = statestabilityReadyState.SessionId,
            SessionPath = statestabilityReadyState.SessionPath,
            BrowserStateStabilityReadyStateVersion = statestabilityReadyState.BrowserStateStabilityReadyStateVersion,
            BrowserStateStabilitySessionVersion = statestabilityReadyState.BrowserStateStabilitySessionVersion,
            LaunchMode = statestabilityReadyState.LaunchMode,
            AssetRootPath = statestabilityReadyState.AssetRootPath,
            ProfilesRootPath = statestabilityReadyState.ProfilesRootPath,
            CacheRootPath = statestabilityReadyState.CacheRootPath,
            ConfigRootPath = statestabilityReadyState.ConfigRootPath,
            SettingsFilePath = statestabilityReadyState.SettingsFilePath,
            StartupProfilePath = statestabilityReadyState.StartupProfilePath,
            RequiredAssets = statestabilityReadyState.RequiredAssets,
            ReadyAssetCount = statestabilityReadyState.ReadyAssetCount,
            CompletedSteps = statestabilityReadyState.CompletedSteps,
            TotalSteps = statestabilityReadyState.TotalSteps,
            Exists = statestabilityReadyState.Exists,
            ReadSucceeded = statestabilityReadyState.ReadSucceeded
        };

        if (!statestabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser interactionstability session blocked for profile '{statestabilityReadyState.ProfileId}'.";
            result.Error = statestabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionStabilitySessionVersion = "runtime-browser-interactionstability-session-v1";
        result.BrowserInteractionStabilityStages =
        [
            "open-browser-interactionstability-session",
            "bind-browser-statestability-ready-state",
            "publish-browser-interactionstability-ready"
        ];
        result.BrowserInteractionStabilitySummary = $"Runtime browser interactionstability session prepared {result.BrowserInteractionStabilityStages.Length} interactionstability stage(s) for profile '{statestabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interactionstability session ready for profile '{statestabilityReadyState.ProfileId}' with {result.BrowserInteractionStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserStateStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserInteractionStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
