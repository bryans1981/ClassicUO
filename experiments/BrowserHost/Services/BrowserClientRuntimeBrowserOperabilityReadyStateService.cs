namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOperabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperabilityReadyStateService : IBrowserClientRuntimeBrowserOperabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserOperabilitySession _runtimeBrowserOperabilitySession;

    public BrowserClientRuntimeBrowserOperabilityReadyStateService(IBrowserClientRuntimeBrowserOperabilitySession runtimeBrowserOperabilitySession)
    {
        _runtimeBrowserOperabilitySession = runtimeBrowserOperabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperabilitySessionResult operabilitySession = await _runtimeBrowserOperabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOperabilityReadyStateResult result = new()
        {
            ProfileId = operabilitySession.ProfileId,
            SessionId = operabilitySession.SessionId,
            SessionPath = operabilitySession.SessionPath,
            BrowserOperabilitySessionVersion = operabilitySession.BrowserOperabilitySessionVersion,
            BrowserStewardshipReadyStateVersion = operabilitySession.BrowserStewardshipReadyStateVersion,
            BrowserStewardshipSessionVersion = operabilitySession.BrowserStewardshipSessionVersion,
            LaunchMode = operabilitySession.LaunchMode,
            AssetRootPath = operabilitySession.AssetRootPath,
            ProfilesRootPath = operabilitySession.ProfilesRootPath,
            CacheRootPath = operabilitySession.CacheRootPath,
            ConfigRootPath = operabilitySession.ConfigRootPath,
            SettingsFilePath = operabilitySession.SettingsFilePath,
            StartupProfilePath = operabilitySession.StartupProfilePath,
            RequiredAssets = operabilitySession.RequiredAssets,
            ReadyAssetCount = operabilitySession.ReadyAssetCount,
            CompletedSteps = operabilitySession.CompletedSteps,
            TotalSteps = operabilitySession.TotalSteps,
            Exists = operabilitySession.Exists,
            ReadSucceeded = operabilitySession.ReadSucceeded
        };

        if (!operabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operability ready state blocked for profile '{operabilitySession.ProfileId}'.";
            result.Error = operabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperabilityReadyStateVersion = "runtime-browser-operability-ready-state-v1";
        result.BrowserOperabilityReadyChecks =
        [
            "browser-stewardship-ready-state-ready",
            "browser-operability-session-ready",
            "browser-operability-ready"
        ];
        result.BrowserOperabilityReadySummary = $"Runtime browser operability ready state passed {result.BrowserOperabilityReadyChecks.Length} operability readiness check(s) for profile '{operabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operability ready state ready for profile '{operabilitySession.ProfileId}' with {result.BrowserOperabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserStewardshipReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStewardshipSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOperabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
