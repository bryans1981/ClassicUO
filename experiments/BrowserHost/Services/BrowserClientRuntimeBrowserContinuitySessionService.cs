namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserContinuitySession
{
    ValueTask<BrowserClientRuntimeBrowserContinuitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserContinuitySessionService : IBrowserClientRuntimeBrowserContinuitySession
{
    private readonly IBrowserClientRuntimeBrowserAvailabilityReadyState _runtimeBrowserAvailabilityReadyState;

    public BrowserClientRuntimeBrowserContinuitySessionService(IBrowserClientRuntimeBrowserAvailabilityReadyState runtimeBrowserAvailabilityReadyState)
    {
        _runtimeBrowserAvailabilityReadyState = runtimeBrowserAvailabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserContinuitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAvailabilityReadyStateResult availabilityReadyState = await _runtimeBrowserAvailabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserContinuitySessionResult result = new()
        {
            ProfileId = availabilityReadyState.ProfileId,
            SessionId = availabilityReadyState.SessionId,
            SessionPath = availabilityReadyState.SessionPath,
            BrowserAvailabilityReadyStateVersion = availabilityReadyState.BrowserAvailabilityReadyStateVersion,
            BrowserAvailabilitySessionVersion = availabilityReadyState.BrowserAvailabilitySessionVersion,
            LaunchMode = availabilityReadyState.LaunchMode,
            AssetRootPath = availabilityReadyState.AssetRootPath,
            ProfilesRootPath = availabilityReadyState.ProfilesRootPath,
            CacheRootPath = availabilityReadyState.CacheRootPath,
            ConfigRootPath = availabilityReadyState.ConfigRootPath,
            SettingsFilePath = availabilityReadyState.SettingsFilePath,
            StartupProfilePath = availabilityReadyState.StartupProfilePath,
            RequiredAssets = availabilityReadyState.RequiredAssets,
            ReadyAssetCount = availabilityReadyState.ReadyAssetCount,
            CompletedSteps = availabilityReadyState.CompletedSteps,
            TotalSteps = availabilityReadyState.TotalSteps,
            Exists = availabilityReadyState.Exists,
            ReadSucceeded = availabilityReadyState.ReadSucceeded
        };

        if (!availabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser continuity session blocked for profile '{availabilityReadyState.ProfileId}'.";
            result.Error = availabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserContinuitySessionVersion = "runtime-browser-continuity-session-v1";
        result.BrowserContinuityStages =
        [
            "open-browser-continuity-session",
            "bind-browser-availability-ready-state",
            "publish-browser-continuity-ready"
        ];
        result.BrowserContinuitySummary = $"Runtime browser continuity session prepared {result.BrowserContinuityStages.Length} continuity stage(s) for profile '{availabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser continuity session ready for profile '{availabilityReadyState.ProfileId}' with {result.BrowserContinuityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserContinuitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserAvailabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAvailabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserContinuityStages { get; set; } = Array.Empty<string>();
    public string BrowserContinuitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
