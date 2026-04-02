namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTaskConfidenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTaskConfidenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTaskConfidenceReadyStateService : IBrowserClientRuntimeBrowserTaskConfidenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserTaskConfidenceSession _runtimeBrowserTaskConfidenceSession;

    public BrowserClientRuntimeBrowserTaskConfidenceReadyStateService(IBrowserClientRuntimeBrowserTaskConfidenceSession runtimeBrowserTaskConfidenceSession)
    {
        _runtimeBrowserTaskConfidenceSession = runtimeBrowserTaskConfidenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTaskConfidenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTaskConfidenceSessionResult taskconfidenceSession = await _runtimeBrowserTaskConfidenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTaskConfidenceReadyStateResult result = new()
        {
            ProfileId = taskconfidenceSession.ProfileId,
            SessionId = taskconfidenceSession.SessionId,
            SessionPath = taskconfidenceSession.SessionPath,
            BrowserTaskConfidenceSessionVersion = taskconfidenceSession.BrowserTaskConfidenceSessionVersion,
            BrowserConfidenceBuildingReadyStateVersion = taskconfidenceSession.BrowserConfidenceBuildingReadyStateVersion,
            BrowserConfidenceBuildingSessionVersion = taskconfidenceSession.BrowserConfidenceBuildingSessionVersion,
            LaunchMode = taskconfidenceSession.LaunchMode,
            AssetRootPath = taskconfidenceSession.AssetRootPath,
            ProfilesRootPath = taskconfidenceSession.ProfilesRootPath,
            CacheRootPath = taskconfidenceSession.CacheRootPath,
            ConfigRootPath = taskconfidenceSession.ConfigRootPath,
            SettingsFilePath = taskconfidenceSession.SettingsFilePath,
            StartupProfilePath = taskconfidenceSession.StartupProfilePath,
            RequiredAssets = taskconfidenceSession.RequiredAssets,
            ReadyAssetCount = taskconfidenceSession.ReadyAssetCount,
            CompletedSteps = taskconfidenceSession.CompletedSteps,
            TotalSteps = taskconfidenceSession.TotalSteps,
            Exists = taskconfidenceSession.Exists,
            ReadSucceeded = taskconfidenceSession.ReadSucceeded
        };

        if (!taskconfidenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser taskconfidence ready state blocked for profile '{taskconfidenceSession.ProfileId}'.";
            result.Error = taskconfidenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTaskConfidenceReadyStateVersion = "runtime-browser-taskconfidence-ready-state-v1";
        result.BrowserTaskConfidenceReadyChecks =
        [
            "browser-confidencebuilding-ready-state-ready",
            "browser-taskconfidence-session-ready",
            "browser-taskconfidence-ready"
        ];
        result.BrowserTaskConfidenceReadySummary = $"Runtime browser taskconfidence ready state passed {result.BrowserTaskConfidenceReadyChecks.Length} taskconfidence readiness check(s) for profile '{taskconfidenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser taskconfidence ready state ready for profile '{taskconfidenceSession.ProfileId}' with {result.BrowserTaskConfidenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTaskConfidenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTaskConfidenceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserTaskConfidenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTaskConfidenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
