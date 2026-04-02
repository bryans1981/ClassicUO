namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserImmersionSession
{
    ValueTask<BrowserClientRuntimeBrowserImmersionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserImmersionSessionService : IBrowserClientRuntimeBrowserImmersionSession
{
    private readonly IBrowserClientRuntimeBrowserEngagementReadyState _runtimeBrowserEngagementReadyState;

    public BrowserClientRuntimeBrowserImmersionSessionService(IBrowserClientRuntimeBrowserEngagementReadyState runtimeBrowserEngagementReadyState)
    {
        _runtimeBrowserEngagementReadyState = runtimeBrowserEngagementReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserImmersionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEngagementReadyStateResult engagementReadyState = await _runtimeBrowserEngagementReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserImmersionSessionResult result = new()
        {
            ProfileId = engagementReadyState.ProfileId,
            SessionId = engagementReadyState.SessionId,
            SessionPath = engagementReadyState.SessionPath,
            BrowserEngagementReadyStateVersion = engagementReadyState.BrowserEngagementReadyStateVersion,
            BrowserEngagementSessionVersion = engagementReadyState.BrowserEngagementSessionVersion,
            LaunchMode = engagementReadyState.LaunchMode,
            AssetRootPath = engagementReadyState.AssetRootPath,
            ProfilesRootPath = engagementReadyState.ProfilesRootPath,
            CacheRootPath = engagementReadyState.CacheRootPath,
            ConfigRootPath = engagementReadyState.ConfigRootPath,
            SettingsFilePath = engagementReadyState.SettingsFilePath,
            StartupProfilePath = engagementReadyState.StartupProfilePath,
            RequiredAssets = engagementReadyState.RequiredAssets,
            ReadyAssetCount = engagementReadyState.ReadyAssetCount,
            CompletedSteps = engagementReadyState.CompletedSteps,
            TotalSteps = engagementReadyState.TotalSteps,
            Exists = engagementReadyState.Exists,
            ReadSucceeded = engagementReadyState.ReadSucceeded
        };

        if (!engagementReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser immersion session blocked for profile '{engagementReadyState.ProfileId}'.";
            result.Error = engagementReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserImmersionSessionVersion = "runtime-browser-immersion-session-v1";
        result.BrowserImmersionStages =
        [
            "open-browser-immersion-session",
            "bind-browser-engagement-ready-state",
            "publish-browser-immersion-ready"
        ];
        result.BrowserImmersionSummary = $"Runtime browser immersion session prepared {result.BrowserImmersionStages.Length} immersion stage(s) for profile '{engagementReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser immersion session ready for profile '{engagementReadyState.ProfileId}' with {result.BrowserImmersionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserImmersionSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserImmersionSessionVersion { get; set; } = string.Empty;
    public string BrowserEngagementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEngagementSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserImmersionStages { get; set; } = Array.Empty<string>();
    public string BrowserImmersionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
