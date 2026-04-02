namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDelightReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDelightReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDelightReadyStateService : IBrowserClientRuntimeBrowserDelightReadyState
{
    private readonly IBrowserClientRuntimeBrowserDelightSession _runtimeBrowserDelightSession;

    public BrowserClientRuntimeBrowserDelightReadyStateService(IBrowserClientRuntimeBrowserDelightSession runtimeBrowserDelightSession)
    {
        _runtimeBrowserDelightSession = runtimeBrowserDelightSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDelightReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDelightSessionResult delightSession = await _runtimeBrowserDelightSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDelightReadyStateResult result = new()
        {
            ProfileId = delightSession.ProfileId,
            SessionId = delightSession.SessionId,
            SessionPath = delightSession.SessionPath,
            BrowserDelightSessionVersion = delightSession.BrowserDelightSessionVersion,
            BrowserSatisfactionReadyStateVersion = delightSession.BrowserSatisfactionReadyStateVersion,
            BrowserSatisfactionSessionVersion = delightSession.BrowserSatisfactionSessionVersion,
            LaunchMode = delightSession.LaunchMode,
            AssetRootPath = delightSession.AssetRootPath,
            ProfilesRootPath = delightSession.ProfilesRootPath,
            CacheRootPath = delightSession.CacheRootPath,
            ConfigRootPath = delightSession.ConfigRootPath,
            SettingsFilePath = delightSession.SettingsFilePath,
            StartupProfilePath = delightSession.StartupProfilePath,
            RequiredAssets = delightSession.RequiredAssets,
            ReadyAssetCount = delightSession.ReadyAssetCount,
            CompletedSteps = delightSession.CompletedSteps,
            TotalSteps = delightSession.TotalSteps,
            Exists = delightSession.Exists,
            ReadSucceeded = delightSession.ReadSucceeded
        };

        if (!delightSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser delight ready state blocked for profile '{delightSession.ProfileId}'.";
            result.Error = delightSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDelightReadyStateVersion = "runtime-browser-delight-ready-state-v1";
        result.BrowserDelightReadyChecks =
        [
            "browser-satisfaction-ready-state-ready",
            "browser-delight-session-ready",
            "browser-delight-ready"
        ];
        result.BrowserDelightReadySummary = $"Runtime browser delight ready state passed {result.BrowserDelightReadyChecks.Length} delight readiness check(s) for profile '{delightSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser delight ready state ready for profile '{delightSession.ProfileId}' with {result.BrowserDelightReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDelightReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDelightReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDelightSessionVersion { get; set; } = string.Empty;
    public string BrowserSatisfactionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSatisfactionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDelightReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDelightReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
