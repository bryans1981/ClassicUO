namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceSustainabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserServiceSustainabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceSustainabilitySessionService : IBrowserClientRuntimeBrowserServiceSustainabilitySession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeSustainabilityReadyState _runtimeBrowserRuntimeSustainabilityReadyState;

    public BrowserClientRuntimeBrowserServiceSustainabilitySessionService(IBrowserClientRuntimeBrowserRuntimeSustainabilityReadyState runtimeBrowserRuntimeSustainabilityReadyState)
    {
        _runtimeBrowserRuntimeSustainabilityReadyState = runtimeBrowserRuntimeSustainabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceSustainabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeSustainabilityReadyStateResult prevReadyState = await _runtimeBrowserRuntimeSustainabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceSustainabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserRuntimeSustainabilityReadyStateVersion = prevReadyState.BrowserRuntimeSustainabilityReadyStateVersion,
            BrowserRuntimeSustainabilitySessionVersion = prevReadyState.BrowserRuntimeSustainabilitySessionVersion,
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
            result.Summary = $"Runtime browser servicesustainability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceSustainabilitySessionVersion = "runtime-browser-servicesustainability-session-v1";
        result.BrowserServiceSustainabilityStages =
        [
            "open-browser-servicesustainability-session",
            "bind-browser-runtimesustainability-ready-state",
            "publish-browser-servicesustainability-ready"
        ];
        result.BrowserServiceSustainabilitySummary = $"Runtime browser servicesustainability session prepared {result.BrowserServiceSustainabilityStages.Length} servicesustainability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser servicesustainability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserServiceSustainabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceSustainabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceSustainabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserServiceSustainabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
