namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveSustainabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLiveSustainabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveSustainabilityReadyStateService : IBrowserClientRuntimeBrowserLiveSustainabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLiveSustainabilitySession _runtimeBrowserLiveSustainabilitySession;

    public BrowserClientRuntimeBrowserLiveSustainabilityReadyStateService(IBrowserClientRuntimeBrowserLiveSustainabilitySession runtimeBrowserLiveSustainabilitySession)
    {
        _runtimeBrowserLiveSustainabilitySession = runtimeBrowserLiveSustainabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveSustainabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveSustainabilitySessionResult livesustainabilitySession = await _runtimeBrowserLiveSustainabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLiveSustainabilityReadyStateResult result = new()
        {
            ProfileId = livesustainabilitySession.ProfileId,
            SessionId = livesustainabilitySession.SessionId,
            SessionPath = livesustainabilitySession.SessionPath,
            BrowserLiveSustainabilitySessionVersion = livesustainabilitySession.BrowserLiveSustainabilitySessionVersion,
            BrowserSustainmentAssuranceReadyStateVersion = livesustainabilitySession.BrowserSustainmentAssuranceReadyStateVersion,
            BrowserSustainmentAssuranceSessionVersion = livesustainabilitySession.BrowserSustainmentAssuranceSessionVersion,
            LaunchMode = livesustainabilitySession.LaunchMode,
            AssetRootPath = livesustainabilitySession.AssetRootPath,
            ProfilesRootPath = livesustainabilitySession.ProfilesRootPath,
            CacheRootPath = livesustainabilitySession.CacheRootPath,
            ConfigRootPath = livesustainabilitySession.ConfigRootPath,
            SettingsFilePath = livesustainabilitySession.SettingsFilePath,
            StartupProfilePath = livesustainabilitySession.StartupProfilePath,
            RequiredAssets = livesustainabilitySession.RequiredAssets,
            ReadyAssetCount = livesustainabilitySession.ReadyAssetCount,
            CompletedSteps = livesustainabilitySession.CompletedSteps,
            TotalSteps = livesustainabilitySession.TotalSteps,
            Exists = livesustainabilitySession.Exists,
            ReadSucceeded = livesustainabilitySession.ReadSucceeded
        };

        if (!livesustainabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser livesustainability ready state blocked for profile '{livesustainabilitySession.ProfileId}'.";
            result.Error = livesustainabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveSustainabilityReadyStateVersion = "runtime-browser-livesustainability-ready-state-v1";
        result.BrowserLiveSustainabilityReadyChecks =
        [
            "browser-sustainmentassurance-ready-state-ready",
            "browser-livesustainability-session-ready",
            "browser-livesustainability-ready"
        ];
        result.BrowserLiveSustainabilityReadySummary = $"Runtime browser livesustainability ready state passed {result.BrowserLiveSustainabilityReadyChecks.Length} livesustainability readiness check(s) for profile '{livesustainabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livesustainability ready state ready for profile '{livesustainabilitySession.ProfileId}' with {result.BrowserLiveSustainabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveSustainabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSustainmentAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveSustainabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLiveSustainabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
