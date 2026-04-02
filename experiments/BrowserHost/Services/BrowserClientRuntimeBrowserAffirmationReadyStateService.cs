namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAffirmationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAffirmationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAffirmationReadyStateService : IBrowserClientRuntimeBrowserAffirmationReadyState
{
    private readonly IBrowserClientRuntimeBrowserAffirmationSession _runtimeBrowserAffirmationSession;

    public BrowserClientRuntimeBrowserAffirmationReadyStateService(IBrowserClientRuntimeBrowserAffirmationSession runtimeBrowserAffirmationSession)
    {
        _runtimeBrowserAffirmationSession = runtimeBrowserAffirmationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAffirmationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAffirmationSessionResult affirmationSession = await _runtimeBrowserAffirmationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAffirmationReadyStateResult result = new()
        {
            ProfileId = affirmationSession.ProfileId,
            SessionId = affirmationSession.SessionId,
            SessionPath = affirmationSession.SessionPath,
            BrowserAffirmationSessionVersion = affirmationSession.BrowserAffirmationSessionVersion,
            BrowserCorroborationReadyStateVersion = affirmationSession.BrowserCorroborationReadyStateVersion,
            BrowserCorroborationSessionVersion = affirmationSession.BrowserCorroborationSessionVersion,
            LaunchMode = affirmationSession.LaunchMode,
            AssetRootPath = affirmationSession.AssetRootPath,
            ProfilesRootPath = affirmationSession.ProfilesRootPath,
            CacheRootPath = affirmationSession.CacheRootPath,
            ConfigRootPath = affirmationSession.ConfigRootPath,
            SettingsFilePath = affirmationSession.SettingsFilePath,
            StartupProfilePath = affirmationSession.StartupProfilePath,
            RequiredAssets = affirmationSession.RequiredAssets,
            ReadyAssetCount = affirmationSession.ReadyAssetCount,
            CompletedSteps = affirmationSession.CompletedSteps,
            TotalSteps = affirmationSession.TotalSteps,
            Exists = affirmationSession.Exists,
            ReadSucceeded = affirmationSession.ReadSucceeded
        };

        if (!affirmationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser affirmation ready state blocked for profile '{affirmationSession.ProfileId}'.";
            result.Error = affirmationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAffirmationReadyStateVersion = "runtime-browser-affirmation-ready-state-v1";
        result.BrowserAffirmationReadyChecks =
        [
            "browser-corroboration-ready-state-ready",
            "browser-affirmation-session-ready",
            "browser-affirmation-ready"
        ];
        result.BrowserAffirmationReadySummary = $"Runtime browser affirmation ready state passed {result.BrowserAffirmationReadyChecks.Length} affirmation readiness check(s) for profile '{affirmationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser affirmation ready state ready for profile '{affirmationSession.ProfileId}' with {result.BrowserAffirmationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAffirmationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAffirmationReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserAffirmationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAffirmationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
