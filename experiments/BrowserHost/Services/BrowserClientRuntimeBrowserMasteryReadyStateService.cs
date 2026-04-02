namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMasteryReadyState
{
    ValueTask<BrowserClientRuntimeBrowserMasteryReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMasteryReadyStateService : IBrowserClientRuntimeBrowserMasteryReadyState
{
    private readonly IBrowserClientRuntimeBrowserMasterySession _runtimeBrowserMasterySession;

    public BrowserClientRuntimeBrowserMasteryReadyStateService(IBrowserClientRuntimeBrowserMasterySession runtimeBrowserMasterySession)
    {
        _runtimeBrowserMasterySession = runtimeBrowserMasterySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMasteryReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMasterySessionResult masterySession = await _runtimeBrowserMasterySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserMasteryReadyStateResult result = new()
        {
            ProfileId = masterySession.ProfileId,
            SessionId = masterySession.SessionId,
            SessionPath = masterySession.SessionPath,
            BrowserMasterySessionVersion = masterySession.BrowserMasterySessionVersion,
            BrowserAgencyReadyStateVersion = masterySession.BrowserAgencyReadyStateVersion,
            BrowserAgencySessionVersion = masterySession.BrowserAgencySessionVersion,
            LaunchMode = masterySession.LaunchMode,
            AssetRootPath = masterySession.AssetRootPath,
            ProfilesRootPath = masterySession.ProfilesRootPath,
            CacheRootPath = masterySession.CacheRootPath,
            ConfigRootPath = masterySession.ConfigRootPath,
            SettingsFilePath = masterySession.SettingsFilePath,
            StartupProfilePath = masterySession.StartupProfilePath,
            RequiredAssets = masterySession.RequiredAssets,
            ReadyAssetCount = masterySession.ReadyAssetCount,
            CompletedSteps = masterySession.CompletedSteps,
            TotalSteps = masterySession.TotalSteps,
            Exists = masterySession.Exists,
            ReadSucceeded = masterySession.ReadSucceeded
        };

        if (!masterySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser mastery ready state blocked for profile '{masterySession.ProfileId}'.";
            result.Error = masterySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMasteryReadyStateVersion = "runtime-browser-mastery-ready-state-v1";
        result.BrowserMasteryReadyChecks =
        [
            "browser-agency-ready-state-ready",
            "browser-mastery-session-ready",
            "browser-mastery-ready"
        ];
        result.BrowserMasteryReadySummary = $"Runtime browser mastery ready state passed {result.BrowserMasteryReadyChecks.Length} mastery readiness check(s) for profile '{masterySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser mastery ready state ready for profile '{masterySession.ProfileId}' with {result.BrowserMasteryReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMasteryReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserMasteryReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMasterySessionVersion { get; set; } = string.Empty;
    public string BrowserAgencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAgencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMasteryReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserMasteryReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
