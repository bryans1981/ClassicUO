namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFlowResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowResilienceReadyStateService : IBrowserClientRuntimeBrowserFlowResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserFlowResilienceSession _runtimeBrowserFlowResilienceSession;

    public BrowserClientRuntimeBrowserFlowResilienceReadyStateService(IBrowserClientRuntimeBrowserFlowResilienceSession runtimeBrowserFlowResilienceSession)
    {
        _runtimeBrowserFlowResilienceSession = runtimeBrowserFlowResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowResilienceSessionResult flowresilienceSession = await _runtimeBrowserFlowResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFlowResilienceReadyStateResult result = new()
        {
            ProfileId = flowresilienceSession.ProfileId,
            SessionId = flowresilienceSession.SessionId,
            SessionPath = flowresilienceSession.SessionPath,
            BrowserFlowResilienceSessionVersion = flowresilienceSession.BrowserFlowResilienceSessionVersion,
            BrowserInteractionResilienceReadyStateVersion = flowresilienceSession.BrowserInteractionResilienceReadyStateVersion,
            BrowserInteractionResilienceSessionVersion = flowresilienceSession.BrowserInteractionResilienceSessionVersion,
            LaunchMode = flowresilienceSession.LaunchMode,
            AssetRootPath = flowresilienceSession.AssetRootPath,
            ProfilesRootPath = flowresilienceSession.ProfilesRootPath,
            CacheRootPath = flowresilienceSession.CacheRootPath,
            ConfigRootPath = flowresilienceSession.ConfigRootPath,
            SettingsFilePath = flowresilienceSession.SettingsFilePath,
            StartupProfilePath = flowresilienceSession.StartupProfilePath,
            RequiredAssets = flowresilienceSession.RequiredAssets,
            ReadyAssetCount = flowresilienceSession.ReadyAssetCount,
            CompletedSteps = flowresilienceSession.CompletedSteps,
            TotalSteps = flowresilienceSession.TotalSteps,
            Exists = flowresilienceSession.Exists,
            ReadSucceeded = flowresilienceSession.ReadSucceeded
        };

        if (!flowresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser flowresilience ready state blocked for profile '{flowresilienceSession.ProfileId}'.";
            result.Error = flowresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowResilienceReadyStateVersion = "runtime-browser-flowresilience-ready-state-v1";
        result.BrowserFlowResilienceReadyChecks =
        [
            "browser-interactionresilience-ready-state-ready",
            "browser-flowresilience-session-ready",
            "browser-flowresilience-ready"
        ];
        result.BrowserFlowResilienceReadySummary = $"Runtime browser flowresilience ready state passed {result.BrowserFlowResilienceReadyChecks.Length} flowresilience readiness check(s) for profile '{flowresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowresilience ready state ready for profile '{flowresilienceSession.ProfileId}' with {result.BrowserFlowResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFlowResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlowResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFlowResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
