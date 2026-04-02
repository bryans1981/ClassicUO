namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSupportabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserSupportabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSupportabilitySessionService : IBrowserClientRuntimeBrowserSupportabilitySession
{
    private readonly IBrowserClientRuntimeBrowserMaintainabilityReadyState _runtimeBrowserMaintainabilityReadyState;

    public BrowserClientRuntimeBrowserSupportabilitySessionService(IBrowserClientRuntimeBrowserMaintainabilityReadyState runtimeBrowserMaintainabilityReadyState)
    {
        _runtimeBrowserMaintainabilityReadyState = runtimeBrowserMaintainabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSupportabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserMaintainabilityReadyStateResult maintainabilityReadyState = await _runtimeBrowserMaintainabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSupportabilitySessionResult result = new()
        {
            ProfileId = maintainabilityReadyState.ProfileId,
            SessionId = maintainabilityReadyState.SessionId,
            SessionPath = maintainabilityReadyState.SessionPath,
            BrowserMaintainabilityReadyStateVersion = maintainabilityReadyState.BrowserMaintainabilityReadyStateVersion,
            BrowserMaintainabilitySessionVersion = maintainabilityReadyState.BrowserMaintainabilitySessionVersion,
            LaunchMode = maintainabilityReadyState.LaunchMode,
            AssetRootPath = maintainabilityReadyState.AssetRootPath,
            ProfilesRootPath = maintainabilityReadyState.ProfilesRootPath,
            CacheRootPath = maintainabilityReadyState.CacheRootPath,
            ConfigRootPath = maintainabilityReadyState.ConfigRootPath,
            SettingsFilePath = maintainabilityReadyState.SettingsFilePath,
            StartupProfilePath = maintainabilityReadyState.StartupProfilePath,
            RequiredAssets = maintainabilityReadyState.RequiredAssets,
            ReadyAssetCount = maintainabilityReadyState.ReadyAssetCount,
            CompletedSteps = maintainabilityReadyState.CompletedSteps,
            TotalSteps = maintainabilityReadyState.TotalSteps,
            Exists = maintainabilityReadyState.Exists,
            ReadSucceeded = maintainabilityReadyState.ReadSucceeded
        };

        if (!maintainabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser supportability session blocked for profile '{maintainabilityReadyState.ProfileId}'.";
            result.Error = maintainabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSupportabilitySessionVersion = "runtime-browser-supportability-session-v1";
        result.BrowserSupportabilityStages =
        [
            "open-browser-supportability-session",
            "bind-browser-maintainability-ready-state",
            "publish-browser-supportability-ready"
        ];
        result.BrowserSupportabilitySummary = $"Runtime browser supportability session prepared {result.BrowserSupportabilityStages.Length} supportability stage(s) for profile '{maintainabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser supportability session ready for profile '{maintainabilityReadyState.ProfileId}' with {result.BrowserSupportabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSupportabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSupportabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserMaintainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMaintainabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSupportabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserSupportabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
