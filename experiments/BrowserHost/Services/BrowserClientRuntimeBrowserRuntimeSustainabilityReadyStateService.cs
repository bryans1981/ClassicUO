namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeSustainabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateService : IBrowserClientRuntimeBrowserRuntimeSustainabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserRuntimeSustainabilitySession _runtimeBrowserRuntimeSustainabilitySession;

    public BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateService(IBrowserClientRuntimeBrowserRuntimeSustainabilitySession runtimeBrowserRuntimeSustainabilitySession)
    {
        _runtimeBrowserRuntimeSustainabilitySession = runtimeBrowserRuntimeSustainabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeSustainabilitySessionResult runtimesustainabilitySession = await _runtimeBrowserRuntimeSustainabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateResult result = new()
        {
            ProfileId = runtimesustainabilitySession.ProfileId,
            SessionId = runtimesustainabilitySession.SessionId,
            SessionPath = runtimesustainabilitySession.SessionPath,
            BrowserRuntimeSustainabilitySessionVersion = runtimesustainabilitySession.BrowserRuntimeSustainabilitySessionVersion,
            BrowserLiveSustainabilityReadyStateVersion = runtimesustainabilitySession.BrowserLiveSustainabilityReadyStateVersion,
            BrowserLiveSustainabilitySessionVersion = runtimesustainabilitySession.BrowserLiveSustainabilitySessionVersion,
            LaunchMode = runtimesustainabilitySession.LaunchMode,
            AssetRootPath = runtimesustainabilitySession.AssetRootPath,
            ProfilesRootPath = runtimesustainabilitySession.ProfilesRootPath,
            CacheRootPath = runtimesustainabilitySession.CacheRootPath,
            ConfigRootPath = runtimesustainabilitySession.ConfigRootPath,
            SettingsFilePath = runtimesustainabilitySession.SettingsFilePath,
            StartupProfilePath = runtimesustainabilitySession.StartupProfilePath,
            RequiredAssets = runtimesustainabilitySession.RequiredAssets,
            ReadyAssetCount = runtimesustainabilitySession.ReadyAssetCount,
            CompletedSteps = runtimesustainabilitySession.CompletedSteps,
            TotalSteps = runtimesustainabilitySession.TotalSteps,
            Exists = runtimesustainabilitySession.Exists,
            ReadSucceeded = runtimesustainabilitySession.ReadSucceeded
        };

        if (!runtimesustainabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimesustainability ready state blocked for profile '{runtimesustainabilitySession.ProfileId}'.";
            result.Error = runtimesustainabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeSustainabilityReadyStateVersion = "runtime-browser-runtimesustainability-ready-state-v1";
        result.BrowserRuntimeSustainabilityReadyChecks =
        [
            "browser-livesustainability-ready-state-ready",
            "browser-runtimesustainability-session-ready",
            "browser-runtimesustainability-ready"
        ];
        result.BrowserRuntimeSustainabilityReadySummary = $"Runtime browser runtimesustainability ready state passed {result.BrowserRuntimeSustainabilityReadyChecks.Length} runtimesustainability readiness check(s) for profile '{runtimesustainabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimesustainability ready state ready for profile '{runtimesustainabilitySession.ProfileId}' with {result.BrowserRuntimeSustainabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeSustainabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeSustainabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
