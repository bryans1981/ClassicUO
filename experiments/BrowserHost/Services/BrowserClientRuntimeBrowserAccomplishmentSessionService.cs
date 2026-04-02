namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAccomplishmentSession
{
    ValueTask<BrowserClientRuntimeBrowserAccomplishmentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAccomplishmentSessionService : IBrowserClientRuntimeBrowserAccomplishmentSession
{
    private readonly IBrowserClientRuntimeBrowserAttainmentReadyState _runtimeBrowserAttainmentReadyState;

    public BrowserClientRuntimeBrowserAccomplishmentSessionService(IBrowserClientRuntimeBrowserAttainmentReadyState runtimeBrowserAttainmentReadyState)
    {
        _runtimeBrowserAttainmentReadyState = runtimeBrowserAttainmentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAccomplishmentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAttainmentReadyStateResult attainmentReadyState = await _runtimeBrowserAttainmentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAccomplishmentSessionResult result = new()
        {
            ProfileId = attainmentReadyState.ProfileId,
            SessionId = attainmentReadyState.SessionId,
            SessionPath = attainmentReadyState.SessionPath,
            BrowserAttainmentReadyStateVersion = attainmentReadyState.BrowserAttainmentReadyStateVersion,
            BrowserAttainmentSessionVersion = attainmentReadyState.BrowserAttainmentSessionVersion,
            LaunchMode = attainmentReadyState.LaunchMode,
            AssetRootPath = attainmentReadyState.AssetRootPath,
            ProfilesRootPath = attainmentReadyState.ProfilesRootPath,
            CacheRootPath = attainmentReadyState.CacheRootPath,
            ConfigRootPath = attainmentReadyState.ConfigRootPath,
            SettingsFilePath = attainmentReadyState.SettingsFilePath,
            StartupProfilePath = attainmentReadyState.StartupProfilePath,
            RequiredAssets = attainmentReadyState.RequiredAssets,
            ReadyAssetCount = attainmentReadyState.ReadyAssetCount,
            CompletedSteps = attainmentReadyState.CompletedSteps,
            TotalSteps = attainmentReadyState.TotalSteps,
            Exists = attainmentReadyState.Exists,
            ReadSucceeded = attainmentReadyState.ReadSucceeded
        };

        if (!attainmentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser accomplishment session blocked for profile '{attainmentReadyState.ProfileId}'.";
            result.Error = attainmentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAccomplishmentSessionVersion = "runtime-browser-accomplishment-session-v1";
        result.BrowserAccomplishmentStages =
        [
            "open-browser-accomplishment-session",
            "bind-browser-attainment-ready-state",
            "publish-browser-accomplishment-ready"
        ];
        result.BrowserAccomplishmentSummary = $"Runtime browser accomplishment session prepared {result.BrowserAccomplishmentStages.Length} accomplishment stage(s) for profile '{attainmentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser accomplishment session ready for profile '{attainmentReadyState.ProfileId}' with {result.BrowserAccomplishmentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAccomplishmentSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserAccomplishmentStages { get; set; } = Array.Empty<string>();
    public string BrowserAccomplishmentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
