namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSupportabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSupportabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSupportabilityReadyStateService : IBrowserClientRuntimeBrowserSupportabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSupportabilitySession _runtimeBrowserSupportabilitySession;

    public BrowserClientRuntimeBrowserSupportabilityReadyStateService(IBrowserClientRuntimeBrowserSupportabilitySession runtimeBrowserSupportabilitySession)
    {
        _runtimeBrowserSupportabilitySession = runtimeBrowserSupportabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSupportabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSupportabilitySessionResult supportabilitySession = await _runtimeBrowserSupportabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSupportabilityReadyStateResult result = new()
        {
            ProfileId = supportabilitySession.ProfileId,
            SessionId = supportabilitySession.SessionId,
            SessionPath = supportabilitySession.SessionPath,
            BrowserSupportabilitySessionVersion = supportabilitySession.BrowserSupportabilitySessionVersion,
            BrowserMaintainabilityReadyStateVersion = supportabilitySession.BrowserMaintainabilityReadyStateVersion,
            BrowserMaintainabilitySessionVersion = supportabilitySession.BrowserMaintainabilitySessionVersion,
            LaunchMode = supportabilitySession.LaunchMode,
            AssetRootPath = supportabilitySession.AssetRootPath,
            ProfilesRootPath = supportabilitySession.ProfilesRootPath,
            CacheRootPath = supportabilitySession.CacheRootPath,
            ConfigRootPath = supportabilitySession.ConfigRootPath,
            SettingsFilePath = supportabilitySession.SettingsFilePath,
            StartupProfilePath = supportabilitySession.StartupProfilePath,
            RequiredAssets = supportabilitySession.RequiredAssets,
            ReadyAssetCount = supportabilitySession.ReadyAssetCount,
            CompletedSteps = supportabilitySession.CompletedSteps,
            TotalSteps = supportabilitySession.TotalSteps,
            Exists = supportabilitySession.Exists,
            ReadSucceeded = supportabilitySession.ReadSucceeded
        };

        if (!supportabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser supportability ready state blocked for profile '{supportabilitySession.ProfileId}'.";
            result.Error = supportabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSupportabilityReadyStateVersion = "runtime-browser-supportability-ready-state-v1";
        result.BrowserSupportabilityReadyChecks =
        [
            "browser-maintainability-ready-state-ready",
            "browser-supportability-session-ready",
            "browser-supportability-ready"
        ];
        result.BrowserSupportabilityReadySummary = $"Runtime browser supportability ready state passed {result.BrowserSupportabilityReadyChecks.Length} supportability readiness check(s) for profile '{supportabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser supportability ready state ready for profile '{supportabilitySession.ProfileId}' with {result.BrowserSupportabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSupportabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSupportabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSupportabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserMaintainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMaintainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSupportabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSupportabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
