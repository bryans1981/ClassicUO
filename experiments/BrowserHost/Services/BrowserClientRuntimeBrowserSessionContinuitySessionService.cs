namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionContinuitySession
{
    ValueTask<BrowserClientRuntimeBrowserSessionContinuitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionContinuitySessionService : IBrowserClientRuntimeBrowserSessionContinuitySession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeContinuityReadyState _runtimeBrowserRuntimeContinuityReadyState;

    public BrowserClientRuntimeBrowserSessionContinuitySessionService(IBrowserClientRuntimeBrowserRuntimeContinuityReadyState runtimeBrowserRuntimeContinuityReadyState)
    {
        _runtimeBrowserRuntimeContinuityReadyState = runtimeBrowserRuntimeContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionContinuitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeContinuityReadyStateResult prevReadyState = await _runtimeBrowserRuntimeContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSessionContinuitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserRuntimeContinuityReadyStateVersion = prevReadyState.BrowserRuntimeContinuityReadyStateVersion,
            BrowserRuntimeContinuitySessionVersion = prevReadyState.BrowserRuntimeContinuitySessionVersion,
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
            result.Summary = $"Runtime browser sessioncontinuity session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionContinuitySessionVersion = "runtime-browser-sessioncontinuity-session-v1";
        result.BrowserSessionContinuityStages =
        [
            "open-browser-sessioncontinuity-session",
            "bind-browser-runtimecontinuity-ready-state",
            "publish-browser-sessioncontinuity-ready"
        ];
        result.BrowserSessionContinuitySummary = $"Runtime browser sessioncontinuity session prepared {result.BrowserSessionContinuityStages.Length} sessioncontinuity stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessioncontinuity session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSessionContinuityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionContinuitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionContinuityStages { get; set; } = Array.Empty<string>();
    public string BrowserSessionContinuitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
