namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserVisibilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserVisibilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserVisibilityReadyStateService : IBrowserClientRuntimeBrowserVisibilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserVisibilitySession _runtimeBrowserVisibilitySession;

    public BrowserClientRuntimeBrowserVisibilityReadyStateService(IBrowserClientRuntimeBrowserVisibilitySession runtimeBrowserVisibilitySession)
    {
        _runtimeBrowserVisibilitySession = runtimeBrowserVisibilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserVisibilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserVisibilitySessionResult visibilitySession = await _runtimeBrowserVisibilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserVisibilityReadyStateResult result = new()
        {
            ProfileId = visibilitySession.ProfileId,
            SessionId = visibilitySession.SessionId,
            SessionPath = visibilitySession.SessionPath,
            BrowserVisibilitySessionVersion = visibilitySession.BrowserVisibilitySessionVersion,
            BrowserTransparencyReadyStateVersion = visibilitySession.BrowserTransparencyReadyStateVersion,
            BrowserTransparencySessionVersion = visibilitySession.BrowserTransparencySessionVersion,
            LaunchMode = visibilitySession.LaunchMode,
            AssetRootPath = visibilitySession.AssetRootPath,
            ProfilesRootPath = visibilitySession.ProfilesRootPath,
            CacheRootPath = visibilitySession.CacheRootPath,
            ConfigRootPath = visibilitySession.ConfigRootPath,
            SettingsFilePath = visibilitySession.SettingsFilePath,
            StartupProfilePath = visibilitySession.StartupProfilePath,
            RequiredAssets = visibilitySession.RequiredAssets,
            ReadyAssetCount = visibilitySession.ReadyAssetCount,
            CompletedSteps = visibilitySession.CompletedSteps,
            TotalSteps = visibilitySession.TotalSteps,
            Exists = visibilitySession.Exists,
            ReadSucceeded = visibilitySession.ReadSucceeded
        };

        if (!visibilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser visibility ready state blocked for profile '{visibilitySession.ProfileId}'.";
            result.Error = visibilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserVisibilityReadyStateVersion = "runtime-browser-visibility-ready-state-v1";
        result.BrowserVisibilityReadyChecks =
        [
            "browser-transparency-ready-state-ready",
            "browser-visibility-session-ready",
            "browser-visibility-ready"
        ];
        result.BrowserVisibilityReadySummary = $"Runtime browser visibility ready state passed {result.BrowserVisibilityReadyChecks.Length} visibility readiness check(s) for profile '{visibilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser visibility ready state ready for profile '{visibilitySession.ProfileId}' with {result.BrowserVisibilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserVisibilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserVisibilityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserVisibilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserVisibilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
