namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserVisibilitySession
{
    ValueTask<BrowserClientRuntimeBrowserVisibilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserVisibilitySessionService : IBrowserClientRuntimeBrowserVisibilitySession
{
    private readonly IBrowserClientRuntimeBrowserTransparencyReadyState _runtimeBrowserTransparencyReadyState;

    public BrowserClientRuntimeBrowserVisibilitySessionService(IBrowserClientRuntimeBrowserTransparencyReadyState runtimeBrowserTransparencyReadyState)
    {
        _runtimeBrowserTransparencyReadyState = runtimeBrowserTransparencyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserVisibilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTransparencyReadyStateResult transparencyReadyState = await _runtimeBrowserTransparencyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserVisibilitySessionResult result = new()
        {
            ProfileId = transparencyReadyState.ProfileId,
            SessionId = transparencyReadyState.SessionId,
            SessionPath = transparencyReadyState.SessionPath,
            BrowserTransparencyReadyStateVersion = transparencyReadyState.BrowserTransparencyReadyStateVersion,
            BrowserTransparencySessionVersion = transparencyReadyState.BrowserTransparencySessionVersion,
            LaunchMode = transparencyReadyState.LaunchMode,
            AssetRootPath = transparencyReadyState.AssetRootPath,
            ProfilesRootPath = transparencyReadyState.ProfilesRootPath,
            CacheRootPath = transparencyReadyState.CacheRootPath,
            ConfigRootPath = transparencyReadyState.ConfigRootPath,
            SettingsFilePath = transparencyReadyState.SettingsFilePath,
            StartupProfilePath = transparencyReadyState.StartupProfilePath,
            RequiredAssets = transparencyReadyState.RequiredAssets,
            ReadyAssetCount = transparencyReadyState.ReadyAssetCount,
            CompletedSteps = transparencyReadyState.CompletedSteps,
            TotalSteps = transparencyReadyState.TotalSteps,
            Exists = transparencyReadyState.Exists,
            ReadSucceeded = transparencyReadyState.ReadSucceeded
        };

        if (!transparencyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser visibility session blocked for profile '{transparencyReadyState.ProfileId}'.";
            result.Error = transparencyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserVisibilitySessionVersion = "runtime-browser-visibility-session-v1";
        result.BrowserVisibilityStages =
        [
            "open-browser-visibility-session",
            "bind-browser-transparency-ready-state",
            "publish-browser-visibility-ready"
        ];
        result.BrowserVisibilitySummary = $"Runtime browser visibility session prepared {result.BrowserVisibilityStages.Length} visibility stage(s) for profile '{transparencyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser visibility session ready for profile '{transparencyReadyState.ProfileId}' with {result.BrowserVisibilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserVisibilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserVisibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserTransparencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTransparencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserVisibilityStages { get; set; } = Array.Empty<string>();
    public string BrowserVisibilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
