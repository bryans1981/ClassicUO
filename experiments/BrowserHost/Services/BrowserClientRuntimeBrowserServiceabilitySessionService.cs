namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserServiceabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceabilitySessionService : IBrowserClientRuntimeBrowserServiceabilitySession
{
    private readonly IBrowserClientRuntimeBrowserOperabilityReadyState _runtimeBrowserOperabilityReadyState;

    public BrowserClientRuntimeBrowserServiceabilitySessionService(IBrowserClientRuntimeBrowserOperabilityReadyState runtimeBrowserOperabilityReadyState)
    {
        _runtimeBrowserOperabilityReadyState = runtimeBrowserOperabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperabilityReadyStateResult operabilityReadyState = await _runtimeBrowserOperabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceabilitySessionResult result = new()
        {
            ProfileId = operabilityReadyState.ProfileId,
            SessionId = operabilityReadyState.SessionId,
            SessionPath = operabilityReadyState.SessionPath,
            BrowserOperabilityReadyStateVersion = operabilityReadyState.BrowserOperabilityReadyStateVersion,
            BrowserOperabilitySessionVersion = operabilityReadyState.BrowserOperabilitySessionVersion,
            LaunchMode = operabilityReadyState.LaunchMode,
            AssetRootPath = operabilityReadyState.AssetRootPath,
            ProfilesRootPath = operabilityReadyState.ProfilesRootPath,
            CacheRootPath = operabilityReadyState.CacheRootPath,
            ConfigRootPath = operabilityReadyState.ConfigRootPath,
            SettingsFilePath = operabilityReadyState.SettingsFilePath,
            StartupProfilePath = operabilityReadyState.StartupProfilePath,
            RequiredAssets = operabilityReadyState.RequiredAssets,
            ReadyAssetCount = operabilityReadyState.ReadyAssetCount,
            CompletedSteps = operabilityReadyState.CompletedSteps,
            TotalSteps = operabilityReadyState.TotalSteps,
            Exists = operabilityReadyState.Exists,
            ReadSucceeded = operabilityReadyState.ReadSucceeded
        };

        if (!operabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceability session blocked for profile '{operabilityReadyState.ProfileId}'.";
            result.Error = operabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceabilitySessionVersion = "runtime-browser-serviceability-session-v1";
        result.BrowserServiceabilityStages =
        [
            "open-browser-serviceability-session",
            "bind-browser-operability-ready-state",
            "publish-browser-serviceability-ready"
        ];
        result.BrowserServiceabilitySummary = $"Runtime browser serviceability session prepared {result.BrowserServiceabilityStages.Length} serviceability stage(s) for profile '{operabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceability session ready for profile '{operabilityReadyState.ProfileId}' with {result.BrowserServiceabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserServiceabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
