namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInvolvementSession
{
    ValueTask<BrowserClientRuntimeBrowserInvolvementSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInvolvementSessionService : IBrowserClientRuntimeBrowserInvolvementSession
{
    private readonly IBrowserClientRuntimeBrowserMomentumReadyState _runtimeBrowserMomentumReadyState;

    public BrowserClientRuntimeBrowserInvolvementSessionService(IBrowserClientRuntimeBrowserMomentumReadyState runtimeBrowserMomentumReadyState)
    {
        _runtimeBrowserMomentumReadyState = runtimeBrowserMomentumReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInvolvementSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMomentumReadyStateResult momentumReadyState = await _runtimeBrowserMomentumReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInvolvementSessionResult result = new()
        {
            ProfileId = momentumReadyState.ProfileId,
            SessionId = momentumReadyState.SessionId,
            SessionPath = momentumReadyState.SessionPath,
            BrowserMomentumReadyStateVersion = momentumReadyState.BrowserMomentumReadyStateVersion,
            BrowserMomentumSessionVersion = momentumReadyState.BrowserMomentumSessionVersion,
            LaunchMode = momentumReadyState.LaunchMode,
            AssetRootPath = momentumReadyState.AssetRootPath,
            ProfilesRootPath = momentumReadyState.ProfilesRootPath,
            CacheRootPath = momentumReadyState.CacheRootPath,
            ConfigRootPath = momentumReadyState.ConfigRootPath,
            SettingsFilePath = momentumReadyState.SettingsFilePath,
            StartupProfilePath = momentumReadyState.StartupProfilePath,
            RequiredAssets = momentumReadyState.RequiredAssets,
            ReadyAssetCount = momentumReadyState.ReadyAssetCount,
            CompletedSteps = momentumReadyState.CompletedSteps,
            TotalSteps = momentumReadyState.TotalSteps,
            Exists = momentumReadyState.Exists,
            ReadSucceeded = momentumReadyState.ReadSucceeded
        };

        if (!momentumReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser involvement session blocked for profile '{momentumReadyState.ProfileId}'.";
            result.Error = momentumReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInvolvementSessionVersion = "runtime-browser-involvement-session-v1";
        result.BrowserInvolvementStages =
        [
            "open-browser-involvement-session",
            "bind-browser-momentum-ready-state",
            "publish-browser-involvement-ready"
        ];
        result.BrowserInvolvementSummary = $"Runtime browser involvement session prepared {result.BrowserInvolvementStages.Length} involvement stage(s) for profile '{momentumReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser involvement session ready for profile '{momentumReadyState.ProfileId}' with {result.BrowserInvolvementStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInvolvementSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInvolvementSessionVersion { get; set; } = string.Empty;
    public string BrowserMomentumReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMomentumSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInvolvementStages { get; set; } = Array.Empty<string>();
    public string BrowserInvolvementSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
