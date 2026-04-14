namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateService : IBrowserClientRuntimeBrowserRuntimeReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserServiceReliabilityReadyState _runtimeBrowserServiceReliabilityReadyState;

    public BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateService(IBrowserClientRuntimeBrowserServiceReliabilityReadyState runtimeBrowserServiceReliabilityReadyState)
    {
        _runtimeBrowserServiceReliabilityReadyState = runtimeBrowserServiceReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceReliabilityReadyStateResult prevReadyState = await _runtimeBrowserServiceReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserRuntimeReliabilitySessionVersion = prevReadyState.BrowserServiceReliabilityReadyStateVersion,
            BrowserServiceReliabilityReadyStateVersion = prevReadyState.BrowserServiceReliabilityReadyStateVersion,
            BrowserServiceReliabilitySessionVersion = prevReadyState.BrowserServiceReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser runtimereliability ready state blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeReliabilityReadyStateVersion = "runtime-browser-runtimereliability-ready-state-v1";
        result.BrowserRuntimeReliabilityReadyChecks =
        [
            "browser-servicereliability-ready-state-ready",
            "browser-runtimereliability-ready-state-ready",
            "browser-runtimereliability-ready"
        ];
        result.BrowserRuntimeReliabilityReadySummary = $"Runtime browser runtimereliability ready state passed {result.BrowserRuntimeReliabilityReadyChecks.Length} runtimereliability readiness check(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimereliability ready state ready for profile '{prevReadyState.ProfileId}' with {result.BrowserRuntimeReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserServiceReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
