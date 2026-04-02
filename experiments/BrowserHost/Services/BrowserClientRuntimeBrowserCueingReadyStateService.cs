namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCueingReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCueingReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCueingReadyStateService : IBrowserClientRuntimeBrowserCueingReadyState
{
    private readonly IBrowserClientRuntimeBrowserCueingSession _runtimeBrowserCueingSession;

    public BrowserClientRuntimeBrowserCueingReadyStateService(IBrowserClientRuntimeBrowserCueingSession runtimeBrowserCueingSession)
    {
        _runtimeBrowserCueingSession = runtimeBrowserCueingSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCueingReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCueingSessionResult cueingSession = await _runtimeBrowserCueingSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCueingReadyStateResult result = new()
        {
            ProfileId = cueingSession.ProfileId,
            SessionId = cueingSession.SessionId,
            SessionPath = cueingSession.SessionPath,
            BrowserCueingSessionVersion = cueingSession.BrowserCueingSessionVersion,
            BrowserAbsorptionReadyStateVersion = cueingSession.BrowserAbsorptionReadyStateVersion,
            BrowserAbsorptionSessionVersion = cueingSession.BrowserAbsorptionSessionVersion,
            LaunchMode = cueingSession.LaunchMode,
            AssetRootPath = cueingSession.AssetRootPath,
            ProfilesRootPath = cueingSession.ProfilesRootPath,
            CacheRootPath = cueingSession.CacheRootPath,
            ConfigRootPath = cueingSession.ConfigRootPath,
            SettingsFilePath = cueingSession.SettingsFilePath,
            StartupProfilePath = cueingSession.StartupProfilePath,
            RequiredAssets = cueingSession.RequiredAssets,
            ReadyAssetCount = cueingSession.ReadyAssetCount,
            CompletedSteps = cueingSession.CompletedSteps,
            TotalSteps = cueingSession.TotalSteps,
            Exists = cueingSession.Exists,
            ReadSucceeded = cueingSession.ReadSucceeded
        };

        if (!cueingSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser cueing ready state blocked for profile '{cueingSession.ProfileId}'.";
            result.Error = cueingSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCueingReadyStateVersion = "runtime-browser-cueing-ready-state-v1";
        result.BrowserCueingReadyChecks =
        [
            "browser-absorption-ready-state-ready",
            "browser-cueing-session-ready",
            "browser-cueing-ready"
        ];
        result.BrowserCueingReadySummary = $"Runtime browser cueing ready state passed {result.BrowserCueingReadyChecks.Length} cueing readiness check(s) for profile '{cueingSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser cueing ready state ready for profile '{cueingSession.ProfileId}' with {result.BrowserCueingReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCueingReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCueingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCueingSessionVersion { get; set; } = string.Empty;
    public string BrowserAbsorptionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAbsorptionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCueingReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCueingReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
