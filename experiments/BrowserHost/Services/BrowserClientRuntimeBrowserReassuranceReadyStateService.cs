namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReassuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReassuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReassuranceReadyStateService : IBrowserClientRuntimeBrowserReassuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserReassuranceSession _runtimeBrowserReassuranceSession;

    public BrowserClientRuntimeBrowserReassuranceReadyStateService(IBrowserClientRuntimeBrowserReassuranceSession runtimeBrowserReassuranceSession)
    {
        _runtimeBrowserReassuranceSession = runtimeBrowserReassuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReassuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReassuranceSessionResult reassuranceSession = await _runtimeBrowserReassuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserReassuranceReadyStateResult result = new()
        {
            ProfileId = reassuranceSession.ProfileId,
            SessionId = reassuranceSession.SessionId,
            SessionPath = reassuranceSession.SessionPath,
            BrowserReassuranceSessionVersion = reassuranceSession.BrowserReassuranceSessionVersion,
            BrowserClosureConfidenceReadyStateVersion = reassuranceSession.BrowserClosureConfidenceReadyStateVersion,
            BrowserClosureConfidenceSessionVersion = reassuranceSession.BrowserClosureConfidenceSessionVersion,
            LaunchMode = reassuranceSession.LaunchMode,
            AssetRootPath = reassuranceSession.AssetRootPath,
            ProfilesRootPath = reassuranceSession.ProfilesRootPath,
            CacheRootPath = reassuranceSession.CacheRootPath,
            ConfigRootPath = reassuranceSession.ConfigRootPath,
            SettingsFilePath = reassuranceSession.SettingsFilePath,
            StartupProfilePath = reassuranceSession.StartupProfilePath,
            RequiredAssets = reassuranceSession.RequiredAssets,
            ReadyAssetCount = reassuranceSession.ReadyAssetCount,
            CompletedSteps = reassuranceSession.CompletedSteps,
            TotalSteps = reassuranceSession.TotalSteps,
            Exists = reassuranceSession.Exists,
            ReadSucceeded = reassuranceSession.ReadSucceeded
        };

        if (!reassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser reassurance ready state blocked for profile '{reassuranceSession.ProfileId}'.";
            result.Error = reassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReassuranceReadyStateVersion = "runtime-browser-reassurance-ready-state-v1";
        result.BrowserReassuranceReadyChecks =
        [
            "browser-closureconfidence-ready-state-ready",
            "browser-reassurance-session-ready",
            "browser-reassurance-ready"
        ];
        result.BrowserReassuranceReadySummary = $"Runtime browser reassurance ready state passed {result.BrowserReassuranceReadyChecks.Length} reassurance readiness check(s) for profile '{reassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser reassurance ready state ready for profile '{reassuranceSession.ProfileId}' with {result.BrowserReassuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReassuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReassuranceReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserReassuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReassuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
