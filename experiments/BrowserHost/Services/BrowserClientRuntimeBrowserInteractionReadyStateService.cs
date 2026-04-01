namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInteractionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionReadyStateService : IBrowserClientRuntimeBrowserInteractionReadyState
{
    private readonly IBrowserClientRuntimeBrowserInteractionSession _runtimeBrowserInteractionSession;

    public BrowserClientRuntimeBrowserInteractionReadyStateService(IBrowserClientRuntimeBrowserInteractionSession runtimeBrowserInteractionSession)
    {
        _runtimeBrowserInteractionSession = runtimeBrowserInteractionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionSessionResult interactionSession = await _runtimeBrowserInteractionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserInteractionReadyStateResult result = new()
        {
            ProfileId = interactionSession.ProfileId,
            SessionId = interactionSession.SessionId,
            SessionPath = interactionSession.SessionPath,
            BrowserInteractionSessionVersion = interactionSession.BrowserInteractionSessionVersion,
            BrowserEventReadyStateVersion = interactionSession.BrowserEventReadyStateVersion,
            BrowserEventSessionVersion = interactionSession.BrowserEventSessionVersion,
            LaunchMode = interactionSession.LaunchMode,
            AssetRootPath = interactionSession.AssetRootPath,
            ProfilesRootPath = interactionSession.ProfilesRootPath,
            CacheRootPath = interactionSession.CacheRootPath,
            ConfigRootPath = interactionSession.ConfigRootPath,
            SettingsFilePath = interactionSession.SettingsFilePath,
            StartupProfilePath = interactionSession.StartupProfilePath,
            RequiredAssets = interactionSession.RequiredAssets,
            ReadyAssetCount = interactionSession.ReadyAssetCount,
            CompletedSteps = interactionSession.CompletedSteps,
            TotalSteps = interactionSession.TotalSteps,
            Exists = interactionSession.Exists,
            ReadSucceeded = interactionSession.ReadSucceeded,
            BrowserInteractionSession = interactionSession
        };

        if (!interactionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser interaction ready state blocked for profile '{interactionSession.ProfileId}'.";
            result.Error = interactionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionReadyStateVersion = "runtime-browser-interaction-ready-state-v1";
        result.BrowserInteractionReadyChecks =
        [
            "browser-event-ready-state-ready",
            "browser-interaction-session-ready",
            "browser-interaction-ready"
        ];
        result.BrowserInteractionReadySummary = $"Runtime browser interaction ready state passed {result.BrowserInteractionReadyChecks.Length} interaction readiness check(s) for profile '{interactionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interaction ready state ready for profile '{interactionSession.ProfileId}' with {result.BrowserInteractionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInteractionReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserInteractionSessionResult BrowserInteractionSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
