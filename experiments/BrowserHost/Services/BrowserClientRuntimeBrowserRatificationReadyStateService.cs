namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRatificationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRatificationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRatificationReadyStateService : IBrowserClientRuntimeBrowserRatificationReadyState
{
    private readonly IBrowserClientRuntimeBrowserRatificationSession _runtimeBrowserRatificationSession;

    public BrowserClientRuntimeBrowserRatificationReadyStateService(IBrowserClientRuntimeBrowserRatificationSession runtimeBrowserRatificationSession)
    {
        _runtimeBrowserRatificationSession = runtimeBrowserRatificationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRatificationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRatificationSessionResult ratificationSession = await _runtimeBrowserRatificationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRatificationReadyStateResult result = new()
        {
            ProfileId = ratificationSession.ProfileId,
            SessionId = ratificationSession.SessionId,
            SessionPath = ratificationSession.SessionPath,
            BrowserRatificationSessionVersion = ratificationSession.BrowserRatificationSessionVersion,
            BrowserAffirmationReadyStateVersion = ratificationSession.BrowserAffirmationReadyStateVersion,
            BrowserAffirmationSessionVersion = ratificationSession.BrowserAffirmationSessionVersion,
            LaunchMode = ratificationSession.LaunchMode,
            AssetRootPath = ratificationSession.AssetRootPath,
            ProfilesRootPath = ratificationSession.ProfilesRootPath,
            CacheRootPath = ratificationSession.CacheRootPath,
            ConfigRootPath = ratificationSession.ConfigRootPath,
            SettingsFilePath = ratificationSession.SettingsFilePath,
            StartupProfilePath = ratificationSession.StartupProfilePath,
            RequiredAssets = ratificationSession.RequiredAssets,
            ReadyAssetCount = ratificationSession.ReadyAssetCount,
            CompletedSteps = ratificationSession.CompletedSteps,
            TotalSteps = ratificationSession.TotalSteps,
            Exists = ratificationSession.Exists,
            ReadSucceeded = ratificationSession.ReadSucceeded
        };

        if (!ratificationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser ratification ready state blocked for profile '{ratificationSession.ProfileId}'.";
            result.Error = ratificationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRatificationReadyStateVersion = "runtime-browser-ratification-ready-state-v1";
        result.BrowserRatificationReadyChecks =
        [
            "browser-affirmation-ready-state-ready",
            "browser-ratification-session-ready",
            "browser-ratification-ready"
        ];
        result.BrowserRatificationReadySummary = $"Runtime browser ratification ready state passed {result.BrowserRatificationReadyChecks.Length} ratification readiness check(s) for profile '{ratificationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser ratification ready state ready for profile '{ratificationSession.ProfileId}' with {result.BrowserRatificationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRatificationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRatificationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRatificationSessionVersion { get; set; } = string.Empty;
    public string BrowserAffirmationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAffirmationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRatificationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRatificationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
