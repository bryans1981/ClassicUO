namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionResilienceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSessionResilienceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionResilienceReadyStateService : IBrowserClientRuntimeBrowserSessionResilienceReadyState
{
    private readonly IBrowserClientRuntimeBrowserSessionResilienceSession _runtimeBrowserSessionResilienceSession;

    public BrowserClientRuntimeBrowserSessionResilienceReadyStateService(IBrowserClientRuntimeBrowserSessionResilienceSession runtimeBrowserSessionResilienceSession)
    {
        _runtimeBrowserSessionResilienceSession = runtimeBrowserSessionResilienceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionResilienceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionResilienceSessionResult sessionresilienceSession = await _runtimeBrowserSessionResilienceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSessionResilienceReadyStateResult result = new()
        {
            ProfileId = sessionresilienceSession.ProfileId,
            SessionId = sessionresilienceSession.SessionId,
            SessionPath = sessionresilienceSession.SessionPath,
            BrowserSessionResilienceSessionVersion = sessionresilienceSession.BrowserSessionResilienceSessionVersion,
            BrowserRuntimeResilienceReadyStateVersion = sessionresilienceSession.BrowserRuntimeResilienceReadyStateVersion,
            BrowserRuntimeResilienceSessionVersion = sessionresilienceSession.BrowserRuntimeResilienceSessionVersion,
            LaunchMode = sessionresilienceSession.LaunchMode,
            AssetRootPath = sessionresilienceSession.AssetRootPath,
            ProfilesRootPath = sessionresilienceSession.ProfilesRootPath,
            CacheRootPath = sessionresilienceSession.CacheRootPath,
            ConfigRootPath = sessionresilienceSession.ConfigRootPath,
            SettingsFilePath = sessionresilienceSession.SettingsFilePath,
            StartupProfilePath = sessionresilienceSession.StartupProfilePath,
            RequiredAssets = sessionresilienceSession.RequiredAssets,
            ReadyAssetCount = sessionresilienceSession.ReadyAssetCount,
            CompletedSteps = sessionresilienceSession.CompletedSteps,
            TotalSteps = sessionresilienceSession.TotalSteps,
            Exists = sessionresilienceSession.Exists,
            ReadSucceeded = sessionresilienceSession.ReadSucceeded
        };

        if (!sessionresilienceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sessionresilience ready state blocked for profile '{sessionresilienceSession.ProfileId}'.";
            result.Error = sessionresilienceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionResilienceReadyStateVersion = "runtime-browser-sessionresilience-ready-state-v1";
        result.BrowserSessionResilienceReadyChecks =
        [
            "browser-runtimeresilience-ready-state-ready",
            "browser-sessionresilience-session-ready",
            "browser-sessionresilience-ready"
        ];
        result.BrowserSessionResilienceReadySummary = $"Runtime browser sessionresilience ready state passed {result.BrowserSessionResilienceReadyChecks.Length} sessionresilience readiness check(s) for profile '{sessionresilienceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessionresilience ready state ready for profile '{sessionresilienceSession.ProfileId}' with {result.BrowserSessionResilienceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionResilienceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionResilienceSessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionResilienceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSessionResilienceReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
