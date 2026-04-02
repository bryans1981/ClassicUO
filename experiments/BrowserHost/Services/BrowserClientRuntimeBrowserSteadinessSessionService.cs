namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadinessSession
{
    ValueTask<BrowserClientRuntimeBrowserSteadinessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadinessSessionService : IBrowserClientRuntimeBrowserSteadinessSession
{
    private readonly IBrowserClientRuntimeBrowserReassuranceReadyState _runtimeBrowserReassuranceReadyState;

    public BrowserClientRuntimeBrowserSteadinessSessionService(IBrowserClientRuntimeBrowserReassuranceReadyState runtimeBrowserReassuranceReadyState)
    {
        _runtimeBrowserReassuranceReadyState = runtimeBrowserReassuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadinessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReassuranceReadyStateResult reassuranceReadyState = await _runtimeBrowserReassuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadinessSessionResult result = new()
        {
            ProfileId = reassuranceReadyState.ProfileId,
            SessionId = reassuranceReadyState.SessionId,
            SessionPath = reassuranceReadyState.SessionPath,
            BrowserReassuranceReadyStateVersion = reassuranceReadyState.BrowserReassuranceReadyStateVersion,
            BrowserReassuranceSessionVersion = reassuranceReadyState.BrowserReassuranceSessionVersion,
            LaunchMode = reassuranceReadyState.LaunchMode,
            AssetRootPath = reassuranceReadyState.AssetRootPath,
            ProfilesRootPath = reassuranceReadyState.ProfilesRootPath,
            CacheRootPath = reassuranceReadyState.CacheRootPath,
            ConfigRootPath = reassuranceReadyState.ConfigRootPath,
            SettingsFilePath = reassuranceReadyState.SettingsFilePath,
            StartupProfilePath = reassuranceReadyState.StartupProfilePath,
            RequiredAssets = reassuranceReadyState.RequiredAssets,
            ReadyAssetCount = reassuranceReadyState.ReadyAssetCount,
            CompletedSteps = reassuranceReadyState.CompletedSteps,
            TotalSteps = reassuranceReadyState.TotalSteps,
            Exists = reassuranceReadyState.Exists,
            ReadSucceeded = reassuranceReadyState.ReadSucceeded
        };

        if (!reassuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadiness session blocked for profile '{reassuranceReadyState.ProfileId}'.";
            result.Error = reassuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadinessSessionVersion = "runtime-browser-steadiness-session-v1";
        result.BrowserSteadinessStages =
        [
            "open-browser-steadiness-session",
            "bind-browser-reassurance-ready-state",
            "publish-browser-steadiness-ready"
        ];
        result.BrowserSteadinessSummary = $"Runtime browser steadiness session prepared {result.BrowserSteadinessStages.Length} steadiness stage(s) for profile '{reassuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadiness session ready for profile '{reassuranceReadyState.ProfileId}' with {result.BrowserSteadinessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadinessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadinessSessionVersion { get; set; } = string.Empty;
    public string BrowserReassuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReassuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadinessStages { get; set; } = Array.Empty<string>();
    public string BrowserSteadinessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
