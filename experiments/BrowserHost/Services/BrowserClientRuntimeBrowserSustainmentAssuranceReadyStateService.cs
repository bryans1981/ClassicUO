namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateService : IBrowserClientRuntimeBrowserSustainmentAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserSustainmentAssuranceSession _runtimeBrowserSustainmentAssuranceSession;

    public BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateService(IBrowserClientRuntimeBrowserSustainmentAssuranceSession runtimeBrowserSustainmentAssuranceSession)
    {
        _runtimeBrowserSustainmentAssuranceSession = runtimeBrowserSustainmentAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentAssuranceSessionResult sustainmentassuranceSession = await _runtimeBrowserSustainmentAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateResult result = new()
        {
            ProfileId = sustainmentassuranceSession.ProfileId,
            SessionId = sustainmentassuranceSession.SessionId,
            SessionPath = sustainmentassuranceSession.SessionPath,
            BrowserSustainmentAssuranceSessionVersion = sustainmentassuranceSession.BrowserSustainmentAssuranceSessionVersion,
            BrowserContinuationReadinessReadyStateVersion = sustainmentassuranceSession.BrowserContinuationReadinessReadyStateVersion,
            BrowserContinuationReadinessSessionVersion = sustainmentassuranceSession.BrowserContinuationReadinessSessionVersion,
            LaunchMode = sustainmentassuranceSession.LaunchMode,
            AssetRootPath = sustainmentassuranceSession.AssetRootPath,
            ProfilesRootPath = sustainmentassuranceSession.ProfilesRootPath,
            CacheRootPath = sustainmentassuranceSession.CacheRootPath,
            ConfigRootPath = sustainmentassuranceSession.ConfigRootPath,
            SettingsFilePath = sustainmentassuranceSession.SettingsFilePath,
            StartupProfilePath = sustainmentassuranceSession.StartupProfilePath,
            RequiredAssets = sustainmentassuranceSession.RequiredAssets,
            ReadyAssetCount = sustainmentassuranceSession.ReadyAssetCount,
            CompletedSteps = sustainmentassuranceSession.CompletedSteps,
            TotalSteps = sustainmentassuranceSession.TotalSteps,
            Exists = sustainmentassuranceSession.Exists,
            ReadSucceeded = sustainmentassuranceSession.ReadSucceeded
        };

        if (!sustainmentassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainmentassurance ready state blocked for profile '{sustainmentassuranceSession.ProfileId}'.";
            result.Error = sustainmentassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentAssuranceReadyStateVersion = "runtime-browser-sustainmentassurance-ready-state-v1";
        result.BrowserSustainmentAssuranceReadyChecks =
        [
            "browser-continuationreadiness-ready-state-ready",
            "browser-sustainmentassurance-session-ready",
            "browser-sustainmentassurance-ready"
        ];
        result.BrowserSustainmentAssuranceReadySummary = $"Runtime browser sustainmentassurance ready state passed {result.BrowserSustainmentAssuranceReadyChecks.Length} sustainmentassurance readiness check(s) for profile '{sustainmentassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentassurance ready state ready for profile '{sustainmentassuranceSession.ProfileId}' with {result.BrowserSustainmentAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainmentAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserContinuationReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainmentAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
