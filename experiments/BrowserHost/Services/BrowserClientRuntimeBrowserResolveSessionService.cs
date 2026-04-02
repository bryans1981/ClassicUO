namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResolveSession
{
    ValueTask<BrowserClientRuntimeBrowserResolveSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResolveSessionService : IBrowserClientRuntimeBrowserResolveSession
{
    private readonly IBrowserClientRuntimeBrowserTenacityReadyState _runtimeBrowserTenacityReadyState;

    public BrowserClientRuntimeBrowserResolveSessionService(IBrowserClientRuntimeBrowserTenacityReadyState runtimeBrowserTenacityReadyState)
    {
        _runtimeBrowserTenacityReadyState = runtimeBrowserTenacityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResolveSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTenacityReadyStateResult tenacityReadyState = await _runtimeBrowserTenacityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserResolveSessionResult result = new()
        {
            ProfileId = tenacityReadyState.ProfileId,
            SessionId = tenacityReadyState.SessionId,
            SessionPath = tenacityReadyState.SessionPath,
            BrowserTenacityReadyStateVersion = tenacityReadyState.BrowserTenacityReadyStateVersion,
            BrowserTenacitySessionVersion = tenacityReadyState.BrowserTenacitySessionVersion,
            LaunchMode = tenacityReadyState.LaunchMode,
            AssetRootPath = tenacityReadyState.AssetRootPath,
            ProfilesRootPath = tenacityReadyState.ProfilesRootPath,
            CacheRootPath = tenacityReadyState.CacheRootPath,
            ConfigRootPath = tenacityReadyState.ConfigRootPath,
            SettingsFilePath = tenacityReadyState.SettingsFilePath,
            StartupProfilePath = tenacityReadyState.StartupProfilePath,
            RequiredAssets = tenacityReadyState.RequiredAssets,
            ReadyAssetCount = tenacityReadyState.ReadyAssetCount,
            CompletedSteps = tenacityReadyState.CompletedSteps,
            TotalSteps = tenacityReadyState.TotalSteps,
            Exists = tenacityReadyState.Exists,
            ReadSucceeded = tenacityReadyState.ReadSucceeded
        };

        if (!tenacityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resolve session blocked for profile '{tenacityReadyState.ProfileId}'.";
            result.Error = tenacityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResolveSessionVersion = "runtime-browser-resolve-session-v1";
        result.BrowserResolveStages =
        [
            "open-browser-resolve-session",
            "bind-browser-tenacity-ready-state",
            "publish-browser-resolve-ready"
        ];
        result.BrowserResolveSummary = $"Runtime browser resolve session prepared {result.BrowserResolveStages.Length} resolve stage(s) for profile '{tenacityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resolve session ready for profile '{tenacityReadyState.ProfileId}' with {result.BrowserResolveStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResolveSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserResolveSessionVersion { get; set; } = string.Empty;
    public string BrowserTenacityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTenacitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResolveStages { get; set; } = Array.Empty<string>();
    public string BrowserResolveSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
