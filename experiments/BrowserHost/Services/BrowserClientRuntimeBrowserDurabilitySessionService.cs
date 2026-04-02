namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDurabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserDurabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDurabilitySessionService : IBrowserClientRuntimeBrowserDurabilitySession
{
    private readonly IBrowserClientRuntimeBrowserContinuityReadyState _runtimeBrowserContinuityReadyState;

    public BrowserClientRuntimeBrowserDurabilitySessionService(IBrowserClientRuntimeBrowserContinuityReadyState runtimeBrowserContinuityReadyState)
    {
        _runtimeBrowserContinuityReadyState = runtimeBrowserContinuityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDurabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserContinuityReadyStateResult continuityReadyState = await _runtimeBrowserContinuityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDurabilitySessionResult result = new()
        {
            ProfileId = continuityReadyState.ProfileId,
            SessionId = continuityReadyState.SessionId,
            SessionPath = continuityReadyState.SessionPath,
            BrowserContinuityReadyStateVersion = continuityReadyState.BrowserContinuityReadyStateVersion,
            BrowserContinuitySessionVersion = continuityReadyState.BrowserContinuitySessionVersion,
            LaunchMode = continuityReadyState.LaunchMode,
            AssetRootPath = continuityReadyState.AssetRootPath,
            ProfilesRootPath = continuityReadyState.ProfilesRootPath,
            CacheRootPath = continuityReadyState.CacheRootPath,
            ConfigRootPath = continuityReadyState.ConfigRootPath,
            SettingsFilePath = continuityReadyState.SettingsFilePath,
            StartupProfilePath = continuityReadyState.StartupProfilePath,
            RequiredAssets = continuityReadyState.RequiredAssets,
            ReadyAssetCount = continuityReadyState.ReadyAssetCount,
            CompletedSteps = continuityReadyState.CompletedSteps,
            TotalSteps = continuityReadyState.TotalSteps,
            Exists = continuityReadyState.Exists,
            ReadSucceeded = continuityReadyState.ReadSucceeded
        };

        if (!continuityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser durability session blocked for profile '{continuityReadyState.ProfileId}'.";
            result.Error = continuityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDurabilitySessionVersion = "runtime-browser-durability-session-v1";
        result.BrowserDurabilityStages =
        [
            "open-browser-durability-session",
            "bind-browser-continuity-ready-state",
            "publish-browser-durability-ready"
        ];
        result.BrowserDurabilitySummary = $"Runtime browser durability session prepared {result.BrowserDurabilityStages.Length} durability stage(s) for profile '{continuityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser durability session ready for profile '{continuityReadyState.ProfileId}' with {result.BrowserDurabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDurabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDurabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserDurabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
