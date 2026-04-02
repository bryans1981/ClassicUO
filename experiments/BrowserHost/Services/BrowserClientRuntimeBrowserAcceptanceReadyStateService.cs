namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAcceptanceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAcceptanceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAcceptanceReadyStateService : IBrowserClientRuntimeBrowserAcceptanceReadyState
{
    private readonly IBrowserClientRuntimeBrowserAcceptanceSession _runtimeBrowserAcceptanceSession;

    public BrowserClientRuntimeBrowserAcceptanceReadyStateService(IBrowserClientRuntimeBrowserAcceptanceSession runtimeBrowserAcceptanceSession)
    {
        _runtimeBrowserAcceptanceSession = runtimeBrowserAcceptanceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAcceptanceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAcceptanceSessionResult acceptanceSession = await _runtimeBrowserAcceptanceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAcceptanceReadyStateResult result = new()
        {
            ProfileId = acceptanceSession.ProfileId,
            SessionId = acceptanceSession.SessionId,
            SessionPath = acceptanceSession.SessionPath,
            BrowserAcceptanceSessionVersion = acceptanceSession.BrowserAcceptanceSessionVersion,
            BrowserEndorsementReadyStateVersion = acceptanceSession.BrowserEndorsementReadyStateVersion,
            BrowserEndorsementSessionVersion = acceptanceSession.BrowserEndorsementSessionVersion,
            LaunchMode = acceptanceSession.LaunchMode,
            AssetRootPath = acceptanceSession.AssetRootPath,
            ProfilesRootPath = acceptanceSession.ProfilesRootPath,
            CacheRootPath = acceptanceSession.CacheRootPath,
            ConfigRootPath = acceptanceSession.ConfigRootPath,
            SettingsFilePath = acceptanceSession.SettingsFilePath,
            StartupProfilePath = acceptanceSession.StartupProfilePath,
            RequiredAssets = acceptanceSession.RequiredAssets,
            ReadyAssetCount = acceptanceSession.ReadyAssetCount,
            CompletedSteps = acceptanceSession.CompletedSteps,
            TotalSteps = acceptanceSession.TotalSteps,
            Exists = acceptanceSession.Exists,
            ReadSucceeded = acceptanceSession.ReadSucceeded
        };

        if (!acceptanceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser acceptance ready state blocked for profile '{acceptanceSession.ProfileId}'.";
            result.Error = acceptanceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAcceptanceReadyStateVersion = "runtime-browser-acceptance-ready-state-v1";
        result.BrowserAcceptanceReadyChecks =
        [
            "browser-endorsement-ready-state-ready",
            "browser-acceptance-session-ready",
            "browser-acceptance-ready"
        ];
        result.BrowserAcceptanceReadySummary = $"Runtime browser acceptance ready state passed {result.BrowserAcceptanceReadyChecks.Length} acceptance readiness check(s) for profile '{acceptanceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser acceptance ready state ready for profile '{acceptanceSession.ProfileId}' with {result.BrowserAcceptanceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAcceptanceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAcceptanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAcceptanceSessionVersion { get; set; } = string.Empty;
    public string BrowserEndorsementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEndorsementSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAcceptanceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAcceptanceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
