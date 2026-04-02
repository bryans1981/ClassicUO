namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEndorsementSession
{
    ValueTask<BrowserClientRuntimeBrowserEndorsementSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEndorsementSessionService : IBrowserClientRuntimeBrowserEndorsementSession
{
    private readonly IBrowserClientRuntimeBrowserRatificationReadyState _runtimeBrowserRatificationReadyState;

    public BrowserClientRuntimeBrowserEndorsementSessionService(IBrowserClientRuntimeBrowserRatificationReadyState runtimeBrowserRatificationReadyState)
    {
        _runtimeBrowserRatificationReadyState = runtimeBrowserRatificationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEndorsementSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRatificationReadyStateResult ratificationReadyState = await _runtimeBrowserRatificationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEndorsementSessionResult result = new()
        {
            ProfileId = ratificationReadyState.ProfileId,
            SessionId = ratificationReadyState.SessionId,
            SessionPath = ratificationReadyState.SessionPath,
            BrowserRatificationReadyStateVersion = ratificationReadyState.BrowserRatificationReadyStateVersion,
            BrowserRatificationSessionVersion = ratificationReadyState.BrowserRatificationSessionVersion,
            LaunchMode = ratificationReadyState.LaunchMode,
            AssetRootPath = ratificationReadyState.AssetRootPath,
            ProfilesRootPath = ratificationReadyState.ProfilesRootPath,
            CacheRootPath = ratificationReadyState.CacheRootPath,
            ConfigRootPath = ratificationReadyState.ConfigRootPath,
            SettingsFilePath = ratificationReadyState.SettingsFilePath,
            StartupProfilePath = ratificationReadyState.StartupProfilePath,
            RequiredAssets = ratificationReadyState.RequiredAssets,
            ReadyAssetCount = ratificationReadyState.ReadyAssetCount,
            CompletedSteps = ratificationReadyState.CompletedSteps,
            TotalSteps = ratificationReadyState.TotalSteps,
            Exists = ratificationReadyState.Exists,
            ReadSucceeded = ratificationReadyState.ReadSucceeded
        };

        if (!ratificationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser endorsement session blocked for profile '{ratificationReadyState.ProfileId}'.";
            result.Error = ratificationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEndorsementSessionVersion = "runtime-browser-endorsement-session-v1";
        result.BrowserEndorsementStages =
        [
            "open-browser-endorsement-session",
            "bind-browser-ratification-ready-state",
            "publish-browser-endorsement-ready"
        ];
        result.BrowserEndorsementSummary = $"Runtime browser endorsement session prepared {result.BrowserEndorsementStages.Length} endorsement stage(s) for profile '{ratificationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser endorsement session ready for profile '{ratificationReadyState.ProfileId}' with {result.BrowserEndorsementStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEndorsementSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserEndorsementStages { get; set; } = Array.Empty<string>();
    public string BrowserEndorsementSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
