namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEngagementSession
{
    ValueTask<BrowserClientRuntimeBrowserEngagementSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEngagementSessionService : IBrowserClientRuntimeBrowserEngagementSession
{
    private readonly IBrowserClientRuntimeBrowserFlowStateReadyState _runtimeBrowserFlowStateReadyState;

    public BrowserClientRuntimeBrowserEngagementSessionService(IBrowserClientRuntimeBrowserFlowStateReadyState runtimeBrowserFlowStateReadyState)
    {
        _runtimeBrowserFlowStateReadyState = runtimeBrowserFlowStateReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEngagementSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowStateReadyStateResult flowstateReadyState = await _runtimeBrowserFlowStateReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEngagementSessionResult result = new()
        {
            ProfileId = flowstateReadyState.ProfileId,
            SessionId = flowstateReadyState.SessionId,
            SessionPath = flowstateReadyState.SessionPath,
            BrowserFlowStateReadyStateVersion = flowstateReadyState.BrowserFlowStateReadyStateVersion,
            BrowserFlowStateSessionVersion = flowstateReadyState.BrowserFlowStateSessionVersion,
            LaunchMode = flowstateReadyState.LaunchMode,
            AssetRootPath = flowstateReadyState.AssetRootPath,
            ProfilesRootPath = flowstateReadyState.ProfilesRootPath,
            CacheRootPath = flowstateReadyState.CacheRootPath,
            ConfigRootPath = flowstateReadyState.ConfigRootPath,
            SettingsFilePath = flowstateReadyState.SettingsFilePath,
            StartupProfilePath = flowstateReadyState.StartupProfilePath,
            RequiredAssets = flowstateReadyState.RequiredAssets,
            ReadyAssetCount = flowstateReadyState.ReadyAssetCount,
            CompletedSteps = flowstateReadyState.CompletedSteps,
            TotalSteps = flowstateReadyState.TotalSteps,
            Exists = flowstateReadyState.Exists,
            ReadSucceeded = flowstateReadyState.ReadSucceeded
        };

        if (!flowstateReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser engagement session blocked for profile '{flowstateReadyState.ProfileId}'.";
            result.Error = flowstateReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEngagementSessionVersion = "runtime-browser-engagement-session-v1";
        result.BrowserEngagementStages =
        [
            "open-browser-engagement-session",
            "bind-browser-flowstate-ready-state",
            "publish-browser-engagement-ready"
        ];
        result.BrowserEngagementSummary = $"Runtime browser engagement session prepared {result.BrowserEngagementStages.Length} engagement stage(s) for profile '{flowstateReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser engagement session ready for profile '{flowstateReadyState.ProfileId}' with {result.BrowserEngagementStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEngagementSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserEngagementSessionVersion { get; set; } = string.Empty;
    public string BrowserFlowStateReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowStateSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEngagementStages { get; set; } = Array.Empty<string>();
    public string BrowserEngagementSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
