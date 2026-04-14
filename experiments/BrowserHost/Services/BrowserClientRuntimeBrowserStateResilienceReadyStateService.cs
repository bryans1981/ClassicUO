namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserStateResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateResilienceReadyStateService : IBrowserClientRuntimeBrowserStateResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserStateResilienceSession _runtimeBrowserStateResilienceSession;

    public BrowserClientRuntimeBrowserStateResilienceReadyStateService(IBrowserClientRuntimeBrowserStateResilienceSession runtimeBrowserStateResilienceSession)
    {
        _runtimeBrowserStateResilienceSession = runtimeBrowserStateResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateResilienceSessionResult stateresilienceSession = await _runtimeBrowserStateResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserStateResilienceReadyStateResult result = new()
        {
            ProfileId = stateresilienceSession.ProfileId,
            SessionId = stateresilienceSession.SessionId,
            SessionPath = stateresilienceSession.SessionPath,
            BrowserStateResilienceSessionVersion = stateresilienceSession.BrowserStateResilienceSessionVersion,
            BrowserSessionResilienceReadyStateVersion = stateresilienceSession.BrowserSessionResilienceReadyStateVersion,
            BrowserSessionResilienceSessionVersion = stateresilienceSession.BrowserSessionResilienceSessionVersion,
            LaunchMode = stateresilienceSession.LaunchMode,
            AssetRootPath = stateresilienceSession.AssetRootPath,
            ProfilesRootPath = stateresilienceSession.ProfilesRootPath,
            CacheRootPath = stateresilienceSession.CacheRootPath,
            ConfigRootPath = stateresilienceSession.ConfigRootPath,
            SettingsFilePath = stateresilienceSession.SettingsFilePath,
            StartupProfilePath = stateresilienceSession.StartupProfilePath,
            RequiredAssets = stateresilienceSession.RequiredAssets,
            ReadyAssetCount = stateresilienceSession.ReadyAssetCount,
            CompletedSteps = stateresilienceSession.CompletedSteps,
            TotalSteps = stateresilienceSession.TotalSteps,
            Exists = stateresilienceSession.Exists,
            ReadSucceeded = stateresilienceSession.ReadSucceeded
        };

        if (!stateresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser stateresilience ready state blocked for profile '{stateresilienceSession.ProfileId}'.";
            result.Error = stateresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateResilienceReadyStateVersion = "runtime-browser-stateresilience-ready-state-v1";
        result.BrowserStateResilienceReadyChecks =
        [
            "browser-sessionresilience-ready-state-ready",
            "browser-stateresilience-session-ready",
            "browser-stateresilience-ready"
        ];
        result.BrowserStateResilienceReadySummary = $"Runtime browser stateresilience ready state passed {result.BrowserStateResilienceReadyChecks.Length} stateresilience readiness check(s) for profile '{stateresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser stateresilience ready state ready for profile '{stateresilienceSession.ProfileId}' with {result.BrowserStateResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserStateResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserSessionResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserStateResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
