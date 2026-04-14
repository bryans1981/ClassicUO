namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserFlowResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowResilienceSessionService : IBrowserClientRuntimeBrowserFlowResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserInteractionResilienceReadyState _runtimeBrowserInteractionResilienceReadyState;

    public BrowserClientRuntimeBrowserFlowResilienceSessionService(IBrowserClientRuntimeBrowserInteractionResilienceReadyState runtimeBrowserInteractionResilienceReadyState)
    {
        _runtimeBrowserInteractionResilienceReadyState = runtimeBrowserInteractionResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionResilienceReadyStateResult prevReadyState = await _runtimeBrowserInteractionResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFlowResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserInteractionResilienceReadyStateVersion = prevReadyState.BrowserInteractionResilienceReadyStateVersion,
            BrowserInteractionResilienceSessionVersion = prevReadyState.BrowserInteractionResilienceSessionVersion,
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
            result.Summary = $"Runtime browser flowresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowResilienceSessionVersion = "runtime-browser-flowresilience-session-v1";
        result.BrowserFlowResilienceStages =
        [
            "open-browser-flowresilience-session",
            "bind-browser-interactionresilience-ready-state",
            "publish-browser-flowresilience-ready"
        ];
        result.BrowserFlowResilienceSummary = $"Runtime browser flowresilience session prepared {result.BrowserFlowResilienceStages.Length} flowresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserFlowResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowResilienceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserFlowResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserFlowResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
