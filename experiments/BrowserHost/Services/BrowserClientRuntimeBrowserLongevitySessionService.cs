namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLongevitySession
{
    ValueTask<BrowserClientRuntimeBrowserLongevitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLongevitySessionService : IBrowserClientRuntimeBrowserLongevitySession
{
    private readonly IBrowserClientRuntimeBrowserSustainabilityReadyState _runtimeBrowserSustainabilityReadyState;

    public BrowserClientRuntimeBrowserLongevitySessionService(IBrowserClientRuntimeBrowserSustainabilityReadyState runtimeBrowserSustainabilityReadyState)
    {
        _runtimeBrowserSustainabilityReadyState = runtimeBrowserSustainabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLongevitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainabilityReadyStateResult sustainabilityReadyState = await _runtimeBrowserSustainabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLongevitySessionResult result = new()
        {
            ProfileId = sustainabilityReadyState.ProfileId,
            SessionId = sustainabilityReadyState.SessionId,
            SessionPath = sustainabilityReadyState.SessionPath,
            BrowserSustainabilityReadyStateVersion = sustainabilityReadyState.BrowserSustainabilityReadyStateVersion,
            BrowserSustainabilitySessionVersion = sustainabilityReadyState.BrowserSustainabilitySessionVersion,
            LaunchMode = sustainabilityReadyState.LaunchMode,
            AssetRootPath = sustainabilityReadyState.AssetRootPath,
            ProfilesRootPath = sustainabilityReadyState.ProfilesRootPath,
            CacheRootPath = sustainabilityReadyState.CacheRootPath,
            ConfigRootPath = sustainabilityReadyState.ConfigRootPath,
            SettingsFilePath = sustainabilityReadyState.SettingsFilePath,
            StartupProfilePath = sustainabilityReadyState.StartupProfilePath,
            RequiredAssets = sustainabilityReadyState.RequiredAssets,
            ReadyAssetCount = sustainabilityReadyState.ReadyAssetCount,
            CompletedSteps = sustainabilityReadyState.CompletedSteps,
            TotalSteps = sustainabilityReadyState.TotalSteps,
            Exists = sustainabilityReadyState.Exists,
            ReadSucceeded = sustainabilityReadyState.ReadSucceeded
        };

        if (!sustainabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser longevity session blocked for profile '{sustainabilityReadyState.ProfileId}'.";
            result.Error = sustainabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLongevitySessionVersion = "runtime-browser-longevity-session-v1";
        result.BrowserLongevityStages =
        [
            "open-browser-longevity-session",
            "bind-browser-sustainability-ready-state",
            "publish-browser-longevity-ready"
        ];
        result.BrowserLongevitySummary = $"Runtime browser longevity session prepared {result.BrowserLongevityStages.Length} longevity stage(s) for profile '{sustainabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser longevity session ready for profile '{sustainabilityReadyState.ProfileId}' with {result.BrowserLongevityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLongevitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLongevitySessionVersion { get; set; } = string.Empty;
    public string BrowserSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLongevityStages { get; set; } = Array.Empty<string>();
    public string BrowserLongevitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
