namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserStateStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateStabilityReadyStateService : IBrowserClientRuntimeBrowserStateStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserStateStabilitySession _runtimeBrowserStateStabilitySession;

    public BrowserClientRuntimeBrowserStateStabilityReadyStateService(IBrowserClientRuntimeBrowserStateStabilitySession runtimeBrowserStateStabilitySession)
    {
        _runtimeBrowserStateStabilitySession = runtimeBrowserStateStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateStabilitySessionResult statestabilitySession = await _runtimeBrowserStateStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserStateStabilityReadyStateResult result = new()
        {
            ProfileId = statestabilitySession.ProfileId,
            SessionId = statestabilitySession.SessionId,
            SessionPath = statestabilitySession.SessionPath,
            BrowserStateStabilitySessionVersion = statestabilitySession.BrowserStateStabilitySessionVersion,
            BrowserSessionStabilityReadyStateVersion = statestabilitySession.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySessionVersion = statestabilitySession.BrowserSessionStabilitySessionVersion,
            LaunchMode = statestabilitySession.LaunchMode,
            AssetRootPath = statestabilitySession.AssetRootPath,
            ProfilesRootPath = statestabilitySession.ProfilesRootPath,
            CacheRootPath = statestabilitySession.CacheRootPath,
            ConfigRootPath = statestabilitySession.ConfigRootPath,
            SettingsFilePath = statestabilitySession.SettingsFilePath,
            StartupProfilePath = statestabilitySession.StartupProfilePath,
            RequiredAssets = statestabilitySession.RequiredAssets,
            ReadyAssetCount = statestabilitySession.ReadyAssetCount,
            CompletedSteps = statestabilitySession.CompletedSteps,
            TotalSteps = statestabilitySession.TotalSteps,
            Exists = statestabilitySession.Exists,
            ReadSucceeded = statestabilitySession.ReadSucceeded
        };

        if (!statestabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser statestability ready state blocked for profile '{statestabilitySession.ProfileId}'.";
            result.Error = statestabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateStabilityReadyStateVersion = "runtime-browser-statestability-ready-state-v1";
        result.BrowserStateStabilityReadyChecks =
        [
            "browser-sessionstability-ready-state-ready",
            "browser-statestability-session-ready",
            "browser-statestability-ready"
        ];
        result.BrowserStateStabilityReadySummary = $"Runtime browser statestability ready state passed {result.BrowserStateStabilityReadyChecks.Length} statestability readiness check(s) for profile '{statestabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser statestability ready state ready for profile '{statestabilitySession.ProfileId}' with {result.BrowserStateStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserStateStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserStateStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
