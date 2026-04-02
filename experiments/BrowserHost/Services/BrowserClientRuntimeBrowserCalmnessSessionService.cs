namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCalmnessSession
{
    ValueTask<BrowserClientRuntimeBrowserCalmnessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCalmnessSessionService : IBrowserClientRuntimeBrowserCalmnessSession
{
    private readonly IBrowserClientRuntimeBrowserComposureReadyState _runtimeBrowserComposureReadyState;

    public BrowserClientRuntimeBrowserCalmnessSessionService(IBrowserClientRuntimeBrowserComposureReadyState runtimeBrowserComposureReadyState)
    {
        _runtimeBrowserComposureReadyState = runtimeBrowserComposureReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCalmnessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComposureReadyStateResult composureReadyState = await _runtimeBrowserComposureReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCalmnessSessionResult result = new()
        {
            ProfileId = composureReadyState.ProfileId,
            SessionId = composureReadyState.SessionId,
            SessionPath = composureReadyState.SessionPath,
            BrowserComposureReadyStateVersion = composureReadyState.BrowserComposureReadyStateVersion,
            BrowserComposureSessionVersion = composureReadyState.BrowserComposureSessionVersion,
            LaunchMode = composureReadyState.LaunchMode,
            AssetRootPath = composureReadyState.AssetRootPath,
            ProfilesRootPath = composureReadyState.ProfilesRootPath,
            CacheRootPath = composureReadyState.CacheRootPath,
            ConfigRootPath = composureReadyState.ConfigRootPath,
            SettingsFilePath = composureReadyState.SettingsFilePath,
            StartupProfilePath = composureReadyState.StartupProfilePath,
            RequiredAssets = composureReadyState.RequiredAssets,
            ReadyAssetCount = composureReadyState.ReadyAssetCount,
            CompletedSteps = composureReadyState.CompletedSteps,
            TotalSteps = composureReadyState.TotalSteps,
            Exists = composureReadyState.Exists,
            ReadSucceeded = composureReadyState.ReadSucceeded
        };

        if (!composureReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser calmness session blocked for profile '{composureReadyState.ProfileId}'.";
            result.Error = composureReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCalmnessSessionVersion = "runtime-browser-calmness-session-v1";
        result.BrowserCalmnessStages =
        [
            "open-browser-calmness-session",
            "bind-browser-composure-ready-state",
            "publish-browser-calmness-ready"
        ];
        result.BrowserCalmnessSummary = $"Runtime browser calmness session prepared {result.BrowserCalmnessStages.Length} calmness stage(s) for profile '{composureReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser calmness session ready for profile '{composureReadyState.ProfileId}' with {result.BrowserCalmnessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCalmnessSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserCalmnessStages { get; set; } = Array.Empty<string>();
    public string BrowserCalmnessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
