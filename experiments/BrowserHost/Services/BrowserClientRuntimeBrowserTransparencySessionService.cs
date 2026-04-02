namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTransparencySession
{
    ValueTask<BrowserClientRuntimeBrowserTransparencySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTransparencySessionService : IBrowserClientRuntimeBrowserTransparencySession
{
    private readonly IBrowserClientRuntimeBrowserComfortReadyState _runtimeBrowserComfortReadyState;

    public BrowserClientRuntimeBrowserTransparencySessionService(IBrowserClientRuntimeBrowserComfortReadyState runtimeBrowserComfortReadyState)
    {
        _runtimeBrowserComfortReadyState = runtimeBrowserComfortReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTransparencySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComfortReadyStateResult comfortReadyState = await _runtimeBrowserComfortReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTransparencySessionResult result = new()
        {
            ProfileId = comfortReadyState.ProfileId,
            SessionId = comfortReadyState.SessionId,
            SessionPath = comfortReadyState.SessionPath,
            BrowserComfortReadyStateVersion = comfortReadyState.BrowserComfortReadyStateVersion,
            BrowserComfortSessionVersion = comfortReadyState.BrowserComfortSessionVersion,
            LaunchMode = comfortReadyState.LaunchMode,
            AssetRootPath = comfortReadyState.AssetRootPath,
            ProfilesRootPath = comfortReadyState.ProfilesRootPath,
            CacheRootPath = comfortReadyState.CacheRootPath,
            ConfigRootPath = comfortReadyState.ConfigRootPath,
            SettingsFilePath = comfortReadyState.SettingsFilePath,
            StartupProfilePath = comfortReadyState.StartupProfilePath,
            RequiredAssets = comfortReadyState.RequiredAssets,
            ReadyAssetCount = comfortReadyState.ReadyAssetCount,
            CompletedSteps = comfortReadyState.CompletedSteps,
            TotalSteps = comfortReadyState.TotalSteps,
            Exists = comfortReadyState.Exists,
            ReadSucceeded = comfortReadyState.ReadSucceeded
        };

        if (!comfortReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser transparency session blocked for profile '{comfortReadyState.ProfileId}'.";
            result.Error = comfortReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTransparencySessionVersion = "runtime-browser-transparency-session-v1";
        result.BrowserTransparencyStages =
        [
            "open-browser-transparency-session",
            "bind-browser-comfort-ready-state",
            "publish-browser-transparency-ready"
        ];
        result.BrowserTransparencySummary = $"Runtime browser transparency session prepared {result.BrowserTransparencyStages.Length} transparency stage(s) for profile '{comfortReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser transparency session ready for profile '{comfortReadyState.ProfileId}' with {result.BrowserTransparencyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTransparencySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTransparencySessionVersion { get; set; } = string.Empty;
    public string BrowserComfortReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComfortSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTransparencyStages { get; set; } = Array.Empty<string>();
    public string BrowserTransparencySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
