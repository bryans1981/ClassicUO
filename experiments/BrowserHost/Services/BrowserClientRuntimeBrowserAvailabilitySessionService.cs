namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAvailabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserAvailabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAvailabilitySessionService : IBrowserClientRuntimeBrowserAvailabilitySession
{
    private readonly IBrowserClientRuntimeBrowserResilienceReadyState _runtimeBrowserResilienceReadyState;

    public BrowserClientRuntimeBrowserAvailabilitySessionService(IBrowserClientRuntimeBrowserResilienceReadyState runtimeBrowserResilienceReadyState)
    {
        _runtimeBrowserResilienceReadyState = runtimeBrowserResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAvailabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResilienceReadyStateResult resilienceReadyState = await _runtimeBrowserResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAvailabilitySessionResult result = new()
        {
            ProfileId = resilienceReadyState.ProfileId,
            SessionId = resilienceReadyState.SessionId,
            SessionPath = resilienceReadyState.SessionPath,
            BrowserResilienceReadyStateVersion = resilienceReadyState.BrowserResilienceReadyStateVersion,
            BrowserResilienceSessionVersion = resilienceReadyState.BrowserResilienceSessionVersion,
            LaunchMode = resilienceReadyState.LaunchMode,
            AssetRootPath = resilienceReadyState.AssetRootPath,
            ProfilesRootPath = resilienceReadyState.ProfilesRootPath,
            CacheRootPath = resilienceReadyState.CacheRootPath,
            ConfigRootPath = resilienceReadyState.ConfigRootPath,
            SettingsFilePath = resilienceReadyState.SettingsFilePath,
            StartupProfilePath = resilienceReadyState.StartupProfilePath,
            RequiredAssets = resilienceReadyState.RequiredAssets,
            ReadyAssetCount = resilienceReadyState.ReadyAssetCount,
            CompletedSteps = resilienceReadyState.CompletedSteps,
            TotalSteps = resilienceReadyState.TotalSteps,
            Exists = resilienceReadyState.Exists,
            ReadSucceeded = resilienceReadyState.ReadSucceeded,
            BrowserResilienceReadyState = resilienceReadyState
        };

        if (!resilienceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser availability session blocked for profile '{resilienceReadyState.ProfileId}'.";
            result.Error = resilienceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAvailabilitySessionVersion = "runtime-browser-availability-session-v1";
        result.BrowserAvailabilityStages =
        [
            "open-browser-availability-session",
            "bind-browser-resilience-ready-state",
            "publish-browser-availability-ready"
        ];
        result.BrowserAvailabilitySummary = $"Runtime browser availability session prepared {result.BrowserAvailabilityStages.Length} availability stage(s) for profile '{resilienceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser availability session ready for profile '{resilienceReadyState.ProfileId}' with {result.BrowserAvailabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAvailabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAvailabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAvailabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserAvailabilitySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserResilienceReadyStateResult BrowserResilienceReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
