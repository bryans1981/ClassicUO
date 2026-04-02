namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOrientationSession
{
    ValueTask<BrowserClientRuntimeBrowserOrientationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOrientationSessionService : IBrowserClientRuntimeBrowserOrientationSession
{
    private readonly IBrowserClientRuntimeBrowserSignpostingReadyState _runtimeBrowserSignpostingReadyState;

    public BrowserClientRuntimeBrowserOrientationSessionService(IBrowserClientRuntimeBrowserSignpostingReadyState runtimeBrowserSignpostingReadyState)
    {
        _runtimeBrowserSignpostingReadyState = runtimeBrowserSignpostingReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOrientationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSignpostingReadyStateResult signpostingReadyState = await _runtimeBrowserSignpostingReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOrientationSessionResult result = new()
        {
            ProfileId = signpostingReadyState.ProfileId,
            SessionId = signpostingReadyState.SessionId,
            SessionPath = signpostingReadyState.SessionPath,
            BrowserSignpostingReadyStateVersion = signpostingReadyState.BrowserSignpostingReadyStateVersion,
            BrowserSignpostingSessionVersion = signpostingReadyState.BrowserSignpostingSessionVersion,
            LaunchMode = signpostingReadyState.LaunchMode,
            AssetRootPath = signpostingReadyState.AssetRootPath,
            ProfilesRootPath = signpostingReadyState.ProfilesRootPath,
            CacheRootPath = signpostingReadyState.CacheRootPath,
            ConfigRootPath = signpostingReadyState.ConfigRootPath,
            SettingsFilePath = signpostingReadyState.SettingsFilePath,
            StartupProfilePath = signpostingReadyState.StartupProfilePath,
            RequiredAssets = signpostingReadyState.RequiredAssets,
            ReadyAssetCount = signpostingReadyState.ReadyAssetCount,
            CompletedSteps = signpostingReadyState.CompletedSteps,
            TotalSteps = signpostingReadyState.TotalSteps,
            Exists = signpostingReadyState.Exists,
            ReadSucceeded = signpostingReadyState.ReadSucceeded
        };

        if (!signpostingReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser orientation session blocked for profile '{signpostingReadyState.ProfileId}'.";
            result.Error = signpostingReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOrientationSessionVersion = "runtime-browser-orientation-session-v1";
        result.BrowserOrientationStages =
        [
            "open-browser-orientation-session",
            "bind-browser-signposting-ready-state",
            "publish-browser-orientation-ready"
        ];
        result.BrowserOrientationSummary = $"Runtime browser orientation session prepared {result.BrowserOrientationStages.Length} orientation stage(s) for profile '{signpostingReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser orientation session ready for profile '{signpostingReadyState.ProfileId}' with {result.BrowserOrientationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOrientationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserOrientationSessionVersion { get; set; } = string.Empty;
    public string BrowserSignpostingReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSignpostingSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOrientationStages { get; set; } = Array.Empty<string>();
    public string BrowserOrientationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
