namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeSustainabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeSustainabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeSustainabilitySessionService : IBrowserClientRuntimeBrowserRuntimeSustainabilitySession
{
    private readonly IBrowserClientRuntimeBrowserLiveSustainabilityReadyState _runtimeBrowserLiveSustainabilityReadyState;

    public BrowserClientRuntimeBrowserRuntimeSustainabilitySessionService(IBrowserClientRuntimeBrowserLiveSustainabilityReadyState runtimeBrowserLiveSustainabilityReadyState)
    {
        _runtimeBrowserLiveSustainabilityReadyState = runtimeBrowserLiveSustainabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeSustainabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveSustainabilityReadyStateResult prevReadyState = await _runtimeBrowserLiveSustainabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeSustainabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLiveSustainabilityReadyStateVersion = prevReadyState.BrowserLiveSustainabilityReadyStateVersion,
            BrowserLiveSustainabilitySessionVersion = prevReadyState.BrowserLiveSustainabilitySessionVersion,
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
            result.Summary = $"Runtime browser runtimesustainability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeSustainabilitySessionVersion = "runtime-browser-runtimesustainability-session-v1";
        result.BrowserRuntimeSustainabilityStages =
        [
            "open-browser-runtimesustainability-session",
            "bind-browser-livesustainability-ready-state",
            "publish-browser-runtimesustainability-ready"
        ];
        result.BrowserRuntimeSustainabilitySummary = $"Runtime browser runtimesustainability session prepared {result.BrowserRuntimeSustainabilityStages.Length} runtimesustainability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimesustainability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserRuntimeSustainabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeSustainabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveSustainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeSustainabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeSustainabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
