namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserProductionStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserProductionStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserProductionStabilitySessionService : IBrowserClientRuntimeBrowserProductionStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserServiceSustainabilityReadyState _runtimeBrowserServiceSustainabilityReadyState;

    public BrowserClientRuntimeBrowserProductionStabilitySessionService(IBrowserClientRuntimeBrowserServiceSustainabilityReadyState runtimeBrowserServiceSustainabilityReadyState)
    {
        _runtimeBrowserServiceSustainabilityReadyState = runtimeBrowserServiceSustainabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserProductionStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceSustainabilityReadyStateResult prevReadyState = await _runtimeBrowserServiceSustainabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserProductionStabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserServiceSustainabilityReadyStateVersion = prevReadyState.BrowserServiceSustainabilityReadyStateVersion,
            BrowserServiceSustainabilitySessionVersion = prevReadyState.BrowserServiceSustainabilitySessionVersion,
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
            result.Summary = $"Runtime browser productionstability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserProductionStabilitySessionVersion = "runtime-browser-productionstability-session-v1";
        result.BrowserProductionStabilityStages =
        [
            "open-browser-productionstability-session",
            "bind-browser-servicesustainability-ready-state",
            "publish-browser-productionstability-ready"
        ];
        result.BrowserProductionStabilitySummary = $"Runtime browser productionstability session prepared {result.BrowserProductionStabilityStages.Length} productionstability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser productionstability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserProductionStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserProductionStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserProductionStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserProductionStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserProductionStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
