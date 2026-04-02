namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTrustworthinessSession
{
    ValueTask<BrowserClientRuntimeBrowserTrustworthinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTrustworthinessSessionService : IBrowserClientRuntimeBrowserTrustworthinessSession
{
    private readonly IBrowserClientRuntimeBrowserReliabilityReadyState _runtimeBrowserReliabilityReadyState;

    public BrowserClientRuntimeBrowserTrustworthinessSessionService(IBrowserClientRuntimeBrowserReliabilityReadyState runtimeBrowserReliabilityReadyState)
    {
        _runtimeBrowserReliabilityReadyState = runtimeBrowserReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTrustworthinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReliabilityReadyStateResult reliabilityReadyState = await _runtimeBrowserReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTrustworthinessSessionResult result = new()
        {
            ProfileId = reliabilityReadyState.ProfileId,
            SessionId = reliabilityReadyState.SessionId,
            SessionPath = reliabilityReadyState.SessionPath,
            BrowserReliabilityReadyStateVersion = reliabilityReadyState.BrowserReliabilityReadyStateVersion,
            BrowserReliabilitySessionVersion = reliabilityReadyState.BrowserReliabilitySessionVersion,
            LaunchMode = reliabilityReadyState.LaunchMode,
            AssetRootPath = reliabilityReadyState.AssetRootPath,
            ProfilesRootPath = reliabilityReadyState.ProfilesRootPath,
            CacheRootPath = reliabilityReadyState.CacheRootPath,
            ConfigRootPath = reliabilityReadyState.ConfigRootPath,
            SettingsFilePath = reliabilityReadyState.SettingsFilePath,
            StartupProfilePath = reliabilityReadyState.StartupProfilePath,
            RequiredAssets = reliabilityReadyState.RequiredAssets,
            ReadyAssetCount = reliabilityReadyState.ReadyAssetCount,
            CompletedSteps = reliabilityReadyState.CompletedSteps,
            TotalSteps = reliabilityReadyState.TotalSteps,
            Exists = reliabilityReadyState.Exists,
            ReadSucceeded = reliabilityReadyState.ReadSucceeded
        };

        if (!reliabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser trustworthiness session blocked for profile '{reliabilityReadyState.ProfileId}'.";
            result.Error = reliabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTrustworthinessSessionVersion = "runtime-browser-trustworthiness-session-v1";
        result.BrowserTrustworthinessStages =
        [
            "open-browser-trustworthiness-session",
            "bind-browser-reliability-ready-state",
            "publish-browser-trustworthiness-ready"
        ];
        result.BrowserTrustworthinessSummary = $"Runtime browser trustworthiness session prepared {result.BrowserTrustworthinessStages.Length} trustworthiness stage(s) for profile '{reliabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser trustworthiness session ready for profile '{reliabilityReadyState.ProfileId}' with {result.BrowserTrustworthinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTrustworthinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTrustworthinessSessionVersion { get; set; } = string.Empty;
    public string BrowserReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTrustworthinessStages { get; set; } = Array.Empty<string>();
    public string BrowserTrustworthinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
