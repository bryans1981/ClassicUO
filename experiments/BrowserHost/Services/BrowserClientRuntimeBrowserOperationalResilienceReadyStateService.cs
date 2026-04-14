namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserOperationalResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalResilienceReadyStateService : IBrowserClientRuntimeBrowserOperationalResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserOperationalResilienceSession _runtimeBrowserOperationalResilienceSession;

    public BrowserClientRuntimeBrowserOperationalResilienceReadyStateService(IBrowserClientRuntimeBrowserOperationalResilienceSession runtimeBrowserOperationalResilienceSession)
    {
        _runtimeBrowserOperationalResilienceSession = runtimeBrowserOperationalResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserOperationalResilienceSessionResult operationalresilienceSession = await _runtimeBrowserOperationalResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserOperationalResilienceReadyStateResult result = new()
        {
            ProfileId = operationalresilienceSession.ProfileId,
            SessionId = operationalresilienceSession.SessionId,
            SessionPath = operationalresilienceSession.SessionPath,
            BrowserOperationalResilienceSessionVersion = operationalresilienceSession.BrowserOperationalResilienceSessionVersion,
            BrowserSteadyOperationReadinessReadyStateVersion = operationalresilienceSession.BrowserSteadyOperationReadinessReadyStateVersion,
            BrowserSteadyOperationReadinessSessionVersion = operationalresilienceSession.BrowserSteadyOperationReadinessSessionVersion,
            LaunchMode = operationalresilienceSession.LaunchMode,
            AssetRootPath = operationalresilienceSession.AssetRootPath,
            ProfilesRootPath = operationalresilienceSession.ProfilesRootPath,
            CacheRootPath = operationalresilienceSession.CacheRootPath,
            ConfigRootPath = operationalresilienceSession.ConfigRootPath,
            SettingsFilePath = operationalresilienceSession.SettingsFilePath,
            StartupProfilePath = operationalresilienceSession.StartupProfilePath,
            RequiredAssets = operationalresilienceSession.RequiredAssets,
            ReadyAssetCount = operationalresilienceSession.ReadyAssetCount,
            CompletedSteps = operationalresilienceSession.CompletedSteps,
            TotalSteps = operationalresilienceSession.TotalSteps,
            Exists = operationalresilienceSession.Exists,
            ReadSucceeded = operationalresilienceSession.ReadSucceeded
        };

        if (!operationalresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operationalresilience ready state blocked for profile '{operationalresilienceSession.ProfileId}'.";
            result.Error = operationalresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalResilienceReadyStateVersion = "runtime-browser-operationalresilience-ready-state-v1";
        result.BrowserOperationalResilienceReadyChecks =
        [
            "browser-steadyoperationreadiness-ready-state-ready",
            "browser-operationalresilience-session-ready",
            "browser-operationalresilience-ready"
        ];
        result.BrowserOperationalResilienceReadySummary = $"Runtime browser operationalresilience ready state passed {result.BrowserOperationalResilienceReadyChecks.Length} operationalresilience readiness check(s) for profile '{operationalresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalresilience ready state ready for profile '{operationalresilienceSession.ProfileId}' with {result.BrowserOperationalResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserOperationalResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserOperationalResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserSteadyOperationReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyOperationReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperationalResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserOperationalResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
