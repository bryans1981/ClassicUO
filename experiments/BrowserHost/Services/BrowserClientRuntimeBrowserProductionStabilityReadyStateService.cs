namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserProductionStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionStabilityReadyStateService : IBrowserClientRuntimeBrowserProductionStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserProductionStabilitySession _runtimeBrowserProductionStabilitySession;

    public BrowserClientRuntimeBrowserProductionStabilityReadyStateService(IBrowserClientRuntimeBrowserProductionStabilitySession runtimeBrowserProductionStabilitySession)
    {
        _runtimeBrowserProductionStabilitySession = runtimeBrowserProductionStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionStabilitySessionResult productionstabilitySession = await _runtimeBrowserProductionStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserProductionStabilityReadyStateResult result = new()
        {
            ProfileId = productionstabilitySession.ProfileId,
            SessionId = productionstabilitySession.SessionId,
            SessionPath = productionstabilitySession.SessionPath,
            BrowserProductionStabilitySessionVersion = productionstabilitySession.BrowserProductionStabilitySessionVersion,
            BrowserServiceSustainabilityReadyStateVersion = productionstabilitySession.BrowserServiceSustainabilityReadyStateVersion,
            BrowserServiceSustainabilitySessionVersion = productionstabilitySession.BrowserServiceSustainabilitySessionVersion,
            LaunchMode = productionstabilitySession.LaunchMode,
            AssetRootPath = productionstabilitySession.AssetRootPath,
            ProfilesRootPath = productionstabilitySession.ProfilesRootPath,
            CacheRootPath = productionstabilitySession.CacheRootPath,
            ConfigRootPath = productionstabilitySession.ConfigRootPath,
            SettingsFilePath = productionstabilitySession.SettingsFilePath,
            StartupProfilePath = productionstabilitySession.StartupProfilePath,
            RequiredAssets = productionstabilitySession.RequiredAssets,
            ReadyAssetCount = productionstabilitySession.ReadyAssetCount,
            CompletedSteps = productionstabilitySession.CompletedSteps,
            TotalSteps = productionstabilitySession.TotalSteps,
            Exists = productionstabilitySession.Exists,
            ReadSucceeded = productionstabilitySession.ReadSucceeded
        };

        if (!productionstabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser productionstability ready state blocked for profile '{productionstabilitySession.ProfileId}'.";
            result.Error = productionstabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionStabilityReadyStateVersion = "runtime-browser-productionstability-ready-state-v1";
        result.BrowserProductionStabilityReadyChecks =
        [
            "browser-servicesustainability-ready-state-ready",
            "browser-productionstability-session-ready",
            "browser-productionstability-ready"
        ];
        result.BrowserProductionStabilityReadySummary = $"Runtime browser productionstability ready state passed {result.BrowserProductionStabilityReadyChecks.Length} productionstability readiness check(s) for profile '{productionstabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionstability ready state ready for profile '{productionstabilitySession.ProfileId}' with {result.BrowserProductionStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserProductionStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
