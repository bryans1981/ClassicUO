namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConfidenceBuildingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserConfidenceBuildingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConfidenceBuildingReadyStateService : IBrowserClientRuntimeBrowserConfidenceBuildingReadyState
{
    private readonly IBrowserClientRuntimeBrowserConfidenceBuildingSession _runtimeBrowserConfidenceBuildingSession;

    public BrowserClientRuntimeBrowserConfidenceBuildingReadyStateService(IBrowserClientRuntimeBrowserConfidenceBuildingSession runtimeBrowserConfidenceBuildingSession)
    {
        _runtimeBrowserConfidenceBuildingSession = runtimeBrowserConfidenceBuildingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConfidenceBuildingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConfidenceBuildingSessionResult confidencebuildingSession = await _runtimeBrowserConfidenceBuildingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserConfidenceBuildingReadyStateResult result = new()
        {
            ProfileId = confidencebuildingSession.ProfileId,
            SessionId = confidencebuildingSession.SessionId,
            SessionPath = confidencebuildingSession.SessionPath,
            BrowserConfidenceBuildingSessionVersion = confidencebuildingSession.BrowserConfidenceBuildingSessionVersion,
            BrowserMasteryReadyStateVersion = confidencebuildingSession.BrowserMasteryReadyStateVersion,
            BrowserMasterySessionVersion = confidencebuildingSession.BrowserMasterySessionVersion,
            LaunchMode = confidencebuildingSession.LaunchMode,
            AssetRootPath = confidencebuildingSession.AssetRootPath,
            ProfilesRootPath = confidencebuildingSession.ProfilesRootPath,
            CacheRootPath = confidencebuildingSession.CacheRootPath,
            ConfigRootPath = confidencebuildingSession.ConfigRootPath,
            SettingsFilePath = confidencebuildingSession.SettingsFilePath,
            StartupProfilePath = confidencebuildingSession.StartupProfilePath,
            RequiredAssets = confidencebuildingSession.RequiredAssets,
            ReadyAssetCount = confidencebuildingSession.ReadyAssetCount,
            CompletedSteps = confidencebuildingSession.CompletedSteps,
            TotalSteps = confidencebuildingSession.TotalSteps,
            Exists = confidencebuildingSession.Exists,
            ReadSucceeded = confidencebuildingSession.ReadSucceeded
        };

        if (!confidencebuildingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser confidencebuilding ready state blocked for profile '{confidencebuildingSession.ProfileId}'.";
            result.Error = confidencebuildingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConfidenceBuildingReadyStateVersion = "runtime-browser-confidencebuilding-ready-state-v1";
        result.BrowserConfidenceBuildingReadyChecks =
        [
            "browser-mastery-ready-state-ready",
            "browser-confidencebuilding-session-ready",
            "browser-confidencebuilding-ready"
        ];
        result.BrowserConfidenceBuildingReadySummary = $"Runtime browser confidencebuilding ready state passed {result.BrowserConfidenceBuildingReadyChecks.Length} confidencebuilding readiness check(s) for profile '{confidencebuildingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser confidencebuilding ready state ready for profile '{confidencebuildingSession.ProfileId}' with {result.BrowserConfidenceBuildingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConfidenceBuildingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserConfidenceBuildingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfidenceBuildingSessionVersion { get; set; } = string.Empty;
    public string BrowserMasteryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMasterySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConfidenceBuildingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserConfidenceBuildingReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
