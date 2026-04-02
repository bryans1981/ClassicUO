namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResolutionSession
{
    ValueTask<BrowserClientRuntimeBrowserResolutionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResolutionSessionService : IBrowserClientRuntimeBrowserResolutionSession
{
    private readonly IBrowserClientRuntimeBrowserCertaintyReadyState _runtimeBrowserCertaintyReadyState;

    public BrowserClientRuntimeBrowserResolutionSessionService(IBrowserClientRuntimeBrowserCertaintyReadyState runtimeBrowserCertaintyReadyState)
    {
        _runtimeBrowserCertaintyReadyState = runtimeBrowserCertaintyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResolutionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCertaintyReadyStateResult certaintyReadyState = await _runtimeBrowserCertaintyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserResolutionSessionResult result = new()
        {
            ProfileId = certaintyReadyState.ProfileId,
            SessionId = certaintyReadyState.SessionId,
            SessionPath = certaintyReadyState.SessionPath,
            BrowserCertaintyReadyStateVersion = certaintyReadyState.BrowserCertaintyReadyStateVersion,
            BrowserCertaintySessionVersion = certaintyReadyState.BrowserCertaintySessionVersion,
            LaunchMode = certaintyReadyState.LaunchMode,
            AssetRootPath = certaintyReadyState.AssetRootPath,
            ProfilesRootPath = certaintyReadyState.ProfilesRootPath,
            CacheRootPath = certaintyReadyState.CacheRootPath,
            ConfigRootPath = certaintyReadyState.ConfigRootPath,
            SettingsFilePath = certaintyReadyState.SettingsFilePath,
            StartupProfilePath = certaintyReadyState.StartupProfilePath,
            RequiredAssets = certaintyReadyState.RequiredAssets,
            ReadyAssetCount = certaintyReadyState.ReadyAssetCount,
            CompletedSteps = certaintyReadyState.CompletedSteps,
            TotalSteps = certaintyReadyState.TotalSteps,
            Exists = certaintyReadyState.Exists,
            ReadSucceeded = certaintyReadyState.ReadSucceeded
        };

        if (!certaintyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resolution session blocked for profile '{certaintyReadyState.ProfileId}'.";
            result.Error = certaintyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResolutionSessionVersion = "runtime-browser-resolution-session-v1";
        result.BrowserResolutionStages =
        [
            "open-browser-resolution-session",
            "bind-browser-certainty-ready-state",
            "publish-browser-resolution-ready"
        ];
        result.BrowserResolutionSummary = $"Runtime browser resolution session prepared {result.BrowserResolutionStages.Length} resolution stage(s) for profile '{certaintyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resolution session ready for profile '{certaintyReadyState.ProfileId}' with {result.BrowserResolutionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResolutionSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserResolutionStages { get; set; } = Array.Empty<string>();
    public string BrowserResolutionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
