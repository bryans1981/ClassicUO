namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowStateSession
{
    ValueTask<BrowserClientRuntimeBrowserFlowStateSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowStateSessionService : IBrowserClientRuntimeBrowserFlowStateSession
{
    private readonly IBrowserClientRuntimeBrowserGoalOrientationReadyState _runtimeBrowserGoalOrientationReadyState;

    public BrowserClientRuntimeBrowserFlowStateSessionService(IBrowserClientRuntimeBrowserGoalOrientationReadyState runtimeBrowserGoalOrientationReadyState)
    {
        _runtimeBrowserGoalOrientationReadyState = runtimeBrowserGoalOrientationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowStateSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoalOrientationReadyStateResult goalorientationReadyState = await _runtimeBrowserGoalOrientationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFlowStateSessionResult result = new()
        {
            ProfileId = goalorientationReadyState.ProfileId,
            SessionId = goalorientationReadyState.SessionId,
            SessionPath = goalorientationReadyState.SessionPath,
            BrowserGoalOrientationReadyStateVersion = goalorientationReadyState.BrowserGoalOrientationReadyStateVersion,
            BrowserGoalOrientationSessionVersion = goalorientationReadyState.BrowserGoalOrientationSessionVersion,
            LaunchMode = goalorientationReadyState.LaunchMode,
            AssetRootPath = goalorientationReadyState.AssetRootPath,
            ProfilesRootPath = goalorientationReadyState.ProfilesRootPath,
            CacheRootPath = goalorientationReadyState.CacheRootPath,
            ConfigRootPath = goalorientationReadyState.ConfigRootPath,
            SettingsFilePath = goalorientationReadyState.SettingsFilePath,
            StartupProfilePath = goalorientationReadyState.StartupProfilePath,
            RequiredAssets = goalorientationReadyState.RequiredAssets,
            ReadyAssetCount = goalorientationReadyState.ReadyAssetCount,
            CompletedSteps = goalorientationReadyState.CompletedSteps,
            TotalSteps = goalorientationReadyState.TotalSteps,
            Exists = goalorientationReadyState.Exists,
            ReadSucceeded = goalorientationReadyState.ReadSucceeded
        };

        if (!goalorientationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser flowstate session blocked for profile '{goalorientationReadyState.ProfileId}'.";
            result.Error = goalorientationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowStateSessionVersion = "runtime-browser-flowstate-session-v1";
        result.BrowserFlowStateStages =
        [
            "open-browser-flowstate-session",
            "bind-browser-goalorientation-ready-state",
            "publish-browser-flowstate-ready"
        ];
        result.BrowserFlowStateSummary = $"Runtime browser flowstate session prepared {result.BrowserFlowStateStages.Length} flowstate stage(s) for profile '{goalorientationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowstate session ready for profile '{goalorientationReadyState.ProfileId}' with {result.BrowserFlowStateStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowStateSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFlowStateSessionVersion { get; set; } = string.Empty;
    public string BrowserGoalOrientationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoalOrientationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlowStateStages { get; set; } = Array.Empty<string>();
    public string BrowserFlowStateSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
