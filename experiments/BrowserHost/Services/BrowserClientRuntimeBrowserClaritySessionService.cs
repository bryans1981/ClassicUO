namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserClaritySession
{
    ValueTask<BrowserClientRuntimeBrowserClaritySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserClaritySessionService : IBrowserClientRuntimeBrowserClaritySession
{
    private readonly IBrowserClientRuntimeBrowserGuidabilityReadyState _runtimeBrowserGuidabilityReadyState;

    public BrowserClientRuntimeBrowserClaritySessionService(IBrowserClientRuntimeBrowserGuidabilityReadyState runtimeBrowserGuidabilityReadyState)
    {
        _runtimeBrowserGuidabilityReadyState = runtimeBrowserGuidabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserClaritySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGuidabilityReadyStateResult guidabilityReadyState = await _runtimeBrowserGuidabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserClaritySessionResult result = new()
        {
            ProfileId = guidabilityReadyState.ProfileId,
            SessionId = guidabilityReadyState.SessionId,
            SessionPath = guidabilityReadyState.SessionPath,
            BrowserGuidabilityReadyStateVersion = guidabilityReadyState.BrowserGuidabilityReadyStateVersion,
            BrowserGuidabilitySessionVersion = guidabilityReadyState.BrowserGuidabilitySessionVersion,
            LaunchMode = guidabilityReadyState.LaunchMode,
            AssetRootPath = guidabilityReadyState.AssetRootPath,
            ProfilesRootPath = guidabilityReadyState.ProfilesRootPath,
            CacheRootPath = guidabilityReadyState.CacheRootPath,
            ConfigRootPath = guidabilityReadyState.ConfigRootPath,
            SettingsFilePath = guidabilityReadyState.SettingsFilePath,
            StartupProfilePath = guidabilityReadyState.StartupProfilePath,
            RequiredAssets = guidabilityReadyState.RequiredAssets,
            ReadyAssetCount = guidabilityReadyState.ReadyAssetCount,
            CompletedSteps = guidabilityReadyState.CompletedSteps,
            TotalSteps = guidabilityReadyState.TotalSteps,
            Exists = guidabilityReadyState.Exists,
            ReadSucceeded = guidabilityReadyState.ReadSucceeded
        };

        if (!guidabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser clarity session blocked for profile '{guidabilityReadyState.ProfileId}'.";
            result.Error = guidabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserClaritySessionVersion = "runtime-browser-clarity-session-v1";
        result.BrowserClarityStages =
        [
            "open-browser-clarity-session",
            "bind-browser-guidability-ready-state",
            "publish-browser-clarity-ready"
        ];
        result.BrowserClaritySummary = $"Runtime browser clarity session prepared {result.BrowserClarityStages.Length} clarity stage(s) for profile '{guidabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser clarity session ready for profile '{guidabilityReadyState.ProfileId}' with {result.BrowserClarityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserClaritySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserClaritySessionVersion { get; set; } = string.Empty;
    public string BrowserGuidabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGuidabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserClarityStages { get; set; } = Array.Empty<string>();
    public string BrowserClaritySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

