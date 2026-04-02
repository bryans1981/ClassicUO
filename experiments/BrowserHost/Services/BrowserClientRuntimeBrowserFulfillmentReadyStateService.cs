namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFulfillmentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFulfillmentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFulfillmentReadyStateService : IBrowserClientRuntimeBrowserFulfillmentReadyState
{
    private readonly IBrowserClientRuntimeBrowserFulfillmentSession _runtimeBrowserFulfillmentSession;

    public BrowserClientRuntimeBrowserFulfillmentReadyStateService(IBrowserClientRuntimeBrowserFulfillmentSession runtimeBrowserFulfillmentSession)
    {
        _runtimeBrowserFulfillmentSession = runtimeBrowserFulfillmentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFulfillmentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFulfillmentSessionResult fulfillmentSession = await _runtimeBrowserFulfillmentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFulfillmentReadyStateResult result = new()
        {
            ProfileId = fulfillmentSession.ProfileId,
            SessionId = fulfillmentSession.SessionId,
            SessionPath = fulfillmentSession.SessionPath,
            BrowserFulfillmentSessionVersion = fulfillmentSession.BrowserFulfillmentSessionVersion,
            BrowserFollowThroughReadyStateVersion = fulfillmentSession.BrowserFollowThroughReadyStateVersion,
            BrowserFollowThroughSessionVersion = fulfillmentSession.BrowserFollowThroughSessionVersion,
            LaunchMode = fulfillmentSession.LaunchMode,
            AssetRootPath = fulfillmentSession.AssetRootPath,
            ProfilesRootPath = fulfillmentSession.ProfilesRootPath,
            CacheRootPath = fulfillmentSession.CacheRootPath,
            ConfigRootPath = fulfillmentSession.ConfigRootPath,
            SettingsFilePath = fulfillmentSession.SettingsFilePath,
            StartupProfilePath = fulfillmentSession.StartupProfilePath,
            RequiredAssets = fulfillmentSession.RequiredAssets,
            ReadyAssetCount = fulfillmentSession.ReadyAssetCount,
            CompletedSteps = fulfillmentSession.CompletedSteps,
            TotalSteps = fulfillmentSession.TotalSteps,
            Exists = fulfillmentSession.Exists,
            ReadSucceeded = fulfillmentSession.ReadSucceeded
        };

        if (!fulfillmentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser fulfillment ready state blocked for profile '{fulfillmentSession.ProfileId}'.";
            result.Error = fulfillmentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFulfillmentReadyStateVersion = "runtime-browser-fulfillment-ready-state-v1";
        result.BrowserFulfillmentReadyChecks =
        [
            "browser-followthrough-ready-state-ready",
            "browser-fulfillment-session-ready",
            "browser-fulfillment-ready"
        ];
        result.BrowserFulfillmentReadySummary = $"Runtime browser fulfillment ready state passed {result.BrowserFulfillmentReadyChecks.Length} fulfillment readiness check(s) for profile '{fulfillmentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser fulfillment ready state ready for profile '{fulfillmentSession.ProfileId}' with {result.BrowserFulfillmentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFulfillmentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFulfillmentReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserFulfillmentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFulfillmentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
