namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserStateReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserStateReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserStateReliabilitySessionService : IBrowserClientRuntimeBrowserStateReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSessionReliabilityReadyState _runtimeBrowserSessionReliabilityReadyState;

    public BrowserClientRuntimeBrowserStateReliabilitySessionService(IBrowserClientRuntimeBrowserSessionReliabilityReadyState runtimeBrowserSessionReliabilityReadyState)
    {
        _runtimeBrowserSessionReliabilityReadyState = runtimeBrowserSessionReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserStateReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionReliabilityReadyStateResult prevReadyState = await _runtimeBrowserSessionReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserStateReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSessionReliabilityReadyStateVersion = prevReadyState.BrowserSessionReliabilityReadyStateVersion,
            BrowserSessionReliabilitySessionVersion = prevReadyState.BrowserSessionReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser statereliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserStateReliabilitySessionVersion = "runtime-browser-statereliability-session-v1";
        result.BrowserStateReliabilityStages =
        [
            "open-browser-statereliability-session",
            "bind-browser-sessionreliability-ready-state",
            "publish-browser-statereliability-ready"
        ];
        result.BrowserStateReliabilitySummary = $"Runtime browser statereliability session prepared {result.BrowserStateReliabilityStages.Length} statereliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser statereliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserStateReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserStateReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserStateReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSessionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserStateReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserStateReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
