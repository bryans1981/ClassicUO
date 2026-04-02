namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTaskConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserTaskConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTaskConfidenceSessionService : IBrowserClientRuntimeBrowserTaskConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserConfidenceBuildingReadyState _runtimeBrowserConfidenceBuildingReadyState;

    public BrowserClientRuntimeBrowserTaskConfidenceSessionService(IBrowserClientRuntimeBrowserConfidenceBuildingReadyState runtimeBrowserConfidenceBuildingReadyState)
    {
        _runtimeBrowserConfidenceBuildingReadyState = runtimeBrowserConfidenceBuildingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTaskConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConfidenceBuildingReadyStateResult confidencebuildingReadyState = await _runtimeBrowserConfidenceBuildingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTaskConfidenceSessionResult result = new()
        {
            ProfileId = confidencebuildingReadyState.ProfileId,
            SessionId = confidencebuildingReadyState.SessionId,
            SessionPath = confidencebuildingReadyState.SessionPath,
            BrowserConfidenceBuildingReadyStateVersion = confidencebuildingReadyState.BrowserConfidenceBuildingReadyStateVersion,
            BrowserConfidenceBuildingSessionVersion = confidencebuildingReadyState.BrowserConfidenceBuildingSessionVersion,
            LaunchMode = confidencebuildingReadyState.LaunchMode,
            AssetRootPath = confidencebuildingReadyState.AssetRootPath,
            ProfilesRootPath = confidencebuildingReadyState.ProfilesRootPath,
            CacheRootPath = confidencebuildingReadyState.CacheRootPath,
            ConfigRootPath = confidencebuildingReadyState.ConfigRootPath,
            SettingsFilePath = confidencebuildingReadyState.SettingsFilePath,
            StartupProfilePath = confidencebuildingReadyState.StartupProfilePath,
            RequiredAssets = confidencebuildingReadyState.RequiredAssets,
            ReadyAssetCount = confidencebuildingReadyState.ReadyAssetCount,
            CompletedSteps = confidencebuildingReadyState.CompletedSteps,
            TotalSteps = confidencebuildingReadyState.TotalSteps,
            Exists = confidencebuildingReadyState.Exists,
            ReadSucceeded = confidencebuildingReadyState.ReadSucceeded
        };

        if (!confidencebuildingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser taskconfidence session blocked for profile '{confidencebuildingReadyState.ProfileId}'.";
            result.Error = confidencebuildingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTaskConfidenceSessionVersion = "runtime-browser-taskconfidence-session-v1";
        result.BrowserTaskConfidenceStages =
        [
            "open-browser-taskconfidence-session",
            "bind-browser-confidencebuilding-ready-state",
            "publish-browser-taskconfidence-ready"
        ];
        result.BrowserTaskConfidenceSummary = $"Runtime browser taskconfidence session prepared {result.BrowserTaskConfidenceStages.Length} taskconfidence stage(s) for profile '{confidencebuildingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser taskconfidence session ready for profile '{confidencebuildingReadyState.ProfileId}' with {result.BrowserTaskConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTaskConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTaskConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserConfidenceBuildingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfidenceBuildingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTaskConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserTaskConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
