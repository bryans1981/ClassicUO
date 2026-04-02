namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCertaintySession
{
    ValueTask<BrowserClientRuntimeBrowserCertaintySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCertaintySessionService : IBrowserClientRuntimeBrowserCertaintySession
{
    private readonly IBrowserClientRuntimeBrowserAssurednessReadyState _runtimeBrowserAssurednessReadyState;

    public BrowserClientRuntimeBrowserCertaintySessionService(IBrowserClientRuntimeBrowserAssurednessReadyState runtimeBrowserAssurednessReadyState)
    {
        _runtimeBrowserAssurednessReadyState = runtimeBrowserAssurednessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCertaintySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssurednessReadyStateResult assurednessReadyState = await _runtimeBrowserAssurednessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCertaintySessionResult result = new()
        {
            ProfileId = assurednessReadyState.ProfileId,
            SessionId = assurednessReadyState.SessionId,
            SessionPath = assurednessReadyState.SessionPath,
            BrowserAssurednessReadyStateVersion = assurednessReadyState.BrowserAssurednessReadyStateVersion,
            BrowserAssurednessSessionVersion = assurednessReadyState.BrowserAssurednessSessionVersion,
            LaunchMode = assurednessReadyState.LaunchMode,
            AssetRootPath = assurednessReadyState.AssetRootPath,
            ProfilesRootPath = assurednessReadyState.ProfilesRootPath,
            CacheRootPath = assurednessReadyState.CacheRootPath,
            ConfigRootPath = assurednessReadyState.ConfigRootPath,
            SettingsFilePath = assurednessReadyState.SettingsFilePath,
            StartupProfilePath = assurednessReadyState.StartupProfilePath,
            RequiredAssets = assurednessReadyState.RequiredAssets,
            ReadyAssetCount = assurednessReadyState.ReadyAssetCount,
            CompletedSteps = assurednessReadyState.CompletedSteps,
            TotalSteps = assurednessReadyState.TotalSteps,
            Exists = assurednessReadyState.Exists,
            ReadSucceeded = assurednessReadyState.ReadSucceeded
        };

        if (!assurednessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser certainty session blocked for profile '{assurednessReadyState.ProfileId}'.";
            result.Error = assurednessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCertaintySessionVersion = "runtime-browser-certainty-session-v1";
        result.BrowserCertaintyStages =
        [
            "open-browser-certainty-session",
            "bind-browser-assuredness-ready-state",
            "publish-browser-certainty-ready"
        ];
        result.BrowserCertaintySummary = $"Runtime browser certainty session prepared {result.BrowserCertaintyStages.Length} certainty stage(s) for profile '{assurednessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser certainty session ready for profile '{assurednessReadyState.ProfileId}' with {result.BrowserCertaintyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCertaintySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCertaintySessionVersion { get; set; } = string.Empty;
    public string BrowserAssurednessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurednessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCertaintyStages { get; set; } = Array.Empty<string>();
    public string BrowserCertaintySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
