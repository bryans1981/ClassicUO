namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLearnabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserLearnabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLearnabilityReadyStateService : IBrowserClientRuntimeBrowserLearnabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserLearnabilitySession _runtimeBrowserLearnabilitySession;

    public BrowserClientRuntimeBrowserLearnabilityReadyStateService(IBrowserClientRuntimeBrowserLearnabilitySession runtimeBrowserLearnabilitySession)
    {
        _runtimeBrowserLearnabilitySession = runtimeBrowserLearnabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLearnabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLearnabilitySessionResult learnabilitySession = await _runtimeBrowserLearnabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserLearnabilityReadyStateResult result = new()
        {
            ProfileId = learnabilitySession.ProfileId,
            SessionId = learnabilitySession.SessionId,
            SessionPath = learnabilitySession.SessionPath,
            BrowserLearnabilitySessionVersion = learnabilitySession.BrowserLearnabilitySessionVersion,
            BrowserDiscoverabilityReadyStateVersion = learnabilitySession.BrowserDiscoverabilityReadyStateVersion,
            BrowserDiscoverabilitySessionVersion = learnabilitySession.BrowserDiscoverabilitySessionVersion,
            LaunchMode = learnabilitySession.LaunchMode,
            AssetRootPath = learnabilitySession.AssetRootPath,
            ProfilesRootPath = learnabilitySession.ProfilesRootPath,
            CacheRootPath = learnabilitySession.CacheRootPath,
            ConfigRootPath = learnabilitySession.ConfigRootPath,
            SettingsFilePath = learnabilitySession.SettingsFilePath,
            StartupProfilePath = learnabilitySession.StartupProfilePath,
            RequiredAssets = learnabilitySession.RequiredAssets,
            ReadyAssetCount = learnabilitySession.ReadyAssetCount,
            CompletedSteps = learnabilitySession.CompletedSteps,
            TotalSteps = learnabilitySession.TotalSteps,
            Exists = learnabilitySession.Exists,
            ReadSucceeded = learnabilitySession.ReadSucceeded
        };

        if (!learnabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser learnability ready state blocked for profile '{learnabilitySession.ProfileId}'.";
            result.Error = learnabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLearnabilityReadyStateVersion = "runtime-browser-learnability-ready-state-v1";
        result.BrowserLearnabilityReadyChecks =
        [
            "browser-discoverability-ready-state-ready",
            "browser-learnability-session-ready",
            "browser-learnability-ready"
        ];
        result.BrowserLearnabilityReadySummary = $"Runtime browser learnability ready state passed {result.BrowserLearnabilityReadyChecks.Length} learnability readiness check(s) for profile '{learnabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser learnability ready state ready for profile '{learnabilitySession.ProfileId}' with {result.BrowserLearnabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLearnabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserLearnabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLearnabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDiscoverabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDiscoverabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLearnabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserLearnabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
