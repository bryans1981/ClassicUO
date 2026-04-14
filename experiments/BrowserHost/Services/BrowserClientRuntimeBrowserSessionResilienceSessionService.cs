namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserSessionResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionResilienceSessionService : IBrowserClientRuntimeBrowserSessionResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeResilienceReadyState _runtimeBrowserRuntimeResilienceReadyState;

    public BrowserClientRuntimeBrowserSessionResilienceSessionService(IBrowserClientRuntimeBrowserRuntimeResilienceReadyState runtimeBrowserRuntimeResilienceReadyState)
    {
        _runtimeBrowserRuntimeResilienceReadyState = runtimeBrowserRuntimeResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeResilienceReadyStateResult prevReadyState = await _runtimeBrowserRuntimeResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSessionResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserRuntimeResilienceReadyStateVersion = prevReadyState.BrowserRuntimeResilienceReadyStateVersion,
            BrowserRuntimeResilienceSessionVersion = prevReadyState.BrowserRuntimeResilienceSessionVersion,
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
            result.Summary = $"Runtime browser sessionresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionResilienceSessionVersion = "runtime-browser-sessionresilience-session-v1";
        result.BrowserSessionResilienceStages =
        [
            "open-browser-sessionresilience-session",
            "bind-browser-runtimeresilience-ready-state",
            "publish-browser-sessionresilience-ready"
        ];
        result.BrowserSessionResilienceSummary = $"Runtime browser sessionresilience session prepared {result.BrowserSessionResilienceStages.Length} sessionresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessionresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSessionResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserSessionResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
