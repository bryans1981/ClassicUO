namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComposureSession
{
    ValueTask<BrowserClientRuntimeBrowserComposureSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComposureSessionService : IBrowserClientRuntimeBrowserComposureSession
{
    private readonly IBrowserClientRuntimeBrowserRelianceReadyState _runtimeBrowserRelianceReadyState;

    public BrowserClientRuntimeBrowserComposureSessionService(IBrowserClientRuntimeBrowserRelianceReadyState runtimeBrowserRelianceReadyState)
    {
        _runtimeBrowserRelianceReadyState = runtimeBrowserRelianceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComposureSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRelianceReadyStateResult relianceReadyState = await _runtimeBrowserRelianceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserComposureSessionResult result = new()
        {
            ProfileId = relianceReadyState.ProfileId,
            SessionId = relianceReadyState.SessionId,
            SessionPath = relianceReadyState.SessionPath,
            BrowserRelianceReadyStateVersion = relianceReadyState.BrowserRelianceReadyStateVersion,
            BrowserRelianceSessionVersion = relianceReadyState.BrowserRelianceSessionVersion,
            LaunchMode = relianceReadyState.LaunchMode,
            AssetRootPath = relianceReadyState.AssetRootPath,
            ProfilesRootPath = relianceReadyState.ProfilesRootPath,
            CacheRootPath = relianceReadyState.CacheRootPath,
            ConfigRootPath = relianceReadyState.ConfigRootPath,
            SettingsFilePath = relianceReadyState.SettingsFilePath,
            StartupProfilePath = relianceReadyState.StartupProfilePath,
            RequiredAssets = relianceReadyState.RequiredAssets,
            ReadyAssetCount = relianceReadyState.ReadyAssetCount,
            CompletedSteps = relianceReadyState.CompletedSteps,
            TotalSteps = relianceReadyState.TotalSteps,
            Exists = relianceReadyState.Exists,
            ReadSucceeded = relianceReadyState.ReadSucceeded
        };

        if (!relianceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser composure session blocked for profile '{relianceReadyState.ProfileId}'.";
            result.Error = relianceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComposureSessionVersion = "runtime-browser-composure-session-v1";
        result.BrowserComposureStages =
        [
            "open-browser-composure-session",
            "bind-browser-reliance-ready-state",
            "publish-browser-composure-ready"
        ];
        result.BrowserComposureSummary = $"Runtime browser composure session prepared {result.BrowserComposureStages.Length} composure stage(s) for profile '{relianceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser composure session ready for profile '{relianceReadyState.ProfileId}' with {result.BrowserComposureStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComposureSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserComposureSessionVersion { get; set; } = string.Empty;
    public string BrowserRelianceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRelianceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComposureStages { get; set; } = Array.Empty<string>();
    public string BrowserComposureSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
