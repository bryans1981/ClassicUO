namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeliberatenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDeliberatenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeliberatenessReadyStateService : IBrowserClientRuntimeBrowserDeliberatenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserDeliberatenessSession _runtimeBrowserDeliberatenessSession;

    public BrowserClientRuntimeBrowserDeliberatenessReadyStateService(IBrowserClientRuntimeBrowserDeliberatenessSession runtimeBrowserDeliberatenessSession)
    {
        _runtimeBrowserDeliberatenessSession = runtimeBrowserDeliberatenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeliberatenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeliberatenessSessionResult deliberatenessSession = await _runtimeBrowserDeliberatenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDeliberatenessReadyStateResult result = new()
        {
            ProfileId = deliberatenessSession.ProfileId,
            SessionId = deliberatenessSession.SessionId,
            SessionPath = deliberatenessSession.SessionPath,
            BrowserDeliberatenessSessionVersion = deliberatenessSession.BrowserDeliberatenessSessionVersion,
            BrowserPurposefulnessReadyStateVersion = deliberatenessSession.BrowserPurposefulnessReadyStateVersion,
            BrowserPurposefulnessSessionVersion = deliberatenessSession.BrowserPurposefulnessSessionVersion,
            LaunchMode = deliberatenessSession.LaunchMode,
            AssetRootPath = deliberatenessSession.AssetRootPath,
            ProfilesRootPath = deliberatenessSession.ProfilesRootPath,
            CacheRootPath = deliberatenessSession.CacheRootPath,
            ConfigRootPath = deliberatenessSession.ConfigRootPath,
            SettingsFilePath = deliberatenessSession.SettingsFilePath,
            StartupProfilePath = deliberatenessSession.StartupProfilePath,
            RequiredAssets = deliberatenessSession.RequiredAssets,
            ReadyAssetCount = deliberatenessSession.ReadyAssetCount,
            CompletedSteps = deliberatenessSession.CompletedSteps,
            TotalSteps = deliberatenessSession.TotalSteps,
            Exists = deliberatenessSession.Exists,
            ReadSucceeded = deliberatenessSession.ReadSucceeded
        };

        if (!deliberatenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser deliberateness ready state blocked for profile '{deliberatenessSession.ProfileId}'.";
            result.Error = deliberatenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeliberatenessReadyStateVersion = "runtime-browser-deliberateness-ready-state-v1";
        result.BrowserDeliberatenessReadyChecks =
        [
            "browser-purposefulness-ready-state-ready",
            "browser-deliberateness-session-ready",
            "browser-deliberateness-ready"
        ];
        result.BrowserDeliberatenessReadySummary = $"Runtime browser deliberateness ready state passed {result.BrowserDeliberatenessReadyChecks.Length} deliberateness readiness check(s) for profile '{deliberatenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deliberateness ready state ready for profile '{deliberatenessSession.ProfileId}' with {result.BrowserDeliberatenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeliberatenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDeliberatenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeliberatenessSessionVersion { get; set; } = string.Empty;
    public string BrowserPurposefulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPurposefulnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeliberatenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDeliberatenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
