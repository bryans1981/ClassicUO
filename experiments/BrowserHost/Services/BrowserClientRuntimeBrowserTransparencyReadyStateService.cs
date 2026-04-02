namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTransparencyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTransparencyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTransparencyReadyStateService : IBrowserClientRuntimeBrowserTransparencyReadyState
{
    private readonly IBrowserClientRuntimeBrowserTransparencySession _runtimeBrowserTransparencySession;

    public BrowserClientRuntimeBrowserTransparencyReadyStateService(IBrowserClientRuntimeBrowserTransparencySession runtimeBrowserTransparencySession)
    {
        _runtimeBrowserTransparencySession = runtimeBrowserTransparencySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTransparencyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTransparencySessionResult transparencySession = await _runtimeBrowserTransparencySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTransparencyReadyStateResult result = new()
        {
            ProfileId = transparencySession.ProfileId,
            SessionId = transparencySession.SessionId,
            SessionPath = transparencySession.SessionPath,
            BrowserTransparencySessionVersion = transparencySession.BrowserTransparencySessionVersion,
            BrowserComfortReadyStateVersion = transparencySession.BrowserComfortReadyStateVersion,
            BrowserComfortSessionVersion = transparencySession.BrowserComfortSessionVersion,
            LaunchMode = transparencySession.LaunchMode,
            AssetRootPath = transparencySession.AssetRootPath,
            ProfilesRootPath = transparencySession.ProfilesRootPath,
            CacheRootPath = transparencySession.CacheRootPath,
            ConfigRootPath = transparencySession.ConfigRootPath,
            SettingsFilePath = transparencySession.SettingsFilePath,
            StartupProfilePath = transparencySession.StartupProfilePath,
            RequiredAssets = transparencySession.RequiredAssets,
            ReadyAssetCount = transparencySession.ReadyAssetCount,
            CompletedSteps = transparencySession.CompletedSteps,
            TotalSteps = transparencySession.TotalSteps,
            Exists = transparencySession.Exists,
            ReadSucceeded = transparencySession.ReadSucceeded
        };

        if (!transparencySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser transparency ready state blocked for profile '{transparencySession.ProfileId}'.";
            result.Error = transparencySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTransparencyReadyStateVersion = "runtime-browser-transparency-ready-state-v1";
        result.BrowserTransparencyReadyChecks =
        [
            "browser-comfort-ready-state-ready",
            "browser-transparency-session-ready",
            "browser-transparency-ready"
        ];
        result.BrowserTransparencyReadySummary = $"Runtime browser transparency ready state passed {result.BrowserTransparencyReadyChecks.Length} transparency readiness check(s) for profile '{transparencySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser transparency ready state ready for profile '{transparencySession.ProfileId}' with {result.BrowserTransparencyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTransparencyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTransparencyReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserTransparencyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTransparencyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
