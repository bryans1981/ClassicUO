namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeOperabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateService : IBrowserClientRuntimeBrowserRuntimeOperabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserRuntimeOperabilitySession _runtimeBrowserRuntimeOperabilitySession;

    public BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateService(IBrowserClientRuntimeBrowserRuntimeOperabilitySession runtimeBrowserRuntimeOperabilitySession)
    {
        _runtimeBrowserRuntimeOperabilitySession = runtimeBrowserRuntimeOperabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeOperabilitySessionResult runtimeoperabilitySession = await _runtimeBrowserRuntimeOperabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateResult result = new()
        {
            ProfileId = runtimeoperabilitySession.ProfileId,
            SessionId = runtimeoperabilitySession.SessionId,
            SessionPath = runtimeoperabilitySession.SessionPath,
            BrowserRuntimeOperabilitySessionVersion = runtimeoperabilitySession.BrowserRuntimeOperabilitySessionVersion,
            BrowserLiveOperabilityReadyStateVersion = runtimeoperabilitySession.BrowserLiveOperabilityReadyStateVersion,
            BrowserLiveOperabilitySessionVersion = runtimeoperabilitySession.BrowserLiveOperabilitySessionVersion,
            LaunchMode = runtimeoperabilitySession.LaunchMode,
            AssetRootPath = runtimeoperabilitySession.AssetRootPath,
            ProfilesRootPath = runtimeoperabilitySession.ProfilesRootPath,
            CacheRootPath = runtimeoperabilitySession.CacheRootPath,
            ConfigRootPath = runtimeoperabilitySession.ConfigRootPath,
            SettingsFilePath = runtimeoperabilitySession.SettingsFilePath,
            StartupProfilePath = runtimeoperabilitySession.StartupProfilePath,
            RequiredAssets = runtimeoperabilitySession.RequiredAssets,
            ReadyAssetCount = runtimeoperabilitySession.ReadyAssetCount,
            CompletedSteps = runtimeoperabilitySession.CompletedSteps,
            TotalSteps = runtimeoperabilitySession.TotalSteps,
            Exists = runtimeoperabilitySession.Exists,
            ReadSucceeded = runtimeoperabilitySession.ReadSucceeded
        };

        if (!runtimeoperabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimeoperability ready state blocked for profile '{runtimeoperabilitySession.ProfileId}'.";
            result.Error = runtimeoperabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeOperabilityReadyStateVersion = "runtime-browser-runtimeoperability-ready-state-v1";
        result.BrowserRuntimeOperabilityReadyChecks =
        [
            "browser-liveoperability-ready-state-ready",
            "browser-runtimeoperability-session-ready",
            "browser-runtimeoperability-ready"
        ];
        result.BrowserRuntimeOperabilityReadySummary = $"Runtime browser runtimeoperability ready state passed {result.BrowserRuntimeOperabilityReadyChecks.Length} runtimeoperability readiness check(s) for profile '{runtimeoperabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimeoperability ready state ready for profile '{runtimeoperabilitySession.ProfileId}' with {result.BrowserRuntimeOperabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeOperabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveOperabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeOperabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeOperabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
