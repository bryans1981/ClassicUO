namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowContinuityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFlowContinuityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowContinuityReadyStateService : IBrowserClientRuntimeBrowserFlowContinuityReadyState
{
    private readonly IBrowserClientRuntimeBrowserFlowContinuitySession _runtimeBrowserFlowContinuitySession;

    public BrowserClientRuntimeBrowserFlowContinuityReadyStateService(IBrowserClientRuntimeBrowserFlowContinuitySession runtimeBrowserFlowContinuitySession)
    {
        _runtimeBrowserFlowContinuitySession = runtimeBrowserFlowContinuitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowContinuityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowContinuitySessionResult flowcontinuitySession = await _runtimeBrowserFlowContinuitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFlowContinuityReadyStateResult result = new()
        {
            ProfileId = flowcontinuitySession.ProfileId,
            SessionId = flowcontinuitySession.SessionId,
            SessionPath = flowcontinuitySession.SessionPath,
            BrowserFlowContinuitySessionVersion = flowcontinuitySession.BrowserFlowContinuitySessionVersion,
            BrowserInteractionContinuityReadyStateVersion = flowcontinuitySession.BrowserInteractionContinuityReadyStateVersion,
            BrowserInteractionContinuitySessionVersion = flowcontinuitySession.BrowserInteractionContinuitySessionVersion,
            LaunchMode = flowcontinuitySession.LaunchMode,
            AssetRootPath = flowcontinuitySession.AssetRootPath,
            ProfilesRootPath = flowcontinuitySession.ProfilesRootPath,
            CacheRootPath = flowcontinuitySession.CacheRootPath,
            ConfigRootPath = flowcontinuitySession.ConfigRootPath,
            SettingsFilePath = flowcontinuitySession.SettingsFilePath,
            StartupProfilePath = flowcontinuitySession.StartupProfilePath,
            RequiredAssets = flowcontinuitySession.RequiredAssets,
            ReadyAssetCount = flowcontinuitySession.ReadyAssetCount,
            CompletedSteps = flowcontinuitySession.CompletedSteps,
            TotalSteps = flowcontinuitySession.TotalSteps,
            Exists = flowcontinuitySession.Exists,
            ReadSucceeded = flowcontinuitySession.ReadSucceeded
        };

        if (!flowcontinuitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser flowcontinuity ready state blocked for profile '{flowcontinuitySession.ProfileId}'.";
            result.Error = flowcontinuitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowContinuityReadyStateVersion = "runtime-browser-flowcontinuity-ready-state-v1";
        result.BrowserFlowContinuityReadyChecks =
        [
            "browser-interactioncontinuity-ready-state-ready",
            "browser-flowcontinuity-session-ready",
            "browser-flowcontinuity-ready"
        ];
        result.BrowserFlowContinuityReadySummary = $"Runtime browser flowcontinuity ready state passed {result.BrowserFlowContinuityReadyChecks.Length} flowcontinuity readiness check(s) for profile '{flowcontinuitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowcontinuity ready state ready for profile '{flowcontinuitySession.ProfileId}' with {result.BrowserFlowContinuityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowContinuityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFlowContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlowContinuityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFlowContinuityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
