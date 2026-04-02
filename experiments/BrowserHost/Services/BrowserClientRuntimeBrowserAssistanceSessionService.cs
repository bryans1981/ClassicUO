namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssistanceSession
{
    ValueTask<BrowserClientRuntimeBrowserAssistanceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssistanceSessionService : IBrowserClientRuntimeBrowserAssistanceSession
{
    private readonly IBrowserClientRuntimeBrowserSteadinessReadyState _runtimeBrowserSteadinessReadyState;

    public BrowserClientRuntimeBrowserAssistanceSessionService(IBrowserClientRuntimeBrowserSteadinessReadyState runtimeBrowserSteadinessReadyState)
    {
        _runtimeBrowserSteadinessReadyState = runtimeBrowserSteadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssistanceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadinessReadyStateResult steadinessReadyState = await _runtimeBrowserSteadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAssistanceSessionResult result = new()
        {
            ProfileId = steadinessReadyState.ProfileId,
            SessionId = steadinessReadyState.SessionId,
            SessionPath = steadinessReadyState.SessionPath,
            BrowserSteadinessReadyStateVersion = steadinessReadyState.BrowserSteadinessReadyStateVersion,
            BrowserSteadinessSessionVersion = steadinessReadyState.BrowserSteadinessSessionVersion,
            LaunchMode = steadinessReadyState.LaunchMode,
            AssetRootPath = steadinessReadyState.AssetRootPath,
            ProfilesRootPath = steadinessReadyState.ProfilesRootPath,
            CacheRootPath = steadinessReadyState.CacheRootPath,
            ConfigRootPath = steadinessReadyState.ConfigRootPath,
            SettingsFilePath = steadinessReadyState.SettingsFilePath,
            StartupProfilePath = steadinessReadyState.StartupProfilePath,
            RequiredAssets = steadinessReadyState.RequiredAssets,
            ReadyAssetCount = steadinessReadyState.ReadyAssetCount,
            CompletedSteps = steadinessReadyState.CompletedSteps,
            TotalSteps = steadinessReadyState.TotalSteps,
            Exists = steadinessReadyState.Exists,
            ReadSucceeded = steadinessReadyState.ReadSucceeded
        };

        if (!steadinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assistance session blocked for profile '{steadinessReadyState.ProfileId}'.";
            result.Error = steadinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssistanceSessionVersion = "runtime-browser-assistance-session-v1";
        result.BrowserAssistanceStages =
        [
            "open-browser-assistance-session",
            "bind-browser-steadiness-ready-state",
            "publish-browser-assistance-ready"
        ];
        result.BrowserAssistanceSummary = $"Runtime browser assistance session prepared {result.BrowserAssistanceStages.Length} assistance stage(s) for profile '{steadinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assistance session ready for profile '{steadinessReadyState.ProfileId}' with {result.BrowserAssistanceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssistanceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserAssistanceStages { get; set; } = Array.Empty<string>();
    public string BrowserAssistanceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
