namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReassuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserReassuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReassuranceSessionService : IBrowserClientRuntimeBrowserReassuranceSession
{
    private readonly IBrowserClientRuntimeBrowserClosureConfidenceReadyState _runtimeBrowserClosureConfidenceReadyState;

    public BrowserClientRuntimeBrowserReassuranceSessionService(IBrowserClientRuntimeBrowserClosureConfidenceReadyState runtimeBrowserClosureConfidenceReadyState)
    {
        _runtimeBrowserClosureConfidenceReadyState = runtimeBrowserClosureConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReassuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserClosureConfidenceReadyStateResult closureconfidenceReadyState = await _runtimeBrowserClosureConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReassuranceSessionResult result = new()
        {
            ProfileId = closureconfidenceReadyState.ProfileId,
            SessionId = closureconfidenceReadyState.SessionId,
            SessionPath = closureconfidenceReadyState.SessionPath,
            BrowserClosureConfidenceReadyStateVersion = closureconfidenceReadyState.BrowserClosureConfidenceReadyStateVersion,
            BrowserClosureConfidenceSessionVersion = closureconfidenceReadyState.BrowserClosureConfidenceSessionVersion,
            LaunchMode = closureconfidenceReadyState.LaunchMode,
            AssetRootPath = closureconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = closureconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = closureconfidenceReadyState.CacheRootPath,
            ConfigRootPath = closureconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = closureconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = closureconfidenceReadyState.StartupProfilePath,
            RequiredAssets = closureconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = closureconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = closureconfidenceReadyState.CompletedSteps,
            TotalSteps = closureconfidenceReadyState.TotalSteps,
            Exists = closureconfidenceReadyState.Exists,
            ReadSucceeded = closureconfidenceReadyState.ReadSucceeded
        };

        if (!closureconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser reassurance session blocked for profile '{closureconfidenceReadyState.ProfileId}'.";
            result.Error = closureconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReassuranceSessionVersion = "runtime-browser-reassurance-session-v1";
        result.BrowserReassuranceStages =
        [
            "open-browser-reassurance-session",
            "bind-browser-closureconfidence-ready-state",
            "publish-browser-reassurance-ready"
        ];
        result.BrowserReassuranceSummary = $"Runtime browser reassurance session prepared {result.BrowserReassuranceStages.Length} reassurance stage(s) for profile '{closureconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser reassurance session ready for profile '{closureconfidenceReadyState.ProfileId}' with {result.BrowserReassuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReassuranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserReassuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserClosureConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClosureConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReassuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserReassuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
