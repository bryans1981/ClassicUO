namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConfirmationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserConfirmationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConfirmationReadyStateService : IBrowserClientRuntimeBrowserConfirmationReadyState
{
    private readonly IBrowserClientRuntimeBrowserConfirmationSession _runtimeBrowserConfirmationSession;

    public BrowserClientRuntimeBrowserConfirmationReadyStateService(IBrowserClientRuntimeBrowserConfirmationSession runtimeBrowserConfirmationSession)
    {
        _runtimeBrowserConfirmationSession = runtimeBrowserConfirmationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConfirmationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConfirmationSessionResult confirmationSession = await _runtimeBrowserConfirmationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserConfirmationReadyStateResult result = new()
        {
            ProfileId = confirmationSession.ProfileId,
            SessionId = confirmationSession.SessionId,
            SessionPath = confirmationSession.SessionPath,
            BrowserConfirmationSessionVersion = confirmationSession.BrowserConfirmationSessionVersion,
            BrowserCompletionAssuranceReadyStateVersion = confirmationSession.BrowserCompletionAssuranceReadyStateVersion,
            BrowserCompletionAssuranceSessionVersion = confirmationSession.BrowserCompletionAssuranceSessionVersion,
            LaunchMode = confirmationSession.LaunchMode,
            AssetRootPath = confirmationSession.AssetRootPath,
            ProfilesRootPath = confirmationSession.ProfilesRootPath,
            CacheRootPath = confirmationSession.CacheRootPath,
            ConfigRootPath = confirmationSession.ConfigRootPath,
            SettingsFilePath = confirmationSession.SettingsFilePath,
            StartupProfilePath = confirmationSession.StartupProfilePath,
            RequiredAssets = confirmationSession.RequiredAssets,
            ReadyAssetCount = confirmationSession.ReadyAssetCount,
            CompletedSteps = confirmationSession.CompletedSteps,
            TotalSteps = confirmationSession.TotalSteps,
            Exists = confirmationSession.Exists,
            ReadSucceeded = confirmationSession.ReadSucceeded
        };

        if (!confirmationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser confirmation ready state blocked for profile '{confirmationSession.ProfileId}'.";
            result.Error = confirmationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConfirmationReadyStateVersion = "runtime-browser-confirmation-ready-state-v1";
        result.BrowserConfirmationReadyChecks =
        [
            "browser-completionassurance-ready-state-ready",
            "browser-confirmation-session-ready",
            "browser-confirmation-ready"
        ];
        result.BrowserConfirmationReadySummary = $"Runtime browser confirmation ready state passed {result.BrowserConfirmationReadyChecks.Length} confirmation readiness check(s) for profile '{confirmationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser confirmation ready state ready for profile '{confirmationSession.ProfileId}' with {result.BrowserConfirmationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConfirmationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserConfirmationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfirmationSessionVersion { get; set; } = string.Empty;
    public string BrowserCompletionAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCompletionAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConfirmationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserConfirmationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
