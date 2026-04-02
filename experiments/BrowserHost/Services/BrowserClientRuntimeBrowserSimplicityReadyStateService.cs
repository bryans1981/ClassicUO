namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSimplicityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSimplicityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSimplicityReadyStateService : IBrowserClientRuntimeBrowserSimplicityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSimplicitySession _runtimeBrowserSimplicitySession;

    public BrowserClientRuntimeBrowserSimplicityReadyStateService(IBrowserClientRuntimeBrowserSimplicitySession runtimeBrowserSimplicitySession)
    {
        _runtimeBrowserSimplicitySession = runtimeBrowserSimplicitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSimplicityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSimplicitySessionResult simplicitySession = await _runtimeBrowserSimplicitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSimplicityReadyStateResult result = new()
        {
            ProfileId = simplicitySession.ProfileId,
            SessionId = simplicitySession.SessionId,
            SessionPath = simplicitySession.SessionPath,
            BrowserSimplicitySessionVersion = simplicitySession.BrowserSimplicitySessionVersion,
            BrowserLegibilityReadyStateVersion = simplicitySession.BrowserLegibilityReadyStateVersion,
            BrowserLegibilitySessionVersion = simplicitySession.BrowserLegibilitySessionVersion,
            LaunchMode = simplicitySession.LaunchMode,
            AssetRootPath = simplicitySession.AssetRootPath,
            ProfilesRootPath = simplicitySession.ProfilesRootPath,
            CacheRootPath = simplicitySession.CacheRootPath,
            ConfigRootPath = simplicitySession.ConfigRootPath,
            SettingsFilePath = simplicitySession.SettingsFilePath,
            StartupProfilePath = simplicitySession.StartupProfilePath,
            RequiredAssets = simplicitySession.RequiredAssets,
            ReadyAssetCount = simplicitySession.ReadyAssetCount,
            CompletedSteps = simplicitySession.CompletedSteps,
            TotalSteps = simplicitySession.TotalSteps,
            Exists = simplicitySession.Exists,
            ReadSucceeded = simplicitySession.ReadSucceeded
        };

        if (!simplicitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser simplicity ready state blocked for profile '{simplicitySession.ProfileId}'.";
            result.Error = simplicitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSimplicityReadyStateVersion = "runtime-browser-simplicity-ready-state-v1";
        result.BrowserSimplicityReadyChecks =
        [
            "browser-legibility-ready-state-ready",
            "browser-simplicity-session-ready",
            "browser-simplicity-ready"
        ];
        result.BrowserSimplicityReadySummary = $"Runtime browser simplicity ready state passed {result.BrowserSimplicityReadyChecks.Length} simplicity readiness check(s) for profile '{simplicitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser simplicity ready state ready for profile '{simplicitySession.ProfileId}' with {result.BrowserSimplicityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSimplicityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSimplicityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSimplicitySessionVersion { get; set; } = string.Empty;
    public string BrowserLegibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLegibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSimplicityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSimplicityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

