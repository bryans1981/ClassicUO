namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRecoverabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRecoverabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRecoverabilityReadyStateService : IBrowserClientRuntimeBrowserRecoverabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserRecoverabilitySession _runtimeBrowserRecoverabilitySession;

    public BrowserClientRuntimeBrowserRecoverabilityReadyStateService(IBrowserClientRuntimeBrowserRecoverabilitySession runtimeBrowserRecoverabilitySession)
    {
        _runtimeBrowserRecoverabilitySession = runtimeBrowserRecoverabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRecoverabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRecoverabilitySessionResult recoverabilitySession = await _runtimeBrowserRecoverabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRecoverabilityReadyStateResult result = new()
        {
            ProfileId = recoverabilitySession.ProfileId,
            SessionId = recoverabilitySession.SessionId,
            SessionPath = recoverabilitySession.SessionPath,
            BrowserRecoverabilitySessionVersion = recoverabilitySession.BrowserRecoverabilitySessionVersion,
            BrowserPersonalizationReadyStateVersion = recoverabilitySession.BrowserPersonalizationReadyStateVersion,
            BrowserPersonalizationSessionVersion = recoverabilitySession.BrowserPersonalizationSessionVersion,
            LaunchMode = recoverabilitySession.LaunchMode,
            AssetRootPath = recoverabilitySession.AssetRootPath,
            ProfilesRootPath = recoverabilitySession.ProfilesRootPath,
            CacheRootPath = recoverabilitySession.CacheRootPath,
            ConfigRootPath = recoverabilitySession.ConfigRootPath,
            SettingsFilePath = recoverabilitySession.SettingsFilePath,
            StartupProfilePath = recoverabilitySession.StartupProfilePath,
            RequiredAssets = recoverabilitySession.RequiredAssets,
            ReadyAssetCount = recoverabilitySession.ReadyAssetCount,
            CompletedSteps = recoverabilitySession.CompletedSteps,
            TotalSteps = recoverabilitySession.TotalSteps,
            Exists = recoverabilitySession.Exists,
            ReadSucceeded = recoverabilitySession.ReadSucceeded
        };

        if (!recoverabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser recoverability ready state blocked for profile '{recoverabilitySession.ProfileId}'.";
            result.Error = recoverabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRecoverabilityReadyStateVersion = "runtime-browser-recoverability-ready-state-v1";
        result.BrowserRecoverabilityReadyChecks =
        [
            "browser-personalization-ready-state-ready",
            "browser-recoverability-session-ready",
            "browser-recoverability-ready"
        ];
        result.BrowserRecoverabilityReadySummary = $"Runtime browser recoverability ready state passed {result.BrowserRecoverabilityReadyChecks.Length} recoverability readiness check(s) for profile '{recoverabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser recoverability ready state ready for profile '{recoverabilitySession.ProfileId}' with {result.BrowserRecoverabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRecoverabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRecoverabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRecoverabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserPersonalizationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPersonalizationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRecoverabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRecoverabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
