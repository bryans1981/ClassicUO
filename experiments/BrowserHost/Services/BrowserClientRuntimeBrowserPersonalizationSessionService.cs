namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPersonalizationSession
{
    ValueTask<BrowserClientRuntimeBrowserPersonalizationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPersonalizationSessionService : IBrowserClientRuntimeBrowserPersonalizationSession
{
    private readonly IBrowserClientRuntimeBrowserFlexibilityReadyState _runtimeBrowserFlexibilityReadyState;

    public BrowserClientRuntimeBrowserPersonalizationSessionService(IBrowserClientRuntimeBrowserFlexibilityReadyState runtimeBrowserFlexibilityReadyState)
    {
        _runtimeBrowserFlexibilityReadyState = runtimeBrowserFlexibilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPersonalizationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlexibilityReadyStateResult flexibilityReadyState = await _runtimeBrowserFlexibilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPersonalizationSessionResult result = new()
        {
            ProfileId = flexibilityReadyState.ProfileId,
            SessionId = flexibilityReadyState.SessionId,
            SessionPath = flexibilityReadyState.SessionPath,
            BrowserFlexibilityReadyStateVersion = flexibilityReadyState.BrowserFlexibilityReadyStateVersion,
            BrowserFlexibilitySessionVersion = flexibilityReadyState.BrowserFlexibilitySessionVersion,
            LaunchMode = flexibilityReadyState.LaunchMode,
            AssetRootPath = flexibilityReadyState.AssetRootPath,
            ProfilesRootPath = flexibilityReadyState.ProfilesRootPath,
            CacheRootPath = flexibilityReadyState.CacheRootPath,
            ConfigRootPath = flexibilityReadyState.ConfigRootPath,
            SettingsFilePath = flexibilityReadyState.SettingsFilePath,
            StartupProfilePath = flexibilityReadyState.StartupProfilePath,
            RequiredAssets = flexibilityReadyState.RequiredAssets,
            ReadyAssetCount = flexibilityReadyState.ReadyAssetCount,
            CompletedSteps = flexibilityReadyState.CompletedSteps,
            TotalSteps = flexibilityReadyState.TotalSteps,
            Exists = flexibilityReadyState.Exists,
            ReadSucceeded = flexibilityReadyState.ReadSucceeded
        };

        if (!flexibilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser personalization session blocked for profile '{flexibilityReadyState.ProfileId}'.";
            result.Error = flexibilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPersonalizationSessionVersion = "runtime-browser-personalization-session-v1";
        result.BrowserPersonalizationStages =
        [
            "open-browser-personalization-session",
            "bind-browser-flexibility-ready-state",
            "publish-browser-personalization-ready"
        ];
        result.BrowserPersonalizationSummary = $"Runtime browser personalization session prepared {result.BrowserPersonalizationStages.Length} personalization stage(s) for profile '{flexibilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser personalization session ready for profile '{flexibilityReadyState.ProfileId}' with {result.BrowserPersonalizationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPersonalizationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPersonalizationSessionVersion { get; set; } = string.Empty;
    public string BrowserFlexibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlexibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPersonalizationStages { get; set; } = Array.Empty<string>();
    public string BrowserPersonalizationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
