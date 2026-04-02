namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSatisfactionSession
{
    ValueTask<BrowserClientRuntimeBrowserSatisfactionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSatisfactionSessionService : IBrowserClientRuntimeBrowserSatisfactionSession
{
    private readonly IBrowserClientRuntimeBrowserForgivenessReadyState _runtimeBrowserForgivenessReadyState;

    public BrowserClientRuntimeBrowserSatisfactionSessionService(IBrowserClientRuntimeBrowserForgivenessReadyState runtimeBrowserForgivenessReadyState)
    {
        _runtimeBrowserForgivenessReadyState = runtimeBrowserForgivenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSatisfactionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserForgivenessReadyStateResult forgivenessReadyState = await _runtimeBrowserForgivenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSatisfactionSessionResult result = new()
        {
            ProfileId = forgivenessReadyState.ProfileId,
            SessionId = forgivenessReadyState.SessionId,
            SessionPath = forgivenessReadyState.SessionPath,
            BrowserForgivenessReadyStateVersion = forgivenessReadyState.BrowserForgivenessReadyStateVersion,
            BrowserForgivenessSessionVersion = forgivenessReadyState.BrowserForgivenessSessionVersion,
            LaunchMode = forgivenessReadyState.LaunchMode,
            AssetRootPath = forgivenessReadyState.AssetRootPath,
            ProfilesRootPath = forgivenessReadyState.ProfilesRootPath,
            CacheRootPath = forgivenessReadyState.CacheRootPath,
            ConfigRootPath = forgivenessReadyState.ConfigRootPath,
            SettingsFilePath = forgivenessReadyState.SettingsFilePath,
            StartupProfilePath = forgivenessReadyState.StartupProfilePath,
            RequiredAssets = forgivenessReadyState.RequiredAssets,
            ReadyAssetCount = forgivenessReadyState.ReadyAssetCount,
            CompletedSteps = forgivenessReadyState.CompletedSteps,
            TotalSteps = forgivenessReadyState.TotalSteps,
            Exists = forgivenessReadyState.Exists,
            ReadSucceeded = forgivenessReadyState.ReadSucceeded
        };

        if (!forgivenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser satisfaction session blocked for profile '{forgivenessReadyState.ProfileId}'.";
            result.Error = forgivenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSatisfactionSessionVersion = "runtime-browser-satisfaction-session-v1";
        result.BrowserSatisfactionStages =
        [
            "open-browser-satisfaction-session",
            "bind-browser-forgiveness-ready-state",
            "publish-browser-satisfaction-ready"
        ];
        result.BrowserSatisfactionSummary = $"Runtime browser satisfaction session prepared {result.BrowserSatisfactionStages.Length} satisfaction stage(s) for profile '{forgivenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser satisfaction session ready for profile '{forgivenessReadyState.ProfileId}' with {result.BrowserSatisfactionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSatisfactionSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSatisfactionSessionVersion { get; set; } = string.Empty;
    public string BrowserForgivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserForgivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSatisfactionStages { get; set; } = Array.Empty<string>();
    public string BrowserSatisfactionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
