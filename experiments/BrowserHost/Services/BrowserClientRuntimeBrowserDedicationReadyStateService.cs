namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDedicationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDedicationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDedicationReadyStateService : IBrowserClientRuntimeBrowserDedicationReadyState
{
    private readonly IBrowserClientRuntimeBrowserDedicationSession _runtimeBrowserDedicationSession;

    public BrowserClientRuntimeBrowserDedicationReadyStateService(IBrowserClientRuntimeBrowserDedicationSession runtimeBrowserDedicationSession)
    {
        _runtimeBrowserDedicationSession = runtimeBrowserDedicationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDedicationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDedicationSessionResult dedicationSession = await _runtimeBrowserDedicationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDedicationReadyStateResult result = new()
        {
            ProfileId = dedicationSession.ProfileId,
            SessionId = dedicationSession.SessionId,
            SessionPath = dedicationSession.SessionPath,
            BrowserDedicationSessionVersion = dedicationSession.BrowserDedicationSessionVersion,
            BrowserCommitmentReadyStateVersion = dedicationSession.BrowserCommitmentReadyStateVersion,
            BrowserCommitmentSessionVersion = dedicationSession.BrowserCommitmentSessionVersion,
            LaunchMode = dedicationSession.LaunchMode,
            AssetRootPath = dedicationSession.AssetRootPath,
            ProfilesRootPath = dedicationSession.ProfilesRootPath,
            CacheRootPath = dedicationSession.CacheRootPath,
            ConfigRootPath = dedicationSession.ConfigRootPath,
            SettingsFilePath = dedicationSession.SettingsFilePath,
            StartupProfilePath = dedicationSession.StartupProfilePath,
            RequiredAssets = dedicationSession.RequiredAssets,
            ReadyAssetCount = dedicationSession.ReadyAssetCount,
            CompletedSteps = dedicationSession.CompletedSteps,
            TotalSteps = dedicationSession.TotalSteps,
            Exists = dedicationSession.Exists,
            ReadSucceeded = dedicationSession.ReadSucceeded
        };

        if (!dedicationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser dedication ready state blocked for profile '{dedicationSession.ProfileId}'.";
            result.Error = dedicationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDedicationReadyStateVersion = "runtime-browser-dedication-ready-state-v1";
        result.BrowserDedicationReadyChecks =
        [
            "browser-commitment-ready-state-ready",
            "browser-dedication-session-ready",
            "browser-dedication-ready"
        ];
        result.BrowserDedicationReadySummary = $"Runtime browser dedication ready state passed {result.BrowserDedicationReadyChecks.Length} dedication readiness check(s) for profile '{dedicationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser dedication ready state ready for profile '{dedicationSession.ProfileId}' with {result.BrowserDedicationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDedicationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDedicationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDedicationSessionVersion { get; set; } = string.Empty;
    public string BrowserCommitmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCommitmentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDedicationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDedicationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
