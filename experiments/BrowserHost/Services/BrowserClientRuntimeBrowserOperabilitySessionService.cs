namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserOperabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserOperabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserOperabilitySessionService : IBrowserClientRuntimeBrowserOperabilitySession
{
    private readonly IBrowserClientRuntimeBrowserStewardshipReadyState _runtimeBrowserStewardshipReadyState;

    public BrowserClientRuntimeBrowserOperabilitySessionService(IBrowserClientRuntimeBrowserStewardshipReadyState runtimeBrowserStewardshipReadyState)
    {
        _runtimeBrowserStewardshipReadyState = runtimeBrowserStewardshipReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserOperabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserStewardshipReadyStateResult stewardshipReadyState = await _runtimeBrowserStewardshipReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserOperabilitySessionResult result = new()
        {
            ProfileId = stewardshipReadyState.ProfileId,
            SessionId = stewardshipReadyState.SessionId,
            SessionPath = stewardshipReadyState.SessionPath,
            BrowserStewardshipReadyStateVersion = stewardshipReadyState.BrowserStewardshipReadyStateVersion,
            BrowserStewardshipSessionVersion = stewardshipReadyState.BrowserStewardshipSessionVersion,
            LaunchMode = stewardshipReadyState.LaunchMode,
            AssetRootPath = stewardshipReadyState.AssetRootPath,
            ProfilesRootPath = stewardshipReadyState.ProfilesRootPath,
            CacheRootPath = stewardshipReadyState.CacheRootPath,
            ConfigRootPath = stewardshipReadyState.ConfigRootPath,
            SettingsFilePath = stewardshipReadyState.SettingsFilePath,
            StartupProfilePath = stewardshipReadyState.StartupProfilePath,
            RequiredAssets = stewardshipReadyState.RequiredAssets,
            ReadyAssetCount = stewardshipReadyState.ReadyAssetCount,
            CompletedSteps = stewardshipReadyState.CompletedSteps,
            TotalSteps = stewardshipReadyState.TotalSteps,
            Exists = stewardshipReadyState.Exists,
            ReadSucceeded = stewardshipReadyState.ReadSucceeded
        };

        if (!stewardshipReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser operability session blocked for profile '{stewardshipReadyState.ProfileId}'.";
            result.Error = stewardshipReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserOperabilitySessionVersion = "runtime-browser-operability-session-v1";
        result.BrowserOperabilityStages =
        [
            "open-browser-operability-session",
            "bind-browser-stewardship-ready-state",
            "publish-browser-operability-ready"
        ];
        result.BrowserOperabilitySummary = $"Runtime browser operability session prepared {result.BrowserOperabilityStages.Length} operability stage(s) for profile '{stewardshipReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser operability session ready for profile '{stewardshipReadyState.ProfileId}' with {result.BrowserOperabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserOperabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserOperabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserStewardshipReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStewardshipSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserOperabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserOperabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
