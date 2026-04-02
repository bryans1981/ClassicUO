namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMaintainabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserMaintainabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMaintainabilitySessionService : IBrowserClientRuntimeBrowserMaintainabilitySession
{
    private readonly IBrowserClientRuntimeBrowserServiceabilityReadyState _runtimeBrowserServiceabilityReadyState;

    public BrowserClientRuntimeBrowserMaintainabilitySessionService(IBrowserClientRuntimeBrowserServiceabilityReadyState runtimeBrowserServiceabilityReadyState)
    {
        _runtimeBrowserServiceabilityReadyState = runtimeBrowserServiceabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMaintainabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceabilityReadyStateResult serviceabilityReadyState = await _runtimeBrowserServiceabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserMaintainabilitySessionResult result = new()
        {
            ProfileId = serviceabilityReadyState.ProfileId,
            SessionId = serviceabilityReadyState.SessionId,
            SessionPath = serviceabilityReadyState.SessionPath,
            BrowserServiceabilityReadyStateVersion = serviceabilityReadyState.BrowserServiceabilityReadyStateVersion,
            BrowserServiceabilitySessionVersion = serviceabilityReadyState.BrowserServiceabilitySessionVersion,
            LaunchMode = serviceabilityReadyState.LaunchMode,
            AssetRootPath = serviceabilityReadyState.AssetRootPath,
            ProfilesRootPath = serviceabilityReadyState.ProfilesRootPath,
            CacheRootPath = serviceabilityReadyState.CacheRootPath,
            ConfigRootPath = serviceabilityReadyState.ConfigRootPath,
            SettingsFilePath = serviceabilityReadyState.SettingsFilePath,
            StartupProfilePath = serviceabilityReadyState.StartupProfilePath,
            RequiredAssets = serviceabilityReadyState.RequiredAssets,
            ReadyAssetCount = serviceabilityReadyState.ReadyAssetCount,
            CompletedSteps = serviceabilityReadyState.CompletedSteps,
            TotalSteps = serviceabilityReadyState.TotalSteps,
            Exists = serviceabilityReadyState.Exists,
            ReadSucceeded = serviceabilityReadyState.ReadSucceeded
        };

        if (!serviceabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser maintainability session blocked for profile '{serviceabilityReadyState.ProfileId}'.";
            result.Error = serviceabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMaintainabilitySessionVersion = "runtime-browser-maintainability-session-v1";
        result.BrowserMaintainabilityStages =
        [
            "open-browser-maintainability-session",
            "bind-browser-serviceability-ready-state",
            "publish-browser-maintainability-ready"
        ];
        result.BrowserMaintainabilitySummary = $"Runtime browser maintainability session prepared {result.BrowserMaintainabilityStages.Length} maintainability stage(s) for profile '{serviceabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser maintainability session ready for profile '{serviceabilityReadyState.ProfileId}' with {result.BrowserMaintainabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMaintainabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserMaintainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMaintainabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserMaintainabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
