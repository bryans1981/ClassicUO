namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResilienceReadyStateService : IBrowserClientRuntimeBrowserResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserResilienceSession _runtimeBrowserResilienceSession;

    public BrowserClientRuntimeBrowserResilienceReadyStateService(IBrowserClientRuntimeBrowserResilienceSession runtimeBrowserResilienceSession)
    {
        _runtimeBrowserResilienceSession = runtimeBrowserResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResilienceSessionResult resilienceSession = await _runtimeBrowserResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserResilienceReadyStateResult result = new()
        {
            ProfileId = resilienceSession.ProfileId,
            SessionId = resilienceSession.SessionId,
            SessionPath = resilienceSession.SessionPath,
            BrowserResilienceSessionVersion = resilienceSession.BrowserResilienceSessionVersion,
            BrowserIntegrityReadyStateVersion = resilienceSession.BrowserIntegrityReadyStateVersion,
            BrowserIntegritySessionVersion = resilienceSession.BrowserIntegritySessionVersion,
            LaunchMode = resilienceSession.LaunchMode,
            AssetRootPath = resilienceSession.AssetRootPath,
            ProfilesRootPath = resilienceSession.ProfilesRootPath,
            CacheRootPath = resilienceSession.CacheRootPath,
            ConfigRootPath = resilienceSession.ConfigRootPath,
            SettingsFilePath = resilienceSession.SettingsFilePath,
            StartupProfilePath = resilienceSession.StartupProfilePath,
            RequiredAssets = resilienceSession.RequiredAssets,
            ReadyAssetCount = resilienceSession.ReadyAssetCount,
            CompletedSteps = resilienceSession.CompletedSteps,
            TotalSteps = resilienceSession.TotalSteps,
            Exists = resilienceSession.Exists,
            ReadSucceeded = resilienceSession.ReadSucceeded,
            BrowserResilienceSession = resilienceSession
        };

        if (!resilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resilience ready state blocked for profile '{resilienceSession.ProfileId}'.";
            result.Error = resilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResilienceReadyStateVersion = "runtime-browser-resilience-ready-state-v1";
        result.BrowserResilienceReadyChecks =
        [
            "browser-integrity-ready-state-ready",
            "browser-resilience-session-ready",
            "browser-resilience-ready"
        ];
        result.BrowserResilienceReadySummary = $"Runtime browser resilience ready state passed {result.BrowserResilienceReadyChecks.Length} resilience readiness check(s) for profile '{resilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resilience ready state ready for profile '{resilienceSession.ProfileId}' with {result.BrowserResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserIntegrityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserIntegritySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserResilienceReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserResilienceSessionResult BrowserResilienceSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
