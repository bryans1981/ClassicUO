namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionContinuitySession
{
    ValueTask<BrowserClientRuntimeBrowserInteractionContinuitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionContinuitySessionService : IBrowserClientRuntimeBrowserInteractionContinuitySession
{
    private readonly IBrowserClientRuntimeBrowserStateContinuityReadyState _runtimeBrowserStateContinuityReadyState;

    public BrowserClientRuntimeBrowserInteractionContinuitySessionService(IBrowserClientRuntimeBrowserStateContinuityReadyState runtimeBrowserStateContinuityReadyState)
    {
        _runtimeBrowserStateContinuityReadyState = runtimeBrowserStateContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionContinuitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateContinuityReadyStateResult prevReadyState = await _runtimeBrowserStateContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInteractionContinuitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserStateContinuityReadyStateVersion = prevReadyState.BrowserStateContinuityReadyStateVersion,
            BrowserStateContinuitySessionVersion = prevReadyState.BrowserStateContinuitySessionVersion,
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
            result.Summary = $"Runtime browser interactioncontinuity session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionContinuitySessionVersion = "runtime-browser-interactioncontinuity-session-v1";
        result.BrowserInteractionContinuityStages =
        [
            "open-browser-interactioncontinuity-session",
            "bind-browser-statecontinuity-ready-state",
            "publish-browser-interactioncontinuity-ready"
        ];
        result.BrowserInteractionContinuitySummary = $"Runtime browser interactioncontinuity session prepared {result.BrowserInteractionContinuityStages.Length} interactioncontinuity stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interactioncontinuity session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserInteractionContinuityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionContinuitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserStateContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionContinuityStages { get; set; } = Array.Empty<string>();
    public string BrowserInteractionContinuitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
