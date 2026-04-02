namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComfortSession
{
    ValueTask<BrowserClientRuntimeBrowserComfortSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComfortSessionService : IBrowserClientRuntimeBrowserComfortSession
{
    private readonly IBrowserClientRuntimeBrowserConfidenceReadyState _runtimeBrowserConfidenceReadyState;

    public BrowserClientRuntimeBrowserComfortSessionService(IBrowserClientRuntimeBrowserConfidenceReadyState runtimeBrowserConfidenceReadyState)
    {
        _runtimeBrowserConfidenceReadyState = runtimeBrowserConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComfortSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConfidenceReadyStateResult confidenceReadyState = await _runtimeBrowserConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserComfortSessionResult result = new()
        {
            ProfileId = confidenceReadyState.ProfileId,
            SessionId = confidenceReadyState.SessionId,
            SessionPath = confidenceReadyState.SessionPath,
            BrowserConfidenceReadyStateVersion = confidenceReadyState.BrowserConfidenceReadyStateVersion,
            BrowserConfidenceSessionVersion = confidenceReadyState.BrowserConfidenceSessionVersion,
            LaunchMode = confidenceReadyState.LaunchMode,
            AssetRootPath = confidenceReadyState.AssetRootPath,
            ProfilesRootPath = confidenceReadyState.ProfilesRootPath,
            CacheRootPath = confidenceReadyState.CacheRootPath,
            ConfigRootPath = confidenceReadyState.ConfigRootPath,
            SettingsFilePath = confidenceReadyState.SettingsFilePath,
            StartupProfilePath = confidenceReadyState.StartupProfilePath,
            RequiredAssets = confidenceReadyState.RequiredAssets,
            ReadyAssetCount = confidenceReadyState.ReadyAssetCount,
            CompletedSteps = confidenceReadyState.CompletedSteps,
            TotalSteps = confidenceReadyState.TotalSteps,
            Exists = confidenceReadyState.Exists,
            ReadSucceeded = confidenceReadyState.ReadSucceeded
        };

        if (!confidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser comfort session blocked for profile '{confidenceReadyState.ProfileId}'.";
            result.Error = confidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComfortSessionVersion = "runtime-browser-comfort-session-v1";
        result.BrowserComfortStages =
        [
            "open-browser-comfort-session",
            "bind-browser-confidence-ready-state",
            "publish-browser-comfort-ready"
        ];
        result.BrowserComfortSummary = $"Runtime browser comfort session prepared {result.BrowserComfortStages.Length} comfort stage(s) for profile '{confidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser comfort session ready for profile '{confidenceReadyState.ProfileId}' with {result.BrowserComfortStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComfortSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserComfortSessionVersion { get; set; } = string.Empty;
    public string BrowserConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComfortStages { get; set; } = Array.Empty<string>();
    public string BrowserComfortSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
