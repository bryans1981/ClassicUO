namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConfidenceBuildingSession
{
    ValueTask<BrowserClientRuntimeBrowserConfidenceBuildingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConfidenceBuildingSessionService : IBrowserClientRuntimeBrowserConfidenceBuildingSession
{
    private readonly IBrowserClientRuntimeBrowserMasteryReadyState _runtimeBrowserMasteryReadyState;

    public BrowserClientRuntimeBrowserConfidenceBuildingSessionService(IBrowserClientRuntimeBrowserMasteryReadyState runtimeBrowserMasteryReadyState)
    {
        _runtimeBrowserMasteryReadyState = runtimeBrowserMasteryReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConfidenceBuildingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMasteryReadyStateResult masteryReadyState = await _runtimeBrowserMasteryReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserConfidenceBuildingSessionResult result = new()
        {
            ProfileId = masteryReadyState.ProfileId,
            SessionId = masteryReadyState.SessionId,
            SessionPath = masteryReadyState.SessionPath,
            BrowserMasteryReadyStateVersion = masteryReadyState.BrowserMasteryReadyStateVersion,
            BrowserMasterySessionVersion = masteryReadyState.BrowserMasterySessionVersion,
            LaunchMode = masteryReadyState.LaunchMode,
            AssetRootPath = masteryReadyState.AssetRootPath,
            ProfilesRootPath = masteryReadyState.ProfilesRootPath,
            CacheRootPath = masteryReadyState.CacheRootPath,
            ConfigRootPath = masteryReadyState.ConfigRootPath,
            SettingsFilePath = masteryReadyState.SettingsFilePath,
            StartupProfilePath = masteryReadyState.StartupProfilePath,
            RequiredAssets = masteryReadyState.RequiredAssets,
            ReadyAssetCount = masteryReadyState.ReadyAssetCount,
            CompletedSteps = masteryReadyState.CompletedSteps,
            TotalSteps = masteryReadyState.TotalSteps,
            Exists = masteryReadyState.Exists,
            ReadSucceeded = masteryReadyState.ReadSucceeded
        };

        if (!masteryReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser confidencebuilding session blocked for profile '{masteryReadyState.ProfileId}'.";
            result.Error = masteryReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConfidenceBuildingSessionVersion = "runtime-browser-confidencebuilding-session-v1";
        result.BrowserConfidenceBuildingStages =
        [
            "open-browser-confidencebuilding-session",
            "bind-browser-mastery-ready-state",
            "publish-browser-confidencebuilding-ready"
        ];
        result.BrowserConfidenceBuildingSummary = $"Runtime browser confidencebuilding session prepared {result.BrowserConfidenceBuildingStages.Length} confidencebuilding stage(s) for profile '{masteryReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser confidencebuilding session ready for profile '{masteryReadyState.ProfileId}' with {result.BrowserConfidenceBuildingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConfidenceBuildingSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserConfidenceBuildingStages { get; set; } = Array.Empty<string>();
    public string BrowserConfidenceBuildingSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
