namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFulfillmentSession
{
    ValueTask<BrowserClientRuntimeBrowserFulfillmentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFulfillmentSessionService : IBrowserClientRuntimeBrowserFulfillmentSession
{
    private readonly IBrowserClientRuntimeBrowserFollowThroughReadyState _runtimeBrowserFollowThroughReadyState;

    public BrowserClientRuntimeBrowserFulfillmentSessionService(IBrowserClientRuntimeBrowserFollowThroughReadyState runtimeBrowserFollowThroughReadyState)
    {
        _runtimeBrowserFollowThroughReadyState = runtimeBrowserFollowThroughReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFulfillmentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFollowThroughReadyStateResult followthroughReadyState = await _runtimeBrowserFollowThroughReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFulfillmentSessionResult result = new()
        {
            ProfileId = followthroughReadyState.ProfileId,
            SessionId = followthroughReadyState.SessionId,
            SessionPath = followthroughReadyState.SessionPath,
            BrowserFollowThroughReadyStateVersion = followthroughReadyState.BrowserFollowThroughReadyStateVersion,
            BrowserFollowThroughSessionVersion = followthroughReadyState.BrowserFollowThroughSessionVersion,
            LaunchMode = followthroughReadyState.LaunchMode,
            AssetRootPath = followthroughReadyState.AssetRootPath,
            ProfilesRootPath = followthroughReadyState.ProfilesRootPath,
            CacheRootPath = followthroughReadyState.CacheRootPath,
            ConfigRootPath = followthroughReadyState.ConfigRootPath,
            SettingsFilePath = followthroughReadyState.SettingsFilePath,
            StartupProfilePath = followthroughReadyState.StartupProfilePath,
            RequiredAssets = followthroughReadyState.RequiredAssets,
            ReadyAssetCount = followthroughReadyState.ReadyAssetCount,
            CompletedSteps = followthroughReadyState.CompletedSteps,
            TotalSteps = followthroughReadyState.TotalSteps,
            Exists = followthroughReadyState.Exists,
            ReadSucceeded = followthroughReadyState.ReadSucceeded
        };

        if (!followthroughReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser fulfillment session blocked for profile '{followthroughReadyState.ProfileId}'.";
            result.Error = followthroughReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFulfillmentSessionVersion = "runtime-browser-fulfillment-session-v1";
        result.BrowserFulfillmentStages =
        [
            "open-browser-fulfillment-session",
            "bind-browser-followthrough-ready-state",
            "publish-browser-fulfillment-ready"
        ];
        result.BrowserFulfillmentSummary = $"Runtime browser fulfillment session prepared {result.BrowserFulfillmentStages.Length} fulfillment stage(s) for profile '{followthroughReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser fulfillment session ready for profile '{followthroughReadyState.ProfileId}' with {result.BrowserFulfillmentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFulfillmentSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFulfillmentSessionVersion { get; set; } = string.Empty;
    public string BrowserFollowThroughReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFollowThroughSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFulfillmentStages { get; set; } = Array.Empty<string>();
    public string BrowserFulfillmentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
