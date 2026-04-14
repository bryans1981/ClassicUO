namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserStateStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateStabilitySessionService : IBrowserClientRuntimeBrowserStateStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSessionStabilityReadyState _runtimeBrowserSessionStabilityReadyState;

    public BrowserClientRuntimeBrowserStateStabilitySessionService(IBrowserClientRuntimeBrowserSessionStabilityReadyState runtimeBrowserSessionStabilityReadyState)
    {
        _runtimeBrowserSessionStabilityReadyState = runtimeBrowserSessionStabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionStabilityReadyStateResult sessionstabilityReadyState = await _runtimeBrowserSessionStabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStateStabilitySessionResult result = new()
        {
            ProfileId = sessionstabilityReadyState.ProfileId,
            SessionId = sessionstabilityReadyState.SessionId,
            SessionPath = sessionstabilityReadyState.SessionPath,
            BrowserSessionStabilityReadyStateVersion = sessionstabilityReadyState.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySessionVersion = sessionstabilityReadyState.BrowserSessionStabilitySessionVersion,
            LaunchMode = sessionstabilityReadyState.LaunchMode,
            AssetRootPath = sessionstabilityReadyState.AssetRootPath,
            ProfilesRootPath = sessionstabilityReadyState.ProfilesRootPath,
            CacheRootPath = sessionstabilityReadyState.CacheRootPath,
            ConfigRootPath = sessionstabilityReadyState.ConfigRootPath,
            SettingsFilePath = sessionstabilityReadyState.SettingsFilePath,
            StartupProfilePath = sessionstabilityReadyState.StartupProfilePath,
            RequiredAssets = sessionstabilityReadyState.RequiredAssets,
            ReadyAssetCount = sessionstabilityReadyState.ReadyAssetCount,
            CompletedSteps = sessionstabilityReadyState.CompletedSteps,
            TotalSteps = sessionstabilityReadyState.TotalSteps,
            Exists = sessionstabilityReadyState.Exists,
            ReadSucceeded = sessionstabilityReadyState.ReadSucceeded
        };

        if (!sessionstabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser statestability session blocked for profile '{sessionstabilityReadyState.ProfileId}'.";
            result.Error = sessionstabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateStabilitySessionVersion = "runtime-browser-statestability-session-v1";
        result.BrowserStateStabilityStages =
        [
            "open-browser-statestability-session",
            "bind-browser-sessionstability-ready-state",
            "publish-browser-statestability-ready"
        ];
        result.BrowserStateStabilitySummary = $"Runtime browser statestability session prepared {result.BrowserStateStabilityStages.Length} statestability stage(s) for profile '{sessionstabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser statestability session ready for profile '{sessionstabilityReadyState.ProfileId}' with {result.BrowserStateStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserStateStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserStateStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
