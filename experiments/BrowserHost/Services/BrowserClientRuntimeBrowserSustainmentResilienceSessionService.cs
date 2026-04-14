namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentResilienceSessionService : IBrowserClientRuntimeBrowserSustainmentResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserContinuationResilienceReadyState _runtimeBrowserContinuationResilienceReadyState;

    public BrowserClientRuntimeBrowserSustainmentResilienceSessionService(IBrowserClientRuntimeBrowserContinuationResilienceReadyState runtimeBrowserContinuationResilienceReadyState)
    {
        _runtimeBrowserContinuationResilienceReadyState = runtimeBrowserContinuationResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationResilienceReadyStateResult prevReadyState = await _runtimeBrowserContinuationResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserContinuationResilienceReadyStateVersion = prevReadyState.BrowserContinuationResilienceReadyStateVersion,
            BrowserContinuationResilienceSessionVersion = prevReadyState.BrowserContinuationResilienceSessionVersion,
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
            result.Summary = $"Runtime browser sustainmentresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentResilienceSessionVersion = "runtime-browser-sustainmentresilience-session-v1";
        result.BrowserSustainmentResilienceStages =
        [
            "open-browser-sustainmentresilience-session",
            "bind-browser-continuationresilience-ready-state",
            "publish-browser-sustainmentresilience-ready"
        ];
        result.BrowserSustainmentResilienceSummary = $"Runtime browser sustainmentresilience session prepared {result.BrowserSustainmentResilienceStages.Length} sustainmentresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSustainmentResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentResilienceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainmentResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserContinuationResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainmentResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
