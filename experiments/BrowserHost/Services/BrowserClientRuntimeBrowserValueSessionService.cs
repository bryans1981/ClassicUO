namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserValueSession
{
    ValueTask<BrowserClientRuntimeBrowserValueSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserValueSessionService : IBrowserClientRuntimeBrowserValueSession
{
    private readonly IBrowserClientRuntimeBrowserUsefulnessReadyState _runtimeBrowserUsefulnessReadyState;

    public BrowserClientRuntimeBrowserValueSessionService(IBrowserClientRuntimeBrowserUsefulnessReadyState runtimeBrowserUsefulnessReadyState)
    {
        _runtimeBrowserUsefulnessReadyState = runtimeBrowserUsefulnessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserValueSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserUsefulnessReadyStateResult usefulnessReadyState = await _runtimeBrowserUsefulnessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserValueSessionResult result = new()
        {
            ProfileId = usefulnessReadyState.ProfileId,
            SessionId = usefulnessReadyState.SessionId,
            SessionPath = usefulnessReadyState.SessionPath,
            BrowserUsefulnessReadyStateVersion = usefulnessReadyState.BrowserUsefulnessReadyStateVersion,
            BrowserUsefulnessSessionVersion = usefulnessReadyState.BrowserUsefulnessSessionVersion,
            LaunchMode = usefulnessReadyState.LaunchMode,
            AssetRootPath = usefulnessReadyState.AssetRootPath,
            ProfilesRootPath = usefulnessReadyState.ProfilesRootPath,
            CacheRootPath = usefulnessReadyState.CacheRootPath,
            ConfigRootPath = usefulnessReadyState.ConfigRootPath,
            SettingsFilePath = usefulnessReadyState.SettingsFilePath,
            StartupProfilePath = usefulnessReadyState.StartupProfilePath,
            RequiredAssets = usefulnessReadyState.RequiredAssets,
            ReadyAssetCount = usefulnessReadyState.ReadyAssetCount,
            CompletedSteps = usefulnessReadyState.CompletedSteps,
            TotalSteps = usefulnessReadyState.TotalSteps,
            Exists = usefulnessReadyState.Exists,
            ReadSucceeded = usefulnessReadyState.ReadSucceeded
        };

        if (!usefulnessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser value session blocked for profile '{usefulnessReadyState.ProfileId}'.";
            result.Error = usefulnessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserValueSessionVersion = "runtime-browser-value-session-v1";
        result.BrowserValueStages =
        [
            "open-browser-value-session",
            "bind-browser-usefulness-ready-state",
            "publish-browser-value-ready"
        ];
        result.BrowserValueSummary = $"Runtime browser value session prepared {result.BrowserValueStages.Length} value stage(s) for profile '{usefulnessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser value session ready for profile '{usefulnessReadyState.ProfileId}' with {result.BrowserValueStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserValueSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserValueSessionVersion { get; set; } = string.Empty;
    public string BrowserUsefulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUsefulnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserValueStages { get; set; } = Array.Empty<string>();
    public string BrowserValueSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
