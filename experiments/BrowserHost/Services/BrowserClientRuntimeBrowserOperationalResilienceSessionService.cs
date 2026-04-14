namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserOperationalResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalResilienceSessionService : IBrowserClientRuntimeBrowserOperationalResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserSteadyOperationReadinessReadyState _runtimeBrowserSteadyOperationReadinessReadyState;

    public BrowserClientRuntimeBrowserOperationalResilienceSessionService(IBrowserClientRuntimeBrowserSteadyOperationReadinessReadyState runtimeBrowserSteadyOperationReadinessReadyState)
    {
        _runtimeBrowserSteadyOperationReadinessReadyState = runtimeBrowserSteadyOperationReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyOperationReadinessReadyStateResult prevReadyState = await _runtimeBrowserSteadyOperationReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOperationalResilienceSessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSteadyOperationReadinessReadyStateVersion = prevReadyState.BrowserSteadyOperationReadinessReadyStateVersion,
            BrowserSteadyOperationReadinessSessionVersion = prevReadyState.BrowserSteadyOperationReadinessSessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operationalresilience session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalResilienceSessionVersion = "runtime-browser-operationalresilience-session-v1";
        result.BrowserOperationalResilienceStages =
        [
            "open-browser-operationalresilience-session",
            "bind-browser-steadyoperationreadiness-ready-state",
            "publish-browser-operationalresilience-ready"
        ];
        result.BrowserOperationalResilienceSummary = $"Runtime browser operationalresilience session prepared {result.BrowserOperationalResilienceStages.Length} operationalresilience stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalresilience session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserOperationalResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalResilienceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserOperationalResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserOperationalResilienceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
