namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssurance2Session
{
    ValueTask<BrowserClientRuntimeBrowserAssurance2SessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssurance2SessionService : IBrowserClientRuntimeBrowserAssurance2Session
{
    private readonly IBrowserClientRuntimeBrowserTrustworthinessReadyState _runtimeBrowserTrustworthinessReadyState;

    public BrowserClientRuntimeBrowserAssurance2SessionService(IBrowserClientRuntimeBrowserTrustworthinessReadyState runtimeBrowserTrustworthinessReadyState)
    {
        _runtimeBrowserTrustworthinessReadyState = runtimeBrowserTrustworthinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssurance2SessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTrustworthinessReadyStateResult trustworthinessReadyState = await _runtimeBrowserTrustworthinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAssurance2SessionResult result = new()
        {
            ProfileId = trustworthinessReadyState.ProfileId,
            SessionId = trustworthinessReadyState.SessionId,
            SessionPath = trustworthinessReadyState.SessionPath,
            BrowserTrustworthinessReadyStateVersion = trustworthinessReadyState.BrowserTrustworthinessReadyStateVersion,
            BrowserTrustworthinessSessionVersion = trustworthinessReadyState.BrowserTrustworthinessSessionVersion,
            LaunchMode = trustworthinessReadyState.LaunchMode,
            AssetRootPath = trustworthinessReadyState.AssetRootPath,
            ProfilesRootPath = trustworthinessReadyState.ProfilesRootPath,
            CacheRootPath = trustworthinessReadyState.CacheRootPath,
            ConfigRootPath = trustworthinessReadyState.ConfigRootPath,
            SettingsFilePath = trustworthinessReadyState.SettingsFilePath,
            StartupProfilePath = trustworthinessReadyState.StartupProfilePath,
            RequiredAssets = trustworthinessReadyState.RequiredAssets,
            ReadyAssetCount = trustworthinessReadyState.ReadyAssetCount,
            CompletedSteps = trustworthinessReadyState.CompletedSteps,
            TotalSteps = trustworthinessReadyState.TotalSteps,
            Exists = trustworthinessReadyState.Exists,
            ReadSucceeded = trustworthinessReadyState.ReadSucceeded
        };

        if (!trustworthinessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assurance2 session blocked for profile '{trustworthinessReadyState.ProfileId}'.";
            result.Error = trustworthinessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssurance2SessionVersion = "runtime-browser-assurance2-session-v1";
        result.BrowserAssurance2Stages =
        [
            "open-browser-assurance2-session",
            "bind-browser-trustworthiness-ready-state",
            "publish-browser-assurance2-ready"
        ];
        result.BrowserAssurance2Summary = $"Runtime browser assurance2 session prepared {result.BrowserAssurance2Stages.Length} assurance2 stage(s) for profile '{trustworthinessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assurance2 session ready for profile '{trustworthinessReadyState.ProfileId}' with {result.BrowserAssurance2Stages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssurance2SessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAssurance2SessionVersion { get; set; } = string.Empty;
    public string BrowserTrustworthinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustworthinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssurance2Stages { get; set; } = Array.Empty<string>();
    public string BrowserAssurance2Summary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
