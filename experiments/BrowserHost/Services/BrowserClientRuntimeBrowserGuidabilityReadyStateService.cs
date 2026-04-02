namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGuidabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGuidabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGuidabilityReadyStateService : IBrowserClientRuntimeBrowserGuidabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserGuidabilitySession _runtimeBrowserGuidabilitySession;

    public BrowserClientRuntimeBrowserGuidabilityReadyStateService(IBrowserClientRuntimeBrowserGuidabilitySession runtimeBrowserGuidabilitySession)
    {
        _runtimeBrowserGuidabilitySession = runtimeBrowserGuidabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGuidabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGuidabilitySessionResult guidabilitySession = await _runtimeBrowserGuidabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGuidabilityReadyStateResult result = new()
        {
            ProfileId = guidabilitySession.ProfileId,
            SessionId = guidabilitySession.SessionId,
            SessionPath = guidabilitySession.SessionPath,
            BrowserGuidabilitySessionVersion = guidabilitySession.BrowserGuidabilitySessionVersion,
            BrowserNavigabilityReadyStateVersion = guidabilitySession.BrowserNavigabilityReadyStateVersion,
            BrowserNavigabilitySessionVersion = guidabilitySession.BrowserNavigabilitySessionVersion,
            LaunchMode = guidabilitySession.LaunchMode,
            AssetRootPath = guidabilitySession.AssetRootPath,
            ProfilesRootPath = guidabilitySession.ProfilesRootPath,
            CacheRootPath = guidabilitySession.CacheRootPath,
            ConfigRootPath = guidabilitySession.ConfigRootPath,
            SettingsFilePath = guidabilitySession.SettingsFilePath,
            StartupProfilePath = guidabilitySession.StartupProfilePath,
            RequiredAssets = guidabilitySession.RequiredAssets,
            ReadyAssetCount = guidabilitySession.ReadyAssetCount,
            CompletedSteps = guidabilitySession.CompletedSteps,
            TotalSteps = guidabilitySession.TotalSteps,
            Exists = guidabilitySession.Exists,
            ReadSucceeded = guidabilitySession.ReadSucceeded
        };

        if (!guidabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser guidability ready state blocked for profile '{guidabilitySession.ProfileId}'.";
            result.Error = guidabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGuidabilityReadyStateVersion = "runtime-browser-guidability-ready-state-v1";
        result.BrowserGuidabilityReadyChecks =
        [
            "browser-navigability-ready-state-ready",
            "browser-guidability-session-ready",
            "browser-guidability-ready"
        ];
        result.BrowserGuidabilityReadySummary = $"Runtime browser guidability ready state passed {result.BrowserGuidabilityReadyChecks.Length} guidability readiness check(s) for profile '{guidabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser guidability ready state ready for profile '{guidabilitySession.ProfileId}' with {result.BrowserGuidabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGuidabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGuidabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGuidabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserNavigabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserNavigabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGuidabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGuidabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

