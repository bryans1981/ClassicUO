namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserObservabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserObservabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserObservabilitySessionService : IBrowserClientRuntimeBrowserObservabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSignalCoherenceReadyState _runtimeBrowserSignalCoherenceReadyState;

    public BrowserClientRuntimeBrowserObservabilitySessionService(IBrowserClientRuntimeBrowserSignalCoherenceReadyState runtimeBrowserSignalCoherenceReadyState)
    {
        _runtimeBrowserSignalCoherenceReadyState = runtimeBrowserSignalCoherenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserObservabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSignalCoherenceReadyStateResult signalcoherenceReadyState = await _runtimeBrowserSignalCoherenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserObservabilitySessionResult result = new()
        {
            ProfileId = signalcoherenceReadyState.ProfileId,
            SessionId = signalcoherenceReadyState.SessionId,
            SessionPath = signalcoherenceReadyState.SessionPath,
            BrowserSignalCoherenceReadyStateVersion = signalcoherenceReadyState.BrowserSignalCoherenceReadyStateVersion,
            BrowserSignalCoherenceSessionVersion = signalcoherenceReadyState.BrowserSignalCoherenceSessionVersion,
            LaunchMode = signalcoherenceReadyState.LaunchMode,
            AssetRootPath = signalcoherenceReadyState.AssetRootPath,
            ProfilesRootPath = signalcoherenceReadyState.ProfilesRootPath,
            CacheRootPath = signalcoherenceReadyState.CacheRootPath,
            ConfigRootPath = signalcoherenceReadyState.ConfigRootPath,
            SettingsFilePath = signalcoherenceReadyState.SettingsFilePath,
            StartupProfilePath = signalcoherenceReadyState.StartupProfilePath,
            RequiredAssets = signalcoherenceReadyState.RequiredAssets,
            ReadyAssetCount = signalcoherenceReadyState.ReadyAssetCount,
            CompletedSteps = signalcoherenceReadyState.CompletedSteps,
            TotalSteps = signalcoherenceReadyState.TotalSteps,
            Exists = signalcoherenceReadyState.Exists,
            ReadSucceeded = signalcoherenceReadyState.ReadSucceeded
        };

        if (!signalcoherenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser observability session blocked for profile '{signalcoherenceReadyState.ProfileId}'.";
            result.Error = signalcoherenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserObservabilitySessionVersion = "runtime-browser-observability-session-v1";
        result.BrowserObservabilityStages =
        [
            "open-browser-observability-session",
            "bind-browser-signalcoherence-ready-state",
            "publish-browser-observability-ready"
        ];
        result.BrowserObservabilitySummary = $"Runtime browser observability session prepared {result.BrowserObservabilityStages.Length} observability stage(s) for profile '{signalcoherenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser observability session ready for profile '{signalcoherenceReadyState.ProfileId}' with {result.BrowserObservabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserObservabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserObservabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSignalCoherenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSignalCoherenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserObservabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserObservabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
