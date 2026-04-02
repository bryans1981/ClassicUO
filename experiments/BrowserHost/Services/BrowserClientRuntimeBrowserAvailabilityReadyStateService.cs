namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAvailabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAvailabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAvailabilityReadyStateService : IBrowserClientRuntimeBrowserAvailabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserAvailabilitySession _runtimeBrowserAvailabilitySession;

    public BrowserClientRuntimeBrowserAvailabilityReadyStateService(IBrowserClientRuntimeBrowserAvailabilitySession runtimeBrowserAvailabilitySession)
    {
        _runtimeBrowserAvailabilitySession = runtimeBrowserAvailabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAvailabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAvailabilitySessionResult availabilitySession = await _runtimeBrowserAvailabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAvailabilityReadyStateResult result = new()
        {
            ProfileId = availabilitySession.ProfileId,
            SessionId = availabilitySession.SessionId,
            SessionPath = availabilitySession.SessionPath,
            BrowserAvailabilitySessionVersion = availabilitySession.BrowserAvailabilitySessionVersion,
            BrowserResilienceReadyStateVersion = availabilitySession.BrowserResilienceReadyStateVersion,
            BrowserResilienceSessionVersion = availabilitySession.BrowserResilienceSessionVersion,
            LaunchMode = availabilitySession.LaunchMode,
            AssetRootPath = availabilitySession.AssetRootPath,
            ProfilesRootPath = availabilitySession.ProfilesRootPath,
            CacheRootPath = availabilitySession.CacheRootPath,
            ConfigRootPath = availabilitySession.ConfigRootPath,
            SettingsFilePath = availabilitySession.SettingsFilePath,
            StartupProfilePath = availabilitySession.StartupProfilePath,
            RequiredAssets = availabilitySession.RequiredAssets,
            ReadyAssetCount = availabilitySession.ReadyAssetCount,
            CompletedSteps = availabilitySession.CompletedSteps,
            TotalSteps = availabilitySession.TotalSteps,
            Exists = availabilitySession.Exists,
            ReadSucceeded = availabilitySession.ReadSucceeded,
            BrowserAvailabilitySession = availabilitySession
        };

        if (!availabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser availability ready state blocked for profile '{availabilitySession.ProfileId}'.";
            result.Error = availabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAvailabilityReadyStateVersion = "runtime-browser-availability-ready-state-v1";
        result.BrowserAvailabilityReadyChecks =
        [
            "browser-resilience-ready-state-ready",
            "browser-availability-session-ready",
            "browser-availability-ready"
        ];
        result.BrowserAvailabilityReadySummary = $"Runtime browser availability ready state passed {result.BrowserAvailabilityReadyChecks.Length} availability readiness check(s) for profile '{availabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser availability ready state ready for profile '{availabilitySession.ProfileId}' with {result.BrowserAvailabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAvailabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAvailabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAvailabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAvailabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAvailabilityReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAvailabilitySessionResult BrowserAvailabilitySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
