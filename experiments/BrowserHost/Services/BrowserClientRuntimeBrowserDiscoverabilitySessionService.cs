namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDiscoverabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserDiscoverabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDiscoverabilitySessionService : IBrowserClientRuntimeBrowserDiscoverabilitySession
{
    private readonly IBrowserClientRuntimeBrowserAdaptabilityReadyState _runtimeBrowserAdaptabilityReadyState;

    public BrowserClientRuntimeBrowserDiscoverabilitySessionService(IBrowserClientRuntimeBrowserAdaptabilityReadyState runtimeBrowserAdaptabilityReadyState)
    {
        _runtimeBrowserAdaptabilityReadyState = runtimeBrowserAdaptabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDiscoverabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAdaptabilityReadyStateResult adaptabilityReadyState = await _runtimeBrowserAdaptabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDiscoverabilitySessionResult result = new()
        {
            ProfileId = adaptabilityReadyState.ProfileId,
            SessionId = adaptabilityReadyState.SessionId,
            SessionPath = adaptabilityReadyState.SessionPath,
            BrowserAdaptabilityReadyStateVersion = adaptabilityReadyState.BrowserAdaptabilityReadyStateVersion,
            BrowserAdaptabilitySessionVersion = adaptabilityReadyState.BrowserAdaptabilitySessionVersion,
            LaunchMode = adaptabilityReadyState.LaunchMode,
            AssetRootPath = adaptabilityReadyState.AssetRootPath,
            ProfilesRootPath = adaptabilityReadyState.ProfilesRootPath,
            CacheRootPath = adaptabilityReadyState.CacheRootPath,
            ConfigRootPath = adaptabilityReadyState.ConfigRootPath,
            SettingsFilePath = adaptabilityReadyState.SettingsFilePath,
            StartupProfilePath = adaptabilityReadyState.StartupProfilePath,
            RequiredAssets = adaptabilityReadyState.RequiredAssets,
            ReadyAssetCount = adaptabilityReadyState.ReadyAssetCount,
            CompletedSteps = adaptabilityReadyState.CompletedSteps,
            TotalSteps = adaptabilityReadyState.TotalSteps,
            Exists = adaptabilityReadyState.Exists,
            ReadSucceeded = adaptabilityReadyState.ReadSucceeded
        };

        if (!adaptabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser discoverability session blocked for profile '{adaptabilityReadyState.ProfileId}'.";
            result.Error = adaptabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDiscoverabilitySessionVersion = "runtime-browser-discoverability-session-v1";
        result.BrowserDiscoverabilityStages =
        [
            "open-browser-discoverability-session",
            "bind-browser-adaptability-ready-state",
            "publish-browser-discoverability-ready"
        ];
        result.BrowserDiscoverabilitySummary = $"Runtime browser discoverability session prepared {result.BrowserDiscoverabilityStages.Length} discoverability stage(s) for profile '{adaptabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser discoverability session ready for profile '{adaptabilityReadyState.ProfileId}' with {result.BrowserDiscoverabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDiscoverabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDiscoverabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserAdaptabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAdaptabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDiscoverabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserDiscoverabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
