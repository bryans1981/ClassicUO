namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssistanceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAssistanceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssistanceReadyStateService : IBrowserClientRuntimeBrowserAssistanceReadyState
{
    private readonly IBrowserClientRuntimeBrowserAssistanceSession _runtimeBrowserAssistanceSession;

    public BrowserClientRuntimeBrowserAssistanceReadyStateService(IBrowserClientRuntimeBrowserAssistanceSession runtimeBrowserAssistanceSession)
    {
        _runtimeBrowserAssistanceSession = runtimeBrowserAssistanceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssistanceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssistanceSessionResult assistanceSession = await _runtimeBrowserAssistanceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAssistanceReadyStateResult result = new()
        {
            ProfileId = assistanceSession.ProfileId,
            SessionId = assistanceSession.SessionId,
            SessionPath = assistanceSession.SessionPath,
            BrowserAssistanceSessionVersion = assistanceSession.BrowserAssistanceSessionVersion,
            BrowserSteadinessReadyStateVersion = assistanceSession.BrowserSteadinessReadyStateVersion,
            BrowserSteadinessSessionVersion = assistanceSession.BrowserSteadinessSessionVersion,
            LaunchMode = assistanceSession.LaunchMode,
            AssetRootPath = assistanceSession.AssetRootPath,
            ProfilesRootPath = assistanceSession.ProfilesRootPath,
            CacheRootPath = assistanceSession.CacheRootPath,
            ConfigRootPath = assistanceSession.ConfigRootPath,
            SettingsFilePath = assistanceSession.SettingsFilePath,
            StartupProfilePath = assistanceSession.StartupProfilePath,
            RequiredAssets = assistanceSession.RequiredAssets,
            ReadyAssetCount = assistanceSession.ReadyAssetCount,
            CompletedSteps = assistanceSession.CompletedSteps,
            TotalSteps = assistanceSession.TotalSteps,
            Exists = assistanceSession.Exists,
            ReadSucceeded = assistanceSession.ReadSucceeded
        };

        if (!assistanceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assistance ready state blocked for profile '{assistanceSession.ProfileId}'.";
            result.Error = assistanceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssistanceReadyStateVersion = "runtime-browser-assistance-ready-state-v1";
        result.BrowserAssistanceReadyChecks =
        [
            "browser-steadiness-ready-state-ready",
            "browser-assistance-session-ready",
            "browser-assistance-ready"
        ];
        result.BrowserAssistanceReadySummary = $"Runtime browser assistance ready state passed {result.BrowserAssistanceReadyChecks.Length} assistance readiness check(s) for profile '{assistanceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assistance ready state ready for profile '{assistanceSession.ProfileId}' with {result.BrowserAssistanceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssistanceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAssistanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssistanceSessionVersion { get; set; } = string.Empty;
    public string BrowserSteadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssistanceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAssistanceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
