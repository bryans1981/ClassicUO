namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentReadinessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentReadinessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentReadinessReadyStateService : IBrowserClientRuntimeBrowserSustainmentReadinessReadyState
{
    private readonly IBrowserClientRuntimeBrowserSustainmentReadinessSession _runtimeBrowserSustainmentReadinessSession;

    public BrowserClientRuntimeBrowserSustainmentReadinessReadyStateService(IBrowserClientRuntimeBrowserSustainmentReadinessSession runtimeBrowserSustainmentReadinessSession)
    {
        _runtimeBrowserSustainmentReadinessSession = runtimeBrowserSustainmentReadinessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentReadinessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentReadinessSessionResult sustainmentreadinessSession = await _runtimeBrowserSustainmentReadinessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentReadinessReadyStateResult result = new()
        {
            ProfileId = sustainmentreadinessSession.ProfileId,
            SessionId = sustainmentreadinessSession.SessionId,
            SessionPath = sustainmentreadinessSession.SessionPath,
            BrowserSustainmentReadinessSessionVersion = sustainmentreadinessSession.BrowserSustainmentReadinessSessionVersion,
            BrowserContinuationAssuranceReadyStateVersion = sustainmentreadinessSession.BrowserContinuationAssuranceReadyStateVersion,
            BrowserContinuationAssuranceSessionVersion = sustainmentreadinessSession.BrowserContinuationAssuranceSessionVersion,
            LaunchMode = sustainmentreadinessSession.LaunchMode,
            AssetRootPath = sustainmentreadinessSession.AssetRootPath,
            ProfilesRootPath = sustainmentreadinessSession.ProfilesRootPath,
            CacheRootPath = sustainmentreadinessSession.CacheRootPath,
            ConfigRootPath = sustainmentreadinessSession.ConfigRootPath,
            SettingsFilePath = sustainmentreadinessSession.SettingsFilePath,
            StartupProfilePath = sustainmentreadinessSession.StartupProfilePath,
            RequiredAssets = sustainmentreadinessSession.RequiredAssets,
            ReadyAssetCount = sustainmentreadinessSession.ReadyAssetCount,
            CompletedSteps = sustainmentreadinessSession.CompletedSteps,
            TotalSteps = sustainmentreadinessSession.TotalSteps,
            Exists = sustainmentreadinessSession.Exists,
            ReadSucceeded = sustainmentreadinessSession.ReadSucceeded
        };

        if (!sustainmentreadinessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainmentreadiness ready state blocked for profile '{sustainmentreadinessSession.ProfileId}'.";
            result.Error = sustainmentreadinessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentReadinessReadyStateVersion = "runtime-browser-sustainmentreadiness-ready-state-v1";
        result.BrowserSustainmentReadinessReadyChecks =
        [
            "browser-continuationassurance-ready-state-ready",
            "browser-sustainmentreadiness-session-ready",
            "browser-sustainmentreadiness-ready"
        ];
        result.BrowserSustainmentReadinessReadySummary = $"Runtime browser sustainmentreadiness ready state passed {result.BrowserSustainmentReadinessReadyChecks.Length} sustainmentreadiness readiness check(s) for profile '{sustainmentreadinessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentreadiness ready state ready for profile '{sustainmentreadinessSession.ProfileId}' with {result.BrowserSustainmentReadinessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentReadinessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainmentReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentReadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserContinuationAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainmentReadinessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentReadinessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
