namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTrustworthinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTrustworthinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTrustworthinessReadyStateService : IBrowserClientRuntimeBrowserTrustworthinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserTrustworthinessSession _runtimeBrowserTrustworthinessSession;

    public BrowserClientRuntimeBrowserTrustworthinessReadyStateService(IBrowserClientRuntimeBrowserTrustworthinessSession runtimeBrowserTrustworthinessSession)
    {
        _runtimeBrowserTrustworthinessSession = runtimeBrowserTrustworthinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTrustworthinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTrustworthinessSessionResult trustworthinessSession = await _runtimeBrowserTrustworthinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTrustworthinessReadyStateResult result = new()
        {
            ProfileId = trustworthinessSession.ProfileId,
            SessionId = trustworthinessSession.SessionId,
            SessionPath = trustworthinessSession.SessionPath,
            BrowserTrustworthinessSessionVersion = trustworthinessSession.BrowserTrustworthinessSessionVersion,
            BrowserReliabilityReadyStateVersion = trustworthinessSession.BrowserReliabilityReadyStateVersion,
            BrowserReliabilitySessionVersion = trustworthinessSession.BrowserReliabilitySessionVersion,
            LaunchMode = trustworthinessSession.LaunchMode,
            AssetRootPath = trustworthinessSession.AssetRootPath,
            ProfilesRootPath = trustworthinessSession.ProfilesRootPath,
            CacheRootPath = trustworthinessSession.CacheRootPath,
            ConfigRootPath = trustworthinessSession.ConfigRootPath,
            SettingsFilePath = trustworthinessSession.SettingsFilePath,
            StartupProfilePath = trustworthinessSession.StartupProfilePath,
            RequiredAssets = trustworthinessSession.RequiredAssets,
            ReadyAssetCount = trustworthinessSession.ReadyAssetCount,
            CompletedSteps = trustworthinessSession.CompletedSteps,
            TotalSteps = trustworthinessSession.TotalSteps,
            Exists = trustworthinessSession.Exists,
            ReadSucceeded = trustworthinessSession.ReadSucceeded
        };

        if (!trustworthinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser trustworthiness ready state blocked for profile '{trustworthinessSession.ProfileId}'.";
            result.Error = trustworthinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTrustworthinessReadyStateVersion = "runtime-browser-trustworthiness-ready-state-v1";
        result.BrowserTrustworthinessReadyChecks =
        [
            "browser-reliability-ready-state-ready",
            "browser-trustworthiness-session-ready",
            "browser-trustworthiness-ready"
        ];
        result.BrowserTrustworthinessReadySummary = $"Runtime browser trustworthiness ready state passed {result.BrowserTrustworthinessReadyChecks.Length} trustworthiness readiness check(s) for profile '{trustworthinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser trustworthiness ready state ready for profile '{trustworthinessSession.ProfileId}' with {result.BrowserTrustworthinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTrustworthinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTrustworthinessReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserTrustworthinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTrustworthinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
