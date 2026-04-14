namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceActivationSession
{
    ValueTask<BrowserClientRuntimeBrowserServiceActivationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceActivationSessionService : IBrowserClientRuntimeBrowserServiceActivationSession
{
    private readonly IBrowserClientRuntimeBrowserGoLiveReadinessReadyState _runtimeBrowserGoLiveReadinessReadyState;

    public BrowserClientRuntimeBrowserServiceActivationSessionService(IBrowserClientRuntimeBrowserGoLiveReadinessReadyState runtimeBrowserGoLiveReadinessReadyState)
    {
        _runtimeBrowserGoLiveReadinessReadyState = runtimeBrowserGoLiveReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceActivationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoLiveReadinessReadyStateResult golivereadinessReadyState = await _runtimeBrowserGoLiveReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceActivationSessionResult result = new()
        {
            ProfileId = golivereadinessReadyState.ProfileId,
            SessionId = golivereadinessReadyState.SessionId,
            SessionPath = golivereadinessReadyState.SessionPath,
            BrowserGoLiveReadinessReadyStateVersion = golivereadinessReadyState.BrowserGoLiveReadinessReadyStateVersion,
            BrowserGoLiveReadinessSessionVersion = golivereadinessReadyState.BrowserGoLiveReadinessSessionVersion,
            LaunchMode = golivereadinessReadyState.LaunchMode,
            AssetRootPath = golivereadinessReadyState.AssetRootPath,
            ProfilesRootPath = golivereadinessReadyState.ProfilesRootPath,
            CacheRootPath = golivereadinessReadyState.CacheRootPath,
            ConfigRootPath = golivereadinessReadyState.ConfigRootPath,
            SettingsFilePath = golivereadinessReadyState.SettingsFilePath,
            StartupProfilePath = golivereadinessReadyState.StartupProfilePath,
            RequiredAssets = golivereadinessReadyState.RequiredAssets,
            ReadyAssetCount = golivereadinessReadyState.ReadyAssetCount,
            CompletedSteps = golivereadinessReadyState.CompletedSteps,
            TotalSteps = golivereadinessReadyState.TotalSteps,
            Exists = golivereadinessReadyState.Exists,
            ReadSucceeded = golivereadinessReadyState.ReadSucceeded
        };

        if (!golivereadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceactivation session blocked for profile '{golivereadinessReadyState.ProfileId}'.";
            result.Error = golivereadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceActivationSessionVersion = "runtime-browser-serviceactivation-session-v1";
        result.BrowserServiceActivationStages =
        [
            "open-browser-serviceactivation-session",
            "bind-browser-golivereadiness-ready-state",
            "publish-browser-serviceactivation-ready"
        ];
        result.BrowserServiceActivationSummary = $"Runtime browser serviceactivation session prepared {result.BrowserServiceActivationStages.Length} serviceactivation stage(s) for profile '{golivereadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceactivation session ready for profile '{golivereadinessReadyState.ProfileId}' with {result.BrowserServiceActivationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceActivationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceActivationSessionVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceActivationStages { get; set; } = Array.Empty<string>();
    public string BrowserServiceActivationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
