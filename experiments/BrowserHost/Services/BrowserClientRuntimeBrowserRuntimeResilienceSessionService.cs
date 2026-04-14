namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeResilienceSessionService : IBrowserClientRuntimeBrowserRuntimeResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserServiceResilienceReadyState _runtimeBrowserServiceResilienceReadyState;

    public BrowserClientRuntimeBrowserRuntimeResilienceSessionService(IBrowserClientRuntimeBrowserServiceResilienceReadyState runtimeBrowserServiceResilienceReadyState)
    {
        _runtimeBrowserServiceResilienceReadyState = runtimeBrowserServiceResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceResilienceReadyStateResult prevReadyState = await _runtimeBrowserServiceResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserServiceResilienceReadyStateVersion = prevReadyState.BrowserServiceResilienceReadyStateVersion,
            BrowserServiceResilienceSessionVersion = prevReadyState.BrowserServiceResilienceSessionVersion,
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
            result.Summary = $"Runtime browser runtimeresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeResilienceSessionVersion = "runtime-browser-runtimeresilience-session-v1";
        result.BrowserRuntimeResilienceStages =
        [
            "open-browser-runtimeresilience-session",
            "bind-browser-serviceresilience-ready-state",
            "publish-browser-runtimeresilience-ready"
        ];
        result.BrowserRuntimeResilienceSummary = $"Runtime browser runtimeresilience session prepared {result.BrowserRuntimeResilienceStages.Length} runtimeresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimeresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserRuntimeResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserServiceResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
