namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserStateResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateResilienceSessionService : IBrowserClientRuntimeBrowserStateResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserSessionResilienceReadyState _runtimeBrowserSessionResilienceReadyState;

    public BrowserClientRuntimeBrowserStateResilienceSessionService(IBrowserClientRuntimeBrowserSessionResilienceReadyState runtimeBrowserSessionResilienceReadyState)
    {
        _runtimeBrowserSessionResilienceReadyState = runtimeBrowserSessionResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionResilienceReadyStateResult prevReadyState = await _runtimeBrowserSessionResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStateResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSessionResilienceReadyStateVersion = prevReadyState.BrowserSessionResilienceReadyStateVersion,
            BrowserSessionResilienceSessionVersion = prevReadyState.BrowserSessionResilienceSessionVersion,
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
            result.Summary = $"Runtime browser stateresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateResilienceSessionVersion = "runtime-browser-stateresilience-session-v1";
        result.BrowserStateResilienceStages =
        [
            "open-browser-stateresilience-session",
            "bind-browser-sessionresilience-ready-state",
            "publish-browser-stateresilience-ready"
        ];
        result.BrowserStateResilienceSummary = $"Runtime browser stateresilience session prepared {result.BrowserStateResilienceStages.Length} stateresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser stateresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserStateResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserStateResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserSessionResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserStateResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
