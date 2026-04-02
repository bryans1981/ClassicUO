namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserObservabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserObservabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserObservabilityReadyStateService : IBrowserClientRuntimeBrowserObservabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserObservabilitySession _runtimeBrowserObservabilitySession;

    public BrowserClientRuntimeBrowserObservabilityReadyStateService(IBrowserClientRuntimeBrowserObservabilitySession runtimeBrowserObservabilitySession)
    {
        _runtimeBrowserObservabilitySession = runtimeBrowserObservabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserObservabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserObservabilitySessionResult observabilitySession = await _runtimeBrowserObservabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserObservabilityReadyStateResult result = new()
        {
            ProfileId = observabilitySession.ProfileId,
            SessionId = observabilitySession.SessionId,
            SessionPath = observabilitySession.SessionPath,
            BrowserObservabilitySessionVersion = observabilitySession.BrowserObservabilitySessionVersion,
            BrowserSignalCoherenceReadyStateVersion = observabilitySession.BrowserSignalCoherenceReadyStateVersion,
            BrowserSignalCoherenceSessionVersion = observabilitySession.BrowserSignalCoherenceSessionVersion,
            LaunchMode = observabilitySession.LaunchMode,
            AssetRootPath = observabilitySession.AssetRootPath,
            ProfilesRootPath = observabilitySession.ProfilesRootPath,
            CacheRootPath = observabilitySession.CacheRootPath,
            ConfigRootPath = observabilitySession.ConfigRootPath,
            SettingsFilePath = observabilitySession.SettingsFilePath,
            StartupProfilePath = observabilitySession.StartupProfilePath,
            RequiredAssets = observabilitySession.RequiredAssets,
            ReadyAssetCount = observabilitySession.ReadyAssetCount,
            CompletedSteps = observabilitySession.CompletedSteps,
            TotalSteps = observabilitySession.TotalSteps,
            Exists = observabilitySession.Exists,
            ReadSucceeded = observabilitySession.ReadSucceeded
        };

        if (!observabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser observability ready state blocked for profile '{observabilitySession.ProfileId}'.";
            result.Error = observabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserObservabilityReadyStateVersion = "runtime-browser-observability-ready-state-v1";
        result.BrowserObservabilityReadyChecks =
        [
            "browser-signalcoherence-ready-state-ready",
            "browser-observability-session-ready",
            "browser-observability-ready"
        ];
        result.BrowserObservabilityReadySummary = $"Runtime browser observability ready state passed {result.BrowserObservabilityReadyChecks.Length} observability readiness check(s) for profile '{observabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser observability ready state ready for profile '{observabilitySession.ProfileId}' with {result.BrowserObservabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserObservabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserObservabilityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserObservabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserObservabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
