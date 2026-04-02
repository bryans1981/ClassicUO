namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLongevityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLongevityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLongevityReadyStateService : IBrowserClientRuntimeBrowserLongevityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLongevitySession _runtimeBrowserLongevitySession;

    public BrowserClientRuntimeBrowserLongevityReadyStateService(IBrowserClientRuntimeBrowserLongevitySession runtimeBrowserLongevitySession)
    {
        _runtimeBrowserLongevitySession = runtimeBrowserLongevitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLongevityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLongevitySessionResult longevitySession = await _runtimeBrowserLongevitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLongevityReadyStateResult result = new()
        {
            ProfileId = longevitySession.ProfileId,
            SessionId = longevitySession.SessionId,
            SessionPath = longevitySession.SessionPath,
            BrowserLongevitySessionVersion = longevitySession.BrowserLongevitySessionVersion,
            BrowserSustainabilityReadyStateVersion = longevitySession.BrowserSustainabilityReadyStateVersion,
            BrowserSustainabilitySessionVersion = longevitySession.BrowserSustainabilitySessionVersion,
            LaunchMode = longevitySession.LaunchMode,
            AssetRootPath = longevitySession.AssetRootPath,
            ProfilesRootPath = longevitySession.ProfilesRootPath,
            CacheRootPath = longevitySession.CacheRootPath,
            ConfigRootPath = longevitySession.ConfigRootPath,
            SettingsFilePath = longevitySession.SettingsFilePath,
            StartupProfilePath = longevitySession.StartupProfilePath,
            RequiredAssets = longevitySession.RequiredAssets,
            ReadyAssetCount = longevitySession.ReadyAssetCount,
            CompletedSteps = longevitySession.CompletedSteps,
            TotalSteps = longevitySession.TotalSteps,
            Exists = longevitySession.Exists,
            ReadSucceeded = longevitySession.ReadSucceeded
        };

        if (!longevitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser longevity ready state blocked for profile '{longevitySession.ProfileId}'.";
            result.Error = longevitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLongevityReadyStateVersion = "runtime-browser-longevity-ready-state-v1";
        result.BrowserLongevityReadyChecks =
        [
            "browser-sustainability-ready-state-ready",
            "browser-longevity-session-ready",
            "browser-longevity-ready"
        ];
        result.BrowserLongevityReadySummary = $"Runtime browser longevity ready state passed {result.BrowserLongevityReadyChecks.Length} longevity readiness check(s) for profile '{longevitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser longevity ready state ready for profile '{longevitySession.ProfileId}' with {result.BrowserLongevityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLongevityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLongevityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLongevitySessionVersion { get; set; } = string.Empty;
    public string BrowserSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLongevityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLongevityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
