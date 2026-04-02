namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAdoptionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAdoptionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAdoptionReadyStateService : IBrowserClientRuntimeBrowserAdoptionReadyState
{
    private readonly IBrowserClientRuntimeBrowserAdoptionSession _runtimeBrowserAdoptionSession;

    public BrowserClientRuntimeBrowserAdoptionReadyStateService(IBrowserClientRuntimeBrowserAdoptionSession runtimeBrowserAdoptionSession)
    {
        _runtimeBrowserAdoptionSession = runtimeBrowserAdoptionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAdoptionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAdoptionSessionResult adoptionSession = await _runtimeBrowserAdoptionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAdoptionReadyStateResult result = new()
        {
            ProfileId = adoptionSession.ProfileId,
            SessionId = adoptionSession.SessionId,
            SessionPath = adoptionSession.SessionPath,
            BrowserAdoptionSessionVersion = adoptionSession.BrowserAdoptionSessionVersion,
            BrowserAcceptanceReadyStateVersion = adoptionSession.BrowserAcceptanceReadyStateVersion,
            BrowserAcceptanceSessionVersion = adoptionSession.BrowserAcceptanceSessionVersion,
            LaunchMode = adoptionSession.LaunchMode,
            AssetRootPath = adoptionSession.AssetRootPath,
            ProfilesRootPath = adoptionSession.ProfilesRootPath,
            CacheRootPath = adoptionSession.CacheRootPath,
            ConfigRootPath = adoptionSession.ConfigRootPath,
            SettingsFilePath = adoptionSession.SettingsFilePath,
            StartupProfilePath = adoptionSession.StartupProfilePath,
            RequiredAssets = adoptionSession.RequiredAssets,
            ReadyAssetCount = adoptionSession.ReadyAssetCount,
            CompletedSteps = adoptionSession.CompletedSteps,
            TotalSteps = adoptionSession.TotalSteps,
            Exists = adoptionSession.Exists,
            ReadSucceeded = adoptionSession.ReadSucceeded
        };

        if (!adoptionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser adoption ready state blocked for profile '{adoptionSession.ProfileId}'.";
            result.Error = adoptionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAdoptionReadyStateVersion = "runtime-browser-adoption-ready-state-v1";
        result.BrowserAdoptionReadyChecks =
        [
            "browser-acceptance-ready-state-ready",
            "browser-adoption-session-ready",
            "browser-adoption-ready"
        ];
        result.BrowserAdoptionReadySummary = $"Runtime browser adoption ready state passed {result.BrowserAdoptionReadyChecks.Length} adoption readiness check(s) for profile '{adoptionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser adoption ready state ready for profile '{adoptionSession.ProfileId}' with {result.BrowserAdoptionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAdoptionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAdoptionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAdoptionSessionVersion { get; set; } = string.Empty;
    public string BrowserAcceptanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAcceptanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAdoptionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAdoptionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
