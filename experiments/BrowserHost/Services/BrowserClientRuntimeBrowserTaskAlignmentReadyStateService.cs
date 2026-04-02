namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTaskAlignmentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTaskAlignmentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTaskAlignmentReadyStateService : IBrowserClientRuntimeBrowserTaskAlignmentReadyState
{
    private readonly IBrowserClientRuntimeBrowserTaskAlignmentSession _runtimeBrowserTaskAlignmentSession;

    public BrowserClientRuntimeBrowserTaskAlignmentReadyStateService(IBrowserClientRuntimeBrowserTaskAlignmentSession runtimeBrowserTaskAlignmentSession)
    {
        _runtimeBrowserTaskAlignmentSession = runtimeBrowserTaskAlignmentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTaskAlignmentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTaskAlignmentSessionResult taskalignmentSession = await _runtimeBrowserTaskAlignmentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTaskAlignmentReadyStateResult result = new()
        {
            ProfileId = taskalignmentSession.ProfileId,
            SessionId = taskalignmentSession.SessionId,
            SessionPath = taskalignmentSession.SessionPath,
            BrowserTaskAlignmentSessionVersion = taskalignmentSession.BrowserTaskAlignmentSessionVersion,
            BrowserNavigationalConfidenceReadyStateVersion = taskalignmentSession.BrowserNavigationalConfidenceReadyStateVersion,
            BrowserNavigationalConfidenceSessionVersion = taskalignmentSession.BrowserNavigationalConfidenceSessionVersion,
            LaunchMode = taskalignmentSession.LaunchMode,
            AssetRootPath = taskalignmentSession.AssetRootPath,
            ProfilesRootPath = taskalignmentSession.ProfilesRootPath,
            CacheRootPath = taskalignmentSession.CacheRootPath,
            ConfigRootPath = taskalignmentSession.ConfigRootPath,
            SettingsFilePath = taskalignmentSession.SettingsFilePath,
            StartupProfilePath = taskalignmentSession.StartupProfilePath,
            RequiredAssets = taskalignmentSession.RequiredAssets,
            ReadyAssetCount = taskalignmentSession.ReadyAssetCount,
            CompletedSteps = taskalignmentSession.CompletedSteps,
            TotalSteps = taskalignmentSession.TotalSteps,
            Exists = taskalignmentSession.Exists,
            ReadSucceeded = taskalignmentSession.ReadSucceeded
        };

        if (!taskalignmentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser taskalignment ready state blocked for profile '{taskalignmentSession.ProfileId}'.";
            result.Error = taskalignmentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTaskAlignmentReadyStateVersion = "runtime-browser-taskalignment-ready-state-v1";
        result.BrowserTaskAlignmentReadyChecks =
        [
            "browser-navigationalconfidence-ready-state-ready",
            "browser-taskalignment-session-ready",
            "browser-taskalignment-ready"
        ];
        result.BrowserTaskAlignmentReadySummary = $"Runtime browser taskalignment ready state passed {result.BrowserTaskAlignmentReadyChecks.Length} taskalignment readiness check(s) for profile '{taskalignmentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser taskalignment ready state ready for profile '{taskalignmentSession.ProfileId}' with {result.BrowserTaskAlignmentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTaskAlignmentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTaskAlignmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTaskAlignmentSessionVersion { get; set; } = string.Empty;
    public string BrowserNavigationalConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserNavigationalConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTaskAlignmentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTaskAlignmentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
