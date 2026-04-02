namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStewardshipReadyState
{
    ValueTask<BrowserClientRuntimeBrowserStewardshipReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStewardshipReadyStateService : IBrowserClientRuntimeBrowserStewardshipReadyState
{
    private readonly IBrowserClientRuntimeBrowserStewardshipSession _runtimeBrowserStewardshipSession;

    public BrowserClientRuntimeBrowserStewardshipReadyStateService(IBrowserClientRuntimeBrowserStewardshipSession runtimeBrowserStewardshipSession)
    {
        _runtimeBrowserStewardshipSession = runtimeBrowserStewardshipSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStewardshipReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStewardshipSessionResult stewardshipSession = await _runtimeBrowserStewardshipSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserStewardshipReadyStateResult result = new()
        {
            ProfileId = stewardshipSession.ProfileId,
            SessionId = stewardshipSession.SessionId,
            SessionPath = stewardshipSession.SessionPath,
            BrowserStewardshipSessionVersion = stewardshipSession.BrowserStewardshipSessionVersion,
            BrowserLongevityReadyStateVersion = stewardshipSession.BrowserLongevityReadyStateVersion,
            BrowserLongevitySessionVersion = stewardshipSession.BrowserLongevitySessionVersion,
            LaunchMode = stewardshipSession.LaunchMode,
            AssetRootPath = stewardshipSession.AssetRootPath,
            ProfilesRootPath = stewardshipSession.ProfilesRootPath,
            CacheRootPath = stewardshipSession.CacheRootPath,
            ConfigRootPath = stewardshipSession.ConfigRootPath,
            SettingsFilePath = stewardshipSession.SettingsFilePath,
            StartupProfilePath = stewardshipSession.StartupProfilePath,
            RequiredAssets = stewardshipSession.RequiredAssets,
            ReadyAssetCount = stewardshipSession.ReadyAssetCount,
            CompletedSteps = stewardshipSession.CompletedSteps,
            TotalSteps = stewardshipSession.TotalSteps,
            Exists = stewardshipSession.Exists,
            ReadSucceeded = stewardshipSession.ReadSucceeded
        };

        if (!stewardshipSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser stewardship ready state blocked for profile '{stewardshipSession.ProfileId}'.";
            result.Error = stewardshipSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStewardshipReadyStateVersion = "runtime-browser-stewardship-ready-state-v1";
        result.BrowserStewardshipReadyChecks =
        [
            "browser-longevity-ready-state-ready",
            "browser-stewardship-session-ready",
            "browser-stewardship-ready"
        ];
        result.BrowserStewardshipReadySummary = $"Runtime browser stewardship ready state passed {result.BrowserStewardshipReadyChecks.Length} stewardship readiness check(s) for profile '{stewardshipSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser stewardship ready state ready for profile '{stewardshipSession.ProfileId}' with {result.BrowserStewardshipReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStewardshipReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserStewardshipReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStewardshipSessionVersion { get; set; } = string.Empty;
    public string BrowserLongevityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLongevitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStewardshipReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserStewardshipReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
