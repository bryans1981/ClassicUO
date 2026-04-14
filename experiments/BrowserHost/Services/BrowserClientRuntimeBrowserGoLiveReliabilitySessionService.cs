namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveReliabilitySessionService : IBrowserClientRuntimeBrowserGoLiveReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserLaunchReliabilityReadyState _runtimeBrowserLaunchReliabilityReadyState;

    public BrowserClientRuntimeBrowserGoLiveReliabilitySessionService(IBrowserClientRuntimeBrowserLaunchReliabilityReadyState runtimeBrowserLaunchReliabilityReadyState)
    {
        _runtimeBrowserLaunchReliabilityReadyState = runtimeBrowserLaunchReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchReliabilityReadyStateResult prevReadyState = await _runtimeBrowserLaunchReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLaunchReliabilityReadyStateVersion = prevReadyState.BrowserLaunchReliabilityReadyStateVersion,
            BrowserLaunchReliabilitySessionVersion = prevReadyState.BrowserLaunchReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser golivereliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveReliabilitySessionVersion = "runtime-browser-golivereliability-session-v1";
        result.BrowserGoLiveReliabilityStages =
        [
            "open-browser-golivereliability-session",
            "bind-browser-launchreliability-ready-state",
            "publish-browser-golivereliability-ready"
        ];
        result.BrowserGoLiveReliabilitySummary = $"Runtime browser golivereliability session prepared {result.BrowserGoLiveReliabilityStages.Length} golivereliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser golivereliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserGoLiveReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserGoLiveReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLaunchReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoLiveReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
