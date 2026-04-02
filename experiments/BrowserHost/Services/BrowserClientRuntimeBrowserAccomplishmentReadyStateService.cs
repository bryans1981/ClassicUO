namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAccomplishmentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAccomplishmentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAccomplishmentReadyStateService : IBrowserClientRuntimeBrowserAccomplishmentReadyState
{
    private readonly IBrowserClientRuntimeBrowserAccomplishmentSession _runtimeBrowserAccomplishmentSession;

    public BrowserClientRuntimeBrowserAccomplishmentReadyStateService(IBrowserClientRuntimeBrowserAccomplishmentSession runtimeBrowserAccomplishmentSession)
    {
        _runtimeBrowserAccomplishmentSession = runtimeBrowserAccomplishmentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAccomplishmentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAccomplishmentSessionResult accomplishmentSession = await _runtimeBrowserAccomplishmentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAccomplishmentReadyStateResult result = new()
        {
            ProfileId = accomplishmentSession.ProfileId,
            SessionId = accomplishmentSession.SessionId,
            SessionPath = accomplishmentSession.SessionPath,
            BrowserAccomplishmentSessionVersion = accomplishmentSession.BrowserAccomplishmentSessionVersion,
            BrowserAttainmentReadyStateVersion = accomplishmentSession.BrowserAttainmentReadyStateVersion,
            BrowserAttainmentSessionVersion = accomplishmentSession.BrowserAttainmentSessionVersion,
            LaunchMode = accomplishmentSession.LaunchMode,
            AssetRootPath = accomplishmentSession.AssetRootPath,
            ProfilesRootPath = accomplishmentSession.ProfilesRootPath,
            CacheRootPath = accomplishmentSession.CacheRootPath,
            ConfigRootPath = accomplishmentSession.ConfigRootPath,
            SettingsFilePath = accomplishmentSession.SettingsFilePath,
            StartupProfilePath = accomplishmentSession.StartupProfilePath,
            RequiredAssets = accomplishmentSession.RequiredAssets,
            ReadyAssetCount = accomplishmentSession.ReadyAssetCount,
            CompletedSteps = accomplishmentSession.CompletedSteps,
            TotalSteps = accomplishmentSession.TotalSteps,
            Exists = accomplishmentSession.Exists,
            ReadSucceeded = accomplishmentSession.ReadSucceeded
        };

        if (!accomplishmentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser accomplishment ready state blocked for profile '{accomplishmentSession.ProfileId}'.";
            result.Error = accomplishmentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAccomplishmentReadyStateVersion = "runtime-browser-accomplishment-ready-state-v1";
        result.BrowserAccomplishmentReadyChecks =
        [
            "browser-attainment-ready-state-ready",
            "browser-accomplishment-session-ready",
            "browser-accomplishment-ready"
        ];
        result.BrowserAccomplishmentReadySummary = $"Runtime browser accomplishment ready state passed {result.BrowserAccomplishmentReadyChecks.Length} accomplishment readiness check(s) for profile '{accomplishmentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser accomplishment ready state ready for profile '{accomplishmentSession.ProfileId}' with {result.BrowserAccomplishmentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAccomplishmentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAccomplishmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAccomplishmentSessionVersion { get; set; } = string.Empty;
    public string BrowserAttainmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAttainmentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAccomplishmentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAccomplishmentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
