namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserNavigationalConfidenceSession
{
    ValueTask<BrowserClientRuntimeBrowserNavigationalConfidenceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserNavigationalConfidenceSessionService : IBrowserClientRuntimeBrowserNavigationalConfidenceSession
{
    private readonly IBrowserClientRuntimeBrowserPerceivabilityReadyState _runtimeBrowserPerceivabilityReadyState;

    public BrowserClientRuntimeBrowserNavigationalConfidenceSessionService(IBrowserClientRuntimeBrowserPerceivabilityReadyState runtimeBrowserPerceivabilityReadyState)
    {
        _runtimeBrowserPerceivabilityReadyState = runtimeBrowserPerceivabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserNavigationalConfidenceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPerceivabilityReadyStateResult perceivabilityReadyState = await _runtimeBrowserPerceivabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserNavigationalConfidenceSessionResult result = new()
        {
            ProfileId = perceivabilityReadyState.ProfileId,
            SessionId = perceivabilityReadyState.SessionId,
            SessionPath = perceivabilityReadyState.SessionPath,
            BrowserPerceivabilityReadyStateVersion = perceivabilityReadyState.BrowserPerceivabilityReadyStateVersion,
            BrowserPerceivabilitySessionVersion = perceivabilityReadyState.BrowserPerceivabilitySessionVersion,
            LaunchMode = perceivabilityReadyState.LaunchMode,
            AssetRootPath = perceivabilityReadyState.AssetRootPath,
            ProfilesRootPath = perceivabilityReadyState.ProfilesRootPath,
            CacheRootPath = perceivabilityReadyState.CacheRootPath,
            ConfigRootPath = perceivabilityReadyState.ConfigRootPath,
            SettingsFilePath = perceivabilityReadyState.SettingsFilePath,
            StartupProfilePath = perceivabilityReadyState.StartupProfilePath,
            RequiredAssets = perceivabilityReadyState.RequiredAssets,
            ReadyAssetCount = perceivabilityReadyState.ReadyAssetCount,
            CompletedSteps = perceivabilityReadyState.CompletedSteps,
            TotalSteps = perceivabilityReadyState.TotalSteps,
            Exists = perceivabilityReadyState.Exists,
            ReadSucceeded = perceivabilityReadyState.ReadSucceeded
        };

        if (!perceivabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser navigationalconfidence session blocked for profile '{perceivabilityReadyState.ProfileId}'.";
            result.Error = perceivabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserNavigationalConfidenceSessionVersion = "runtime-browser-navigationalconfidence-session-v1";
        result.BrowserNavigationalConfidenceStages =
        [
            "open-browser-navigationalconfidence-session",
            "bind-browser-perceivability-ready-state",
            "publish-browser-navigationalconfidence-ready"
        ];
        result.BrowserNavigationalConfidenceSummary = $"Runtime browser navigationalconfidence session prepared {result.BrowserNavigationalConfidenceStages.Length} navigationalconfidence stage(s) for profile '{perceivabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser navigationalconfidence session ready for profile '{perceivabilityReadyState.ProfileId}' with {result.BrowserNavigationalConfidenceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserNavigationalConfidenceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserNavigationalConfidenceSessionVersion { get; set; } = string.Empty;
    public string BrowserPerceivabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPerceivabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserNavigationalConfidenceStages { get; set; } = Array.Empty<string>();
    public string BrowserNavigationalConfidenceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
