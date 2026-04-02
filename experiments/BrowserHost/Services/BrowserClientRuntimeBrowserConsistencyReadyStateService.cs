namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConsistencyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserConsistencyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConsistencyReadyStateService : IBrowserClientRuntimeBrowserConsistencyReadyState
{
    private readonly IBrowserClientRuntimeBrowserConsistencySession _runtimeBrowserConsistencySession;

    public BrowserClientRuntimeBrowserConsistencyReadyStateService(IBrowserClientRuntimeBrowserConsistencySession runtimeBrowserConsistencySession)
    {
        _runtimeBrowserConsistencySession = runtimeBrowserConsistencySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConsistencyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConsistencySessionResult consistencySession = await _runtimeBrowserConsistencySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserConsistencyReadyStateResult result = new()
        {
            ProfileId = consistencySession.ProfileId,
            SessionId = consistencySession.SessionId,
            SessionPath = consistencySession.SessionPath,
            BrowserConsistencySessionVersion = consistencySession.BrowserConsistencySessionVersion,
            BrowserIntuitivenessReadyStateVersion = consistencySession.BrowserIntuitivenessReadyStateVersion,
            BrowserIntuitivenessSessionVersion = consistencySession.BrowserIntuitivenessSessionVersion,
            LaunchMode = consistencySession.LaunchMode,
            AssetRootPath = consistencySession.AssetRootPath,
            ProfilesRootPath = consistencySession.ProfilesRootPath,
            CacheRootPath = consistencySession.CacheRootPath,
            ConfigRootPath = consistencySession.ConfigRootPath,
            SettingsFilePath = consistencySession.SettingsFilePath,
            StartupProfilePath = consistencySession.StartupProfilePath,
            RequiredAssets = consistencySession.RequiredAssets,
            ReadyAssetCount = consistencySession.ReadyAssetCount,
            CompletedSteps = consistencySession.CompletedSteps,
            TotalSteps = consistencySession.TotalSteps,
            Exists = consistencySession.Exists,
            ReadSucceeded = consistencySession.ReadSucceeded
        };

        if (!consistencySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser consistency ready state blocked for profile '{consistencySession.ProfileId}'.";
            result.Error = consistencySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConsistencyReadyStateVersion = "runtime-browser-consistency-ready-state-v1";
        result.BrowserConsistencyReadyChecks =
        [
            "browser-intuitiveness-ready-state-ready",
            "browser-consistency-session-ready",
            "browser-consistency-ready"
        ];
        result.BrowserConsistencyReadySummary = $"Runtime browser consistency ready state passed {result.BrowserConsistencyReadyChecks.Length} consistency readiness check(s) for profile '{consistencySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser consistency ready state ready for profile '{consistencySession.ProfileId}' with {result.BrowserConsistencyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConsistencyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserConsistencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConsistencySessionVersion { get; set; } = string.Empty;
    public string BrowserIntuitivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserIntuitivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConsistencyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserConsistencyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

