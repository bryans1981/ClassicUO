namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTrustReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTrustReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTrustReadyStateService : IBrowserClientRuntimeBrowserTrustReadyState
{
    private readonly IBrowserClientRuntimeBrowserTrustSession _runtimeBrowserTrustSession;

    public BrowserClientRuntimeBrowserTrustReadyStateService(IBrowserClientRuntimeBrowserTrustSession runtimeBrowserTrustSession)
    {
        _runtimeBrowserTrustSession = runtimeBrowserTrustSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTrustReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTrustSessionResult trustSession = await _runtimeBrowserTrustSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTrustReadyStateResult result = new()
        {
            ProfileId = trustSession.ProfileId,
            SessionId = trustSession.SessionId,
            SessionPath = trustSession.SessionPath,
            BrowserTrustSessionVersion = trustSession.BrowserTrustSessionVersion,
            BrowserGovernanceReadyStateVersion = trustSession.BrowserGovernanceReadyStateVersion,
            BrowserGovernanceSessionVersion = trustSession.BrowserGovernanceSessionVersion,
            LaunchMode = trustSession.LaunchMode,
            AssetRootPath = trustSession.AssetRootPath,
            ProfilesRootPath = trustSession.ProfilesRootPath,
            CacheRootPath = trustSession.CacheRootPath,
            ConfigRootPath = trustSession.ConfigRootPath,
            SettingsFilePath = trustSession.SettingsFilePath,
            StartupProfilePath = trustSession.StartupProfilePath,
            RequiredAssets = trustSession.RequiredAssets,
            ReadyAssetCount = trustSession.ReadyAssetCount,
            CompletedSteps = trustSession.CompletedSteps,
            TotalSteps = trustSession.TotalSteps,
            Exists = trustSession.Exists,
            ReadSucceeded = trustSession.ReadSucceeded,
            BrowserTrustSession = trustSession
        };

        if (!trustSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser trust ready state blocked for profile '{trustSession.ProfileId}'.";
            result.Error = trustSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTrustReadyStateVersion = "runtime-browser-trust-ready-state-v1";
        result.BrowserTrustReadyChecks =
        [
            "browser-governance-ready-state-ready",
            "browser-trust-session-ready",
            "browser-trust-ready"
        ];
        result.BrowserTrustReadySummary = $"Runtime browser trust ready state passed {result.BrowserTrustReadyChecks.Length} trust readiness check(s) for profile '{trustSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser trust ready state ready for profile '{trustSession.ProfileId}' with {result.BrowserTrustReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTrustReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTrustReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustSessionVersion { get; set; } = string.Empty;
    public string BrowserGovernanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGovernanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTrustReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTrustReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserTrustSessionResult BrowserTrustSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
