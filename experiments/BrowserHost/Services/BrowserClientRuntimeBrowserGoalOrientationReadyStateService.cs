namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoalOrientationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGoalOrientationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoalOrientationReadyStateService : IBrowserClientRuntimeBrowserGoalOrientationReadyState
{
    private readonly IBrowserClientRuntimeBrowserGoalOrientationSession _runtimeBrowserGoalOrientationSession;

    public BrowserClientRuntimeBrowserGoalOrientationReadyStateService(IBrowserClientRuntimeBrowserGoalOrientationSession runtimeBrowserGoalOrientationSession)
    {
        _runtimeBrowserGoalOrientationSession = runtimeBrowserGoalOrientationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoalOrientationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoalOrientationSessionResult goalorientationSession = await _runtimeBrowserGoalOrientationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGoalOrientationReadyStateResult result = new()
        {
            ProfileId = goalorientationSession.ProfileId,
            SessionId = goalorientationSession.SessionId,
            SessionPath = goalorientationSession.SessionPath,
            BrowserGoalOrientationSessionVersion = goalorientationSession.BrowserGoalOrientationSessionVersion,
            BrowserTaskAlignmentReadyStateVersion = goalorientationSession.BrowserTaskAlignmentReadyStateVersion,
            BrowserTaskAlignmentSessionVersion = goalorientationSession.BrowserTaskAlignmentSessionVersion,
            LaunchMode = goalorientationSession.LaunchMode,
            AssetRootPath = goalorientationSession.AssetRootPath,
            ProfilesRootPath = goalorientationSession.ProfilesRootPath,
            CacheRootPath = goalorientationSession.CacheRootPath,
            ConfigRootPath = goalorientationSession.ConfigRootPath,
            SettingsFilePath = goalorientationSession.SettingsFilePath,
            StartupProfilePath = goalorientationSession.StartupProfilePath,
            RequiredAssets = goalorientationSession.RequiredAssets,
            ReadyAssetCount = goalorientationSession.ReadyAssetCount,
            CompletedSteps = goalorientationSession.CompletedSteps,
            TotalSteps = goalorientationSession.TotalSteps,
            Exists = goalorientationSession.Exists,
            ReadSucceeded = goalorientationSession.ReadSucceeded
        };

        if (!goalorientationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser goalorientation ready state blocked for profile '{goalorientationSession.ProfileId}'.";
            result.Error = goalorientationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoalOrientationReadyStateVersion = "runtime-browser-goalorientation-ready-state-v1";
        result.BrowserGoalOrientationReadyChecks =
        [
            "browser-taskalignment-ready-state-ready",
            "browser-goalorientation-session-ready",
            "browser-goalorientation-ready"
        ];
        result.BrowserGoalOrientationReadySummary = $"Runtime browser goalorientation ready state passed {result.BrowserGoalOrientationReadyChecks.Length} goalorientation readiness check(s) for profile '{goalorientationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser goalorientation ready state ready for profile '{goalorientationSession.ProfileId}' with {result.BrowserGoalOrientationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoalOrientationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGoalOrientationReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoalOrientationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGoalOrientationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
