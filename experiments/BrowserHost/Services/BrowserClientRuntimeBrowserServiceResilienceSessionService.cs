namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserServiceResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceResilienceSessionService : IBrowserClientRuntimeBrowserServiceResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserGoLiveResilienceReadyState _runtimeBrowserGoLiveResilienceReadyState;

    public BrowserClientRuntimeBrowserServiceResilienceSessionService(IBrowserClientRuntimeBrowserGoLiveResilienceReadyState runtimeBrowserGoLiveResilienceReadyState)
    {
        _runtimeBrowserGoLiveResilienceReadyState = runtimeBrowserGoLiveResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGoLiveResilienceReadyStateResult prevReadyState = await _runtimeBrowserGoLiveResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserGoLiveResilienceReadyStateVersion = prevReadyState.BrowserGoLiveResilienceReadyStateVersion,
            BrowserGoLiveResilienceSessionVersion = prevReadyState.BrowserGoLiveResilienceSessionVersion,
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
            result.Summary = $"Runtime browser serviceresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceResilienceSessionVersion = "runtime-browser-serviceresilience-session-v1";
        result.BrowserServiceResilienceStages =
        [
            "open-browser-serviceresilience-session",
            "bind-browser-goliveresilience-ready-state",
            "publish-browser-serviceresilience-ready"
        ];
        result.BrowserServiceResilienceSummary = $"Runtime browser serviceresilience session prepared {result.BrowserServiceResilienceStages.Length} serviceresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserServiceResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserGoLiveResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserServiceResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
