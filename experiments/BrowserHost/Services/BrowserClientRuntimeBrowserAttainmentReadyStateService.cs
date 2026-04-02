namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAttainmentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAttainmentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAttainmentReadyStateService : IBrowserClientRuntimeBrowserAttainmentReadyState
{
    private readonly IBrowserClientRuntimeBrowserAttainmentSession _runtimeBrowserAttainmentSession;

    public BrowserClientRuntimeBrowserAttainmentReadyStateService(IBrowserClientRuntimeBrowserAttainmentSession runtimeBrowserAttainmentSession)
    {
        _runtimeBrowserAttainmentSession = runtimeBrowserAttainmentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAttainmentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAttainmentSessionResult attainmentSession = await _runtimeBrowserAttainmentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAttainmentReadyStateResult result = new()
        {
            ProfileId = attainmentSession.ProfileId,
            SessionId = attainmentSession.SessionId,
            SessionPath = attainmentSession.SessionPath,
            BrowserAttainmentSessionVersion = attainmentSession.BrowserAttainmentSessionVersion,
            BrowserFulfillmentReadyStateVersion = attainmentSession.BrowserFulfillmentReadyStateVersion,
            BrowserFulfillmentSessionVersion = attainmentSession.BrowserFulfillmentSessionVersion,
            LaunchMode = attainmentSession.LaunchMode,
            AssetRootPath = attainmentSession.AssetRootPath,
            ProfilesRootPath = attainmentSession.ProfilesRootPath,
            CacheRootPath = attainmentSession.CacheRootPath,
            ConfigRootPath = attainmentSession.ConfigRootPath,
            SettingsFilePath = attainmentSession.SettingsFilePath,
            StartupProfilePath = attainmentSession.StartupProfilePath,
            RequiredAssets = attainmentSession.RequiredAssets,
            ReadyAssetCount = attainmentSession.ReadyAssetCount,
            CompletedSteps = attainmentSession.CompletedSteps,
            TotalSteps = attainmentSession.TotalSteps,
            Exists = attainmentSession.Exists,
            ReadSucceeded = attainmentSession.ReadSucceeded
        };

        if (!attainmentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser attainment ready state blocked for profile '{attainmentSession.ProfileId}'.";
            result.Error = attainmentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAttainmentReadyStateVersion = "runtime-browser-attainment-ready-state-v1";
        result.BrowserAttainmentReadyChecks =
        [
            "browser-fulfillment-ready-state-ready",
            "browser-attainment-session-ready",
            "browser-attainment-ready"
        ];
        result.BrowserAttainmentReadySummary = $"Runtime browser attainment ready state passed {result.BrowserAttainmentReadyChecks.Length} attainment readiness check(s) for profile '{attainmentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser attainment ready state ready for profile '{attainmentSession.ProfileId}' with {result.BrowserAttainmentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAttainmentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAttainmentReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserAttainmentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAttainmentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
