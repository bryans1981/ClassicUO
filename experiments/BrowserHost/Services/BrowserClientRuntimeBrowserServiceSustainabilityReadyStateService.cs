namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceSustainabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceSustainabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceSustainabilityReadyStateService : IBrowserClientRuntimeBrowserServiceSustainabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceSustainabilitySession _runtimeBrowserServiceSustainabilitySession;

    public BrowserClientRuntimeBrowserServiceSustainabilityReadyStateService(IBrowserClientRuntimeBrowserServiceSustainabilitySession runtimeBrowserServiceSustainabilitySession)
    {
        _runtimeBrowserServiceSustainabilitySession = runtimeBrowserServiceSustainabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceSustainabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceSustainabilitySessionResult servicesustainabilitySession = await _runtimeBrowserServiceSustainabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserServiceSustainabilityReadyStateResult result = new()
        {
            ProfileId = servicesustainabilitySession.ProfileId,
            SessionId = servicesustainabilitySession.SessionId,
            SessionPath = servicesustainabilitySession.SessionPath,
            BrowserServiceSustainabilitySessionVersion = servicesustainabilitySession.BrowserServiceSustainabilitySessionVersion,
            BrowserRuntimeSustainabilityReadyStateVersion = servicesustainabilitySession.BrowserRuntimeSustainabilityReadyStateVersion,
            BrowserRuntimeSustainabilitySessionVersion = servicesustainabilitySession.BrowserRuntimeSustainabilitySessionVersion,
            LaunchMode = servicesustainabilitySession.LaunchMode,
            AssetRootPath = servicesustainabilitySession.AssetRootPath,
            ProfilesRootPath = servicesustainabilitySession.ProfilesRootPath,
            CacheRootPath = servicesustainabilitySession.CacheRootPath,
            ConfigRootPath = servicesustainabilitySession.ConfigRootPath,
            SettingsFilePath = servicesustainabilitySession.SettingsFilePath,
            StartupProfilePath = servicesustainabilitySession.StartupProfilePath,
            RequiredAssets = servicesustainabilitySession.RequiredAssets,
            ReadyAssetCount = servicesustainabilitySession.ReadyAssetCount,
            CompletedSteps = servicesustainabilitySession.CompletedSteps,
            TotalSteps = servicesustainabilitySession.TotalSteps,
            Exists = servicesustainabilitySession.Exists,
            ReadSucceeded = servicesustainabilitySession.ReadSucceeded
        };

        if (!servicesustainabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser servicesustainability ready state blocked for profile '{servicesustainabilitySession.ProfileId}'.";
            result.Error = servicesustainabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceSustainabilityReadyStateVersion = "runtime-browser-servicesustainability-ready-state-v1";
        result.BrowserServiceSustainabilityReadyChecks =
        [
            "browser-runtimesustainability-ready-state-ready",
            "browser-servicesustainability-session-ready",
            "browser-servicesustainability-ready"
        ];
        result.BrowserServiceSustainabilityReadySummary = $"Runtime browser servicesustainability ready state passed {result.BrowserServiceSustainabilityReadyChecks.Length} servicesustainability readiness check(s) for profile '{servicesustainabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser servicesustainability ready state ready for profile '{servicesustainabilitySession.ProfileId}' with {result.BrowserServiceSustainabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceSustainabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceSustainabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceSustainabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
