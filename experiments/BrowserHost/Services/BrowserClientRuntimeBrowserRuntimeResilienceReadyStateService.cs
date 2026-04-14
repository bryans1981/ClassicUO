namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeResilienceReadyStateService : IBrowserClientRuntimeBrowserRuntimeResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserRuntimeResilienceSession _runtimeBrowserRuntimeResilienceSession;

    public BrowserClientRuntimeBrowserRuntimeResilienceReadyStateService(IBrowserClientRuntimeBrowserRuntimeResilienceSession runtimeBrowserRuntimeResilienceSession)
    {
        _runtimeBrowserRuntimeResilienceSession = runtimeBrowserRuntimeResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeResilienceSessionResult runtimeresilienceSession = await _runtimeBrowserRuntimeResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeResilienceReadyStateResult result = new()
        {
            ProfileId = runtimeresilienceSession.ProfileId,
            SessionId = runtimeresilienceSession.SessionId,
            SessionPath = runtimeresilienceSession.SessionPath,
            BrowserRuntimeResilienceSessionVersion = runtimeresilienceSession.BrowserRuntimeResilienceSessionVersion,
            BrowserServiceResilienceReadyStateVersion = runtimeresilienceSession.BrowserServiceResilienceReadyStateVersion,
            BrowserServiceResilienceSessionVersion = runtimeresilienceSession.BrowserServiceResilienceSessionVersion,
            LaunchMode = runtimeresilienceSession.LaunchMode,
            AssetRootPath = runtimeresilienceSession.AssetRootPath,
            ProfilesRootPath = runtimeresilienceSession.ProfilesRootPath,
            CacheRootPath = runtimeresilienceSession.CacheRootPath,
            ConfigRootPath = runtimeresilienceSession.ConfigRootPath,
            SettingsFilePath = runtimeresilienceSession.SettingsFilePath,
            StartupProfilePath = runtimeresilienceSession.StartupProfilePath,
            RequiredAssets = runtimeresilienceSession.RequiredAssets,
            ReadyAssetCount = runtimeresilienceSession.ReadyAssetCount,
            CompletedSteps = runtimeresilienceSession.CompletedSteps,
            TotalSteps = runtimeresilienceSession.TotalSteps,
            Exists = runtimeresilienceSession.Exists,
            ReadSucceeded = runtimeresilienceSession.ReadSucceeded
        };

        if (!runtimeresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimeresilience ready state blocked for profile '{runtimeresilienceSession.ProfileId}'.";
            result.Error = runtimeresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeResilienceReadyStateVersion = "runtime-browser-runtimeresilience-ready-state-v1";
        result.BrowserRuntimeResilienceReadyChecks =
        [
            "browser-serviceresilience-ready-state-ready",
            "browser-runtimeresilience-session-ready",
            "browser-runtimeresilience-ready"
        ];
        result.BrowserRuntimeResilienceReadySummary = $"Runtime browser runtimeresilience ready state passed {result.BrowserRuntimeResilienceReadyChecks.Length} runtimeresilience readiness check(s) for profile '{runtimeresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimeresilience ready state ready for profile '{runtimeresilienceSession.ProfileId}' with {result.BrowserRuntimeResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserServiceResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
