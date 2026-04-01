namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionSession
{
    ValueTask<BrowserClientRuntimeBrowserInteractionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionSessionService : IBrowserClientRuntimeBrowserInteractionSession
{
    private readonly IBrowserClientRuntimeBrowserEventReadyState _runtimeBrowserEventReadyState;

    public BrowserClientRuntimeBrowserInteractionSessionService(IBrowserClientRuntimeBrowserEventReadyState runtimeBrowserEventReadyState)
    {
        _runtimeBrowserEventReadyState = runtimeBrowserEventReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEventReadyStateResult eventReadyState = await _runtimeBrowserEventReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInteractionSessionResult result = new()
        {
            ProfileId = eventReadyState.ProfileId,
            SessionId = eventReadyState.SessionId,
            SessionPath = eventReadyState.SessionPath,
            BrowserEventReadyStateVersion = eventReadyState.BrowserEventReadyStateVersion,
            BrowserEventSessionVersion = eventReadyState.BrowserEventSessionVersion,
            LaunchMode = eventReadyState.LaunchMode,
            AssetRootPath = eventReadyState.AssetRootPath,
            ProfilesRootPath = eventReadyState.ProfilesRootPath,
            CacheRootPath = eventReadyState.CacheRootPath,
            ConfigRootPath = eventReadyState.ConfigRootPath,
            SettingsFilePath = eventReadyState.SettingsFilePath,
            StartupProfilePath = eventReadyState.StartupProfilePath,
            RequiredAssets = eventReadyState.RequiredAssets,
            ReadyAssetCount = eventReadyState.ReadyAssetCount,
            CompletedSteps = eventReadyState.CompletedSteps,
            TotalSteps = eventReadyState.TotalSteps,
            Exists = eventReadyState.Exists,
            ReadSucceeded = eventReadyState.ReadSucceeded,
            BrowserEventReadyState = eventReadyState
        };

        if (!eventReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser interaction session blocked for profile '{eventReadyState.ProfileId}'.";
            result.Error = eventReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionSessionVersion = "runtime-browser-interaction-session-v1";
        result.BrowserInteractionStages =
        [
            "open-browser-interaction-session",
            "bind-browser-event-ready-state",
            "publish-browser-interaction-ready"
        ];
        result.BrowserInteractionSummary = $"Runtime browser interaction session prepared {result.BrowserInteractionStages.Length} interaction stage(s) for profile '{eventReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interaction session ready for profile '{eventReadyState.ProfileId}' with {result.BrowserInteractionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionSessionVersion { get; set; } = string.Empty;
    public string BrowserEventReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEventSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionStages { get; set; } = Array.Empty<string>();
    public string BrowserInteractionSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserEventReadyStateResult BrowserEventReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
