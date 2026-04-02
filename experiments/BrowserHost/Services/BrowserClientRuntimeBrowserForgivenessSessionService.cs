namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserForgivenessSession
{
    ValueTask<BrowserClientRuntimeBrowserForgivenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserForgivenessSessionService : IBrowserClientRuntimeBrowserForgivenessSession
{
    private readonly IBrowserClientRuntimeBrowserRecoverabilityReadyState _runtimeBrowserRecoverabilityReadyState;

    public BrowserClientRuntimeBrowserForgivenessSessionService(IBrowserClientRuntimeBrowserRecoverabilityReadyState runtimeBrowserRecoverabilityReadyState)
    {
        _runtimeBrowserRecoverabilityReadyState = runtimeBrowserRecoverabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserForgivenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRecoverabilityReadyStateResult recoverabilityReadyState = await _runtimeBrowserRecoverabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserForgivenessSessionResult result = new()
        {
            ProfileId = recoverabilityReadyState.ProfileId,
            SessionId = recoverabilityReadyState.SessionId,
            SessionPath = recoverabilityReadyState.SessionPath,
            BrowserRecoverabilityReadyStateVersion = recoverabilityReadyState.BrowserRecoverabilityReadyStateVersion,
            BrowserRecoverabilitySessionVersion = recoverabilityReadyState.BrowserRecoverabilitySessionVersion,
            LaunchMode = recoverabilityReadyState.LaunchMode,
            AssetRootPath = recoverabilityReadyState.AssetRootPath,
            ProfilesRootPath = recoverabilityReadyState.ProfilesRootPath,
            CacheRootPath = recoverabilityReadyState.CacheRootPath,
            ConfigRootPath = recoverabilityReadyState.ConfigRootPath,
            SettingsFilePath = recoverabilityReadyState.SettingsFilePath,
            StartupProfilePath = recoverabilityReadyState.StartupProfilePath,
            RequiredAssets = recoverabilityReadyState.RequiredAssets,
            ReadyAssetCount = recoverabilityReadyState.ReadyAssetCount,
            CompletedSteps = recoverabilityReadyState.CompletedSteps,
            TotalSteps = recoverabilityReadyState.TotalSteps,
            Exists = recoverabilityReadyState.Exists,
            ReadSucceeded = recoverabilityReadyState.ReadSucceeded
        };

        if (!recoverabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser forgiveness session blocked for profile '{recoverabilityReadyState.ProfileId}'.";
            result.Error = recoverabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserForgivenessSessionVersion = "runtime-browser-forgiveness-session-v1";
        result.BrowserForgivenessStages =
        [
            "open-browser-forgiveness-session",
            "bind-browser-recoverability-ready-state",
            "publish-browser-forgiveness-ready"
        ];
        result.BrowserForgivenessSummary = $"Runtime browser forgiveness session prepared {result.BrowserForgivenessStages.Length} forgiveness stage(s) for profile '{recoverabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser forgiveness session ready for profile '{recoverabilityReadyState.ProfileId}' with {result.BrowserForgivenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserForgivenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserForgivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserRecoverabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRecoverabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserForgivenessStages { get; set; } = Array.Empty<string>();
    public string BrowserForgivenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
