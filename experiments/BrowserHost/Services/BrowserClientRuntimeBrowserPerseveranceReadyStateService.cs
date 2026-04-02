namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPerseveranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPerseveranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPerseveranceReadyStateService : IBrowserClientRuntimeBrowserPerseveranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserPerseveranceSession _runtimeBrowserPerseveranceSession;

    public BrowserClientRuntimeBrowserPerseveranceReadyStateService(IBrowserClientRuntimeBrowserPerseveranceSession runtimeBrowserPerseveranceSession)
    {
        _runtimeBrowserPerseveranceSession = runtimeBrowserPerseveranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPerseveranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPerseveranceSessionResult perseveranceSession = await _runtimeBrowserPerseveranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPerseveranceReadyStateResult result = new()
        {
            ProfileId = perseveranceSession.ProfileId,
            SessionId = perseveranceSession.SessionId,
            SessionPath = perseveranceSession.SessionPath,
            BrowserPerseveranceSessionVersion = perseveranceSession.BrowserPerseveranceSessionVersion,
            BrowserDedicationReadyStateVersion = perseveranceSession.BrowserDedicationReadyStateVersion,
            BrowserDedicationSessionVersion = perseveranceSession.BrowserDedicationSessionVersion,
            LaunchMode = perseveranceSession.LaunchMode,
            AssetRootPath = perseveranceSession.AssetRootPath,
            ProfilesRootPath = perseveranceSession.ProfilesRootPath,
            CacheRootPath = perseveranceSession.CacheRootPath,
            ConfigRootPath = perseveranceSession.ConfigRootPath,
            SettingsFilePath = perseveranceSession.SettingsFilePath,
            StartupProfilePath = perseveranceSession.StartupProfilePath,
            RequiredAssets = perseveranceSession.RequiredAssets,
            ReadyAssetCount = perseveranceSession.ReadyAssetCount,
            CompletedSteps = perseveranceSession.CompletedSteps,
            TotalSteps = perseveranceSession.TotalSteps,
            Exists = perseveranceSession.Exists,
            ReadSucceeded = perseveranceSession.ReadSucceeded
        };

        if (!perseveranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser perseverance ready state blocked for profile '{perseveranceSession.ProfileId}'.";
            result.Error = perseveranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPerseveranceReadyStateVersion = "runtime-browser-perseverance-ready-state-v1";
        result.BrowserPerseveranceReadyChecks =
        [
            "browser-dedication-ready-state-ready",
            "browser-perseverance-session-ready",
            "browser-perseverance-ready"
        ];
        result.BrowserPerseveranceReadySummary = $"Runtime browser perseverance ready state passed {result.BrowserPerseveranceReadyChecks.Length} perseverance readiness check(s) for profile '{perseveranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser perseverance ready state ready for profile '{perseveranceSession.ProfileId}' with {result.BrowserPerseveranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPerseveranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPerseveranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPerseveranceSessionVersion { get; set; } = string.Empty;
    public string BrowserDedicationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDedicationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPerseveranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPerseveranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
