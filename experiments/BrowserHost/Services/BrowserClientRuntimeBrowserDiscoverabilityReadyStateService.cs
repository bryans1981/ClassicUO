namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDiscoverabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDiscoverabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDiscoverabilityReadyStateService : IBrowserClientRuntimeBrowserDiscoverabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserDiscoverabilitySession _runtimeBrowserDiscoverabilitySession;

    public BrowserClientRuntimeBrowserDiscoverabilityReadyStateService(IBrowserClientRuntimeBrowserDiscoverabilitySession runtimeBrowserDiscoverabilitySession)
    {
        _runtimeBrowserDiscoverabilitySession = runtimeBrowserDiscoverabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDiscoverabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDiscoverabilitySessionResult discoverabilitySession = await _runtimeBrowserDiscoverabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDiscoverabilityReadyStateResult result = new()
        {
            ProfileId = discoverabilitySession.ProfileId,
            SessionId = discoverabilitySession.SessionId,
            SessionPath = discoverabilitySession.SessionPath,
            BrowserDiscoverabilitySessionVersion = discoverabilitySession.BrowserDiscoverabilitySessionVersion,
            BrowserAdaptabilityReadyStateVersion = discoverabilitySession.BrowserAdaptabilityReadyStateVersion,
            BrowserAdaptabilitySessionVersion = discoverabilitySession.BrowserAdaptabilitySessionVersion,
            LaunchMode = discoverabilitySession.LaunchMode,
            AssetRootPath = discoverabilitySession.AssetRootPath,
            ProfilesRootPath = discoverabilitySession.ProfilesRootPath,
            CacheRootPath = discoverabilitySession.CacheRootPath,
            ConfigRootPath = discoverabilitySession.ConfigRootPath,
            SettingsFilePath = discoverabilitySession.SettingsFilePath,
            StartupProfilePath = discoverabilitySession.StartupProfilePath,
            RequiredAssets = discoverabilitySession.RequiredAssets,
            ReadyAssetCount = discoverabilitySession.ReadyAssetCount,
            CompletedSteps = discoverabilitySession.CompletedSteps,
            TotalSteps = discoverabilitySession.TotalSteps,
            Exists = discoverabilitySession.Exists,
            ReadSucceeded = discoverabilitySession.ReadSucceeded
        };

        if (!discoverabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser discoverability ready state blocked for profile '{discoverabilitySession.ProfileId}'.";
            result.Error = discoverabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDiscoverabilityReadyStateVersion = "runtime-browser-discoverability-ready-state-v1";
        result.BrowserDiscoverabilityReadyChecks =
        [
            "browser-adaptability-ready-state-ready",
            "browser-discoverability-session-ready",
            "browser-discoverability-ready"
        ];
        result.BrowserDiscoverabilityReadySummary = $"Runtime browser discoverability ready state passed {result.BrowserDiscoverabilityReadyChecks.Length} discoverability readiness check(s) for profile '{discoverabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser discoverability ready state ready for profile '{discoverabilitySession.ProfileId}' with {result.BrowserDiscoverabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDiscoverabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDiscoverabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDiscoverabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserAdaptabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAdaptabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDiscoverabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDiscoverabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
