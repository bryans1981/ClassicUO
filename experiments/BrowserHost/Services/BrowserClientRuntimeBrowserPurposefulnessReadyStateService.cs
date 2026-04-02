namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPurposefulnessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserPurposefulnessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPurposefulnessReadyStateService : IBrowserClientRuntimeBrowserPurposefulnessReadyState
{
    private readonly IBrowserClientRuntimeBrowserPurposefulnessSession _runtimeBrowserPurposefulnessSession;

    public BrowserClientRuntimeBrowserPurposefulnessReadyStateService(IBrowserClientRuntimeBrowserPurposefulnessSession runtimeBrowserPurposefulnessSession)
    {
        _runtimeBrowserPurposefulnessSession = runtimeBrowserPurposefulnessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPurposefulnessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPurposefulnessSessionResult purposefulnessSession = await _runtimeBrowserPurposefulnessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserPurposefulnessReadyStateResult result = new()
        {
            ProfileId = purposefulnessSession.ProfileId,
            SessionId = purposefulnessSession.SessionId,
            SessionPath = purposefulnessSession.SessionPath,
            BrowserPurposefulnessSessionVersion = purposefulnessSession.BrowserPurposefulnessSessionVersion,
            BrowserConvictionReadyStateVersion = purposefulnessSession.BrowserConvictionReadyStateVersion,
            BrowserConvictionSessionVersion = purposefulnessSession.BrowserConvictionSessionVersion,
            LaunchMode = purposefulnessSession.LaunchMode,
            AssetRootPath = purposefulnessSession.AssetRootPath,
            ProfilesRootPath = purposefulnessSession.ProfilesRootPath,
            CacheRootPath = purposefulnessSession.CacheRootPath,
            ConfigRootPath = purposefulnessSession.ConfigRootPath,
            SettingsFilePath = purposefulnessSession.SettingsFilePath,
            StartupProfilePath = purposefulnessSession.StartupProfilePath,
            RequiredAssets = purposefulnessSession.RequiredAssets,
            ReadyAssetCount = purposefulnessSession.ReadyAssetCount,
            CompletedSteps = purposefulnessSession.CompletedSteps,
            TotalSteps = purposefulnessSession.TotalSteps,
            Exists = purposefulnessSession.Exists,
            ReadSucceeded = purposefulnessSession.ReadSucceeded
        };

        if (!purposefulnessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser purposefulness ready state blocked for profile '{purposefulnessSession.ProfileId}'.";
            result.Error = purposefulnessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPurposefulnessReadyStateVersion = "runtime-browser-purposefulness-ready-state-v1";
        result.BrowserPurposefulnessReadyChecks =
        [
            "browser-conviction-ready-state-ready",
            "browser-purposefulness-session-ready",
            "browser-purposefulness-ready"
        ];
        result.BrowserPurposefulnessReadySummary = $"Runtime browser purposefulness ready state passed {result.BrowserPurposefulnessReadyChecks.Length} purposefulness readiness check(s) for profile '{purposefulnessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser purposefulness ready state ready for profile '{purposefulnessSession.ProfileId}' with {result.BrowserPurposefulnessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPurposefulnessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserPurposefulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPurposefulnessSessionVersion { get; set; } = string.Empty;
    public string BrowserConvictionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConvictionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPurposefulnessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserPurposefulnessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
