namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserForgivenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserForgivenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserForgivenessReadyStateService : IBrowserClientRuntimeBrowserForgivenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserForgivenessSession _runtimeBrowserForgivenessSession;

    public BrowserClientRuntimeBrowserForgivenessReadyStateService(IBrowserClientRuntimeBrowserForgivenessSession runtimeBrowserForgivenessSession)
    {
        _runtimeBrowserForgivenessSession = runtimeBrowserForgivenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserForgivenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserForgivenessSessionResult forgivenessSession = await _runtimeBrowserForgivenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserForgivenessReadyStateResult result = new()
        {
            ProfileId = forgivenessSession.ProfileId,
            SessionId = forgivenessSession.SessionId,
            SessionPath = forgivenessSession.SessionPath,
            BrowserForgivenessSessionVersion = forgivenessSession.BrowserForgivenessSessionVersion,
            BrowserRecoverabilityReadyStateVersion = forgivenessSession.BrowserRecoverabilityReadyStateVersion,
            BrowserRecoverabilitySessionVersion = forgivenessSession.BrowserRecoverabilitySessionVersion,
            LaunchMode = forgivenessSession.LaunchMode,
            AssetRootPath = forgivenessSession.AssetRootPath,
            ProfilesRootPath = forgivenessSession.ProfilesRootPath,
            CacheRootPath = forgivenessSession.CacheRootPath,
            ConfigRootPath = forgivenessSession.ConfigRootPath,
            SettingsFilePath = forgivenessSession.SettingsFilePath,
            StartupProfilePath = forgivenessSession.StartupProfilePath,
            RequiredAssets = forgivenessSession.RequiredAssets,
            ReadyAssetCount = forgivenessSession.ReadyAssetCount,
            CompletedSteps = forgivenessSession.CompletedSteps,
            TotalSteps = forgivenessSession.TotalSteps,
            Exists = forgivenessSession.Exists,
            ReadSucceeded = forgivenessSession.ReadSucceeded
        };

        if (!forgivenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser forgiveness ready state blocked for profile '{forgivenessSession.ProfileId}'.";
            result.Error = forgivenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserForgivenessReadyStateVersion = "runtime-browser-forgiveness-ready-state-v1";
        result.BrowserForgivenessReadyChecks =
        [
            "browser-recoverability-ready-state-ready",
            "browser-forgiveness-session-ready",
            "browser-forgiveness-ready"
        ];
        result.BrowserForgivenessReadySummary = $"Runtime browser forgiveness ready state passed {result.BrowserForgivenessReadyChecks.Length} forgiveness readiness check(s) for profile '{forgivenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser forgiveness ready state ready for profile '{forgivenessSession.ProfileId}' with {result.BrowserForgivenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserForgivenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserForgivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserForgivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserRecoverabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRecoverabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserForgivenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserForgivenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
