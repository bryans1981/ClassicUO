namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveContinuitySession
{
    ValueTask<BrowserClientRuntimeBrowserLiveContinuitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveContinuitySessionService : IBrowserClientRuntimeBrowserLiveContinuitySession
{
    private readonly IBrowserClientRuntimeBrowserProductionDurabilityReadyState _runtimeBrowserProductionDurabilityReadyState;

    public BrowserClientRuntimeBrowserLiveContinuitySessionService(IBrowserClientRuntimeBrowserProductionDurabilityReadyState runtimeBrowserProductionDurabilityReadyState)
    {
        _runtimeBrowserProductionDurabilityReadyState = runtimeBrowserProductionDurabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveContinuitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserProductionDurabilityReadyStateResult prevReadyState = await _runtimeBrowserProductionDurabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveContinuitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserProductionDurabilityReadyStateVersion = prevReadyState.BrowserProductionDurabilityReadyStateVersion,
            BrowserProductionDurabilitySessionVersion = prevReadyState.BrowserProductionDurabilitySessionVersion,
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
            result.Summary = $"Runtime browser livecontinuity session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveContinuitySessionVersion = "runtime-browser-livecontinuity-session-v1";
        result.BrowserLiveContinuityStages =
        [
            "open-browser-livecontinuity-session",
            "bind-browser-productiondurability-ready-state",
            "publish-browser-livecontinuity-ready"
        ];
        result.BrowserLiveContinuitySummary = $"Runtime browser livecontinuity session prepared {result.BrowserLiveContinuityStages.Length} livecontinuity stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livecontinuity session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLiveContinuityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveContinuitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserProductionDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserProductionDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveContinuityStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveContinuitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
