namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCueingSession
{
    ValueTask<BrowserClientRuntimeBrowserCueingSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCueingSessionService : IBrowserClientRuntimeBrowserCueingSession
{
    private readonly IBrowserClientRuntimeBrowserAbsorptionReadyState _runtimeBrowserAbsorptionReadyState;

    public BrowserClientRuntimeBrowserCueingSessionService(IBrowserClientRuntimeBrowserAbsorptionReadyState runtimeBrowserAbsorptionReadyState)
    {
        _runtimeBrowserAbsorptionReadyState = runtimeBrowserAbsorptionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCueingSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAbsorptionReadyStateResult absorptionReadyState = await _runtimeBrowserAbsorptionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCueingSessionResult result = new()
        {
            ProfileId = absorptionReadyState.ProfileId,
            SessionId = absorptionReadyState.SessionId,
            SessionPath = absorptionReadyState.SessionPath,
            BrowserAbsorptionReadyStateVersion = absorptionReadyState.BrowserAbsorptionReadyStateVersion,
            BrowserAbsorptionSessionVersion = absorptionReadyState.BrowserAbsorptionSessionVersion,
            LaunchMode = absorptionReadyState.LaunchMode,
            AssetRootPath = absorptionReadyState.AssetRootPath,
            ProfilesRootPath = absorptionReadyState.ProfilesRootPath,
            CacheRootPath = absorptionReadyState.CacheRootPath,
            ConfigRootPath = absorptionReadyState.ConfigRootPath,
            SettingsFilePath = absorptionReadyState.SettingsFilePath,
            StartupProfilePath = absorptionReadyState.StartupProfilePath,
            RequiredAssets = absorptionReadyState.RequiredAssets,
            ReadyAssetCount = absorptionReadyState.ReadyAssetCount,
            CompletedSteps = absorptionReadyState.CompletedSteps,
            TotalSteps = absorptionReadyState.TotalSteps,
            Exists = absorptionReadyState.Exists,
            ReadSucceeded = absorptionReadyState.ReadSucceeded
        };

        if (!absorptionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser cueing session blocked for profile '{absorptionReadyState.ProfileId}'.";
            result.Error = absorptionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCueingSessionVersion = "runtime-browser-cueing-session-v1";
        result.BrowserCueingStages =
        [
            "open-browser-cueing-session",
            "bind-browser-absorption-ready-state",
            "publish-browser-cueing-ready"
        ];
        result.BrowserCueingSummary = $"Runtime browser cueing session prepared {result.BrowserCueingStages.Length} cueing stage(s) for profile '{absorptionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser cueing session ready for profile '{absorptionReadyState.ProfileId}' with {result.BrowserCueingStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCueingSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCueingSessionVersion { get; set; } = string.Empty;
    public string BrowserAbsorptionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAbsorptionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCueingStages { get; set; } = Array.Empty<string>();
    public string BrowserCueingSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
