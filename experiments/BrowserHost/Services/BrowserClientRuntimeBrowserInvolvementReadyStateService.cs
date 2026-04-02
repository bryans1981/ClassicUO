namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInvolvementReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInvolvementReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInvolvementReadyStateService : IBrowserClientRuntimeBrowserInvolvementReadyState
{
    private readonly IBrowserClientRuntimeBrowserInvolvementSession _runtimeBrowserInvolvementSession;

    public BrowserClientRuntimeBrowserInvolvementReadyStateService(IBrowserClientRuntimeBrowserInvolvementSession runtimeBrowserInvolvementSession)
    {
        _runtimeBrowserInvolvementSession = runtimeBrowserInvolvementSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInvolvementReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInvolvementSessionResult involvementSession = await _runtimeBrowserInvolvementSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserInvolvementReadyStateResult result = new()
        {
            ProfileId = involvementSession.ProfileId,
            SessionId = involvementSession.SessionId,
            SessionPath = involvementSession.SessionPath,
            BrowserInvolvementSessionVersion = involvementSession.BrowserInvolvementSessionVersion,
            BrowserMomentumReadyStateVersion = involvementSession.BrowserMomentumReadyStateVersion,
            BrowserMomentumSessionVersion = involvementSession.BrowserMomentumSessionVersion,
            LaunchMode = involvementSession.LaunchMode,
            AssetRootPath = involvementSession.AssetRootPath,
            ProfilesRootPath = involvementSession.ProfilesRootPath,
            CacheRootPath = involvementSession.CacheRootPath,
            ConfigRootPath = involvementSession.ConfigRootPath,
            SettingsFilePath = involvementSession.SettingsFilePath,
            StartupProfilePath = involvementSession.StartupProfilePath,
            RequiredAssets = involvementSession.RequiredAssets,
            ReadyAssetCount = involvementSession.ReadyAssetCount,
            CompletedSteps = involvementSession.CompletedSteps,
            TotalSteps = involvementSession.TotalSteps,
            Exists = involvementSession.Exists,
            ReadSucceeded = involvementSession.ReadSucceeded
        };

        if (!involvementSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser involvement ready state blocked for profile '{involvementSession.ProfileId}'.";
            result.Error = involvementSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInvolvementReadyStateVersion = "runtime-browser-involvement-ready-state-v1";
        result.BrowserInvolvementReadyChecks =
        [
            "browser-momentum-ready-state-ready",
            "browser-involvement-session-ready",
            "browser-involvement-ready"
        ];
        result.BrowserInvolvementReadySummary = $"Runtime browser involvement ready state passed {result.BrowserInvolvementReadyChecks.Length} involvement readiness check(s) for profile '{involvementSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser involvement ready state ready for profile '{involvementSession.ProfileId}' with {result.BrowserInvolvementReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInvolvementReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInvolvementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInvolvementSessionVersion { get; set; } = string.Empty;
    public string BrowserMomentumReadyStateVersion { get; set; } = string.Empty;
    public string BrowserMomentumSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInvolvementReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInvolvementReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
