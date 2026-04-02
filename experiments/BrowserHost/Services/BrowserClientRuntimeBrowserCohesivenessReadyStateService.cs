namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCohesivenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCohesivenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCohesivenessReadyStateService : IBrowserClientRuntimeBrowserCohesivenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserCohesivenessSession _runtimeBrowserCohesivenessSession;

    public BrowserClientRuntimeBrowserCohesivenessReadyStateService(IBrowserClientRuntimeBrowserCohesivenessSession runtimeBrowserCohesivenessSession)
    {
        _runtimeBrowserCohesivenessSession = runtimeBrowserCohesivenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCohesivenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCohesivenessSessionResult cohesivenessSession = await _runtimeBrowserCohesivenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCohesivenessReadyStateResult result = new()
        {
            ProfileId = cohesivenessSession.ProfileId,
            SessionId = cohesivenessSession.SessionId,
            SessionPath = cohesivenessSession.SessionPath,
            BrowserCohesivenessSessionVersion = cohesivenessSession.BrowserCohesivenessSessionVersion,
            BrowserConsistencyReadyStateVersion = cohesivenessSession.BrowserConsistencyReadyStateVersion,
            BrowserConsistencySessionVersion = cohesivenessSession.BrowserConsistencySessionVersion,
            LaunchMode = cohesivenessSession.LaunchMode,
            AssetRootPath = cohesivenessSession.AssetRootPath,
            ProfilesRootPath = cohesivenessSession.ProfilesRootPath,
            CacheRootPath = cohesivenessSession.CacheRootPath,
            ConfigRootPath = cohesivenessSession.ConfigRootPath,
            SettingsFilePath = cohesivenessSession.SettingsFilePath,
            StartupProfilePath = cohesivenessSession.StartupProfilePath,
            RequiredAssets = cohesivenessSession.RequiredAssets,
            ReadyAssetCount = cohesivenessSession.ReadyAssetCount,
            CompletedSteps = cohesivenessSession.CompletedSteps,
            TotalSteps = cohesivenessSession.TotalSteps,
            Exists = cohesivenessSession.Exists,
            ReadSucceeded = cohesivenessSession.ReadSucceeded
        };

        if (!cohesivenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser cohesiveness ready state blocked for profile '{cohesivenessSession.ProfileId}'.";
            result.Error = cohesivenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCohesivenessReadyStateVersion = "runtime-browser-cohesiveness-ready-state-v1";
        result.BrowserCohesivenessReadyChecks =
        [
            "browser-consistency-ready-state-ready",
            "browser-cohesiveness-session-ready",
            "browser-cohesiveness-ready"
        ];
        result.BrowserCohesivenessReadySummary = $"Runtime browser cohesiveness ready state passed {result.BrowserCohesivenessReadyChecks.Length} cohesiveness readiness check(s) for profile '{cohesivenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser cohesiveness ready state ready for profile '{cohesivenessSession.ProfileId}' with {result.BrowserCohesivenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCohesivenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCohesivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCohesivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserConsistencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConsistencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCohesivenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCohesivenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

