namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateContinuitySession
{
    ValueTask<BrowserClientRuntimeBrowserStateContinuitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateContinuitySessionService : IBrowserClientRuntimeBrowserStateContinuitySession
{
    private readonly IBrowserClientRuntimeBrowserSessionContinuityReadyState _runtimeBrowserSessionContinuityReadyState;

    public BrowserClientRuntimeBrowserStateContinuitySessionService(IBrowserClientRuntimeBrowserSessionContinuityReadyState runtimeBrowserSessionContinuityReadyState)
    {
        _runtimeBrowserSessionContinuityReadyState = runtimeBrowserSessionContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateContinuitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionContinuityReadyStateResult prevReadyState = await _runtimeBrowserSessionContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStateContinuitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSessionContinuityReadyStateVersion = prevReadyState.BrowserSessionContinuityReadyStateVersion,
            BrowserSessionContinuitySessionVersion = prevReadyState.BrowserSessionContinuitySessionVersion,
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
            result.Summary = $"Runtime browser statecontinuity session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateContinuitySessionVersion = "runtime-browser-statecontinuity-session-v1";
        result.BrowserStateContinuityStages =
        [
            "open-browser-statecontinuity-session",
            "bind-browser-sessioncontinuity-ready-state",
            "publish-browser-statecontinuity-ready"
        ];
        result.BrowserStateContinuitySummary = $"Runtime browser statecontinuity session prepared {result.BrowserStateContinuityStages.Length} statecontinuity stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser statecontinuity session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserStateContinuityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateContinuitySessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserStateContinuityStages { get; set; } = Array.Empty<string>();
    public string BrowserStateContinuitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
