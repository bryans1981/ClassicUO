namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFollowThroughReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFollowThroughReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFollowThroughReadyStateService : IBrowserClientRuntimeBrowserFollowThroughReadyState
{
    private readonly IBrowserClientRuntimeBrowserFollowThroughSession _runtimeBrowserFollowThroughSession;

    public BrowserClientRuntimeBrowserFollowThroughReadyStateService(IBrowserClientRuntimeBrowserFollowThroughSession runtimeBrowserFollowThroughSession)
    {
        _runtimeBrowserFollowThroughSession = runtimeBrowserFollowThroughSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFollowThroughReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFollowThroughSessionResult followthroughSession = await _runtimeBrowserFollowThroughSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFollowThroughReadyStateResult result = new()
        {
            ProfileId = followthroughSession.ProfileId,
            SessionId = followthroughSession.SessionId,
            SessionPath = followthroughSession.SessionPath,
            BrowserFollowThroughSessionVersion = followthroughSession.BrowserFollowThroughSessionVersion,
            BrowserDeterminationReadyStateVersion = followthroughSession.BrowserDeterminationReadyStateVersion,
            BrowserDeterminationSessionVersion = followthroughSession.BrowserDeterminationSessionVersion,
            LaunchMode = followthroughSession.LaunchMode,
            AssetRootPath = followthroughSession.AssetRootPath,
            ProfilesRootPath = followthroughSession.ProfilesRootPath,
            CacheRootPath = followthroughSession.CacheRootPath,
            ConfigRootPath = followthroughSession.ConfigRootPath,
            SettingsFilePath = followthroughSession.SettingsFilePath,
            StartupProfilePath = followthroughSession.StartupProfilePath,
            RequiredAssets = followthroughSession.RequiredAssets,
            ReadyAssetCount = followthroughSession.ReadyAssetCount,
            CompletedSteps = followthroughSession.CompletedSteps,
            TotalSteps = followthroughSession.TotalSteps,
            Exists = followthroughSession.Exists,
            ReadSucceeded = followthroughSession.ReadSucceeded
        };

        if (!followthroughSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser followthrough ready state blocked for profile '{followthroughSession.ProfileId}'.";
            result.Error = followthroughSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFollowThroughReadyStateVersion = "runtime-browser-followthrough-ready-state-v1";
        result.BrowserFollowThroughReadyChecks =
        [
            "browser-determination-ready-state-ready",
            "browser-followthrough-session-ready",
            "browser-followthrough-ready"
        ];
        result.BrowserFollowThroughReadySummary = $"Runtime browser followthrough ready state passed {result.BrowserFollowThroughReadyChecks.Length} followthrough readiness check(s) for profile '{followthroughSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser followthrough ready state ready for profile '{followthroughSession.ProfileId}' with {result.BrowserFollowThroughReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFollowThroughReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFollowThroughReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFollowThroughSessionVersion { get; set; } = string.Empty;
    public string BrowserDeterminationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeterminationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFollowThroughReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFollowThroughReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
