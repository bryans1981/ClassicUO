namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperationalStabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserOperationalStabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperationalStabilitySessionService : IBrowserClientRuntimeBrowserOperationalStabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSteadyStateReadinessReadyState _runtimeBrowserSteadyStateReadinessReadyState;

    public BrowserClientRuntimeBrowserOperationalStabilitySessionService(IBrowserClientRuntimeBrowserSteadyStateReadinessReadyState runtimeBrowserSteadyStateReadinessReadyState)
    {
        _runtimeBrowserSteadyStateReadinessReadyState = runtimeBrowserSteadyStateReadinessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperationalStabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSteadyStateReadinessReadyStateResult prevReadyState = await _runtimeBrowserSteadyStateReadinessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOperationalStabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSteadyStateReadinessReadyStateVersion = prevReadyState.BrowserSteadyStateReadinessReadyStateVersion,
            BrowserSteadyStateReadinessSessionVersion = prevReadyState.BrowserSteadyStateReadinessSessionVersion,
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
            result.Summary = $"Runtime browser operationalstability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperationalStabilitySessionVersion = "runtime-browser-operationalstability-session-v1";
        result.BrowserOperationalStabilityStages =
        [
            "open-browser-operationalstability-session",
            "bind-browser-steadystatereadiness-ready-state",
            "publish-browser-operationalstability-ready"
        ];
        result.BrowserOperationalStabilitySummary = $"Runtime browser operationalstability session prepared {result.BrowserOperationalStabilityStages.Length} operationalstability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operationalstability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserOperationalStabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperationalStabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserOperationalStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSteadyStateReadinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSteadyStateReadinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperationalStabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserOperationalStabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
