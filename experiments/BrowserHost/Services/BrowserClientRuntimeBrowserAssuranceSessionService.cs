namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssuranceSessionService : IBrowserClientRuntimeBrowserAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserTrustReadyState _runtimeBrowserTrustReadyState;

    public BrowserClientRuntimeBrowserAssuranceSessionService(IBrowserClientRuntimeBrowserTrustReadyState runtimeBrowserTrustReadyState)
    {
        _runtimeBrowserTrustReadyState = runtimeBrowserTrustReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTrustReadyStateResult trustReadyState = await _runtimeBrowserTrustReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAssuranceSessionResult result = new()
        {
            ProfileId = trustReadyState.ProfileId,
            SessionId = trustReadyState.SessionId,
            SessionPath = trustReadyState.SessionPath,
            BrowserTrustReadyStateVersion = trustReadyState.BrowserTrustReadyStateVersion,
            BrowserTrustSessionVersion = trustReadyState.BrowserTrustSessionVersion,
            LaunchMode = trustReadyState.LaunchMode,
            AssetRootPath = trustReadyState.AssetRootPath,
            ProfilesRootPath = trustReadyState.ProfilesRootPath,
            CacheRootPath = trustReadyState.CacheRootPath,
            ConfigRootPath = trustReadyState.ConfigRootPath,
            SettingsFilePath = trustReadyState.SettingsFilePath,
            StartupProfilePath = trustReadyState.StartupProfilePath,
            RequiredAssets = trustReadyState.RequiredAssets,
            ReadyAssetCount = trustReadyState.ReadyAssetCount,
            CompletedSteps = trustReadyState.CompletedSteps,
            TotalSteps = trustReadyState.TotalSteps,
            Exists = trustReadyState.Exists,
            ReadSucceeded = trustReadyState.ReadSucceeded,
            BrowserTrustReadyState = trustReadyState
        };

        if (!trustReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assurance session blocked for profile '{trustReadyState.ProfileId}'.";
            result.Error = trustReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssuranceSessionVersion = "runtime-browser-assurance-session-v1";
        result.BrowserAssuranceStages =
        [
            "open-browser-assurance-session",
            "bind-browser-trust-ready-state",
            "publish-browser-assurance-ready"
        ];
        result.BrowserAssuranceSummary = $"Runtime browser assurance session prepared {result.BrowserAssuranceStages.Length} assurance stage(s) for profile '{trustReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assurance session ready for profile '{trustReadyState.ProfileId}' with {result.BrowserAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssuranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserTrustReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserAssuranceSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserTrustReadyStateResult BrowserTrustReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
