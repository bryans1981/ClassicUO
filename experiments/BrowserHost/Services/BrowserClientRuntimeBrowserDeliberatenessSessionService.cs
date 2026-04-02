namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeliberatenessSession
{
    ValueTask<BrowserClientRuntimeBrowserDeliberatenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeliberatenessSessionService : IBrowserClientRuntimeBrowserDeliberatenessSession
{
    private readonly IBrowserClientRuntimeBrowserPurposefulnessReadyState _runtimeBrowserPurposefulnessReadyState;

    public BrowserClientRuntimeBrowserDeliberatenessSessionService(IBrowserClientRuntimeBrowserPurposefulnessReadyState runtimeBrowserPurposefulnessReadyState)
    {
        _runtimeBrowserPurposefulnessReadyState = runtimeBrowserPurposefulnessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeliberatenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPurposefulnessReadyStateResult purposefulnessReadyState = await _runtimeBrowserPurposefulnessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDeliberatenessSessionResult result = new()
        {
            ProfileId = purposefulnessReadyState.ProfileId,
            SessionId = purposefulnessReadyState.SessionId,
            SessionPath = purposefulnessReadyState.SessionPath,
            BrowserPurposefulnessReadyStateVersion = purposefulnessReadyState.BrowserPurposefulnessReadyStateVersion,
            BrowserPurposefulnessSessionVersion = purposefulnessReadyState.BrowserPurposefulnessSessionVersion,
            LaunchMode = purposefulnessReadyState.LaunchMode,
            AssetRootPath = purposefulnessReadyState.AssetRootPath,
            ProfilesRootPath = purposefulnessReadyState.ProfilesRootPath,
            CacheRootPath = purposefulnessReadyState.CacheRootPath,
            ConfigRootPath = purposefulnessReadyState.ConfigRootPath,
            SettingsFilePath = purposefulnessReadyState.SettingsFilePath,
            StartupProfilePath = purposefulnessReadyState.StartupProfilePath,
            RequiredAssets = purposefulnessReadyState.RequiredAssets,
            ReadyAssetCount = purposefulnessReadyState.ReadyAssetCount,
            CompletedSteps = purposefulnessReadyState.CompletedSteps,
            TotalSteps = purposefulnessReadyState.TotalSteps,
            Exists = purposefulnessReadyState.Exists,
            ReadSucceeded = purposefulnessReadyState.ReadSucceeded
        };

        if (!purposefulnessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser deliberateness session blocked for profile '{purposefulnessReadyState.ProfileId}'.";
            result.Error = purposefulnessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeliberatenessSessionVersion = "runtime-browser-deliberateness-session-v1";
        result.BrowserDeliberatenessStages =
        [
            "open-browser-deliberateness-session",
            "bind-browser-purposefulness-ready-state",
            "publish-browser-deliberateness-ready"
        ];
        result.BrowserDeliberatenessSummary = $"Runtime browser deliberateness session prepared {result.BrowserDeliberatenessStages.Length} deliberateness stage(s) for profile '{purposefulnessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser deliberateness session ready for profile '{purposefulnessReadyState.ProfileId}' with {result.BrowserDeliberatenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeliberatenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDeliberatenessSessionVersion { get; set; } = string.Empty;
    public string BrowserPurposefulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPurposefulnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeliberatenessStages { get; set; } = Array.Empty<string>();
    public string BrowserDeliberatenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
