namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMaintainabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserMaintainabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMaintainabilityReadyStateService : IBrowserClientRuntimeBrowserMaintainabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserMaintainabilitySession _runtimeBrowserMaintainabilitySession;

    public BrowserClientRuntimeBrowserMaintainabilityReadyStateService(IBrowserClientRuntimeBrowserMaintainabilitySession runtimeBrowserMaintainabilitySession)
    {
        _runtimeBrowserMaintainabilitySession = runtimeBrowserMaintainabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMaintainabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMaintainabilitySessionResult maintainabilitySession = await _runtimeBrowserMaintainabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserMaintainabilityReadyStateResult result = new()
        {
            ProfileId = maintainabilitySession.ProfileId,
            SessionId = maintainabilitySession.SessionId,
            SessionPath = maintainabilitySession.SessionPath,
            BrowserMaintainabilitySessionVersion = maintainabilitySession.BrowserMaintainabilitySessionVersion,
            BrowserServiceabilityReadyStateVersion = maintainabilitySession.BrowserServiceabilityReadyStateVersion,
            BrowserServiceabilitySessionVersion = maintainabilitySession.BrowserServiceabilitySessionVersion,
            LaunchMode = maintainabilitySession.LaunchMode,
            AssetRootPath = maintainabilitySession.AssetRootPath,
            ProfilesRootPath = maintainabilitySession.ProfilesRootPath,
            CacheRootPath = maintainabilitySession.CacheRootPath,
            ConfigRootPath = maintainabilitySession.ConfigRootPath,
            SettingsFilePath = maintainabilitySession.SettingsFilePath,
            StartupProfilePath = maintainabilitySession.StartupProfilePath,
            RequiredAssets = maintainabilitySession.RequiredAssets,
            ReadyAssetCount = maintainabilitySession.ReadyAssetCount,
            CompletedSteps = maintainabilitySession.CompletedSteps,
            TotalSteps = maintainabilitySession.TotalSteps,
            Exists = maintainabilitySession.Exists,
            ReadSucceeded = maintainabilitySession.ReadSucceeded
        };

        if (!maintainabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser maintainability ready state blocked for profile '{maintainabilitySession.ProfileId}'.";
            result.Error = maintainabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMaintainabilityReadyStateVersion = "runtime-browser-maintainability-ready-state-v1";
        result.BrowserMaintainabilityReadyChecks =
        [
            "browser-serviceability-ready-state-ready",
            "browser-maintainability-session-ready",
            "browser-maintainability-ready"
        ];
        result.BrowserMaintainabilityReadySummary = $"Runtime browser maintainability ready state passed {result.BrowserMaintainabilityReadyChecks.Length} maintainability readiness check(s) for profile '{maintainabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser maintainability ready state ready for profile '{maintainabilitySession.ProfileId}' with {result.BrowserMaintainabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMaintainabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserMaintainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMaintainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMaintainabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserMaintainabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
