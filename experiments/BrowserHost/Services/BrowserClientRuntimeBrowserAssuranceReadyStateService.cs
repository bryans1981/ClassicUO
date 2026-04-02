namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssuranceReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAssuranceReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssuranceReadyStateService : IBrowserClientRuntimeBrowserAssuranceReadyState
{
    private readonly IBrowserClientRuntimeBrowserAssuranceSession _runtimeBrowserAssuranceSession;

    public BrowserClientRuntimeBrowserAssuranceReadyStateService(IBrowserClientRuntimeBrowserAssuranceSession runtimeBrowserAssuranceSession)
    {
        _runtimeBrowserAssuranceSession = runtimeBrowserAssuranceSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssuranceReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssuranceSessionResult assuranceSession = await _runtimeBrowserAssuranceSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAssuranceReadyStateResult result = new()
        {
            ProfileId = assuranceSession.ProfileId,
            SessionId = assuranceSession.SessionId,
            SessionPath = assuranceSession.SessionPath,
            BrowserAssuranceSessionVersion = assuranceSession.BrowserAssuranceSessionVersion,
            BrowserTrustReadyStateVersion = assuranceSession.BrowserTrustReadyStateVersion,
            BrowserTrustSessionVersion = assuranceSession.BrowserTrustSessionVersion,
            LaunchMode = assuranceSession.LaunchMode,
            AssetRootPath = assuranceSession.AssetRootPath,
            ProfilesRootPath = assuranceSession.ProfilesRootPath,
            CacheRootPath = assuranceSession.CacheRootPath,
            ConfigRootPath = assuranceSession.ConfigRootPath,
            SettingsFilePath = assuranceSession.SettingsFilePath,
            StartupProfilePath = assuranceSession.StartupProfilePath,
            RequiredAssets = assuranceSession.RequiredAssets,
            ReadyAssetCount = assuranceSession.ReadyAssetCount,
            CompletedSteps = assuranceSession.CompletedSteps,
            TotalSteps = assuranceSession.TotalSteps,
            Exists = assuranceSession.Exists,
            ReadSucceeded = assuranceSession.ReadSucceeded,
            BrowserAssuranceSession = assuranceSession
        };

        if (!assuranceSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assurance ready state blocked for profile '{assuranceSession.ProfileId}'.";
            result.Error = assuranceSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssuranceReadyStateVersion = "runtime-browser-assurance-ready-state-v1";
        result.BrowserAssuranceReadyChecks =
        [
            "browser-trust-ready-state-ready",
            "browser-assurance-session-ready",
            "browser-assurance-ready"
        ];
        result.BrowserAssuranceReadySummary = $"Runtime browser assurance ready state passed {result.BrowserAssuranceReadyChecks.Length} assurance readiness check(s) for profile '{assuranceSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assurance ready state ready for profile '{assuranceSession.ProfileId}' with {result.BrowserAssuranceReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssuranceReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserTrustReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTrustSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssuranceReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAssuranceReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAssuranceSessionResult BrowserAssuranceSession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
