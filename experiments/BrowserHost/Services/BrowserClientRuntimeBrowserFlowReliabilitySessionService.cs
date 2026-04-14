namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlowReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserFlowReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlowReliabilitySessionService : IBrowserClientRuntimeBrowserFlowReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserInteractionReliabilityReadyState _runtimeBrowserInteractionReliabilityReadyState;

    public BrowserClientRuntimeBrowserFlowReliabilitySessionService(IBrowserClientRuntimeBrowserInteractionReliabilityReadyState runtimeBrowserInteractionReliabilityReadyState)
    {
        _runtimeBrowserInteractionReliabilityReadyState = runtimeBrowserInteractionReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlowReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionReliabilityReadyStateResult prevReadyState = await _runtimeBrowserInteractionReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFlowReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserInteractionReliabilityReadyStateVersion = prevReadyState.BrowserInteractionReliabilityReadyStateVersion,
            BrowserInteractionReliabilitySessionVersion = prevReadyState.BrowserInteractionReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser flowreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlowReliabilitySessionVersion = "runtime-browser-flowreliability-session-v1";
        result.BrowserFlowReliabilityStages =
        [
            "open-browser-flowreliability-session",
            "bind-browser-interactionreliability-ready-state",
            "publish-browser-flowreliability-ready"
        ];
        result.BrowserFlowReliabilitySummary = $"Runtime browser flowreliability session prepared {result.BrowserFlowReliabilityStages.Length} flowreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flowreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserFlowReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlowReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFlowReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserInteractionReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlowReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserFlowReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
