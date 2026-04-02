namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCertaintyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCertaintyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCertaintyReadyStateService : IBrowserClientRuntimeBrowserCertaintyReadyState
{
    private readonly IBrowserClientRuntimeBrowserCertaintySession _runtimeBrowserCertaintySession;

    public BrowserClientRuntimeBrowserCertaintyReadyStateService(IBrowserClientRuntimeBrowserCertaintySession runtimeBrowserCertaintySession)
    {
        _runtimeBrowserCertaintySession = runtimeBrowserCertaintySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCertaintyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCertaintySessionResult certaintySession = await _runtimeBrowserCertaintySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCertaintyReadyStateResult result = new()
        {
            ProfileId = certaintySession.ProfileId,
            SessionId = certaintySession.SessionId,
            SessionPath = certaintySession.SessionPath,
            BrowserCertaintySessionVersion = certaintySession.BrowserCertaintySessionVersion,
            BrowserAssurednessReadyStateVersion = certaintySession.BrowserAssurednessReadyStateVersion,
            BrowserAssurednessSessionVersion = certaintySession.BrowserAssurednessSessionVersion,
            LaunchMode = certaintySession.LaunchMode,
            AssetRootPath = certaintySession.AssetRootPath,
            ProfilesRootPath = certaintySession.ProfilesRootPath,
            CacheRootPath = certaintySession.CacheRootPath,
            ConfigRootPath = certaintySession.ConfigRootPath,
            SettingsFilePath = certaintySession.SettingsFilePath,
            StartupProfilePath = certaintySession.StartupProfilePath,
            RequiredAssets = certaintySession.RequiredAssets,
            ReadyAssetCount = certaintySession.ReadyAssetCount,
            CompletedSteps = certaintySession.CompletedSteps,
            TotalSteps = certaintySession.TotalSteps,
            Exists = certaintySession.Exists,
            ReadSucceeded = certaintySession.ReadSucceeded
        };

        if (!certaintySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser certainty ready state blocked for profile '{certaintySession.ProfileId}'.";
            result.Error = certaintySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCertaintyReadyStateVersion = "runtime-browser-certainty-ready-state-v1";
        result.BrowserCertaintyReadyChecks =
        [
            "browser-assuredness-ready-state-ready",
            "browser-certainty-session-ready",
            "browser-certainty-ready"
        ];
        result.BrowserCertaintyReadySummary = $"Runtime browser certainty ready state passed {result.BrowserCertaintyReadyChecks.Length} certainty readiness check(s) for profile '{certaintySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser certainty ready state ready for profile '{certaintySession.ProfileId}' with {result.BrowserCertaintyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCertaintyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCertaintyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCertaintySessionVersion { get; set; } = string.Empty;
    public string BrowserAssurednessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurednessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCertaintyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCertaintyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
