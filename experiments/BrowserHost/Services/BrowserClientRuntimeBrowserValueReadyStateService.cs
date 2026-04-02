namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserValueReadyState
{
    ValueTask<BrowserClientRuntimeBrowserValueReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserValueReadyStateService : IBrowserClientRuntimeBrowserValueReadyState
{
    private readonly IBrowserClientRuntimeBrowserValueSession _runtimeBrowserValueSession;

    public BrowserClientRuntimeBrowserValueReadyStateService(IBrowserClientRuntimeBrowserValueSession runtimeBrowserValueSession)
    {
        _runtimeBrowserValueSession = runtimeBrowserValueSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserValueReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserValueSessionResult valueSession = await _runtimeBrowserValueSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserValueReadyStateResult result = new()
        {
            ProfileId = valueSession.ProfileId,
            SessionId = valueSession.SessionId,
            SessionPath = valueSession.SessionPath,
            BrowserValueSessionVersion = valueSession.BrowserValueSessionVersion,
            BrowserUsefulnessReadyStateVersion = valueSession.BrowserUsefulnessReadyStateVersion,
            BrowserUsefulnessSessionVersion = valueSession.BrowserUsefulnessSessionVersion,
            LaunchMode = valueSession.LaunchMode,
            AssetRootPath = valueSession.AssetRootPath,
            ProfilesRootPath = valueSession.ProfilesRootPath,
            CacheRootPath = valueSession.CacheRootPath,
            ConfigRootPath = valueSession.ConfigRootPath,
            SettingsFilePath = valueSession.SettingsFilePath,
            StartupProfilePath = valueSession.StartupProfilePath,
            RequiredAssets = valueSession.RequiredAssets,
            ReadyAssetCount = valueSession.ReadyAssetCount,
            CompletedSteps = valueSession.CompletedSteps,
            TotalSteps = valueSession.TotalSteps,
            Exists = valueSession.Exists,
            ReadSucceeded = valueSession.ReadSucceeded
        };

        if (!valueSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser value ready state blocked for profile '{valueSession.ProfileId}'.";
            result.Error = valueSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserValueReadyStateVersion = "runtime-browser-value-ready-state-v1";
        result.BrowserValueReadyChecks =
        [
            "browser-usefulness-ready-state-ready",
            "browser-value-session-ready",
            "browser-value-ready"
        ];
        result.BrowserValueReadySummary = $"Runtime browser value ready state passed {result.BrowserValueReadyChecks.Length} value readiness check(s) for profile '{valueSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser value ready state ready for profile '{valueSession.ProfileId}' with {result.BrowserValueReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserValueReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserValueReadyStateVersion { get; set; } = string.Empty;
    public string BrowserValueSessionVersion { get; set; } = string.Empty;
    public string BrowserUsefulnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUsefulnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserValueReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserValueReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
