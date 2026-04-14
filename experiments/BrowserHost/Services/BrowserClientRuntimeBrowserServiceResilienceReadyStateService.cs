namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceResilienceReadyStateService : IBrowserClientRuntimeBrowserServiceResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceResilienceSession _runtimeBrowserServiceResilienceSession;

    public BrowserClientRuntimeBrowserServiceResilienceReadyStateService(IBrowserClientRuntimeBrowserServiceResilienceSession runtimeBrowserServiceResilienceSession)
    {
        _runtimeBrowserServiceResilienceSession = runtimeBrowserServiceResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceResilienceSessionResult serviceresilienceSession = await _runtimeBrowserServiceResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserServiceResilienceReadyStateResult result = new()
        {
            ProfileId = serviceresilienceSession.ProfileId,
            SessionId = serviceresilienceSession.SessionId,
            SessionPath = serviceresilienceSession.SessionPath,
            BrowserServiceResilienceSessionVersion = serviceresilienceSession.BrowserServiceResilienceSessionVersion,
            BrowserGoLiveResilienceReadyStateVersion = serviceresilienceSession.BrowserGoLiveResilienceReadyStateVersion,
            BrowserGoLiveResilienceSessionVersion = serviceresilienceSession.BrowserGoLiveResilienceSessionVersion,
            LaunchMode = serviceresilienceSession.LaunchMode,
            AssetRootPath = serviceresilienceSession.AssetRootPath,
            ProfilesRootPath = serviceresilienceSession.ProfilesRootPath,
            CacheRootPath = serviceresilienceSession.CacheRootPath,
            ConfigRootPath = serviceresilienceSession.ConfigRootPath,
            SettingsFilePath = serviceresilienceSession.SettingsFilePath,
            StartupProfilePath = serviceresilienceSession.StartupProfilePath,
            RequiredAssets = serviceresilienceSession.RequiredAssets,
            ReadyAssetCount = serviceresilienceSession.ReadyAssetCount,
            CompletedSteps = serviceresilienceSession.CompletedSteps,
            TotalSteps = serviceresilienceSession.TotalSteps,
            Exists = serviceresilienceSession.Exists,
            ReadSucceeded = serviceresilienceSession.ReadSucceeded
        };

        if (!serviceresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceresilience ready state blocked for profile '{serviceresilienceSession.ProfileId}'.";
            result.Error = serviceresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceResilienceReadyStateVersion = "runtime-browser-serviceresilience-ready-state-v1";
        result.BrowserServiceResilienceReadyChecks =
        [
            "browser-goliveresilience-ready-state-ready",
            "browser-serviceresilience-session-ready",
            "browser-serviceresilience-ready"
        ];
        result.BrowserServiceResilienceReadySummary = $"Runtime browser serviceresilience ready state passed {result.BrowserServiceResilienceReadyChecks.Length} serviceresilience readiness check(s) for profile '{serviceresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceresilience ready state ready for profile '{serviceresilienceSession.ProfileId}' with {result.BrowserServiceResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserGoLiveResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
