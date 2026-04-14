namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceActivationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceActivationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceActivationReadyStateService : IBrowserClientRuntimeBrowserServiceActivationReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceActivationSession _runtimeBrowserServiceActivationSession;

    public BrowserClientRuntimeBrowserServiceActivationReadyStateService(IBrowserClientRuntimeBrowserServiceActivationSession runtimeBrowserServiceActivationSession)
    {
        _runtimeBrowserServiceActivationSession = runtimeBrowserServiceActivationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceActivationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceActivationSessionResult serviceactivationSession = await _runtimeBrowserServiceActivationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserServiceActivationReadyStateResult result = new()
        {
            ProfileId = serviceactivationSession.ProfileId,
            SessionId = serviceactivationSession.SessionId,
            SessionPath = serviceactivationSession.SessionPath,
            BrowserServiceActivationSessionVersion = serviceactivationSession.BrowserServiceActivationSessionVersion,
            BrowserGoLiveReadinessReadyStateVersion = serviceactivationSession.BrowserGoLiveReadinessReadyStateVersion,
            BrowserGoLiveReadinessSessionVersion = serviceactivationSession.BrowserGoLiveReadinessSessionVersion,
            LaunchMode = serviceactivationSession.LaunchMode,
            AssetRootPath = serviceactivationSession.AssetRootPath,
            ProfilesRootPath = serviceactivationSession.ProfilesRootPath,
            CacheRootPath = serviceactivationSession.CacheRootPath,
            ConfigRootPath = serviceactivationSession.ConfigRootPath,
            SettingsFilePath = serviceactivationSession.SettingsFilePath,
            StartupProfilePath = serviceactivationSession.StartupProfilePath,
            RequiredAssets = serviceactivationSession.RequiredAssets,
            ReadyAssetCount = serviceactivationSession.ReadyAssetCount,
            CompletedSteps = serviceactivationSession.CompletedSteps,
            TotalSteps = serviceactivationSession.TotalSteps,
            Exists = serviceactivationSession.Exists,
            ReadSucceeded = serviceactivationSession.ReadSucceeded
        };

        if (!serviceactivationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceactivation ready state blocked for profile '{serviceactivationSession.ProfileId}'.";
            result.Error = serviceactivationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceActivationReadyStateVersion = "runtime-browser-serviceactivation-ready-state-v1";
        result.BrowserServiceActivationReadyChecks =
        [
            "browser-golivereadiness-ready-state-ready",
            "browser-serviceactivation-session-ready",
            "browser-serviceactivation-ready"
        ];
        result.BrowserServiceActivationReadySummary = $"Runtime browser serviceactivation ready state passed {result.BrowserServiceActivationReadyChecks.Length} serviceactivation readiness check(s) for profile '{serviceactivationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceactivation ready state ready for profile '{serviceactivationSession.ProfileId}' with {result.BrowserServiceActivationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceActivationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceActivationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceActivationSessionVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGoLiveReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceActivationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceActivationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
