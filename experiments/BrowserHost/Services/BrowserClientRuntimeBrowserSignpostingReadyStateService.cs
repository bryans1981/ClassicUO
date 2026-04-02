namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSignpostingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSignpostingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSignpostingReadyStateService : IBrowserClientRuntimeBrowserSignpostingReadyState
{
    private readonly IBrowserClientRuntimeBrowserSignpostingSession _runtimeBrowserSignpostingSession;

    public BrowserClientRuntimeBrowserSignpostingReadyStateService(IBrowserClientRuntimeBrowserSignpostingSession runtimeBrowserSignpostingSession)
    {
        _runtimeBrowserSignpostingSession = runtimeBrowserSignpostingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSignpostingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSignpostingSessionResult signpostingSession = await _runtimeBrowserSignpostingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSignpostingReadyStateResult result = new()
        {
            ProfileId = signpostingSession.ProfileId,
            SessionId = signpostingSession.SessionId,
            SessionPath = signpostingSession.SessionPath,
            BrowserSignpostingSessionVersion = signpostingSession.BrowserSignpostingSessionVersion,
            BrowserAffordanceReadyStateVersion = signpostingSession.BrowserAffordanceReadyStateVersion,
            BrowserAffordanceSessionVersion = signpostingSession.BrowserAffordanceSessionVersion,
            LaunchMode = signpostingSession.LaunchMode,
            AssetRootPath = signpostingSession.AssetRootPath,
            ProfilesRootPath = signpostingSession.ProfilesRootPath,
            CacheRootPath = signpostingSession.CacheRootPath,
            ConfigRootPath = signpostingSession.ConfigRootPath,
            SettingsFilePath = signpostingSession.SettingsFilePath,
            StartupProfilePath = signpostingSession.StartupProfilePath,
            RequiredAssets = signpostingSession.RequiredAssets,
            ReadyAssetCount = signpostingSession.ReadyAssetCount,
            CompletedSteps = signpostingSession.CompletedSteps,
            TotalSteps = signpostingSession.TotalSteps,
            Exists = signpostingSession.Exists,
            ReadSucceeded = signpostingSession.ReadSucceeded
        };

        if (!signpostingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser signposting ready state blocked for profile '{signpostingSession.ProfileId}'.";
            result.Error = signpostingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSignpostingReadyStateVersion = "runtime-browser-signposting-ready-state-v1";
        result.BrowserSignpostingReadyChecks =
        [
            "browser-affordance-ready-state-ready",
            "browser-signposting-session-ready",
            "browser-signposting-ready"
        ];
        result.BrowserSignpostingReadySummary = $"Runtime browser signposting ready state passed {result.BrowserSignpostingReadyChecks.Length} signposting readiness check(s) for profile '{signpostingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser signposting ready state ready for profile '{signpostingSession.ProfileId}' with {result.BrowserSignpostingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSignpostingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSignpostingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSignpostingSessionVersion { get; set; } = string.Empty;
    public string BrowserAffordanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAffordanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSignpostingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSignpostingReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
