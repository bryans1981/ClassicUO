namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserIntentionalitySession
{
    ValueTask<BrowserClientRuntimeBrowserIntentionalitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserIntentionalitySessionService : IBrowserClientRuntimeBrowserIntentionalitySession
{
    private readonly IBrowserClientRuntimeBrowserClarityOfPurposeReadyState _runtimeBrowserClarityOfPurposeReadyState;

    public BrowserClientRuntimeBrowserIntentionalitySessionService(IBrowserClientRuntimeBrowserClarityOfPurposeReadyState runtimeBrowserClarityOfPurposeReadyState)
    {
        _runtimeBrowserClarityOfPurposeReadyState = runtimeBrowserClarityOfPurposeReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserIntentionalitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserClarityOfPurposeReadyStateResult clarityofpurposeReadyState = await _runtimeBrowserClarityOfPurposeReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserIntentionalitySessionResult result = new()
        {
            ProfileId = clarityofpurposeReadyState.ProfileId,
            SessionId = clarityofpurposeReadyState.SessionId,
            SessionPath = clarityofpurposeReadyState.SessionPath,
            BrowserClarityOfPurposeReadyStateVersion = clarityofpurposeReadyState.BrowserClarityOfPurposeReadyStateVersion,
            BrowserClarityOfPurposeSessionVersion = clarityofpurposeReadyState.BrowserClarityOfPurposeSessionVersion,
            LaunchMode = clarityofpurposeReadyState.LaunchMode,
            AssetRootPath = clarityofpurposeReadyState.AssetRootPath,
            ProfilesRootPath = clarityofpurposeReadyState.ProfilesRootPath,
            CacheRootPath = clarityofpurposeReadyState.CacheRootPath,
            ConfigRootPath = clarityofpurposeReadyState.ConfigRootPath,
            SettingsFilePath = clarityofpurposeReadyState.SettingsFilePath,
            StartupProfilePath = clarityofpurposeReadyState.StartupProfilePath,
            RequiredAssets = clarityofpurposeReadyState.RequiredAssets,
            ReadyAssetCount = clarityofpurposeReadyState.ReadyAssetCount,
            CompletedSteps = clarityofpurposeReadyState.CompletedSteps,
            TotalSteps = clarityofpurposeReadyState.TotalSteps,
            Exists = clarityofpurposeReadyState.Exists,
            ReadSucceeded = clarityofpurposeReadyState.ReadSucceeded
        };

        if (!clarityofpurposeReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser intentionality session blocked for profile '{clarityofpurposeReadyState.ProfileId}'.";
            result.Error = clarityofpurposeReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserIntentionalitySessionVersion = "runtime-browser-intentionality-session-v1";
        result.BrowserIntentionalityStages =
        [
            "open-browser-intentionality-session",
            "bind-browser-clarityofpurpose-ready-state",
            "publish-browser-intentionality-ready"
        ];
        result.BrowserIntentionalitySummary = $"Runtime browser intentionality session prepared {result.BrowserIntentionalityStages.Length} intentionality stage(s) for profile '{clarityofpurposeReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser intentionality session ready for profile '{clarityofpurposeReadyState.ProfileId}' with {result.BrowserIntentionalityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserIntentionalitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserIntentionalitySessionVersion { get; set; } = string.Empty;
    public string BrowserClarityOfPurposeReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClarityOfPurposeSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserIntentionalityStages { get; set; } = Array.Empty<string>();
    public string BrowserIntentionalitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
