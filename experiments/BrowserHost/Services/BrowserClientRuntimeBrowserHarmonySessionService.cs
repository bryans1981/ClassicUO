namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserHarmonySession
{
    ValueTask<BrowserClientRuntimeBrowserHarmonySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserHarmonySessionService : IBrowserClientRuntimeBrowserHarmonySession
{
    private readonly IBrowserClientRuntimeBrowserFluencyReadyState _runtimeBrowserFluencyReadyState;

    public BrowserClientRuntimeBrowserHarmonySessionService(IBrowserClientRuntimeBrowserFluencyReadyState runtimeBrowserFluencyReadyState)
    {
        _runtimeBrowserFluencyReadyState = runtimeBrowserFluencyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserHarmonySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFluencyReadyStateResult fluencyReadyState = await _runtimeBrowserFluencyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserHarmonySessionResult result = new()
        {
            ProfileId = fluencyReadyState.ProfileId,
            SessionId = fluencyReadyState.SessionId,
            SessionPath = fluencyReadyState.SessionPath,
            BrowserFluencyReadyStateVersion = fluencyReadyState.BrowserFluencyReadyStateVersion,
            BrowserFluencySessionVersion = fluencyReadyState.BrowserFluencySessionVersion,
            LaunchMode = fluencyReadyState.LaunchMode,
            AssetRootPath = fluencyReadyState.AssetRootPath,
            ProfilesRootPath = fluencyReadyState.ProfilesRootPath,
            CacheRootPath = fluencyReadyState.CacheRootPath,
            ConfigRootPath = fluencyReadyState.ConfigRootPath,
            SettingsFilePath = fluencyReadyState.SettingsFilePath,
            StartupProfilePath = fluencyReadyState.StartupProfilePath,
            RequiredAssets = fluencyReadyState.RequiredAssets,
            ReadyAssetCount = fluencyReadyState.ReadyAssetCount,
            CompletedSteps = fluencyReadyState.CompletedSteps,
            TotalSteps = fluencyReadyState.TotalSteps,
            Exists = fluencyReadyState.Exists,
            ReadSucceeded = fluencyReadyState.ReadSucceeded
        };

        if (!fluencyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser harmony session blocked for profile '{fluencyReadyState.ProfileId}'.";
            result.Error = fluencyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserHarmonySessionVersion = "runtime-browser-harmony-session-v1";
        result.BrowserHarmonyStages =
        [
            "open-browser-harmony-session",
            "bind-browser-fluency-ready-state",
            "publish-browser-harmony-ready"
        ];
        result.BrowserHarmonySummary = $"Runtime browser harmony session prepared {result.BrowserHarmonyStages.Length} harmony stage(s) for profile '{fluencyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser harmony session ready for profile '{fluencyReadyState.ProfileId}' with {result.BrowserHarmonyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserHarmonySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserHarmonySessionVersion { get; set; } = string.Empty;
    public string BrowserFluencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFluencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserHarmonyStages { get; set; } = Array.Empty<string>();
    public string BrowserHarmonySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

