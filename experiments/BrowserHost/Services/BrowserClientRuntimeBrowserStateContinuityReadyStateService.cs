namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateContinuityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserStateContinuityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateContinuityReadyStateService : IBrowserClientRuntimeBrowserStateContinuityReadyState
{
    private readonly IBrowserClientRuntimeBrowserStateContinuitySession _runtimeBrowserStateContinuitySession;

    public BrowserClientRuntimeBrowserStateContinuityReadyStateService(IBrowserClientRuntimeBrowserStateContinuitySession runtimeBrowserStateContinuitySession)
    {
        _runtimeBrowserStateContinuitySession = runtimeBrowserStateContinuitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateContinuityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateContinuitySessionResult statecontinuitySession = await _runtimeBrowserStateContinuitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserStateContinuityReadyStateResult result = new()
        {
            ProfileId = statecontinuitySession.ProfileId,
            SessionId = statecontinuitySession.SessionId,
            SessionPath = statecontinuitySession.SessionPath,
            BrowserStateContinuitySessionVersion = statecontinuitySession.BrowserStateContinuitySessionVersion,
            BrowserSessionContinuityReadyStateVersion = statecontinuitySession.BrowserSessionContinuityReadyStateVersion,
            BrowserSessionContinuitySessionVersion = statecontinuitySession.BrowserSessionContinuitySessionVersion,
            LaunchMode = statecontinuitySession.LaunchMode,
            AssetRootPath = statecontinuitySession.AssetRootPath,
            ProfilesRootPath = statecontinuitySession.ProfilesRootPath,
            CacheRootPath = statecontinuitySession.CacheRootPath,
            ConfigRootPath = statecontinuitySession.ConfigRootPath,
            SettingsFilePath = statecontinuitySession.SettingsFilePath,
            StartupProfilePath = statecontinuitySession.StartupProfilePath,
            RequiredAssets = statecontinuitySession.RequiredAssets,
            ReadyAssetCount = statecontinuitySession.ReadyAssetCount,
            CompletedSteps = statecontinuitySession.CompletedSteps,
            TotalSteps = statecontinuitySession.TotalSteps,
            Exists = statecontinuitySession.Exists,
            ReadSucceeded = statecontinuitySession.ReadSucceeded
        };

        if (!statecontinuitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser statecontinuity ready state blocked for profile '{statecontinuitySession.ProfileId}'.";
            result.Error = statecontinuitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateContinuityReadyStateVersion = "runtime-browser-statecontinuity-ready-state-v1";
        result.BrowserStateContinuityReadyChecks =
        [
            "browser-sessioncontinuity-ready-state-ready",
            "browser-statecontinuity-session-ready",
            "browser-statecontinuity-ready"
        ];
        result.BrowserStateContinuityReadySummary = $"Runtime browser statecontinuity ready state passed {result.BrowserStateContinuityReadyChecks.Length} statecontinuity readiness check(s) for profile '{statecontinuitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser statecontinuity ready state ready for profile '{statecontinuitySession.ProfileId}' with {result.BrowserStateContinuityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateContinuityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserStateContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserSessionContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateContinuityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserStateContinuityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
