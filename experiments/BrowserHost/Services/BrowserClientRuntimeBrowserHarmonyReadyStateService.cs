namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserHarmonyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserHarmonyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserHarmonyReadyStateService : IBrowserClientRuntimeBrowserHarmonyReadyState
{
    private readonly IBrowserClientRuntimeBrowserHarmonySession _runtimeBrowserHarmonySession;

    public BrowserClientRuntimeBrowserHarmonyReadyStateService(IBrowserClientRuntimeBrowserHarmonySession runtimeBrowserHarmonySession)
    {
        _runtimeBrowserHarmonySession = runtimeBrowserHarmonySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserHarmonyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserHarmonySessionResult harmonySession = await _runtimeBrowserHarmonySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserHarmonyReadyStateResult result = new()
        {
            ProfileId = harmonySession.ProfileId,
            SessionId = harmonySession.SessionId,
            SessionPath = harmonySession.SessionPath,
            BrowserHarmonySessionVersion = harmonySession.BrowserHarmonySessionVersion,
            BrowserFluencyReadyStateVersion = harmonySession.BrowserFluencyReadyStateVersion,
            BrowserFluencySessionVersion = harmonySession.BrowserFluencySessionVersion,
            LaunchMode = harmonySession.LaunchMode,
            AssetRootPath = harmonySession.AssetRootPath,
            ProfilesRootPath = harmonySession.ProfilesRootPath,
            CacheRootPath = harmonySession.CacheRootPath,
            ConfigRootPath = harmonySession.ConfigRootPath,
            SettingsFilePath = harmonySession.SettingsFilePath,
            StartupProfilePath = harmonySession.StartupProfilePath,
            RequiredAssets = harmonySession.RequiredAssets,
            ReadyAssetCount = harmonySession.ReadyAssetCount,
            CompletedSteps = harmonySession.CompletedSteps,
            TotalSteps = harmonySession.TotalSteps,
            Exists = harmonySession.Exists,
            ReadSucceeded = harmonySession.ReadSucceeded
        };

        if (!harmonySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser harmony ready state blocked for profile '{harmonySession.ProfileId}'.";
            result.Error = harmonySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserHarmonyReadyStateVersion = "runtime-browser-harmony-ready-state-v1";
        result.BrowserHarmonyReadyChecks =
        [
            "browser-fluency-ready-state-ready",
            "browser-harmony-session-ready",
            "browser-harmony-ready"
        ];
        result.BrowserHarmonyReadySummary = $"Runtime browser harmony ready state passed {result.BrowserHarmonyReadyChecks.Length} harmony readiness check(s) for profile '{harmonySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser harmony ready state ready for profile '{harmonySession.ProfileId}' with {result.BrowserHarmonyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserHarmonyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserHarmonyReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserHarmonyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserHarmonyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

