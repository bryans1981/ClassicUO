namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeReliabilitySessionService : IBrowserClientRuntimeBrowserRuntimeReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserServiceReliabilityReadyState _runtimeBrowserServiceReliabilityReadyState;

    public BrowserClientRuntimeBrowserRuntimeReliabilitySessionService(IBrowserClientRuntimeBrowserServiceReliabilityReadyState runtimeBrowserServiceReliabilityReadyState)
    {
        _runtimeBrowserServiceReliabilityReadyState = runtimeBrowserServiceReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceReliabilityReadyStateResult prevReadyState = await _runtimeBrowserServiceReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
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
            result.Summary = $"Runtime browser runtimereliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeReliabilitySessionVersion = "runtime-browser-runtimereliability-session-v1";
        result.BrowserRuntimeReliabilityStages =
        [
            "open-browser-runtimereliability-session",
            "bind-browser-servicereliability-ready-state",
            "publish-browser-runtimereliability-ready"
        ];
        result.BrowserRuntimeReliabilitySummary = $"Runtime browser runtimereliability session prepared {result.BrowserRuntimeReliabilityStages.Length} runtimereliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimereliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserRuntimeReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeReliabilitySessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserRuntimeReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
