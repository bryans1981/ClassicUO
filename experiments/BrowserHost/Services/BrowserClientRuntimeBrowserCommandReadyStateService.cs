namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCommandReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCommandReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCommandReadyStateService : IBrowserClientRuntimeBrowserCommandReadyState
{
    private readonly IBrowserClientRuntimeBrowserCommandSession _runtimeBrowserCommandSession;

    public BrowserClientRuntimeBrowserCommandReadyStateService(IBrowserClientRuntimeBrowserCommandSession runtimeBrowserCommandSession)
    {
        _runtimeBrowserCommandSession = runtimeBrowserCommandSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCommandReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCommandSessionResult commandSession = await _runtimeBrowserCommandSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCommandReadyStateResult result = new()
        {
            ProfileId = commandSession.ProfileId,
            SessionId = commandSession.SessionId,
            SessionPath = commandSession.SessionPath,
            BrowserCommandSessionVersion = commandSession.BrowserCommandSessionVersion,
            BrowserPointerReadyStateVersion = commandSession.BrowserPointerReadyStateVersion,
            BrowserPointerSessionVersion = commandSession.BrowserPointerSessionVersion,
            LaunchMode = commandSession.LaunchMode,
            AssetRootPath = commandSession.AssetRootPath,
            ProfilesRootPath = commandSession.ProfilesRootPath,
            CacheRootPath = commandSession.CacheRootPath,
            ConfigRootPath = commandSession.ConfigRootPath,
            SettingsFilePath = commandSession.SettingsFilePath,
            StartupProfilePath = commandSession.StartupProfilePath,
            RequiredAssets = commandSession.RequiredAssets,
            ReadyAssetCount = commandSession.ReadyAssetCount,
            CompletedSteps = commandSession.CompletedSteps,
            TotalSteps = commandSession.TotalSteps,
            Exists = commandSession.Exists,
            ReadSucceeded = commandSession.ReadSucceeded,
            BrowserCommandSession = commandSession
        };

        if (!commandSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser command ready state blocked for profile '{commandSession.ProfileId}'.";
            result.Error = commandSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCommandReadyStateVersion = "runtime-browser-command-ready-state-v1";
        result.BrowserCommandReadyChecks =
        [
            "browser-pointer-ready-state-ready",
            "browser-command-session-ready",
            "browser-command-ready"
        ];
        result.BrowserCommandReadySummary = $"Runtime browser command ready state passed {result.BrowserCommandReadyChecks.Length} command readiness check(s) for profile '{commandSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser command ready state ready for profile '{commandSession.ProfileId}' with {result.BrowserCommandReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCommandReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCommandReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCommandSessionVersion { get; set; } = string.Empty;
    public string BrowserPointerReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPointerSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCommandReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCommandReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserCommandSessionResult BrowserCommandSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
