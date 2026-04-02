namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTrustSession
{
    ValueTask<BrowserClientRuntimeBrowserTrustSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTrustSessionService : IBrowserClientRuntimeBrowserTrustSession
{
    private readonly IBrowserClientRuntimeBrowserGovernanceReadyState _runtimeBrowserGovernanceReadyState;

    public BrowserClientRuntimeBrowserTrustSessionService(IBrowserClientRuntimeBrowserGovernanceReadyState runtimeBrowserGovernanceReadyState)
    {
        _runtimeBrowserGovernanceReadyState = runtimeBrowserGovernanceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTrustSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserGovernanceReadyStateResult governanceReadyState = await _runtimeBrowserGovernanceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTrustSessionResult result = new()
        {
            ProfileId = governanceReadyState.ProfileId,
            SessionId = governanceReadyState.SessionId,
            SessionPath = governanceReadyState.SessionPath,
            BrowserGovernanceReadyStateVersion = governanceReadyState.BrowserGovernanceReadyStateVersion,
            BrowserGovernanceSessionVersion = governanceReadyState.BrowserGovernanceSessionVersion,
            LaunchMode = governanceReadyState.LaunchMode,
            AssetRootPath = governanceReadyState.AssetRootPath,
            ProfilesRootPath = governanceReadyState.ProfilesRootPath,
            CacheRootPath = governanceReadyState.CacheRootPath,
            ConfigRootPath = governanceReadyState.ConfigRootPath,
            SettingsFilePath = governanceReadyState.SettingsFilePath,
            StartupProfilePath = governanceReadyState.StartupProfilePath,
            RequiredAssets = governanceReadyState.RequiredAssets,
            ReadyAssetCount = governanceReadyState.ReadyAssetCount,
            CompletedSteps = governanceReadyState.CompletedSteps,
            TotalSteps = governanceReadyState.TotalSteps,
            Exists = governanceReadyState.Exists,
            ReadSucceeded = governanceReadyState.ReadSucceeded,
            BrowserGovernanceReadyState = governanceReadyState
        };

        if (!governanceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser trust session blocked for profile '{governanceReadyState.ProfileId}'.";
            result.Error = governanceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTrustSessionVersion = "runtime-browser-trust-session-v1";
        result.BrowserTrustStages =
        [
            "open-browser-trust-session",
            "bind-browser-governance-ready-state",
            "publish-browser-trust-ready"
        ];
        result.BrowserTrustSummary = $"Runtime browser trust session prepared {result.BrowserTrustStages.Length} trust stage(s) for profile '{governanceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser trust session ready for profile '{governanceReadyState.ProfileId}' with {result.BrowserTrustStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTrustSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserTrustStages { get; set; } = Array.Empty<string>();
    public string BrowserTrustSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserGovernanceReadyStateResult BrowserGovernanceReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
