namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSignalCoherenceSession
{
    ValueTask<BrowserClientRuntimeBrowserSignalCoherenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSignalCoherenceSessionService : IBrowserClientRuntimeBrowserSignalCoherenceSession
{
    private readonly IBrowserClientRuntimeBrowserDeliberatenessReadyState _runtimeBrowserDeliberatenessReadyState;

    public BrowserClientRuntimeBrowserSignalCoherenceSessionService(IBrowserClientRuntimeBrowserDeliberatenessReadyState runtimeBrowserDeliberatenessReadyState)
    {
        _runtimeBrowserDeliberatenessReadyState = runtimeBrowserDeliberatenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSignalCoherenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeliberatenessReadyStateResult deliberatenessReadyState = await _runtimeBrowserDeliberatenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSignalCoherenceSessionResult result = new()
        {
            ProfileId = deliberatenessReadyState.ProfileId,
            SessionId = deliberatenessReadyState.SessionId,
            SessionPath = deliberatenessReadyState.SessionPath,
            BrowserDeliberatenessReadyStateVersion = deliberatenessReadyState.BrowserDeliberatenessReadyStateVersion,
            BrowserDeliberatenessSessionVersion = deliberatenessReadyState.BrowserDeliberatenessSessionVersion,
            LaunchMode = deliberatenessReadyState.LaunchMode,
            AssetRootPath = deliberatenessReadyState.AssetRootPath,
            ProfilesRootPath = deliberatenessReadyState.ProfilesRootPath,
            CacheRootPath = deliberatenessReadyState.CacheRootPath,
            ConfigRootPath = deliberatenessReadyState.ConfigRootPath,
            SettingsFilePath = deliberatenessReadyState.SettingsFilePath,
            StartupProfilePath = deliberatenessReadyState.StartupProfilePath,
            RequiredAssets = deliberatenessReadyState.RequiredAssets,
            ReadyAssetCount = deliberatenessReadyState.ReadyAssetCount,
            CompletedSteps = deliberatenessReadyState.CompletedSteps,
            TotalSteps = deliberatenessReadyState.TotalSteps,
            Exists = deliberatenessReadyState.Exists,
            ReadSucceeded = deliberatenessReadyState.ReadSucceeded
        };

        if (!deliberatenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser signalcoherence session blocked for profile '{deliberatenessReadyState.ProfileId}'.";
            result.Error = deliberatenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSignalCoherenceSessionVersion = "runtime-browser-signalcoherence-session-v1";
        result.BrowserSignalCoherenceStages =
        [
            "open-browser-signalcoherence-session",
            "bind-browser-deliberateness-ready-state",
            "publish-browser-signalcoherence-ready"
        ];
        result.BrowserSignalCoherenceSummary = $"Runtime browser signalcoherence session prepared {result.BrowserSignalCoherenceStages.Length} signalcoherence stage(s) for profile '{deliberatenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser signalcoherence session ready for profile '{deliberatenessReadyState.ProfileId}' with {result.BrowserSignalCoherenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSignalCoherenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSignalCoherenceSessionVersion { get; set; } = string.Empty;
    public string BrowserDeliberatenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeliberatenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSignalCoherenceStages { get; set; } = Array.Empty<string>();
    public string BrowserSignalCoherenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
