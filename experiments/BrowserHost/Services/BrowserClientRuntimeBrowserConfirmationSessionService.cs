namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConfirmationSession
{
    ValueTask<BrowserClientRuntimeBrowserConfirmationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConfirmationSessionService : IBrowserClientRuntimeBrowserConfirmationSession
{
    private readonly IBrowserClientRuntimeBrowserCompletionAssuranceReadyState _runtimeBrowserCompletionAssuranceReadyState;

    public BrowserClientRuntimeBrowserConfirmationSessionService(IBrowserClientRuntimeBrowserCompletionAssuranceReadyState runtimeBrowserCompletionAssuranceReadyState)
    {
        _runtimeBrowserCompletionAssuranceReadyState = runtimeBrowserCompletionAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConfirmationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCompletionAssuranceReadyStateResult completionassuranceReadyState = await _runtimeBrowserCompletionAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserConfirmationSessionResult result = new()
        {
            ProfileId = completionassuranceReadyState.ProfileId,
            SessionId = completionassuranceReadyState.SessionId,
            SessionPath = completionassuranceReadyState.SessionPath,
            BrowserCompletionAssuranceReadyStateVersion = completionassuranceReadyState.BrowserCompletionAssuranceReadyStateVersion,
            BrowserCompletionAssuranceSessionVersion = completionassuranceReadyState.BrowserCompletionAssuranceSessionVersion,
            LaunchMode = completionassuranceReadyState.LaunchMode,
            AssetRootPath = completionassuranceReadyState.AssetRootPath,
            ProfilesRootPath = completionassuranceReadyState.ProfilesRootPath,
            CacheRootPath = completionassuranceReadyState.CacheRootPath,
            ConfigRootPath = completionassuranceReadyState.ConfigRootPath,
            SettingsFilePath = completionassuranceReadyState.SettingsFilePath,
            StartupProfilePath = completionassuranceReadyState.StartupProfilePath,
            RequiredAssets = completionassuranceReadyState.RequiredAssets,
            ReadyAssetCount = completionassuranceReadyState.ReadyAssetCount,
            CompletedSteps = completionassuranceReadyState.CompletedSteps,
            TotalSteps = completionassuranceReadyState.TotalSteps,
            Exists = completionassuranceReadyState.Exists,
            ReadSucceeded = completionassuranceReadyState.ReadSucceeded
        };

        if (!completionassuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser confirmation session blocked for profile '{completionassuranceReadyState.ProfileId}'.";
            result.Error = completionassuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConfirmationSessionVersion = "runtime-browser-confirmation-session-v1";
        result.BrowserConfirmationStages =
        [
            "open-browser-confirmation-session",
            "bind-browser-completionassurance-ready-state",
            "publish-browser-confirmation-ready"
        ];
        result.BrowserConfirmationSummary = $"Runtime browser confirmation session prepared {result.BrowserConfirmationStages.Length} confirmation stage(s) for profile '{completionassuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser confirmation session ready for profile '{completionassuranceReadyState.ProfileId}' with {result.BrowserConfirmationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConfirmationSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserConfirmationStages { get; set; } = Array.Empty<string>();
    public string BrowserConfirmationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
