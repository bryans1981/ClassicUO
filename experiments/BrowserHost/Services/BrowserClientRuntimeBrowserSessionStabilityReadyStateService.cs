namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSessionStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionStabilityReadyStateService : IBrowserClientRuntimeBrowserSessionStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSessionStabilitySession _runtimeBrowserSessionStabilitySession;

    public BrowserClientRuntimeBrowserSessionStabilityReadyStateService(IBrowserClientRuntimeBrowserSessionStabilitySession runtimeBrowserSessionStabilitySession)
    {
        _runtimeBrowserSessionStabilitySession = runtimeBrowserSessionStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionStabilitySessionResult sessionstabilitySession = await _runtimeBrowserSessionStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult result = new()
        {
            ProfileId = sessionstabilitySession.ProfileId,
            SessionId = sessionstabilitySession.SessionId,
            SessionPath = sessionstabilitySession.SessionPath,
            BrowserSessionStabilitySessionVersion = sessionstabilitySession.BrowserSessionStabilitySessionVersion,
            BrowserRuntimeStabilityReadyStateVersion = sessionstabilitySession.BrowserRuntimeStabilityReadyStateVersion,
            BrowserRuntimeStabilitySessionVersion = sessionstabilitySession.BrowserRuntimeStabilitySessionVersion,
            LaunchMode = sessionstabilitySession.LaunchMode,
            AssetRootPath = sessionstabilitySession.AssetRootPath,
            ProfilesRootPath = sessionstabilitySession.ProfilesRootPath,
            CacheRootPath = sessionstabilitySession.CacheRootPath,
            ConfigRootPath = sessionstabilitySession.ConfigRootPath,
            SettingsFilePath = sessionstabilitySession.SettingsFilePath,
            StartupProfilePath = sessionstabilitySession.StartupProfilePath,
            RequiredAssets = sessionstabilitySession.RequiredAssets,
            ReadyAssetCount = sessionstabilitySession.ReadyAssetCount,
            CompletedSteps = sessionstabilitySession.CompletedSteps,
            TotalSteps = sessionstabilitySession.TotalSteps,
            Exists = sessionstabilitySession.Exists,
            ReadSucceeded = sessionstabilitySession.ReadSucceeded
        };

        if (!sessionstabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sessionstability ready state blocked for profile '{sessionstabilitySession.ProfileId}'.";
            result.Error = sessionstabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionStabilityReadyStateVersion = "runtime-browser-sessionstability-ready-state-v1";
        result.BrowserSessionStabilityReadyChecks =
        [
            "browser-runtimestability-ready-state-ready",
            "browser-sessionstability-session-ready",
            "browser-sessionstability-ready"
        ];
        result.BrowserSessionStabilityReadySummary = $"Runtime browser sessionstability ready state passed {result.BrowserSessionStabilityReadyChecks.Length} sessionstability readiness check(s) for profile '{sessionstabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessionstability ready state ready for profile '{sessionstabilitySession.ProfileId}' with {result.BrowserSessionStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSessionStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
