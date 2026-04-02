namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserPurposefulnessSession
{
    ValueTask<BrowserClientRuntimeBrowserPurposefulnessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserPurposefulnessSessionService : IBrowserClientRuntimeBrowserPurposefulnessSession
{
    private readonly IBrowserClientRuntimeBrowserConvictionReadyState _runtimeBrowserConvictionReadyState;

    public BrowserClientRuntimeBrowserPurposefulnessSessionService(IBrowserClientRuntimeBrowserConvictionReadyState runtimeBrowserConvictionReadyState)
    {
        _runtimeBrowserConvictionReadyState = runtimeBrowserConvictionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserPurposefulnessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConvictionReadyStateResult convictionReadyState = await _runtimeBrowserConvictionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserPurposefulnessSessionResult result = new()
        {
            ProfileId = convictionReadyState.ProfileId,
            SessionId = convictionReadyState.SessionId,
            SessionPath = convictionReadyState.SessionPath,
            BrowserConvictionReadyStateVersion = convictionReadyState.BrowserConvictionReadyStateVersion,
            BrowserConvictionSessionVersion = convictionReadyState.BrowserConvictionSessionVersion,
            LaunchMode = convictionReadyState.LaunchMode,
            AssetRootPath = convictionReadyState.AssetRootPath,
            ProfilesRootPath = convictionReadyState.ProfilesRootPath,
            CacheRootPath = convictionReadyState.CacheRootPath,
            ConfigRootPath = convictionReadyState.ConfigRootPath,
            SettingsFilePath = convictionReadyState.SettingsFilePath,
            StartupProfilePath = convictionReadyState.StartupProfilePath,
            RequiredAssets = convictionReadyState.RequiredAssets,
            ReadyAssetCount = convictionReadyState.ReadyAssetCount,
            CompletedSteps = convictionReadyState.CompletedSteps,
            TotalSteps = convictionReadyState.TotalSteps,
            Exists = convictionReadyState.Exists,
            ReadSucceeded = convictionReadyState.ReadSucceeded
        };

        if (!convictionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser purposefulness session blocked for profile '{convictionReadyState.ProfileId}'.";
            result.Error = convictionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserPurposefulnessSessionVersion = "runtime-browser-purposefulness-session-v1";
        result.BrowserPurposefulnessStages =
        [
            "open-browser-purposefulness-session",
            "bind-browser-conviction-ready-state",
            "publish-browser-purposefulness-ready"
        ];
        result.BrowserPurposefulnessSummary = $"Runtime browser purposefulness session prepared {result.BrowserPurposefulnessStages.Length} purposefulness stage(s) for profile '{convictionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser purposefulness session ready for profile '{convictionReadyState.ProfileId}' with {result.BrowserPurposefulnessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserPurposefulnessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserPurposefulnessSessionVersion { get; set; } = string.Empty;
    public string BrowserConvictionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConvictionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserPurposefulnessStages { get; set; } = Array.Empty<string>();
    public string BrowserPurposefulnessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
