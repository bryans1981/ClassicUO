namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceabilityReadyStateService : IBrowserClientRuntimeBrowserServiceabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceabilitySession _runtimeBrowserServiceabilitySession;

    public BrowserClientRuntimeBrowserServiceabilityReadyStateService(IBrowserClientRuntimeBrowserServiceabilitySession runtimeBrowserServiceabilitySession)
    {
        _runtimeBrowserServiceabilitySession = runtimeBrowserServiceabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceabilitySessionResult serviceabilitySession = await _runtimeBrowserServiceabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserServiceabilityReadyStateResult result = new()
        {
            ProfileId = serviceabilitySession.ProfileId,
            SessionId = serviceabilitySession.SessionId,
            SessionPath = serviceabilitySession.SessionPath,
            BrowserServiceabilitySessionVersion = serviceabilitySession.BrowserServiceabilitySessionVersion,
            BrowserOperabilityReadyStateVersion = serviceabilitySession.BrowserOperabilityReadyStateVersion,
            BrowserOperabilitySessionVersion = serviceabilitySession.BrowserOperabilitySessionVersion,
            LaunchMode = serviceabilitySession.LaunchMode,
            AssetRootPath = serviceabilitySession.AssetRootPath,
            ProfilesRootPath = serviceabilitySession.ProfilesRootPath,
            CacheRootPath = serviceabilitySession.CacheRootPath,
            ConfigRootPath = serviceabilitySession.ConfigRootPath,
            SettingsFilePath = serviceabilitySession.SettingsFilePath,
            StartupProfilePath = serviceabilitySession.StartupProfilePath,
            RequiredAssets = serviceabilitySession.RequiredAssets,
            ReadyAssetCount = serviceabilitySession.ReadyAssetCount,
            CompletedSteps = serviceabilitySession.CompletedSteps,
            TotalSteps = serviceabilitySession.TotalSteps,
            Exists = serviceabilitySession.Exists,
            ReadSucceeded = serviceabilitySession.ReadSucceeded
        };

        if (!serviceabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceability ready state blocked for profile '{serviceabilitySession.ProfileId}'.";
            result.Error = serviceabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceabilityReadyStateVersion = "runtime-browser-serviceability-ready-state-v1";
        result.BrowserServiceabilityReadyChecks =
        [
            "browser-operability-ready-state-ready",
            "browser-serviceability-session-ready",
            "browser-serviceability-ready"
        ];
        result.BrowserServiceabilityReadySummary = $"Runtime browser serviceability ready state passed {result.BrowserServiceabilityReadyChecks.Length} serviceability readiness check(s) for profile '{serviceabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceability ready state ready for profile '{serviceabilitySession.ProfileId}' with {result.BrowserServiceabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
