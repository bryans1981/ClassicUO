namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDirectionalityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDirectionalityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDirectionalityReadyStateService : IBrowserClientRuntimeBrowserDirectionalityReadyState
{
    private readonly IBrowserClientRuntimeBrowserDirectionalitySession _runtimeBrowserDirectionalitySession;

    public BrowserClientRuntimeBrowserDirectionalityReadyStateService(IBrowserClientRuntimeBrowserDirectionalitySession runtimeBrowserDirectionalitySession)
    {
        _runtimeBrowserDirectionalitySession = runtimeBrowserDirectionalitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDirectionalityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDirectionalitySessionResult directionalitySession = await _runtimeBrowserDirectionalitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDirectionalityReadyStateResult result = new()
        {
            ProfileId = directionalitySession.ProfileId,
            SessionId = directionalitySession.SessionId,
            SessionPath = directionalitySession.SessionPath,
            BrowserDirectionalitySessionVersion = directionalitySession.BrowserDirectionalitySessionVersion,
            BrowserObservabilityReadyStateVersion = directionalitySession.BrowserObservabilityReadyStateVersion,
            BrowserObservabilitySessionVersion = directionalitySession.BrowserObservabilitySessionVersion,
            LaunchMode = directionalitySession.LaunchMode,
            AssetRootPath = directionalitySession.AssetRootPath,
            ProfilesRootPath = directionalitySession.ProfilesRootPath,
            CacheRootPath = directionalitySession.CacheRootPath,
            ConfigRootPath = directionalitySession.ConfigRootPath,
            SettingsFilePath = directionalitySession.SettingsFilePath,
            StartupProfilePath = directionalitySession.StartupProfilePath,
            RequiredAssets = directionalitySession.RequiredAssets,
            ReadyAssetCount = directionalitySession.ReadyAssetCount,
            CompletedSteps = directionalitySession.CompletedSteps,
            TotalSteps = directionalitySession.TotalSteps,
            Exists = directionalitySession.Exists,
            ReadSucceeded = directionalitySession.ReadSucceeded
        };

        if (!directionalitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser directionality ready state blocked for profile '{directionalitySession.ProfileId}'.";
            result.Error = directionalitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDirectionalityReadyStateVersion = "runtime-browser-directionality-ready-state-v1";
        result.BrowserDirectionalityReadyChecks =
        [
            "browser-observability-ready-state-ready",
            "browser-directionality-session-ready",
            "browser-directionality-ready"
        ];
        result.BrowserDirectionalityReadySummary = $"Runtime browser directionality ready state passed {result.BrowserDirectionalityReadyChecks.Length} directionality readiness check(s) for profile '{directionalitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser directionality ready state ready for profile '{directionalitySession.ProfileId}' with {result.BrowserDirectionalityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDirectionalityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDirectionalityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDirectionalitySessionVersion { get; set; } = string.Empty;
    public string BrowserObservabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserObservabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDirectionalityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDirectionalityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
