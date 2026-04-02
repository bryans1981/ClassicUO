namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAttainmentSession
{
    ValueTask<BrowserClientRuntimeBrowserAttainmentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAttainmentSessionService : IBrowserClientRuntimeBrowserAttainmentSession
{
    private readonly IBrowserClientRuntimeBrowserFulfillmentReadyState _runtimeBrowserFulfillmentReadyState;

    public BrowserClientRuntimeBrowserAttainmentSessionService(IBrowserClientRuntimeBrowserFulfillmentReadyState runtimeBrowserFulfillmentReadyState)
    {
        _runtimeBrowserFulfillmentReadyState = runtimeBrowserFulfillmentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAttainmentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFulfillmentReadyStateResult fulfillmentReadyState = await _runtimeBrowserFulfillmentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAttainmentSessionResult result = new()
        {
            ProfileId = fulfillmentReadyState.ProfileId,
            SessionId = fulfillmentReadyState.SessionId,
            SessionPath = fulfillmentReadyState.SessionPath,
            BrowserFulfillmentReadyStateVersion = fulfillmentReadyState.BrowserFulfillmentReadyStateVersion,
            BrowserFulfillmentSessionVersion = fulfillmentReadyState.BrowserFulfillmentSessionVersion,
            LaunchMode = fulfillmentReadyState.LaunchMode,
            AssetRootPath = fulfillmentReadyState.AssetRootPath,
            ProfilesRootPath = fulfillmentReadyState.ProfilesRootPath,
            CacheRootPath = fulfillmentReadyState.CacheRootPath,
            ConfigRootPath = fulfillmentReadyState.ConfigRootPath,
            SettingsFilePath = fulfillmentReadyState.SettingsFilePath,
            StartupProfilePath = fulfillmentReadyState.StartupProfilePath,
            RequiredAssets = fulfillmentReadyState.RequiredAssets,
            ReadyAssetCount = fulfillmentReadyState.ReadyAssetCount,
            CompletedSteps = fulfillmentReadyState.CompletedSteps,
            TotalSteps = fulfillmentReadyState.TotalSteps,
            Exists = fulfillmentReadyState.Exists,
            ReadSucceeded = fulfillmentReadyState.ReadSucceeded
        };

        if (!fulfillmentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser attainment session blocked for profile '{fulfillmentReadyState.ProfileId}'.";
            result.Error = fulfillmentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAttainmentSessionVersion = "runtime-browser-attainment-session-v1";
        result.BrowserAttainmentStages =
        [
            "open-browser-attainment-session",
            "bind-browser-fulfillment-ready-state",
            "publish-browser-attainment-ready"
        ];
        result.BrowserAttainmentSummary = $"Runtime browser attainment session prepared {result.BrowserAttainmentStages.Length} attainment stage(s) for profile '{fulfillmentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser attainment session ready for profile '{fulfillmentReadyState.ProfileId}' with {result.BrowserAttainmentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAttainmentSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAttainmentSessionVersion { get; set; } = string.Empty;
    public string BrowserFulfillmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFulfillmentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAttainmentStages { get; set; } = Array.Empty<string>();
    public string BrowserAttainmentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
