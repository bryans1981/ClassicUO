namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuationReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserContinuationReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuationReliabilitySessionService : IBrowserClientRuntimeBrowserContinuationReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserFlowReliabilityReadyState _runtimeBrowserFlowReliabilityReadyState;

    public BrowserClientRuntimeBrowserContinuationReliabilitySessionService(IBrowserClientRuntimeBrowserFlowReliabilityReadyState runtimeBrowserFlowReliabilityReadyState)
    {
        _runtimeBrowserFlowReliabilityReadyState = runtimeBrowserFlowReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuationReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlowReliabilityReadyStateResult prevReadyState = await _runtimeBrowserFlowReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserContinuationReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserFlowReliabilityReadyStateVersion = prevReadyState.BrowserFlowReliabilityReadyStateVersion,
            BrowserFlowReliabilitySessionVersion = prevReadyState.BrowserFlowReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser continuationreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuationReliabilitySessionVersion = "runtime-browser-continuationreliability-session-v1";
        result.BrowserContinuationReliabilityStages =
        [
            "open-browser-continuationreliability-session",
            "bind-browser-flowreliability-ready-state",
            "publish-browser-continuationreliability-ready"
        ];
        result.BrowserContinuationReliabilitySummary = $"Runtime browser continuationreliability session prepared {result.BrowserContinuationReliabilityStages.Length} continuationreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuationreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserContinuationReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuationReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuationReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserFlowReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlowReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuationReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserContinuationReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
