namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPrivacySession
{
    ValueTask<BrowserClientRuntimeBrowserPrivacySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPrivacySessionService : IBrowserClientRuntimeBrowserPrivacySession
{
    private readonly IBrowserClientRuntimeBrowserComplianceReadyState _runtimeBrowserComplianceReadyState;

    public BrowserClientRuntimeBrowserPrivacySessionService(IBrowserClientRuntimeBrowserComplianceReadyState runtimeBrowserComplianceReadyState)
    {
        _runtimeBrowserComplianceReadyState = runtimeBrowserComplianceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPrivacySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserComplianceReadyStateResult complianceReadyState = await _runtimeBrowserComplianceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPrivacySessionResult result = new()
        {
            ProfileId = complianceReadyState.ProfileId,
            SessionId = complianceReadyState.SessionId,
            SessionPath = complianceReadyState.SessionPath,
            BrowserComplianceReadyStateVersion = complianceReadyState.BrowserComplianceReadyStateVersion,
            BrowserComplianceSessionVersion = complianceReadyState.BrowserComplianceSessionVersion,
            LaunchMode = complianceReadyState.LaunchMode,
            AssetRootPath = complianceReadyState.AssetRootPath,
            ProfilesRootPath = complianceReadyState.ProfilesRootPath,
            CacheRootPath = complianceReadyState.CacheRootPath,
            ConfigRootPath = complianceReadyState.ConfigRootPath,
            SettingsFilePath = complianceReadyState.SettingsFilePath,
            StartupProfilePath = complianceReadyState.StartupProfilePath,
            RequiredAssets = complianceReadyState.RequiredAssets,
            ReadyAssetCount = complianceReadyState.ReadyAssetCount,
            CompletedSteps = complianceReadyState.CompletedSteps,
            TotalSteps = complianceReadyState.TotalSteps,
            Exists = complianceReadyState.Exists,
            ReadSucceeded = complianceReadyState.ReadSucceeded,
            BrowserComplianceReadyState = complianceReadyState
        };

        if (!complianceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser privacy session blocked for profile '{complianceReadyState.ProfileId}'.";
            result.Error = complianceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPrivacySessionVersion = "runtime-browser-privacy-session-v1";
        result.BrowserPrivacyStages =
        [
            "open-browser-privacy-session",
            "bind-browser-compliance-ready-state",
            "publish-browser-privacy-ready"
        ];
        result.BrowserPrivacySummary = $"Runtime browser privacy session prepared {result.BrowserPrivacyStages.Length} privacy stage(s) for profile '{complianceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser privacy session ready for profile '{complianceReadyState.ProfileId}' with {result.BrowserPrivacyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPrivacySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPrivacySessionVersion { get; set; } = string.Empty;
    public string BrowserComplianceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserComplianceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPrivacyStages { get; set; } = Array.Empty<string>();
    public string BrowserPrivacySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserComplianceReadyStateResult BrowserComplianceReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
