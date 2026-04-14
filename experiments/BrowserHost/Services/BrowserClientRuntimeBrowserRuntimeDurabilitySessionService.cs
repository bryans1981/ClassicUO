namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeDurabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeDurabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeDurabilitySessionService : IBrowserClientRuntimeBrowserRuntimeDurabilitySession
{
    private readonly IBrowserClientRuntimeBrowserLiveResilienceReadyState _runtimeBrowserLiveResilienceReadyState;

    public BrowserClientRuntimeBrowserRuntimeDurabilitySessionService(IBrowserClientRuntimeBrowserLiveResilienceReadyState runtimeBrowserLiveResilienceReadyState)
    {
        _runtimeBrowserLiveResilienceReadyState = runtimeBrowserLiveResilienceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeDurabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLiveResilienceReadyStateResult prevReadyState = await _runtimeBrowserLiveResilienceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeDurabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserLiveResilienceReadyStateVersion = prevReadyState.BrowserLiveResilienceReadyStateVersion,
            BrowserLiveResilienceSessionVersion = prevReadyState.BrowserLiveResilienceSessionVersion,
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
            result.Summary = $"Runtime browser runtimedurability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeDurabilitySessionVersion = "runtime-browser-runtimedurability-session-v1";
        result.BrowserRuntimeDurabilityStages =
        [
            "open-browser-runtimedurability-session",
            "bind-browser-liveresilience-ready-state",
            "publish-browser-runtimedurability-ready"
        ];
        result.BrowserRuntimeDurabilitySummary = $"Runtime browser runtimedurability session prepared {result.BrowserRuntimeDurabilityStages.Length} runtimedurability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimedurability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserRuntimeDurabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeDurabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeDurabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeDurabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
