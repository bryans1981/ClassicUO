namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFluencySession
{
    ValueTask<BrowserClientRuntimeBrowserFluencySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFluencySessionService : IBrowserClientRuntimeBrowserFluencySession
{
    private readonly IBrowserClientRuntimeBrowserFamiliarityReadyState _runtimeBrowserFamiliarityReadyState;

    public BrowserClientRuntimeBrowserFluencySessionService(IBrowserClientRuntimeBrowserFamiliarityReadyState runtimeBrowserFamiliarityReadyState)
    {
        _runtimeBrowserFamiliarityReadyState = runtimeBrowserFamiliarityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFluencySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFamiliarityReadyStateResult familiarityReadyState = await _runtimeBrowserFamiliarityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFluencySessionResult result = new()
        {
            ProfileId = familiarityReadyState.ProfileId,
            SessionId = familiarityReadyState.SessionId,
            SessionPath = familiarityReadyState.SessionPath,
            BrowserFamiliarityReadyStateVersion = familiarityReadyState.BrowserFamiliarityReadyStateVersion,
            BrowserFamiliaritySessionVersion = familiarityReadyState.BrowserFamiliaritySessionVersion,
            LaunchMode = familiarityReadyState.LaunchMode,
            AssetRootPath = familiarityReadyState.AssetRootPath,
            ProfilesRootPath = familiarityReadyState.ProfilesRootPath,
            CacheRootPath = familiarityReadyState.CacheRootPath,
            ConfigRootPath = familiarityReadyState.ConfigRootPath,
            SettingsFilePath = familiarityReadyState.SettingsFilePath,
            StartupProfilePath = familiarityReadyState.StartupProfilePath,
            RequiredAssets = familiarityReadyState.RequiredAssets,
            ReadyAssetCount = familiarityReadyState.ReadyAssetCount,
            CompletedSteps = familiarityReadyState.CompletedSteps,
            TotalSteps = familiarityReadyState.TotalSteps,
            Exists = familiarityReadyState.Exists,
            ReadSucceeded = familiarityReadyState.ReadSucceeded
        };

        if (!familiarityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser fluency session blocked for profile '{familiarityReadyState.ProfileId}'.";
            result.Error = familiarityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFluencySessionVersion = "runtime-browser-fluency-session-v1";
        result.BrowserFluencyStages =
        [
            "open-browser-fluency-session",
            "bind-browser-familiarity-ready-state",
            "publish-browser-fluency-ready"
        ];
        result.BrowserFluencySummary = $"Runtime browser fluency session prepared {result.BrowserFluencyStages.Length} fluency stage(s) for profile '{familiarityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser fluency session ready for profile '{familiarityReadyState.ProfileId}' with {result.BrowserFluencyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFluencySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFluencySessionVersion { get; set; } = string.Empty;
    public string BrowserFamiliarityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFamiliaritySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFluencyStages { get; set; } = Array.Empty<string>();
    public string BrowserFluencySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

