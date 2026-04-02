namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMasterySession
{
    ValueTask<BrowserClientRuntimeBrowserMasterySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMasterySessionService : IBrowserClientRuntimeBrowserMasterySession
{
    private readonly IBrowserClientRuntimeBrowserAgencyReadyState _runtimeBrowserAgencyReadyState;

    public BrowserClientRuntimeBrowserMasterySessionService(IBrowserClientRuntimeBrowserAgencyReadyState runtimeBrowserAgencyReadyState)
    {
        _runtimeBrowserAgencyReadyState = runtimeBrowserAgencyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMasterySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAgencyReadyStateResult agencyReadyState = await _runtimeBrowserAgencyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserMasterySessionResult result = new()
        {
            ProfileId = agencyReadyState.ProfileId,
            SessionId = agencyReadyState.SessionId,
            SessionPath = agencyReadyState.SessionPath,
            BrowserAgencyReadyStateVersion = agencyReadyState.BrowserAgencyReadyStateVersion,
            BrowserAgencySessionVersion = agencyReadyState.BrowserAgencySessionVersion,
            LaunchMode = agencyReadyState.LaunchMode,
            AssetRootPath = agencyReadyState.AssetRootPath,
            ProfilesRootPath = agencyReadyState.ProfilesRootPath,
            CacheRootPath = agencyReadyState.CacheRootPath,
            ConfigRootPath = agencyReadyState.ConfigRootPath,
            SettingsFilePath = agencyReadyState.SettingsFilePath,
            StartupProfilePath = agencyReadyState.StartupProfilePath,
            RequiredAssets = agencyReadyState.RequiredAssets,
            ReadyAssetCount = agencyReadyState.ReadyAssetCount,
            CompletedSteps = agencyReadyState.CompletedSteps,
            TotalSteps = agencyReadyState.TotalSteps,
            Exists = agencyReadyState.Exists,
            ReadSucceeded = agencyReadyState.ReadSucceeded
        };

        if (!agencyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser mastery session blocked for profile '{agencyReadyState.ProfileId}'.";
            result.Error = agencyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMasterySessionVersion = "runtime-browser-mastery-session-v1";
        result.BrowserMasteryStages =
        [
            "open-browser-mastery-session",
            "bind-browser-agency-ready-state",
            "publish-browser-mastery-ready"
        ];
        result.BrowserMasterySummary = $"Runtime browser mastery session prepared {result.BrowserMasteryStages.Length} mastery stage(s) for profile '{agencyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser mastery session ready for profile '{agencyReadyState.ProfileId}' with {result.BrowserMasteryStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMasterySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserMasterySessionVersion { get; set; } = string.Empty;
    public string BrowserAgencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAgencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMasteryStages { get; set; } = Array.Empty<string>();
    public string BrowserMasterySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
