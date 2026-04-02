namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPersonalizationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPersonalizationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPersonalizationReadyStateService : IBrowserClientRuntimeBrowserPersonalizationReadyState
{
    private readonly IBrowserClientRuntimeBrowserPersonalizationSession _runtimeBrowserPersonalizationSession;

    public BrowserClientRuntimeBrowserPersonalizationReadyStateService(IBrowserClientRuntimeBrowserPersonalizationSession runtimeBrowserPersonalizationSession)
    {
        _runtimeBrowserPersonalizationSession = runtimeBrowserPersonalizationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPersonalizationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPersonalizationSessionResult personalizationSession = await _runtimeBrowserPersonalizationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPersonalizationReadyStateResult result = new()
        {
            ProfileId = personalizationSession.ProfileId,
            SessionId = personalizationSession.SessionId,
            SessionPath = personalizationSession.SessionPath,
            BrowserPersonalizationSessionVersion = personalizationSession.BrowserPersonalizationSessionVersion,
            BrowserFlexibilityReadyStateVersion = personalizationSession.BrowserFlexibilityReadyStateVersion,
            BrowserFlexibilitySessionVersion = personalizationSession.BrowserFlexibilitySessionVersion,
            LaunchMode = personalizationSession.LaunchMode,
            AssetRootPath = personalizationSession.AssetRootPath,
            ProfilesRootPath = personalizationSession.ProfilesRootPath,
            CacheRootPath = personalizationSession.CacheRootPath,
            ConfigRootPath = personalizationSession.ConfigRootPath,
            SettingsFilePath = personalizationSession.SettingsFilePath,
            StartupProfilePath = personalizationSession.StartupProfilePath,
            RequiredAssets = personalizationSession.RequiredAssets,
            ReadyAssetCount = personalizationSession.ReadyAssetCount,
            CompletedSteps = personalizationSession.CompletedSteps,
            TotalSteps = personalizationSession.TotalSteps,
            Exists = personalizationSession.Exists,
            ReadSucceeded = personalizationSession.ReadSucceeded
        };

        if (!personalizationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser personalization ready state blocked for profile '{personalizationSession.ProfileId}'.";
            result.Error = personalizationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPersonalizationReadyStateVersion = "runtime-browser-personalization-ready-state-v1";
        result.BrowserPersonalizationReadyChecks =
        [
            "browser-flexibility-ready-state-ready",
            "browser-personalization-session-ready",
            "browser-personalization-ready"
        ];
        result.BrowserPersonalizationReadySummary = $"Runtime browser personalization ready state passed {result.BrowserPersonalizationReadyChecks.Length} personalization readiness check(s) for profile '{personalizationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser personalization ready state ready for profile '{personalizationSession.ProfileId}' with {result.BrowserPersonalizationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPersonalizationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPersonalizationReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserPersonalizationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPersonalizationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
