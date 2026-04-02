namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTaskFitReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTaskFitReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTaskFitReadyStateService : IBrowserClientRuntimeBrowserTaskFitReadyState
{
    private readonly IBrowserClientRuntimeBrowserTaskFitSession _runtimeBrowserTaskFitSession;

    public BrowserClientRuntimeBrowserTaskFitReadyStateService(IBrowserClientRuntimeBrowserTaskFitSession runtimeBrowserTaskFitSession)
    {
        _runtimeBrowserTaskFitSession = runtimeBrowserTaskFitSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTaskFitReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTaskFitSessionResult taskfitSession = await _runtimeBrowserTaskFitSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTaskFitReadyStateResult result = new()
        {
            ProfileId = taskfitSession.ProfileId,
            SessionId = taskfitSession.SessionId,
            SessionPath = taskfitSession.SessionPath,
            BrowserTaskFitSessionVersion = taskfitSession.BrowserTaskFitSessionVersion,
            BrowserDirectionalityReadyStateVersion = taskfitSession.BrowserDirectionalityReadyStateVersion,
            BrowserDirectionalitySessionVersion = taskfitSession.BrowserDirectionalitySessionVersion,
            LaunchMode = taskfitSession.LaunchMode,
            AssetRootPath = taskfitSession.AssetRootPath,
            ProfilesRootPath = taskfitSession.ProfilesRootPath,
            CacheRootPath = taskfitSession.CacheRootPath,
            ConfigRootPath = taskfitSession.ConfigRootPath,
            SettingsFilePath = taskfitSession.SettingsFilePath,
            StartupProfilePath = taskfitSession.StartupProfilePath,
            RequiredAssets = taskfitSession.RequiredAssets,
            ReadyAssetCount = taskfitSession.ReadyAssetCount,
            CompletedSteps = taskfitSession.CompletedSteps,
            TotalSteps = taskfitSession.TotalSteps,
            Exists = taskfitSession.Exists,
            ReadSucceeded = taskfitSession.ReadSucceeded
        };

        if (!taskfitSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser taskfit ready state blocked for profile '{taskfitSession.ProfileId}'.";
            result.Error = taskfitSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTaskFitReadyStateVersion = "runtime-browser-taskfit-ready-state-v1";
        result.BrowserTaskFitReadyChecks =
        [
            "browser-directionality-ready-state-ready",
            "browser-taskfit-session-ready",
            "browser-taskfit-ready"
        ];
        result.BrowserTaskFitReadySummary = $"Runtime browser taskfit ready state passed {result.BrowserTaskFitReadyChecks.Length} taskfit readiness check(s) for profile '{taskfitSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser taskfit ready state ready for profile '{taskfitSession.ProfileId}' with {result.BrowserTaskFitReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTaskFitReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTaskFitReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskFitSessionVersion { get; set; } = string.Empty;
    public string BrowserDirectionalityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDirectionalitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTaskFitReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTaskFitReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
