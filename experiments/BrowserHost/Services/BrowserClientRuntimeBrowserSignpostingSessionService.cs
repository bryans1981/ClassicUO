namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSignpostingSession
{
    ValueTask<BrowserClientRuntimeBrowserSignpostingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSignpostingSessionService : IBrowserClientRuntimeBrowserSignpostingSession
{
    private readonly IBrowserClientRuntimeBrowserAffordanceReadyState _runtimeBrowserAffordanceReadyState;

    public BrowserClientRuntimeBrowserSignpostingSessionService(IBrowserClientRuntimeBrowserAffordanceReadyState runtimeBrowserAffordanceReadyState)
    {
        _runtimeBrowserAffordanceReadyState = runtimeBrowserAffordanceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSignpostingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAffordanceReadyStateResult affordanceReadyState = await _runtimeBrowserAffordanceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSignpostingSessionResult result = new()
        {
            ProfileId = affordanceReadyState.ProfileId,
            SessionId = affordanceReadyState.SessionId,
            SessionPath = affordanceReadyState.SessionPath,
            BrowserAffordanceReadyStateVersion = affordanceReadyState.BrowserAffordanceReadyStateVersion,
            BrowserAffordanceSessionVersion = affordanceReadyState.BrowserAffordanceSessionVersion,
            LaunchMode = affordanceReadyState.LaunchMode,
            AssetRootPath = affordanceReadyState.AssetRootPath,
            ProfilesRootPath = affordanceReadyState.ProfilesRootPath,
            CacheRootPath = affordanceReadyState.CacheRootPath,
            ConfigRootPath = affordanceReadyState.ConfigRootPath,
            SettingsFilePath = affordanceReadyState.SettingsFilePath,
            StartupProfilePath = affordanceReadyState.StartupProfilePath,
            RequiredAssets = affordanceReadyState.RequiredAssets,
            ReadyAssetCount = affordanceReadyState.ReadyAssetCount,
            CompletedSteps = affordanceReadyState.CompletedSteps,
            TotalSteps = affordanceReadyState.TotalSteps,
            Exists = affordanceReadyState.Exists,
            ReadSucceeded = affordanceReadyState.ReadSucceeded
        };

        if (!affordanceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser signposting session blocked for profile '{affordanceReadyState.ProfileId}'.";
            result.Error = affordanceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSignpostingSessionVersion = "runtime-browser-signposting-session-v1";
        result.BrowserSignpostingStages =
        [
            "open-browser-signposting-session",
            "bind-browser-affordance-ready-state",
            "publish-browser-signposting-ready"
        ];
        result.BrowserSignpostingSummary = $"Runtime browser signposting session prepared {result.BrowserSignpostingStages.Length} signposting stage(s) for profile '{affordanceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser signposting session ready for profile '{affordanceReadyState.ProfileId}' with {result.BrowserSignpostingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSignpostingSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSignpostingSessionVersion { get; set; } = string.Empty;
    public string BrowserAffordanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAffordanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSignpostingStages { get; set; } = Array.Empty<string>();
    public string BrowserSignpostingSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
