namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResponsivenessSession
{
    ValueTask<BrowserClientRuntimeBrowserResponsivenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResponsivenessSessionService : IBrowserClientRuntimeBrowserResponsivenessSession
{
    private readonly IBrowserClientRuntimeBrowserVisibilityReadyState _runtimeBrowserVisibilityReadyState;

    public BrowserClientRuntimeBrowserResponsivenessSessionService(IBrowserClientRuntimeBrowserVisibilityReadyState runtimeBrowserVisibilityReadyState)
    {
        _runtimeBrowserVisibilityReadyState = runtimeBrowserVisibilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResponsivenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserVisibilityReadyStateResult visibilityReadyState = await _runtimeBrowserVisibilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserResponsivenessSessionResult result = new()
        {
            ProfileId = visibilityReadyState.ProfileId,
            SessionId = visibilityReadyState.SessionId,
            SessionPath = visibilityReadyState.SessionPath,
            BrowserVisibilityReadyStateVersion = visibilityReadyState.BrowserVisibilityReadyStateVersion,
            BrowserVisibilitySessionVersion = visibilityReadyState.BrowserVisibilitySessionVersion,
            LaunchMode = visibilityReadyState.LaunchMode,
            AssetRootPath = visibilityReadyState.AssetRootPath,
            ProfilesRootPath = visibilityReadyState.ProfilesRootPath,
            CacheRootPath = visibilityReadyState.CacheRootPath,
            ConfigRootPath = visibilityReadyState.ConfigRootPath,
            SettingsFilePath = visibilityReadyState.SettingsFilePath,
            StartupProfilePath = visibilityReadyState.StartupProfilePath,
            RequiredAssets = visibilityReadyState.RequiredAssets,
            ReadyAssetCount = visibilityReadyState.ReadyAssetCount,
            CompletedSteps = visibilityReadyState.CompletedSteps,
            TotalSteps = visibilityReadyState.TotalSteps,
            Exists = visibilityReadyState.Exists,
            ReadSucceeded = visibilityReadyState.ReadSucceeded
        };

        if (!visibilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser responsiveness session blocked for profile '{visibilityReadyState.ProfileId}'.";
            result.Error = visibilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResponsivenessSessionVersion = "runtime-browser-responsiveness-session-v1";
        result.BrowserResponsivenessStages =
        [
            "open-browser-responsiveness-session",
            "bind-browser-visibility-ready-state",
            "publish-browser-responsiveness-ready"
        ];
        result.BrowserResponsivenessSummary = $"Runtime browser responsiveness session prepared {result.BrowserResponsivenessStages.Length} responsiveness stage(s) for profile '{visibilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser responsiveness session ready for profile '{visibilityReadyState.ProfileId}' with {result.BrowserResponsivenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResponsivenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserResponsivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserVisibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserVisibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResponsivenessStages { get; set; } = Array.Empty<string>();
    public string BrowserResponsivenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
