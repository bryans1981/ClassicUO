namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResolveReadyState
{
    ValueTask<BrowserClientRuntimeBrowserResolveReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResolveReadyStateService : IBrowserClientRuntimeBrowserResolveReadyState
{
    private readonly IBrowserClientRuntimeBrowserResolveSession _runtimeBrowserResolveSession;

    public BrowserClientRuntimeBrowserResolveReadyStateService(IBrowserClientRuntimeBrowserResolveSession runtimeBrowserResolveSession)
    {
        _runtimeBrowserResolveSession = runtimeBrowserResolveSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResolveReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResolveSessionResult resolveSession = await _runtimeBrowserResolveSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserResolveReadyStateResult result = new()
        {
            ProfileId = resolveSession.ProfileId,
            SessionId = resolveSession.SessionId,
            SessionPath = resolveSession.SessionPath,
            BrowserResolveSessionVersion = resolveSession.BrowserResolveSessionVersion,
            BrowserTenacityReadyStateVersion = resolveSession.BrowserTenacityReadyStateVersion,
            BrowserTenacitySessionVersion = resolveSession.BrowserTenacitySessionVersion,
            LaunchMode = resolveSession.LaunchMode,
            AssetRootPath = resolveSession.AssetRootPath,
            ProfilesRootPath = resolveSession.ProfilesRootPath,
            CacheRootPath = resolveSession.CacheRootPath,
            ConfigRootPath = resolveSession.ConfigRootPath,
            SettingsFilePath = resolveSession.SettingsFilePath,
            StartupProfilePath = resolveSession.StartupProfilePath,
            RequiredAssets = resolveSession.RequiredAssets,
            ReadyAssetCount = resolveSession.ReadyAssetCount,
            CompletedSteps = resolveSession.CompletedSteps,
            TotalSteps = resolveSession.TotalSteps,
            Exists = resolveSession.Exists,
            ReadSucceeded = resolveSession.ReadSucceeded
        };

        if (!resolveSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resolve ready state blocked for profile '{resolveSession.ProfileId}'.";
            result.Error = resolveSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResolveReadyStateVersion = "runtime-browser-resolve-ready-state-v1";
        result.BrowserResolveReadyChecks =
        [
            "browser-tenacity-ready-state-ready",
            "browser-resolve-session-ready",
            "browser-resolve-ready"
        ];
        result.BrowserResolveReadySummary = $"Runtime browser resolve ready state passed {result.BrowserResolveReadyChecks.Length} resolve readiness check(s) for profile '{resolveSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resolve ready state ready for profile '{resolveSession.ProfileId}' with {result.BrowserResolveReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResolveReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserResolveReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResolveSessionVersion { get; set; } = string.Empty;
    public string BrowserTenacityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTenacitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResolveReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserResolveReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
