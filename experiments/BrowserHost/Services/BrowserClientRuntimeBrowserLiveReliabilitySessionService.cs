namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLiveReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveReliabilitySessionService : IBrowserClientRuntimeBrowserLiveReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSustainmentReliabilityReadyState _runtimeBrowserSustainmentReliabilityReadyState;

    public BrowserClientRuntimeBrowserLiveReliabilitySessionService(IBrowserClientRuntimeBrowserSustainmentReliabilityReadyState runtimeBrowserSustainmentReliabilityReadyState)
    {
        _runtimeBrowserSustainmentReliabilityReadyState = runtimeBrowserSustainmentReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentReliabilityReadyStateResult prevReadyState = await _runtimeBrowserSustainmentReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSustainmentReliabilityReadyStateVersion = prevReadyState.BrowserSustainmentReliabilityReadyStateVersion,
            BrowserSustainmentReliabilitySessionVersion = prevReadyState.BrowserSustainmentReliabilitySessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser livereliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveReliabilitySessionVersion = "runtime-browser-livereliability-session-v1";
        result.BrowserLiveReliabilityStages =
        [
            "open-browser-livereliability-session",
            "bind-browser-sustainmentreliability-ready-state",
            "publish-browser-livereliability-ready"
        ];
        result.BrowserLiveReliabilitySummary = $"Runtime browser livereliability session prepared {result.BrowserLiveReliabilityStages.Length} livereliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livereliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLiveReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSustainmentReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
