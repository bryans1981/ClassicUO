namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserClarityOfPurposeSession
{
    ValueTask<BrowserClientRuntimeBrowserClarityOfPurposeSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserClarityOfPurposeSessionService : IBrowserClientRuntimeBrowserClarityOfPurposeSession
{
    private readonly IBrowserClientRuntimeBrowserAssurance2ReadyState _runtimeBrowserAssurance2ReadyState;

    public BrowserClientRuntimeBrowserClarityOfPurposeSessionService(IBrowserClientRuntimeBrowserAssurance2ReadyState runtimeBrowserAssurance2ReadyState)
    {
        _runtimeBrowserAssurance2ReadyState = runtimeBrowserAssurance2ReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserClarityOfPurposeSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssurance2ReadyStateResult assurance2ReadyState = await _runtimeBrowserAssurance2ReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserClarityOfPurposeSessionResult result = new()
        {
            ProfileId = assurance2ReadyState.ProfileId,
            SessionId = assurance2ReadyState.SessionId,
            SessionPath = assurance2ReadyState.SessionPath,
            BrowserAssurance2ReadyStateVersion = assurance2ReadyState.BrowserAssurance2ReadyStateVersion,
            BrowserAssurance2SessionVersion = assurance2ReadyState.BrowserAssurance2SessionVersion,
            LaunchMode = assurance2ReadyState.LaunchMode,
            AssetRootPath = assurance2ReadyState.AssetRootPath,
            ProfilesRootPath = assurance2ReadyState.ProfilesRootPath,
            CacheRootPath = assurance2ReadyState.CacheRootPath,
            ConfigRootPath = assurance2ReadyState.ConfigRootPath,
            SettingsFilePath = assurance2ReadyState.SettingsFilePath,
            StartupProfilePath = assurance2ReadyState.StartupProfilePath,
            RequiredAssets = assurance2ReadyState.RequiredAssets,
            ReadyAssetCount = assurance2ReadyState.ReadyAssetCount,
            CompletedSteps = assurance2ReadyState.CompletedSteps,
            TotalSteps = assurance2ReadyState.TotalSteps,
            Exists = assurance2ReadyState.Exists,
            ReadSucceeded = assurance2ReadyState.ReadSucceeded
        };

        if (!assurance2ReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser clarityofpurpose session blocked for profile '{assurance2ReadyState.ProfileId}'.";
            result.Error = assurance2ReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserClarityOfPurposeSessionVersion = "runtime-browser-clarityofpurpose-session-v1";
        result.BrowserClarityOfPurposeStages =
        [
            "open-browser-clarityofpurpose-session",
            "bind-browser-assurance2-ready-state",
            "publish-browser-clarityofpurpose-ready"
        ];
        result.BrowserClarityOfPurposeSummary = $"Runtime browser clarityofpurpose session prepared {result.BrowserClarityOfPurposeStages.Length} clarityofpurpose stage(s) for profile '{assurance2ReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser clarityofpurpose session ready for profile '{assurance2ReadyState.ProfileId}' with {result.BrowserClarityOfPurposeStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserClarityOfPurposeSessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserClarityOfPurposeStages { get; set; } = Array.Empty<string>();
    public string BrowserClarityOfPurposeSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
