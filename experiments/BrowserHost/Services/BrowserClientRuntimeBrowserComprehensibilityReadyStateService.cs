namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComprehensibilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserComprehensibilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComprehensibilityReadyStateService : IBrowserClientRuntimeBrowserComprehensibilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserComprehensibilitySession _runtimeBrowserComprehensibilitySession;

    public BrowserClientRuntimeBrowserComprehensibilityReadyStateService(IBrowserClientRuntimeBrowserComprehensibilitySession runtimeBrowserComprehensibilitySession)
    {
        _runtimeBrowserComprehensibilitySession = runtimeBrowserComprehensibilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComprehensibilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComprehensibilitySessionResult comprehensibilitySession = await _runtimeBrowserComprehensibilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserComprehensibilityReadyStateResult result = new()
        {
            ProfileId = comprehensibilitySession.ProfileId,
            SessionId = comprehensibilitySession.SessionId,
            SessionPath = comprehensibilitySession.SessionPath,
            BrowserComprehensibilitySessionVersion = comprehensibilitySession.BrowserComprehensibilitySessionVersion,
            BrowserScannabilityReadyStateVersion = comprehensibilitySession.BrowserScannabilityReadyStateVersion,
            BrowserScannabilitySessionVersion = comprehensibilitySession.BrowserScannabilitySessionVersion,
            LaunchMode = comprehensibilitySession.LaunchMode,
            AssetRootPath = comprehensibilitySession.AssetRootPath,
            ProfilesRootPath = comprehensibilitySession.ProfilesRootPath,
            CacheRootPath = comprehensibilitySession.CacheRootPath,
            ConfigRootPath = comprehensibilitySession.ConfigRootPath,
            SettingsFilePath = comprehensibilitySession.SettingsFilePath,
            StartupProfilePath = comprehensibilitySession.StartupProfilePath,
            RequiredAssets = comprehensibilitySession.RequiredAssets,
            ReadyAssetCount = comprehensibilitySession.ReadyAssetCount,
            CompletedSteps = comprehensibilitySession.CompletedSteps,
            TotalSteps = comprehensibilitySession.TotalSteps,
            Exists = comprehensibilitySession.Exists,
            ReadSucceeded = comprehensibilitySession.ReadSucceeded
        };

        if (!comprehensibilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser comprehensibility ready state blocked for profile '{comprehensibilitySession.ProfileId}'.";
            result.Error = comprehensibilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComprehensibilityReadyStateVersion = "runtime-browser-comprehensibility-ready-state-v1";
        result.BrowserComprehensibilityReadyChecks =
        [
            "browser-scannability-ready-state-ready",
            "browser-comprehensibility-session-ready",
            "browser-comprehensibility-ready"
        ];
        result.BrowserComprehensibilityReadySummary = $"Runtime browser comprehensibility ready state passed {result.BrowserComprehensibilityReadyChecks.Length} comprehensibility readiness check(s) for profile '{comprehensibilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser comprehensibility ready state ready for profile '{comprehensibilitySession.ProfileId}' with {result.BrowserComprehensibilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComprehensibilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserComprehensibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComprehensibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserScannabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserScannabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComprehensibilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserComprehensibilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

