namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeOperabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeOperabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeOperabilitySessionService : IBrowserClientRuntimeBrowserRuntimeOperabilitySession
{
    private readonly IBrowserClientRuntimeBrowserLiveOperabilityReadyState _runtimeBrowserLiveOperabilityReadyState;

    public BrowserClientRuntimeBrowserRuntimeOperabilitySessionService(IBrowserClientRuntimeBrowserLiveOperabilityReadyState runtimeBrowserLiveOperabilityReadyState)
    {
        _runtimeBrowserLiveOperabilityReadyState = runtimeBrowserLiveOperabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeOperabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveOperabilityReadyStateResult liveoperabilityReadyState = await _runtimeBrowserLiveOperabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeOperabilitySessionResult result = new()
        {
            ProfileId = liveoperabilityReadyState.ProfileId,
            SessionId = liveoperabilityReadyState.SessionId,
            SessionPath = liveoperabilityReadyState.SessionPath,
            BrowserLiveOperabilityReadyStateVersion = liveoperabilityReadyState.BrowserLiveOperabilityReadyStateVersion,
            BrowserLiveOperabilitySessionVersion = liveoperabilityReadyState.BrowserLiveOperabilitySessionVersion,
            LaunchMode = liveoperabilityReadyState.LaunchMode,
            AssetRootPath = liveoperabilityReadyState.AssetRootPath,
            ProfilesRootPath = liveoperabilityReadyState.ProfilesRootPath,
            CacheRootPath = liveoperabilityReadyState.CacheRootPath,
            ConfigRootPath = liveoperabilityReadyState.ConfigRootPath,
            SettingsFilePath = liveoperabilityReadyState.SettingsFilePath,
            StartupProfilePath = liveoperabilityReadyState.StartupProfilePath,
            RequiredAssets = liveoperabilityReadyState.RequiredAssets,
            ReadyAssetCount = liveoperabilityReadyState.ReadyAssetCount,
            CompletedSteps = liveoperabilityReadyState.CompletedSteps,
            TotalSteps = liveoperabilityReadyState.TotalSteps,
            Exists = liveoperabilityReadyState.Exists,
            ReadSucceeded = liveoperabilityReadyState.ReadSucceeded
        };

        if (!liveoperabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimeoperability session blocked for profile '{liveoperabilityReadyState.ProfileId}'.";
            result.Error = liveoperabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeOperabilitySessionVersion = "runtime-browser-runtimeoperability-session-v1";
        result.BrowserRuntimeOperabilityStages =
        [
            "open-browser-runtimeoperability-session",
            "bind-browser-liveoperability-ready-state",
            "publish-browser-runtimeoperability-ready"
        ];
        result.BrowserRuntimeOperabilitySummary = $"Runtime browser runtimeoperability session prepared {result.BrowserRuntimeOperabilityStages.Length} runtimeoperability stage(s) for profile '{liveoperabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimeoperability session ready for profile '{liveoperabilityReadyState.ProfileId}' with {result.BrowserRuntimeOperabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeOperabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeOperabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveOperabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeOperabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeOperabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
