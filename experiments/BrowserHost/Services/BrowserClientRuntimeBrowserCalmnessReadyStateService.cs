namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCalmnessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCalmnessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCalmnessReadyStateService : IBrowserClientRuntimeBrowserCalmnessReadyState
{
    private readonly IBrowserClientRuntimeBrowserCalmnessSession _runtimeBrowserCalmnessSession;

    public BrowserClientRuntimeBrowserCalmnessReadyStateService(IBrowserClientRuntimeBrowserCalmnessSession runtimeBrowserCalmnessSession)
    {
        _runtimeBrowserCalmnessSession = runtimeBrowserCalmnessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCalmnessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCalmnessSessionResult calmnessSession = await _runtimeBrowserCalmnessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCalmnessReadyStateResult result = new()
        {
            ProfileId = calmnessSession.ProfileId,
            SessionId = calmnessSession.SessionId,
            SessionPath = calmnessSession.SessionPath,
            BrowserCalmnessSessionVersion = calmnessSession.BrowserCalmnessSessionVersion,
            BrowserComposureReadyStateVersion = calmnessSession.BrowserComposureReadyStateVersion,
            BrowserComposureSessionVersion = calmnessSession.BrowserComposureSessionVersion,
            LaunchMode = calmnessSession.LaunchMode,
            AssetRootPath = calmnessSession.AssetRootPath,
            ProfilesRootPath = calmnessSession.ProfilesRootPath,
            CacheRootPath = calmnessSession.CacheRootPath,
            ConfigRootPath = calmnessSession.ConfigRootPath,
            SettingsFilePath = calmnessSession.SettingsFilePath,
            StartupProfilePath = calmnessSession.StartupProfilePath,
            RequiredAssets = calmnessSession.RequiredAssets,
            ReadyAssetCount = calmnessSession.ReadyAssetCount,
            CompletedSteps = calmnessSession.CompletedSteps,
            TotalSteps = calmnessSession.TotalSteps,
            Exists = calmnessSession.Exists,
            ReadSucceeded = calmnessSession.ReadSucceeded
        };

        if (!calmnessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser calmness ready state blocked for profile '{calmnessSession.ProfileId}'.";
            result.Error = calmnessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCalmnessReadyStateVersion = "runtime-browser-calmness-ready-state-v1";
        result.BrowserCalmnessReadyChecks =
        [
            "browser-composure-ready-state-ready",
            "browser-calmness-session-ready",
            "browser-calmness-ready"
        ];
        result.BrowserCalmnessReadySummary = $"Runtime browser calmness ready state passed {result.BrowserCalmnessReadyChecks.Length} calmness readiness check(s) for profile '{calmnessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser calmness ready state ready for profile '{calmnessSession.ProfileId}' with {result.BrowserCalmnessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCalmnessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCalmnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCalmnessSessionVersion { get; set; } = string.Empty;
    public string BrowserComposureReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComposureSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCalmnessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCalmnessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
