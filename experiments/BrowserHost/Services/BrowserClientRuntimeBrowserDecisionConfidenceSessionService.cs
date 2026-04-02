namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDecisionConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserDecisionConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDecisionConfidenceSessionService : IBrowserClientRuntimeBrowserDecisionConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserTaskConfidenceReadyState _runtimeBrowserTaskConfidenceReadyState;

    public BrowserClientRuntimeBrowserDecisionConfidenceSessionService(IBrowserClientRuntimeBrowserTaskConfidenceReadyState runtimeBrowserTaskConfidenceReadyState)
    {
        _runtimeBrowserTaskConfidenceReadyState = runtimeBrowserTaskConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDecisionConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTaskConfidenceReadyStateResult taskconfidenceReadyState = await _runtimeBrowserTaskConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDecisionConfidenceSessionResult result = new()
        {
            ProfileId = taskconfidenceReadyState.ProfileId,
            SessionId = taskconfidenceReadyState.SessionId,
            SessionPath = taskconfidenceReadyState.SessionPath,
            BrowserTaskConfidenceReadyStateVersion = taskconfidenceReadyState.BrowserTaskConfidenceReadyStateVersion,
            BrowserTaskConfidenceSessionVersion = taskconfidenceReadyState.BrowserTaskConfidenceSessionVersion,
            LaunchMode = taskconfidenceReadyState.LaunchMode,
            AssetRootPath = taskconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = taskconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = taskconfidenceReadyState.CacheRootPath,
            ConfigRootPath = taskconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = taskconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = taskconfidenceReadyState.StartupProfilePath,
            RequiredAssets = taskconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = taskconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = taskconfidenceReadyState.CompletedSteps,
            TotalSteps = taskconfidenceReadyState.TotalSteps,
            Exists = taskconfidenceReadyState.Exists,
            ReadSucceeded = taskconfidenceReadyState.ReadSucceeded
        };

        if (!taskconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser decisionconfidence session blocked for profile '{taskconfidenceReadyState.ProfileId}'.";
            result.Error = taskconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDecisionConfidenceSessionVersion = "runtime-browser-decisionconfidence-session-v1";
        result.BrowserDecisionConfidenceStages =
        [
            "open-browser-decisionconfidence-session",
            "bind-browser-taskconfidence-ready-state",
            "publish-browser-decisionconfidence-ready"
        ];
        result.BrowserDecisionConfidenceSummary = $"Runtime browser decisionconfidence session prepared {result.BrowserDecisionConfidenceStages.Length} decisionconfidence stage(s) for profile '{taskconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser decisionconfidence session ready for profile '{taskconfidenceReadyState.ProfileId}' with {result.BrowserDecisionConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDecisionConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDecisionConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserTaskConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDecisionConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserDecisionConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
