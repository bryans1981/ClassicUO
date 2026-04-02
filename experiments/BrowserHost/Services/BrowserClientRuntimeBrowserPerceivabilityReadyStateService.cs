namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPerceivabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPerceivabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPerceivabilityReadyStateService : IBrowserClientRuntimeBrowserPerceivabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserPerceivabilitySession _runtimeBrowserPerceivabilitySession;

    public BrowserClientRuntimeBrowserPerceivabilityReadyStateService(IBrowserClientRuntimeBrowserPerceivabilitySession runtimeBrowserPerceivabilitySession)
    {
        _runtimeBrowserPerceivabilitySession = runtimeBrowserPerceivabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPerceivabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPerceivabilitySessionResult perceivabilitySession = await _runtimeBrowserPerceivabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPerceivabilityReadyStateResult result = new()
        {
            ProfileId = perceivabilitySession.ProfileId,
            SessionId = perceivabilitySession.SessionId,
            SessionPath = perceivabilitySession.SessionPath,
            BrowserPerceivabilitySessionVersion = perceivabilitySession.BrowserPerceivabilitySessionVersion,
            BrowserConsistencyOfFeedbackReadyStateVersion = perceivabilitySession.BrowserConsistencyOfFeedbackReadyStateVersion,
            BrowserConsistencyOfFeedbackSessionVersion = perceivabilitySession.BrowserConsistencyOfFeedbackSessionVersion,
            LaunchMode = perceivabilitySession.LaunchMode,
            AssetRootPath = perceivabilitySession.AssetRootPath,
            ProfilesRootPath = perceivabilitySession.ProfilesRootPath,
            CacheRootPath = perceivabilitySession.CacheRootPath,
            ConfigRootPath = perceivabilitySession.ConfigRootPath,
            SettingsFilePath = perceivabilitySession.SettingsFilePath,
            StartupProfilePath = perceivabilitySession.StartupProfilePath,
            RequiredAssets = perceivabilitySession.RequiredAssets,
            ReadyAssetCount = perceivabilitySession.ReadyAssetCount,
            CompletedSteps = perceivabilitySession.CompletedSteps,
            TotalSteps = perceivabilitySession.TotalSteps,
            Exists = perceivabilitySession.Exists,
            ReadSucceeded = perceivabilitySession.ReadSucceeded
        };

        if (!perceivabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser perceivability ready state blocked for profile '{perceivabilitySession.ProfileId}'.";
            result.Error = perceivabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPerceivabilityReadyStateVersion = "runtime-browser-perceivability-ready-state-v1";
        result.BrowserPerceivabilityReadyChecks =
        [
            "browser-consistencyoffeedback-ready-state-ready",
            "browser-perceivability-session-ready",
            "browser-perceivability-ready"
        ];
        result.BrowserPerceivabilityReadySummary = $"Runtime browser perceivability ready state passed {result.BrowserPerceivabilityReadyChecks.Length} perceivability readiness check(s) for profile '{perceivabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser perceivability ready state ready for profile '{perceivabilitySession.ProfileId}' with {result.BrowserPerceivabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPerceivabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPerceivabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPerceivabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserConsistencyOfFeedbackReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConsistencyOfFeedbackSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPerceivabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPerceivabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
