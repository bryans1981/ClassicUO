namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserVerificationReadyState
{
    ValueTask<BrowserClientRuntimeBrowserVerificationReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserVerificationReadyStateService : IBrowserClientRuntimeBrowserVerificationReadyState
{
    private readonly IBrowserClientRuntimeBrowserVerificationSession _runtimeBrowserVerificationSession;

    public BrowserClientRuntimeBrowserVerificationReadyStateService(IBrowserClientRuntimeBrowserVerificationSession runtimeBrowserVerificationSession)
    {
        _runtimeBrowserVerificationSession = runtimeBrowserVerificationSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserVerificationReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserVerificationSessionResult verificationSession = await _runtimeBrowserVerificationSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserVerificationReadyStateResult result = new()
        {
            ProfileId = verificationSession.ProfileId,
            SessionId = verificationSession.SessionId,
            SessionPath = verificationSession.SessionPath,
            BrowserVerificationSessionVersion = verificationSession.BrowserVerificationSessionVersion,
            BrowserConfirmationReadyStateVersion = verificationSession.BrowserConfirmationReadyStateVersion,
            BrowserConfirmationSessionVersion = verificationSession.BrowserConfirmationSessionVersion,
            LaunchMode = verificationSession.LaunchMode,
            AssetRootPath = verificationSession.AssetRootPath,
            ProfilesRootPath = verificationSession.ProfilesRootPath,
            CacheRootPath = verificationSession.CacheRootPath,
            ConfigRootPath = verificationSession.ConfigRootPath,
            SettingsFilePath = verificationSession.SettingsFilePath,
            StartupProfilePath = verificationSession.StartupProfilePath,
            RequiredAssets = verificationSession.RequiredAssets,
            ReadyAssetCount = verificationSession.ReadyAssetCount,
            CompletedSteps = verificationSession.CompletedSteps,
            TotalSteps = verificationSession.TotalSteps,
            Exists = verificationSession.Exists,
            ReadSucceeded = verificationSession.ReadSucceeded
        };

        if (!verificationSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser verification ready state blocked for profile '{verificationSession.ProfileId}'.";
            result.Error = verificationSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserVerificationReadyStateVersion = "runtime-browser-verification-ready-state-v1";
        result.BrowserVerificationReadyChecks =
        [
            "browser-confirmation-ready-state-ready",
            "browser-verification-session-ready",
            "browser-verification-ready"
        ];
        result.BrowserVerificationReadySummary = $"Runtime browser verification ready state passed {result.BrowserVerificationReadyChecks.Length} verification readiness check(s) for profile '{verificationSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser verification ready state ready for profile '{verificationSession.ProfileId}' with {result.BrowserVerificationReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserVerificationReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserVerificationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserVerificationSessionVersion { get; set; } = string.Empty;
    public string BrowserConfirmationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConfirmationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserVerificationReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserVerificationReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
