namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStewardshipSession
{
    ValueTask<BrowserClientRuntimeBrowserStewardshipSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStewardshipSessionService : IBrowserClientRuntimeBrowserStewardshipSession
{
    private readonly IBrowserClientRuntimeBrowserLongevityReadyState _runtimeBrowserLongevityReadyState;

    public BrowserClientRuntimeBrowserStewardshipSessionService(IBrowserClientRuntimeBrowserLongevityReadyState runtimeBrowserLongevityReadyState)
    {
        _runtimeBrowserLongevityReadyState = runtimeBrowserLongevityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStewardshipSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLongevityReadyStateResult longevityReadyState = await _runtimeBrowserLongevityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStewardshipSessionResult result = new()
        {
            ProfileId = longevityReadyState.ProfileId,
            SessionId = longevityReadyState.SessionId,
            SessionPath = longevityReadyState.SessionPath,
            BrowserLongevityReadyStateVersion = longevityReadyState.BrowserLongevityReadyStateVersion,
            BrowserLongevitySessionVersion = longevityReadyState.BrowserLongevitySessionVersion,
            LaunchMode = longevityReadyState.LaunchMode,
            AssetRootPath = longevityReadyState.AssetRootPath,
            ProfilesRootPath = longevityReadyState.ProfilesRootPath,
            CacheRootPath = longevityReadyState.CacheRootPath,
            ConfigRootPath = longevityReadyState.ConfigRootPath,
            SettingsFilePath = longevityReadyState.SettingsFilePath,
            StartupProfilePath = longevityReadyState.StartupProfilePath,
            RequiredAssets = longevityReadyState.RequiredAssets,
            ReadyAssetCount = longevityReadyState.ReadyAssetCount,
            CompletedSteps = longevityReadyState.CompletedSteps,
            TotalSteps = longevityReadyState.TotalSteps,
            Exists = longevityReadyState.Exists,
            ReadSucceeded = longevityReadyState.ReadSucceeded
        };

        if (!longevityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser stewardship session blocked for profile '{longevityReadyState.ProfileId}'.";
            result.Error = longevityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStewardshipSessionVersion = "runtime-browser-stewardship-session-v1";
        result.BrowserStewardshipStages =
        [
            "open-browser-stewardship-session",
            "bind-browser-longevity-ready-state",
            "publish-browser-stewardship-ready"
        ];
        result.BrowserStewardshipSummary = $"Runtime browser stewardship session prepared {result.BrowserStewardshipStages.Length} stewardship stage(s) for profile '{longevityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser stewardship session ready for profile '{longevityReadyState.ProfileId}' with {result.BrowserStewardshipStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStewardshipSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserStewardshipSessionVersion { get; set; } = string.Empty;
    public string BrowserLongevityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLongevitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStewardshipStages { get; set; } = Array.Empty<string>();
    public string BrowserStewardshipSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
