namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReadabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserReadabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReadabilitySessionService : IBrowserClientRuntimeBrowserReadabilitySession
{
    private readonly IBrowserClientRuntimeBrowserHarmonyReadyState _runtimeBrowserHarmonyReadyState;

    public BrowserClientRuntimeBrowserReadabilitySessionService(IBrowserClientRuntimeBrowserHarmonyReadyState runtimeBrowserHarmonyReadyState)
    {
        _runtimeBrowserHarmonyReadyState = runtimeBrowserHarmonyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReadabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserHarmonyReadyStateResult harmonyReadyState = await _runtimeBrowserHarmonyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReadabilitySessionResult result = new()
        {
            ProfileId = harmonyReadyState.ProfileId,
            SessionId = harmonyReadyState.SessionId,
            SessionPath = harmonyReadyState.SessionPath,
            BrowserHarmonyReadyStateVersion = harmonyReadyState.BrowserHarmonyReadyStateVersion,
            BrowserHarmonySessionVersion = harmonyReadyState.BrowserHarmonySessionVersion,
            LaunchMode = harmonyReadyState.LaunchMode,
            AssetRootPath = harmonyReadyState.AssetRootPath,
            ProfilesRootPath = harmonyReadyState.ProfilesRootPath,
            CacheRootPath = harmonyReadyState.CacheRootPath,
            ConfigRootPath = harmonyReadyState.ConfigRootPath,
            SettingsFilePath = harmonyReadyState.SettingsFilePath,
            StartupProfilePath = harmonyReadyState.StartupProfilePath,
            RequiredAssets = harmonyReadyState.RequiredAssets,
            ReadyAssetCount = harmonyReadyState.ReadyAssetCount,
            CompletedSteps = harmonyReadyState.CompletedSteps,
            TotalSteps = harmonyReadyState.TotalSteps,
            Exists = harmonyReadyState.Exists,
            ReadSucceeded = harmonyReadyState.ReadSucceeded
        };

        if (!harmonyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser readability session blocked for profile '{harmonyReadyState.ProfileId}'.";
            result.Error = harmonyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReadabilitySessionVersion = "runtime-browser-readability-session-v1";
        result.BrowserReadabilityStages =
        [
            "open-browser-readability-session",
            "bind-browser-harmony-ready-state",
            "publish-browser-readability-ready"
        ];
        result.BrowserReadabilitySummary = $"Runtime browser readability session prepared {result.BrowserReadabilityStages.Length} readability stage(s) for profile '{harmonyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser readability session ready for profile '{harmonyReadyState.ProfileId}' with {result.BrowserReadabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReadabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserReadabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserHarmonyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHarmonySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReadabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserReadabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

