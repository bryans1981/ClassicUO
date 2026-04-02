namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserIntuitivenessSession
{
    ValueTask<BrowserClientRuntimeBrowserIntuitivenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserIntuitivenessSessionService : IBrowserClientRuntimeBrowserIntuitivenessSession
{
    private readonly IBrowserClientRuntimeBrowserClarityReadyState _runtimeBrowserClarityReadyState;

    public BrowserClientRuntimeBrowserIntuitivenessSessionService(IBrowserClientRuntimeBrowserClarityReadyState runtimeBrowserClarityReadyState)
    {
        _runtimeBrowserClarityReadyState = runtimeBrowserClarityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserIntuitivenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserClarityReadyStateResult clarityReadyState = await _runtimeBrowserClarityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserIntuitivenessSessionResult result = new()
        {
            ProfileId = clarityReadyState.ProfileId,
            SessionId = clarityReadyState.SessionId,
            SessionPath = clarityReadyState.SessionPath,
            BrowserClarityReadyStateVersion = clarityReadyState.BrowserClarityReadyStateVersion,
            BrowserClaritySessionVersion = clarityReadyState.BrowserClaritySessionVersion,
            LaunchMode = clarityReadyState.LaunchMode,
            AssetRootPath = clarityReadyState.AssetRootPath,
            ProfilesRootPath = clarityReadyState.ProfilesRootPath,
            CacheRootPath = clarityReadyState.CacheRootPath,
            ConfigRootPath = clarityReadyState.ConfigRootPath,
            SettingsFilePath = clarityReadyState.SettingsFilePath,
            StartupProfilePath = clarityReadyState.StartupProfilePath,
            RequiredAssets = clarityReadyState.RequiredAssets,
            ReadyAssetCount = clarityReadyState.ReadyAssetCount,
            CompletedSteps = clarityReadyState.CompletedSteps,
            TotalSteps = clarityReadyState.TotalSteps,
            Exists = clarityReadyState.Exists,
            ReadSucceeded = clarityReadyState.ReadSucceeded
        };

        if (!clarityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser intuitiveness session blocked for profile '{clarityReadyState.ProfileId}'.";
            result.Error = clarityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserIntuitivenessSessionVersion = "runtime-browser-intuitiveness-session-v1";
        result.BrowserIntuitivenessStages =
        [
            "open-browser-intuitiveness-session",
            "bind-browser-clarity-ready-state",
            "publish-browser-intuitiveness-ready"
        ];
        result.BrowserIntuitivenessSummary = $"Runtime browser intuitiveness session prepared {result.BrowserIntuitivenessStages.Length} intuitiveness stage(s) for profile '{clarityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser intuitiveness session ready for profile '{clarityReadyState.ProfileId}' with {result.BrowserIntuitivenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserIntuitivenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserIntuitivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserClarityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClaritySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserIntuitivenessStages { get; set; } = Array.Empty<string>();
    public string BrowserIntuitivenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

