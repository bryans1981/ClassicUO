namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyContinuityReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionService : IBrowserClientRuntimeBrowserSteadyContinuityReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserLiveContinuityReadyState _runtimeBrowserLiveContinuityReadyState;

    public BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionService(IBrowserClientRuntimeBrowserLiveContinuityReadyState runtimeBrowserLiveContinuityReadyState)
    {
        _runtimeBrowserLiveContinuityReadyState = runtimeBrowserLiveContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveContinuityReadyStateResult prevReadyState = await _runtimeBrowserLiveContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLiveContinuityReadyStateVersion = prevReadyState.BrowserLiveContinuityReadyStateVersion,
            BrowserLiveContinuitySessionVersion = prevReadyState.BrowserLiveContinuitySessionVersion,
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
            result.Summary = $"Runtime browser steadycontinuityreadiness session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyContinuityReadinessSessionVersion = "runtime-browser-steadycontinuityreadiness-session-v1";
        result.BrowserSteadyContinuityReadinessStages =
        [
            "open-browser-steadycontinuityreadiness-session",
            "bind-browser-livecontinuity-ready-state",
            "publish-browser-steadycontinuityreadiness-ready"
        ];
        result.BrowserSteadyContinuityReadinessSummary = $"Runtime browser steadycontinuityreadiness session prepared {result.BrowserSteadyContinuityReadinessStages.Length} steadycontinuityreadiness stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadycontinuityreadiness session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSteadyContinuityReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyContinuityReadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyContinuityReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserLiveContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyContinuityReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserSteadyContinuityReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
