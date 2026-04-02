namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSatisfactionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSatisfactionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSatisfactionReadyStateService : IBrowserClientRuntimeBrowserSatisfactionReadyState
{
    private readonly IBrowserClientRuntimeBrowserSatisfactionSession _runtimeBrowserSatisfactionSession;

    public BrowserClientRuntimeBrowserSatisfactionReadyStateService(IBrowserClientRuntimeBrowserSatisfactionSession runtimeBrowserSatisfactionSession)
    {
        _runtimeBrowserSatisfactionSession = runtimeBrowserSatisfactionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSatisfactionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSatisfactionSessionResult satisfactionSession = await _runtimeBrowserSatisfactionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSatisfactionReadyStateResult result = new()
        {
            ProfileId = satisfactionSession.ProfileId,
            SessionId = satisfactionSession.SessionId,
            SessionPath = satisfactionSession.SessionPath,
            BrowserSatisfactionSessionVersion = satisfactionSession.BrowserSatisfactionSessionVersion,
            BrowserForgivenessReadyStateVersion = satisfactionSession.BrowserForgivenessReadyStateVersion,
            BrowserForgivenessSessionVersion = satisfactionSession.BrowserForgivenessSessionVersion,
            LaunchMode = satisfactionSession.LaunchMode,
            AssetRootPath = satisfactionSession.AssetRootPath,
            ProfilesRootPath = satisfactionSession.ProfilesRootPath,
            CacheRootPath = satisfactionSession.CacheRootPath,
            ConfigRootPath = satisfactionSession.ConfigRootPath,
            SettingsFilePath = satisfactionSession.SettingsFilePath,
            StartupProfilePath = satisfactionSession.StartupProfilePath,
            RequiredAssets = satisfactionSession.RequiredAssets,
            ReadyAssetCount = satisfactionSession.ReadyAssetCount,
            CompletedSteps = satisfactionSession.CompletedSteps,
            TotalSteps = satisfactionSession.TotalSteps,
            Exists = satisfactionSession.Exists,
            ReadSucceeded = satisfactionSession.ReadSucceeded
        };

        if (!satisfactionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser satisfaction ready state blocked for profile '{satisfactionSession.ProfileId}'.";
            result.Error = satisfactionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSatisfactionReadyStateVersion = "runtime-browser-satisfaction-ready-state-v1";
        result.BrowserSatisfactionReadyChecks =
        [
            "browser-forgiveness-ready-state-ready",
            "browser-satisfaction-session-ready",
            "browser-satisfaction-ready"
        ];
        result.BrowserSatisfactionReadySummary = $"Runtime browser satisfaction ready state passed {result.BrowserSatisfactionReadyChecks.Length} satisfaction readiness check(s) for profile '{satisfactionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser satisfaction ready state ready for profile '{satisfactionSession.ProfileId}' with {result.BrowserSatisfactionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSatisfactionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSatisfactionReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserSatisfactionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSatisfactionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
