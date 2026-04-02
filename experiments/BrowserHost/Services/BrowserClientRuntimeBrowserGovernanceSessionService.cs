namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGovernanceSession
{
    ValueTask<BrowserClientRuntimeBrowserGovernanceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGovernanceSessionService : IBrowserClientRuntimeBrowserGovernanceSession
{
    private readonly IBrowserClientRuntimeBrowserPrivacyReadyState _runtimeBrowserPrivacyReadyState;

    public BrowserClientRuntimeBrowserGovernanceSessionService(IBrowserClientRuntimeBrowserPrivacyReadyState runtimeBrowserPrivacyReadyState)
    {
        _runtimeBrowserPrivacyReadyState = runtimeBrowserPrivacyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGovernanceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPrivacyReadyStateResult privacyReadyState = await _runtimeBrowserPrivacyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGovernanceSessionResult result = new()
        {
            ProfileId = privacyReadyState.ProfileId,
            SessionId = privacyReadyState.SessionId,
            SessionPath = privacyReadyState.SessionPath,
            BrowserPrivacyReadyStateVersion = privacyReadyState.BrowserPrivacyReadyStateVersion,
            BrowserPrivacySessionVersion = privacyReadyState.BrowserPrivacySessionVersion,
            LaunchMode = privacyReadyState.LaunchMode,
            AssetRootPath = privacyReadyState.AssetRootPath,
            ProfilesRootPath = privacyReadyState.ProfilesRootPath,
            CacheRootPath = privacyReadyState.CacheRootPath,
            ConfigRootPath = privacyReadyState.ConfigRootPath,
            SettingsFilePath = privacyReadyState.SettingsFilePath,
            StartupProfilePath = privacyReadyState.StartupProfilePath,
            RequiredAssets = privacyReadyState.RequiredAssets,
            ReadyAssetCount = privacyReadyState.ReadyAssetCount,
            CompletedSteps = privacyReadyState.CompletedSteps,
            TotalSteps = privacyReadyState.TotalSteps,
            Exists = privacyReadyState.Exists,
            ReadSucceeded = privacyReadyState.ReadSucceeded,
            BrowserPrivacyReadyState = privacyReadyState
        };

        if (!privacyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser governance session blocked for profile '{privacyReadyState.ProfileId}'.";
            result.Error = privacyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGovernanceSessionVersion = "runtime-browser-governance-session-v1";
        result.BrowserGovernanceStages =
        [
            "open-browser-governance-session",
            "bind-browser-privacy-ready-state",
            "publish-browser-governance-ready"
        ];
        result.BrowserGovernanceSummary = $"Runtime browser governance session prepared {result.BrowserGovernanceStages.Length} governance stage(s) for profile '{privacyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser governance session ready for profile '{privacyReadyState.ProfileId}' with {result.BrowserGovernanceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGovernanceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserGovernanceStages { get; set; } = Array.Empty<string>();
    public string BrowserGovernanceSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPrivacyReadyStateResult BrowserPrivacyReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
