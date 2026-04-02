namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMemorabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserMemorabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMemorabilityReadyStateService : IBrowserClientRuntimeBrowserMemorabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserMemorabilitySession _runtimeBrowserMemorabilitySession;

    public BrowserClientRuntimeBrowserMemorabilityReadyStateService(IBrowserClientRuntimeBrowserMemorabilitySession runtimeBrowserMemorabilitySession)
    {
        _runtimeBrowserMemorabilitySession = runtimeBrowserMemorabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMemorabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMemorabilitySessionResult memorabilitySession = await _runtimeBrowserMemorabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserMemorabilityReadyStateResult result = new()
        {
            ProfileId = memorabilitySession.ProfileId,
            SessionId = memorabilitySession.SessionId,
            SessionPath = memorabilitySession.SessionPath,
            BrowserMemorabilitySessionVersion = memorabilitySession.BrowserMemorabilitySessionVersion,
            BrowserComprehensibilityReadyStateVersion = memorabilitySession.BrowserComprehensibilityReadyStateVersion,
            BrowserComprehensibilitySessionVersion = memorabilitySession.BrowserComprehensibilitySessionVersion,
            LaunchMode = memorabilitySession.LaunchMode,
            AssetRootPath = memorabilitySession.AssetRootPath,
            ProfilesRootPath = memorabilitySession.ProfilesRootPath,
            CacheRootPath = memorabilitySession.CacheRootPath,
            ConfigRootPath = memorabilitySession.ConfigRootPath,
            SettingsFilePath = memorabilitySession.SettingsFilePath,
            StartupProfilePath = memorabilitySession.StartupProfilePath,
            RequiredAssets = memorabilitySession.RequiredAssets,
            ReadyAssetCount = memorabilitySession.ReadyAssetCount,
            CompletedSteps = memorabilitySession.CompletedSteps,
            TotalSteps = memorabilitySession.TotalSteps,
            Exists = memorabilitySession.Exists,
            ReadSucceeded = memorabilitySession.ReadSucceeded
        };

        if (!memorabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser memorability ready state blocked for profile '{memorabilitySession.ProfileId}'.";
            result.Error = memorabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMemorabilityReadyStateVersion = "runtime-browser-memorability-ready-state-v1";
        result.BrowserMemorabilityReadyChecks =
        [
            "browser-comprehensibility-ready-state-ready",
            "browser-memorability-session-ready",
            "browser-memorability-ready"
        ];
        result.BrowserMemorabilityReadySummary = $"Runtime browser memorability ready state passed {result.BrowserMemorabilityReadyChecks.Length} memorability readiness check(s) for profile '{memorabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser memorability ready state ready for profile '{memorabilitySession.ProfileId}' with {result.BrowserMemorabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMemorabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserMemorabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMemorabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserComprehensibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComprehensibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMemorabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserMemorabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
