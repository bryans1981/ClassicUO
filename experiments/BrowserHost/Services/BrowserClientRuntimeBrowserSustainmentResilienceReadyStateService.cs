namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainmentResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSustainmentResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainmentResilienceReadyStateService : IBrowserClientRuntimeBrowserSustainmentResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserSustainmentResilienceSession _runtimeBrowserSustainmentResilienceSession;

    public BrowserClientRuntimeBrowserSustainmentResilienceReadyStateService(IBrowserClientRuntimeBrowserSustainmentResilienceSession runtimeBrowserSustainmentResilienceSession)
    {
        _runtimeBrowserSustainmentResilienceSession = runtimeBrowserSustainmentResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainmentResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentResilienceSessionResult sustainmentresilienceSession = await _runtimeBrowserSustainmentResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSustainmentResilienceReadyStateResult result = new()
        {
            ProfileId = sustainmentresilienceSession.ProfileId,
            SessionId = sustainmentresilienceSession.SessionId,
            SessionPath = sustainmentresilienceSession.SessionPath,
            BrowserSustainmentResilienceSessionVersion = sustainmentresilienceSession.BrowserSustainmentResilienceSessionVersion,
            BrowserContinuationResilienceReadyStateVersion = sustainmentresilienceSession.BrowserContinuationResilienceReadyStateVersion,
            BrowserContinuationResilienceSessionVersion = sustainmentresilienceSession.BrowserContinuationResilienceSessionVersion,
            LaunchMode = sustainmentresilienceSession.LaunchMode,
            AssetRootPath = sustainmentresilienceSession.AssetRootPath,
            ProfilesRootPath = sustainmentresilienceSession.ProfilesRootPath,
            CacheRootPath = sustainmentresilienceSession.CacheRootPath,
            ConfigRootPath = sustainmentresilienceSession.ConfigRootPath,
            SettingsFilePath = sustainmentresilienceSession.SettingsFilePath,
            StartupProfilePath = sustainmentresilienceSession.StartupProfilePath,
            RequiredAssets = sustainmentresilienceSession.RequiredAssets,
            ReadyAssetCount = sustainmentresilienceSession.ReadyAssetCount,
            CompletedSteps = sustainmentresilienceSession.CompletedSteps,
            TotalSteps = sustainmentresilienceSession.TotalSteps,
            Exists = sustainmentresilienceSession.Exists,
            ReadSucceeded = sustainmentresilienceSession.ReadSucceeded
        };

        if (!sustainmentresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainmentresilience ready state blocked for profile '{sustainmentresilienceSession.ProfileId}'.";
            result.Error = sustainmentresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainmentResilienceReadyStateVersion = "runtime-browser-sustainmentresilience-ready-state-v1";
        result.BrowserSustainmentResilienceReadyChecks =
        [
            "browser-continuationresilience-ready-state-ready",
            "browser-sustainmentresilience-session-ready",
            "browser-sustainmentresilience-ready"
        ];
        result.BrowserSustainmentResilienceReadySummary = $"Runtime browser sustainmentresilience ready state passed {result.BrowserSustainmentResilienceReadyChecks.Length} sustainmentresilience readiness check(s) for profile '{sustainmentresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainmentresilience ready state ready for profile '{sustainmentresilienceSession.ProfileId}' with {result.BrowserSustainmentResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainmentResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainmentResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserContinuationResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuationResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainmentResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSustainmentResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
