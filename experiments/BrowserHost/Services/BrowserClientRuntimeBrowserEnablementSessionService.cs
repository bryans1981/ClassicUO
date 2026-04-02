namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEnablementSession
{
    ValueTask<BrowserClientRuntimeBrowserEnablementSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEnablementSessionService : IBrowserClientRuntimeBrowserEnablementSession
{
    private readonly IBrowserClientRuntimeBrowserAssistanceReadyState _runtimeBrowserAssistanceReadyState;

    public BrowserClientRuntimeBrowserEnablementSessionService(IBrowserClientRuntimeBrowserAssistanceReadyState runtimeBrowserAssistanceReadyState)
    {
        _runtimeBrowserAssistanceReadyState = runtimeBrowserAssistanceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEnablementSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssistanceReadyStateResult assistanceReadyState = await _runtimeBrowserAssistanceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEnablementSessionResult result = new()
        {
            ProfileId = assistanceReadyState.ProfileId,
            SessionId = assistanceReadyState.SessionId,
            SessionPath = assistanceReadyState.SessionPath,
            BrowserAssistanceReadyStateVersion = assistanceReadyState.BrowserAssistanceReadyStateVersion,
            BrowserAssistanceSessionVersion = assistanceReadyState.BrowserAssistanceSessionVersion,
            LaunchMode = assistanceReadyState.LaunchMode,
            AssetRootPath = assistanceReadyState.AssetRootPath,
            ProfilesRootPath = assistanceReadyState.ProfilesRootPath,
            CacheRootPath = assistanceReadyState.CacheRootPath,
            ConfigRootPath = assistanceReadyState.ConfigRootPath,
            SettingsFilePath = assistanceReadyState.SettingsFilePath,
            StartupProfilePath = assistanceReadyState.StartupProfilePath,
            RequiredAssets = assistanceReadyState.RequiredAssets,
            ReadyAssetCount = assistanceReadyState.ReadyAssetCount,
            CompletedSteps = assistanceReadyState.CompletedSteps,
            TotalSteps = assistanceReadyState.TotalSteps,
            Exists = assistanceReadyState.Exists,
            ReadSucceeded = assistanceReadyState.ReadSucceeded
        };

        if (!assistanceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser enablement session blocked for profile '{assistanceReadyState.ProfileId}'.";
            result.Error = assistanceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEnablementSessionVersion = "runtime-browser-enablement-session-v1";
        result.BrowserEnablementStages =
        [
            "open-browser-enablement-session",
            "bind-browser-assistance-ready-state",
            "publish-browser-enablement-ready"
        ];
        result.BrowserEnablementSummary = $"Runtime browser enablement session prepared {result.BrowserEnablementStages.Length} enablement stage(s) for profile '{assistanceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser enablement session ready for profile '{assistanceReadyState.ProfileId}' with {result.BrowserEnablementStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEnablementSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserEnablementSessionVersion { get; set; } = string.Empty;
    public string BrowserAssistanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssistanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEnablementStages { get; set; } = Array.Empty<string>();
    public string BrowserEnablementSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
