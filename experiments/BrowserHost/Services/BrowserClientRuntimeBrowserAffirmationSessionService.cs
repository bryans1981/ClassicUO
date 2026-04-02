namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAffirmationSession
{
    ValueTask<BrowserClientRuntimeBrowserAffirmationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAffirmationSessionService : IBrowserClientRuntimeBrowserAffirmationSession
{
    private readonly IBrowserClientRuntimeBrowserCorroborationReadyState _runtimeBrowserCorroborationReadyState;

    public BrowserClientRuntimeBrowserAffirmationSessionService(IBrowserClientRuntimeBrowserCorroborationReadyState runtimeBrowserCorroborationReadyState)
    {
        _runtimeBrowserCorroborationReadyState = runtimeBrowserCorroborationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAffirmationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCorroborationReadyStateResult corroborationReadyState = await _runtimeBrowserCorroborationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAffirmationSessionResult result = new()
        {
            ProfileId = corroborationReadyState.ProfileId,
            SessionId = corroborationReadyState.SessionId,
            SessionPath = corroborationReadyState.SessionPath,
            BrowserCorroborationReadyStateVersion = corroborationReadyState.BrowserCorroborationReadyStateVersion,
            BrowserCorroborationSessionVersion = corroborationReadyState.BrowserCorroborationSessionVersion,
            LaunchMode = corroborationReadyState.LaunchMode,
            AssetRootPath = corroborationReadyState.AssetRootPath,
            ProfilesRootPath = corroborationReadyState.ProfilesRootPath,
            CacheRootPath = corroborationReadyState.CacheRootPath,
            ConfigRootPath = corroborationReadyState.ConfigRootPath,
            SettingsFilePath = corroborationReadyState.SettingsFilePath,
            StartupProfilePath = corroborationReadyState.StartupProfilePath,
            RequiredAssets = corroborationReadyState.RequiredAssets,
            ReadyAssetCount = corroborationReadyState.ReadyAssetCount,
            CompletedSteps = corroborationReadyState.CompletedSteps,
            TotalSteps = corroborationReadyState.TotalSteps,
            Exists = corroborationReadyState.Exists,
            ReadSucceeded = corroborationReadyState.ReadSucceeded
        };

        if (!corroborationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser affirmation session blocked for profile '{corroborationReadyState.ProfileId}'.";
            result.Error = corroborationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAffirmationSessionVersion = "runtime-browser-affirmation-session-v1";
        result.BrowserAffirmationStages =
        [
            "open-browser-affirmation-session",
            "bind-browser-corroboration-ready-state",
            "publish-browser-affirmation-ready"
        ];
        result.BrowserAffirmationSummary = $"Runtime browser affirmation session prepared {result.BrowserAffirmationStages.Length} affirmation stage(s) for profile '{corroborationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser affirmation session ready for profile '{corroborationReadyState.ProfileId}' with {result.BrowserAffirmationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAffirmationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAffirmationSessionVersion { get; set; } = string.Empty;
    public string BrowserCorroborationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCorroborationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAffirmationStages { get; set; } = Array.Empty<string>();
    public string BrowserAffirmationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
