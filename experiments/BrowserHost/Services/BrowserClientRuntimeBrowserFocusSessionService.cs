namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFocusSession
{
    ValueTask<BrowserClientRuntimeBrowserFocusSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFocusSessionService : IBrowserClientRuntimeBrowserFocusSession
{
    private readonly IBrowserClientRuntimeBrowserInteractionReadyState _runtimeBrowserInteractionReadyState;

    public BrowserClientRuntimeBrowserFocusSessionService(IBrowserClientRuntimeBrowserInteractionReadyState runtimeBrowserInteractionReadyState)
    {
        _runtimeBrowserInteractionReadyState = runtimeBrowserInteractionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFocusSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionReadyStateResult interactionReadyState = await _runtimeBrowserInteractionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFocusSessionResult result = new()
        {
            ProfileId = interactionReadyState.ProfileId,
            SessionId = interactionReadyState.SessionId,
            SessionPath = interactionReadyState.SessionPath,
            BrowserInteractionReadyStateVersion = interactionReadyState.BrowserInteractionReadyStateVersion,
            BrowserInteractionSessionVersion = interactionReadyState.BrowserInteractionSessionVersion,
            LaunchMode = interactionReadyState.LaunchMode,
            AssetRootPath = interactionReadyState.AssetRootPath,
            ProfilesRootPath = interactionReadyState.ProfilesRootPath,
            CacheRootPath = interactionReadyState.CacheRootPath,
            ConfigRootPath = interactionReadyState.ConfigRootPath,
            SettingsFilePath = interactionReadyState.SettingsFilePath,
            StartupProfilePath = interactionReadyState.StartupProfilePath,
            RequiredAssets = interactionReadyState.RequiredAssets,
            ReadyAssetCount = interactionReadyState.ReadyAssetCount,
            CompletedSteps = interactionReadyState.CompletedSteps,
            TotalSteps = interactionReadyState.TotalSteps,
            Exists = interactionReadyState.Exists,
            ReadSucceeded = interactionReadyState.ReadSucceeded,
            BrowserInteractionReadyState = interactionReadyState
        };

        if (!interactionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser focus session blocked for profile '{interactionReadyState.ProfileId}'.";
            result.Error = interactionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFocusSessionVersion = "runtime-browser-focus-session-v1";
        result.BrowserFocusStages =
        [
            "open-browser-focus-session",
            "bind-browser-interaction-ready-state",
            "publish-browser-focus-ready"
        ];
        result.BrowserFocusSummary = $"Runtime browser focus session prepared {result.BrowserFocusStages.Length} focus stage(s) for profile '{interactionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser focus session ready for profile '{interactionReadyState.ProfileId}' with {result.BrowserFocusStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFocusSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFocusSessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFocusStages { get; set; } = Array.Empty<string>();
    public string BrowserFocusSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserInteractionReadyStateResult BrowserInteractionReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
