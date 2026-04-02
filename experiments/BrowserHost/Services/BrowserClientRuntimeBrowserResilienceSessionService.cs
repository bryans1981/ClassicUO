namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserResilienceSession
{
    ValueTask<BrowserClientRuntimeBrowserResilienceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserResilienceSessionService : IBrowserClientRuntimeBrowserResilienceSession
{
    private readonly IBrowserClientRuntimeBrowserIntegrityReadyState _runtimeBrowserIntegrityReadyState;

    public BrowserClientRuntimeBrowserResilienceSessionService(IBrowserClientRuntimeBrowserIntegrityReadyState runtimeBrowserIntegrityReadyState)
    {
        _runtimeBrowserIntegrityReadyState = runtimeBrowserIntegrityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserResilienceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserIntegrityReadyStateResult integrityReadyState = await _runtimeBrowserIntegrityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserResilienceSessionResult result = new()
        {
            ProfileId = integrityReadyState.ProfileId,
            SessionId = integrityReadyState.SessionId,
            SessionPath = integrityReadyState.SessionPath,
            BrowserIntegrityReadyStateVersion = integrityReadyState.BrowserIntegrityReadyStateVersion,
            BrowserIntegritySessionVersion = integrityReadyState.BrowserIntegritySessionVersion,
            LaunchMode = integrityReadyState.LaunchMode,
            AssetRootPath = integrityReadyState.AssetRootPath,
            ProfilesRootPath = integrityReadyState.ProfilesRootPath,
            CacheRootPath = integrityReadyState.CacheRootPath,
            ConfigRootPath = integrityReadyState.ConfigRootPath,
            SettingsFilePath = integrityReadyState.SettingsFilePath,
            StartupProfilePath = integrityReadyState.StartupProfilePath,
            RequiredAssets = integrityReadyState.RequiredAssets,
            ReadyAssetCount = integrityReadyState.ReadyAssetCount,
            CompletedSteps = integrityReadyState.CompletedSteps,
            TotalSteps = integrityReadyState.TotalSteps,
            Exists = integrityReadyState.Exists,
            ReadSucceeded = integrityReadyState.ReadSucceeded,
            BrowserIntegrityReadyState = integrityReadyState
        };

        if (!integrityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser resilience session blocked for profile '{integrityReadyState.ProfileId}'.";
            result.Error = integrityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserResilienceSessionVersion = "runtime-browser-resilience-session-v1";
        result.BrowserResilienceStages =
        [
            "open-browser-resilience-session",
            "bind-browser-integrity-ready-state",
            "publish-browser-resilience-ready"
        ];
        result.BrowserResilienceSummary = $"Runtime browser resilience session prepared {result.BrowserResilienceStages.Length} resilience stage(s) for profile '{integrityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser resilience session ready for profile '{integrityReadyState.ProfileId}' with {result.BrowserResilienceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserResilienceSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserResilienceStages { get; set; } = Array.Empty<string>();
    public string BrowserResilienceSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserIntegrityReadyStateResult BrowserIntegrityReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
