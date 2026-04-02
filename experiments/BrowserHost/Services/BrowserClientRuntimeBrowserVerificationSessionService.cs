namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserVerificationSession
{
    ValueTask<BrowserClientRuntimeBrowserVerificationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserVerificationSessionService : IBrowserClientRuntimeBrowserVerificationSession
{
    private readonly IBrowserClientRuntimeBrowserConfirmationReadyState _runtimeBrowserConfirmationReadyState;

    public BrowserClientRuntimeBrowserVerificationSessionService(IBrowserClientRuntimeBrowserConfirmationReadyState runtimeBrowserConfirmationReadyState)
    {
        _runtimeBrowserConfirmationReadyState = runtimeBrowserConfirmationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserVerificationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConfirmationReadyStateResult confirmationReadyState = await _runtimeBrowserConfirmationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserVerificationSessionResult result = new()
        {
            ProfileId = confirmationReadyState.ProfileId,
            SessionId = confirmationReadyState.SessionId,
            SessionPath = confirmationReadyState.SessionPath,
            BrowserConfirmationReadyStateVersion = confirmationReadyState.BrowserConfirmationReadyStateVersion,
            BrowserConfirmationSessionVersion = confirmationReadyState.BrowserConfirmationSessionVersion,
            LaunchMode = confirmationReadyState.LaunchMode,
            AssetRootPath = confirmationReadyState.AssetRootPath,
            ProfilesRootPath = confirmationReadyState.ProfilesRootPath,
            CacheRootPath = confirmationReadyState.CacheRootPath,
            ConfigRootPath = confirmationReadyState.ConfigRootPath,
            SettingsFilePath = confirmationReadyState.SettingsFilePath,
            StartupProfilePath = confirmationReadyState.StartupProfilePath,
            RequiredAssets = confirmationReadyState.RequiredAssets,
            ReadyAssetCount = confirmationReadyState.ReadyAssetCount,
            CompletedSteps = confirmationReadyState.CompletedSteps,
            TotalSteps = confirmationReadyState.TotalSteps,
            Exists = confirmationReadyState.Exists,
            ReadSucceeded = confirmationReadyState.ReadSucceeded
        };

        if (!confirmationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser verification session blocked for profile '{confirmationReadyState.ProfileId}'.";
            result.Error = confirmationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserVerificationSessionVersion = "runtime-browser-verification-session-v1";
        result.BrowserVerificationStages =
        [
            "open-browser-verification-session",
            "bind-browser-confirmation-ready-state",
            "publish-browser-verification-ready"
        ];
        result.BrowserVerificationSummary = $"Runtime browser verification session prepared {result.BrowserVerificationStages.Length} verification stage(s) for profile '{confirmationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser verification session ready for profile '{confirmationReadyState.ProfileId}' with {result.BrowserVerificationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserVerificationSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserVerificationStages { get; set; } = Array.Empty<string>();
    public string BrowserVerificationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
