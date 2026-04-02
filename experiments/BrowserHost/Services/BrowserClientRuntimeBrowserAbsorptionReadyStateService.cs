namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAbsorptionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAbsorptionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAbsorptionReadyStateService : IBrowserClientRuntimeBrowserAbsorptionReadyState
{
    private readonly IBrowserClientRuntimeBrowserAbsorptionSession _runtimeBrowserAbsorptionSession;

    public BrowserClientRuntimeBrowserAbsorptionReadyStateService(IBrowserClientRuntimeBrowserAbsorptionSession runtimeBrowserAbsorptionSession)
    {
        _runtimeBrowserAbsorptionSession = runtimeBrowserAbsorptionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAbsorptionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAbsorptionSessionResult absorptionSession = await _runtimeBrowserAbsorptionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAbsorptionReadyStateResult result = new()
        {
            ProfileId = absorptionSession.ProfileId,
            SessionId = absorptionSession.SessionId,
            SessionPath = absorptionSession.SessionPath,
            BrowserAbsorptionSessionVersion = absorptionSession.BrowserAbsorptionSessionVersion,
            BrowserInvolvementReadyStateVersion = absorptionSession.BrowserInvolvementReadyStateVersion,
            BrowserInvolvementSessionVersion = absorptionSession.BrowserInvolvementSessionVersion,
            LaunchMode = absorptionSession.LaunchMode,
            AssetRootPath = absorptionSession.AssetRootPath,
            ProfilesRootPath = absorptionSession.ProfilesRootPath,
            CacheRootPath = absorptionSession.CacheRootPath,
            ConfigRootPath = absorptionSession.ConfigRootPath,
            SettingsFilePath = absorptionSession.SettingsFilePath,
            StartupProfilePath = absorptionSession.StartupProfilePath,
            RequiredAssets = absorptionSession.RequiredAssets,
            ReadyAssetCount = absorptionSession.ReadyAssetCount,
            CompletedSteps = absorptionSession.CompletedSteps,
            TotalSteps = absorptionSession.TotalSteps,
            Exists = absorptionSession.Exists,
            ReadSucceeded = absorptionSession.ReadSucceeded
        };

        if (!absorptionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser absorption ready state blocked for profile '{absorptionSession.ProfileId}'.";
            result.Error = absorptionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAbsorptionReadyStateVersion = "runtime-browser-absorption-ready-state-v1";
        result.BrowserAbsorptionReadyChecks =
        [
            "browser-involvement-ready-state-ready",
            "browser-absorption-session-ready",
            "browser-absorption-ready"
        ];
        result.BrowserAbsorptionReadySummary = $"Runtime browser absorption ready state passed {result.BrowserAbsorptionReadyChecks.Length} absorption readiness check(s) for profile '{absorptionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser absorption ready state ready for profile '{absorptionSession.ProfileId}' with {result.BrowserAbsorptionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAbsorptionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAbsorptionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAbsorptionSessionVersion { get; set; } = string.Empty;
    public string BrowserInvolvementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInvolvementSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAbsorptionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAbsorptionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
