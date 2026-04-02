namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserUnderstandabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserUnderstandabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserUnderstandabilitySessionService : IBrowserClientRuntimeBrowserUnderstandabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSimplicityReadyState _runtimeBrowserSimplicityReadyState;

    public BrowserClientRuntimeBrowserUnderstandabilitySessionService(IBrowserClientRuntimeBrowserSimplicityReadyState runtimeBrowserSimplicityReadyState)
    {
        _runtimeBrowserSimplicityReadyState = runtimeBrowserSimplicityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserUnderstandabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSimplicityReadyStateResult simplicityReadyState = await _runtimeBrowserSimplicityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserUnderstandabilitySessionResult result = new()
        {
            ProfileId = simplicityReadyState.ProfileId,
            SessionId = simplicityReadyState.SessionId,
            SessionPath = simplicityReadyState.SessionPath,
            BrowserSimplicityReadyStateVersion = simplicityReadyState.BrowserSimplicityReadyStateVersion,
            BrowserSimplicitySessionVersion = simplicityReadyState.BrowserSimplicitySessionVersion,
            LaunchMode = simplicityReadyState.LaunchMode,
            AssetRootPath = simplicityReadyState.AssetRootPath,
            ProfilesRootPath = simplicityReadyState.ProfilesRootPath,
            CacheRootPath = simplicityReadyState.CacheRootPath,
            ConfigRootPath = simplicityReadyState.ConfigRootPath,
            SettingsFilePath = simplicityReadyState.SettingsFilePath,
            StartupProfilePath = simplicityReadyState.StartupProfilePath,
            RequiredAssets = simplicityReadyState.RequiredAssets,
            ReadyAssetCount = simplicityReadyState.ReadyAssetCount,
            CompletedSteps = simplicityReadyState.CompletedSteps,
            TotalSteps = simplicityReadyState.TotalSteps,
            Exists = simplicityReadyState.Exists,
            ReadSucceeded = simplicityReadyState.ReadSucceeded
        };

        if (!simplicityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser understandability session blocked for profile '{simplicityReadyState.ProfileId}'.";
            result.Error = simplicityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserUnderstandabilitySessionVersion = "runtime-browser-understandability-session-v1";
        result.BrowserUnderstandabilityStages =
        [
            "open-browser-understandability-session",
            "bind-browser-simplicity-ready-state",
            "publish-browser-understandability-ready"
        ];
        result.BrowserUnderstandabilitySummary = $"Runtime browser understandability session prepared {result.BrowserUnderstandabilityStages.Length} understandability stage(s) for profile '{simplicityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser understandability session ready for profile '{simplicityReadyState.ProfileId}' with {result.BrowserUnderstandabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserUnderstandabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserUnderstandabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSimplicityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSimplicitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserUnderstandabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserUnderstandabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

