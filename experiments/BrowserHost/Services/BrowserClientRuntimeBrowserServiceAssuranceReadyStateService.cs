namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserServiceAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceAssuranceReadyStateService : IBrowserClientRuntimeBrowserServiceAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceAssuranceSession _runtimeBrowserServiceAssuranceSession;

    public BrowserClientRuntimeBrowserServiceAssuranceReadyStateService(IBrowserClientRuntimeBrowserServiceAssuranceSession runtimeBrowserServiceAssuranceSession)
    {
        _runtimeBrowserServiceAssuranceSession = runtimeBrowserServiceAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceAssuranceSessionResult serviceassuranceSession = await _runtimeBrowserServiceAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserServiceAssuranceReadyStateResult result = new()
        {
            ProfileId = serviceassuranceSession.ProfileId,
            SessionId = serviceassuranceSession.SessionId,
            SessionPath = serviceassuranceSession.SessionPath,
            BrowserServiceAssuranceSessionVersion = serviceassuranceSession.BrowserServiceAssuranceSessionVersion,
            BrowserRuntimeOperabilityReadyStateVersion = serviceassuranceSession.BrowserRuntimeOperabilityReadyStateVersion,
            BrowserRuntimeOperabilitySessionVersion = serviceassuranceSession.BrowserRuntimeOperabilitySessionVersion,
            LaunchMode = serviceassuranceSession.LaunchMode,
            AssetRootPath = serviceassuranceSession.AssetRootPath,
            ProfilesRootPath = serviceassuranceSession.ProfilesRootPath,
            CacheRootPath = serviceassuranceSession.CacheRootPath,
            ConfigRootPath = serviceassuranceSession.ConfigRootPath,
            SettingsFilePath = serviceassuranceSession.SettingsFilePath,
            StartupProfilePath = serviceassuranceSession.StartupProfilePath,
            RequiredAssets = serviceassuranceSession.RequiredAssets,
            ReadyAssetCount = serviceassuranceSession.ReadyAssetCount,
            CompletedSteps = serviceassuranceSession.CompletedSteps,
            TotalSteps = serviceassuranceSession.TotalSteps,
            Exists = serviceassuranceSession.Exists,
            ReadSucceeded = serviceassuranceSession.ReadSucceeded
        };

        if (!serviceassuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceassurance ready state blocked for profile '{serviceassuranceSession.ProfileId}'.";
            result.Error = serviceassuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceAssuranceReadyStateVersion = "runtime-browser-serviceassurance-ready-state-v1";
        result.BrowserServiceAssuranceReadyChecks =
        [
            "browser-runtimeoperability-ready-state-ready",
            "browser-serviceassurance-session-ready",
            "browser-serviceassurance-ready"
        ];
        result.BrowserServiceAssuranceReadySummary = $"Runtime browser serviceassurance ready state passed {result.BrowserServiceAssuranceReadyChecks.Length} serviceassurance readiness check(s) for profile '{serviceassuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceassurance ready state ready for profile '{serviceassuranceSession.ProfileId}' with {result.BrowserServiceAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeOperabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserServiceAssuranceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
