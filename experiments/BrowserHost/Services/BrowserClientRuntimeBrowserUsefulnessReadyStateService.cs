namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserUsefulnessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserUsefulnessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserUsefulnessReadyStateService : IBrowserClientRuntimeBrowserUsefulnessReadyState
{
    private readonly IBrowserClientRuntimeBrowserUsefulnessSession _runtimeBrowserUsefulnessSession;

    public BrowserClientRuntimeBrowserUsefulnessReadyStateService(IBrowserClientRuntimeBrowserUsefulnessSession runtimeBrowserUsefulnessSession)
    {
        _runtimeBrowserUsefulnessSession = runtimeBrowserUsefulnessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserUsefulnessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserUsefulnessSessionResult usefulnessSession = await _runtimeBrowserUsefulnessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserUsefulnessReadyStateResult result = new()
        {
            ProfileId = usefulnessSession.ProfileId,
            SessionId = usefulnessSession.SessionId,
            SessionPath = usefulnessSession.SessionPath,
            BrowserUsefulnessSessionVersion = usefulnessSession.BrowserUsefulnessSessionVersion,
            BrowserDelightReadyStateVersion = usefulnessSession.BrowserDelightReadyStateVersion,
            BrowserDelightSessionVersion = usefulnessSession.BrowserDelightSessionVersion,
            LaunchMode = usefulnessSession.LaunchMode,
            AssetRootPath = usefulnessSession.AssetRootPath,
            ProfilesRootPath = usefulnessSession.ProfilesRootPath,
            CacheRootPath = usefulnessSession.CacheRootPath,
            ConfigRootPath = usefulnessSession.ConfigRootPath,
            SettingsFilePath = usefulnessSession.SettingsFilePath,
            StartupProfilePath = usefulnessSession.StartupProfilePath,
            RequiredAssets = usefulnessSession.RequiredAssets,
            ReadyAssetCount = usefulnessSession.ReadyAssetCount,
            CompletedSteps = usefulnessSession.CompletedSteps,
            TotalSteps = usefulnessSession.TotalSteps,
            Exists = usefulnessSession.Exists,
            ReadSucceeded = usefulnessSession.ReadSucceeded
        };

        if (!usefulnessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser usefulness ready state blocked for profile '{usefulnessSession.ProfileId}'.";
            result.Error = usefulnessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserUsefulnessReadyStateVersion = "runtime-browser-usefulness-ready-state-v1";
        result.BrowserUsefulnessReadyChecks =
        [
            "browser-delight-ready-state-ready",
            "browser-usefulness-session-ready",
            "browser-usefulness-ready"
        ];
        result.BrowserUsefulnessReadySummary = $"Runtime browser usefulness ready state passed {result.BrowserUsefulnessReadyChecks.Length} usefulness readiness check(s) for profile '{usefulnessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser usefulness ready state ready for profile '{usefulnessSession.ProfileId}' with {result.BrowserUsefulnessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserUsefulnessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserUsefulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUsefulnessSessionVersion { get; set; } = string.Empty;
    public string BrowserDelightReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDelightSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserUsefulnessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserUsefulnessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
