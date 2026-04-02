namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMomentumSession
{
    ValueTask<BrowserClientRuntimeBrowserMomentumSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMomentumSessionService : IBrowserClientRuntimeBrowserMomentumSession
{
    private readonly IBrowserClientRuntimeBrowserAimfulnessReadyState _runtimeBrowserAimfulnessReadyState;

    public BrowserClientRuntimeBrowserMomentumSessionService(IBrowserClientRuntimeBrowserAimfulnessReadyState runtimeBrowserAimfulnessReadyState)
    {
        _runtimeBrowserAimfulnessReadyState = runtimeBrowserAimfulnessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMomentumSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAimfulnessReadyStateResult aimfulnessReadyState = await _runtimeBrowserAimfulnessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserMomentumSessionResult result = new()
        {
            ProfileId = aimfulnessReadyState.ProfileId,
            SessionId = aimfulnessReadyState.SessionId,
            SessionPath = aimfulnessReadyState.SessionPath,
            BrowserAimfulnessReadyStateVersion = aimfulnessReadyState.BrowserAimfulnessReadyStateVersion,
            BrowserAimfulnessSessionVersion = aimfulnessReadyState.BrowserAimfulnessSessionVersion,
            LaunchMode = aimfulnessReadyState.LaunchMode,
            AssetRootPath = aimfulnessReadyState.AssetRootPath,
            ProfilesRootPath = aimfulnessReadyState.ProfilesRootPath,
            CacheRootPath = aimfulnessReadyState.CacheRootPath,
            ConfigRootPath = aimfulnessReadyState.ConfigRootPath,
            SettingsFilePath = aimfulnessReadyState.SettingsFilePath,
            StartupProfilePath = aimfulnessReadyState.StartupProfilePath,
            RequiredAssets = aimfulnessReadyState.RequiredAssets,
            ReadyAssetCount = aimfulnessReadyState.ReadyAssetCount,
            CompletedSteps = aimfulnessReadyState.CompletedSteps,
            TotalSteps = aimfulnessReadyState.TotalSteps,
            Exists = aimfulnessReadyState.Exists,
            ReadSucceeded = aimfulnessReadyState.ReadSucceeded
        };

        if (!aimfulnessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser momentum session blocked for profile '{aimfulnessReadyState.ProfileId}'.";
            result.Error = aimfulnessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMomentumSessionVersion = "runtime-browser-momentum-session-v1";
        result.BrowserMomentumStages =
        [
            "open-browser-momentum-session",
            "bind-browser-aimfulness-ready-state",
            "publish-browser-momentum-ready"
        ];
        result.BrowserMomentumSummary = $"Runtime browser momentum session prepared {result.BrowserMomentumStages.Length} momentum stage(s) for profile '{aimfulnessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser momentum session ready for profile '{aimfulnessReadyState.ProfileId}' with {result.BrowserMomentumStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMomentumSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserMomentumSessionVersion { get; set; } = string.Empty;
    public string BrowserAimfulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAimfulnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMomentumStages { get; set; } = Array.Empty<string>();
    public string BrowserMomentumSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
