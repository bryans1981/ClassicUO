namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLegibilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLegibilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLegibilityReadyStateService : IBrowserClientRuntimeBrowserLegibilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLegibilitySession _runtimeBrowserLegibilitySession;

    public BrowserClientRuntimeBrowserLegibilityReadyStateService(IBrowserClientRuntimeBrowserLegibilitySession runtimeBrowserLegibilitySession)
    {
        _runtimeBrowserLegibilitySession = runtimeBrowserLegibilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLegibilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLegibilitySessionResult legibilitySession = await _runtimeBrowserLegibilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLegibilityReadyStateResult result = new()
        {
            ProfileId = legibilitySession.ProfileId,
            SessionId = legibilitySession.SessionId,
            SessionPath = legibilitySession.SessionPath,
            BrowserLegibilitySessionVersion = legibilitySession.BrowserLegibilitySessionVersion,
            BrowserReadabilityReadyStateVersion = legibilitySession.BrowserReadabilityReadyStateVersion,
            BrowserReadabilitySessionVersion = legibilitySession.BrowserReadabilitySessionVersion,
            LaunchMode = legibilitySession.LaunchMode,
            AssetRootPath = legibilitySession.AssetRootPath,
            ProfilesRootPath = legibilitySession.ProfilesRootPath,
            CacheRootPath = legibilitySession.CacheRootPath,
            ConfigRootPath = legibilitySession.ConfigRootPath,
            SettingsFilePath = legibilitySession.SettingsFilePath,
            StartupProfilePath = legibilitySession.StartupProfilePath,
            RequiredAssets = legibilitySession.RequiredAssets,
            ReadyAssetCount = legibilitySession.ReadyAssetCount,
            CompletedSteps = legibilitySession.CompletedSteps,
            TotalSteps = legibilitySession.TotalSteps,
            Exists = legibilitySession.Exists,
            ReadSucceeded = legibilitySession.ReadSucceeded
        };

        if (!legibilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser legibility ready state blocked for profile '{legibilitySession.ProfileId}'.";
            result.Error = legibilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLegibilityReadyStateVersion = "runtime-browser-legibility-ready-state-v1";
        result.BrowserLegibilityReadyChecks =
        [
            "browser-readability-ready-state-ready",
            "browser-legibility-session-ready",
            "browser-legibility-ready"
        ];
        result.BrowserLegibilityReadySummary = $"Runtime browser legibility ready state passed {result.BrowserLegibilityReadyChecks.Length} legibility readiness check(s) for profile '{legibilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser legibility ready state ready for profile '{legibilitySession.ProfileId}' with {result.BrowserLegibilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLegibilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLegibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLegibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserReadabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReadabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLegibilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLegibilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

