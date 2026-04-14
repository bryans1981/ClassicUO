namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentReadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentReadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentReadinessSessionService : IBrowserClientRuntimeBrowserSustainmentReadinessSession
{
    private readonly IBrowserClientRuntimeBrowserContinuationAssuranceReadyState _runtimeBrowserContinuationAssuranceReadyState;

    public BrowserClientRuntimeBrowserSustainmentReadinessSessionService(IBrowserClientRuntimeBrowserContinuationAssuranceReadyState runtimeBrowserContinuationAssuranceReadyState)
    {
        _runtimeBrowserContinuationAssuranceReadyState = runtimeBrowserContinuationAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentReadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationAssuranceReadyStateResult continuationassuranceReadyState = await _runtimeBrowserContinuationAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentReadinessSessionResult result = new()
        {
            ProfileId = continuationassuranceReadyState.ProfileId,
            SessionId = continuationassuranceReadyState.SessionId,
            SessionPath = continuationassuranceReadyState.SessionPath,
            BrowserContinuationAssuranceReadyStateVersion = continuationassuranceReadyState.BrowserContinuationAssuranceReadyStateVersion,
            BrowserContinuationAssuranceSessionVersion = continuationassuranceReadyState.BrowserContinuationAssuranceSessionVersion,
            LaunchMode = continuationassuranceReadyState.LaunchMode,
            AssetRootPath = continuationassuranceReadyState.AssetRootPath,
            ProfilesRootPath = continuationassuranceReadyState.ProfilesRootPath,
            CacheRootPath = continuationassuranceReadyState.CacheRootPath,
            ConfigRootPath = continuationassuranceReadyState.ConfigRootPath,
            SettingsFilePath = continuationassuranceReadyState.SettingsFilePath,
            StartupProfilePath = continuationassuranceReadyState.StartupProfilePath,
            RequiredAssets = continuationassuranceReadyState.RequiredAssets,
            ReadyAssetCount = continuationassuranceReadyState.ReadyAssetCount,
            CompletedSteps = continuationassuranceReadyState.CompletedSteps,
            TotalSteps = continuationassuranceReadyState.TotalSteps,
            Exists = continuationassuranceReadyState.Exists,
            ReadSucceeded = continuationassuranceReadyState.ReadSucceeded
        };

        if (!continuationassuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainmentreadiness session blocked for profile '{continuationassuranceReadyState.ProfileId}'.";
            result.Error = continuationassuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentReadinessSessionVersion = "runtime-browser-sustainmentreadiness-session-v1";
        result.BrowserSustainmentReadinessStages =
        [
            "open-browser-sustainmentreadiness-session",
            "bind-browser-continuationassurance-ready-state",
            "publish-browser-sustainmentreadiness-ready"
        ];
        result.BrowserSustainmentReadinessSummary = $"Runtime browser sustainmentreadiness session prepared {result.BrowserSustainmentReadinessStages.Length} sustainmentreadiness stage(s) for profile '{continuationassuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentreadiness session ready for profile '{continuationassuranceReadyState.ProfileId}' with {result.BrowserSustainmentReadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentReadinessSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserSustainmentReadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentReadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
