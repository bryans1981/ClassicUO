namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoalOrientationSession
{
    ValueTask<BrowserClientRuntimeBrowserGoalOrientationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoalOrientationSessionService : IBrowserClientRuntimeBrowserGoalOrientationSession
{
    private readonly IBrowserClientRuntimeBrowserTaskAlignmentReadyState _runtimeBrowserTaskAlignmentReadyState;

    public BrowserClientRuntimeBrowserGoalOrientationSessionService(IBrowserClientRuntimeBrowserTaskAlignmentReadyState runtimeBrowserTaskAlignmentReadyState)
    {
        _runtimeBrowserTaskAlignmentReadyState = runtimeBrowserTaskAlignmentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoalOrientationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTaskAlignmentReadyStateResult taskalignmentReadyState = await _runtimeBrowserTaskAlignmentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGoalOrientationSessionResult result = new()
        {
            ProfileId = taskalignmentReadyState.ProfileId,
            SessionId = taskalignmentReadyState.SessionId,
            SessionPath = taskalignmentReadyState.SessionPath,
            BrowserTaskAlignmentReadyStateVersion = taskalignmentReadyState.BrowserTaskAlignmentReadyStateVersion,
            BrowserTaskAlignmentSessionVersion = taskalignmentReadyState.BrowserTaskAlignmentSessionVersion,
            LaunchMode = taskalignmentReadyState.LaunchMode,
            AssetRootPath = taskalignmentReadyState.AssetRootPath,
            ProfilesRootPath = taskalignmentReadyState.ProfilesRootPath,
            CacheRootPath = taskalignmentReadyState.CacheRootPath,
            ConfigRootPath = taskalignmentReadyState.ConfigRootPath,
            SettingsFilePath = taskalignmentReadyState.SettingsFilePath,
            StartupProfilePath = taskalignmentReadyState.StartupProfilePath,
            RequiredAssets = taskalignmentReadyState.RequiredAssets,
            ReadyAssetCount = taskalignmentReadyState.ReadyAssetCount,
            CompletedSteps = taskalignmentReadyState.CompletedSteps,
            TotalSteps = taskalignmentReadyState.TotalSteps,
            Exists = taskalignmentReadyState.Exists,
            ReadSucceeded = taskalignmentReadyState.ReadSucceeded
        };

        if (!taskalignmentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser goalorientation session blocked for profile '{taskalignmentReadyState.ProfileId}'.";
            result.Error = taskalignmentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoalOrientationSessionVersion = "runtime-browser-goalorientation-session-v1";
        result.BrowserGoalOrientationStages =
        [
            "open-browser-goalorientation-session",
            "bind-browser-taskalignment-ready-state",
            "publish-browser-goalorientation-ready"
        ];
        result.BrowserGoalOrientationSummary = $"Runtime browser goalorientation session prepared {result.BrowserGoalOrientationStages.Length} goalorientation stage(s) for profile '{taskalignmentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser goalorientation session ready for profile '{taskalignmentReadyState.ProfileId}' with {result.BrowserGoalOrientationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoalOrientationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserGoalOrientationSessionVersion { get; set; } = string.Empty;
    public string BrowserTaskAlignmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskAlignmentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoalOrientationStages { get; set; } = Array.Empty<string>();
    public string BrowserGoalOrientationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
