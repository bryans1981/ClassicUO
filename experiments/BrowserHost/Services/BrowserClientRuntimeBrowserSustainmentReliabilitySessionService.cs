namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentReliabilitySessionService : IBrowserClientRuntimeBrowserSustainmentReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserContinuationReliabilityReadyState _runtimeBrowserContinuationReliabilityReadyState;

    public BrowserClientRuntimeBrowserSustainmentReliabilitySessionService(IBrowserClientRuntimeBrowserContinuationReliabilityReadyState runtimeBrowserContinuationReliabilityReadyState)
    {
        _runtimeBrowserContinuationReliabilityReadyState = runtimeBrowserContinuationReliabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuationReliabilityReadyStateResult prevReadyState = await _runtimeBrowserContinuationReliabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentReliabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserContinuationReliabilityReadyStateVersion = prevReadyState.BrowserContinuationReliabilityReadyStateVersion,
            BrowserContinuationReliabilitySessionVersion = prevReadyState.BrowserContinuationReliabilitySessionVersion,
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
            result.Summary = $"Runtime browser sustainmentreliability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentReliabilitySessionVersion = "runtime-browser-sustainmentreliability-session-v1";
        result.BrowserSustainmentReliabilityStages =
        [
            "open-browser-sustainmentreliability-session",
            "bind-browser-continuationreliability-ready-state",
            "publish-browser-sustainmentreliability-ready"
        ];
        result.BrowserSustainmentReliabilitySummary = $"Runtime browser sustainmentreliability session prepared {result.BrowserSustainmentReliabilityStages.Length} sustainmentreliability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentreliability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserSustainmentReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentReliabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainmentReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserContinuationReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationReliabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainmentReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
