namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserMemorabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserMemorabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserMemorabilitySessionService : IBrowserClientRuntimeBrowserMemorabilitySession
{
    private readonly IBrowserClientRuntimeBrowserComprehensibilityReadyState _runtimeBrowserComprehensibilityReadyState;

    public BrowserClientRuntimeBrowserMemorabilitySessionService(IBrowserClientRuntimeBrowserComprehensibilityReadyState runtimeBrowserComprehensibilityReadyState)
    {
        _runtimeBrowserComprehensibilityReadyState = runtimeBrowserComprehensibilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserMemorabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComprehensibilityReadyStateResult comprehensibilityReadyState = await _runtimeBrowserComprehensibilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserMemorabilitySessionResult result = new()
        {
            ProfileId = comprehensibilityReadyState.ProfileId,
            SessionId = comprehensibilityReadyState.SessionId,
            SessionPath = comprehensibilityReadyState.SessionPath,
            BrowserComprehensibilityReadyStateVersion = comprehensibilityReadyState.BrowserComprehensibilityReadyStateVersion,
            BrowserComprehensibilitySessionVersion = comprehensibilityReadyState.BrowserComprehensibilitySessionVersion,
            LaunchMode = comprehensibilityReadyState.LaunchMode,
            AssetRootPath = comprehensibilityReadyState.AssetRootPath,
            ProfilesRootPath = comprehensibilityReadyState.ProfilesRootPath,
            CacheRootPath = comprehensibilityReadyState.CacheRootPath,
            ConfigRootPath = comprehensibilityReadyState.ConfigRootPath,
            SettingsFilePath = comprehensibilityReadyState.SettingsFilePath,
            StartupProfilePath = comprehensibilityReadyState.StartupProfilePath,
            RequiredAssets = comprehensibilityReadyState.RequiredAssets,
            ReadyAssetCount = comprehensibilityReadyState.ReadyAssetCount,
            CompletedSteps = comprehensibilityReadyState.CompletedSteps,
            TotalSteps = comprehensibilityReadyState.TotalSteps,
            Exists = comprehensibilityReadyState.Exists,
            ReadSucceeded = comprehensibilityReadyState.ReadSucceeded
        };

        if (!comprehensibilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser memorability session blocked for profile '{comprehensibilityReadyState.ProfileId}'.";
            result.Error = comprehensibilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserMemorabilitySessionVersion = "runtime-browser-memorability-session-v1";
        result.BrowserMemorabilityStages =
        [
            "open-browser-memorability-session",
            "bind-browser-comprehensibility-ready-state",
            "publish-browser-memorability-ready"
        ];
        result.BrowserMemorabilitySummary = $"Runtime browser memorability session prepared {result.BrowserMemorabilityStages.Length} memorability stage(s) for profile '{comprehensibilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser memorability session ready for profile '{comprehensibilityReadyState.ProfileId}' with {result.BrowserMemorabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserMemorabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserMemorabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserComprehensibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComprehensibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserMemorabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserMemorabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
