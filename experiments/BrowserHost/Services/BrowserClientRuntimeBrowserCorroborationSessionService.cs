namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCorroborationSession
{
    ValueTask<BrowserClientRuntimeBrowserCorroborationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCorroborationSessionService : IBrowserClientRuntimeBrowserCorroborationSession
{
    private readonly IBrowserClientRuntimeBrowserVerificationReadyState _runtimeBrowserVerificationReadyState;

    public BrowserClientRuntimeBrowserCorroborationSessionService(IBrowserClientRuntimeBrowserVerificationReadyState runtimeBrowserVerificationReadyState)
    {
        _runtimeBrowserVerificationReadyState = runtimeBrowserVerificationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCorroborationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserVerificationReadyStateResult verificationReadyState = await _runtimeBrowserVerificationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCorroborationSessionResult result = new()
        {
            ProfileId = verificationReadyState.ProfileId,
            SessionId = verificationReadyState.SessionId,
            SessionPath = verificationReadyState.SessionPath,
            BrowserVerificationReadyStateVersion = verificationReadyState.BrowserVerificationReadyStateVersion,
            BrowserVerificationSessionVersion = verificationReadyState.BrowserVerificationSessionVersion,
            LaunchMode = verificationReadyState.LaunchMode,
            AssetRootPath = verificationReadyState.AssetRootPath,
            ProfilesRootPath = verificationReadyState.ProfilesRootPath,
            CacheRootPath = verificationReadyState.CacheRootPath,
            ConfigRootPath = verificationReadyState.ConfigRootPath,
            SettingsFilePath = verificationReadyState.SettingsFilePath,
            StartupProfilePath = verificationReadyState.StartupProfilePath,
            RequiredAssets = verificationReadyState.RequiredAssets,
            ReadyAssetCount = verificationReadyState.ReadyAssetCount,
            CompletedSteps = verificationReadyState.CompletedSteps,
            TotalSteps = verificationReadyState.TotalSteps,
            Exists = verificationReadyState.Exists,
            ReadSucceeded = verificationReadyState.ReadSucceeded
        };

        if (!verificationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser corroboration session blocked for profile '{verificationReadyState.ProfileId}'.";
            result.Error = verificationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCorroborationSessionVersion = "runtime-browser-corroboration-session-v1";
        result.BrowserCorroborationStages =
        [
            "open-browser-corroboration-session",
            "bind-browser-verification-ready-state",
            "publish-browser-corroboration-ready"
        ];
        result.BrowserCorroborationSummary = $"Runtime browser corroboration session prepared {result.BrowserCorroborationStages.Length} corroboration stage(s) for profile '{verificationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser corroboration session ready for profile '{verificationReadyState.ProfileId}' with {result.BrowserCorroborationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCorroborationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCorroborationSessionVersion { get; set; } = string.Empty;
    public string BrowserVerificationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserVerificationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCorroborationStages { get; set; } = Array.Empty<string>();
    public string BrowserCorroborationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
