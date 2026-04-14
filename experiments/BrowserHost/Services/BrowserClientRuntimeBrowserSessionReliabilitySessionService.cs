namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserSessionReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionReliabilitySessionService : IBrowserClientRuntimeBrowserSessionReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeReliabilityReadyState _runtimeBrowserRuntimeReliabilityReadyState;

    public BrowserClientRuntimeBrowserSessionReliabilitySessionService(IBrowserClientRuntimeBrowserRuntimeReliabilityReadyState runtimeBrowserRuntimeReliabilityReadyState)
    {
        _runtimeBrowserRuntimeReliabilityReadyState = runtimeBrowserRuntimeReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeReliabilityReadyStateResult prevReadyState = await _runtimeBrowserRuntimeReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSessionReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserRuntimeReliabilityReadyStateVersion = prevReadyState.BrowserRuntimeReliabilityReadyStateVersion,
            BrowserRuntimeReliabilitySessionVersion = prevReadyState.BrowserRuntimeReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser sessionreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionReliabilitySessionVersion = "runtime-browser-sessionreliability-session-v1";
        result.BrowserSessionReliabilityStages =
        [
            "open-browser-sessionreliability-session",
            "bind-browser-runtimereliability-ready-state",
            "publish-browser-sessionreliability-ready"
        ];
        result.BrowserSessionReliabilitySummary = $"Runtime browser sessionreliability session prepared {result.BrowserSessionReliabilityStages.Length} sessionreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessionreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSessionReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserSessionReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
