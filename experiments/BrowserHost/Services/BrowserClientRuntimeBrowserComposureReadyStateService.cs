namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComposureReadyState
{
    ValueTask<BrowserClientRuntimeBrowserComposureReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComposureReadyStateService : IBrowserClientRuntimeBrowserComposureReadyState
{
    private readonly IBrowserClientRuntimeBrowserComposureSession _runtimeBrowserComposureSession;

    public BrowserClientRuntimeBrowserComposureReadyStateService(IBrowserClientRuntimeBrowserComposureSession runtimeBrowserComposureSession)
    {
        _runtimeBrowserComposureSession = runtimeBrowserComposureSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComposureReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComposureSessionResult composureSession = await _runtimeBrowserComposureSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserComposureReadyStateResult result = new()
        {
            ProfileId = composureSession.ProfileId,
            SessionId = composureSession.SessionId,
            SessionPath = composureSession.SessionPath,
            BrowserComposureSessionVersion = composureSession.BrowserComposureSessionVersion,
            BrowserRelianceReadyStateVersion = composureSession.BrowserRelianceReadyStateVersion,
            BrowserRelianceSessionVersion = composureSession.BrowserRelianceSessionVersion,
            LaunchMode = composureSession.LaunchMode,
            AssetRootPath = composureSession.AssetRootPath,
            ProfilesRootPath = composureSession.ProfilesRootPath,
            CacheRootPath = composureSession.CacheRootPath,
            ConfigRootPath = composureSession.ConfigRootPath,
            SettingsFilePath = composureSession.SettingsFilePath,
            StartupProfilePath = composureSession.StartupProfilePath,
            RequiredAssets = composureSession.RequiredAssets,
            ReadyAssetCount = composureSession.ReadyAssetCount,
            CompletedSteps = composureSession.CompletedSteps,
            TotalSteps = composureSession.TotalSteps,
            Exists = composureSession.Exists,
            ReadSucceeded = composureSession.ReadSucceeded
        };

        if (!composureSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser composure ready state blocked for profile '{composureSession.ProfileId}'.";
            result.Error = composureSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComposureReadyStateVersion = "runtime-browser-composure-ready-state-v1";
        result.BrowserComposureReadyChecks =
        [
            "browser-reliance-ready-state-ready",
            "browser-composure-session-ready",
            "browser-composure-ready"
        ];
        result.BrowserComposureReadySummary = $"Runtime browser composure ready state passed {result.BrowserComposureReadyChecks.Length} composure readiness check(s) for profile '{composureSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser composure ready state ready for profile '{composureSession.ProfileId}' with {result.BrowserComposureReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComposureReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserComposureReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComposureSessionVersion { get; set; } = string.Empty;
    public string BrowserRelianceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRelianceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComposureReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserComposureReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
