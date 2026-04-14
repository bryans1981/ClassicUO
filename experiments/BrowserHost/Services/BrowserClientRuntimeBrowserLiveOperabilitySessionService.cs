namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveOperabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLiveOperabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveOperabilitySessionService : IBrowserClientRuntimeBrowserLiveOperabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSustainmentReadinessReadyState _runtimeBrowserSustainmentReadinessReadyState;

    public BrowserClientRuntimeBrowserLiveOperabilitySessionService(IBrowserClientRuntimeBrowserSustainmentReadinessReadyState runtimeBrowserSustainmentReadinessReadyState)
    {
        _runtimeBrowserSustainmentReadinessReadyState = runtimeBrowserSustainmentReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveOperabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentReadinessReadyStateResult sustainmentreadinessReadyState = await _runtimeBrowserSustainmentReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveOperabilitySessionResult result = new()
        {
            ProfileId = sustainmentreadinessReadyState.ProfileId,
            SessionId = sustainmentreadinessReadyState.SessionId,
            SessionPath = sustainmentreadinessReadyState.SessionPath,
            BrowserSustainmentReadinessReadyStateVersion = sustainmentreadinessReadyState.BrowserSustainmentReadinessReadyStateVersion,
            BrowserSustainmentReadinessSessionVersion = sustainmentreadinessReadyState.BrowserSustainmentReadinessSessionVersion,
            LaunchMode = sustainmentreadinessReadyState.LaunchMode,
            AssetRootPath = sustainmentreadinessReadyState.AssetRootPath,
            ProfilesRootPath = sustainmentreadinessReadyState.ProfilesRootPath,
            CacheRootPath = sustainmentreadinessReadyState.CacheRootPath,
            ConfigRootPath = sustainmentreadinessReadyState.ConfigRootPath,
            SettingsFilePath = sustainmentreadinessReadyState.SettingsFilePath,
            StartupProfilePath = sustainmentreadinessReadyState.StartupProfilePath,
            RequiredAssets = sustainmentreadinessReadyState.RequiredAssets,
            ReadyAssetCount = sustainmentreadinessReadyState.ReadyAssetCount,
            CompletedSteps = sustainmentreadinessReadyState.CompletedSteps,
            TotalSteps = sustainmentreadinessReadyState.TotalSteps,
            Exists = sustainmentreadinessReadyState.Exists,
            ReadSucceeded = sustainmentreadinessReadyState.ReadSucceeded
        };

        if (!sustainmentreadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser liveoperability session blocked for profile '{sustainmentreadinessReadyState.ProfileId}'.";
            result.Error = sustainmentreadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveOperabilitySessionVersion = "runtime-browser-liveoperability-session-v1";
        result.BrowserLiveOperabilityStages =
        [
            "open-browser-liveoperability-session",
            "bind-browser-sustainmentreadiness-ready-state",
            "publish-browser-liveoperability-ready"
        ];
        result.BrowserLiveOperabilitySummary = $"Runtime browser liveoperability session prepared {result.BrowserLiveOperabilityStages.Length} liveoperability stage(s) for profile '{sustainmentreadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser liveoperability session ready for profile '{sustainmentreadinessReadyState.ProfileId}' with {result.BrowserLiveOperabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveOperabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveOperabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSustainmentReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveOperabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveOperabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
