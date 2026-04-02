namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFamiliarityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFamiliarityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFamiliarityReadyStateService : IBrowserClientRuntimeBrowserFamiliarityReadyState
{
    private readonly IBrowserClientRuntimeBrowserFamiliaritySession _runtimeBrowserFamiliaritySession;

    public BrowserClientRuntimeBrowserFamiliarityReadyStateService(IBrowserClientRuntimeBrowserFamiliaritySession runtimeBrowserFamiliaritySession)
    {
        _runtimeBrowserFamiliaritySession = runtimeBrowserFamiliaritySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFamiliarityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFamiliaritySessionResult familiaritySession = await _runtimeBrowserFamiliaritySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFamiliarityReadyStateResult result = new()
        {
            ProfileId = familiaritySession.ProfileId,
            SessionId = familiaritySession.SessionId,
            SessionPath = familiaritySession.SessionPath,
            BrowserFamiliaritySessionVersion = familiaritySession.BrowserFamiliaritySessionVersion,
            BrowserPredictabilityReadyStateVersion = familiaritySession.BrowserPredictabilityReadyStateVersion,
            BrowserPredictabilitySessionVersion = familiaritySession.BrowserPredictabilitySessionVersion,
            LaunchMode = familiaritySession.LaunchMode,
            AssetRootPath = familiaritySession.AssetRootPath,
            ProfilesRootPath = familiaritySession.ProfilesRootPath,
            CacheRootPath = familiaritySession.CacheRootPath,
            ConfigRootPath = familiaritySession.ConfigRootPath,
            SettingsFilePath = familiaritySession.SettingsFilePath,
            StartupProfilePath = familiaritySession.StartupProfilePath,
            RequiredAssets = familiaritySession.RequiredAssets,
            ReadyAssetCount = familiaritySession.ReadyAssetCount,
            CompletedSteps = familiaritySession.CompletedSteps,
            TotalSteps = familiaritySession.TotalSteps,
            Exists = familiaritySession.Exists,
            ReadSucceeded = familiaritySession.ReadSucceeded
        };

        if (!familiaritySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser familiarity ready state blocked for profile '{familiaritySession.ProfileId}'.";
            result.Error = familiaritySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFamiliarityReadyStateVersion = "runtime-browser-familiarity-ready-state-v1";
        result.BrowserFamiliarityReadyChecks =
        [
            "browser-predictability-ready-state-ready",
            "browser-familiarity-session-ready",
            "browser-familiarity-ready"
        ];
        result.BrowserFamiliarityReadySummary = $"Runtime browser familiarity ready state passed {result.BrowserFamiliarityReadyChecks.Length} familiarity readiness check(s) for profile '{familiaritySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser familiarity ready state ready for profile '{familiaritySession.ProfileId}' with {result.BrowserFamiliarityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFamiliarityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFamiliarityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFamiliaritySessionVersion { get; set; } = string.Empty;
    public string BrowserPredictabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPredictabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFamiliarityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFamiliarityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

