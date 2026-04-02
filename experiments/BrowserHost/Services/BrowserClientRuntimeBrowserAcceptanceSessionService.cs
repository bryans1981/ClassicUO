namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAcceptanceSession
{
    ValueTask<BrowserClientRuntimeBrowserAcceptanceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAcceptanceSessionService : IBrowserClientRuntimeBrowserAcceptanceSession
{
    private readonly IBrowserClientRuntimeBrowserEndorsementReadyState _runtimeBrowserEndorsementReadyState;

    public BrowserClientRuntimeBrowserAcceptanceSessionService(IBrowserClientRuntimeBrowserEndorsementReadyState runtimeBrowserEndorsementReadyState)
    {
        _runtimeBrowserEndorsementReadyState = runtimeBrowserEndorsementReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAcceptanceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEndorsementReadyStateResult endorsementReadyState = await _runtimeBrowserEndorsementReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAcceptanceSessionResult result = new()
        {
            ProfileId = endorsementReadyState.ProfileId,
            SessionId = endorsementReadyState.SessionId,
            SessionPath = endorsementReadyState.SessionPath,
            BrowserEndorsementReadyStateVersion = endorsementReadyState.BrowserEndorsementReadyStateVersion,
            BrowserEndorsementSessionVersion = endorsementReadyState.BrowserEndorsementSessionVersion,
            LaunchMode = endorsementReadyState.LaunchMode,
            AssetRootPath = endorsementReadyState.AssetRootPath,
            ProfilesRootPath = endorsementReadyState.ProfilesRootPath,
            CacheRootPath = endorsementReadyState.CacheRootPath,
            ConfigRootPath = endorsementReadyState.ConfigRootPath,
            SettingsFilePath = endorsementReadyState.SettingsFilePath,
            StartupProfilePath = endorsementReadyState.StartupProfilePath,
            RequiredAssets = endorsementReadyState.RequiredAssets,
            ReadyAssetCount = endorsementReadyState.ReadyAssetCount,
            CompletedSteps = endorsementReadyState.CompletedSteps,
            TotalSteps = endorsementReadyState.TotalSteps,
            Exists = endorsementReadyState.Exists,
            ReadSucceeded = endorsementReadyState.ReadSucceeded
        };

        if (!endorsementReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser acceptance session blocked for profile '{endorsementReadyState.ProfileId}'.";
            result.Error = endorsementReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAcceptanceSessionVersion = "runtime-browser-acceptance-session-v1";
        result.BrowserAcceptanceStages =
        [
            "open-browser-acceptance-session",
            "bind-browser-endorsement-ready-state",
            "publish-browser-acceptance-ready"
        ];
        result.BrowserAcceptanceSummary = $"Runtime browser acceptance session prepared {result.BrowserAcceptanceStages.Length} acceptance stage(s) for profile '{endorsementReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser acceptance session ready for profile '{endorsementReadyState.ProfileId}' with {result.BrowserAcceptanceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAcceptanceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserAcceptanceStages { get; set; } = Array.Empty<string>();
    public string BrowserAcceptanceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
