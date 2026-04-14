namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceDurabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserServiceDurabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceDurabilitySessionService : IBrowserClientRuntimeBrowserServiceDurabilitySession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeDurabilityReadyState _runtimeBrowserRuntimeDurabilityReadyState;

    public BrowserClientRuntimeBrowserServiceDurabilitySessionService(IBrowserClientRuntimeBrowserRuntimeDurabilityReadyState runtimeBrowserRuntimeDurabilityReadyState)
    {
        _runtimeBrowserRuntimeDurabilityReadyState = runtimeBrowserRuntimeDurabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceDurabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateResult prevReadyState = await _runtimeBrowserRuntimeDurabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceDurabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserRuntimeDurabilityReadyStateVersion = prevReadyState.BrowserRuntimeDurabilityReadyStateVersion,
            BrowserRuntimeDurabilitySessionVersion = prevReadyState.BrowserRuntimeDurabilitySessionVersion,
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
            result.Summary = $"Runtime browser servicedurability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceDurabilitySessionVersion = "runtime-browser-servicedurability-session-v1";
        result.BrowserServiceDurabilityStages =
        [
            "open-browser-servicedurability-session",
            "bind-browser-runtimedurability-ready-state",
            "publish-browser-servicedurability-ready"
        ];
        result.BrowserServiceDurabilitySummary = $"Runtime browser servicedurability session prepared {result.BrowserServiceDurabilityStages.Length} servicedurability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser servicedurability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserServiceDurabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceDurabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceDurabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserServiceDurabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
