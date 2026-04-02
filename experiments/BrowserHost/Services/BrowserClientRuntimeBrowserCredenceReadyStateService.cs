namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCredenceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCredenceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCredenceReadyStateService : IBrowserClientRuntimeBrowserCredenceReadyState
{
    private readonly IBrowserClientRuntimeBrowserCredenceSession _runtimeBrowserCredenceSession;

    public BrowserClientRuntimeBrowserCredenceReadyStateService(IBrowserClientRuntimeBrowserCredenceSession runtimeBrowserCredenceSession)
    {
        _runtimeBrowserCredenceSession = runtimeBrowserCredenceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCredenceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCredenceSessionResult credenceSession = await _runtimeBrowserCredenceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCredenceReadyStateResult result = new()
        {
            ProfileId = credenceSession.ProfileId,
            SessionId = credenceSession.SessionId,
            SessionPath = credenceSession.SessionPath,
            BrowserCredenceSessionVersion = credenceSession.BrowserCredenceSessionVersion,
            BrowserCompletionConfidenceReadyStateVersion = credenceSession.BrowserCompletionConfidenceReadyStateVersion,
            BrowserCompletionConfidenceSessionVersion = credenceSession.BrowserCompletionConfidenceSessionVersion,
            LaunchMode = credenceSession.LaunchMode,
            AssetRootPath = credenceSession.AssetRootPath,
            ProfilesRootPath = credenceSession.ProfilesRootPath,
            CacheRootPath = credenceSession.CacheRootPath,
            ConfigRootPath = credenceSession.ConfigRootPath,
            SettingsFilePath = credenceSession.SettingsFilePath,
            StartupProfilePath = credenceSession.StartupProfilePath,
            RequiredAssets = credenceSession.RequiredAssets,
            ReadyAssetCount = credenceSession.ReadyAssetCount,
            CompletedSteps = credenceSession.CompletedSteps,
            TotalSteps = credenceSession.TotalSteps,
            Exists = credenceSession.Exists,
            ReadSucceeded = credenceSession.ReadSucceeded
        };

        if (!credenceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser credence ready state blocked for profile '{credenceSession.ProfileId}'.";
            result.Error = credenceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCredenceReadyStateVersion = "runtime-browser-credence-ready-state-v1";
        result.BrowserCredenceReadyChecks =
        [
            "browser-completionconfidence-ready-state-ready",
            "browser-credence-session-ready",
            "browser-credence-ready"
        ];
        result.BrowserCredenceReadySummary = $"Runtime browser credence ready state passed {result.BrowserCredenceReadyChecks.Length} credence readiness check(s) for profile '{credenceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser credence ready state ready for profile '{credenceSession.ProfileId}' with {result.BrowserCredenceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCredenceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCredenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCredenceSessionVersion { get; set; } = string.Empty;
    public string BrowserCompletionConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCompletionConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCredenceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCredenceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
