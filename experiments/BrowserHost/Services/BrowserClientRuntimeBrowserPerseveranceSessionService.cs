namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPerseveranceSession
{
    ValueTask<BrowserClientRuntimeBrowserPerseveranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPerseveranceSessionService : IBrowserClientRuntimeBrowserPerseveranceSession
{
    private readonly IBrowserClientRuntimeBrowserDedicationReadyState _runtimeBrowserDedicationReadyState;

    public BrowserClientRuntimeBrowserPerseveranceSessionService(IBrowserClientRuntimeBrowserDedicationReadyState runtimeBrowserDedicationReadyState)
    {
        _runtimeBrowserDedicationReadyState = runtimeBrowserDedicationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPerseveranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDedicationReadyStateResult dedicationReadyState = await _runtimeBrowserDedicationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPerseveranceSessionResult result = new()
        {
            ProfileId = dedicationReadyState.ProfileId,
            SessionId = dedicationReadyState.SessionId,
            SessionPath = dedicationReadyState.SessionPath,
            BrowserDedicationReadyStateVersion = dedicationReadyState.BrowserDedicationReadyStateVersion,
            BrowserDedicationSessionVersion = dedicationReadyState.BrowserDedicationSessionVersion,
            LaunchMode = dedicationReadyState.LaunchMode,
            AssetRootPath = dedicationReadyState.AssetRootPath,
            ProfilesRootPath = dedicationReadyState.ProfilesRootPath,
            CacheRootPath = dedicationReadyState.CacheRootPath,
            ConfigRootPath = dedicationReadyState.ConfigRootPath,
            SettingsFilePath = dedicationReadyState.SettingsFilePath,
            StartupProfilePath = dedicationReadyState.StartupProfilePath,
            RequiredAssets = dedicationReadyState.RequiredAssets,
            ReadyAssetCount = dedicationReadyState.ReadyAssetCount,
            CompletedSteps = dedicationReadyState.CompletedSteps,
            TotalSteps = dedicationReadyState.TotalSteps,
            Exists = dedicationReadyState.Exists,
            ReadSucceeded = dedicationReadyState.ReadSucceeded
        };

        if (!dedicationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser perseverance session blocked for profile '{dedicationReadyState.ProfileId}'.";
            result.Error = dedicationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPerseveranceSessionVersion = "runtime-browser-perseverance-session-v1";
        result.BrowserPerseveranceStages =
        [
            "open-browser-perseverance-session",
            "bind-browser-dedication-ready-state",
            "publish-browser-perseverance-ready"
        ];
        result.BrowserPerseveranceSummary = $"Runtime browser perseverance session prepared {result.BrowserPerseveranceStages.Length} perseverance stage(s) for profile '{dedicationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser perseverance session ready for profile '{dedicationReadyState.ProfileId}' with {result.BrowserPerseveranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPerseveranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPerseveranceSessionVersion { get; set; } = string.Empty;
    public string BrowserDedicationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDedicationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPerseveranceStages { get; set; } = Array.Empty<string>();
    public string BrowserPerseveranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
