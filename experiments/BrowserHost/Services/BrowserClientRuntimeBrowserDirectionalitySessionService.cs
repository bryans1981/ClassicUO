namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDirectionalitySession
{
    ValueTask<BrowserClientRuntimeBrowserDirectionalitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDirectionalitySessionService : IBrowserClientRuntimeBrowserDirectionalitySession
{
    private readonly IBrowserClientRuntimeBrowserObservabilityReadyState _runtimeBrowserObservabilityReadyState;

    public BrowserClientRuntimeBrowserDirectionalitySessionService(IBrowserClientRuntimeBrowserObservabilityReadyState runtimeBrowserObservabilityReadyState)
    {
        _runtimeBrowserObservabilityReadyState = runtimeBrowserObservabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDirectionalitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserObservabilityReadyStateResult observabilityReadyState = await _runtimeBrowserObservabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDirectionalitySessionResult result = new()
        {
            ProfileId = observabilityReadyState.ProfileId,
            SessionId = observabilityReadyState.SessionId,
            SessionPath = observabilityReadyState.SessionPath,
            BrowserObservabilityReadyStateVersion = observabilityReadyState.BrowserObservabilityReadyStateVersion,
            BrowserObservabilitySessionVersion = observabilityReadyState.BrowserObservabilitySessionVersion,
            LaunchMode = observabilityReadyState.LaunchMode,
            AssetRootPath = observabilityReadyState.AssetRootPath,
            ProfilesRootPath = observabilityReadyState.ProfilesRootPath,
            CacheRootPath = observabilityReadyState.CacheRootPath,
            ConfigRootPath = observabilityReadyState.ConfigRootPath,
            SettingsFilePath = observabilityReadyState.SettingsFilePath,
            StartupProfilePath = observabilityReadyState.StartupProfilePath,
            RequiredAssets = observabilityReadyState.RequiredAssets,
            ReadyAssetCount = observabilityReadyState.ReadyAssetCount,
            CompletedSteps = observabilityReadyState.CompletedSteps,
            TotalSteps = observabilityReadyState.TotalSteps,
            Exists = observabilityReadyState.Exists,
            ReadSucceeded = observabilityReadyState.ReadSucceeded
        };

        if (!observabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser directionality session blocked for profile '{observabilityReadyState.ProfileId}'.";
            result.Error = observabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDirectionalitySessionVersion = "runtime-browser-directionality-session-v1";
        result.BrowserDirectionalityStages =
        [
            "open-browser-directionality-session",
            "bind-browser-observability-ready-state",
            "publish-browser-directionality-ready"
        ];
        result.BrowserDirectionalitySummary = $"Runtime browser directionality session prepared {result.BrowserDirectionalityStages.Length} directionality stage(s) for profile '{observabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser directionality session ready for profile '{observabilityReadyState.ProfileId}' with {result.BrowserDirectionalityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDirectionalitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDirectionalitySessionVersion { get; set; } = string.Empty;
    public string BrowserObservabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserObservabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDirectionalityStages { get; set; } = Array.Empty<string>();
    public string BrowserDirectionalitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
