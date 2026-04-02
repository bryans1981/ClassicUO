namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssurance2ReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAssurance2ReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssurance2ReadyStateService : IBrowserClientRuntimeBrowserAssurance2ReadyState
{
    private readonly IBrowserClientRuntimeBrowserAssurance2Session _runtimeBrowserAssurance2Session;

    public BrowserClientRuntimeBrowserAssurance2ReadyStateService(IBrowserClientRuntimeBrowserAssurance2Session runtimeBrowserAssurance2Session)
    {
        _runtimeBrowserAssurance2Session = runtimeBrowserAssurance2Session;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssurance2ReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssurance2SessionResult assurance2Session = await _runtimeBrowserAssurance2Session.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAssurance2ReadyStateResult result = new()
        {
            ProfileId = assurance2Session.ProfileId,
            SessionId = assurance2Session.SessionId,
            SessionPath = assurance2Session.SessionPath,
            BrowserAssurance2SessionVersion = assurance2Session.BrowserAssurance2SessionVersion,
            BrowserTrustworthinessReadyStateVersion = assurance2Session.BrowserTrustworthinessReadyStateVersion,
            BrowserTrustworthinessSessionVersion = assurance2Session.BrowserTrustworthinessSessionVersion,
            LaunchMode = assurance2Session.LaunchMode,
            AssetRootPath = assurance2Session.AssetRootPath,
            ProfilesRootPath = assurance2Session.ProfilesRootPath,
            CacheRootPath = assurance2Session.CacheRootPath,
            ConfigRootPath = assurance2Session.ConfigRootPath,
            SettingsFilePath = assurance2Session.SettingsFilePath,
            StartupProfilePath = assurance2Session.StartupProfilePath,
            RequiredAssets = assurance2Session.RequiredAssets,
            ReadyAssetCount = assurance2Session.ReadyAssetCount,
            CompletedSteps = assurance2Session.CompletedSteps,
            TotalSteps = assurance2Session.TotalSteps,
            Exists = assurance2Session.Exists,
            ReadSucceeded = assurance2Session.ReadSucceeded
        };

        if (!assurance2Session.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assurance2 ready state blocked for profile '{assurance2Session.ProfileId}'.";
            result.Error = assurance2Session.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssurance2ReadyStateVersion = "runtime-browser-assurance2-ready-state-v1";
        result.BrowserAssurance2ReadyChecks =
        [
            "browser-trustworthiness-ready-state-ready",
            "browser-assurance2-session-ready",
            "browser-assurance2-ready"
        ];
        result.BrowserAssurance2ReadySummary = $"Runtime browser assurance2 ready state passed {result.BrowserAssurance2ReadyChecks.Length} assurance2 readiness check(s) for profile '{assurance2Session.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assurance2 ready state ready for profile '{assurance2Session.ProfileId}' with {result.BrowserAssurance2ReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssurance2ReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAssurance2ReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurance2SessionVersion { get; set; } = string.Empty;
    public string BrowserTrustworthinessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustworthinessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssurance2ReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAssurance2ReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
