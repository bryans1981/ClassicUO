namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeterminationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDeterminationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeterminationReadyStateService : IBrowserClientRuntimeBrowserDeterminationReadyState
{
    private readonly IBrowserClientRuntimeBrowserDeterminationSession _runtimeBrowserDeterminationSession;

    public BrowserClientRuntimeBrowserDeterminationReadyStateService(IBrowserClientRuntimeBrowserDeterminationSession runtimeBrowserDeterminationSession)
    {
        _runtimeBrowserDeterminationSession = runtimeBrowserDeterminationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeterminationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeterminationSessionResult determinationSession = await _runtimeBrowserDeterminationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDeterminationReadyStateResult result = new()
        {
            ProfileId = determinationSession.ProfileId,
            SessionId = determinationSession.SessionId,
            SessionPath = determinationSession.SessionPath,
            BrowserDeterminationSessionVersion = determinationSession.BrowserDeterminationSessionVersion,
            BrowserResolveReadyStateVersion = determinationSession.BrowserResolveReadyStateVersion,
            BrowserResolveSessionVersion = determinationSession.BrowserResolveSessionVersion,
            LaunchMode = determinationSession.LaunchMode,
            AssetRootPath = determinationSession.AssetRootPath,
            ProfilesRootPath = determinationSession.ProfilesRootPath,
            CacheRootPath = determinationSession.CacheRootPath,
            ConfigRootPath = determinationSession.ConfigRootPath,
            SettingsFilePath = determinationSession.SettingsFilePath,
            StartupProfilePath = determinationSession.StartupProfilePath,
            RequiredAssets = determinationSession.RequiredAssets,
            ReadyAssetCount = determinationSession.ReadyAssetCount,
            CompletedSteps = determinationSession.CompletedSteps,
            TotalSteps = determinationSession.TotalSteps,
            Exists = determinationSession.Exists,
            ReadSucceeded = determinationSession.ReadSucceeded
        };

        if (!determinationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser determination ready state blocked for profile '{determinationSession.ProfileId}'.";
            result.Error = determinationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeterminationReadyStateVersion = "runtime-browser-determination-ready-state-v1";
        result.BrowserDeterminationReadyChecks =
        [
            "browser-resolve-ready-state-ready",
            "browser-determination-session-ready",
            "browser-determination-ready"
        ];
        result.BrowserDeterminationReadySummary = $"Runtime browser determination ready state passed {result.BrowserDeterminationReadyChecks.Length} determination readiness check(s) for profile '{determinationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser determination ready state ready for profile '{determinationSession.ProfileId}' with {result.BrowserDeterminationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeterminationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDeterminationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeterminationSessionVersion { get; set; } = string.Empty;
    public string BrowserResolveReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResolveSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeterminationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDeterminationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
