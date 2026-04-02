namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGovernanceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserGovernanceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGovernanceReadyStateService : IBrowserClientRuntimeBrowserGovernanceReadyState
{
    private readonly IBrowserClientRuntimeBrowserGovernanceSession _runtimeBrowserGovernanceSession;

    public BrowserClientRuntimeBrowserGovernanceReadyStateService(IBrowserClientRuntimeBrowserGovernanceSession runtimeBrowserGovernanceSession)
    {
        _runtimeBrowserGovernanceSession = runtimeBrowserGovernanceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGovernanceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGovernanceSessionResult governanceSession = await _runtimeBrowserGovernanceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserGovernanceReadyStateResult result = new()
        {
            ProfileId = governanceSession.ProfileId,
            SessionId = governanceSession.SessionId,
            SessionPath = governanceSession.SessionPath,
            BrowserGovernanceSessionVersion = governanceSession.BrowserGovernanceSessionVersion,
            BrowserPrivacyReadyStateVersion = governanceSession.BrowserPrivacyReadyStateVersion,
            BrowserPrivacySessionVersion = governanceSession.BrowserPrivacySessionVersion,
            LaunchMode = governanceSession.LaunchMode,
            AssetRootPath = governanceSession.AssetRootPath,
            ProfilesRootPath = governanceSession.ProfilesRootPath,
            CacheRootPath = governanceSession.CacheRootPath,
            ConfigRootPath = governanceSession.ConfigRootPath,
            SettingsFilePath = governanceSession.SettingsFilePath,
            StartupProfilePath = governanceSession.StartupProfilePath,
            RequiredAssets = governanceSession.RequiredAssets,
            ReadyAssetCount = governanceSession.ReadyAssetCount,
            CompletedSteps = governanceSession.CompletedSteps,
            TotalSteps = governanceSession.TotalSteps,
            Exists = governanceSession.Exists,
            ReadSucceeded = governanceSession.ReadSucceeded,
            BrowserGovernanceSession = governanceSession
        };

        if (!governanceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser governance ready state blocked for profile '{governanceSession.ProfileId}'.";
            result.Error = governanceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGovernanceReadyStateVersion = "runtime-browser-governance-ready-state-v1";
        result.BrowserGovernanceReadyChecks =
        [
            "browser-privacy-ready-state-ready",
            "browser-governance-session-ready",
            "browser-governance-ready"
        ];
        result.BrowserGovernanceReadySummary = $"Runtime browser governance ready state passed {result.BrowserGovernanceReadyChecks.Length} governance readiness check(s) for profile '{governanceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser governance ready state ready for profile '{governanceSession.ProfileId}' with {result.BrowserGovernanceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGovernanceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserGovernanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGovernanceSessionVersion { get; set; } = string.Empty;
    public string BrowserPrivacyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPrivacySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGovernanceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserGovernanceReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserGovernanceSessionResult BrowserGovernanceSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
