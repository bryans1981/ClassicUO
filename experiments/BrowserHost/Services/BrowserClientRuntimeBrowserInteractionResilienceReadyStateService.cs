namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInteractionResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionResilienceReadyStateService : IBrowserClientRuntimeBrowserInteractionResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserInteractionResilienceSession _runtimeBrowserInteractionResilienceSession;

    public BrowserClientRuntimeBrowserInteractionResilienceReadyStateService(IBrowserClientRuntimeBrowserInteractionResilienceSession runtimeBrowserInteractionResilienceSession)
    {
        _runtimeBrowserInteractionResilienceSession = runtimeBrowserInteractionResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionResilienceSessionResult interactionresilienceSession = await _runtimeBrowserInteractionResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserInteractionResilienceReadyStateResult result = new()
        {
            ProfileId = interactionresilienceSession.ProfileId,
            SessionId = interactionresilienceSession.SessionId,
            SessionPath = interactionresilienceSession.SessionPath,
            BrowserInteractionResilienceSessionVersion = interactionresilienceSession.BrowserInteractionResilienceSessionVersion,
            BrowserStateResilienceReadyStateVersion = interactionresilienceSession.BrowserStateResilienceReadyStateVersion,
            BrowserStateResilienceSessionVersion = interactionresilienceSession.BrowserStateResilienceSessionVersion,
            LaunchMode = interactionresilienceSession.LaunchMode,
            AssetRootPath = interactionresilienceSession.AssetRootPath,
            ProfilesRootPath = interactionresilienceSession.ProfilesRootPath,
            CacheRootPath = interactionresilienceSession.CacheRootPath,
            ConfigRootPath = interactionresilienceSession.ConfigRootPath,
            SettingsFilePath = interactionresilienceSession.SettingsFilePath,
            StartupProfilePath = interactionresilienceSession.StartupProfilePath,
            RequiredAssets = interactionresilienceSession.RequiredAssets,
            ReadyAssetCount = interactionresilienceSession.ReadyAssetCount,
            CompletedSteps = interactionresilienceSession.CompletedSteps,
            TotalSteps = interactionresilienceSession.TotalSteps,
            Exists = interactionresilienceSession.Exists,
            ReadSucceeded = interactionresilienceSession.ReadSucceeded
        };

        if (!interactionresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser interactionresilience ready state blocked for profile '{interactionresilienceSession.ProfileId}'.";
            result.Error = interactionresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionResilienceReadyStateVersion = "runtime-browser-interactionresilience-ready-state-v1";
        result.BrowserInteractionResilienceReadyChecks =
        [
            "browser-stateresilience-ready-state-ready",
            "browser-interactionresilience-session-ready",
            "browser-interactionresilience-ready"
        ];
        result.BrowserInteractionResilienceReadySummary = $"Runtime browser interactionresilience ready state passed {result.BrowserInteractionResilienceReadyChecks.Length} interactionresilience readiness check(s) for profile '{interactionresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interactionresilience ready state ready for profile '{interactionresilienceSession.ProfileId}' with {result.BrowserInteractionResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserStateResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInteractionResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
