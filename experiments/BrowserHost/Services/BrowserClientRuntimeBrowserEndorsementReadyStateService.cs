namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEndorsementReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEndorsementReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEndorsementReadyStateService : IBrowserClientRuntimeBrowserEndorsementReadyState
{
    private readonly IBrowserClientRuntimeBrowserEndorsementSession _runtimeBrowserEndorsementSession;

    public BrowserClientRuntimeBrowserEndorsementReadyStateService(IBrowserClientRuntimeBrowserEndorsementSession runtimeBrowserEndorsementSession)
    {
        _runtimeBrowserEndorsementSession = runtimeBrowserEndorsementSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEndorsementReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEndorsementSessionResult endorsementSession = await _runtimeBrowserEndorsementSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEndorsementReadyStateResult result = new()
        {
            ProfileId = endorsementSession.ProfileId,
            SessionId = endorsementSession.SessionId,
            SessionPath = endorsementSession.SessionPath,
            BrowserEndorsementSessionVersion = endorsementSession.BrowserEndorsementSessionVersion,
            BrowserRatificationReadyStateVersion = endorsementSession.BrowserRatificationReadyStateVersion,
            BrowserRatificationSessionVersion = endorsementSession.BrowserRatificationSessionVersion,
            LaunchMode = endorsementSession.LaunchMode,
            AssetRootPath = endorsementSession.AssetRootPath,
            ProfilesRootPath = endorsementSession.ProfilesRootPath,
            CacheRootPath = endorsementSession.CacheRootPath,
            ConfigRootPath = endorsementSession.ConfigRootPath,
            SettingsFilePath = endorsementSession.SettingsFilePath,
            StartupProfilePath = endorsementSession.StartupProfilePath,
            RequiredAssets = endorsementSession.RequiredAssets,
            ReadyAssetCount = endorsementSession.ReadyAssetCount,
            CompletedSteps = endorsementSession.CompletedSteps,
            TotalSteps = endorsementSession.TotalSteps,
            Exists = endorsementSession.Exists,
            ReadSucceeded = endorsementSession.ReadSucceeded
        };

        if (!endorsementSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser endorsement ready state blocked for profile '{endorsementSession.ProfileId}'.";
            result.Error = endorsementSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEndorsementReadyStateVersion = "runtime-browser-endorsement-ready-state-v1";
        result.BrowserEndorsementReadyChecks =
        [
            "browser-ratification-ready-state-ready",
            "browser-endorsement-session-ready",
            "browser-endorsement-ready"
        ];
        result.BrowserEndorsementReadySummary = $"Runtime browser endorsement ready state passed {result.BrowserEndorsementReadyChecks.Length} endorsement readiness check(s) for profile '{endorsementSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser endorsement ready state ready for profile '{endorsementSession.ProfileId}' with {result.BrowserEndorsementReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEndorsementReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEndorsementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEndorsementSessionVersion { get; set; } = string.Empty;
    public string BrowserRatificationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRatificationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEndorsementReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEndorsementReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
