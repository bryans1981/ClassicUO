namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCohesivenessSession
{
    ValueTask<BrowserClientRuntimeBrowserCohesivenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCohesivenessSessionService : IBrowserClientRuntimeBrowserCohesivenessSession
{
    private readonly IBrowserClientRuntimeBrowserConsistencyReadyState _runtimeBrowserConsistencyReadyState;

    public BrowserClientRuntimeBrowserCohesivenessSessionService(IBrowserClientRuntimeBrowserConsistencyReadyState runtimeBrowserConsistencyReadyState)
    {
        _runtimeBrowserConsistencyReadyState = runtimeBrowserConsistencyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCohesivenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConsistencyReadyStateResult consistencyReadyState = await _runtimeBrowserConsistencyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCohesivenessSessionResult result = new()
        {
            ProfileId = consistencyReadyState.ProfileId,
            SessionId = consistencyReadyState.SessionId,
            SessionPath = consistencyReadyState.SessionPath,
            BrowserConsistencyReadyStateVersion = consistencyReadyState.BrowserConsistencyReadyStateVersion,
            BrowserConsistencySessionVersion = consistencyReadyState.BrowserConsistencySessionVersion,
            LaunchMode = consistencyReadyState.LaunchMode,
            AssetRootPath = consistencyReadyState.AssetRootPath,
            ProfilesRootPath = consistencyReadyState.ProfilesRootPath,
            CacheRootPath = consistencyReadyState.CacheRootPath,
            ConfigRootPath = consistencyReadyState.ConfigRootPath,
            SettingsFilePath = consistencyReadyState.SettingsFilePath,
            StartupProfilePath = consistencyReadyState.StartupProfilePath,
            RequiredAssets = consistencyReadyState.RequiredAssets,
            ReadyAssetCount = consistencyReadyState.ReadyAssetCount,
            CompletedSteps = consistencyReadyState.CompletedSteps,
            TotalSteps = consistencyReadyState.TotalSteps,
            Exists = consistencyReadyState.Exists,
            ReadSucceeded = consistencyReadyState.ReadSucceeded
        };

        if (!consistencyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser cohesiveness session blocked for profile '{consistencyReadyState.ProfileId}'.";
            result.Error = consistencyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCohesivenessSessionVersion = "runtime-browser-cohesiveness-session-v1";
        result.BrowserCohesivenessStages =
        [
            "open-browser-cohesiveness-session",
            "bind-browser-consistency-ready-state",
            "publish-browser-cohesiveness-ready"
        ];
        result.BrowserCohesivenessSummary = $"Runtime browser cohesiveness session prepared {result.BrowserCohesivenessStages.Length} cohesiveness stage(s) for profile '{consistencyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser cohesiveness session ready for profile '{consistencyReadyState.ProfileId}' with {result.BrowserCohesivenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCohesivenessSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserCohesivenessStages { get; set; } = Array.Empty<string>();
    public string BrowserCohesivenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

