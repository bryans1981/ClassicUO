namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssurednessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAssurednessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssurednessReadyStateService : IBrowserClientRuntimeBrowserAssurednessReadyState
{
    private readonly IBrowserClientRuntimeBrowserAssurednessSession _runtimeBrowserAssurednessSession;

    public BrowserClientRuntimeBrowserAssurednessReadyStateService(IBrowserClientRuntimeBrowserAssurednessSession runtimeBrowserAssurednessSession)
    {
        _runtimeBrowserAssurednessSession = runtimeBrowserAssurednessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssurednessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssurednessSessionResult assurednessSession = await _runtimeBrowserAssurednessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAssurednessReadyStateResult result = new()
        {
            ProfileId = assurednessSession.ProfileId,
            SessionId = assurednessSession.SessionId,
            SessionPath = assurednessSession.SessionPath,
            BrowserAssurednessSessionVersion = assurednessSession.BrowserAssurednessSessionVersion,
            BrowserCalmnessReadyStateVersion = assurednessSession.BrowserCalmnessReadyStateVersion,
            BrowserCalmnessSessionVersion = assurednessSession.BrowserCalmnessSessionVersion,
            LaunchMode = assurednessSession.LaunchMode,
            AssetRootPath = assurednessSession.AssetRootPath,
            ProfilesRootPath = assurednessSession.ProfilesRootPath,
            CacheRootPath = assurednessSession.CacheRootPath,
            ConfigRootPath = assurednessSession.ConfigRootPath,
            SettingsFilePath = assurednessSession.SettingsFilePath,
            StartupProfilePath = assurednessSession.StartupProfilePath,
            RequiredAssets = assurednessSession.RequiredAssets,
            ReadyAssetCount = assurednessSession.ReadyAssetCount,
            CompletedSteps = assurednessSession.CompletedSteps,
            TotalSteps = assurednessSession.TotalSteps,
            Exists = assurednessSession.Exists,
            ReadSucceeded = assurednessSession.ReadSucceeded
        };

        if (!assurednessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assuredness ready state blocked for profile '{assurednessSession.ProfileId}'.";
            result.Error = assurednessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssurednessReadyStateVersion = "runtime-browser-assuredness-ready-state-v1";
        result.BrowserAssurednessReadyChecks =
        [
            "browser-calmness-ready-state-ready",
            "browser-assuredness-session-ready",
            "browser-assuredness-ready"
        ];
        result.BrowserAssurednessReadySummary = $"Runtime browser assuredness ready state passed {result.BrowserAssurednessReadyChecks.Length} assuredness readiness check(s) for profile '{assurednessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assuredness ready state ready for profile '{assurednessSession.ProfileId}' with {result.BrowserAssurednessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssurednessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAssurednessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurednessSessionVersion { get; set; } = string.Empty;
    public string BrowserCalmnessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCalmnessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssurednessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAssurednessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
