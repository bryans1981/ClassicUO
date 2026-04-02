namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssurednessSession
{
    ValueTask<BrowserClientRuntimeBrowserAssurednessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssurednessSessionService : IBrowserClientRuntimeBrowserAssurednessSession
{
    private readonly IBrowserClientRuntimeBrowserCalmnessReadyState _runtimeBrowserCalmnessReadyState;

    public BrowserClientRuntimeBrowserAssurednessSessionService(IBrowserClientRuntimeBrowserCalmnessReadyState runtimeBrowserCalmnessReadyState)
    {
        _runtimeBrowserCalmnessReadyState = runtimeBrowserCalmnessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssurednessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCalmnessReadyStateResult calmnessReadyState = await _runtimeBrowserCalmnessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAssurednessSessionResult result = new()
        {
            ProfileId = calmnessReadyState.ProfileId,
            SessionId = calmnessReadyState.SessionId,
            SessionPath = calmnessReadyState.SessionPath,
            BrowserCalmnessReadyStateVersion = calmnessReadyState.BrowserCalmnessReadyStateVersion,
            BrowserCalmnessSessionVersion = calmnessReadyState.BrowserCalmnessSessionVersion,
            LaunchMode = calmnessReadyState.LaunchMode,
            AssetRootPath = calmnessReadyState.AssetRootPath,
            ProfilesRootPath = calmnessReadyState.ProfilesRootPath,
            CacheRootPath = calmnessReadyState.CacheRootPath,
            ConfigRootPath = calmnessReadyState.ConfigRootPath,
            SettingsFilePath = calmnessReadyState.SettingsFilePath,
            StartupProfilePath = calmnessReadyState.StartupProfilePath,
            RequiredAssets = calmnessReadyState.RequiredAssets,
            ReadyAssetCount = calmnessReadyState.ReadyAssetCount,
            CompletedSteps = calmnessReadyState.CompletedSteps,
            TotalSteps = calmnessReadyState.TotalSteps,
            Exists = calmnessReadyState.Exists,
            ReadSucceeded = calmnessReadyState.ReadSucceeded
        };

        if (!calmnessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assuredness session blocked for profile '{calmnessReadyState.ProfileId}'.";
            result.Error = calmnessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssurednessSessionVersion = "runtime-browser-assuredness-session-v1";
        result.BrowserAssurednessStages =
        [
            "open-browser-assuredness-session",
            "bind-browser-calmness-ready-state",
            "publish-browser-assuredness-ready"
        ];
        result.BrowserAssurednessSummary = $"Runtime browser assuredness session prepared {result.BrowserAssurednessStages.Length} assuredness stage(s) for profile '{calmnessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assuredness session ready for profile '{calmnessReadyState.ProfileId}' with {result.BrowserAssurednessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssurednessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAssurednessSessionVersion { get; set; } = string.Empty;
    public string BrowserCalmnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCalmnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssurednessStages { get; set; } = Array.Empty<string>();
    public string BrowserAssurednessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
