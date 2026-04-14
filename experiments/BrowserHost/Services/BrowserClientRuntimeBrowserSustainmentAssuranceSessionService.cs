namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentAssuranceSessionService : IBrowserClientRuntimeBrowserSustainmentAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserContinuationReadinessReadyState _runtimeBrowserContinuationReadinessReadyState;

    public BrowserClientRuntimeBrowserSustainmentAssuranceSessionService(IBrowserClientRuntimeBrowserContinuationReadinessReadyState runtimeBrowserContinuationReadinessReadyState)
    {
        _runtimeBrowserContinuationReadinessReadyState = runtimeBrowserContinuationReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationReadinessReadyStateResult prevReadyState = await _runtimeBrowserContinuationReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentAssuranceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserContinuationReadinessReadyStateVersion = prevReadyState.BrowserContinuationReadinessReadyStateVersion,
            BrowserContinuationReadinessSessionVersion = prevReadyState.BrowserContinuationReadinessSessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainmentassurance session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentAssuranceSessionVersion = "runtime-browser-sustainmentassurance-session-v1";
        result.BrowserSustainmentAssuranceStages =
        [
            "open-browser-sustainmentassurance-session",
            "bind-browser-continuationreadiness-ready-state",
            "publish-browser-sustainmentassurance-ready"
        ];
        result.BrowserSustainmentAssuranceSummary = $"Runtime browser sustainmentassurance session prepared {result.BrowserSustainmentAssuranceStages.Length} sustainmentassurance stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentassurance session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSustainmentAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentAssuranceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserSustainmentAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
