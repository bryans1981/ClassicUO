namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSignalCoherenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSignalCoherenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSignalCoherenceReadyStateService : IBrowserClientRuntimeBrowserSignalCoherenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserSignalCoherenceSession _runtimeBrowserSignalCoherenceSession;

    public BrowserClientRuntimeBrowserSignalCoherenceReadyStateService(IBrowserClientRuntimeBrowserSignalCoherenceSession runtimeBrowserSignalCoherenceSession)
    {
        _runtimeBrowserSignalCoherenceSession = runtimeBrowserSignalCoherenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSignalCoherenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSignalCoherenceSessionResult signalcoherenceSession = await _runtimeBrowserSignalCoherenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSignalCoherenceReadyStateResult result = new()
        {
            ProfileId = signalcoherenceSession.ProfileId,
            SessionId = signalcoherenceSession.SessionId,
            SessionPath = signalcoherenceSession.SessionPath,
            BrowserSignalCoherenceSessionVersion = signalcoherenceSession.BrowserSignalCoherenceSessionVersion,
            BrowserDeliberatenessReadyStateVersion = signalcoherenceSession.BrowserDeliberatenessReadyStateVersion,
            BrowserDeliberatenessSessionVersion = signalcoherenceSession.BrowserDeliberatenessSessionVersion,
            LaunchMode = signalcoherenceSession.LaunchMode,
            AssetRootPath = signalcoherenceSession.AssetRootPath,
            ProfilesRootPath = signalcoherenceSession.ProfilesRootPath,
            CacheRootPath = signalcoherenceSession.CacheRootPath,
            ConfigRootPath = signalcoherenceSession.ConfigRootPath,
            SettingsFilePath = signalcoherenceSession.SettingsFilePath,
            StartupProfilePath = signalcoherenceSession.StartupProfilePath,
            RequiredAssets = signalcoherenceSession.RequiredAssets,
            ReadyAssetCount = signalcoherenceSession.ReadyAssetCount,
            CompletedSteps = signalcoherenceSession.CompletedSteps,
            TotalSteps = signalcoherenceSession.TotalSteps,
            Exists = signalcoherenceSession.Exists,
            ReadSucceeded = signalcoherenceSession.ReadSucceeded
        };

        if (!signalcoherenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser signalcoherence ready state blocked for profile '{signalcoherenceSession.ProfileId}'.";
            result.Error = signalcoherenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSignalCoherenceReadyStateVersion = "runtime-browser-signalcoherence-ready-state-v1";
        result.BrowserSignalCoherenceReadyChecks =
        [
            "browser-deliberateness-ready-state-ready",
            "browser-signalcoherence-session-ready",
            "browser-signalcoherence-ready"
        ];
        result.BrowserSignalCoherenceReadySummary = $"Runtime browser signalcoherence ready state passed {result.BrowserSignalCoherenceReadyChecks.Length} signalcoherence readiness check(s) for profile '{signalcoherenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser signalcoherence ready state ready for profile '{signalcoherenceSession.ProfileId}' with {result.BrowserSignalCoherenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSignalCoherenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSignalCoherenceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserSignalCoherenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSignalCoherenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
