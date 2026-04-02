namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCorroborationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCorroborationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCorroborationReadyStateService : IBrowserClientRuntimeBrowserCorroborationReadyState
{
    private readonly IBrowserClientRuntimeBrowserCorroborationSession _runtimeBrowserCorroborationSession;

    public BrowserClientRuntimeBrowserCorroborationReadyStateService(IBrowserClientRuntimeBrowserCorroborationSession runtimeBrowserCorroborationSession)
    {
        _runtimeBrowserCorroborationSession = runtimeBrowserCorroborationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCorroborationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCorroborationSessionResult corroborationSession = await _runtimeBrowserCorroborationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCorroborationReadyStateResult result = new()
        {
            ProfileId = corroborationSession.ProfileId,
            SessionId = corroborationSession.SessionId,
            SessionPath = corroborationSession.SessionPath,
            BrowserCorroborationSessionVersion = corroborationSession.BrowserCorroborationSessionVersion,
            BrowserVerificationReadyStateVersion = corroborationSession.BrowserVerificationReadyStateVersion,
            BrowserVerificationSessionVersion = corroborationSession.BrowserVerificationSessionVersion,
            LaunchMode = corroborationSession.LaunchMode,
            AssetRootPath = corroborationSession.AssetRootPath,
            ProfilesRootPath = corroborationSession.ProfilesRootPath,
            CacheRootPath = corroborationSession.CacheRootPath,
            ConfigRootPath = corroborationSession.ConfigRootPath,
            SettingsFilePath = corroborationSession.SettingsFilePath,
            StartupProfilePath = corroborationSession.StartupProfilePath,
            RequiredAssets = corroborationSession.RequiredAssets,
            ReadyAssetCount = corroborationSession.ReadyAssetCount,
            CompletedSteps = corroborationSession.CompletedSteps,
            TotalSteps = corroborationSession.TotalSteps,
            Exists = corroborationSession.Exists,
            ReadSucceeded = corroborationSession.ReadSucceeded
        };

        if (!corroborationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser corroboration ready state blocked for profile '{corroborationSession.ProfileId}'.";
            result.Error = corroborationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCorroborationReadyStateVersion = "runtime-browser-corroboration-ready-state-v1";
        result.BrowserCorroborationReadyChecks =
        [
            "browser-verification-ready-state-ready",
            "browser-corroboration-session-ready",
            "browser-corroboration-ready"
        ];
        result.BrowserCorroborationReadySummary = $"Runtime browser corroboration ready state passed {result.BrowserCorroborationReadyChecks.Length} corroboration readiness check(s) for profile '{corroborationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser corroboration ready state ready for profile '{corroborationSession.ProfileId}' with {result.BrowserCorroborationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCorroborationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCorroborationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCorroborationSessionVersion { get; set; } = string.Empty;
    public string BrowserVerificationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserVerificationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCorroborationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCorroborationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
