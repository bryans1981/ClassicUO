namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserInteractionReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionReliabilitySessionService : IBrowserClientRuntimeBrowserInteractionReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserStateReliabilityReadyState _runtimeBrowserStateReliabilityReadyState;

    public BrowserClientRuntimeBrowserInteractionReliabilitySessionService(IBrowserClientRuntimeBrowserStateReliabilityReadyState runtimeBrowserStateReliabilityReadyState)
    {
        _runtimeBrowserStateReliabilityReadyState = runtimeBrowserStateReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStateReliabilityReadyStateResult prevReadyState = await _runtimeBrowserStateReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInteractionReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserStateReliabilityReadyStateVersion = prevReadyState.BrowserStateReliabilityReadyStateVersion,
            BrowserStateReliabilitySessionVersion = prevReadyState.BrowserStateReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser interactionreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionReliabilitySessionVersion = "runtime-browser-interactionreliability-session-v1";
        result.BrowserInteractionReliabilityStages =
        [
            "open-browser-interactionreliability-session",
            "bind-browser-statereliability-ready-state",
            "publish-browser-interactionreliability-ready"
        ];
        result.BrowserInteractionReliabilitySummary = $"Runtime browser interactionreliability session prepared {result.BrowserInteractionReliabilityStages.Length} interactionreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interactionreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserInteractionReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserStateReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInteractionReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserInteractionReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
