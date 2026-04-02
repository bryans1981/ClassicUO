namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowStateReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFlowStateReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowStateReadyStateService : IBrowserClientRuntimeBrowserFlowStateReadyState
{
    private readonly IBrowserClientRuntimeBrowserFlowStateSession _runtimeBrowserFlowStateSession;

    public BrowserClientRuntimeBrowserFlowStateReadyStateService(IBrowserClientRuntimeBrowserFlowStateSession runtimeBrowserFlowStateSession)
    {
        _runtimeBrowserFlowStateSession = runtimeBrowserFlowStateSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowStateReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowStateSessionResult flowstateSession = await _runtimeBrowserFlowStateSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFlowStateReadyStateResult result = new()
        {
            ProfileId = flowstateSession.ProfileId,
            SessionId = flowstateSession.SessionId,
            SessionPath = flowstateSession.SessionPath,
            BrowserFlowStateSessionVersion = flowstateSession.BrowserFlowStateSessionVersion,
            BrowserGoalOrientationReadyStateVersion = flowstateSession.BrowserGoalOrientationReadyStateVersion,
            BrowserGoalOrientationSessionVersion = flowstateSession.BrowserGoalOrientationSessionVersion,
            LaunchMode = flowstateSession.LaunchMode,
            AssetRootPath = flowstateSession.AssetRootPath,
            ProfilesRootPath = flowstateSession.ProfilesRootPath,
            CacheRootPath = flowstateSession.CacheRootPath,
            ConfigRootPath = flowstateSession.ConfigRootPath,
            SettingsFilePath = flowstateSession.SettingsFilePath,
            StartupProfilePath = flowstateSession.StartupProfilePath,
            RequiredAssets = flowstateSession.RequiredAssets,
            ReadyAssetCount = flowstateSession.ReadyAssetCount,
            CompletedSteps = flowstateSession.CompletedSteps,
            TotalSteps = flowstateSession.TotalSteps,
            Exists = flowstateSession.Exists,
            ReadSucceeded = flowstateSession.ReadSucceeded
        };

        if (!flowstateSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser flowstate ready state blocked for profile '{flowstateSession.ProfileId}'.";
            result.Error = flowstateSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowStateReadyStateVersion = "runtime-browser-flowstate-ready-state-v1";
        result.BrowserFlowStateReadyChecks =
        [
            "browser-goalorientation-ready-state-ready",
            "browser-flowstate-session-ready",
            "browser-flowstate-ready"
        ];
        result.BrowserFlowStateReadySummary = $"Runtime browser flowstate ready state passed {result.BrowserFlowStateReadyChecks.Length} flowstate readiness check(s) for profile '{flowstateSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowstate ready state ready for profile '{flowstateSession.ProfileId}' with {result.BrowserFlowStateReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowStateReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFlowStateReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlowStateReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFlowStateReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
