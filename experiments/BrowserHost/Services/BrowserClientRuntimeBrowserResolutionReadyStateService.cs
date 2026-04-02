namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResolutionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserResolutionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResolutionReadyStateService : IBrowserClientRuntimeBrowserResolutionReadyState
{
    private readonly IBrowserClientRuntimeBrowserResolutionSession _runtimeBrowserResolutionSession;

    public BrowserClientRuntimeBrowserResolutionReadyStateService(IBrowserClientRuntimeBrowserResolutionSession runtimeBrowserResolutionSession)
    {
        _runtimeBrowserResolutionSession = runtimeBrowserResolutionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResolutionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResolutionSessionResult resolutionSession = await _runtimeBrowserResolutionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserResolutionReadyStateResult result = new()
        {
            ProfileId = resolutionSession.ProfileId,
            SessionId = resolutionSession.SessionId,
            SessionPath = resolutionSession.SessionPath,
            BrowserResolutionSessionVersion = resolutionSession.BrowserResolutionSessionVersion,
            BrowserCertaintyReadyStateVersion = resolutionSession.BrowserCertaintyReadyStateVersion,
            BrowserCertaintySessionVersion = resolutionSession.BrowserCertaintySessionVersion,
            LaunchMode = resolutionSession.LaunchMode,
            AssetRootPath = resolutionSession.AssetRootPath,
            ProfilesRootPath = resolutionSession.ProfilesRootPath,
            CacheRootPath = resolutionSession.CacheRootPath,
            ConfigRootPath = resolutionSession.ConfigRootPath,
            SettingsFilePath = resolutionSession.SettingsFilePath,
            StartupProfilePath = resolutionSession.StartupProfilePath,
            RequiredAssets = resolutionSession.RequiredAssets,
            ReadyAssetCount = resolutionSession.ReadyAssetCount,
            CompletedSteps = resolutionSession.CompletedSteps,
            TotalSteps = resolutionSession.TotalSteps,
            Exists = resolutionSession.Exists,
            ReadSucceeded = resolutionSession.ReadSucceeded
        };

        if (!resolutionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resolution ready state blocked for profile '{resolutionSession.ProfileId}'.";
            result.Error = resolutionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResolutionReadyStateVersion = "runtime-browser-resolution-ready-state-v1";
        result.BrowserResolutionReadyChecks =
        [
            "browser-certainty-ready-state-ready",
            "browser-resolution-session-ready",
            "browser-resolution-ready"
        ];
        result.BrowserResolutionReadySummary = $"Runtime browser resolution ready state passed {result.BrowserResolutionReadyChecks.Length} resolution readiness check(s) for profile '{resolutionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resolution ready state ready for profile '{resolutionSession.ProfileId}' with {result.BrowserResolutionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResolutionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserResolutionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResolutionSessionVersion { get; set; } = string.Empty;
    public string BrowserCertaintyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCertaintySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResolutionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserResolutionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
