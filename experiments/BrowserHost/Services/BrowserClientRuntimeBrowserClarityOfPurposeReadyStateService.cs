namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserClarityOfPurposeReadyState
{
    ValueTask<BrowserClientRuntimeBrowserClarityOfPurposeReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserClarityOfPurposeReadyStateService : IBrowserClientRuntimeBrowserClarityOfPurposeReadyState
{
    private readonly IBrowserClientRuntimeBrowserClarityOfPurposeSession _runtimeBrowserClarityOfPurposeSession;

    public BrowserClientRuntimeBrowserClarityOfPurposeReadyStateService(IBrowserClientRuntimeBrowserClarityOfPurposeSession runtimeBrowserClarityOfPurposeSession)
    {
        _runtimeBrowserClarityOfPurposeSession = runtimeBrowserClarityOfPurposeSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserClarityOfPurposeReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserClarityOfPurposeSessionResult clarityofpurposeSession = await _runtimeBrowserClarityOfPurposeSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserClarityOfPurposeReadyStateResult result = new()
        {
            ProfileId = clarityofpurposeSession.ProfileId,
            SessionId = clarityofpurposeSession.SessionId,
            SessionPath = clarityofpurposeSession.SessionPath,
            BrowserClarityOfPurposeSessionVersion = clarityofpurposeSession.BrowserClarityOfPurposeSessionVersion,
            BrowserAssurance2ReadyStateVersion = clarityofpurposeSession.BrowserAssurance2ReadyStateVersion,
            BrowserAssurance2SessionVersion = clarityofpurposeSession.BrowserAssurance2SessionVersion,
            LaunchMode = clarityofpurposeSession.LaunchMode,
            AssetRootPath = clarityofpurposeSession.AssetRootPath,
            ProfilesRootPath = clarityofpurposeSession.ProfilesRootPath,
            CacheRootPath = clarityofpurposeSession.CacheRootPath,
            ConfigRootPath = clarityofpurposeSession.ConfigRootPath,
            SettingsFilePath = clarityofpurposeSession.SettingsFilePath,
            StartupProfilePath = clarityofpurposeSession.StartupProfilePath,
            RequiredAssets = clarityofpurposeSession.RequiredAssets,
            ReadyAssetCount = clarityofpurposeSession.ReadyAssetCount,
            CompletedSteps = clarityofpurposeSession.CompletedSteps,
            TotalSteps = clarityofpurposeSession.TotalSteps,
            Exists = clarityofpurposeSession.Exists,
            ReadSucceeded = clarityofpurposeSession.ReadSucceeded
        };

        if (!clarityofpurposeSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser clarityofpurpose ready state blocked for profile '{clarityofpurposeSession.ProfileId}'.";
            result.Error = clarityofpurposeSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserClarityOfPurposeReadyStateVersion = "runtime-browser-clarityofpurpose-ready-state-v1";
        result.BrowserClarityOfPurposeReadyChecks =
        [
            "browser-assurance2-ready-state-ready",
            "browser-clarityofpurpose-session-ready",
            "browser-clarityofpurpose-ready"
        ];
        result.BrowserClarityOfPurposeReadySummary = $"Runtime browser clarityofpurpose ready state passed {result.BrowserClarityOfPurposeReadyChecks.Length} clarityofpurpose readiness check(s) for profile '{clarityofpurposeSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser clarityofpurpose ready state ready for profile '{clarityofpurposeSession.ProfileId}' with {result.BrowserClarityOfPurposeReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserClarityOfPurposeReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserClarityOfPurposeReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClarityOfPurposeSessionVersion { get; set; } = string.Empty;
    public string BrowserAssurance2ReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurance2SessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserClarityOfPurposeReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserClarityOfPurposeReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
