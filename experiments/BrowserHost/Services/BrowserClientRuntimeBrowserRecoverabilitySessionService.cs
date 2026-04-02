namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRecoverabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserRecoverabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRecoverabilitySessionService : IBrowserClientRuntimeBrowserRecoverabilitySession
{
    private readonly IBrowserClientRuntimeBrowserPersonalizationReadyState _runtimeBrowserPersonalizationReadyState;

    public BrowserClientRuntimeBrowserRecoverabilitySessionService(IBrowserClientRuntimeBrowserPersonalizationReadyState runtimeBrowserPersonalizationReadyState)
    {
        _runtimeBrowserPersonalizationReadyState = runtimeBrowserPersonalizationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRecoverabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPersonalizationReadyStateResult personalizationReadyState = await _runtimeBrowserPersonalizationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRecoverabilitySessionResult result = new()
        {
            ProfileId = personalizationReadyState.ProfileId,
            SessionId = personalizationReadyState.SessionId,
            SessionPath = personalizationReadyState.SessionPath,
            BrowserPersonalizationReadyStateVersion = personalizationReadyState.BrowserPersonalizationReadyStateVersion,
            BrowserPersonalizationSessionVersion = personalizationReadyState.BrowserPersonalizationSessionVersion,
            LaunchMode = personalizationReadyState.LaunchMode,
            AssetRootPath = personalizationReadyState.AssetRootPath,
            ProfilesRootPath = personalizationReadyState.ProfilesRootPath,
            CacheRootPath = personalizationReadyState.CacheRootPath,
            ConfigRootPath = personalizationReadyState.ConfigRootPath,
            SettingsFilePath = personalizationReadyState.SettingsFilePath,
            StartupProfilePath = personalizationReadyState.StartupProfilePath,
            RequiredAssets = personalizationReadyState.RequiredAssets,
            ReadyAssetCount = personalizationReadyState.ReadyAssetCount,
            CompletedSteps = personalizationReadyState.CompletedSteps,
            TotalSteps = personalizationReadyState.TotalSteps,
            Exists = personalizationReadyState.Exists,
            ReadSucceeded = personalizationReadyState.ReadSucceeded
        };

        if (!personalizationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser recoverability session blocked for profile '{personalizationReadyState.ProfileId}'.";
            result.Error = personalizationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRecoverabilitySessionVersion = "runtime-browser-recoverability-session-v1";
        result.BrowserRecoverabilityStages =
        [
            "open-browser-recoverability-session",
            "bind-browser-personalization-ready-state",
            "publish-browser-recoverability-ready"
        ];
        result.BrowserRecoverabilitySummary = $"Runtime browser recoverability session prepared {result.BrowserRecoverabilityStages.Length} recoverability stage(s) for profile '{personalizationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser recoverability session ready for profile '{personalizationReadyState.ProfileId}' with {result.BrowserRecoverabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRecoverabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRecoverabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserPersonalizationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPersonalizationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRecoverabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserRecoverabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
