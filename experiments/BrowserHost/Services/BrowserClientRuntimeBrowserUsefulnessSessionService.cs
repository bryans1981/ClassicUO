namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserUsefulnessSession
{
    ValueTask<BrowserClientRuntimeBrowserUsefulnessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserUsefulnessSessionService : IBrowserClientRuntimeBrowserUsefulnessSession
{
    private readonly IBrowserClientRuntimeBrowserDelightReadyState _runtimeBrowserDelightReadyState;

    public BrowserClientRuntimeBrowserUsefulnessSessionService(IBrowserClientRuntimeBrowserDelightReadyState runtimeBrowserDelightReadyState)
    {
        _runtimeBrowserDelightReadyState = runtimeBrowserDelightReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserUsefulnessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDelightReadyStateResult delightReadyState = await _runtimeBrowserDelightReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserUsefulnessSessionResult result = new()
        {
            ProfileId = delightReadyState.ProfileId,
            SessionId = delightReadyState.SessionId,
            SessionPath = delightReadyState.SessionPath,
            BrowserDelightReadyStateVersion = delightReadyState.BrowserDelightReadyStateVersion,
            BrowserDelightSessionVersion = delightReadyState.BrowserDelightSessionVersion,
            LaunchMode = delightReadyState.LaunchMode,
            AssetRootPath = delightReadyState.AssetRootPath,
            ProfilesRootPath = delightReadyState.ProfilesRootPath,
            CacheRootPath = delightReadyState.CacheRootPath,
            ConfigRootPath = delightReadyState.ConfigRootPath,
            SettingsFilePath = delightReadyState.SettingsFilePath,
            StartupProfilePath = delightReadyState.StartupProfilePath,
            RequiredAssets = delightReadyState.RequiredAssets,
            ReadyAssetCount = delightReadyState.ReadyAssetCount,
            CompletedSteps = delightReadyState.CompletedSteps,
            TotalSteps = delightReadyState.TotalSteps,
            Exists = delightReadyState.Exists,
            ReadSucceeded = delightReadyState.ReadSucceeded
        };

        if (!delightReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser usefulness session blocked for profile '{delightReadyState.ProfileId}'.";
            result.Error = delightReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserUsefulnessSessionVersion = "runtime-browser-usefulness-session-v1";
        result.BrowserUsefulnessStages =
        [
            "open-browser-usefulness-session",
            "bind-browser-delight-ready-state",
            "publish-browser-usefulness-ready"
        ];
        result.BrowserUsefulnessSummary = $"Runtime browser usefulness session prepared {result.BrowserUsefulnessStages.Length} usefulness stage(s) for profile '{delightReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser usefulness session ready for profile '{delightReadyState.ProfileId}' with {result.BrowserUsefulnessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserUsefulnessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserUsefulnessSessionVersion { get; set; } = string.Empty;
    public string BrowserDelightReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDelightSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserUsefulnessStages { get; set; } = Array.Empty<string>();
    public string BrowserUsefulnessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
