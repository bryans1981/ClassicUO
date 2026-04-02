namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEnablementReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEnablementReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEnablementReadyStateService : IBrowserClientRuntimeBrowserEnablementReadyState
{
    private readonly IBrowserClientRuntimeBrowserEnablementSession _runtimeBrowserEnablementSession;

    public BrowserClientRuntimeBrowserEnablementReadyStateService(IBrowserClientRuntimeBrowserEnablementSession runtimeBrowserEnablementSession)
    {
        _runtimeBrowserEnablementSession = runtimeBrowserEnablementSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEnablementReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEnablementSessionResult enablementSession = await _runtimeBrowserEnablementSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEnablementReadyStateResult result = new()
        {
            ProfileId = enablementSession.ProfileId,
            SessionId = enablementSession.SessionId,
            SessionPath = enablementSession.SessionPath,
            BrowserEnablementSessionVersion = enablementSession.BrowserEnablementSessionVersion,
            BrowserAssistanceReadyStateVersion = enablementSession.BrowserAssistanceReadyStateVersion,
            BrowserAssistanceSessionVersion = enablementSession.BrowserAssistanceSessionVersion,
            LaunchMode = enablementSession.LaunchMode,
            AssetRootPath = enablementSession.AssetRootPath,
            ProfilesRootPath = enablementSession.ProfilesRootPath,
            CacheRootPath = enablementSession.CacheRootPath,
            ConfigRootPath = enablementSession.ConfigRootPath,
            SettingsFilePath = enablementSession.SettingsFilePath,
            StartupProfilePath = enablementSession.StartupProfilePath,
            RequiredAssets = enablementSession.RequiredAssets,
            ReadyAssetCount = enablementSession.ReadyAssetCount,
            CompletedSteps = enablementSession.CompletedSteps,
            TotalSteps = enablementSession.TotalSteps,
            Exists = enablementSession.Exists,
            ReadSucceeded = enablementSession.ReadSucceeded
        };

        if (!enablementSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser enablement ready state blocked for profile '{enablementSession.ProfileId}'.";
            result.Error = enablementSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEnablementReadyStateVersion = "runtime-browser-enablement-ready-state-v1";
        result.BrowserEnablementReadyChecks =
        [
            "browser-assistance-ready-state-ready",
            "browser-enablement-session-ready",
            "browser-enablement-ready"
        ];
        result.BrowserEnablementReadySummary = $"Runtime browser enablement ready state passed {result.BrowserEnablementReadyChecks.Length} enablement readiness check(s) for profile '{enablementSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser enablement ready state ready for profile '{enablementSession.ProfileId}' with {result.BrowserEnablementReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEnablementReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEnablementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEnablementSessionVersion { get; set; } = string.Empty;
    public string BrowserAssistanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssistanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEnablementReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEnablementReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
