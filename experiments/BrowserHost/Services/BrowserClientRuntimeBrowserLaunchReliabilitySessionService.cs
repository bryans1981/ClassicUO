namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLaunchReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLaunchReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLaunchReliabilitySessionService : IBrowserClientRuntimeBrowserLaunchReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserReleaseReliabilityReadyState _runtimeBrowserReleaseReliabilityReadyState;

    public BrowserClientRuntimeBrowserLaunchReliabilitySessionService(IBrowserClientRuntimeBrowserReleaseReliabilityReadyState runtimeBrowserReleaseReliabilityReadyState)
    {
        _runtimeBrowserReleaseReliabilityReadyState = runtimeBrowserReleaseReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLaunchReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReleaseReliabilityReadyStateResult prevReadyState = await _runtimeBrowserReleaseReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLaunchReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserReleaseReliabilityReadyStateVersion = prevReadyState.BrowserReleaseReliabilityReadyStateVersion,
            BrowserReleaseReliabilitySessionVersion = prevReadyState.BrowserReleaseReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser launchreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLaunchReliabilitySessionVersion = "runtime-browser-launchreliability-session-v1";
        result.BrowserLaunchReliabilityStages =
        [
            "open-browser-launchreliability-session",
            "bind-browser-releasereliability-ready-state",
            "publish-browser-launchreliability-ready"
        ];
        result.BrowserLaunchReliabilitySummary = $"Runtime browser launchreliability session prepared {result.BrowserLaunchReliabilityStages.Length} launchreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser launchreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLaunchReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLaunchReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLaunchReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserReleaseReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReleaseReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLaunchReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLaunchReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
