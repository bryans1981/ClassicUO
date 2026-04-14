namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceDurabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceDurabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceDurabilityReadyStateService : IBrowserClientRuntimeBrowserServiceDurabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceDurabilitySession _runtimeBrowserServiceDurabilitySession;

    public BrowserClientRuntimeBrowserServiceDurabilityReadyStateService(IBrowserClientRuntimeBrowserServiceDurabilitySession runtimeBrowserServiceDurabilitySession)
    {
        _runtimeBrowserServiceDurabilitySession = runtimeBrowserServiceDurabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceDurabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceDurabilitySessionResult servicedurabilitySession = await _runtimeBrowserServiceDurabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserServiceDurabilityReadyStateResult result = new()
        {
            ProfileId = servicedurabilitySession.ProfileId,
            SessionId = servicedurabilitySession.SessionId,
            SessionPath = servicedurabilitySession.SessionPath,
            BrowserServiceDurabilitySessionVersion = servicedurabilitySession.BrowserServiceDurabilitySessionVersion,
            BrowserRuntimeDurabilityReadyStateVersion = servicedurabilitySession.BrowserRuntimeDurabilityReadyStateVersion,
            BrowserRuntimeDurabilitySessionVersion = servicedurabilitySession.BrowserRuntimeDurabilitySessionVersion,
            LaunchMode = servicedurabilitySession.LaunchMode,
            AssetRootPath = servicedurabilitySession.AssetRootPath,
            ProfilesRootPath = servicedurabilitySession.ProfilesRootPath,
            CacheRootPath = servicedurabilitySession.CacheRootPath,
            ConfigRootPath = servicedurabilitySession.ConfigRootPath,
            SettingsFilePath = servicedurabilitySession.SettingsFilePath,
            StartupProfilePath = servicedurabilitySession.StartupProfilePath,
            RequiredAssets = servicedurabilitySession.RequiredAssets,
            ReadyAssetCount = servicedurabilitySession.ReadyAssetCount,
            CompletedSteps = servicedurabilitySession.CompletedSteps,
            TotalSteps = servicedurabilitySession.TotalSteps,
            Exists = servicedurabilitySession.Exists,
            ReadSucceeded = servicedurabilitySession.ReadSucceeded
        };

        if (!servicedurabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser servicedurability ready state blocked for profile '{servicedurabilitySession.ProfileId}'.";
            result.Error = servicedurabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceDurabilityReadyStateVersion = "runtime-browser-servicedurability-ready-state-v1";
        result.BrowserServiceDurabilityReadyChecks =
        [
            "browser-runtimedurability-ready-state-ready",
            "browser-servicedurability-session-ready",
            "browser-servicedurability-ready"
        ];
        result.BrowserServiceDurabilityReadySummary = $"Runtime browser servicedurability ready state passed {result.BrowserServiceDurabilityReadyChecks.Length} servicedurability readiness check(s) for profile '{servicedurabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser servicedurability ready state ready for profile '{servicedurabilitySession.ProfileId}' with {result.BrowserServiceDurabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceDurabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceDurabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceDurabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
