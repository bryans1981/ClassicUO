namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConsistencySession
{
    ValueTask<BrowserClientRuntimeBrowserConsistencySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConsistencySessionService : IBrowserClientRuntimeBrowserConsistencySession
{
    private readonly IBrowserClientRuntimeBrowserIntuitivenessReadyState _runtimeBrowserIntuitivenessReadyState;

    public BrowserClientRuntimeBrowserConsistencySessionService(IBrowserClientRuntimeBrowserIntuitivenessReadyState runtimeBrowserIntuitivenessReadyState)
    {
        _runtimeBrowserIntuitivenessReadyState = runtimeBrowserIntuitivenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConsistencySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserIntuitivenessReadyStateResult intuitivenessReadyState = await _runtimeBrowserIntuitivenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserConsistencySessionResult result = new()
        {
            ProfileId = intuitivenessReadyState.ProfileId,
            SessionId = intuitivenessReadyState.SessionId,
            SessionPath = intuitivenessReadyState.SessionPath,
            BrowserIntuitivenessReadyStateVersion = intuitivenessReadyState.BrowserIntuitivenessReadyStateVersion,
            BrowserIntuitivenessSessionVersion = intuitivenessReadyState.BrowserIntuitivenessSessionVersion,
            LaunchMode = intuitivenessReadyState.LaunchMode,
            AssetRootPath = intuitivenessReadyState.AssetRootPath,
            ProfilesRootPath = intuitivenessReadyState.ProfilesRootPath,
            CacheRootPath = intuitivenessReadyState.CacheRootPath,
            ConfigRootPath = intuitivenessReadyState.ConfigRootPath,
            SettingsFilePath = intuitivenessReadyState.SettingsFilePath,
            StartupProfilePath = intuitivenessReadyState.StartupProfilePath,
            RequiredAssets = intuitivenessReadyState.RequiredAssets,
            ReadyAssetCount = intuitivenessReadyState.ReadyAssetCount,
            CompletedSteps = intuitivenessReadyState.CompletedSteps,
            TotalSteps = intuitivenessReadyState.TotalSteps,
            Exists = intuitivenessReadyState.Exists,
            ReadSucceeded = intuitivenessReadyState.ReadSucceeded
        };

        if (!intuitivenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser consistency session blocked for profile '{intuitivenessReadyState.ProfileId}'.";
            result.Error = intuitivenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConsistencySessionVersion = "runtime-browser-consistency-session-v1";
        result.BrowserConsistencyStages =
        [
            "open-browser-consistency-session",
            "bind-browser-intuitiveness-ready-state",
            "publish-browser-consistency-ready"
        ];
        result.BrowserConsistencySummary = $"Runtime browser consistency session prepared {result.BrowserConsistencyStages.Length} consistency stage(s) for profile '{intuitivenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser consistency session ready for profile '{intuitivenessReadyState.ProfileId}' with {result.BrowserConsistencyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConsistencySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserConsistencySessionVersion { get; set; } = string.Empty;
    public string BrowserIntuitivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserIntuitivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConsistencyStages { get; set; } = Array.Empty<string>();
    public string BrowserConsistencySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

