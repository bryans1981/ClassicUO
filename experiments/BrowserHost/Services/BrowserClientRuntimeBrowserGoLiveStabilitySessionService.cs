namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGoLiveStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserGoLiveStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGoLiveStabilitySessionService : IBrowserClientRuntimeBrowserGoLiveStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserLaunchStabilityReadyState _runtimeBrowserLaunchStabilityReadyState;

    public BrowserClientRuntimeBrowserGoLiveStabilitySessionService(IBrowserClientRuntimeBrowserLaunchStabilityReadyState runtimeBrowserLaunchStabilityReadyState)
    {
        _runtimeBrowserLaunchStabilityReadyState = runtimeBrowserLaunchStabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGoLiveStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLaunchStabilityReadyStateResult prevReadyState = await _runtimeBrowserLaunchStabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGoLiveStabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLaunchStabilityReadyStateVersion = prevReadyState.BrowserLaunchStabilityReadyStateVersion,
            BrowserLaunchStabilitySessionVersion = prevReadyState.BrowserLaunchStabilitySessionVersion,
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
            result.Summary = $"Runtime browser golivestability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGoLiveStabilitySessionVersion = "runtime-browser-golivestability-session-v1";
        result.BrowserGoLiveStabilityStages =
        [
            "open-browser-golivestability-session",
            "bind-browser-launchstability-ready-state",
            "publish-browser-golivestability-ready"
        ];
        result.BrowserGoLiveStabilitySummary = $"Runtime browser golivestability session prepared {result.BrowserGoLiveStabilityStages.Length} golivestability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser golivestability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserGoLiveStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGoLiveStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserGoLiveStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLaunchStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLaunchStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGoLiveStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserGoLiveStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
