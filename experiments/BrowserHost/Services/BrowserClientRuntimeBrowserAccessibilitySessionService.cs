namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAccessibilitySession
{
    ValueTask<BrowserClientRuntimeBrowserAccessibilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAccessibilitySessionService : IBrowserClientRuntimeBrowserAccessibilitySession
{
    private readonly IBrowserClientRuntimeBrowserUsabilityReadyState _runtimeBrowserUsabilityReadyState;

    public BrowserClientRuntimeBrowserAccessibilitySessionService(IBrowserClientRuntimeBrowserUsabilityReadyState runtimeBrowserUsabilityReadyState)
    {
        _runtimeBrowserUsabilityReadyState = runtimeBrowserUsabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAccessibilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserUsabilityReadyStateResult usabilityReadyState = await _runtimeBrowserUsabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAccessibilitySessionResult result = new()
        {
            ProfileId = usabilityReadyState.ProfileId,
            SessionId = usabilityReadyState.SessionId,
            SessionPath = usabilityReadyState.SessionPath,
            BrowserUsabilityReadyStateVersion = usabilityReadyState.BrowserUsabilityReadyStateVersion,
            BrowserUsabilitySessionVersion = usabilityReadyState.BrowserUsabilitySessionVersion,
            LaunchMode = usabilityReadyState.LaunchMode,
            AssetRootPath = usabilityReadyState.AssetRootPath,
            ProfilesRootPath = usabilityReadyState.ProfilesRootPath,
            CacheRootPath = usabilityReadyState.CacheRootPath,
            ConfigRootPath = usabilityReadyState.ConfigRootPath,
            SettingsFilePath = usabilityReadyState.SettingsFilePath,
            StartupProfilePath = usabilityReadyState.StartupProfilePath,
            RequiredAssets = usabilityReadyState.RequiredAssets,
            ReadyAssetCount = usabilityReadyState.ReadyAssetCount,
            CompletedSteps = usabilityReadyState.CompletedSteps,
            TotalSteps = usabilityReadyState.TotalSteps,
            Exists = usabilityReadyState.Exists,
            ReadSucceeded = usabilityReadyState.ReadSucceeded
        };

        if (!usabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser accessibility session blocked for profile '{usabilityReadyState.ProfileId}'.";
            result.Error = usabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAccessibilitySessionVersion = "runtime-browser-accessibility-session-v1";
        result.BrowserAccessibilityStages =
        [
            "open-browser-accessibility-session",
            "bind-browser-usability-ready-state",
            "publish-browser-accessibility-ready"
        ];
        result.BrowserAccessibilitySummary = $"Runtime browser accessibility session prepared {result.BrowserAccessibilityStages.Length} accessibility stage(s) for profile '{usabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser accessibility session ready for profile '{usabilityReadyState.ProfileId}' with {result.BrowserAccessibilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAccessibilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAccessibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserUsabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUsabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAccessibilityStages { get; set; } = Array.Empty<string>();
    public string BrowserAccessibilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
